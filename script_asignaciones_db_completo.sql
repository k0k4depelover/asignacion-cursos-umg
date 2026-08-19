-- =====================================================================
-- Sistema de Asignación de Cursos Universitarios (UMG)
-- Script FINAL: esquema completo + datos semilla (usuarios, catálogo,
-- secciones) listo para ejecutar de una sola vez.
--
-- Basado en `script_asignaciones_db.sql` (esquema, sin cambios) +
-- INSERTs de datos demo (nuevos, agregados para poder usar la app
-- de inmediato).
--
-- Uso:
--   mysql -u root -p < script_asignaciones_db_completo.sql
-- o desde un cliente (Workbench, DBeaver, HeidiSQL, etc.) simplemente
-- ejecutar todo el archivo.
-- =====================================================================

-- Si la base ya existe de una corrida anterior y quieres partir limpio,
-- descomenta la siguiente línea (BORRA todos los datos existentes):
-- DROP DATABASE IF EXISTS asignacion_cursos;

CREATE DATABASE IF NOT EXISTS asignacion_cursos
    CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

USE asignacion_cursos;

-- =====================================================================
-- 1. TABLAS SIN DEPENDENCIAS (no tienen FK hacia otras tablas)
-- =====================================================================

CREATE TABLE rol (
    id_rol          INT AUTO_INCREMENT PRIMARY KEY,
    nombre_rol      VARCHAR(255) NOT NULL,
    descripcion_rol TEXT NULL,
    estado_rol      VARCHAR(50) NOT NULL DEFAULT 'activo'
) ENGINE=InnoDB;

CREATE TABLE permiso (
    id_permiso          INT AUTO_INCREMENT PRIMARY KEY,
    nombre_permiso      VARCHAR(255) NOT NULL,
    descripcion_permiso TEXT NULL
) ENGINE=InnoDB;

CREATE TABLE facultad (
    id_facultad     INT AUTO_INCREMENT PRIMARY KEY,
    codigo_facultad VARCHAR(50) NOT NULL,
    nombre_facultad VARCHAR(255) NOT NULL,
    estado_facultad VARCHAR(50) NOT NULL DEFAULT 'activo'
) ENGINE=InnoDB;

CREATE TABLE edificio (
    id_edificio        INT AUTO_INCREMENT PRIMARY KEY,
    codigo_edificio    VARCHAR(50) NOT NULL,
    nombre_edificio    VARCHAR(255) NOT NULL,
    sede_edificio      VARCHAR(255) NOT NULL,
    ubicacion_edificio VARCHAR(255) NULL,
    estado_edificio    VARCHAR(50) NOT NULL DEFAULT 'activo'
) ENGINE=InnoDB;

CREATE TABLE periodo_academico (
    id_periodo_academico          INT AUTO_INCREMENT PRIMARY KEY,
    codigo_periodo_academico      VARCHAR(50) NOT NULL,
    descripcion_periodo_academico VARCHAR(255) NULL,
    tipo_periodo_academico        VARCHAR(50) NOT NULL,
    fecha_inicio_periodo_academico        DATE NOT NULL,
    fecha_fin_periodo_academico           DATE NOT NULL,
    permite_inscripcion_periodo_academico BOOLEAN NOT NULL DEFAULT FALSE,
    permite_asignacion_periodo_academico  BOOLEAN NOT NULL DEFAULT FALSE,
    estado_periodo_academico      VARCHAR(50) NOT NULL DEFAULT 'activo'
) ENGINE=InnoDB;

CREATE TABLE curso (
    id_curso                   INT AUTO_INCREMENT PRIMARY KEY,
    codigo_curso                VARCHAR(50) NOT NULL,
    nombre_curso                VARCHAR(255) NOT NULL,
    creditos_curso               INT NOT NULL,
    requiere_laboratorio_curso   BOOLEAN NOT NULL DEFAULT FALSE,
    estado_curso                 VARCHAR(50) NOT NULL DEFAULT 'activo'
) ENGINE=InnoDB;

-- =====================================================================
-- 2. TABLAS CON UNA SOLA DEPENDENCIA
-- =====================================================================

