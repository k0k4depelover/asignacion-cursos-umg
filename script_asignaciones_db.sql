CREATE DATABASE asignacion_cursos;


USE asignacion_cursos;


-- =====================================================================
-- Sistema de Asignación de Cursos Universitarios
-- Script de creación de base de datos
-- Sigue el estándar del documento "Estandarización y normalización":
--   - Tablas: singular, minúsculas
--   - Columnas: snake_case
--   - INT AUTO_INCREMENT PRIMARY KEY para identificadores
--   - VARCHAR(255) para nombres/textos cortos
--   - TEXT para descripciones largas
--   - DATETIME/DATE/TIME para fechas y horas
--   - DECIMAL(10,2) para valores monetarios
-- =====================================================================

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
    id_edificio     INT AUTO_INCREMENT PRIMARY KEY,
    codigo_edificio VARCHAR(50) NOT NULL,
    nombre_edificio VARCHAR(255) NOT NULL,
    sede            VARCHAR(255) NOT NULL,
    ubicacion       VARCHAR(255) NULL,
    estado_edificio VARCHAR(50) NOT NULL DEFAULT 'activo'
) ENGINE=InnoDB;

CREATE TABLE periodo_academico (
    id_periodo          INT AUTO_INCREMENT PRIMARY KEY,
    codigo_periodo       VARCHAR(50) NOT NULL,
    descripcion_periodo  VARCHAR(255) NULL,
    tipo_periodo         VARCHAR(50) NOT NULL,
    fecha_inicio         DATE NOT NULL,
    fecha_fin            DATE NOT NULL,
    permite_inscripcion  BOOLEAN NOT NULL DEFAULT FALSE,
    permite_asignacion   BOOLEAN NOT NULL DEFAULT FALSE,
    estado_periodo       VARCHAR(50) NOT NULL DEFAULT 'activo'
) ENGINE=InnoDB;

CREATE TABLE curso (
    id_curso              INT AUTO_INCREMENT PRIMARY KEY,
    codigo_curso           VARCHAR(50) NOT NULL,
    nombre_curso           VARCHAR(255) NOT NULL,
    creditos_curso                INT NOT NULL,
    requiere_laboratorio    BOOLEAN NOT NULL DEFAULT FALSE,
    estado_curso            VARCHAR(50) NOT NULL DEFAULT 'activo'
) ENGINE=InnoDB;

-- =====================================================================
-- 2. TABLAS CON UNA SOLA DEPENDENCIA
-- =====================================================================

-- usuario depende de rol (1:N -> un rol tiene muchos usuarios)
CREATE TABLE usuario (
    id_usuario            INT AUTO_INCREMENT PRIMARY KEY,
    nombre_usuario         VARCHAR(255) NOT NULL,
    correo_login            VARCHAR(255) NOT NULL UNIQUE,
    correo_recuperacion     VARCHAR(255) NULL,
    contrasena_hash         VARCHAR(255) NOT NULL,
    tiene_pass_temporal     BOOLEAN NOT NULL DEFAULT FALSE,
    estado_usuario          VARCHAR(50) NOT NULL DEFAULT 'activo',
    fecha_registro_usuario          DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    id_rol                  INT NOT NULL,
    CONSTRAINT fk_usuario_rol FOREIGN KEY (id_rol) REFERENCES rol (id_rol)
) ENGINE=InnoDB;

-- carrera depende de facultad (1:N -> una facultad tiene muchas carreras)
CREATE TABLE carrera (
    id_carrera      INT AUTO_INCREMENT PRIMARY KEY,
    codigo_carrera   VARCHAR(50) NOT NULL,
    nombre_carrera   VARCHAR(255) NOT NULL,
    total_ciclos     INT NOT NULL,
    estado_carrera   VARCHAR(50) NOT NULL DEFAULT 'activo',
    id_facultad      INT NOT NULL,
    CONSTRAINT fk_carrera_facultad FOREIGN KEY (id_facultad) REFERENCES facultad (id_facultad)
) ENGINE=InnoDB;

-- salon depende de edificio (1:N -> un edificio tiene muchos salones)
CREATE TABLE salon (
    id_salon        INT AUTO_INCREMENT PRIMARY KEY,
    codigo_salon     VARCHAR(50) NOT NULL,
    nombre_salon     VARCHAR(255) NOT NULL,
    capacidad_salon        INT NOT NULL,
    tipo_espacio     VARCHAR(50) NOT NULL,
    nivel_salon            INT NOT NULL,
    estado_salon     VARCHAR(50) NOT NULL DEFAULT 'activo',
    id_edificio      INT NOT NULL,
    CONSTRAINT fk_salon_edificio FOREIGN KEY (id_edificio) REFERENCES edificio (id_edificio)
) ENGINE=InnoDB;

