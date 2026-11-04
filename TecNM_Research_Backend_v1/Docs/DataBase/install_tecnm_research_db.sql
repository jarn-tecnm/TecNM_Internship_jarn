-- =====================================================================
-- TECNM RESEARCH — INSTALACIÓN COMPLETA DE DESARROLLO
-- Archivo único: instalar_tecnm_research.sql
-- PostgreSQL 16 o posterior. Ejecutar con psql, NO con Query Tool de pgAdmin.
-- =====================================================================
-- ATENCIÓN: CADA EJECUCIÓN BORRA tecnm_research_db, incluso sus datos,
-- y crea o actualiza el usuario dedicado tecnm_user_research.
-- Contraseña del usuario de la aplicación: Temporal-2026
-- Este archivo es para instalar/reiniciar el entorno del alumno.
-- No es una migración ni debe ejecutarse contra una base con datos que conservar.
--
-- PASOS (WINDOWS / SQL SHELL — OPCIÓN RECOMENDADA PARA EL ALUMNO):
-- 1. Tener PostgreSQL 16 o posterior instalado y el servidor iniciado.
-- 2. Guardar este archivo en C:/TecNM/instalar_tecnm_research.sql
-- 3. Abrir SQL Shell (psql) desde el menú Inicio de Windows.
-- 4. Conectar con Server=localhost, Database=postgres, Port=5432, Username=postgres.
--    Usar el puerto real si es diferente; escribir la contraseña administrativa.
-- 5. En el indicador postgres=# ejecutar esta línea y presionar Enter:
--    \i 'C:/TecNM/instalar_tecnm_research.sql'
-- 6. Esperar el mensaje INSTALACION COMPLETADA CORRECTAMENTE.
--    Si hay ERROR y no aparece ese mensaje, la instalación no terminó.
-- 7. La aplicación usará base=tecnm_research_db, usuario=tecnm_user_research,
--    contraseña=Temporal-2026; la contraseña administrativa no se cambia.
--
-- ALTERNATIVA (WINDOWS / POWERSHELL):
-- 1. Tener PostgreSQL 16 o posterior instalado y el servidor iniciado.
-- 2. Guardar este archivo en C:/TecNM/instalar_tecnm_research.sql
-- 3. Abrir PowerShell y ejecutar (si psql está en PATH):
--    psql -X -h localhost -p 5432 -U postgres -d postgres -W -f "C:/TecNM/instalar_tecnm_research.sql"
--    Si psql no se reconoce, usar su ruta completa. Ejemplo para PostgreSQL 18:
--    & "C:/Program Files/PostgreSQL/18/bin/psql.exe" -X -h localhost -p 5432 -U postgres -d postgres -W -f "C:/TecNM/instalar_tecnm_research.sql"
--    Ajustar 18 a la versión instalada y 5432 al puerto configurado.
-- 4. Escribir la contraseña ADMINISTRATIVA de postgres cuando se solicite.
--    No es Temporal-2026, salvo que hayas configurado así a postgres.
-- 5. Esperar el mensaje final: INSTALACION COMPLETADA CORRECTAMENTE.
--    En PowerShell, $LASTEXITCODE debe devolver 0 después de la ejecución.
-- 6. Conectar la aplicación o pgAdmin con:
--    servidor: localhost | puerto: 5432 | base: tecnm_research_db
--    usuario: tecnm_user_research | contraseña: Temporal-2026
--
-- LINUX / MACOS (si postgres usa autenticación con contraseña):
--    psql -X -h localhost -p 5432 -U postgres -d postgres -W -f ./instalar_tecnm_research.sql
--
-- No usar -1 ni --single-transaction. No copiar el contenido al Query Tool.
-- El archivo controla sus propias conexiones y transacciones mediante psql.
-- Se detiene en el primer error. Si falla la carga, tablas y ejemplos se revierten;
-- la base anterior NO se recupera porque su eliminación es independiente.
-- Los datos y metadatos documentales DEMO son ficticios; no se incluyen PDFs.
-- Los usuarios DEMO no crean identidades ni sesiones en la macroplataforma.
-- Todas las tablas conservan sus campos comunes y notes como último campo.
-- Revisión funcional: permisos por roles, cadena proyecto/objetivo/hito/tarea,
-- evidencias por historial y una sola decisión formal: Subdirección Académica.
-- Revisión adicional: proyectos de cualquier organización y productos unificados.
-- organization_id identifica la organización del proyecto, no una afiliación.
-- is_managed_in_platform=TRUE: gestión y revisión dentro de la plataforma.
-- is_managed_in_platform=FALSE: antecedente externo/histórico del CV; permite
-- registrar participantes y productos sin protocolo, plan, presupuesto ni envío.
-- La organización por sí sola no determina si se requiere revisión interna.
-- Los cuerpos académicos y su pertenencia son opcionales. Cada cuerpo tiene
-- una organización y un historial de grados de consolidación. Los proyectos
-- pueden asociarse con cero, uno o varios cuerpos, sin heredar permisos.
-- El grado actual se obtiene del historial por changed_at DESC e id DESC.
-- Sigue siendo obligatorio registrar un responsable del proyecto.
-- TODAS las decisiones académicas se insertan en academic_authorizations_history;
-- no se actualiza projects_submissions para registrar decisiones.
-- El tipo se asigna en files.documents; el formato opcional se asigna al tipo.
-- Los formatos guardan su propia ubicación, sin relación directa con documents.
-- Los porcentajes del proyecto se validan entre 0 y 100. La API los actualiza;
-- la fórmula de agregación queda pendiente de definición (no se presume promedio).
-- Versión revisada: los campos created_by, updated_by y deleted_by se
-- conservan como BIGINT NULL, sin claves foráneas ni índices de relación.
-- La API debe validar sus valores al registrar acciones de usuarios.
-- Las relaciones funcionales con access.users (roles, perfiles, revisiones,
-- autorizaciones y otras acciones) se conservan.
-- El propietario controla la base y sus objetos, sin ser superusuario del servidor.
-- Si el usuario ya existe, se reutiliza y se restablece su contraseña.
-- Sus objetos y permisos fuera de la base objetivo se conservan; no se borra el rol.

\set ON_ERROR_STOP on
\set AUTOCOMMIT on
\pset pager off
\connect postgres
\encoding UTF8

\echo '1/5 Verificando el servidor y la cuenta administrativa...'
DO $preflight$
BEGIN
    IF current_setting('server_version_num')::int < 160000 THEN
        RAISE EXCEPTION 'Se requiere PostgreSQL 16 o posterior';
    END IF;
    IF NOT (SELECT rolsuper FROM pg_roles WHERE rolname=current_user) THEN
        RAISE EXCEPTION 'Ejecute con la cuenta administrativa postgres (o un superusuario)';
    END IF;
END $preflight$;

\echo '2/5 Recreando la base y preparando el usuario dedicado...'
-- Fuera de cualquier transacción: psql envía cada sentencia por separado.
DROP DATABASE IF EXISTS tecnm_research_db WITH (FORCE);

BEGIN;
SET LOCAL password_encryption = 'scram-sha-256';
DO $application_user$
BEGIN
    IF EXISTS (SELECT 1 FROM pg_roles WHERE rolname = 'tecnm_user_research') THEN
        ALTER ROLE tecnm_user_research WITH
            LOGIN PASSWORD 'Temporal-2026'
            NOSUPERUSER NOCREATEDB NOCREATEROLE NOREPLICATION NOBYPASSRLS
            CONNECTION LIMIT -1 VALID UNTIL 'infinity';
        RAISE NOTICE 'Usuario tecnm_user_research existente: contraseña y opciones de acceso restablecidas.';
    ELSE
        CREATE ROLE tecnm_user_research WITH
            LOGIN PASSWORD 'Temporal-2026'
            NOSUPERUSER NOCREATEDB NOCREATEROLE NOREPLICATION NOBYPASSRLS
            CONNECTION LIMIT -1 VALID UNTIL 'infinity';
        RAISE NOTICE 'Usuario tecnm_user_research creado.';
    END IF;
END $application_user$;
COMMIT;

CREATE DATABASE tecnm_research_db
    WITH OWNER=tecnm_user_research ENCODING='UTF8' TEMPLATE=template0;
GRANT ALL PRIVILEGES ON DATABASE tecnm_research_db TO tecnm_user_research;

\connect tecnm_research_db
\encoding UTF8
GRANT ALL PRIVILEGES ON SCHEMA public TO tecnm_user_research;
-- Crear TODOS los objetos como el propietario de la aplicación.
SET ROLE tecnm_user_research;

\echo '3/5 Creando la estructura revisada...'
BEGIN;
SET LOCAL TIME ZONE 'UTC';

DO $$ BEGIN IF current_database() <> 'tecnm_research_db' THEN RAISE EXCEPTION 'Base de datos incorrecta'; END IF; END $$;

CREATE SCHEMA access;

CREATE SCHEMA academic;

CREATE SCHEMA calls;

CREATE SCHEMA research;

CREATE SCHEMA reviews;

CREATE SCHEMA files;

CREATE TABLE access.users (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    identity_provider VARCHAR(100) NOT NULL,
    external_subject VARCHAR(250) NOT NULL,
    email VARCHAR(320) NOT NULL,
    given_name VARCHAR(150) NOT NULL,
    family_name VARCHAR(200) NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL,
    UNIQUE (identity_provider, external_subject)
);

CREATE TABLE access.c_roles (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    code VARCHAR(80) NOT NULL UNIQUE CHECK (btrim(code) <> ''),
    name VARCHAR(250) NOT NULL CHECK (btrim(name) <> ''),
    description TEXT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL
);

CREATE TABLE access.c_permissions (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    code VARCHAR(80) NOT NULL UNIQUE CHECK (btrim(code) <> ''),
    name VARCHAR(250) NOT NULL CHECK (btrim(name) <> ''),
    description TEXT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL
);

CREATE TABLE access.users_roles (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    user_id BIGINT NOT NULL REFERENCES access.users(id) ON DELETE RESTRICT,
    role_id BIGINT NOT NULL REFERENCES access.c_roles(id) ON DELETE RESTRICT,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL
);

CREATE TABLE access.roles_permissions (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    role_id BIGINT NOT NULL REFERENCES access.c_roles(id) ON DELETE RESTRICT,
    permission_id BIGINT NOT NULL REFERENCES access.c_permissions(id) ON DELETE RESTRICT,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL
);

CREATE TABLE academic.c_organization_types (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    code VARCHAR(80) NOT NULL UNIQUE CHECK (btrim(code) <> ''),
    name VARCHAR(250) NOT NULL CHECK (btrim(name) <> ''),
    description TEXT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL
);

CREATE TABLE academic.c_organizations (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    code VARCHAR(80) NOT NULL UNIQUE CHECK (btrim(code) <> ''),
    name VARCHAR(250) NOT NULL CHECK (btrim(name) <> ''),
    description TEXT NULL,
    organization_type_id BIGINT NOT NULL REFERENCES academic.c_organization_types(id) ON DELETE RESTRICT,
    country_code CHAR(2) NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL
);

CREATE TABLE academic.c_academic_programs (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    code VARCHAR(80) NOT NULL UNIQUE CHECK (btrim(code) <> ''),
    name VARCHAR(250) NOT NULL CHECK (btrim(name) <> ''),
    description TEXT NULL,
    organization_id BIGINT NOT NULL REFERENCES academic.c_organizations(id) ON DELETE RESTRICT,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL
);

CREATE TABLE academic.c_academic_fields (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    code VARCHAR(80) NOT NULL UNIQUE CHECK (btrim(code) <> ''),
    name VARCHAR(250) NOT NULL CHECK (btrim(name) <> ''),
    description TEXT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL
);

CREATE TABLE academic.c_research_lines (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    code VARCHAR(80) NOT NULL UNIQUE CHECK (btrim(code) <> ''),
    name VARCHAR(250) NOT NULL CHECK (btrim(name) <> ''),
    description TEXT NULL,
    academic_field_id BIGINT NOT NULL REFERENCES academic.c_academic_fields(id) ON DELETE RESTRICT,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL
);

CREATE TABLE academic.c_degree_levels (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    code VARCHAR(80) NOT NULL UNIQUE CHECK (btrim(code) <> ''),
    name VARCHAR(250) NOT NULL CHECK (btrim(name) <> ''),
    description TEXT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL
);

CREATE TABLE academic.researchers (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    user_id BIGINT NOT NULL REFERENCES access.users(id) ON DELETE RESTRICT,
    orcid VARCHAR(19) NULL UNIQUE,
    professional_summary TEXT NULL,
    research_interests TEXT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL,
    UNIQUE (user_id)
);

CREATE TABLE academic.researchers_organizations (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    researcher_id BIGINT NOT NULL REFERENCES academic.researchers(id) ON DELETE RESTRICT,
    organization_id BIGINT NOT NULL REFERENCES academic.c_organizations(id) ON DELETE RESTRICT,
    employee_number VARCHAR(80) NULL,
    starts_on DATE NOT NULL,
    ends_on DATE NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL,
    CHECK (ends_on IS NULL OR ends_on >= starts_on)
);

CREATE TABLE academic.researchers_degrees (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    researcher_id BIGINT NOT NULL REFERENCES academic.researchers(id) ON DELETE RESTRICT,
    degree_level_id BIGINT NOT NULL REFERENCES academic.c_degree_levels(id) ON DELETE RESTRICT,
    degree_title VARCHAR(300) NOT NULL,
    institution_name VARCHAR(300) NOT NULL,
    graduation_year INT NULL CHECK (graduation_year BETWEEN 1900 AND 2200),
    professional_license VARCHAR(100) NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL
);

CREATE TABLE academic.researchers_experiences (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    researcher_id BIGINT NOT NULL REFERENCES academic.researchers(id) ON DELETE RESTRICT,
    organization_name VARCHAR(300) NOT NULL,
    position_title VARCHAR(250) NOT NULL,
    description TEXT NULL,
    starts_on DATE NOT NULL,
    ends_on DATE NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL,
    CHECK (ends_on IS NULL OR ends_on >= starts_on)
);

CREATE TABLE academic.researchers_research_lines (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    researcher_id BIGINT NOT NULL REFERENCES academic.researchers(id) ON DELETE RESTRICT,
    research_line_id BIGINT NOT NULL REFERENCES academic.c_research_lines(id) ON DELETE RESTRICT,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL
);

CREATE TABLE academic.students (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    user_id BIGINT NOT NULL REFERENCES access.users(id) ON DELETE RESTRICT,
    academic_program_id BIGINT NOT NULL REFERENCES academic.c_academic_programs(id) ON DELETE RESTRICT,
    student_number VARCHAR(80) NOT NULL,
    semester INT NULL CHECK (semester BETWEEN 1 AND 30),
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL,
    UNIQUE (user_id)
);

CREATE TABLE calls.c_call_types (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    code VARCHAR(80) NOT NULL UNIQUE CHECK (btrim(code) <> ''),
    name VARCHAR(250) NOT NULL CHECK (btrim(name) <> ''),
    description TEXT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL
);

CREATE TABLE calls.c_calls (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    code VARCHAR(80) NOT NULL UNIQUE CHECK (btrim(code) <> ''),
    name VARCHAR(250) NOT NULL CHECK (btrim(name) <> ''),
    description TEXT NULL,
    organization_id BIGINT NOT NULL REFERENCES academic.c_organizations(id) ON DELETE RESTRICT,
    call_type_id BIGINT NOT NULL REFERENCES calls.c_call_types(id) ON DELETE RESTRICT,
    opens_at TIMESTAMPTZ NOT NULL,
    closes_at TIMESTAMPTZ NOT NULL,
    CHECK (closes_at > opens_at),
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL
);

CREATE TABLE files.c_document_templates (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    code VARCHAR(80) NOT NULL,
    name VARCHAR(300) NOT NULL,
    original_file_name VARCHAR(300) NOT NULL,
    storage_key TEXT NOT NULL UNIQUE,
    media_type VARCHAR(150) NOT NULL,
    size_bytes BIGINT NOT NULL CHECK (size_bytes > 0),
    sha256 CHAR(64) NOT NULL CHECK (sha256 ~ '^[0-9a-f]{64}$'),
    version_number INT NOT NULL CHECK (version_number > 0),
    is_current BOOLEAN NOT NULL DEFAULT TRUE,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL,
    UNIQUE (code, version_number)
);

CREATE TABLE files.c_document_types (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    code VARCHAR(80) NOT NULL UNIQUE,
    name VARCHAR(250) NOT NULL,
    description TEXT NULL,
    template_id BIGINT NULL REFERENCES files.c_document_templates(id) ON DELETE RESTRICT,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL
);

CREATE TABLE files.documents (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    document_type_id BIGINT NOT NULL REFERENCES files.c_document_types(id) ON DELETE RESTRICT,
    original_file_name VARCHAR(300) NOT NULL,
    storage_key TEXT NOT NULL UNIQUE,
    media_type VARCHAR(150) NOT NULL,
    size_bytes BIGINT NOT NULL CHECK (size_bytes > 0),
    sha256 CHAR(64) NOT NULL CHECK (sha256 ~ '^[0-9a-f]{64}$'),
    uploaded_by BIGINT NOT NULL REFERENCES access.users(id) ON DELETE RESTRICT,
    uploaded_at TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL,
    UNIQUE (id, document_type_id)
);

CREATE TABLE calls.calls_document_requirements (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    call_id BIGINT NOT NULL REFERENCES calls.c_calls(id) ON DELETE RESTRICT,
    document_type_id BIGINT NOT NULL REFERENCES files.c_document_types(id) ON DELETE RESTRICT,
    title VARCHAR(300) NOT NULL,
    workflow_stage VARCHAR(40) NOT NULL CHECK (workflow_stage IN ('application','submission','academic_authorization','progress','closure')),
    is_required BOOLEAN NOT NULL DEFAULT TRUE,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL
);

CREATE TABLE calls.calls_requirements (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    call_id BIGINT NOT NULL REFERENCES calls.c_calls(id) ON DELETE RESTRICT,
    title VARCHAR(300) NOT NULL,
    description TEXT NOT NULL,
    is_required BOOLEAN NOT NULL DEFAULT TRUE,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL
);

CREATE TABLE research.c_currencies (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    code VARCHAR(80) NOT NULL UNIQUE CHECK (btrim(code) <> ''),
    name VARCHAR(250) NOT NULL CHECK (btrim(name) <> ''),
    description TEXT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL
);

CREATE TABLE research.c_budget_categories (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    code VARCHAR(80) NOT NULL UNIQUE CHECK (btrim(code) <> ''),
    name VARCHAR(250) NOT NULL CHECK (btrim(name) <> ''),
    description TEXT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL
);