-- usuario depende de rol (1:N -> un rol tiene muchos usuarios)
CREATE TABLE usuario (
    id_usuario                  INT AUTO_INCREMENT PRIMARY KEY,
    nombre_usuario               VARCHAR(255) NOT NULL,
    correo_login_usuario         VARCHAR(255) NOT NULL UNIQUE,
    correo_recuperacion_usuario  VARCHAR(255) NULL,
    contrasena_hash_usuario      VARCHAR(255) NOT NULL,
    tiene_pass_temporal_usuario  BOOLEAN NOT NULL DEFAULT FALSE,
    estado_usuario               VARCHAR(50) NOT NULL DEFAULT 'activo',
    fecha_registro_usuario       DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    id_rol_usuario               INT NOT NULL,
    CONSTRAINT fk_usuario_rol FOREIGN KEY (id_rol_usuario) REFERENCES rol (id_rol)
) ENGINE=InnoDB;

-- carrera depende de facultad (1:N -> una facultad tiene muchas carreras)
CREATE TABLE carrera (
    id_carrera            INT AUTO_INCREMENT PRIMARY KEY,
    codigo_carrera         VARCHAR(50) NOT NULL,
    nombre_carrera         VARCHAR(255) NOT NULL,
    total_ciclos_carrera   INT NOT NULL,
    estado_carrera         VARCHAR(50) NOT NULL DEFAULT 'activo',
    id_facultad_carrera    INT NOT NULL,
    CONSTRAINT fk_carrera_facultad FOREIGN KEY (id_facultad_carrera) REFERENCES facultad (id_facultad)
) ENGINE=InnoDB;

-- salon depende de edificio (1:N -> un edificio tiene muchos salones)
CREATE TABLE salon (
    id_salon           INT AUTO_INCREMENT PRIMARY KEY,
    codigo_salon        VARCHAR(50) NOT NULL,
    nombre_salon        VARCHAR(255) NOT NULL,
    capacidad_salon      INT NOT NULL,
    tipo_espacio_salon   VARCHAR(50) NOT NULL,
    nivel_salon          INT NOT NULL,
    estado_salon         VARCHAR(50) NOT NULL DEFAULT 'activo',
    id_edificio_salon    INT NOT NULL,
    CONSTRAINT fk_salon_edificio FOREIGN KEY (id_edificio_salon) REFERENCES edificio (id_edificio)
) ENGINE=InnoDB;

-- =====================================================================
-- 3. TABLAS CON DEPENDENCIAS DE SEGUNDO NIVEL
-- =====================================================================

-- pensum depende de carrera
CREATE TABLE pensum (
    id_pensum          INT AUTO_INCREMENT PRIMARY KEY,
    codigo_pensum       VARCHAR(50) NOT NULL,
    anio_pensum         INT NOT NULL,
    jornada_pensum      VARCHAR(50) NOT NULL,
    estado_pensum       VARCHAR(50) NOT NULL DEFAULT 'activo',
    id_carrera_pensum   INT NOT NULL,
    CONSTRAINT fk_pensum_carrera FOREIGN KEY (id_carrera_pensum) REFERENCES carrera (id_carrera)
) ENGINE=InnoDB;

-- laboratorio depende de salon
CREATE TABLE laboratorio (
    id_laboratorio           INT AUTO_INCREMENT PRIMARY KEY,
    nombre_laboratorio        VARCHAR(255) NOT NULL,
    descripcion_laboratorio   TEXT NULL,
    estado_laboratorio        VARCHAR(50) NOT NULL DEFAULT 'activo',
    id_salon_laboratorio      INT NOT NULL,
    CONSTRAINT fk_laboratorio_salon FOREIGN KEY (id_salon_laboratorio) REFERENCES salon (id_salon)
) ENGINE=InnoDB;

