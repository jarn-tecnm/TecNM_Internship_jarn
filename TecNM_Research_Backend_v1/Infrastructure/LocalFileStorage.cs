using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using TecNM.Research.Common;

namespace TecNM.Research.Infrastructure;

public record StoredFile(string Key, string OriginalName, string MediaType, long Size, string Sha256);
public interface ILocalFileStorage
{
    Task<StoredFile> SaveAsync(IFormFile file, CancellationToken ct);
    FileStream Open(string key);
}

// Archivos fuera de wwwroot; el cliente nunca elige rutas ni recibe StorageKey.
public sealed class LocalFileStorage : ILocalFileStorage
{
    public const long MaxBytes = 30L * 1024 * 1024;
    private readonly string _root;
    public LocalFileStorage(IWebHostEnvironment environment, IConfiguration configuration)
    {
        _root = Path.GetFullPath(Path.Combine(environment.ContentRootPath, configuration["Storage:Root"] ?? Path.Combine("App_Data", "files")));
        var webRoot = Path.GetFullPath(environment.WebRootPath ?? Path.Combine(environment.ContentRootPath, "wwwroot"));
        if (_root.Equals(webRoot, StringComparison.OrdinalIgnoreCase) || _root.StartsWith(webRoot + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase)) throw new InvalidOperationException("Storage:Root debe estar fuera de wwwroot.");
        Directory.CreateDirectory(_root);
    }
    public async Task<StoredFile> SaveAsync(IFormFile file, CancellationToken ct)
    {
        if (file is null || file.Length <= 0) throw new ApiFault(400, "Selecciona un archivo con contenido.");
        if (file.Length > MaxBytes) throw new ApiFault(413, "El archivo excede 30 MiB.");
        var name = Path.GetFileName(file.FileName.Replace('\\', '/'));
        if (string.IsNullOrWhiteSpace(name) || name.Length > 300 || name.Any(char.IsControl)) throw new ApiFault(400, "Nombre de archivo inválido (máximo 300 caracteres).");
        var extension = Path.GetExtension(name).ToLowerInvariant();
        var mediaType = extension switch { ".pdf" => "application/pdf", ".png" => "image/png", ".jpg" or ".jpeg" => "image/jpeg", ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document", ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", _ => throw new ApiFault(400, "Formatos permitidos: PDF, PNG, JPG, DOCX y XLSX.") };
        var key = Guid.NewGuid().ToString("N") + extension;
        var path = Resolve(key);
        try
        {
            long total = 0;
            string hash;
            await using (var input = file.OpenReadStream())
            await using (var output = new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.None, 81920, true))
            using (var sha = IncrementalHash.CreateHash(HashAlgorithmName.SHA256))
            {
                var buffer = new byte[81920];
                int count;
                while ((count = await input.ReadAsync(buffer.AsMemory(), ct)) != 0)
                {
                    total += count;
                    if (total > MaxBytes) throw new ApiFault(413, "El archivo excede 30 MiB.");
                    sha.AppendData(buffer, 0, count);
                    await output.WriteAsync(buffer.AsMemory(0, count), ct);
                }
                hash = Convert.ToHexString(sha.GetHashAndReset()).ToLowerInvariant();
            }
            if (total == 0) throw new ApiFault(400, "El archivo está vacío.");
            ValidateFormat(path, extension);
            return new(key, name, mediaType, total, hash);
        }
        catch
        {
            // Aún no existe ninguna fila en BD que pueda referenciar este archivo incompleto.
            File.Delete(path);
            throw;
        }
    }
    public FileStream Open(string key)
    {
        var path = Resolve(key);
        if (!File.Exists(path)) throw new ApiFault(404, "El archivo físico no está disponible.");
        return new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 81920, true);
    }
    private string Resolve(string key)
    {
        if (string.IsNullOrWhiteSpace(key) || Path.IsPathRooted(key)) throw new ApiFault(409, "Clave de almacenamiento inválida.");
        var path = Path.GetFullPath(Path.Combine(_root, key));
        if (!path.StartsWith(_root.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase)) throw new ApiFault(409, "La ruta está fuera del almacenamiento configurado.");
        return path;
    }
    private static void ValidateFormat(string path, string extension)
    {
        using var stream = File.OpenRead(path);
        Span<byte> header = stackalloc byte[8];
        var count = stream.Read(header);
        var valid = extension switch
        {
            ".pdf" => count >= 5 && Encoding.ASCII.GetString(header[..5]) == "%PDF-",
            ".png" => count >= 8 && header.SequenceEqual(new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 }),
            ".jpg" or ".jpeg" => count >= 3 && header[0] == 255 && header[1] == 216 && header[2] == 255,
            ".docx" or ".xlsx" => ValidOfficePackage(stream, extension),
            _ => false
        };
        if (!valid) throw new ApiFault(400, "El contenido no corresponde al formato declarado.");
    }
    private static bool ValidOfficePackage(Stream stream, string extension)
    {
        try
        {
            stream.Position = 0;
            using var zip = new ZipArchive(stream, ZipArchiveMode.Read, true);
            if (zip.Entries.Count > 5000) return false;
            long total = 0;
            foreach (var entry in zip.Entries)
            {
                if (entry.Length < 0 || entry.Length > 200L * 1024 * 1024) return false;
                total += entry.Length;
                if (total > 200L * 1024 * 1024) return false;
            }
            if (zip.GetEntry("[Content_Types].xml") is null) return false;
            if (zip.Entries.Any(e => e.FullName.EndsWith("vbaProject.bin", StringComparison.OrdinalIgnoreCase))) return false;
            return zip.GetEntry(extension == ".docx" ? "word/document.xml" : "xl/workbook.xml") is not null;
        }
        catch (InvalidDataException) { return false; }
    }
}