-- =====================================================================
-- 3. TABLAS CON DEPENDENCIAS DE SEGUNDO NIVEL
-- =====================================================================

-- pensum depende de carrera
CREATE TABLE pensum (
    id_pensum       INT AUTO_INCREMENT PRIMARY KEY,
    codigo_pensum    VARCHAR(50) NOT NULL,
    anio_pensum      INT NOT NULL,
    jornada_pensum          VARCHAR(50) NOT NULL,
    estado_pensum    VARCHAR(50) NOT NULL DEFAULT 'activo',
    id_carrera       INT NOT NULL,
    CONSTRAINT fk_pensum_carrera FOREIGN KEY (id_carrera) REFERENCES carrera (id_carrera)
) ENGINE=InnoDB;

-- laboratorio depende de salon
CREATE TABLE laboratorio (
    id_laboratorio          INT AUTO_INCREMENT PRIMARY KEY,
    nombre_laboratorio       VARCHAR(255) NOT NULL,
    descripcion_laboratorio  TEXT NULL,
    estado_laboratorio       VARCHAR(50) NOT NULL DEFAULT 'activo',
    id_salon                 INT NOT NULL,
    CONSTRAINT fk_laboratorio_salon FOREIGN KEY (id_salon) REFERENCES salon (id_salon)
) ENGINE=InnoDB;

-- estudiante depende de usuario y pensum
CREATE TABLE estudiante (
    id_estudiante        INT AUTO_INCREMENT PRIMARY KEY,
    carnet_estudiante     VARCHAR(50) NOT NULL UNIQUE,
    dpi_estudiante         VARCHAR(20) NOT NULL UNIQUE,
    nombres_estudiante     VARCHAR(255) NOT NULL,
    apellidos_estudiante   VARCHAR(255) NOT NULL,
    fecha_nacimiento       DATE NOT NULL,
    direccion_estudiante              VARCHAR(255) NULL,
    telefono_estudiante               VARCHAR(20) NULL,
    ciclo_actual           INT NOT NULL DEFAULT 1,
    estado_estudiante      VARCHAR(50) NOT NULL DEFAULT 'activo',
    id_usuario             INT NOT NULL,
    id_pensum              INT NOT NULL,
    CONSTRAINT fk_estudiante_usuario FOREIGN KEY (id_usuario) REFERENCES usuario (id_usuario),
    CONSTRAINT fk_estudiante_pensum FOREIGN KEY (id_pensum) REFERENCES pensum (id_pensum)
) ENGINE=InnoDB;

-- catedratico depende de usuario
CREATE TABLE catedratico (
    id_catedratico        INT AUTO_INCREMENT PRIMARY KEY,
    codigo_catedratico     VARCHAR(50) NOT NULL UNIQUE,
    dpi_catedratico         VARCHAR(20) NOT NULL UNIQUE,
    nombres_catedratico     VARCHAR(255) NOT NULL,
    apellidos_catedratico   VARCHAR(255) NOT NULL,
    telefono_catedratico                VARCHAR(20) NULL,
    profesion_catedratico               VARCHAR(255) NULL,
    estado_catedratico      VARCHAR(50) NOT NULL DEFAULT 'activo',
    id_usuario              INT NOT NULL,
    CONSTRAINT fk_catedratico_usuario FOREIGN KEY (id_usuario) REFERENCES usuario (id_usuario)
) ENGINE=InnoDB;

-- =====================================================================
-- 4. TABLAS ASOCIATIVAS (relaciones N:M)
-- =====================================================================

-- rol_permiso: N:M entre rol y permiso, con PK COMPUESTA (sin id propio)
CREATE TABLE rol_permiso (
    id_rol      INT NOT NULL,
    id_permiso  INT NOT NULL,
    PRIMARY KEY (id_rol, id_permiso),
    CONSTRAINT fk_rolpermiso_rol FOREIGN KEY (id_rol) REFERENCES rol (id_rol),
    CONSTRAINT fk_rolpermiso_permiso FOREIGN KEY (id_permiso) REFERENCES permiso (id_permiso)
) ENGINE=InnoDB;