-- estudiante depende de usuario y pensum
CREATE TABLE estudiante (
    id_estudiante                INT AUTO_INCREMENT PRIMARY KEY,
    carnet_estudiante             VARCHAR(50) NOT NULL UNIQUE,
    dpi_estudiante                 VARCHAR(20) NOT NULL UNIQUE,
    nombres_estudiante             VARCHAR(255) NOT NULL,
    apellidos_estudiante           VARCHAR(255) NOT NULL,
    fecha_nacimiento_estudiante    DATE NOT NULL,
    direccion_estudiante           VARCHAR(255) NULL,
    telefono_estudiante            VARCHAR(20) NULL,
    ciclo_actual_estudiante        INT NOT NULL DEFAULT 1,
    estado_estudiante              VARCHAR(50) NOT NULL DEFAULT 'activo',
    id_usuario_estudiante          INT NOT NULL,
    id_pensum_estudiante           INT NOT NULL,
    CONSTRAINT fk_estudiante_usuario FOREIGN KEY (id_usuario_estudiante) REFERENCES usuario (id_usuario),
    CONSTRAINT fk_estudiante_pensum FOREIGN KEY (id_pensum_estudiante) REFERENCES pensum (id_pensum)
) ENGINE=InnoDB;

-- catedratico depende de usuario
CREATE TABLE catedratico (
    id_catedratico          INT AUTO_INCREMENT PRIMARY KEY,
    codigo_catedratico       VARCHAR(50) NOT NULL UNIQUE,
    dpi_catedratico           VARCHAR(20) NOT NULL UNIQUE,
    nombres_catedratico       VARCHAR(255) NOT NULL,
    apellidos_catedratico     VARCHAR(255) NOT NULL,
    telefono_catedratico      VARCHAR(20) NULL,
    profesion_catedratico     VARCHAR(255) NULL,
    estado_catedratico        VARCHAR(50) NOT NULL DEFAULT 'activo',
    id_usuario_catedratico    INT NOT NULL,
    CONSTRAINT fk_catedratico_usuario FOREIGN KEY (id_usuario_catedratico) REFERENCES usuario (id_usuario)
) ENGINE=InnoDB;

-- =====================================================================
-- 4. TABLAS ASOCIATIVAS PURAS (relaciones N:M sin atributos propios
--    relevantes): SIN id propio, PK COMPUESTA por las 2 FK.
-- =====================================================================

-- rol_permiso: N:M entre rol y permiso
CREATE TABLE rol_permiso (
    id_rol      INT NOT NULL,
    id_permiso  INT NOT NULL,
    PRIMARY KEY (id_rol, id_permiso),
    CONSTRAINT fk_rolpermiso_rol FOREIGN KEY (id_rol) REFERENCES rol (id_rol),
    CONSTRAINT fk_rolpermiso_permiso FOREIGN KEY (id_permiso) REFERENCES permiso (id_permiso)
) ENGINE=InnoDB;

-- pensum_curso: N:M entre pensum y curso, con datos propios (ciclo,
-- es_obligatorio). Aun así sigue siendo asociativa pura para efectos de
-- llave: PK compuesta (id_pensum, id_curso), sin id propio.
CREATE TABLE pensum_curso (
    id_pensum_curso                INT AUTO_INCREMENT PRIMARY KEY,
    id_pensum                     INT NOT NULL,
    id_curso                      INT NOT NULL,
    ciclo_pensum_curso            INT NOT NULL,
    es_obligatorio_pensum_curso   BOOLEAN NOT NULL DEFAULT TRUE,
    UNIQUE KEY uq_pensum_curso (id_pensum, id_curso),
    CONSTRAINT fk_pensumcurso_pensum FOREIGN KEY (id_pensum) REFERENCES pensum (id_pensum),
    CONSTRAINT fk_pensumcurso_curso FOREIGN KEY (id_curso) REFERENCES curso (id_curso)
) ENGINE=InnoDB;

-- =====================================================================
-- 5. REQUISITOS DE CURSOS
-- =====================================================================