CREATE TABLE research.c_funding_sources (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    code VARCHAR(80) NOT NULL UNIQUE CHECK (btrim(code) <> ''),
    name VARCHAR(250) NOT NULL CHECK (btrim(name) <> ''),
    description TEXT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL
);

CREATE TABLE research.c_researchers_roles (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    code VARCHAR(80) NOT NULL UNIQUE CHECK (btrim(code) <> ''),
    name VARCHAR(250) NOT NULL CHECK (btrim(name) <> ''),
    description TEXT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL
);

CREATE TABLE research.c_project_statuses (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    code VARCHAR(80) NOT NULL UNIQUE CHECK (btrim(code) <> ''),
    name VARCHAR(250) NOT NULL CHECK (btrim(name) <> ''),
    description TEXT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL
);

CREATE TABLE research.c_objective_statuses (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    code VARCHAR(80) NOT NULL UNIQUE CHECK (btrim(code) <> ''),
    name VARCHAR(250) NOT NULL CHECK (btrim(name) <> ''),
    description TEXT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL
);

CREATE TABLE research.c_milestone_statuses (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    code VARCHAR(80) NOT NULL UNIQUE CHECK (btrim(code) <> ''),
    name VARCHAR(250) NOT NULL CHECK (btrim(name) <> ''),
    description TEXT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL
);

CREATE TABLE research.c_research_outputs_types (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    code VARCHAR(80) NOT NULL UNIQUE CHECK (btrim(code) <> ''),
    name VARCHAR(250) NOT NULL CHECK (btrim(name) <> ''),
    description TEXT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL
);

CREATE TABLE research.c_task_statuses (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    code VARCHAR(80) NOT NULL UNIQUE,
    name VARCHAR(250) NOT NULL,
    description TEXT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL
);

CREATE TABLE research.projects (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    reference_code VARCHAR(100) NOT NULL UNIQUE,
    title VARCHAR(500) NOT NULL,
    organization_id BIGINT NOT NULL REFERENCES academic.c_organizations(id) ON DELETE RESTRICT,
    is_managed_in_platform BOOLEAN NOT NULL DEFAULT TRUE,
    currency_id BIGINT NOT NULL REFERENCES research.c_currencies(id) ON DELETE RESTRICT,
    abstract TEXT NULL,
    problem_statement TEXT NULL,
    justification TEXT NULL,
    theoretical_framework TEXT NULL,
    methodology TEXT NULL,
    expected_results TEXT NULL,
    bibliographic_references TEXT NULL,
    starts_on DATE NOT NULL,
    ends_on DATE NOT NULL,
    objectives_progress_percentage NUMERIC(5,2) NOT NULL DEFAULT 0 CHECK (objectives_progress_percentage BETWEEN 0 AND 100),
    milestones_progress_percentage NUMERIC(5,2) NOT NULL DEFAULT 0 CHECK (milestones_progress_percentage BETWEEN 0 AND 100),
    tasks_progress_percentage NUMERIC(5,2) NOT NULL DEFAULT 0 CHECK (tasks_progress_percentage BETWEEN 0 AND 100),
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL,
    CHECK (ends_on >= starts_on)
);

CREATE TABLE research.projects_researchers (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    project_id BIGINT NOT NULL REFERENCES research.projects(id) ON DELETE RESTRICT,
    researcher_id BIGINT NOT NULL REFERENCES academic.researchers(id) ON DELETE RESTRICT,
    researcher_role_id BIGINT NOT NULL REFERENCES research.c_researchers_roles(id) ON DELETE RESTRICT,
    contribution TEXT NULL,
    starts_on DATE NOT NULL,
    ends_on DATE NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL,
    UNIQUE (id, project_id),
    CHECK (ends_on IS NULL OR ends_on >= starts_on)
);

CREATE TABLE research.projects_leads (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    project_researcher_id BIGINT NOT NULL REFERENCES research.projects_researchers(id) ON DELETE RESTRICT,
    starts_at TIMESTAMPTZ NOT NULL,
    ends_at TIMESTAMPTZ NULL,
    is_current BOOLEAN NOT NULL DEFAULT TRUE,
    change_reason TEXT NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL,
    CHECK (ends_at IS NULL OR ends_at >= starts_at),
    CHECK (is_current = (ends_at IS NULL))
);

CREATE TABLE research.projects_students (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    project_id BIGINT NOT NULL REFERENCES research.projects(id) ON DELETE RESTRICT,
    student_id BIGINT NOT NULL REFERENCES academic.students(id) ON DELETE RESTRICT,
    activities TEXT NULL,
    starts_on DATE NOT NULL,
    ends_on DATE NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL,
    CHECK (ends_on IS NULL OR ends_on >= starts_on)
);

CREATE TABLE research.projects_research_lines (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    project_id BIGINT NOT NULL REFERENCES research.projects(id) ON DELETE RESTRICT,
    research_line_id BIGINT NOT NULL REFERENCES academic.c_research_lines(id) ON DELETE RESTRICT,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL
);

CREATE TABLE research.objectives (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    project_id BIGINT NOT NULL REFERENCES research.projects(id) ON DELETE RESTRICT,
    objective_type VARCHAR(10) NOT NULL CHECK (objective_type IN ('general','specific')),
    description TEXT NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL,
    UNIQUE (id, project_id)
);

CREATE TABLE research.milestones (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    objective_id BIGINT NOT NULL REFERENCES research.objectives(id) ON DELETE RESTRICT,
    title VARCHAR(300) NOT NULL,
    description TEXT NULL,
    due_on DATE NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL
);

CREATE TABLE research.tasks (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    milestone_id BIGINT NOT NULL REFERENCES research.milestones(id) ON DELETE RESTRICT,
    title VARCHAR(300) NOT NULL,
    description TEXT NULL,
    starts_on DATE NOT NULL,
    ends_on DATE NOT NULL,
    responsible_participant_id BIGINT NULL REFERENCES research.projects_researchers(id) ON DELETE RESTRICT,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL,
    CHECK (ends_on >= starts_on)
);

CREATE TABLE research.projects_status_history (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    project_id BIGINT NOT NULL REFERENCES research.projects(id) ON DELETE RESTRICT,
    status_id BIGINT NOT NULL REFERENCES research.c_project_statuses(id) ON DELETE RESTRICT,
    changed_by BIGINT NOT NULL REFERENCES access.users(id) ON DELETE RESTRICT,
    changed_at TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    reason TEXT NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL
);

CREATE TABLE research.objectives_status_history (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    objective_id BIGINT NOT NULL REFERENCES research.objectives(id) ON DELETE RESTRICT,
    status_id BIGINT NOT NULL REFERENCES research.c_objective_statuses(id) ON DELETE RESTRICT,
    completion_percentage NUMERIC(5,2) NOT NULL DEFAULT 0 CHECK (completion_percentage BETWEEN 0 AND 100),
    changed_by BIGINT NOT NULL REFERENCES access.users(id) ON DELETE RESTRICT,
    changed_at TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    reason TEXT NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL
);

CREATE TABLE research.objectives_status_history_documents (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    status_history_id BIGINT NOT NULL REFERENCES research.objectives_status_history(id) ON DELETE RESTRICT,
    document_id BIGINT NOT NULL,
    document_type_id BIGINT NOT NULL,
    title VARCHAR(300) NOT NULL CHECK (btrim(title) <> ''),
    version_number INT NOT NULL DEFAULT 1 CHECK (version_number > 0),
    is_current BOOLEAN NOT NULL DEFAULT TRUE,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL,
    FOREIGN KEY (document_id, document_type_id) REFERENCES files.documents(id, document_type_id)
);

CREATE TABLE research.milestones_status_history (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    milestone_id BIGINT NOT NULL REFERENCES research.milestones(id) ON DELETE RESTRICT,
    status_id BIGINT NOT NULL REFERENCES research.c_milestone_statuses(id) ON DELETE RESTRICT,
    completion_percentage NUMERIC(5,2) NOT NULL DEFAULT 0 CHECK (completion_percentage BETWEEN 0 AND 100),
    changed_by BIGINT NOT NULL REFERENCES access.users(id) ON DELETE RESTRICT,
    changed_at TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    reason TEXT NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL
);

CREATE TABLE research.milestones_status_history_documents (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    status_history_id BIGINT NOT NULL REFERENCES research.milestones_status_history(id) ON DELETE RESTRICT,
    document_id BIGINT NOT NULL,
    document_type_id BIGINT NOT NULL,
    title VARCHAR(300) NOT NULL CHECK (btrim(title) <> ''),
    version_number INT NOT NULL DEFAULT 1 CHECK (version_number > 0),
    is_current BOOLEAN NOT NULL DEFAULT TRUE,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL,
    FOREIGN KEY (document_id, document_type_id) REFERENCES files.documents(id, document_type_id)
);

CREATE TABLE research.tasks_status_history (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    task_id BIGINT NOT NULL REFERENCES research.tasks(id) ON DELETE RESTRICT,
    status_id BIGINT NOT NULL REFERENCES research.c_task_statuses(id) ON DELETE RESTRICT,
    completion_percentage NUMERIC(5,2) NOT NULL DEFAULT 0 CHECK (completion_percentage BETWEEN 0 AND 100),
    changed_by BIGINT NOT NULL REFERENCES access.users(id) ON DELETE RESTRICT,
    changed_at TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    reason TEXT NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL
);

CREATE TABLE research.tasks_status_history_documents (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    status_history_id BIGINT NOT NULL REFERENCES research.tasks_status_history(id) ON DELETE RESTRICT,
    document_id BIGINT NOT NULL,
    document_type_id BIGINT NOT NULL,
    title VARCHAR(300) NOT NULL CHECK (btrim(title) <> ''),
    version_number INT NOT NULL DEFAULT 1 CHECK (version_number > 0),
    is_current BOOLEAN NOT NULL DEFAULT TRUE,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL,
    FOREIGN KEY (document_id, document_type_id) REFERENCES files.documents(id, document_type_id)
);

CREATE TABLE research.projects_documents (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    project_id BIGINT NOT NULL REFERENCES research.projects(id) ON DELETE RESTRICT,
    document_id BIGINT NOT NULL,
    document_type_id BIGINT NOT NULL,
    title VARCHAR(300) NOT NULL CHECK (btrim(title) <> ''),
    version_number INT NOT NULL DEFAULT 1 CHECK (version_number > 0),
    is_current BOOLEAN NOT NULL DEFAULT TRUE,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL,
    FOREIGN KEY (document_id, document_type_id) REFERENCES files.documents(id, document_type_id),
    UNIQUE (id, project_id, document_id)
);

CREATE TABLE research.projects_applications (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    project_id BIGINT NOT NULL REFERENCES research.projects(id) ON DELETE RESTRICT,
    call_id BIGINT NOT NULL REFERENCES calls.c_calls(id) ON DELETE RESTRICT,
    application_number VARCHAR(100) NULL,
    applied_at TIMESTAMPTZ NULL,
    requested_amount NUMERIC(18,2) NOT NULL DEFAULT 0 CHECK (requested_amount >= 0),
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL,
    UNIQUE (id, project_id)
);

CREATE TABLE research.projects_applications_documents (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    application_id BIGINT NOT NULL REFERENCES research.projects_applications(id) ON DELETE RESTRICT,
    document_id BIGINT NOT NULL,
    document_type_id BIGINT NOT NULL,
    title VARCHAR(300) NOT NULL CHECK (btrim(title) <> ''),
    version_number INT NOT NULL DEFAULT 1 CHECK (version_number > 0),
    is_current BOOLEAN NOT NULL DEFAULT TRUE,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL,
    FOREIGN KEY (document_id, document_type_id) REFERENCES files.documents(id, document_type_id)
);

CREATE TABLE research.projects_budget_items (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    project_id BIGINT NOT NULL REFERENCES research.projects(id) ON DELETE RESTRICT,
    budget_category_id BIGINT NOT NULL REFERENCES research.c_budget_categories(id) ON DELETE RESTRICT,
    description TEXT NOT NULL,
    quantity NUMERIC(12,3) NOT NULL CHECK (quantity > 0),
    unit_cost NUMERIC(18,2) NOT NULL CHECK (unit_cost >= 0),
    total_amount NUMERIC(24,2) GENERATED ALWAYS AS (round(quantity * unit_cost, 2)) STORED,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL,
    UNIQUE (id, project_id)
);

CREATE TABLE research.projects_budget_documents (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    budget_item_id BIGINT NOT NULL REFERENCES research.projects_budget_items(id) ON DELETE RESTRICT,
    document_id BIGINT NOT NULL,
    document_type_id BIGINT NOT NULL,
    title VARCHAR(300) NOT NULL CHECK (btrim(title) <> ''),
    version_number INT NOT NULL DEFAULT 1 CHECK (version_number > 0),
    is_current BOOLEAN NOT NULL DEFAULT TRUE,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL,
    FOREIGN KEY (document_id, document_type_id) REFERENCES files.documents(id, document_type_id)
);

CREATE TABLE research.projects_funding (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    project_id BIGINT NOT NULL REFERENCES research.projects(id) ON DELETE RESTRICT,
    funding_source_id BIGINT NOT NULL REFERENCES research.c_funding_sources(id) ON DELETE RESTRICT,
    application_id BIGINT NULL,
    reference_number VARCHAR(100) NULL,
    requested_amount NUMERIC(18,2) NOT NULL CHECK (requested_amount >= 0),
    approved_amount NUMERIC(18,2) NOT NULL DEFAULT 0 CHECK (approved_amount >= 0),
    received_amount NUMERIC(18,2) NOT NULL DEFAULT 0 CHECK (received_amount >= 0),
    awarded_on DATE NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL,
    UNIQUE (id, project_id),
    FOREIGN KEY (application_id, project_id) REFERENCES research.projects_applications(id, project_id)
);

CREATE TABLE research.projects_funding_documents (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    funding_id BIGINT NOT NULL REFERENCES research.projects_funding(id) ON DELETE RESTRICT,
    document_id BIGINT NOT NULL,
    document_type_id BIGINT NOT NULL,
    title VARCHAR(300) NOT NULL CHECK (btrim(title) <> ''),
    version_number INT NOT NULL DEFAULT 1 CHECK (version_number > 0),
    is_current BOOLEAN NOT NULL DEFAULT TRUE,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL,
    FOREIGN KEY (document_id, document_type_id) REFERENCES files.documents(id, document_type_id)
);

CREATE TABLE research.projects_expenses (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    budget_item_id BIGINT NOT NULL REFERENCES research.projects_budget_items(id) ON DELETE RESTRICT,
    funding_id BIGINT NULL REFERENCES research.projects_funding(id) ON DELETE RESTRICT,
    description TEXT NOT NULL,
    spent_on DATE NOT NULL,
    amount NUMERIC(18,2) NOT NULL CHECK (amount > 0),
    supplier_name VARCHAR(300) NULL,
    receipt_number VARCHAR(100) NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL
);

CREATE TABLE research.projects_expenses_documents (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    expense_id BIGINT NOT NULL REFERENCES research.projects_expenses(id) ON DELETE RESTRICT,
    document_id BIGINT NOT NULL,
    document_type_id BIGINT NOT NULL,
    title VARCHAR(300) NOT NULL CHECK (btrim(title) <> ''),
    version_number INT NOT NULL DEFAULT 1 CHECK (version_number > 0),
    is_current BOOLEAN NOT NULL DEFAULT TRUE,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL,
    FOREIGN KEY (document_id, document_type_id) REFERENCES files.documents(id, document_type_id)
);

CREATE TABLE research.research_outputs (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    project_id BIGINT NOT NULL REFERENCES research.projects(id) ON DELETE RESTRICT,
    output_type_id BIGINT NOT NULL REFERENCES research.c_research_outputs_types(id) ON DELETE RESTRICT,
    title VARCHAR(500) NOT NULL,
    description TEXT NULL,
    produced_on DATE NULL,
    doi VARCHAR(250) NULL,
    external_url TEXT NULL,
    bibliographic_citation TEXT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL
);

CREATE TABLE research.research_outputs_researchers (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    research_output_id BIGINT NOT NULL REFERENCES research.research_outputs(id) ON DELETE RESTRICT,
    researcher_id BIGINT NOT NULL REFERENCES academic.researchers(id) ON DELETE RESTRICT,
    author_order INT NOT NULL CHECK (author_order > 0),
    is_corresponding_author BOOLEAN NOT NULL DEFAULT FALSE,
    contribution TEXT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL
);

CREATE TABLE research.research_outputs_documents (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    research_output_id BIGINT NOT NULL REFERENCES research.research_outputs(id) ON DELETE RESTRICT,
    document_id BIGINT NOT NULL,
    document_type_id BIGINT NOT NULL,
    title VARCHAR(300) NOT NULL CHECK (btrim(title) <> ''),
    version_number INT NOT NULL DEFAULT 1 CHECK (version_number > 0),
    is_current BOOLEAN NOT NULL DEFAULT TRUE,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL,
    FOREIGN KEY (document_id, document_type_id) REFERENCES files.documents(id, document_type_id)
);







CREATE TABLE reviews.c_committee_roles (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    code VARCHAR(80) NOT NULL UNIQUE CHECK (btrim(code) <> ''),
    name VARCHAR(250) NOT NULL CHECK (btrim(name) <> ''),
    description TEXT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL
);

CREATE TABLE reviews.c_review_outcomes (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    code VARCHAR(80) NOT NULL UNIQUE CHECK (btrim(code) <> ''),
    name VARCHAR(250) NOT NULL CHECK (btrim(name) <> ''),
    description TEXT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL
);

CREATE TABLE reviews.c_academic_authorization_statuses (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    code VARCHAR(80) NOT NULL UNIQUE CHECK (btrim(code) <> ''),
    name VARCHAR(250) NOT NULL CHECK (btrim(name) <> ''),
    description TEXT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL
);

CREATE TABLE reviews.c_review_statuses (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    code VARCHAR(80) NOT NULL UNIQUE,
    name VARCHAR(250) NOT NULL,
    description TEXT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL
);

CREATE TABLE reviews.committees (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    name VARCHAR(300) NOT NULL,
    academic_program_id BIGINT NULL REFERENCES academic.c_academic_programs(id) ON DELETE RESTRICT,
    starts_on DATE NOT NULL,
    ends_on DATE NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL,
    CHECK (ends_on IS NULL OR ends_on >= starts_on)
);

CREATE TABLE reviews.committees_members (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    committee_id BIGINT NOT NULL REFERENCES reviews.committees(id) ON DELETE RESTRICT,
    user_id BIGINT NOT NULL REFERENCES access.users(id) ON DELETE RESTRICT,
    committee_role_id BIGINT NOT NULL REFERENCES reviews.c_committee_roles(id) ON DELETE RESTRICT,
    starts_on DATE NOT NULL,
    ends_on DATE NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL,
    UNIQUE (id, committee_id),
    CHECK (ends_on IS NULL OR ends_on >= starts_on)
);