-- pensum_curso: N:M entre pensum y curso, con datos propios (ciclo, es_obligatorio)
-- por eso lleva su propio id autoincremental en vez de PK compuesta
CREATE TABLE pensum_curso (
    id_pensum_curso   INT AUTO_INCREMENT PRIMARY KEY,
    id_pensum          INT NOT NULL,
    id_curso           INT NOT NULL,
    ciclo              INT NOT NULL,
    es_obligatorio     BOOLEAN NOT NULL DEFAULT TRUE,
    CONSTRAINT fk_pensumcurso_pensum FOREIGN KEY (id_pensum) REFERENCES pensum (id_pensum),
    CONSTRAINT fk_pensumcurso_curso FOREIGN KEY (id_curso) REFERENCES curso (id_curso),
    CONSTRAINT uq_pensum_curso UNIQUE (id_pensum, id_curso)
) ENGINE=InnoDB;

-- =====================================================================
-- 5. REQUISITOS DE CURSOS (auto-referencia a curso vía id_curso_requerido)
-- =====================================================================

CREATE TABLE requisito_curso (
    id_requisito            INT AUTO_INCREMENT PRIMARY KEY,
    id_pensum_curso          INT NOT NULL,
    tipo_requisito           VARCHAR(50) NOT NULL,
    id_curso_requerido       INT NOT NULL,
    creditos_minimos         INT NULL,
    descripcion_requisito    VARCHAR(255) NULL,
    CONSTRAINT fk_requisito_pensumcurso FOREIGN KEY (id_pensum_curso) REFERENCES pensum_curso (id_pensum_curso),
    CONSTRAINT fk_requisito_cursorequerido FOREIGN KEY (id_curso_requerido) REFERENCES curso (id_curso)
) ENGINE=InnoDB;

-- =====================================================================
-- 6. SECCIONES (el curso "ofrecido" en un periodo específico)
-- =====================================================================

CREATE TABLE seccion (
    id_seccion      INT AUTO_INCREMENT PRIMARY KEY,
    codigo_seccion   VARCHAR(50) NOT NULL,
    jornada          VARCHAR(50) NOT NULL,
    cupo_maximo      INT NOT NULL,
    estado_seccion   VARCHAR(50) NOT NULL DEFAULT 'activo',
    id_curso         INT NOT NULL,
    id_periodo       INT NOT NULL,
    id_catedratico   INT NOT NULL,
    id_salon         INT NOT NULL,
    CONSTRAINT fk_seccion_curso FOREIGN KEY (id_curso) REFERENCES curso (id_curso),
    CONSTRAINT fk_seccion_periodo FOREIGN KEY (id_periodo) REFERENCES periodo_academico (id_periodo),
    CONSTRAINT fk_seccion_catedratico FOREIGN KEY (id_catedratico) REFERENCES catedratico (id_catedratico),
    CONSTRAINT fk_seccion_salon FOREIGN KEY (id_salon) REFERENCES salon (id_salon)
) ENGINE=InnoDB;

-- horario_seccion depende de seccion (1:N -> una seccion tiene varios horarios)
CREATE TABLE horario_seccion (
    id_horario      INT AUTO_INCREMENT PRIMARY KEY,
    dia_semana       VARCHAR(20) NOT NULL,
    hora_inicio      TIME NOT NULL,
    hora_fin         TIME NOT NULL,
    tipo_sesion      VARCHAR(50) NOT NULL,
    id_seccion       INT NOT NULL,
    CONSTRAINT fk_horario_seccion FOREIGN KEY (id_seccion) REFERENCES seccion (id_seccion)
) ENGINE=InnoDB;

-- seccion_laboratorio: N:M entre seccion y laboratorio, con datos propios (horario, costo)
CREATE TABLE seccion_laboratorio (
    id_seccion_laboratorio  INT AUTO_INCREMENT PRIMARY KEY,
    dia_semana               VARCHAR(20) NOT NULL,
    hora_inicio               TIME NOT NULL,
    hora_fin                  TIME NOT NULL,
    costo_extra                DECIMAL(10,2) NOT NULL DEFAULT 0.00,
    id_seccion                 INT NOT NULL,
    id_laboratorio             INT NOT NULL,
    CONSTRAINT fk_secclab_seccion FOREIGN KEY (id_seccion) REFERENCES seccion (id_seccion),
    CONSTRAINT fk_secclab_laboratorio FOREIGN KEY (id_laboratorio) REFERENCES laboratorio (id_laboratorio)
) ENGINE=InnoDB;