CREATE TABLE requisito_curso (
    id_requisito                  INT AUTO_INCREMENT PRIMARY KEY,
    tipo_requisito                VARCHAR(50) NOT NULL,
    creditos_minimos_requisito    INT NULL,
    descripcion_requisito         VARCHAR(255) NULL,
    id_pensum_curso_requisito     INT NOT NULL,
    id_curso_requisito            INT NOT NULL,
    CONSTRAINT fk_requisito_pensumcurso FOREIGN KEY (id_pensum_curso_requisito) REFERENCES pensum_curso (id_pensum_curso),
    CONSTRAINT fk_requisito_cursorequerido FOREIGN KEY (id_curso_requisito) REFERENCES curso (id_curso)
) ENGINE=InnoDB;

-- =====================================================================
-- 6. SECCIONES (el curso "ofrecido" en un periodo específico)
-- =====================================================================

CREATE TABLE seccion (
    id_seccion                    INT AUTO_INCREMENT PRIMARY KEY,
    codigo_seccion                VARCHAR(50) NOT NULL,
    jornada_seccion                VARCHAR(50) NOT NULL,
    cupo_maximo_seccion            INT NOT NULL,
    estado_seccion                 VARCHAR(50) NOT NULL DEFAULT 'activo',
    id_curso_seccion               INT NOT NULL,
    id_periodo_academico_seccion   INT NOT NULL,
    id_catedratico_seccion         INT NOT NULL,
    id_salon_seccion               INT NOT NULL,
    CONSTRAINT fk_seccion_curso FOREIGN KEY (id_curso_seccion) REFERENCES curso (id_curso),
    CONSTRAINT fk_seccion_periodo FOREIGN KEY (id_periodo_academico_seccion) REFERENCES periodo_academico (id_periodo_academico),
    CONSTRAINT fk_seccion_catedratico FOREIGN KEY (id_catedratico_seccion) REFERENCES catedratico (id_catedratico),
    CONSTRAINT fk_seccion_salon FOREIGN KEY (id_salon_seccion) REFERENCES salon (id_salon)
) ENGINE=InnoDB;

-- horario_seccion depende de seccion (1:N -> una seccion tiene varios horarios)
CREATE TABLE horario_seccion (
    id_horario           INT AUTO_INCREMENT PRIMARY KEY,
    dia_semana_horario    VARCHAR(20) NOT NULL,
    hora_inicio_horario   TIME NOT NULL,
    hora_fin_horario      TIME NOT NULL,
    tipo_sesion_horario   VARCHAR(50) NOT NULL,
    id_seccion_horario    INT NOT NULL,
    CONSTRAINT fk_horario_seccion FOREIGN KEY (id_seccion_horario) REFERENCES seccion (id_seccion)
) ENGINE=InnoDB;

-- seccion_laboratorio: N:M entre seccion y laboratorio, con datos propios
-- (horario, costo). Tabla asociativa pura: PK compuesta, sin id propio.
CREATE TABLE seccion_laboratorio (
    id_seccion_laboratorio           INT AUTO_INCREMENT PRIMARY KEY,
    dia_semana_seccion_laboratorio   VARCHAR(20) NOT NULL,
    hora_inicio_seccion_laboratorio  TIME NOT NULL,
    hora_fin_seccion_laboratorio     TIME NOT NULL,
    costo_extra_seccion_laboratorio  DECIMAL(10,2) NOT NULL DEFAULT 0.00,
    id_seccion                       INT NOT NULL,
    id_laboratorio                   INT NOT NULL,
    UNIQUE KEY uq_seccion_laboratorio (id_seccion, id_laboratorio),
    CONSTRAINT fk_secclab_seccion FOREIGN KEY (id_seccion) REFERENCES seccion (id_seccion),
    CONSTRAINT fk_secclab_laboratorio FOREIGN KEY (id_laboratorio) REFERENCES laboratorio (id_laboratorio)
) ENGINE=InnoDB;

-- =====================================================================
-- 7. INSCRIPCIÓN -> ASIGNACIÓN -> DETALLE_ASIGNACIÓN
--    (se dejan sin datos: se generan usando el asistente de inscripción
--     de la app, con el usuario estudiante@umg.edu.gt)
-- =====================================================================