CREATE TABLE reviews.projects_submissions (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    project_id BIGINT NOT NULL REFERENCES research.projects(id) ON DELETE RESTRICT,
    committee_id BIGINT NOT NULL REFERENCES reviews.committees(id) ON DELETE RESTRICT,
    application_id BIGINT NULL,
    version_number INT NOT NULL CHECK (version_number > 0),
    submitted_by BIGINT NOT NULL REFERENCES access.users(id) ON DELETE RESTRICT,
    submitted_at TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    processing_status VARCHAR(20) NOT NULL DEFAULT 'in_process' CHECK (processing_status IN ('in_process','completed','withdrawn')),
    snapshot JSONB NOT NULL CHECK (jsonb_typeof(snapshot) = 'object' AND snapshot <> '{}'::jsonb),
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL,
    UNIQUE (project_id, version_number),
    UNIQUE (id, committee_id),
    FOREIGN KEY (application_id, project_id) REFERENCES research.projects_applications(id, project_id)
);

CREATE TABLE reviews.submissions_documents (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    submission_id BIGINT NOT NULL REFERENCES reviews.projects_submissions(id) ON DELETE RESTRICT,
    project_document_id BIGINT NOT NULL REFERENCES research.projects_documents(id) ON DELETE RESTRICT,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL,
    UNIQUE (submission_id, project_document_id)
);

CREATE TABLE reviews.projects_reviews (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    submission_id BIGINT NOT NULL,
    committee_id BIGINT NOT NULL,
    committee_member_id BIGINT NOT NULL,
    status_id BIGINT NOT NULL REFERENCES reviews.c_review_statuses(id) ON DELETE RESTRICT,
    outcome_id BIGINT NULL REFERENCES reviews.c_review_outcomes(id) ON DELETE RESTRICT,
    assessment TEXT NULL,
    reviewed_at TIMESTAMPTZ NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL,
    FOREIGN KEY (submission_id, committee_id) REFERENCES reviews.projects_submissions(id, committee_id),
    FOREIGN KEY (committee_member_id, committee_id) REFERENCES reviews.committees_members(id, committee_id),
    UNIQUE (submission_id, committee_member_id)
);

CREATE TABLE reviews.academic_authorizations_history (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    submission_id BIGINT NOT NULL REFERENCES reviews.projects_submissions(id) ON DELETE RESTRICT,
    status_id BIGINT NOT NULL REFERENCES reviews.c_academic_authorization_statuses(id) ON DELETE RESTRICT,
    authorized_by BIGINT NOT NULL REFERENCES access.users(id) ON DELETE RESTRICT,
    decided_at TIMESTAMPTZ NOT NULL,
    rationale TEXT NOT NULL CHECK (btrim(rationale) <> ''),
    document_id BIGINT NOT NULL REFERENCES files.documents(id) ON DELETE RESTRICT,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL
);

CREATE TABLE access.audit_logs (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    actor_user_id BIGINT NULL REFERENCES access.users(id) ON DELETE RESTRICT,
    action VARCHAR(100) NOT NULL,
    entity_schema VARCHAR(63) NOT NULL,
    entity_table VARCHAR(63) NOT NULL,
    entity_id BIGINT NULL,
    project_id BIGINT NULL REFERENCES research.projects(id) ON DELETE RESTRICT,
    occurred_at TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    request_id VARCHAR(100) NULL,
    details JSONB NOT NULL DEFAULT '{}'::jsonb,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL
);

CREATE UNIQUE INDEX users_roles_live_uq ON access.users_roles (user_id, role_id) WHERE is_active AND deleted_at IS NULL;

CREATE UNIQUE INDEX roles_permissions_live_uq ON access.roles_permissions (role_id, permission_id) WHERE is_active AND deleted_at IS NULL;

CREATE UNIQUE INDEX researchers_organizations_live_uq ON academic.researchers_organizations (researcher_id, organization_id) WHERE is_active AND deleted_at IS NULL;

CREATE UNIQUE INDEX researchers_research_lines_live_uq ON academic.researchers_research_lines (researcher_id, research_line_id) WHERE is_active AND deleted_at IS NULL;

CREATE UNIQUE INDEX students_live_uq ON academic.students (academic_program_id, student_number) WHERE is_active AND deleted_at IS NULL;

CREATE UNIQUE INDEX c_document_templates_current_uq ON files.c_document_templates (code) WHERE is_current AND deleted_at IS NULL;

CREATE UNIQUE INDEX calls_document_requirements_live_uq ON calls.calls_document_requirements (call_id, document_type_id, title, workflow_stage) WHERE is_active AND deleted_at IS NULL;

CREATE UNIQUE INDEX projects_researchers_live_uq ON research.projects_researchers (project_id, researcher_id) WHERE is_active AND deleted_at IS NULL;

CREATE UNIQUE INDEX projects_leads_participant_current_uq ON research.projects_leads (project_researcher_id) WHERE is_current AND deleted_at IS NULL;

CREATE UNIQUE INDEX projects_students_live_uq ON research.projects_students (project_id, student_id) WHERE is_active AND deleted_at IS NULL;

CREATE UNIQUE INDEX projects_research_lines_live_uq ON research.projects_research_lines (project_id, research_line_id) WHERE is_active AND deleted_at IS NULL;

CREATE UNIQUE INDEX objectives_general_uq ON research.objectives (project_id) WHERE objective_type = 'general' AND is_active AND deleted_at IS NULL;

CREATE UNIQUE INDEX objectives_status_history_documents_current_uq ON research.objectives_status_history_documents (status_history_id, lower(btrim(title)), document_type_id) WHERE is_current AND deleted_at IS NULL;

CREATE UNIQUE INDEX objectives_status_history_documents_version_uq ON research.objectives_status_history_documents (status_history_id, lower(btrim(title)), document_type_id, version_number);

CREATE UNIQUE INDEX milestones_status_history_documents_current_uq ON research.milestones_status_history_documents (status_history_id, lower(btrim(title)), document_type_id) WHERE is_current AND deleted_at IS NULL;

CREATE UNIQUE INDEX milestones_status_history_documents_version_uq ON research.milestones_status_history_documents (status_history_id, lower(btrim(title)), document_type_id, version_number);

CREATE UNIQUE INDEX tasks_status_history_documents_current_uq ON research.tasks_status_history_documents (status_history_id, lower(btrim(title)), document_type_id) WHERE is_current AND deleted_at IS NULL;

CREATE UNIQUE INDEX tasks_status_history_documents_version_uq ON research.tasks_status_history_documents (status_history_id, lower(btrim(title)), document_type_id, version_number);

CREATE UNIQUE INDEX projects_documents_current_uq ON research.projects_documents (project_id, lower(btrim(title)), document_type_id) WHERE is_current AND deleted_at IS NULL;

CREATE UNIQUE INDEX projects_documents_version_uq ON research.projects_documents (project_id, lower(btrim(title)), document_type_id, version_number);

CREATE UNIQUE INDEX projects_applications_live_uq ON research.projects_applications (project_id, call_id) WHERE is_active AND deleted_at IS NULL;

CREATE UNIQUE INDEX projects_applications_documents_current_uq ON research.projects_applications_documents (application_id, lower(btrim(title)), document_type_id) WHERE is_current AND deleted_at IS NULL;

CREATE UNIQUE INDEX projects_applications_documents_version_uq ON research.projects_applications_documents (application_id, lower(btrim(title)), document_type_id, version_number);

CREATE UNIQUE INDEX projects_budget_documents_current_uq ON research.projects_budget_documents (budget_item_id, lower(btrim(title)), document_type_id) WHERE is_current AND deleted_at IS NULL;

CREATE UNIQUE INDEX projects_budget_documents_version_uq ON research.projects_budget_documents (budget_item_id, lower(btrim(title)), document_type_id, version_number);

CREATE UNIQUE INDEX projects_funding_documents_current_uq ON research.projects_funding_documents (funding_id, lower(btrim(title)), document_type_id) WHERE is_current AND deleted_at IS NULL;

CREATE UNIQUE INDEX projects_funding_documents_version_uq ON research.projects_funding_documents (funding_id, lower(btrim(title)), document_type_id, version_number);

CREATE UNIQUE INDEX projects_expenses_documents_current_uq ON research.projects_expenses_documents (expense_id, lower(btrim(title)), document_type_id) WHERE is_current AND deleted_at IS NULL;

CREATE UNIQUE INDEX projects_expenses_documents_version_uq ON research.projects_expenses_documents (expense_id, lower(btrim(title)), document_type_id, version_number);

CREATE UNIQUE INDEX research_outputs_researchers_live_uq ON research.research_outputs_researchers (research_output_id, researcher_id) WHERE is_active AND deleted_at IS NULL;

CREATE UNIQUE INDEX research_outputs_researchers_author_order_uq ON research.research_outputs_researchers (research_output_id, author_order) WHERE is_active AND deleted_at IS NULL;

CREATE UNIQUE INDEX research_outputs_documents_current_uq ON research.research_outputs_documents (research_output_id, lower(btrim(title)), document_type_id) WHERE is_current AND deleted_at IS NULL;

CREATE UNIQUE INDEX research_outputs_documents_version_uq ON research.research_outputs_documents (research_output_id, lower(btrim(title)), document_type_id, version_number);





CREATE UNIQUE INDEX committees_members_live_uq ON reviews.committees_members (committee_id, user_id) WHERE is_active AND deleted_at IS NULL;

CREATE UNIQUE INDEX committees_members_role_uq ON reviews.committees_members (committee_id, committee_role_id) WHERE is_active AND deleted_at IS NULL;

CREATE UNIQUE INDEX projects_submissions_in_process_uq ON reviews.projects_submissions (project_id) WHERE processing_status = 'in_process' AND deleted_at IS NULL;

-- Cuerpos académicos: información básica, afiliación opcional y grado de consolidación.
-- La pertenencia NO asigna roles de acceso. Los proyectos se vinculan explícitamente.
-- No se exige un número mínimo de integrantes ni se implementa el trámite PRODEP.
CREATE TABLE academic.c_academic_group_levels (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    code VARCHAR(80) NOT NULL UNIQUE CHECK (btrim(code) <> ''),
    name VARCHAR(250) NOT NULL CHECK (btrim(name) <> ''),
    description TEXT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL
);
CREATE TABLE academic.academic_groups (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    code VARCHAR(100) NOT NULL UNIQUE CHECK (btrim(code) <> ''),
    name VARCHAR(300) NOT NULL CHECK (btrim(name) <> ''),
    organization_id BIGINT NOT NULL REFERENCES academic.c_organizations(id) ON DELETE RESTRICT,
    description TEXT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL
);
CREATE TABLE academic.academic_groups_researchers (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    academic_group_id BIGINT NOT NULL REFERENCES academic.academic_groups(id) ON DELETE RESTRICT,
    researcher_id BIGINT NOT NULL REFERENCES academic.researchers(id) ON DELETE RESTRICT,
    starts_on DATE NOT NULL,
    ends_on DATE NULL,
    is_leader BOOLEAN NOT NULL DEFAULT FALSE,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL,
    CHECK (ends_on IS NULL OR ends_on >= starts_on),
    UNIQUE (academic_group_id, researcher_id, starts_on)
);
CREATE TABLE academic.academic_groups_status_history (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    academic_group_id BIGINT NOT NULL REFERENCES academic.academic_groups(id) ON DELETE RESTRICT,
    level_id BIGINT NOT NULL REFERENCES academic.c_academic_group_levels(id) ON DELETE RESTRICT,
    changed_by BIGINT NOT NULL REFERENCES access.users(id) ON DELETE RESTRICT,
    changed_at TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    reason TEXT NOT NULL CHECK (btrim(reason) <> ''),
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL
);
CREATE TABLE research.projects_academic_groups (
    id BIGINT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    project_id BIGINT NOT NULL REFERENCES research.projects(id) ON DELETE RESTRICT,
    academic_group_id BIGINT NOT NULL REFERENCES academic.academic_groups(id) ON DELETE RESTRICT,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    is_visible BOOLEAN NOT NULL DEFAULT TRUE,
    display_order INT NOT NULL DEFAULT 0,
    created_by BIGINT NULL,
    updated_by BIGINT NULL,
    deleted_by BIGINT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    deleted_at TIMESTAMP WITH TIME ZONE NULL,
    notes TEXT NULL
);

CREATE UNIQUE INDEX academic_groups_researchers_current_uq
ON academic.academic_groups_researchers(academic_group_id,researcher_id)
WHERE ends_on IS NULL AND is_active AND deleted_at IS NULL;
CREATE UNIQUE INDEX academic_groups_researchers_leader_uq
ON academic.academic_groups_researchers(academic_group_id)
WHERE is_leader AND ends_on IS NULL AND is_active AND deleted_at IS NULL;
CREATE UNIQUE INDEX projects_academic_groups_live_uq
ON research.projects_academic_groups(project_id,academic_group_id) WHERE is_active AND deleted_at IS NULL;
CREATE INDEX academic_groups_status_history_latest_idx
ON academic.academic_groups_status_history(academic_group_id,changed_at DESC,id DESC);
-- El grado vigente se consulta en el último registro del historial, sin duplicarlo
-- en academic_groups. Para conocer el grado en una fecha, filtrar changed_at <= fecha.

-- Índices para las claves foráneas funcionales. No hay FKs de created_by/updated_by/deleted_by.
DO $$ DECLARE r RECORD; BEGIN
 FOR r IN SELECT c.conrelid::regclass tbl,c.oid,string_agg(quote_ident(a.attname),', ' ORDER BY k.ord) cols
 FROM pg_constraint c JOIN pg_class t ON t.oid=c.conrelid JOIN pg_namespace n ON n.oid=t.relnamespace
 CROSS JOIN LATERAL unnest(c.conkey) WITH ORDINALITY k(attnum,ord)
 JOIN pg_attribute a ON a.attrelid=c.conrelid AND a.attnum=k.attnum
 WHERE c.contype='f' AND n.nspname IN ('access','academic','calls','research','reviews','files') GROUP BY c.conrelid,c.oid
 LOOP EXECUTE format('CREATE INDEX %I ON %s (%s)','fk_'||r.oid||'_idx',r.tbl,r.cols); END LOOP;
END $$;
CREATE FUNCTION access.touch_updated_at() RETURNS TRIGGER LANGUAGE plpgsql AS $$
BEGIN NEW.updated_at:=clock_timestamp(); RETURN NEW; END $$;
CREATE FUNCTION access.preserve_history() RETURNS TRIGGER LANGUAGE plpgsql AS $$
BEGIN RAISE EXCEPTION '%.% es histórico/inmutable; agregue otro registro',TG_TABLE_SCHEMA,TG_TABLE_NAME; END $$;

CREATE TRIGGER set_updated_at BEFORE UPDATE ON access.users FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON access.c_roles FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON access.c_permissions FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON access.users_roles FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON access.roles_permissions FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON academic.c_organization_types FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON academic.c_organizations FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON academic.c_academic_programs FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON academic.c_academic_fields FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON academic.c_research_lines FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON academic.c_degree_levels FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON academic.researchers FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON academic.researchers_organizations FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON academic.researchers_degrees FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON academic.researchers_experiences FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON academic.researchers_research_lines FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON academic.students FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON calls.c_call_types FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON calls.c_calls FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON files.c_document_templates FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON files.c_document_types FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON files.documents FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON calls.calls_document_requirements FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON calls.calls_requirements FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON research.c_currencies FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON research.c_budget_categories FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON research.c_funding_sources FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON research.c_researchers_roles FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON research.c_project_statuses FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON research.c_objective_statuses FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON research.c_milestone_statuses FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON research.c_research_outputs_types FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON research.c_task_statuses FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON research.projects FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON research.projects_researchers FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON research.projects_leads FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON research.projects_students FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON research.projects_research_lines FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON research.objectives FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON research.milestones FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON research.tasks FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON research.projects_status_history FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON research.objectives_status_history FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON research.objectives_status_history_documents FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON research.milestones_status_history FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON research.milestones_status_history_documents FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON research.tasks_status_history FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON research.tasks_status_history_documents FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON research.projects_documents FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON research.projects_applications FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON research.projects_applications_documents FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON research.projects_budget_items FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON research.projects_budget_documents FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON research.projects_funding FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON research.projects_funding_documents FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON research.projects_expenses FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON research.projects_expenses_documents FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON research.research_outputs FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON research.research_outputs_researchers FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON research.research_outputs_documents FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();




CREATE TRIGGER set_updated_at BEFORE UPDATE ON reviews.c_committee_roles FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON reviews.c_review_outcomes FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON reviews.c_academic_authorization_statuses FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON reviews.c_review_statuses FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON reviews.committees FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON reviews.committees_members FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON reviews.projects_submissions FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON reviews.submissions_documents FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON reviews.projects_reviews FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON reviews.academic_authorizations_history FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER set_updated_at BEFORE UPDATE ON access.audit_logs FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER preserve_history BEFORE UPDATE OR DELETE ON access.audit_logs FOR EACH ROW EXECUTE FUNCTION access.preserve_history();

CREATE TRIGGER preserve_history BEFORE UPDATE OR DELETE ON files.documents FOR EACH ROW EXECUTE FUNCTION access.preserve_history();

CREATE TRIGGER preserve_history BEFORE UPDATE OR DELETE ON research.projects_status_history FOR EACH ROW EXECUTE FUNCTION access.preserve_history();

CREATE TRIGGER preserve_history BEFORE UPDATE OR DELETE ON research.objectives_status_history FOR EACH ROW EXECUTE FUNCTION access.preserve_history();

CREATE TRIGGER preserve_history BEFORE UPDATE OR DELETE ON research.milestones_status_history FOR EACH ROW EXECUTE FUNCTION access.preserve_history();

CREATE TRIGGER preserve_history BEFORE UPDATE OR DELETE ON research.tasks_status_history FOR EACH ROW EXECUTE FUNCTION access.preserve_history();

CREATE TRIGGER preserve_history BEFORE UPDATE OR DELETE ON reviews.academic_authorizations_history FOR EACH ROW EXECUTE FUNCTION access.preserve_history();

CREATE VIEW research.projects_current_status AS SELECT DISTINCT ON (project_id) * FROM research.projects_status_history ORDER BY project_id,changed_at DESC,id DESC;

CREATE VIEW research.objectives_current_status AS SELECT DISTINCT ON (objective_id) * FROM research.objectives_status_history ORDER BY objective_id,changed_at DESC,id DESC;