-- =====================================================================
-- 7. INSCRIPCIÓN -> ASIGNACIÓN -> DETALLE_ASIGNACIÓN
--    (cadena 1:N encadenada: un estudiante se inscribe por periodo,
--     una inscripción genera una asignación,
--     una asignación tiene el detalle por cada sección tomada)
-- =====================================================================

CREATE TABLE inscripcion (
    id_inscripcion      INT AUTO_INCREMENT PRIMARY KEY,
    fecha_inscripcion    DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    costo_inscripcion    DECIMAL(10,2) NOT NULL DEFAULT 0.00,
    monto_mensual        DECIMAL(10,2) NOT NULL DEFAULT 0.00,
    ciclo_inscrito       INT NOT NULL,
    estado_solvencia     VARCHAR(50) NOT NULL DEFAULT 'solvente',
    estado_inscripcion   VARCHAR(50) NOT NULL DEFAULT 'activo',
    id_estudiante        INT NOT NULL,
    id_periodo           INT NOT NULL,
    CONSTRAINT fk_inscripcion_estudiante FOREIGN KEY (id_estudiante) REFERENCES estudiante (id_estudiante),
    CONSTRAINT fk_inscripcion_periodo FOREIGN KEY (id_periodo) REFERENCES periodo_academico (id_periodo)
) ENGINE=InnoDB;

-- asignacion depende de inscripcion, relación 1:1 (una inscripción genera una asignación)
CREATE TABLE asignacion (
    id_asignacion            INT AUTO_INCREMENT PRIMARY KEY,
    fecha_asignacion          DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    subtotal_laboratorios     DECIMAL(10,2) NOT NULL DEFAULT 0.00,
    total_pago                DECIMAL(10,2) NOT NULL DEFAULT 0.00,
    estado_asignacion         VARCHAR(50) NOT NULL DEFAULT 'activo',
    id_inscripcion            INT NOT NULL UNIQUE,
    CONSTRAINT fk_asignacion_inscripcion FOREIGN KEY (id_inscripcion) REFERENCES inscripcion (id_inscripcion)
) ENGINE=InnoDB;

-- detalle_asignacion: N:M entre asignacion y seccion (cada fila = una sección tomada
-- dentro de una asignación), con datos propios (nota, resultado, costo)
CREATE TABLE detalle_asignacion (
    id_detalle_asignacion   INT AUTO_INCREMENT PRIMARY KEY,
    estado_detalle           VARCHAR(50) NOT NULL DEFAULT 'activo',
    costo_laboratorio        DECIMAL(10,2) NOT NULL DEFAULT 0.00,
    nota_final                DECIMAL(5,2) NULL,
    resultado                 VARCHAR(50) NULL,
    fecha_resultado            DATETIME NULL,
    id_asignacion              INT NOT NULL,
    id_seccion                 INT NOT NULL,
    CONSTRAINT fk_detalle_asignacion FOREIGN KEY (id_asignacion) REFERENCES asignacion (id_asignacion),
    CONSTRAINT fk_detalle_seccion FOREIGN KEY (id_seccion) REFERENCES seccion (id_seccion),
    CONSTRAINT uq_asignacion_seccion UNIQUE (id_asignacion, id_seccion)
) ENGINE=InnoDB;

-- =====================================================================
-- 1. INSERTAR ROLES (si no existen)
-- =====================================================================
INSERT INTO rol (id_rol, nombre_rol, estado_rol) 
VALUES 
  (1, 'Administrador', 'activo'),
  (2, 'Estudiante', 'activo'),
  (3, 'Catedratico', 'activo')
ON DUPLICATE KEY UPDATE 
  nombre_rol = VALUES(nombre_rol),
  estado_rol = VALUES(estado_rol);