CREATE TABLE inscripcion (
    id_inscripcion                  INT AUTO_INCREMENT PRIMARY KEY,
    fecha_inscripcion                DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    costo_inscripcion                DECIMAL(10,2) NOT NULL DEFAULT 0.00,
    monto_mensual                    DECIMAL(10,2) NOT NULL DEFAULT 0.00,
    ciclo_inscrito                   INT NOT NULL,
    estado_solvencia                 VARCHAR(50) NOT NULL DEFAULT 'solvente',
    estado_inscripcion               VARCHAR(50) NOT NULL DEFAULT 'activo',
    id_estudiante_inscripcion        INT NOT NULL,
    id_periodo_academico_inscripcion INT NOT NULL,
    CONSTRAINT fk_inscripcion_estudiante FOREIGN KEY (id_estudiante_inscripcion) REFERENCES estudiante (id_estudiante),
    CONSTRAINT fk_inscripcion_periodo FOREIGN KEY (id_periodo_academico_inscripcion) REFERENCES periodo_academico (id_periodo_academico)
) ENGINE=InnoDB;

-- asignacion depende de inscripcion, relación 1:1 (una inscripción genera una asignación)
CREATE TABLE asignacion (
    id_asignacion              INT AUTO_INCREMENT PRIMARY KEY,
    fecha_asignacion            DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    subtotal_laboratorios       DECIMAL(10,2) NOT NULL DEFAULT 0.00,
    total_pago                  DECIMAL(10,2) NOT NULL DEFAULT 0.00,
    estado_asignacion           VARCHAR(50) NOT NULL DEFAULT 'activo',
    id_inscripcion_asignacion   INT NOT NULL UNIQUE,
    CONSTRAINT fk_asignacion_inscripcion FOREIGN KEY (id_inscripcion_asignacion) REFERENCES inscripcion (id_inscripcion)
) ENGINE=InnoDB;

-- detalle_asignacion: N:M entre asignacion y seccion, con atributos propios
-- (nota, resultado, costo). No es asociativa pura, conserva id propio.
CREATE TABLE detalle_asignacion (
    id_detalle_asignacion   INT AUTO_INCREMENT PRIMARY KEY,
    estado_detalle           VARCHAR(50) NOT NULL DEFAULT 'activo',
    costo_laboratorio        DECIMAL(10,2) NOT NULL DEFAULT 0.00,
    nota_final                DECIMAL(5,2) NULL,
    resultado                 VARCHAR(50) NULL,
    fecha_resultado            DATETIME NULL,
    id_asignacion_detalle      INT NOT NULL,
    id_seccion_detalle         INT NOT NULL,
    CONSTRAINT fk_detalle_asignacion FOREIGN KEY (id_asignacion_detalle) REFERENCES asignacion (id_asignacion),
    CONSTRAINT fk_detalle_seccion FOREIGN KEY (id_seccion_detalle) REFERENCES seccion (id_seccion),
    CONSTRAINT uq_asignacion_seccion UNIQUE (id_asignacion_detalle, id_seccion_detalle)
) ENGINE=InnoDB;

-- =====================================================================
-- 8. DATOS SEMILLA (SEED) — catálogo mínimo + 3 usuarios demo
--    (uno por rol) para poder iniciar sesión de inmediato en la app.
--
--    Las contraseñas están hasheadas con BCrypt (work factor 11), igual
--    que como las genera Asignacion.Services.Auth.AuthService /
--    Asignacion.Data.Seeding.DbSeeder. Contraseñas en texto plano:
--      admin@umg.edu.gt        -> Admin2025!
--      estudiante@umg.edu.gt   -> Est2025!
--      catedratico@umg.edu.gt  -> Cat2025!
-- =====================================================================

-- --- Roles y permisos ---------------------------------------------------
INSERT INTO rol (id_rol, nombre_rol, descripcion_rol, estado_rol) VALUES
    (1, 'Administrador', 'Acceso total al sistema', 'activo'),
    (2, 'Estudiante', 'Puede inscribirse y ver su información académica', 'activo'),
    (3, 'Catedratico', 'Gestiona sus secciones y calificaciones', 'activo');