CREATE VIEW research.milestones_current_status AS SELECT DISTINCT ON (milestone_id) * FROM research.milestones_status_history ORDER BY milestone_id,changed_at DESC,id DESC;

CREATE VIEW research.tasks_current_status AS SELECT DISTINCT ON (task_id) * FROM research.tasks_status_history ORDER BY task_id,changed_at DESC,id DESC;

CREATE TRIGGER set_updated_at BEFORE UPDATE ON academic.c_academic_group_levels FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();
CREATE TRIGGER set_updated_at BEFORE UPDATE ON academic.academic_groups FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();
CREATE TRIGGER set_updated_at BEFORE UPDATE ON academic.academic_groups_researchers FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();
CREATE TRIGGER set_updated_at BEFORE UPDATE ON academic.academic_groups_status_history FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();
CREATE TRIGGER set_updated_at BEFORE UPDATE ON research.projects_academic_groups FOR EACH ROW EXECUTE FUNCTION access.touch_updated_at();

CREATE TRIGGER preserve_history BEFORE UPDATE OR DELETE ON academic.academic_groups_status_history
FOR EACH ROW EXECUTE FUNCTION access.preserve_history();

CREATE FUNCTION academic.validate_group_membership() RETURNS TRIGGER LANGUAGE plpgsql AS $$
BEGIN
 IF TG_OP='DELETE' THEN RAISE EXCEPTION 'Para dar de baja un integrante registre ends_on; conserve su historial'; END IF;
 IF TG_OP='UPDATE' THEN
  IF NEW.academic_group_id<>OLD.academic_group_id OR NEW.researcher_id<>OLD.researcher_id OR NEW.starts_on<>OLD.starts_on THEN
   RAISE EXCEPTION 'No cambie la identidad de una afiliación; cierre el periodo e inserte otro'; END IF;
 END IF;
 -- Serializar cambios de miembros del mismo cuerpo académico.
 UPDATE academic.academic_groups SET updated_at=updated_at WHERE id=NEW.academic_group_id;
 IF NEW.is_active AND NEW.deleted_at IS NULL AND EXISTS(
  SELECT 1 FROM academic.academic_groups_researchers m
  WHERE m.academic_group_id=NEW.academic_group_id AND m.researcher_id=NEW.researcher_id AND m.id<>NEW.id
   AND m.is_active AND m.deleted_at IS NULL
   AND daterange(m.starts_on,m.ends_on,'[]') && daterange(NEW.starts_on,NEW.ends_on,'[]')
 ) THEN RAISE EXCEPTION 'Los periodos de pertenencia del mismo investigador y cuerpo no pueden solaparse'; END IF;
 IF NEW.is_leader AND NEW.is_active AND NEW.deleted_at IS NULL AND EXISTS(
  SELECT 1 FROM academic.academic_groups_researchers m WHERE m.academic_group_id=NEW.academic_group_id
   AND m.id<>NEW.id AND m.is_leader AND m.is_active AND m.deleted_at IS NULL
   AND daterange(m.starts_on,m.ends_on,'[]') && daterange(NEW.starts_on,NEW.ends_on,'[]')
 ) THEN RAISE EXCEPTION 'Los periodos de liderazgo del mismo cuerpo no pueden solaparse'; END IF;
 RETURN NEW;
END $$;
CREATE TRIGGER validate_group_membership BEFORE INSERT OR UPDATE OR DELETE ON academic.academic_groups_researchers
FOR EACH ROW EXECUTE FUNCTION academic.validate_group_membership();

CREATE FUNCTION academic.validate_group_status() RETURNS TRIGGER LANGUAGE plpgsql AS $$
DECLARE previous_at TIMESTAMPTZ;
BEGIN
 UPDATE academic.academic_groups SET updated_at=updated_at WHERE id=NEW.academic_group_id;
 IF NOT FOUND THEN RAISE EXCEPTION 'El cuerpo académico no existe'; END IF;
 IF NOT EXISTS(SELECT 1 FROM academic.c_academic_group_levels WHERE id=NEW.level_id AND is_active AND deleted_at IS NULL) THEN
  RAISE EXCEPTION 'Grado de consolidación inexistente o inactivo'; END IF;
 SELECT max(changed_at) INTO previous_at FROM academic.academic_groups_status_history WHERE academic_group_id=NEW.academic_group_id;
 IF NEW.changed_at<previous_at THEN RAISE EXCEPTION 'Fecha del cambio de grado fuera de secuencia'; END IF;
 RETURN NEW;
END $$;
CREATE TRIGGER validate_group_status BEFORE INSERT ON academic.academic_groups_status_history
FOR EACH ROW EXECUTE FUNCTION academic.validate_group_status();

-- No se cambia de propietario una fila ya creada: evita cruces posteriores de proyectos.
CREATE FUNCTION research.guard_parent() RETURNS TRIGGER LANGUAGE plpgsql AS $$
BEGIN IF (to_jsonb(NEW)->TG_ARGV[0]) IS DISTINCT FROM (to_jsonb(OLD)->TG_ARGV[0]) THEN
 RAISE EXCEPTION 'No se permite cambiar el vínculo padre de %.%',TG_TABLE_SCHEMA,TG_TABLE_NAME; END IF;
 RETURN NEW; END $$;

CREATE TRIGGER guard_parent BEFORE UPDATE ON research.projects_researchers FOR EACH ROW EXECUTE FUNCTION research.guard_parent('project_id');

CREATE TRIGGER guard_parent BEFORE UPDATE ON research.projects_budget_items FOR EACH ROW EXECUTE FUNCTION research.guard_parent('project_id');

CREATE TRIGGER guard_parent BEFORE UPDATE ON research.projects_funding FOR EACH ROW EXECUTE FUNCTION research.guard_parent('project_id');

CREATE TRIGGER guard_parent BEFORE UPDATE ON research.projects_applications FOR EACH ROW EXECUTE FUNCTION research.guard_parent('project_id');

CREATE TRIGGER guard_parent BEFORE UPDATE ON research.objectives FOR EACH ROW EXECUTE FUNCTION research.guard_parent('project_id');

CREATE TRIGGER guard_parent BEFORE UPDATE ON research.milestones FOR EACH ROW EXECUTE FUNCTION research.guard_parent('objective_id');

CREATE TRIGGER guard_parent BEFORE UPDATE ON research.tasks FOR EACH ROW EXECUTE FUNCTION research.guard_parent('milestone_id');

CREATE FUNCTION research.validate_task_responsible() RETURNS TRIGGER LANGUAGE plpgsql AS $$
DECLARE p BIGINT; BEGIN
 SELECT o.project_id INTO p FROM research.milestones m JOIN research.objectives o ON o.id=m.objective_id WHERE m.id=NEW.milestone_id;
 IF NEW.responsible_participant_id IS NOT NULL AND NOT EXISTS (
 SELECT 1 FROM research.projects_researchers WHERE id=NEW.responsible_participant_id AND project_id=p) THEN
  RAISE EXCEPTION 'El responsable de la tarea debe participar en el mismo proyecto'; END IF;
 RETURN NEW; END $$;
CREATE TRIGGER validate_task_responsible BEFORE INSERT OR UPDATE ON research.tasks FOR EACH ROW EXECUTE FUNCTION research.validate_task_responsible();
CREATE FUNCTION research.validate_expense() RETURNS TRIGGER LANGUAGE plpgsql AS $$
DECLARE p BIGINT; BEGIN
 SELECT project_id INTO p FROM research.projects_budget_items WHERE id=NEW.budget_item_id;
 IF NEW.funding_id IS NOT NULL AND NOT EXISTS (SELECT 1 FROM research.projects_funding WHERE id=NEW.funding_id AND project_id=p) THEN
  RAISE EXCEPTION 'El financiamiento del gasto debe pertenecer al proyecto de la partida'; END IF;
 RETURN NEW; END $$;
CREATE TRIGGER validate_expense BEFORE INSERT OR UPDATE ON research.projects_expenses FOR EACH ROW EXECUTE FUNCTION research.validate_expense();

-- Las responsabilidades se relacionan SOLO con projects_researchers.
-- Se serializan los cambios sobre el proyecto y se comprueba exactamente uno al COMMIT.
CREATE FUNCTION research.guard_lead() RETURNS TRIGGER LANGUAGE plpgsql AS $$
DECLARE p BIGINT; BEGIN
 IF TG_OP='DELETE' THEN RAISE EXCEPTION 'No se elimina el historial de responsables'; END IF;
 IF TG_OP='UPDATE' THEN
  IF NOT OLD.is_current OR NEW.project_researcher_id<>OLD.project_researcher_id OR NEW.starts_at<>OLD.starts_at
   OR NOT NEW.is_active OR NEW.deleted_at IS NOT NULL THEN RAISE EXCEPTION 'Solo se permite cerrar el periodo de un responsable vigente'; END IF;
 END IF;
 SELECT project_id INTO p FROM research.projects_researchers WHERE id=NEW.project_researcher_id;
 UPDATE research.projects SET updated_at=updated_at WHERE id=p;
 RETURN NEW; END $$;
CREATE TRIGGER guard_lead BEFORE INSERT OR UPDATE OR DELETE ON research.projects_leads FOR EACH ROW EXECUTE FUNCTION research.guard_lead();
CREATE FUNCTION research.check_project_lead() RETURNS TRIGGER LANGUAGE plpgsql AS $$
DECLARE p BIGINT;n INT;BEGIN
 IF TG_TABLE_NAME='projects' THEN p:=COALESCE(NEW.id,OLD.id);
 ELSIF TG_TABLE_NAME='projects_researchers' THEN p:=COALESCE(NEW.project_id,OLD.project_id);
 ELSE SELECT project_id INTO p FROM research.projects_researchers WHERE id=COALESCE(NEW.project_researcher_id,OLD.project_researcher_id); END IF;
 IF EXISTS(SELECT 1 FROM research.projects WHERE id=p AND is_active AND deleted_at IS NULL) THEN
  SELECT count(*) INTO n FROM research.projects_leads l JOIN research.projects_researchers pr ON pr.id=l.project_researcher_id
  WHERE pr.project_id=p AND l.is_current AND l.is_active AND l.deleted_at IS NULL AND pr.is_active AND pr.deleted_at IS NULL AND pr.ends_on IS NULL;
  IF n<>1 THEN RAISE EXCEPTION 'El proyecto % requiere exactamente un responsable vigente y participante activo',p;END IF;
 END IF;RETURN NULL; END $$;
CREATE CONSTRAINT TRIGGER require_project_lead AFTER INSERT OR UPDATE ON research.projects DEFERRABLE INITIALLY DEFERRED FOR EACH ROW EXECUTE FUNCTION research.check_project_lead();
CREATE CONSTRAINT TRIGGER require_project_lead AFTER INSERT OR UPDATE OR DELETE ON research.projects_leads DEFERRABLE INITIALLY DEFERRED FOR EACH ROW EXECUTE FUNCTION research.check_project_lead();
CREATE CONSTRAINT TRIGGER require_project_lead AFTER UPDATE OR DELETE ON research.projects_researchers DEFERRABLE INITIALLY DEFERRED FOR EACH ROW EXECUTE FUNCTION research.check_project_lead();

-- Una sola versión vigente por documento lógico en su contexto propietario.
-- document_type_id en las relaciones es una parte verificada de la FK al archivo,
-- permite índices únicos por propietario+título+tipo y no tiene FK adicional al catálogo.
CREATE FUNCTION files.guard_document_version() RETURNS TRIGGER LANGUAGE plpgsql AS $$
BEGIN
 IF TG_OP='DELETE' THEN RAISE EXCEPTION 'No se eliminan versiones documentales';END IF;
 IF (to_jsonb(NEW)-ARRAY['is_current','is_active','is_visible','display_order','updated_by','updated_at','deleted_by','deleted_at','notes']) IS DISTINCT FROM
    (to_jsonb(OLD)-ARRAY['is_current','is_active','is_visible','display_order','updated_by','updated_at','deleted_by','deleted_at','notes']) THEN
  RAISE EXCEPTION 'Inserte una nueva versión para cambiar el documento';END IF;RETURN NEW;END $$;
CREATE FUNCTION files.require_current_version() RETURNS TRIGGER LANGUAGE plpgsql AS $$
DECLARE n INT;total INT;owner_id BIGINT;BEGIN
 IF TG_TABLE_NAME='c_document_templates' THEN
 SELECT count(*),count(*) FILTER(WHERE is_current) INTO total,n FROM files.c_document_templates WHERE code=NEW.code AND deleted_at IS NULL;
 ELSE
 owner_id:=(to_jsonb(NEW)->>TG_ARGV[0])::bigint;
 EXECUTE format('SELECT count(*),count(*) FILTER(WHERE is_current) FROM %I.%I WHERE %I=$1 AND lower(btrim(title))=lower(btrim($2)) AND document_type_id=$3 AND deleted_at IS NULL',TG_TABLE_SCHEMA,TG_TABLE_NAME,TG_ARGV[0])
 INTO total,n USING owner_id,NEW.title,NEW.document_type_id;
 END IF;
 IF total>0 AND n<>1 THEN RAISE EXCEPTION 'Cada documento debe tener exactamente una versión vigente';END IF;RETURN NULL;END $$;

CREATE TRIGGER immutable_version BEFORE UPDATE OR DELETE ON files.c_document_templates FOR EACH ROW EXECUTE FUNCTION files.guard_document_version();

CREATE CONSTRAINT TRIGGER require_current_version AFTER INSERT OR UPDATE ON files.c_document_templates DEFERRABLE INITIALLY DEFERRED FOR EACH ROW EXECUTE FUNCTION files.require_current_version('code');

CREATE TRIGGER immutable_version BEFORE UPDATE OR DELETE ON research.objectives_status_history_documents FOR EACH ROW EXECUTE FUNCTION files.guard_document_version();

CREATE CONSTRAINT TRIGGER require_current_version AFTER INSERT OR UPDATE ON research.objectives_status_history_documents DEFERRABLE INITIALLY DEFERRED FOR EACH ROW EXECUTE FUNCTION files.require_current_version('status_history_id');

CREATE TRIGGER immutable_version BEFORE UPDATE OR DELETE ON research.milestones_status_history_documents FOR EACH ROW EXECUTE FUNCTION files.guard_document_version();

CREATE CONSTRAINT TRIGGER require_current_version AFTER INSERT OR UPDATE ON research.milestones_status_history_documents DEFERRABLE INITIALLY DEFERRED FOR EACH ROW EXECUTE FUNCTION files.require_current_version('status_history_id');

CREATE TRIGGER immutable_version BEFORE UPDATE OR DELETE ON research.tasks_status_history_documents FOR EACH ROW EXECUTE FUNCTION files.guard_document_version();

CREATE CONSTRAINT TRIGGER require_current_version AFTER INSERT OR UPDATE ON research.tasks_status_history_documents DEFERRABLE INITIALLY DEFERRED FOR EACH ROW EXECUTE FUNCTION files.require_current_version('status_history_id');

CREATE TRIGGER immutable_version BEFORE UPDATE OR DELETE ON research.projects_documents FOR EACH ROW EXECUTE FUNCTION files.guard_document_version();

CREATE CONSTRAINT TRIGGER require_current_version AFTER INSERT OR UPDATE ON research.projects_documents DEFERRABLE INITIALLY DEFERRED FOR EACH ROW EXECUTE FUNCTION files.require_current_version('project_id');

CREATE TRIGGER immutable_version BEFORE UPDATE OR DELETE ON research.projects_applications_documents FOR EACH ROW EXECUTE FUNCTION files.guard_document_version();

CREATE CONSTRAINT TRIGGER require_current_version AFTER INSERT OR UPDATE ON research.projects_applications_documents DEFERRABLE INITIALLY DEFERRED FOR EACH ROW EXECUTE FUNCTION files.require_current_version('application_id');

CREATE TRIGGER immutable_version BEFORE UPDATE OR DELETE ON research.projects_budget_documents FOR EACH ROW EXECUTE FUNCTION files.guard_document_version();

CREATE CONSTRAINT TRIGGER require_current_version AFTER INSERT OR UPDATE ON research.projects_budget_documents DEFERRABLE INITIALLY DEFERRED FOR EACH ROW EXECUTE FUNCTION files.require_current_version('budget_item_id');

CREATE TRIGGER immutable_version BEFORE UPDATE OR DELETE ON research.projects_funding_documents FOR EACH ROW EXECUTE FUNCTION files.guard_document_version();

CREATE CONSTRAINT TRIGGER require_current_version AFTER INSERT OR UPDATE ON research.projects_funding_documents DEFERRABLE INITIALLY DEFERRED FOR EACH ROW EXECUTE FUNCTION files.require_current_version('funding_id');

CREATE TRIGGER immutable_version BEFORE UPDATE OR DELETE ON research.projects_expenses_documents FOR EACH ROW EXECUTE FUNCTION files.guard_document_version();

CREATE CONSTRAINT TRIGGER require_current_version AFTER INSERT OR UPDATE ON research.projects_expenses_documents DEFERRABLE INITIALLY DEFERRED FOR EACH ROW EXECUTE FUNCTION files.require_current_version('expense_id');

CREATE TRIGGER immutable_version BEFORE UPDATE OR DELETE ON research.research_outputs_documents FOR EACH ROW EXECUTE FUNCTION files.guard_document_version();

CREATE CONSTRAINT TRIGGER require_current_version AFTER INSERT OR UPDATE ON research.research_outputs_documents DEFERRABLE INITIALLY DEFERRED FOR EACH ROW EXECUTE FUNCTION files.require_current_version('research_output_id');



