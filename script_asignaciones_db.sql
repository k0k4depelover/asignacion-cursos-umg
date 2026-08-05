-- CREATE DATABASE asignacion_cursos;

-- USE asignacion_cursos;

-- =====================================================================
-- Sistema de Asignación de Cursos Universitarios
-- Script de creación de base de datos (CORREGIDO)
-- Sigue el estándar del documento "Estandarización y normalización":
--   - Tablas: singular, minúsculas
--   - Columnas: snake_case, con el nombre de la tabla actual al final
--     (ej. id_edificio_salon = FK a edificio, dentro de la tabla salon)
--   - Tablas asociativas puras (N:M sin atributos propios): SIN id propio,
--     PK compuesta = las 2 columnas FK (2 FK = 2 PK). Ej: rol_permiso,
--     pensum_curso, seccion_laboratorio. En ese caso las columnas se dejan
--     con el nombre "desnudo" (id_rol, id_pensum, id_seccion, etc.) para
--     evitar ambigüedad, ya que el sufijo sería el nombre de la propia
--     tabla asociativa.
--   - Tablas que relacionan 2 entidades pero SÍ tienen atributos propios
--     relevantes (ej. requisito_curso, detalle_asignacion) sí conservan
--     un id propio autoincremental, porque no son una simple tabla puente.
--   - INT AUTO_INCREMENT PRIMARY KEY para identificadores de entidades reales
--   - VARCHAR(255) para nombres/textos cortos
--   - TEXT para descripciones largas
--   - DATETIME/DATE/TIME para fechas y horas
--   - DECIMAL(10,2) para valores monetarios
--
-- CAMBIOS respecto a la versión original:
--   1. periodo_academico: se corrigieron los FK de seccion e inscripcion,
--      que apuntaban a una columna inexistente "id_periodo" (la PK real
--      es id_periodo_academico).
--   2. horario_seccion: se corrigió el typo "id_seccion_hoarario" ->
--      "id_seccion_horario" (el FK ya usaba el nombre correcto).
--   3. seccion_laboratorio: las columnas declaradas no coincidían con las
--      columnas usadas en los FOREIGN KEY. Se renombraron a id_seccion /
--      id_laboratorio (tabla asociativa pura, 2 FK = 2 PK, sin id propio).
--   4. pensum_curso: se eliminó el UNIQUE redundante que además referenciaba
--      columnas inexistentes (id_pensum/id_curso en vez de id_pensum_curso/
--      id_curso_pensum). Se renombraron las columnas a id_pensum / id_curso
--      (tabla asociativa pura). Se corrigió el comentario que decía
--      incorrectamente que la tabla llevaba id propio autoincremental.
--   5. requisito_curso: el FK a pensum_curso apuntaba a una sola columna de
--      una PK compuesta (inválido). Se cambió a un FK compuesto de 2
--      columnas hacia (id_pensum, id_curso) de pensum_curso.
--   6. Se estandarizaron sufijos faltantes: inscripcion.id_estudiante ->
--      id_estudiante_inscripcion, inscripcion.id_periodo ->
--      id_periodo_academico_inscripcion, asignacion.id_inscripcion ->
--      id_inscripcion_asignacion, detalle_asignacion.id_asignacion/id_seccion
--      -> id_asignacion_detalle/id_seccion_detalle.
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
    id_pensum                     INT NOT NULL,
    id_curso                      INT NOT NULL,
    ciclo_pensum_curso            INT NOT NULL,
    es_obligatorio_pensum_curso   BOOLEAN NOT NULL DEFAULT TRUE,
    PRIMARY KEY (id_pensum, id_curso),
    CONSTRAINT fk_pensumcurso_pensum FOREIGN KEY (id_pensum) REFERENCES pensum (id_pensum),
    CONSTRAINT fk_pensumcurso_curso FOREIGN KEY (id_curso) REFERENCES curso (id_curso)
) ENGINE=InnoDB;

-- =====================================================================
-- 5. REQUISITOS DE CURSOS
--    No es una tabla asociativa pura: tiene atributos propios (tipo,
--    créditos mínimos, descripción), por lo que conserva un id propio
--    autoincremental. Se relaciona con pensum_curso mediante un FK
--    compuesto (2 columnas), ya que la PK de pensum_curso es compuesta.
-- =====================================================================

CREATE TABLE requisito_curso (
    id_requisito                  INT AUTO_INCREMENT PRIMARY KEY,
    tipo_requisito                VARCHAR(50) NOT NULL,
    creditos_minimos_requisito    INT NULL,
    descripcion_requisito         VARCHAR(255) NULL,
    id_pensum_requisito           INT NOT NULL,
    id_curso_pensum_requisito     INT NOT NULL,
    id_curso_requisito            INT NOT NULL,
    CONSTRAINT fk_requisito_pensumcurso FOREIGN KEY (id_pensum_requisito, id_curso_pensum_requisito)
        REFERENCES pensum_curso (id_pensum, id_curso),
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
    dia_semana_seccion_laboratorio   VARCHAR(20) NOT NULL,
    hora_inicio_seccion_laboratorio  TIME NOT NULL,
    hora_fin_seccion_laboratorio     TIME NOT NULL,
    costo_extra_seccion_laboratorio  DECIMAL(10,2) NOT NULL DEFAULT 0.00,
    id_seccion                       INT NOT NULL,
    id_laboratorio                   INT NOT NULL,
    PRIMARY KEY (id_seccion, id_laboratorio),
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

-- detalle_asignacion: N:M entre asignacion y seccion (cada fila = una
-- sección tomada dentro de una asignación), con atributos propios
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