-- =====================================================================
-- 2. INSERTAR USUARIOS DE PRUEBA CON CONTRASEÑAS DIFERENTES
--    Hashes generados con BCrypt para cada contraseña
-- =====================================================================
INSERT INTO usuario (
  nombre_usuario,
  correo_login,
  correo_recuperacion,
  contrasena_hash,
  tiene_pass_temporal,
  estado_usuario,
  fecha_registro_usuario,
  id_rol
) VALUES 
(
  'Administrador',
  'admin@umg.edu.gt',
  'admin@correo.com',
  '$2a$11$jlfmGso3rBYDZf7wtZRO3.uFkcQ2jmSvmXU4skk/.HXVwg9EFZhUO', -- Admin2025!
  false,
  'activo',
  NOW(),
  1
),
(
  'Estudiante',
  'estudiante@umg.edu.gt',
  'estudiante@correo.com',
  '$2a$11$KTEi7kst4Lgzi.Fj901GGOZNI9qZPjYtC1DTs7FddEdixYudBuyYq', -- Est2025!
  false,
  'activo',
  NOW(),
  2
),
(
  'Catedratico',
  'catedratico@umg.edu.gt',
  'catedratico@correo.com',
  '$2a$11$XJIwhKqMHs6sRmj7bUUAvOl8l3Kau6LksZGQGwM3Jv9alH0P2XP9W', -- Cat2025!
  false,
  'activo',
  NOW(),
  3
);

-- =====================================================================
-- 3. DATOS DE CATÁLOGO / INFRAESTRUCTURA / PROGRAMACIÓN DE DEMOSTRACIÓN
--    (necesarios para que los 3 usuarios de prueba tengan algo que ver
--     al iniciar sesión: un pensum con cursos, un período abierto,
--     una sección asignada al catedrático demo, y el perfil de
--     estudiante/catedrático vinculado a sus respectivos usuarios)
-- =====================================================================

INSERT INTO facultad (id_facultad, codigo_facultad, nombre_facultad, estado_facultad) VALUES
  (1, 'FISICC', 'Facultad de Ingeniería en Sistemas, Informática y Ciencias de la Computación', 'activo')
ON DUPLICATE KEY UPDATE nombre_facultad = VALUES(nombre_facultad);

INSERT INTO carrera (id_carrera, codigo_carrera, nombre_carrera, total_ciclos, estado_carrera, id_facultad) VALUES
  (1, 'ISIS', 'Ingeniería en Sistemas', 10, 'activo', 1)
ON DUPLICATE KEY UPDATE nombre_carrera = VALUES(nombre_carrera);

INSERT INTO pensum (id_pensum, codigo_pensum, anio_pensum, jornada_pensum, estado_pensum, id_carrera) VALUES
  (1, 'PENSUM2020', 2020, 'Nocturna', 'activo', 1)
ON DUPLICATE KEY UPDATE codigo_pensum = VALUES(codigo_pensum);

INSERT INTO curso (id_curso, codigo_curso, nombre_curso, creditos_curso, requiere_laboratorio, estado_curso) VALUES
  (1, 'MAT101', 'Matemática Básica 1', 4, FALSE, 'activo'),
  (2, 'PROG101', 'Introducción a la Programación', 5, TRUE, 'activo'),
  (3, 'PROG102', 'Programación Orientada a Objetos', 5, TRUE, 'activo'),
  (4, 'BD101', 'Bases de Datos 1', 4, TRUE, 'activo'),
  (5, 'RED101', 'Redes de Computadoras', 4, FALSE, 'activo'),
  (6, 'ING101', 'Inglés Técnico 1', 3, FALSE, 'activo')
ON DUPLICATE KEY UPDATE nombre_curso = VALUES(nombre_curso);

INSERT INTO pensum_curso (id_pensum_curso, id_pensum, id_curso, ciclo, es_obligatorio) VALUES
  (1, 1, 1, 1, TRUE),  -- MAT101 ciclo 1
  (2, 1, 2, 1, TRUE),  -- PROG101 ciclo 1
  (3, 1, 6, 1, TRUE),  -- ING101 ciclo 1
  (4, 1, 3, 2, TRUE),  -- PROG102 ciclo 2 (requiere PROG101)
  (5, 1, 4, 2, TRUE),  -- BD101 ciclo 2 (requiere PROG101)
  (6, 1, 5, 3, TRUE)   -- RED101 ciclo 3
ON DUPLICATE KEY UPDATE ciclo = VALUES(ciclo);

INSERT INTO requisito_curso (id_requisito, id_pensum_curso, tipo_requisito, id_curso_requerido, creditos_minimos, descripcion_requisito) VALUES
  (1, 4, 'curso_aprobado', 2, NULL, 'Debe haber aprobado Introducción a la Programación'),
  (2, 5, 'curso_aprobado', 2, NULL, 'Debe haber aprobado Introducción a la Programación')