-- El snapshot y los documentos de propuesta se conservan aunque se edite el proyecto.
CREATE FUNCTION reviews.capture_project_snapshot(p BIGINT,a BIGINT) RETURNS JSONB LANGUAGE sql STABLE AS $$
SELECT jsonb_build_object(
 'project',to_jsonb(pr),
 'participants',(SELECT coalesce(jsonb_agg(to_jsonb(r)),'[]'::jsonb) FROM research.projects_researchers r WHERE r.project_id=p),
 'lead_history',(SELECT coalesce(jsonb_agg(to_jsonb(l)),'[]'::jsonb) FROM research.projects_leads l JOIN research.projects_researchers r ON r.id=l.project_researcher_id WHERE r.project_id=p),
 'students',(SELECT coalesce(jsonb_agg(to_jsonb(s)),'[]'::jsonb) FROM research.projects_students s WHERE s.project_id=p),
 'objectives',(SELECT coalesce(jsonb_agg(to_jsonb(o)),'[]'::jsonb) FROM research.objectives o WHERE o.project_id=p),
 'milestones',(SELECT coalesce(jsonb_agg(to_jsonb(m)),'[]'::jsonb) FROM research.milestones m JOIN research.objectives o ON o.id=m.objective_id WHERE o.project_id=p),
 'tasks',(SELECT coalesce(jsonb_agg(to_jsonb(t)),'[]'::jsonb) FROM research.tasks t JOIN research.milestones m ON m.id=t.milestone_id JOIN research.objectives o ON o.id=m.objective_id WHERE o.project_id=p),
 'budget',(SELECT coalesce(jsonb_agg(to_jsonb(b)),'[]'::jsonb) FROM research.projects_budget_items b WHERE b.project_id=p),
 'funding',(SELECT coalesce(jsonb_agg(to_jsonb(f)),'[]'::jsonb) FROM research.projects_funding f WHERE f.project_id=p),
 'research_lines',(SELECT coalesce(jsonb_agg(to_jsonb(l)),'[]'::jsonb) FROM research.projects_research_lines l WHERE l.project_id=p),
 'academic_groups',(SELECT coalesce(jsonb_agg(jsonb_build_object(
  'association',to_jsonb(pg),'group',to_jsonb(g),
  'memberships',(SELECT coalesce(jsonb_agg(to_jsonb(gm)),'[]'::jsonb) FROM academic.academic_groups_researchers gm WHERE gm.academic_group_id=g.id AND gm.is_active AND gm.deleted_at IS NULL),
  'level_history',(SELECT coalesce(jsonb_agg(to_jsonb(gh)),'[]'::jsonb) FROM academic.academic_groups_status_history gh WHERE gh.academic_group_id=g.id)
 )),'[]'::jsonb) FROM research.projects_academic_groups pg JOIN academic.academic_groups g ON g.id=pg.academic_group_id
 WHERE pg.project_id=p AND pg.is_active AND pg.deleted_at IS NULL),
 'application',(SELECT to_jsonb(ap) FROM research.projects_applications ap WHERE ap.id=a),
 'document_requirements',(SELECT coalesce(jsonb_agg(to_jsonb(r)),'[]'::jsonb) FROM calls.calls_document_requirements r JOIN research.projects_applications ap ON ap.call_id=r.call_id WHERE ap.id=a),
 'general_requirements',(SELECT coalesce(jsonb_agg(to_jsonb(r)),'[]'::jsonb) FROM calls.calls_requirements r JOIN research.projects_applications ap ON ap.call_id=r.call_id WHERE ap.id=a),
 'documents',(SELECT coalesce(jsonb_agg(jsonb_build_object('association',to_jsonb(pd),'file',to_jsonb(d))),'[]'::jsonb) FROM research.projects_documents pd JOIN files.documents d ON d.id=pd.document_id WHERE pd.project_id=p AND pd.is_current AND pd.is_active AND pd.deleted_at IS NULL)
) FROM research.projects pr WHERE pr.id=p $$;

-- El envío conserva la propuesta original. Las decisiones NO se almacenan aquí.
CREATE FUNCTION reviews.validate_submission() RETURNS TRIGGER LANGUAGE plpgsql AS $$
BEGIN
 IF TG_OP='DELETE' THEN RAISE EXCEPTION 'No se eliminan envíos'; END IF;
 IF TG_OP='INSERT' THEN
  UPDATE research.projects SET updated_at=updated_at WHERE id=NEW.project_id;
  IF NOT EXISTS(SELECT 1 FROM research.projects WHERE id=NEW.project_id AND is_managed_in_platform) THEN
   RAISE EXCEPTION 'Los proyectos de antecedente no requieren revisión interna; para tramitar el proyecto habilite su gestión en la plataforma'; END IF;
  IF NOT EXISTS(SELECT 1 FROM reviews.committees WHERE id=NEW.committee_id AND is_active AND deleted_at IS NULL
   AND starts_on<=NEW.submitted_at::date AND (ends_on IS NULL OR ends_on>=NEW.submitted_at::date)) THEN
   RAISE EXCEPTION 'Comité no vigente para el envío'; END IF;
  IF NOT EXISTS(SELECT 1 FROM research.projects_leads l JOIN research.projects_researchers pr ON pr.id=l.project_researcher_id
   WHERE pr.project_id=NEW.project_id AND l.is_current AND l.is_active AND l.deleted_at IS NULL) THEN
   RAISE EXCEPTION 'Asigne un responsable antes de enviar'; END IF;
  NEW.snapshot:=reviews.capture_project_snapshot(NEW.project_id,NEW.application_id);
 ELSE
  IF (to_jsonb(NEW)-ARRAY['processing_status','updated_by','updated_at']) IS DISTINCT FROM
     (to_jsonb(OLD)-ARRAY['processing_status','updated_by','updated_at']) THEN
   RAISE EXCEPTION 'El contenido original del envío es inmutable'; END IF;
  IF OLD.processing_status<>'in_process' AND NEW.processing_status<>OLD.processing_status THEN
   RAISE EXCEPTION 'No se reabre un envío cerrado'; END IF;
 END IF;
 RETURN NEW;
END $$;
CREATE TRIGGER validate_submission BEFORE INSERT OR UPDATE OR DELETE ON reviews.projects_submissions
FOR EACH ROW EXECUTE FUNCTION reviews.validate_submission();

-- TODAS las decisiones, incluida la vigente, se insertan una sola vez en el historial.
-- Vigente: ORDER BY decided_at DESC, id DESC LIMIT 1 para el envío consultado.
-- La fila del envío se bloquea para serializar decisiones simultáneas.
CREATE FUNCTION reviews.validate_academic_decision() RETURNS TRIGGER LANGUAGE plpgsql AS $$
DECLARE p BIGINT; sent_at TIMESTAMPTZ; latest_at TIMESTAMPTZ;
BEGIN
 SELECT project_id,submitted_at INTO p,sent_at FROM reviews.projects_submissions
 WHERE id=NEW.submission_id FOR UPDATE;
 IF NOT FOUND THEN RAISE EXCEPTION 'El envío no existe'; END IF;
 IF NOT EXISTS(SELECT 1 FROM access.users_roles ur JOIN access.c_roles r ON r.id=ur.role_id
  JOIN access.users u ON u.id=ur.user_id WHERE ur.user_id=NEW.authorized_by AND r.code='academic_authority'
  AND ur.is_active AND ur.deleted_at IS NULL AND r.is_active AND r.deleted_at IS NULL
  AND u.is_active AND u.deleted_at IS NULL) THEN
  RAISE EXCEPTION 'El usuario no tiene el rol de Subdirección Académica'; END IF;
 IF NOT EXISTS(SELECT 1 FROM reviews.c_academic_authorization_statuses WHERE id=NEW.status_id AND is_active AND deleted_at IS NULL) THEN
  RAISE EXCEPTION 'Estado académico inexistente o inactivo'; END IF;
 IF NOT EXISTS(SELECT 1 FROM files.documents d JOIN files.c_document_types t ON t.id=d.document_type_id
  JOIN research.projects_documents pd ON pd.document_id=d.id
  WHERE d.id=NEW.document_id AND t.code='academic_authorization' AND pd.project_id=p) THEN
  RAISE EXCEPTION 'El documento académico debe estar registrado en el mismo proyecto y tener el tipo académico'; END IF;
 IF NEW.decided_at<sent_at THEN RAISE EXCEPTION 'La decisión no puede ser anterior al envío'; END IF;
 SELECT max(decided_at) INTO latest_at FROM reviews.academic_authorizations_history WHERE submission_id=NEW.submission_id;
 IF NEW.decided_at<latest_at THEN RAISE EXCEPTION 'Fecha académica fuera de secuencia'; END IF;
 IF NEW.rationale IS NULL OR btrim(NEW.rationale)='' THEN RAISE EXCEPTION 'Indique los motivos de la decisión académica'; END IF;
 -- Ningún dictamen ni resultado favorable del comité es requisito para decidir.
 RETURN NEW;
END $$;
CREATE TRIGGER validate_academic_decision BEFORE INSERT ON reviews.academic_authorizations_history
FOR EACH ROW EXECUTE FUNCTION reviews.validate_academic_decision();
CREATE INDEX academic_authorizations_history_latest_idx
ON reviews.academic_authorizations_history(submission_id,decided_at DESC,id DESC);

CREATE FUNCTION reviews.pin_submission_documents() RETURNS TRIGGER LANGUAGE plpgsql AS $$
BEGIN
 INSERT INTO reviews.submissions_documents(submission_id,project_document_id)
 SELECT NEW.id,pd.id FROM research.projects_documents pd WHERE pd.project_id=NEW.project_id AND pd.is_current AND pd.is_active AND pd.deleted_at IS NULL;
 RETURN NEW;END $$;
CREATE TRIGGER pin_submission_documents AFTER INSERT ON reviews.projects_submissions FOR EACH ROW EXECUTE FUNCTION reviews.pin_submission_documents();
CREATE FUNCTION reviews.guard_manifest() RETURNS TRIGGER LANGUAGE plpgsql AS $$
BEGIN
 IF TG_OP<>'INSERT' THEN RAISE EXCEPTION 'El manifiesto documental del envío es inmutable';END IF;
 IF NOT EXISTS(SELECT 1 FROM reviews.projects_submissions s JOIN research.projects_documents pd ON pd.project_id=s.project_id
  WHERE s.id=NEW.submission_id AND pd.id=NEW.project_document_id AND s.xmin::text::bigint=txid_current()%4294967296) THEN
  RAISE EXCEPTION 'Documento ajeno al proyecto o agregado fuera de la transacción del envío';END IF;
 RETURN NEW;END $$;
CREATE TRIGGER guard_manifest BEFORE INSERT OR UPDATE OR DELETE ON reviews.submissions_documents FOR EACH ROW EXECUTE FUNCTION reviews.guard_manifest();

CREATE FUNCTION reviews.validate_review() RETURNS TRIGGER LANGUAGE plpgsql AS $$
DECLARE state_code TEXT;s reviews.projects_submissions%ROWTYPE;BEGIN
 IF TG_OP='DELETE' THEN RAISE EXCEPTION 'No se eliminan revisiones';END IF;
 IF TG_OP='UPDATE' THEN
  IF OLD.submission_id<>NEW.submission_id OR OLD.committee_member_id<>NEW.committee_member_id OR OLD.committee_id<>NEW.committee_id THEN RAISE EXCEPTION 'La identidad de la revisión no se modifica';END IF;
  IF EXISTS(SELECT 1 FROM reviews.c_review_statuses WHERE id=OLD.status_id AND code='completed') THEN RAISE EXCEPTION 'Una revisión finalizada es inmutable';END IF;
 END IF;
 SELECT * INTO s FROM reviews.projects_submissions WHERE id=NEW.submission_id FOR UPDATE;
 IF s.processing_status<>'in_process' THEN RAISE EXCEPTION 'El envío no está en proceso';END IF;
 IF NOT EXISTS(SELECT 1 FROM reviews.committees_members m WHERE m.id=NEW.committee_member_id AND m.committee_id=s.committee_id
 AND m.is_active AND m.deleted_at IS NULL AND m.starts_on<=COALESCE(NEW.reviewed_at,s.submitted_at)::date
 AND (m.ends_on IS NULL OR m.ends_on>=COALESCE(NEW.reviewed_at,s.submitted_at)::date)) THEN RAISE EXCEPTION 'El revisor no es integrante vigente del comité asignado';END IF;
 SELECT code INTO state_code FROM reviews.c_review_statuses WHERE id=NEW.status_id;
 IF state_code='completed' THEN
  IF NEW.outcome_id IS NULL OR NEW.reviewed_at IS NULL OR NEW.assessment IS NULL OR btrim(NEW.assessment)='' THEN RAISE EXCEPTION 'Una revisión finalizada requiere resultado, fecha y motivos';END IF;
  IF NEW.reviewed_at<s.submitted_at THEN RAISE EXCEPTION 'La revisión no puede ser anterior al envío';END IF;
 ELSIF NEW.outcome_id IS NOT NULL OR NEW.reviewed_at IS NOT NULL THEN RAISE EXCEPTION 'El resultado y la fecha corresponden a una revisión finalizada';END IF;
 RETURN NEW;END $$;
CREATE TRIGGER validate_review BEFORE INSERT OR UPDATE OR DELETE ON reviews.projects_reviews FOR EACH ROW EXECUTE FUNCTION reviews.validate_review();
CREATE FUNCTION reviews.guard_member_identity() RETURNS TRIGGER LANGUAGE plpgsql AS $$
BEGIN IF NEW.committee_id<>OLD.committee_id OR NEW.user_id<>OLD.user_id OR NEW.committee_role_id<>OLD.committee_role_id OR NEW.starts_on<>OLD.starts_on THEN
 RAISE EXCEPTION 'Cierre la membresía e inserte otra; no cambie la identidad histórica del integrante';END IF;RETURN NEW;END $$;
CREATE TRIGGER guard_member_identity BEFORE UPDATE ON reviews.committees_members FOR EACH ROW EXECUTE FUNCTION reviews.guard_member_identity();
-- La API autentica al actor de cada operación y fija los campos de usuario;
-- las FKs de auditoría genérica siguen ausentes por decisión de diseño.
-- Cada decisión académica se inserta directamente en su historial. La actual
-- se consulta por fecha e id; no hay copia en el envío ni vista de decisión actual.
-- Ediciones de propuesta/envío deben serializarse por proyecto en el backend.
-- Alta de proyecto: insertar proyecto, participación y responsable en la misma
-- transacción. Para cambiar responsable, cerrar su periodo e insertar el nuevo
-- antes del COMMIT; la validación diferida exige exactamente uno vigente.

\echo '4/5 Cargando ejemplos de la estructura revisada...'
DO $$
DECLARE
    v_3001 BIGINT;
    v_3002 BIGINT;
    v_3003 BIGINT;
    v_3004 BIGINT;
    v_3005 BIGINT;
    v_3006 BIGINT;
    v_3007 BIGINT;
    v_3008 BIGINT;
    v_3009 BIGINT;
    v_3010 BIGINT;
    v_3011 BIGINT;
    v_3012 BIGINT;
    v_3013 BIGINT;
    v_3014 BIGINT;
    v_2001 BIGINT;
    v_2002 BIGINT;
    v_2003 BIGINT;
    v_2004 BIGINT;
    v_1 BIGINT;
    v_2 BIGINT;
    v_3 BIGINT;
    v_4 BIGINT;
    v_5 BIGINT;
    v_6 BIGINT;
    v_7 BIGINT;
    v_8 BIGINT;
    v_9 BIGINT;
    v_10 BIGINT;
    v_11 BIGINT;
    v_12 BIGINT;
    v_13 BIGINT;
    v_14 BIGINT;
    v_15 BIGINT;
    v_16 BIGINT;
    v_17 BIGINT;
    v_18 BIGINT;
    v_19 BIGINT;
    v_20 BIGINT;
    v_21 BIGINT;
    v_22 BIGINT;
    v_23 BIGINT;
    v_24 BIGINT;
    v_25 BIGINT;
    v_26 BIGINT;
    v_27 BIGINT;
    v_28 BIGINT;
    v_29 BIGINT;
    v_30 BIGINT;
    v_31 BIGINT;
    v_32 BIGINT;
    v_33 BIGINT;
    v_34 BIGINT;
    v_35 BIGINT;
    v_36 BIGINT;
    v_37 BIGINT;
    v_38 BIGINT;
    v_39 BIGINT;
    v_40 BIGINT;
    v_41 BIGINT;
    v_42 BIGINT;
    v_43 BIGINT;
    v_44 BIGINT;
    v_45 BIGINT;
    v_46 BIGINT;
    v_47 BIGINT;
    v_48 BIGINT;
    v_49 BIGINT;
    v_50 BIGINT;
    v_51 BIGINT;
    v_52 BIGINT;
    v_53 BIGINT;
    v_54 BIGINT;
    v_55 BIGINT;
    v_56 BIGINT;
    v_57 BIGINT;
    v_58 BIGINT;
    v_59 BIGINT;
    v_60 BIGINT;
    v_61 BIGINT;
    v_62 BIGINT;
    v_63 BIGINT;
    v_64 BIGINT;
    v_65 BIGINT;
    v_66 BIGINT;
    v_67 BIGINT;
    v_68 BIGINT;
    v_69 BIGINT;
    v_70 BIGINT;
    v_71 BIGINT;
    v_72 BIGINT;
    v_73 BIGINT;
    v_74 BIGINT;
    v_75 BIGINT;
    v_76 BIGINT;
    v_77 BIGINT;
    v_78 BIGINT;
    v_79 BIGINT;
    v_80 BIGINT;
    v_81 BIGINT;
    v_82 BIGINT;
    v_83 BIGINT;
    v_84 BIGINT;
    v_85 BIGINT;
    v_86 BIGINT;
    v_87 BIGINT;
    v_88 BIGINT;
    v_89 BIGINT;
    v_90 BIGINT;
    v_91 BIGINT;
    v_92 BIGINT;
    v_93 BIGINT;
    v_94 BIGINT;
    v_95 BIGINT;
    v_96 BIGINT;
    v_97 BIGINT;
    v_98 BIGINT;
    v_99 BIGINT;
    v_100 BIGINT;
    v_101 BIGINT;
    v_102 BIGINT;
    v_103 BIGINT;
    v_104 BIGINT;
    v_105 BIGINT;
    v_106 BIGINT;
    v_107 BIGINT;
    v_108 BIGINT;
    v_109 BIGINT;
    v_110 BIGINT;
    v_111 BIGINT;
    v_112 BIGINT;
    v_113 BIGINT;
    v_114 BIGINT;
    v_115 BIGINT;
    v_116 BIGINT;
    v_117 BIGINT;
    v_118 BIGINT;
    v_119 BIGINT;
    v_120 BIGINT;
    v_121 BIGINT;
    v_122 BIGINT;
    v_123 BIGINT;
    v_124 BIGINT;
    v_125 BIGINT;
    v_126 BIGINT;
    v_127 BIGINT;
    v_128 BIGINT;
    v_129 BIGINT;
    v_130 BIGINT;
    v_131 BIGINT;
    v_132 BIGINT;
    v_133 BIGINT;
    v_134 BIGINT;
    v_135 BIGINT;
    v_136 BIGINT;
    v_137 BIGINT;
    v_138 BIGINT;
    v_139 BIGINT;
    v_140 BIGINT;
    v_141 BIGINT;
    v_142 BIGINT;
    v_143 BIGINT;
    v_144 BIGINT;
    v_145 BIGINT;
    v_146 BIGINT;
    v_147 BIGINT;
    v_148 BIGINT;
    v_149 BIGINT;
    v_150 BIGINT;
    v_151 BIGINT;
    v_152 BIGINT;
    v_153 BIGINT;
    v_154 BIGINT;
    v_155 BIGINT;
    v_156 BIGINT;
    v_157 BIGINT;
    v_158 BIGINT;
    v_159 BIGINT;
    v_160 BIGINT;
    v_161 BIGINT;
    v_162 BIGINT;
    v_163 BIGINT;
    v_164 BIGINT;
    v_165 BIGINT;
    v_166 BIGINT;
    v_167 BIGINT;
    v_168 BIGINT;
    v_169 BIGINT;
    v_170 BIGINT;
    v_171 BIGINT;
    v_172 BIGINT;
    v_173 BIGINT;
    v_174 BIGINT;
    v_175 BIGINT;
    v_176 BIGINT;
    v_177 BIGINT;
    v_178 BIGINT;
    v_179 BIGINT;
    v_180 BIGINT;
    v_181 BIGINT;
    v_182 BIGINT;
    v_183 BIGINT;
    v_184 BIGINT;
    v_185 BIGINT;
    v_186 BIGINT;
    v_187 BIGINT;
    v_188 BIGINT;
    v_189 BIGINT;
    v_190 BIGINT;
    v_191 BIGINT;
    v_192 BIGINT;
    v_193 BIGINT;
    v_194 BIGINT;
    v_195 BIGINT;
    v_196 BIGINT;
    v_197 BIGINT;
    v_198 BIGINT;
    v_199 BIGINT;
    v_200 BIGINT;
    v_201 BIGINT;
    v_202 BIGINT;
    v_203 BIGINT;
    v_204 BIGINT;
    v_205 BIGINT;
    v_206 BIGINT;
    v_207 BIGINT;
    v_208 BIGINT;
    v_1001 BIGINT;
    v_1002 BIGINT;
    v_1003 BIGINT;
    v_1004 BIGINT;
    v_1005 BIGINT;
    v_1006 BIGINT;
    v_1007 BIGINT;
    v_1008 BIGINT;
    v_1009 BIGINT;
    v_1010 BIGINT;
    v_1011 BIGINT;
    v_1012 BIGINT;
    v_1013 BIGINT;
    v_1014 BIGINT;