INSERT INTO permiso (id_permiso, nombre_permiso, descripcion_permiso) VALUES
    (1, 'gestionar_usuarios', 'Crear, editar y desactivar usuarios del sistema'),
    (2, 'gestionar_catalogo', 'Administrar facultades, carreras, pensums y cursos'),
    (3, 'gestionar_matricula', 'Administrar periodos, secciones e inscripciones'),
    (4, 'generar_reportes', 'Acceso a los reportes administrativos');

INSERT INTO rol_permiso (id_rol, id_permiso) VALUES
    (1, 1), (1, 2), (1, 3), (1, 4);

-- --- Catálogo académico --------------------------------------------------
INSERT INTO facultad (id_facultad, codigo_facultad, nombre_facultad, estado_facultad) VALUES
    (1, 'ING', 'Facultad de Ingeniería', 'activo');

INSERT INTO carrera (id_carrera, codigo_carrera, nombre_carrera, total_ciclos_carrera, estado_carrera, id_facultad_carrera) VALUES
    (1, 'ISC', 'Ingeniería en Sistemas', 10, 'activo', 1);

INSERT INTO pensum (id_pensum, codigo_pensum, anio_pensum, jornada_pensum, estado_pensum, id_carrera_pensum) VALUES
    (1, 'PENSUM-2023', 2023, 'Fin de semana', 'activo', 1);

INSERT INTO curso (id_curso, codigo_curso, nombre_curso, creditos_curso, requiere_laboratorio_curso, estado_curso) VALUES
    (1, 'MAT101', 'Matemática Básica', 5, FALSE, 'activo'),
    (2, 'PRG101', 'Programación I', 5, TRUE, 'activo'),
    (3, 'PRG201', 'Programación II', 5, TRUE, 'activo'),
    (4, 'BD101', 'Bases de Datos I', 4, TRUE, 'activo'),
    (5, 'ADM101', 'Administración General', 3, FALSE, 'activo'),
    (6, 'ING101', 'Inglés Técnico I', 3, FALSE, 'activo');

INSERT INTO pensum_curso (id_pensum, id_curso, ciclo_pensum_curso, es_obligatorio_pensum_curso) VALUES
    (1, 1, 1, TRUE),
    (1, 2, 1, TRUE),
    (1, 6, 1, TRUE),
    (1, 5, 2, TRUE),
    (1, 3, 2, TRUE),
    (1, 4, 3, TRUE);

-- PRG201 requiere haber aprobado PRG101
-- (id_pensum_curso_requisito = 5: la 5ta fila insertada en pensum_curso arriba es (1, 3, ...))
INSERT INTO requisito_curso (tipo_requisito, creditos_minimos_requisito, descripcion_requisito, id_pensum_curso_requisito, id_curso_requisito) VALUES
    ('curso_aprobado', NULL, 'Debe haber aprobado Programación I', 5, 2);

-- --- Infraestructura -------------------------------------------------------
INSERT INTO edificio (id_edificio, codigo_edificio, nombre_edificio, sede_edificio, ubicacion_edificio, estado_edificio) VALUES
    (1, 'ED-A', 'Edificio A', 'Campus Central', 'Zona 1, Ciudad de Guatemala', 'activo');

INSERT INTO salon (id_salon, codigo_salon, nombre_salon, capacidad_salon, tipo_espacio_salon, nivel_salon, estado_salon, id_edificio_salon) VALUES
    (1, 'A-101', 'Aula 101', 40, 'aula', 1, 'activo', 1),
    (2, 'A-LAB1', 'Laboratorio de Cómputo 1', 25, 'laboratorio', 1, 'activo', 1);

INSERT INTO laboratorio (id_laboratorio, nombre_laboratorio, descripcion_laboratorio, estado_laboratorio, id_salon_laboratorio) VALUES
    (1, 'Laboratorio de Cómputo 1', 'Equipo con software de desarrollo y ofimática', 'activo', 2);

INSERT INTO periodo_academico (id_periodo_academico, codigo_periodo_academico, descripcion_periodo_academico, tipo_periodo_academico, fecha_inicio_periodo_academico, fecha_fin_periodo_academico, permite_inscripcion_periodo_academico, permite_asignacion_periodo_academico, estado_periodo_academico) VALUES
    (1, '2026-S2', 'Segundo Semestre 2026', 'semestral', '2026-07-01', '2026-11-30', TRUE, TRUE, 'activo');