ON DUPLICATE KEY UPDATE descripcion_requisito = VALUES(descripcion_requisito);

INSERT INTO edificio (id_edificio, codigo_edificio, nombre_edificio, sede, ubicacion, estado_edificio) VALUES
  (1, 'ED-A', 'Edificio A', 'Central', '1a avenida, zona 1', 'activo')
ON DUPLICATE KEY UPDATE nombre_edificio = VALUES(nombre_edificio);

INSERT INTO salon (id_salon, codigo_salon, nombre_salon, capacidad_salon, tipo_espacio, nivel_salon, estado_salon, id_edificio) VALUES
  (1, 'A-101', 'Aula 101', 40, 'aula', 1, 'activo', 1),
  (2, 'A-LAB1', 'Laboratorio de Cómputo 1', 25, 'laboratorio', 1, 'activo', 1)
ON DUPLICATE KEY UPDATE nombre_salon = VALUES(nombre_salon);

INSERT INTO laboratorio (id_laboratorio, nombre_laboratorio, descripcion_laboratorio, estado_laboratorio, id_salon) VALUES
  (1, 'Laboratorio de Cómputo 1', 'Laboratorio con 25 estaciones de trabajo', 'activo', 2)
ON DUPLICATE KEY UPDATE nombre_laboratorio = VALUES(nombre_laboratorio);

INSERT INTO periodo_academico (id_periodo, codigo_periodo, descripcion_periodo, tipo_periodo, fecha_inicio, fecha_fin, permite_inscripcion, permite_asignacion, estado_periodo) VALUES
  (1, '2026-2', 'Segundo Semestre 2026', 'Semestre', '2026-07-01', '2026-11-30', TRUE, TRUE, 'activo')
ON DUPLICATE KEY UPDATE descripcion_periodo = VALUES(descripcion_periodo);

INSERT INTO estudiante (id_estudiante, carnet_estudiante, dpi_estudiante, nombres_estudiante, apellidos_estudiante, fecha_nacimiento, direccion_estudiante, telefono_estudiante, ciclo_actual, estado_estudiante, id_usuario, id_pensum) VALUES
  (1, '20230012519', '1234567890101', 'Estudiante', 'Demo', '2003-05-14', 'Ciudad de Guatemala', '55501234', 2, 'activo', 2, 1)
ON DUPLICATE KEY UPDATE nombres_estudiante = VALUES(nombres_estudiante);

INSERT INTO catedratico (id_catedratico, codigo_catedratico, dpi_catedratico, nombres_catedratico, apellidos_catedratico, telefono_catedratico, profesion_catedratico, estado_catedratico, id_usuario) VALUES
  (1, 'CAT-001', '9876543210101', 'Catedrático', 'Demo', '55509876', 'Ingeniero en Sistemas', 'activo', 3)
ON DUPLICATE KEY UPDATE nombres_catedratico = VALUES(nombres_catedratico);

INSERT INTO seccion (id_seccion, codigo_seccion, jornada, cupo_maximo, estado_seccion, id_curso, id_periodo, id_catedratico, id_salon) VALUES
  (1, 'PROG101-A', 'Nocturna', 30, 'activo', 2, 1, 1, 1),
  (2, 'BD101-A', 'Nocturna', 25, 'activo', 4, 1, 1, 1)
ON DUPLICATE KEY UPDATE codigo_seccion = VALUES(codigo_seccion);

INSERT INTO horario_seccion (id_horario, dia_semana, hora_inicio, hora_fin, tipo_sesion, id_seccion) VALUES
  (1, 'Lunes', '18:00:00', '20:00:00', 'teoria', 1),
  (2, 'Miercoles', '18:00:00', '20:00:00', 'teoria', 1),
  (3, 'Martes', '18:00:00', '20:00:00', 'teoria', 2),
  (4, 'Jueves', '18:00:00', '20:00:00', 'teoria', 2)
ON DUPLICATE KEY UPDATE dia_semana = VALUES(dia_semana);

INSERT INTO seccion_laboratorio (id_seccion_laboratorio, dia_semana, hora_inicio, hora_fin, costo_extra, id_seccion, id_laboratorio) VALUES
  (1, 'Viernes', '18:00:00', '20:00:00', 150.00, 1, 1),
  (2, 'Viernes', '20:00:00', '22:00:00', 150.00, 2, 1)
ON DUPLICATE KEY UPDATE dia_semana = VALUES(dia_semana);