BEGIN
    INSERT INTO reviews.c_review_statuses (code, name)
    VALUES ('pending', 'Pendiente') RETURNING id INTO v_1001;
    INSERT INTO reviews.c_review_statuses (code, name)
    VALUES ('in_progress', 'En proceso') RETURNING id INTO v_1002;
    INSERT INTO reviews.c_review_statuses (code, name)
    VALUES ('completed', 'Finalizada') RETURNING id INTO v_1003;
    INSERT INTO research.c_task_statuses (code, name)
    VALUES ('pending', 'Pendiente') RETURNING id INTO v_1004;
    INSERT INTO research.c_task_statuses (code, name)
    VALUES ('in_progress', 'En proceso') RETURNING id INTO v_1005;
    INSERT INTO research.c_task_statuses (code, name)
    VALUES ('completed', 'Completada') RETURNING id INTO v_1006;
    INSERT INTO access.c_roles (code, name)
    VALUES ('administrator', 'Administrador') RETURNING id INTO v_1;
    INSERT INTO access.c_roles (code, name)
    VALUES ('researcher', 'Investigador') RETURNING id INTO v_2;
    INSERT INTO access.c_roles (code, name)
    VALUES ('student', 'Estudiante') RETURNING id INTO v_3;
    INSERT INTO access.c_roles (code, name)
    VALUES ('committee_member', 'Integrante de comité') RETURNING id INTO v_4;
    INSERT INTO access.c_roles (code, name)
    VALUES ('academic_authority', 'Subdirección Académica') RETURNING id INTO v_5;
    INSERT INTO access.c_roles (code, name)
    VALUES ('viewer', 'Consulta autorizada') RETURNING id INTO v_6;
    INSERT INTO access.c_permissions (code, name)
    VALUES ('projects.edit', 'Editar proyectos') RETURNING id INTO v_7;
    INSERT INTO access.c_permissions (code, name)
    VALUES ('projects.read', 'Consultar proyectos') RETURNING id INTO v_8;
    INSERT INTO access.c_permissions (code, name)
    VALUES ('projects.submit', 'Enviar a revisión') RETURNING id INTO v_9;
    INSERT INTO access.c_permissions (code, name)
    VALUES ('reviews.write', 'Registrar revisiones') RETURNING id INTO v_10;
    INSERT INTO access.c_permissions (code, name)
    VALUES ('academic.authorize', 'Autorizar proyectos') RETURNING id INTO v_11;
    INSERT INTO access.c_permissions (code, name)
    VALUES ('reports.export', 'Exportar Excel y PDF') RETURNING id INTO v_12;
    INSERT INTO access.c_permissions (code, name)
    VALUES ('reference_data.manage', 'Administrar catálogos') RETURNING id INTO v_13;
    INSERT INTO access.users (identity_provider, external_subject, email, given_name, family_name)
    VALUES ('demo-platform', 'demo-admin', 'admin@example.invalid', 'Administración', 'Demostración') RETURNING id INTO v_14;
    INSERT INTO access.users (identity_provider, external_subject, email, given_name, family_name)
    VALUES ('demo-platform', 'demo-ana', 'ana@example.invalid', 'Ana', 'García Demo') RETURNING id INTO v_15;
    INSERT INTO access.users (identity_provider, external_subject, email, given_name, family_name)
    VALUES ('demo-platform', 'demo-luis', 'luis@example.invalid', 'Luis', 'Pérez Demo') RETURNING id INTO v_16;
    INSERT INTO access.users (identity_provider, external_subject, email, given_name, family_name)
    VALUES ('demo-platform', 'demo-sofia', 'sofia@example.invalid', 'Sofía', 'López Demo') RETURNING id INTO v_17;
    INSERT INTO access.users (identity_provider, external_subject, email, given_name, family_name)
    VALUES ('demo-platform', 'demo-president', 'president@example.invalid', 'Patricia', 'Comité Demo') RETURNING id INTO v_18;
    INSERT INTO access.users (identity_provider, external_subject, email, given_name, family_name)
    VALUES ('demo-platform', 'demo-secretary', 'secretary@example.invalid', 'Sergio', 'Comité Demo') RETURNING id INTO v_19;
    INSERT INTO access.users (identity_provider, external_subject, email, given_name, family_name)
    VALUES ('demo-platform', 'demo-head', 'head@example.invalid', 'Carmen', 'Carrera Demo') RETURNING id INTO v_20;
    INSERT INTO access.users (identity_provider, external_subject, email, given_name, family_name)
    VALUES ('demo-platform', 'demo-authority', 'authority@example.invalid', 'Alma', 'Académica Demo') RETURNING id INTO v_21;
    INSERT INTO access.users_roles (user_id, role_id, created_by)
    VALUES (v_14, v_1, v_14) RETURNING id INTO v_22;
    INSERT INTO access.users_roles (user_id, role_id, created_by)
    VALUES (v_15, v_2, v_14) RETURNING id INTO v_23;
    INSERT INTO access.users_roles (user_id, role_id, created_by)
    VALUES (v_16, v_2, v_14) RETURNING id INTO v_24;
    INSERT INTO access.users_roles (user_id, role_id, created_by)
    VALUES (v_17, v_3, v_14) RETURNING id INTO v_25;
    INSERT INTO access.users_roles (user_id, role_id, created_by)
    VALUES (v_18, v_4, v_14) RETURNING id INTO v_26;
    INSERT INTO access.users_roles (user_id, role_id, created_by)
    VALUES (v_19, v_4, v_14) RETURNING id INTO v_27;
    INSERT INTO access.users_roles (user_id, role_id, created_by)
    VALUES (v_20, v_4, v_14) RETURNING id INTO v_28;
    INSERT INTO access.users_roles (user_id, role_id, created_by)
    VALUES (v_21, v_5, v_14) RETURNING id INTO v_29;
    INSERT INTO access.roles_permissions (role_id, permission_id)
    VALUES (v_1, v_7) RETURNING id INTO v_30;
    INSERT INTO access.roles_permissions (role_id, permission_id)
    VALUES (v_1, v_8) RETURNING id INTO v_31;
    INSERT INTO access.roles_permissions (role_id, permission_id)
    VALUES (v_1, v_9) RETURNING id INTO v_32;
    INSERT INTO access.roles_permissions (role_id, permission_id)
    VALUES (v_1, v_10) RETURNING id INTO v_33;
    INSERT INTO access.roles_permissions (role_id, permission_id)
    VALUES (v_1, v_11) RETURNING id INTO v_34;
    INSERT INTO access.roles_permissions (role_id, permission_id)
    VALUES (v_1, v_12) RETURNING id INTO v_35;
    INSERT INTO access.roles_permissions (role_id, permission_id)
    VALUES (v_1, v_13) RETURNING id INTO v_36;
    INSERT INTO access.roles_permissions (role_id, permission_id)
    VALUES (v_2, v_7) RETURNING id INTO v_37;
    INSERT INTO access.roles_permissions (role_id, permission_id)
    VALUES (v_2, v_8) RETURNING id INTO v_38;
    INSERT INTO access.roles_permissions (role_id, permission_id)
    VALUES (v_2, v_9) RETURNING id INTO v_39;
    INSERT INTO access.roles_permissions (role_id, permission_id)
    VALUES (v_2, v_12) RETURNING id INTO v_40;
    INSERT INTO access.roles_permissions (role_id, permission_id)
    VALUES (v_4, v_8) RETURNING id INTO v_41;
    INSERT INTO access.roles_permissions (role_id, permission_id)
    VALUES (v_4, v_10) RETURNING id INTO v_42;
    INSERT INTO access.roles_permissions (role_id, permission_id)
    VALUES (v_5, v_8) RETURNING id INTO v_43;
    INSERT INTO access.roles_permissions (role_id, permission_id)
    VALUES (v_5, v_11) RETURNING id INTO v_44;
    INSERT INTO access.roles_permissions (role_id, permission_id)
    VALUES (v_6, v_8) RETURNING id INTO v_45;
    INSERT INTO access.roles_permissions (role_id, permission_id)
    VALUES (v_6, v_12) RETURNING id INTO v_46;
    INSERT INTO academic.c_organization_types (code, name)
    VALUES ('university', 'Universidad') RETURNING id INTO v_47;
    INSERT INTO academic.c_organizations (code, name, organization_type_id, country_code)
    VALUES ('DEMO-TECNM', 'Instituto Tecnológico de Monclova — DEMO', v_47, 'MX') RETURNING id INTO v_48;
    INSERT INTO academic.c_organizations (code, name, organization_type_id, country_code)
    VALUES ('DEMO-EXT', 'Universidad externa — DEMO', v_47, 'MX') RETURNING id INTO v_49;
    INSERT INTO academic.c_academic_programs (code, name, organization_id)
    VALUES ('DEMO-IT', 'Ingeniería Informática — DEMO', v_48) RETURNING id INTO v_50;
    INSERT INTO academic.c_academic_fields (code, name)
    VALUES ('engineering', 'Ingeniería y tecnología') RETURNING id INTO v_51;
    INSERT INTO academic.c_research_lines (code, name, academic_field_id)
    VALUES ('educational_technology', 'Tecnología educativa', v_51) RETURNING id INTO v_52;
    INSERT INTO academic.c_degree_levels (code, name)
    VALUES ('bachelor', 'Licenciatura') RETURNING id INTO v_53;
    INSERT INTO academic.c_degree_levels (code, name)
    VALUES ('master', 'Maestría') RETURNING id INTO v_54;
    INSERT INTO academic.c_degree_levels (code, name)
    VALUES ('doctorate', 'Doctorado') RETURNING id INTO v_55;
    INSERT INTO academic.researchers (user_id, professional_summary)
    VALUES (v_15, 'Investigadora de demostración en tecnología educativa.') RETURNING id INTO v_56;
    INSERT INTO academic.researchers (user_id, professional_summary)
    VALUES (v_16, 'Investigador externo de demostración.') RETURNING id INTO v_57;
    INSERT INTO academic.researchers_organizations (researcher_id, organization_id, employee_number, starts_on)
    VALUES (v_56, v_48, 'DEMO-001', '2025-01-01') RETURNING id INTO v_58;
    INSERT INTO academic.researchers_organizations (researcher_id, organization_id, employee_number, starts_on)
    VALUES (v_56, v_49, 'DEMO-002', '2025-01-01') RETURNING id INTO v_59;
    INSERT INTO academic.researchers_organizations (researcher_id, organization_id, employee_number, starts_on)
    VALUES (v_57, v_49, 'DEMO-003', '2025-01-01') RETURNING id INTO v_60;
    INSERT INTO academic.researchers_degrees (researcher_id, degree_level_id, degree_title, institution_name, graduation_year)
    VALUES (v_56, v_54, 'Maestría en Sistemas — DEMO', 'Universidad DEMO', 2020) RETURNING id INTO v_61;
    INSERT INTO academic.researchers_experiences (researcher_id, organization_name, position_title, starts_on)
    VALUES (v_56, 'Institución DEMO', 'Profesor investigador', '2021-01-01') RETURNING id INTO v_62;
    INSERT INTO academic.researchers_research_lines (researcher_id, research_line_id)
    VALUES (v_56, v_52) RETURNING id INTO v_63;
    INSERT INTO academic.researchers_degrees (researcher_id, degree_level_id, degree_title, institution_name, graduation_year)
    VALUES (v_57, v_54, 'Maestría en Sistemas — DEMO', 'Universidad DEMO', 2020) RETURNING id INTO v_64;
    INSERT INTO academic.researchers_experiences (researcher_id, organization_name, position_title, starts_on)
    VALUES (v_57, 'Institución DEMO', 'Profesor investigador', '2021-01-01') RETURNING id INTO v_65;
    INSERT INTO academic.researchers_research_lines (researcher_id, research_line_id)
    VALUES (v_57, v_52) RETURNING id INTO v_66;
    INSERT INTO academic.students (user_id, academic_program_id, student_number, semester)
    VALUES (v_17, v_50, 'DEMO-AL-001', 3) RETURNING id INTO v_67;
    INSERT INTO calls.c_call_types (code, name)
    VALUES ('internal', 'Financiamiento institucional') RETURNING id INTO v_68;
    INSERT INTO calls.c_calls (code, name, organization_id, call_type_id, opens_at, closes_at)
    VALUES ('DEMO-2026', 'Convocatoria de investigación DEMO 2026', v_48, v_68, '2026-01-01T00:00:00Z', '2026-12-31T23:59:59Z') RETURNING id INTO v_69;
    INSERT INTO files.c_document_types (code, name)
    VALUES ('protocol', 'Protocolo') RETURNING id INTO v_70;
    INSERT INTO files.c_document_types (code, name)
    VALUES ('rights_assignment', 'Carta de cesión de derechos') RETURNING id INTO v_71;
    INSERT INTO files.c_document_types (code, name)
    VALUES ('academic_authorization', 'Autorización académica') RETURNING id INTO v_73;
    INSERT INTO files.c_document_types (code, name)
    VALUES ('evidence', 'Evidencia') RETURNING id INTO v_74;
    INSERT INTO files.c_document_types (code, name)
    VALUES ('budget_support', 'Soporte de presupuesto') RETURNING id INTO v_75;
    INSERT INTO files.c_document_types (code, name)
    VALUES ('funding_agreement', 'Convenio de financiamiento') RETURNING id INTO v_76;
    INSERT INTO files.c_document_types (code, name)
    VALUES ('receipt', 'Comprobante de gasto') RETURNING id INTO v_77;
    INSERT INTO files.c_document_types (code, name)
    VALUES ('research_output', 'Producto de investigación') RETURNING id INTO v_78;
    INSERT INTO files.c_document_templates (code, name, version_number, is_current, original_file_name, storage_key, media_type, size_bytes, sha256)
    VALUES ('rights_assignment', 'Carta de cesión de derechos', 1, FALSE, 'carta_cesion_formato_v1.pdf', 'demo/templates/rights-v1.pdf', 'application/pdf', 1024, '0000000000000000000000000000000000000000000000000000000000000000') RETURNING id INTO v_80;
    INSERT INTO files.c_document_templates (code, name, version_number, is_current, original_file_name, storage_key, media_type, size_bytes, sha256)
    VALUES ('rights_assignment', 'Carta de cesión de derechos', 2, TRUE, 'carta_cesion_formato_v2.pdf', 'demo/templates/rights-v2.pdf', 'application/pdf', 1024, '0000000000000000000000000000000000000000000000000000000000000000') RETURNING id INTO v_82;
    UPDATE files.c_document_types SET template_id=v_82 WHERE id=v_71;
    INSERT INTO calls.calls_document_requirements (call_id, document_type_id, title, workflow_stage, is_required)
    VALUES (v_69, v_70, 'Protocolo de investigación', 'submission', TRUE) RETURNING id INTO v_83;
    INSERT INTO calls.calls_document_requirements (call_id, document_type_id, title, workflow_stage, is_required)
    VALUES (v_69, v_71, 'Carta firmada', 'submission', TRUE) RETURNING id INTO v_84;
    INSERT INTO calls.calls_requirements (call_id, title, description, is_required)
    VALUES (v_69, 'Participación estudiantil', 'Incluir al menos un estudiante en el equipo.', TRUE) RETURNING id INTO v_85;
    INSERT INTO research.c_currencies (code, name)
    VALUES ('MXN', 'Peso mexicano') RETURNING id INTO v_86;
    INSERT INTO research.c_currencies (code, name)
    VALUES ('USD', 'Dólar estadounidense') RETURNING id INTO v_87;
    INSERT INTO research.c_budget_categories (code, name)
    VALUES ('materials', 'Materiales y suministros') RETURNING id INTO v_88;
    INSERT INTO research.c_funding_sources (code, name)
    VALUES ('institutional', 'Recursos institucionales') RETURNING id INTO v_89;
    INSERT INTO research.c_researchers_roles (code, name)
    VALUES ('researcher', 'Investigador participante') RETURNING id INTO v_90;
    INSERT INTO research.c_researchers_roles (code, name)
    VALUES ('collaborator', 'Colaborador') RETURNING id INTO v_91;
    INSERT INTO research.c_researchers_roles (code, name)
    VALUES ('advisor', 'Asesor') RETURNING id INTO v_92;
    INSERT INTO research.c_project_statuses (code, name)
    VALUES ('draft', 'Borrador') RETURNING id INTO v_93;
    INSERT INTO research.c_project_statuses (code, name)
    VALUES ('under_review', 'En revisión') RETURNING id INTO v_94;
    INSERT INTO research.c_project_statuses (code, name)
    VALUES ('authorized', 'Autorizado') RETURNING id INTO v_95;
    INSERT INTO research.c_project_statuses (code, name)
    VALUES ('in_progress', 'En ejecución') RETURNING id INTO v_96;
    INSERT INTO research.c_project_statuses (code, name)
    VALUES ('completed', 'Concluido') RETURNING id INTO v_97;
    INSERT INTO research.c_project_statuses (code, name)
    VALUES ('cancelled', 'Cancelado') RETURNING id INTO v_98;
    INSERT INTO research.c_objective_statuses (code, name)
    VALUES ('pending', 'Pendiente') RETURNING id INTO v_99;
    INSERT INTO research.c_objective_statuses (code, name)
    VALUES ('in_progress', 'En proceso') RETURNING id INTO v_100;
    INSERT INTO research.c_objective_statuses (code, name)
    VALUES ('completed', 'Cumplido') RETURNING id INTO v_101;
    INSERT INTO research.c_milestone_statuses (code, name)
    VALUES ('pending', 'Pendiente') RETURNING id INTO v_102;
    INSERT INTO research.c_milestone_statuses (code, name)
    VALUES ('in_progress', 'En proceso') RETURNING id INTO v_103;
    INSERT INTO research.c_milestone_statuses (code, name)
    VALUES ('completed', 'Completado') RETURNING id INTO v_104;
    INSERT INTO research.c_research_outputs_types (code, name)
    VALUES ('article', 'Artículo') RETURNING id INTO v_105;
    INSERT INTO research.c_research_outputs_types (code, name)
    VALUES ('software', 'Software') RETURNING id INTO v_106;
    INSERT INTO research.c_research_outputs_types (code, name)
    VALUES ('prototype', 'Prototipo') RETURNING id INTO v_107;
    INSERT INTO research.c_research_outputs_types (code, name)
    VALUES ('book', 'Libro') RETURNING id INTO v_108;
    INSERT INTO research.projects (organization_id, is_managed_in_platform, reference_code, title, currency_id, abstract, problem_statement, justification, theoretical_framework, methodology, expected_results, bibliographic_references, starts_on, ends_on, created_by, objectives_progress_percentage, milestones_progress_percentage, tasks_progress_percentage)
    VALUES (v_48, TRUE, 'DEMO-PRJ-001', 'Plataforma de aprendizaje informal — DEMO', v_86, 'Proyecto ficticio para probar el flujo del sistema.', 'Problema de investigación de demostración.', 'Justificación de demostración.', 'Marco teórico de demostración.', 'Metodología de demostración.', 'Un prototipo y una publicación de demostración.', 'Referencias ficticias, no usar como bibliografía.', '2026-02-01', '2026-12-31', v_15, 25, 50, 65) RETURNING id INTO v_109;
    INSERT INTO research.projects_researchers (project_id, researcher_id, researcher_role_id, starts_on)
    VALUES (v_109, v_56, v_90, '2026-02-01') RETURNING id INTO v_110;
    INSERT INTO research.projects_researchers (project_id, researcher_id, researcher_role_id, starts_on)
    VALUES (v_109, v_57, v_91, '2026-02-01') RETURNING id INTO v_111;
    INSERT INTO research.projects_leads (project_researcher_id, starts_at, is_current, change_reason)
    VALUES (v_110, '2026-02-01T00:00:00Z', TRUE, 'Nombramiento inicial') RETURNING id INTO v_112;
    INSERT INTO research.projects_students (project_id, student_id, activities, starts_on)
    VALUES (v_109, v_67, 'Apoyo en captura y pruebas.', '2026-02-01') RETURNING id INTO v_114;
    INSERT INTO research.projects_research_lines (project_id, research_line_id)
    VALUES (v_109, v_52) RETURNING id INTO v_115;
    INSERT INTO research.objectives (project_id, objective_type, description)
    VALUES (v_109, 'general', 'Evaluar una herramienta educativa de demostración.') RETURNING id INTO v_116;
    INSERT INTO research.objectives (project_id, objective_type, description)
    VALUES (v_109, 'specific', 'Desarrollar y evaluar un prototipo.') RETURNING id INTO v_117;
    INSERT INTO research.milestones (objective_id, title, due_on)
    VALUES (v_117, 'Prototipo funcional', '2026-09-30') RETURNING id INTO v_118;
    INSERT INTO research.tasks (milestone_id, title, starts_on, ends_on, responsible_participant_id)
    VALUES (v_118, 'Implementar prototipo', '2026-02-01', '2026-09-01', v_110) RETURNING id INTO v_119;
    INSERT INTO research.tasks_status_history (task_id, status_id, completion_percentage, changed_by, changed_at, reason)
    VALUES (v_119, v_1005, 65, v_15, '2026-06-30T10:00:00Z', 'Implementación parcial del prototipo — DEMO.') RETURNING id INTO v_1007;
    INSERT INTO research.projects_status_history (project_id, status_id, changed_by, changed_at, reason)
    VALUES (v_109, v_93, v_15, '2026-02-01T09:00:00Z', 'Registro inicial — DEMO') RETURNING id INTO v_120;
    INSERT INTO research.objectives_status_history (objective_id, status_id, changed_by, changed_at, reason, completion_percentage)
    VALUES (v_116, v_99, v_15, '2026-02-01T09:00:00Z', 'Registro inicial — DEMO', 0) RETURNING id INTO v_121;
    INSERT INTO research.objectives_status_history (objective_id, status_id, changed_by, changed_at, reason, completion_percentage)
    VALUES (v_117, v_100, v_15, '2026-02-01T09:00:00Z', 'Registro inicial — DEMO', 25) RETURNING id INTO v_122;
    INSERT INTO research.milestones_status_history (milestone_id, status_id, changed_by, changed_at, reason, completion_percentage)
    VALUES (v_118, v_103, v_15, '2026-02-01T09:00:00Z', 'Registro inicial — DEMO', 50) RETURNING id INTO v_123;
    INSERT INTO research.projects (organization_id, is_managed_in_platform, reference_code, title, currency_id, abstract, problem_statement, justification, theoretical_framework, methodology, expected_results, bibliographic_references, starts_on, ends_on, created_by, objectives_progress_percentage, milestones_progress_percentage, tasks_progress_percentage)
    VALUES (v_48, TRUE, 'DEMO-PRJ-002', 'Análisis de recursos educativos — DEMO', v_86, 'Proyecto ficticio para probar el flujo del sistema.', 'Problema de investigación de demostración.', 'Justificación de demostración.', 'Marco teórico de demostración.', 'Metodología de demostración.', 'Un prototipo y una publicación de demostración.', 'Referencias ficticias, no usar como bibliografía.', '2026-02-01', '2026-12-31', v_15, 0, 0, 0) RETURNING id INTO v_125;
    INSERT INTO research.projects_researchers (project_id, researcher_id, researcher_role_id, starts_on)
    VALUES (v_125, v_56, v_90, '2026-02-01') RETURNING id INTO v_126;
    INSERT INTO research.projects_researchers (project_id, researcher_id, researcher_role_id, starts_on)
    VALUES (v_125, v_57, v_91, '2026-02-01') RETURNING id INTO v_127;
    INSERT INTO research.projects_leads (project_researcher_id, starts_at, is_current, change_reason)
    VALUES (v_126, '2026-02-01T00:00:00Z', TRUE, 'Nombramiento inicial') RETURNING id INTO v_128;
    INSERT INTO research.projects_students (project_id, student_id, activities, starts_on)
    VALUES (v_125, v_67, 'Apoyo en captura y pruebas.', '2026-02-01') RETURNING id INTO v_129;
    INSERT INTO research.projects_research_lines (project_id, research_line_id)
    VALUES (v_125, v_52) RETURNING id INTO v_130;
    INSERT INTO research.objectives (project_id, objective_type, description)
    VALUES (v_125, 'general', 'Evaluar una herramienta educativa de demostración.') RETURNING id INTO v_131;
    INSERT INTO research.objectives (project_id, objective_type, description)
    VALUES (v_125, 'specific', 'Desarrollar y evaluar un prototipo.') RETURNING id INTO v_132;
    INSERT INTO research.milestones (objective_id, title, due_on)
    VALUES (v_132, 'Prototipo funcional', '2026-09-30') RETURNING id INTO v_133;
    INSERT INTO research.tasks (milestone_id, title, starts_on, ends_on, responsible_participant_id)
    VALUES (v_133, 'Implementar prototipo', '2026-02-01', '2026-09-01', v_126) RETURNING id INTO v_134;
    INSERT INTO research.tasks_status_history (task_id, status_id, completion_percentage, changed_by, changed_at, reason)
    VALUES (v_134, v_1005, 65, v_15, '2026-06-30T10:00:00Z', 'Implementación parcial del prototipo — DEMO.') RETURNING id INTO v_1008;
    INSERT INTO research.projects_status_history (project_id, status_id, changed_by, changed_at, reason)
    VALUES (v_125, v_93, v_15, '2026-02-01T09:00:00Z', 'Registro inicial — DEMO') RETURNING id INTO v_135;
    INSERT INTO research.objectives_status_history (objective_id, status_id, changed_by, changed_at, reason, completion_percentage)
    VALUES (v_131, v_99, v_15, '2026-02-01T09:00:00Z', 'Registro inicial — DEMO', 0) RETURNING id INTO v_136;
    INSERT INTO research.objectives_status_history (objective_id, status_id, changed_by, changed_at, reason, completion_percentage)
    VALUES (v_132, v_100, v_15, '2026-02-01T09:00:00Z', 'Registro inicial — DEMO', 25) RETURNING id INTO v_137;
    INSERT INTO research.milestones_status_history (milestone_id, status_id, changed_by, changed_at, reason, completion_percentage)
    VALUES (v_133, v_103, v_15, '2026-02-01T09:00:00Z', 'Registro inicial — DEMO', 50) RETURNING id INTO v_138;
    INSERT INTO files.documents (document_type_id, original_file_name, storage_key, media_type, size_bytes, sha256, uploaded_by, uploaded_at, notes)
    VALUES (v_70, 'protocolo_v1.pdf', 'demo/projects/1/protocol-v1.pdf', 'application/pdf', 1024, '0000000000000000000000000000000000000000000000000000000000000000', v_15, '2026-02-01T10:00:00Z', 'DEMO: metadatos ficticios, archivo no disponible. El hash no representa un archivo real.') RETURNING id INTO v_140;
    INSERT INTO research.projects_documents (project_id, document_id, document_type_id, title, version_number, is_current)
    VALUES (v_109, v_140, v_70, 'Protocolo de investigación', 1, FALSE) RETURNING id INTO v_141;
    INSERT INTO files.documents (document_type_id, original_file_name, storage_key, media_type, size_bytes, sha256, uploaded_by, uploaded_at, notes)
    VALUES (v_70, 'protocolo_v2.pdf', 'demo/projects/1/protocol-v2.pdf', 'application/pdf', 1024, '0000000000000000000000000000000000000000000000000000000000000000', v_15, '2026-02-01T10:00:00Z', 'DEMO: metadatos ficticios, archivo no disponible. El hash no representa un archivo real.') RETURNING id INTO v_142;
    INSERT INTO research.projects_documents (project_id, document_id, document_type_id, title, version_number, is_current)
    VALUES (v_109, v_142, v_70, 'Protocolo de investigación', 2, TRUE) RETURNING id INTO v_143;
    INSERT INTO files.documents (document_type_id, original_file_name, storage_key, media_type, size_bytes, sha256, uploaded_by, uploaded_at, notes)
    VALUES (v_71, 'carta_cesion_firmada.pdf', 'demo/projects/1/rights.pdf', 'application/pdf', 1024, '0000000000000000000000000000000000000000000000000000000000000000', v_15, '2026-02-01T10:00:00Z', 'DEMO: metadatos ficticios, archivo no disponible. El hash no representa un archivo real.') RETURNING id INTO v_144;
    INSERT INTO research.projects_documents (project_id, document_id, document_type_id, title, version_number, is_current)
    VALUES (v_109, v_144, v_71, 'Carta de cesión firmada', 1, TRUE) RETURNING id INTO v_145;
    INSERT INTO files.documents (document_type_id, original_file_name, storage_key, media_type, size_bytes, sha256, uploaded_by, uploaded_at, notes)
    VALUES (v_74, 'evidencia_hito.pdf', 'demo/projects/1/milestone.pdf', 'application/pdf', 1024, '0000000000000000000000000000000000000000000000000000000000000000', v_15, '2026-02-01T10:00:00Z', 'DEMO: metadatos ficticios, archivo no disponible. El hash no representa un archivo real.') RETURNING id INTO v_146;
    INSERT INTO research.milestones_status_history_documents (document_id, document_type_id, title, version_number, is_current, status_history_id)
    VALUES (v_146, v_74, 'Captura del prototipo', 1, TRUE, v_123) RETURNING id INTO v_147;
    INSERT INTO research.projects_applications (project_id, call_id, application_number, applied_at, requested_amount)
    VALUES (v_109, v_69, 'DEMO-APP-001', '2026-02-03T10:00:00Z', 20000) RETURNING id INTO v_148;
    INSERT INTO research.projects_applications_documents (application_id, document_id, document_type_id, title, version_number, is_current)
    VALUES (v_148, v_142, v_70, 'Protocolo de postulación', 1, TRUE) RETURNING id INTO v_149;
    INSERT INTO research.projects_budget_items (project_id, budget_category_id, description, quantity, unit_cost)
    VALUES (v_109, v_88, 'Componentes para prototipo', 10, 1200) RETURNING id INTO v_150;
    INSERT INTO files.documents (document_type_id, original_file_name, storage_key, media_type, size_bytes, sha256, uploaded_by, uploaded_at, notes)
    VALUES (v_75, 'cotizacion.pdf', 'demo/projects/1/quotation.pdf', 'application/pdf', 1024, '0000000000000000000000000000000000000000000000000000000000000000', v_15, '2026-02-01T10:00:00Z', 'DEMO: metadatos ficticios, archivo no disponible. El hash no representa un archivo real.') RETURNING id INTO v_151;
    INSERT INTO research.projects_budget_documents (budget_item_id, document_id, document_type_id, title, version_number, is_current)
    VALUES (v_150, v_151, v_75, 'Cotización', 1, TRUE) RETURNING id INTO v_152;
    INSERT INTO research.projects_funding (project_id, funding_source_id, application_id, reference_number, requested_amount, approved_amount, received_amount)
    VALUES (v_109, v_89, v_148, 'DEMO-FIN-001', 20000, 0, 0) RETURNING id INTO v_153;
    INSERT INTO files.documents (document_type_id, original_file_name, storage_key, media_type, size_bytes, sha256, uploaded_by, uploaded_at, notes)
    VALUES (v_76, 'convenio.pdf', 'demo/projects/1/funding.pdf', 'application/pdf', 1024, '0000000000000000000000000000000000000000000000000000000000000000', v_15, '2026-02-01T10:00:00Z', 'DEMO: metadatos ficticios, archivo no disponible. El hash no representa un archivo real.') RETURNING id INTO v_154;
    INSERT INTO research.projects_funding_documents (funding_id, document_id, document_type_id, title, version_number, is_current)
    VALUES (v_153, v_154, v_76, 'Convenio de financiamiento', 1, TRUE) RETURNING id INTO v_155;
    INSERT INTO research.projects_expenses (budget_item_id, funding_id, description, spent_on, amount, supplier_name, receipt_number)
    VALUES (v_150, v_153, 'Compra inicial de componentes', '2026-03-10', 6000, 'Proveedor ficticio DEMO', 'DEMO-REC-001') RETURNING id INTO v_156;
    INSERT INTO files.documents (document_type_id, original_file_name, storage_key, media_type, size_bytes, sha256, uploaded_by, uploaded_at, notes)
    VALUES (v_77, 'comprobante_demo.pdf', 'demo/projects/1/receipt.pdf', 'application/pdf', 1024, '0000000000000000000000000000000000000000000000000000000000000000', v_15, '2026-02-01T10:00:00Z', 'DEMO: metadatos ficticios, archivo no disponible. El hash no representa un archivo real.') RETURNING id INTO v_157;
    INSERT INTO research.projects_expenses_documents (expense_id, document_id, document_type_id, title, version_number, is_current)
    VALUES (v_156, v_157, v_77, 'Comprobante de compra', 1, TRUE) RETURNING id INTO v_158;
    INSERT INTO research.research_outputs (project_id, output_type_id, title, produced_on, description)
    VALUES (v_109, v_106, 'Prototipo educativo — DEMO', '2026-09-01', 'Producto ficticio del proyecto.') RETURNING id INTO v_163;
    INSERT INTO research.projects
        (reference_code,title,organization_id,is_managed_in_platform,currency_id,starts_on,ends_on,created_by)
    VALUES ('DEMO-EXT-001','Proyecto histórico en universidad externa — DEMO',v_49,FALSE,v_86,'2024-01-01','2025-12-31',v_16)
    RETURNING id INTO v_2001;
    INSERT INTO research.projects_researchers(project_id,researcher_id,researcher_role_id,starts_on)
    VALUES(v_2001,v_57,v_90,'2024-01-01') RETURNING id INTO v_2002;
    INSERT INTO research.projects_leads(project_researcher_id,starts_at,change_reason)
    VALUES(v_2002,'2024-01-01T00:00:00Z','Responsable registrado del proyecto histórico — DEMO') RETURNING id INTO v_2003;
    INSERT INTO research.projects_status_history(project_id,status_id,changed_by,changed_at,reason)
    VALUES(v_2001,v_97,v_16,'2025-12-31T10:00:00Z','Antecedente histórico concluido — DEMO') RETURNING id INTO v_2004;
    INSERT INTO research.research_outputs (project_id, output_type_id, title, produced_on, bibliographic_citation)
    VALUES (v_2001, v_105, 'Artículo externo — DEMO', '2025-05-01', 'Referencia ficticia para probar el CV.') RETURNING id INTO v_164;
    INSERT INTO research.research_outputs_researchers (research_output_id, researcher_id, author_order, is_corresponding_author)
    VALUES (v_163, v_56, 1, TRUE) RETURNING id INTO v_165;
    INSERT INTO research.research_outputs_researchers (research_output_id, researcher_id, author_order, is_corresponding_author)
    VALUES (v_163, v_57, 2, FALSE) RETURNING id INTO v_166;
    INSERT INTO research.research_outputs_researchers (researcher_id, author_order, is_corresponding_author, research_output_id)
    VALUES (v_57, 1, TRUE, v_164) RETURNING id INTO v_167;
    INSERT INTO files.documents (document_type_id, original_file_name, storage_key, media_type, size_bytes, sha256, uploaded_by, uploaded_at, notes)
    VALUES (v_78, 'descripcion_producto.pdf', 'demo/projects/1/output.pdf', 'application/pdf', 1024, '0000000000000000000000000000000000000000000000000000000000000000', v_15, '2026-02-01T10:00:00Z', 'DEMO: metadatos ficticios, archivo no disponible. El hash no representa un archivo real.') RETURNING id INTO v_168;
    INSERT INTO research.research_outputs_documents (research_output_id, document_id, document_type_id, title, version_number, is_current)
    VALUES (v_163, v_168, v_78, 'Descripción del producto', 1, TRUE) RETURNING id INTO v_169;
    INSERT INTO reviews.c_committee_roles (code, name)
    VALUES ('president', 'Presidente') RETURNING id INTO v_170;
    INSERT INTO reviews.c_committee_roles (code, name)
    VALUES ('secretary', 'Secretario') RETURNING id INTO v_171;
    INSERT INTO reviews.c_committee_roles (code, name)
    VALUES ('program_head', 'Jefe de carrera') RETURNING id INTO v_172;
    INSERT INTO reviews.c_review_outcomes (code, name)
    VALUES ('approved', 'Aprobado') RETURNING id INTO v_173;
    INSERT INTO reviews.c_review_outcomes (code, name)
    VALUES ('approved_with_minor_changes', 'Aprobado con cambios menores') RETURNING id INTO v_174;
    INSERT INTO reviews.c_review_outcomes (code, name)
    VALUES ('not_approved', 'No aprobado') RETURNING id INTO v_175;
    INSERT INTO reviews.c_academic_authorization_statuses (code, name)
    VALUES ('authorized', 'Autorizado') RETURNING id INTO v_179;
    INSERT INTO reviews.c_academic_authorization_statuses (code, name)
    VALUES ('changes_requested', 'Requiere correcciones') RETURNING id INTO v_180;
    INSERT INTO reviews.c_academic_authorization_statuses (code, name)
    VALUES ('rejected', 'Rechazado') RETURNING id INTO v_181;
    INSERT INTO reviews.c_academic_authorization_statuses (code, name)
    VALUES ('revoked', 'Revocado') RETURNING id INTO v_182;
    INSERT INTO reviews.committees (name, academic_program_id, starts_on, ends_on)
    VALUES ('Comité de Investigación DEMO 2026', v_50, '2026-01-01', '2026-12-31') RETURNING id INTO v_183;
    INSERT INTO reviews.committees_members (committee_id, user_id, committee_role_id, starts_on, ends_on)
    VALUES (v_183, v_18, v_170, '2026-01-01', '2026-12-31') RETURNING id INTO v_184;
    INSERT INTO reviews.committees_members (committee_id, user_id, committee_role_id, starts_on, ends_on)
    VALUES (v_183, v_19, v_171, '2026-01-01', '2026-12-31') RETURNING id INTO v_185;
    INSERT INTO reviews.committees_members (committee_id, user_id, committee_role_id, starts_on, ends_on)
    VALUES (v_183, v_20, v_172, '2026-01-01', '2026-12-31') RETURNING id INTO v_186;
    UPDATE research.projects_documents SET is_current=FALSE WHERE id=v_143;
    UPDATE research.projects_documents SET is_current=TRUE WHERE id=v_141;
    -- Cuerpos y grados ficticios: ejemplos de pertenencia, baja y evolución.
    INSERT INTO academic.c_academic_group_levels(code,name,display_order)
    VALUES('caef','En formación',1) RETURNING id INTO v_3001;
    INSERT INTO academic.c_academic_group_levels(code,name,display_order)
    VALUES('caec','En consolidación',2) RETURNING id INTO v_3002;
    INSERT INTO academic.c_academic_group_levels(code,name,display_order)
    VALUES('cac','Consolidado',3) RETURNING id INTO v_3003;
    INSERT INTO academic.academic_groups(code,name,organization_id,description)
    VALUES('DEMO-CA-001','Tecnologías para el aprendizaje — DEMO',v_48,'Cuerpo académico ficticio del TecNM.') RETURNING id INTO v_3004;
    INSERT INTO academic.academic_groups(code,name,organization_id,description)
    VALUES('DEMO-CA-002','Investigación educativa aplicada — DEMO',v_49,'Cuerpo académico ficticio de otra organización.') RETURNING id INTO v_3005;
    INSERT INTO academic.academic_groups_researchers(academic_group_id,researcher_id,starts_on,is_leader)
    VALUES(v_3004,v_56,'2024-01-01',TRUE) RETURNING id INTO v_3006;
    INSERT INTO academic.academic_groups_researchers(academic_group_id,researcher_id,starts_on,ends_on)
    VALUES(v_3004,v_57,'2024-01-01','2024-12-31') RETURNING id INTO v_3007;
    INSERT INTO academic.academic_groups_researchers(academic_group_id,researcher_id,starts_on,is_leader)
    VALUES(v_3005,v_57,'2025-01-01',TRUE) RETURNING id INTO v_3008;
    INSERT INTO academic.academic_groups_status_history(academic_group_id,level_id,changed_by,changed_at,reason)
    VALUES(v_3004,v_3001,v_14,'2024-01-01T10:00:00Z','Registro inicial en formación — DEMO.') RETURNING id INTO v_3009;
    INSERT INTO academic.academic_groups_status_history(academic_group_id,level_id,changed_by,changed_at,reason)
    VALUES(v_3004,v_3002,v_14,'2025-01-01T10:00:00Z','Cambio a en consolidación — DEMO.') RETURNING id INTO v_3010;
    INSERT INTO academic.academic_groups_status_history(academic_group_id,level_id,changed_by,changed_at,reason)
    VALUES(v_3004,v_3003,v_14,'2026-01-01T10:00:00Z','Reconocimiento como consolidado — DEMO.') RETURNING id INTO v_3011;
    INSERT INTO academic.academic_groups_status_history(academic_group_id,level_id,changed_by,changed_at,reason)
    VALUES(v_3005,v_3001,v_14,'2025-01-01T10:00:00Z','Registro inicial en formación — DEMO.');
    INSERT INTO research.projects_academic_groups(project_id,academic_group_id)
    VALUES(v_109,v_3004) RETURNING id INTO v_3012;
    INSERT INTO research.projects_academic_groups(project_id,academic_group_id)
    VALUES(v_109,v_3005);
    -- Sergio también es investigador; su perfil no está adscrito a ningún cuerpo.
    INSERT INTO academic.researchers(user_id,professional_summary)
    VALUES(v_19,'Investigador de demostración sin pertenencia a un cuerpo académico.') RETURNING id INTO v_3013;
    INSERT INTO access.users_roles(user_id,role_id) VALUES(v_19,v_2) RETURNING id INTO v_3014;
    INSERT INTO reviews.projects_submissions (project_id, committee_id, application_id, version_number, submitted_by, submitted_at, processing_status)
    VALUES (v_109, v_183, v_148, 1, v_15, '2026-02-04T10:00:00Z', 'completed') RETURNING id INTO v_187;
    UPDATE research.projects_documents SET is_current=FALSE WHERE id=v_141;
    UPDATE research.projects_documents SET is_current=TRUE WHERE id=v_143;
    INSERT INTO reviews.projects_submissions (project_id, committee_id, application_id, version_number, submitted_by, submitted_at, processing_status)
    VALUES (v_109, v_183, v_148, 2, v_15, '2026-02-10T10:00:00Z', 'in_process') RETURNING id INTO v_189;
    INSERT INTO reviews.projects_reviews (submission_id, committee_id, committee_member_id, outcome_id, assessment, reviewed_at, status_id)
    VALUES (v_189, v_183, v_184, v_173, 'Revisión favorable de demostración.', '2026-02-11T10:00:00Z', v_1003) RETURNING id INTO v_192;
    INSERT INTO reviews.projects_reviews (submission_id, committee_id, committee_member_id, outcome_id, assessment, reviewed_at, status_id)
    VALUES (v_189, v_183, v_185, v_174, 'Aprobado con cambios menores: precisar indicadores de evaluación — DEMO.', '2026-02-11T10:00:00Z', v_1003) RETURNING id INTO v_193;
    INSERT INTO reviews.projects_reviews (submission_id, committee_id, committee_member_id, outcome_id, assessment, reviewed_at, status_id)
    VALUES (v_189, v_183, v_186, v_175, 'No aprobado: justificar el alcance de la muestra — DEMO.', '2026-02-11T10:00:00Z', v_1003) RETURNING id INTO v_194;
    INSERT INTO files.documents (document_type_id, original_file_name, storage_key, media_type, size_bytes, sha256, uploaded_by, uploaded_at, notes)
    VALUES (v_73, 'autorizacion_v1.pdf', 'demo/projects/1/auth-v1.pdf', 'application/pdf', 1024, '0000000000000000000000000000000000000000000000000000000000000000', v_21, '2026-02-01T10:00:00Z', 'DEMO: metadatos ficticios, archivo no disponible. El hash no representa un archivo real.') RETURNING id INTO v_201;
    INSERT INTO research.projects_documents (project_id, document_id, document_type_id, title, version_number, is_current)
    VALUES (v_109, v_201, v_73, 'Autorización académica', 1, FALSE) RETURNING id INTO v_202;
    INSERT INTO files.documents (document_type_id, original_file_name, storage_key, media_type, size_bytes, sha256, uploaded_by, uploaded_at, notes)
    VALUES (v_73, 'autorizacion_v2.pdf', 'demo/projects/1/auth-v2.pdf', 'application/pdf', 1024, '0000000000000000000000000000000000000000000000000000000000000000', v_21, '2026-02-01T10:00:00Z', 'DEMO: metadatos ficticios, archivo no disponible. El hash no representa un archivo real.') RETURNING id INTO v_203;
    INSERT INTO research.projects_documents (project_id, document_id, document_type_id, title, version_number, is_current)
    VALUES (v_109, v_203, v_73, 'Autorización académica', 2, TRUE) RETURNING id INTO v_204;
    INSERT INTO reviews.academic_authorizations_history
        (submission_id,status_id,authorized_by,decided_at,rationale,document_id,created_by)
    VALUES(v_189,v_180,v_21,'2026-02-14T10:00:00Z','Precisar el financiamiento — DEMO',v_201,v_21);
    INSERT INTO reviews.academic_authorizations_history
        (submission_id,status_id,authorized_by,decided_at,rationale,document_id,created_by)
    VALUES(v_189,v_179,v_21,'2026-02-16T10:00:00Z','Proyecto autorizado — DEMO',v_203,v_21);
    UPDATE reviews.projects_submissions SET processing_status = 'completed', updated_by = v_21 WHERE id = v_189;
    INSERT INTO research.projects_status_history (project_id, status_id, changed_by, changed_at, reason)
    VALUES (v_109, v_95, v_21, '2026-02-16T10:00:00Z', 'Autorización académica registrada.') RETURNING id INTO v_207;
    UPDATE research.projects_funding SET approved_amount = 15000, received_amount = 15000, awarded_on = '2026-02-15', updated_by = v_14 WHERE id = v_153;
    UPDATE research.projects_leads SET is_current = FALSE, ends_at = '2026-05-01T00:00:00Z', updated_by = v_14 WHERE id = v_112;
    INSERT INTO research.projects_leads (project_researcher_id, starts_at, is_current, change_reason)
    VALUES (v_111, '2026-05-01T00:00:00Z', TRUE, 'Sustitución de responsable — DEMO') RETURNING id INTO v_113;
    INSERT INTO access.audit_logs (actor_user_id, action, entity_schema, entity_table, entity_id, project_id, request_id, details)
    VALUES (v_14, 'demo.seed', 'research', 'projects', v_109, v_109, 'demo-seed-2026', '{"demo": true, "description": "Carga inicial ficticia para desarrollo."}'::jsonb) RETURNING id INTO v_208;
    INSERT INTO research.objectives_status_history_documents (status_history_id, document_id, document_type_id, title, version_number, is_current)
    VALUES (v_121, v_146, v_74, 'Evidencia del avance del objetivo', 1, TRUE) RETURNING id INTO v_1009;
    INSERT INTO research.tasks_status_history_documents (status_history_id, document_id, document_type_id, title, version_number, is_current)
    VALUES (v_1007, v_146, v_74, 'Evidencia de implementación de la tarea', 1, TRUE) RETURNING id INTO v_1010;
    INSERT INTO files.documents (document_type_id, original_file_name, storage_key, media_type, size_bytes, sha256, uploaded_by, notes)
    VALUES (v_78, 'producto_externo_demo.pdf', 'demo/external/producto_externo_demo.pdf', 'application/pdf', 1024, '0000000000000000000000000000000000000000000000000000000000000000', v_16, 'DEMO: metadatos ficticios, archivo no disponible.') RETURNING id INTO v_1011;
    INSERT INTO research.research_outputs_documents (document_id, document_type_id, title, version_number, is_current, research_output_id)
    VALUES (v_1011, v_78, 'Evidencia del producto externo', 1, TRUE, v_164) RETURNING id INTO v_1012;
    INSERT INTO reviews.projects_submissions (project_id, committee_id, version_number, submitted_by, submitted_at)
    VALUES (v_125, v_183, 1, v_15, '2026-08-01T10:00:00Z') RETURNING id INTO v_1013;
    INSERT INTO reviews.projects_reviews (submission_id, committee_id, committee_member_id, status_id)
    VALUES (v_1013, v_183, v_184, v_1001) RETURNING id INTO v_1014;