-- --- Usuarios demo (uno por rol) -------------------------------------------
-- admin@umg.edu.gt / Admin2025!
-- estudiante@umg.edu.gt / Est2025!
-- catedratico@umg.edu.gt / Cat2025!
INSERT INTO usuario (id_usuario, nombre_usuario, correo_login_usuario, correo_recuperacion_usuario, contrasena_hash_usuario, tiene_pass_temporal_usuario, estado_usuario, id_rol_usuario) VALUES
    (1, 'Administrador Demo', 'admin@umg.edu.gt', NULL, '$2a$11$Y2Bd/bGjdUh4mA9yhQztbejzt.2w.kY13btj2WsYcU4.LReeNqh7e', FALSE, 'activo', 1),
    (2, 'Estudiante Demo', 'estudiante@umg.edu.gt', NULL, '$2a$11$6lXuMYN.MKGidGDBfBEwpuZqzoPURnTu9nYH5MEnRRtaX9QmRqf.6', FALSE, 'activo', 2),
    (3, 'Catedratico Demo', 'catedratico@umg.edu.gt', NULL, '$2a$11$EUwN1Ha7hK9MK8.a26bxzOrzIAlLckeemqrvPWHi8XixC.bSlCUBG', FALSE, 'activo', 3);

INSERT INTO estudiante (id_estudiante, carnet_estudiante, dpi_estudiante, nombres_estudiante, apellidos_estudiante, fecha_nacimiento_estudiante, direccion_estudiante, telefono_estudiante, ciclo_actual_estudiante, estado_estudiante, id_usuario_estudiante, id_pensum_estudiante) VALUES
    (1, '1234-23-1234', '1234567890123', 'Estudiante', 'Demo', '2000-01-15', 'Ciudad de Guatemala', '55551234', 1, 'activo', 2, 1);

INSERT INTO catedratico (id_catedratico, codigo_catedratico, dpi_catedratico, nombres_catedratico, apellidos_catedratico, telefono_catedratico, profesion_catedratico, estado_catedratico, id_usuario_catedratico) VALUES
    (1, 'CAT-001', '9876543210123', 'Catedratico', 'Demo', '55555678', 'Ingeniero en Sistemas', 'activo', 3);

-- --- Secciones ofrecidas en el periodo 2026-S2 -----------------------------
INSERT INTO seccion (id_seccion, codigo_seccion, jornada_seccion, cupo_maximo_seccion, estado_seccion, id_curso_seccion, id_periodo_academico_seccion, id_catedratico_seccion, id_salon_seccion) VALUES
    (1, 'MAT101-A', 'Fin de semana', 35, 'activo', 1, 1, 1, 1),
    (2, 'PRG101-A', 'Fin de semana', 30, 'activo', 2, 1, 1, 1),
    (3, 'BD101-A', 'Fin de semana', 25, 'activo', 4, 1, 1, 1);

INSERT INTO horario_seccion (dia_semana_horario, hora_inicio_horario, hora_fin_horario, tipo_sesion_horario, id_seccion_horario) VALUES
    ('Sabado', '07:00:00', '10:00:00', 'teoria', 1),
    ('Sabado', '10:00:00', '13:00:00', 'teoria', 2),
    ('Domingo', '07:00:00', '10:00:00', 'teoria', 3);

INSERT INTO seccion_laboratorio (dia_semana_seccion_laboratorio, hora_inicio_seccion_laboratorio, hora_fin_seccion_laboratorio, costo_extra_seccion_laboratorio, id_seccion, id_laboratorio) VALUES
    ('Sabado', '13:00:00', '15:00:00', 150.00, 2, 1),
    ('Domingo', '10:00:00', '12:00:00', 150.00, 3, 1);

-- inscripcion / asignacion / detalle_asignacion se dejan vacías a propósito:
-- inicia sesión como estudiante@umg.edu.gt y usa el asistente de inscripción
-- de la app para generarlas de punta a punta.
