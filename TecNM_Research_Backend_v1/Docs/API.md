# Contratos HTTP

Base: `/api/v1`. Todas las respuestas JSON usan camelCase. Listados: `items`, `totalCount`, `pageNumber`, `pageSize`, `totalPages`, `hasPreviousPage`, `hasNextPage`. Parámetros comunes: pageNumber=1, pageSize=20 (1..100), search e includeDeleted=false. Parámetros inválidos: 400. Registro ausente: 404. Conflicto: 409. Carga demasiado grande: 413. Errores: ProblemDetails.

## CRUD de convocatorias y catálogos

Estas rutas tienen el mismo conjunto de operaciones:

- `/calls`
- `/call-types`
- `/document-types`
- `/calls/{callId}/requirements`
- `/calls/{callId}/document-requirements`

| Verbo | Sufijo | Resultado |
|---|---|---|
| GET | vacío | Listado paginado. |
| GET | /{id} | Registro o detalle; excluye eliminados. |
| POST | vacío | Crear; 201 y Location. |
| PUT | /{id} | Actualizar campos editables; 200. |
| PATCH | /{id}/active | `{ "isActive": true }` o false; 204. |
| PATCH | /{id}/visibility | `{ "isVisible": true }` o false; 204. |
| DELETE | /{id} | Eliminación lógica; 204. |
| PATCH | /{id}/restore | Restauración inactiva y oculta; 204. |

`/calls/options`, `/call-types/options` y `/document-types/options`: GET con id/code/name, máximo 200 entradas. `/calls/open`: GET paginado sin notas internas, activas/visibles/abiertas. Calls admite organizationId, callTypeId y statusId (1..4). `/calls/{id}` incluye call, requirements y documentRequirements; estas colecciones son de gestión, incluyen inactivos/ocultos y excluyen eliminados.

### Crear/editar convocatoria

```json
{
  "code": "INV-2027-01",
  "name": "Investigación 2027",
  "description": "Recepción de propuestas",
  "organizationId": 1,
  "callTypeId": 1,
  "opensAt": "2027-01-01T00:00:00-06:00",
  "closesAt": "2027-02-01T00:00:00-06:00",
  "displayOrder": 1,
  "notes": "Notas de gestión"
}
```

### Crear/editar tipo de convocatoria

```json
{ "code": "INTERNA", "name": "Interna", "description": "Convocatoria institucional", "displayOrder": 1, "notes": null }
```

### Crear/editar tipo de documento

```json
{ "code": "PROPUESTA", "name": "Propuesta de investigación", "description": null, "templateId": 1, "displayOrder": 1, "notes": null }
```

TemplateId admite null para un documento sin plantilla. Para quitar una asociación en PUT, enviar templateId=null. El id seleccionado debe ser de una versión vigente y activa. Los PUT sustituyen los campos editables completos; no son PATCH parciales.

### Requisito general

```json
{ "title": "Adscripción", "description": "Contar con adscripción vigente", "isRequired": true, "displayOrder": 1, "notes": null }
```

### Documento requerido

```json
{ "title": "Propuesta firmada", "documentTypeId": 1, "workflowStage": "application", "isRequired": true, "displayOrder": 1, "notes": null }
```

WorkflowStage: application, submission, academic_authorization, progress o closure. Se devuelve además documentTypeName, templateId y templateName.

## Documentos físicos

| Verbo | Ruta | Operación |
|---|---|---|
| GET | /documents | Listado paginado; admite documentTypeId y search. |
| GET | /documents/{id} | Metadatos con id/nombre del tipo y del autor. |
| POST | /documents | multipart/form-data: DocumentTypeId, File, DisplayOrder opcional y Notes opcional. |
| GET | /documents/{id}/download | Archivo adjunto. |
| GET | /documents/{id}/references | Conteo de referencias por proceso (incluye históricas). |
| PUT | /documents/{id} | 409: documento inmutable por diseño de la base. |
| DELETE | /documents/{id} | 409: documento histórico; modificar su vínculo desde el proceso propietario. |

No hay uploadedBy, sha256, storageKey o ruta física en el contrato de carga. Los determina el servidor. IncludeDeleted no modifica este listado: los documentos no admiten borrado en el esquema actual. Una carga no adjunta automáticamente el documento a un proyecto ni cumple por sí sola un requisito de convocatoria.

## Plantillas y versiones

| Verbo | Ruta | Operación |
|---|---|---|
| GET | /document-templates | Paginado; currentOnly=true por defecto, search e includeDeleted. |
| GET | /document-templates/{id} | Metadatos de una versión no eliminada. |
| GET | /document-templates/versions/{code} | Historial paginado de la familia; acepta includeDeleted. |
| POST | /document-templates | Multipart: Code, Name, File, DisplayOrder y Notes opcionales. Crea v1 o siguiente versión y la hace vigente. |
| PUT | /document-templates/{id} | JSON: displayOrder y notes. Solo metadatos permitidos por trigger. |
| PATCH | /document-templates/{id}/current | Seleccionar una versión activa como vigente; 204. |
| PATCH | /document-templates/{id}/active | JSON: isActive; 204. |
| PATCH | /document-templates/{id}/visibility | JSON: isVisible; 204. |
| DELETE | /document-templates/{id} | Eliminación lógica; exige desvincular tipos y preservar otra vigente cuando corresponda. |
| PATCH | /document-templates/{id}/restore | Restaurar inactiva y oculta; 204. |
| GET | /document-templates/{id}/download | Archivo adjunto de la versión solicitada. |

La familia puede tener varios tipos de documento asociados según el modelo existente. Cambiar su vigente actualiza todos los tipos que seguían esa familia. El documento cargado conserva su tipo, no una FK a la plantilla; esta última se usa como formato de referencia vigente, no como copia del archivo recibido.