END $$;


\echo '5/5 Verificando estructura, ejemplos y permisos...'
DO $verify$
DECLARE r RECORD;n BIGINT;
BEGIN
 IF (SELECT count(*) FROM pg_namespace WHERE nspname IN ('access','academic','calls','research','reviews','files'))<>6 THEN RAISE EXCEPTION 'Se requieren seis esquemas';END IF;
 IF (SELECT count(*) FROM pg_class c JOIN pg_namespace ns ON ns.oid=c.relnamespace WHERE ns.nspname IN ('access','academic','calls','research','reviews','files') AND c.relkind='r')<>76 THEN RAISE EXCEPTION 'Se requieren 76 tablas';END IF;
 FOR r IN SELECT ns.nspname,c.relname,c.relowner FROM pg_class c JOIN pg_namespace ns ON ns.oid=c.relnamespace
 WHERE ns.nspname IN ('access','academic','calls','research','reviews','files') AND c.relkind='r'
 LOOP
  IF pg_get_userbyid(r.relowner)<>'tecnm_user_research' THEN RAISE EXCEPTION 'Propietario incorrecto en %.%',r.nspname,r.relname;END IF;
  EXECUTE format('SELECT count(*) FROM %I.%I',r.nspname,r.relname) INTO n;
  IF n=0 THEN RAISE EXCEPTION 'Faltan ejemplos en %.%',r.nspname,r.relname;END IF;
 END LOOP;
 IF (SELECT count(*) FROM research.projects)<>3 THEN RAISE EXCEPTION 'Se requieren tres proyectos DEMO';END IF;
 IF NOT EXISTS(SELECT 1 FROM research.projects p JOIN academic.c_organizations o ON o.id=p.organization_id
  WHERE p.reference_code='DEMO-EXT-001' AND o.code='DEMO-EXT' AND NOT p.is_managed_in_platform) THEN
  RAISE EXCEPTION 'Falta el ejemplo del proyecto externo';END IF;
 IF (SELECT count(*) FROM reviews.academic_authorizations_history)<>2 THEN RAISE EXCEPTION 'Se requieren dos decisiones académicas de ejemplo';END IF;
 IF (SELECT count(*) FROM academic.academic_groups)<>2 THEN RAISE EXCEPTION 'Faltan cuerpos académicos de ejemplo';END IF;
 IF (SELECT count(*) FROM academic.academic_groups_status_history)<>4 THEN RAISE EXCEPTION 'Falta el historial de grados de consolidación';END IF;
 IF NOT EXISTS(SELECT 1 FROM academic.researchers ar WHERE NOT EXISTS(SELECT 1 FROM academic.academic_groups_researchers m WHERE m.researcher_id=ar.id)) THEN
  RAISE EXCEPTION 'Falta el ejemplo de investigador sin cuerpo académico';END IF;
END $verify$;
-- Tablas y ejemplos se confirman juntos. Los triggers diferidos verifican responsables y versiones.
COMMIT;
SELECT current_database() database_name,current_user objects_owner,6 application_schemas,76 tables_with_sample_data;
SELECT p.reference_code,u.given_name||' '||u.family_name current_lead
FROM research.projects p JOIN research.projects_researchers pr ON pr.project_id=p.id
JOIN research.projects_leads l ON l.project_researcher_id=pr.id AND l.is_current
JOIN academic.researchers r ON r.id=pr.researcher_id JOIN access.users u ON u.id=r.user_id ORDER BY p.reference_code;
-- Última decisión por envío: fecha descendente y después id descendente.
SELECT s.id,s.version_number,st.name academic_status,d.decided_at
FROM reviews.projects_submissions s
LEFT JOIN LATERAL (
 SELECT h.status_id,h.decided_at FROM reviews.academic_authorizations_history h
 WHERE h.submission_id=s.id ORDER BY h.decided_at DESC,h.id DESC LIMIT 1
) d ON TRUE
LEFT JOIN reviews.c_academic_authorization_statuses st ON st.id=d.status_id ORDER BY s.id;
SELECT p.reference_code,o.name organization,p.is_managed_in_platform
FROM research.projects p JOIN academic.c_organizations o ON o.id=p.organization_id ORDER BY p.reference_code;
-- Grado actual: última entrada del historial (no existe una copia en academic_groups).
SELECT g.code,g.name,l.name consolidation_level,h.changed_at
FROM academic.academic_groups g
LEFT JOIN LATERAL (SELECT gh.level_id,gh.changed_at FROM academic.academic_groups_status_history gh
 WHERE gh.academic_group_id=g.id ORDER BY gh.changed_at DESC,gh.id DESC LIMIT 1) h ON TRUE
LEFT JOIN academic.c_academic_group_levels l ON l.id=h.level_id ORDER BY g.code;
-- Integrantes vigentes a la fecha de consulta; las bajas permanecen en la tabla.
SELECT g.code,u.given_name||' '||u.family_name researcher,m.is_leader,m.starts_on
FROM academic.academic_groups_researchers m JOIN academic.academic_groups g ON g.id=m.academic_group_id
JOIN academic.researchers r ON r.id=m.researcher_id JOIN access.users u ON u.id=r.user_id
WHERE m.is_active AND m.deleted_at IS NULL AND m.starts_on<=CURRENT_DATE
 AND (m.ends_on IS NULL OR m.ends_on>=CURRENT_DATE) ORDER BY g.code,u.family_name;
RESET ROLE;
\echo 'INSTALACION COMPLETADA CORRECTAMENTE.'
\echo 'Base: tecnm_research_db | Usuario: tecnm_user_research'
\echo '76 tablas con datos de ejemplo. Los documentos y formatos son metadatos ficticios.'
