create database asignacion_cursos;


use asignacion_cursos;


-- =====================================================================
-- sistema de asignación de cursos universitarios
-- script de creación de base de datos
-- sigue el estándar del documento "estandarización_y_normalización":
--   - tablas: singular, minúsculas
--   - columnas: snake_case
--   - int auto_increment primary key para identificadores
--   - varchar(255) para nombres/textos cortos
--   - text para descripciones largas
--   - datetime/date/time para fechas y horas
--   - decimal(10,2) para valores monetarios
-- =====================================================================

-- =====================================================================
-- 1. tablas sin dependencias (no tienen fk hacia otras tablas)
-- =====================================================================

create table rol (
    id_rol          int auto_increment primary key,
    nombre_rol      varchar(255) not null,
    descripcion_rol text null,
    estado_rol      varchar(50) not null default 'activo'
) engine=innodb;

create table permiso (
    id_permiso          int auto_increment primary key,
    nombre_permiso      varchar(255) not null,
    descripcion_permiso text null
) engine=innodb;

create table facultad (
    id_facultad     int auto_increment primary key,
    codigo_facultad varchar(50) not null,
    nombre_facultad varchar(255) not null,
    estado_facultad varchar(50) not null default 'activo'
) engine=innodb;

create table edificio (
    id_edificio     int auto_increment primary key,
    codigo_edificio varchar(50) not null,
    nombre_edificio varchar(255) not null,
    sede            varchar(255) not null,
    ubicacion       varchar(255) null,
    estado_edificio varchar(50) not null default 'activo'
) engine=innodb;

create table periodo_academico (
    id_periodo          int auto_increment primary key,
    codigo_periodo       varchar(50) not null,
    descripcion_periodo  varchar(255) null,
    tipo_periodo         varchar(50) not null,
    fecha_inicio         date not null,
    fecha_fin            date not null,
    permite_inscripcion  boolean not null default false,
    permite_asignacion   boolean not null default false,
    estado_periodo       varchar(50) not null default 'activo'
) engine=innodb;

create table curso (
    id_curso              int auto_increment primary key,
    codigo_curso           varchar(50) not null,
    nombre_curso           varchar(255) not null,
    creditos_curso                int not null,
    requiere_laboratorio    boolean not null default false,
    estado_curso            varchar(50) not null default 'activo'
) engine=innodb;

-- =====================================================================
-- 2. tablas con una sola dependencia
-- =====================================================================

-- usuario depende de rol (1:n -> un rol tiene muchos usuarios)
create table usuario (
    id_usuario            int auto_increment primary key,
    nombre_usuario         varchar(255) not null,
    correo_login            varchar(255) not null unique,
    correo_recuperacion     varchar(255) null,
    contrasena_hash         varchar(255) not null,
    tiene_pass_temporal     boolean not null default false,
    estado_usuario          varchar(50) not null default 'activo',
    fecha_registro_usuario          datetime not null default current_timestamp,
    id_rol                  int not null,
    constraint fk_usuario_rol foreign key (id_rol) references rol (id_rol)
) engine=innodb;

-- carrera depende de facultad (1:n -> una facultad tiene muchas carreras)
create table carrera (
    id_carrera      int auto_increment primary key,
    codigo_carrera   varchar(50) not null,
    nombre_carrera   varchar(255) not null,
    total_ciclos     int not null,
    estado_carrera   varchar(50) not null default 'activo',
    id_facultad      int not null,
    constraint fk_carrera_facultad foreign key (id_facultad) references facultad (id_facultad)
) engine=innodb;

-- salon depende de edificio (1:n -> un edificio tiene muchos salones)
create table salon (
    id_salon        int auto_increment primary key,
    codigo_salon     varchar(50) not null,
    nombre_salon     varchar(255) not null,
    capacidad_salon        int not null,
    tipo_espacio     varchar(50) not null,
    nivel_salon            int not null,
    estado_salon     varchar(50) not null default 'activo',
    id_edificio      int not null,
    constraint fk_salon_edificio foreign key (id_edificio) references edificio (id_edificio)
) engine=innodb;

-- =====================================================================
-- 3. tablas con dependencias de segundo nivel
-- =====================================================================

-- pensum depende de carrera
create table pensum (
    id_pensum       int auto_increment primary key,
    codigo_pensum    varchar(50) not null,
    anio_pensum      int not null,
    jornada_pensum          varchar(50) not null,
    estado_pensum    varchar(50) not null default 'activo',
    id_carrera       int not null,
    constraint fk_pensum_carrera foreign key (id_carrera) references carrera (id_carrera)
) engine=innodb;

-- laboratorio depende de salon
create table laboratorio (
    id_laboratorio          int auto_increment primary key,
    nombre_laboratorio       varchar(255) not null,
    descripcion_laboratorio  text null,
    estado_laboratorio       varchar(50) not null default 'activo',
    id_salon                 int not null,
    constraint fk_laboratorio_salon foreign key (id_salon) references salon (id_salon)
) engine=innodb;

-- estudiante depende de usuario y pensum
create table estudiante (
    id_estudiante        int auto_increment primary key,
    carnet_estudiante     varchar(50) not null unique,
    dpi_estudiante         varchar(20) not null unique,
    nombres_estudiante     varchar(255) not null,
    apellidos_estudiante   varchar(255) not null,
    fecha_nacimiento       date not null,
    direccion_estudiante              varchar(255) null,
    telefono_estudiante               varchar(20) null,
    ciclo_actual           int not null default 1,
    estado_estudiante      varchar(50) not null default 'activo',
    id_usuario             int not null,
    id_pensum              int not null,
    constraint fk_estudiante_usuario foreign key (id_usuario) references usuario (id_usuario),
    constraint fk_estudiante_pensum foreign key (id_pensum) references pensum (id_pensum)
) engine=innodb;

-- catedratico depende de usuario
create table catedratico (
    id_catedratico        int auto_increment primary key,
    codigo_catedratico     varchar(50) not null unique,
    dpi_catedratico         varchar(20) not null unique,
    nombres_catedratico     varchar(255) not null,
    apellidos_catedratico   varchar(255) not null,
    telefono_catedratico                varchar(20) null,
    profesion_catedratico               varchar(255) null,
    estado_catedratico      varchar(50) not null default 'activo',
    id_usuario              int not null,
    constraint fk_catedratico_usuario foreign key (id_usuario) references usuario (id_usuario)
) engine=innodb;

-- =====================================================================
-- 4. tablas asociativas (relaciones n:m)
-- =====================================================================

-- rol_permiso: n:m entre rol y permiso, con pk compuesta (sin id propio)
create table rol_permiso (
    id_rol      int not null,
    id_permiso  int not null,
    primary key (id_rol, id_permiso),
    constraint fk_rolpermiso_rol foreign key (id_rol) references rol (id_rol),
    constraint fk_rolpermiso_permiso foreign key (id_permiso) references permiso (id_permiso)
) engine=innodb;

-- pensum_curso: n:m entre pensum y curso, con datos propios (ciclo, es_obligatorio)
-- por eso lleva su propio id autoincremental en vez de pk compuesta
create table pensum_curso (
    id_pensum_curso   int auto_increment primary key,
    id_pensum          int not null,
    id_curso           int not null,
    ciclo              int not null,
    es_obligatorio     boolean not null default true,
    constraint fk_pensumcurso_pensum foreign key (id_pensum) references pensum (id_pensum),
    constraint fk_pensumcurso_curso foreign key (id_curso) references curso (id_curso),
    constraint uq_pensum_curso unique (id_pensum, id_curso)
) engine=innodb;

-- =====================================================================
-- 5. requisitos de cursos (auto-referencia a curso vía id_curso_requerido)
-- =====================================================================

create table requisito_curso (
    id_requisito            int auto_increment primary key,
    id_pensum_curso          int not null,
    tipo_requisito           varchar(50) not null,
    id_curso_requerido       int not null,
    creditos_minimos         int null,
    descripcion_requisito    varchar(255) null,
    constraint fk_requisito_pensumcurso foreign key (id_pensum_curso) references pensum_curso (id_pensum_curso),
    constraint fk_requisito_cursorequerido foreign key (id_curso_requerido) references curso (id_curso)
) engine=innodb;

-- =====================================================================
-- 6. secciones (el curso "ofrecido" en un periodo específico)
-- =====================================================================

create table seccion (
    id_seccion      int auto_increment primary key,
    codigo_seccion   varchar(50) not null,
    jornada          varchar(50) not null,
    cupo_maximo      int not null,
    estado_seccion   varchar(50) not null default 'activo',
    id_curso         int not null,
    id_periodo       int not null,
    id_catedratico   int not null,
    id_salon         int not null,
    constraint fk_seccion_curso foreign key (id_curso) references curso (id_curso),
    constraint fk_seccion_periodo foreign key (id_periodo) references periodo_academico (id_periodo),
    constraint fk_seccion_catedratico foreign key (id_catedratico) references catedratico (id_catedratico),
    constraint fk_seccion_salon foreign key (id_salon) references salon (id_salon)
) engine=innodb;

-- horario_seccion depende de seccion (1:n -> una seccion tiene varios horarios)
create table horario_seccion (
    id_horario      int auto_increment primary key,
    dia_semana       varchar(20) not null,
    hora_inicio      time not null,
    hora_fin         time not null,
    tipo_sesion      varchar(50) not null,
    id_seccion       int not null,
    constraint fk_horario_seccion foreign key (id_seccion) references seccion (id_seccion)
) engine=innodb;

-- seccion_laboratorio: n:m entre seccion y laboratorio, con datos propios (horario, costo)
create table seccion_laboratorio (
    id_seccion_laboratorio  int auto_increment primary key,
    dia_semana               varchar(20) not null,
    hora_inicio               time not null,
    hora_fin                  time not null,
    costo_extra                decimal(10,2) not null default 0.00,
    id_seccion                 int not null,
    id_laboratorio             int not null,
    constraint fk_secclab_seccion foreign key (id_seccion) references seccion (id_seccion),
    constraint fk_secclab_laboratorio foreign key (id_laboratorio) references laboratorio (id_laboratorio)
) engine=innodb;

-- =====================================================================
-- 7. inscripción -> asignación -> detalle_asignación
--    (cadena 1:n encadenada: un estudiante se inscribe por periodo,
--     una inscripción genera una asignación,
--     una asignación tiene el detalle por cada sección tomada)
-- =====================================================================

create table inscripcion (
    id_inscripcion      int auto_increment primary key,
    fecha_inscripcion    datetime not null default current_timestamp,
    costo_inscripcion    decimal(10,2) not null default 0.00,
    monto_mensual        decimal(10,2) not null default 0.00,
    ciclo_inscrito       int not null,
    estado_solvencia     varchar(50) not null default 'solvente',
    estado_inscripcion   varchar(50) not null default 'activo',
    id_estudiante        int not null,
    id_periodo           int not null,
    constraint fk_inscripcion_estudiante foreign key (id_estudiante) references estudiante (id_estudiante),
    constraint fk_inscripcion_periodo foreign key (id_periodo) references periodo_academico (id_periodo)
) engine=innodb;

-- asignacion depende de inscripcion, relación 1:1 (una inscripción genera una asignación)
create table asignacion (
    id_asignacion            int auto_increment primary key,
    fecha_asignacion          datetime not null default current_timestamp,
    subtotal_laboratorios     decimal(10,2) not null default 0.00,
    total_pago                decimal(10,2) not null default 0.00,
    estado_asignacion         varchar(50) not null default 'activo',
    id_inscripcion            int not null unique,
    constraint fk_asignacion_inscripcion foreign key (id_inscripcion) references inscripcion (id_inscripcion)
) engine=innodb;

-- detalle_asignacion: n:m entre asignacion y seccion (cada fila = una sección tomada
-- dentro de una asignación), con datos propios (nota, resultado, costo)
create table detalle_asignacion (
    id_detalle_asignacion   int auto_increment primary key,
    estado_detalle           varchar(50) not null default 'activo',
    costo_laboratorio        decimal(10,2) not null default 0.00,
    nota_final                decimal(5,2) null,
    resultado                 varchar(50) null,
    fecha_resultado            datetime null,
    id_asignacion              int not null,
    id_seccion                 int not null,
    constraint fk_detalle_asignacion foreign key (id_asignacion) references asignacion (id_asignacion),
    constraint fk_detalle_seccion foreign key (id_seccion) references seccion (id_seccion),
    constraint uq_asignacion_seccion unique (id_asignacion, id_seccion)
) engine=innodb;

-- registro: asignación de catedráticos a secciones
create table registro (
    id_registro         int auto_increment primary key,
    fecha_registro      datetime not null default current_timestamp,
    estado_registro     varchar(50) not null default 'activo',
    id_catedratico      int not null,
    id_seccion          int not null,
    constraint fk_registro_catedratico foreign key (id_catedratico) references catedratico (id_catedratico),
    constraint fk_registro_seccion foreign key (id_seccion) references seccion (id_seccion),
    constraint uq_registro_catedratico_seccion unique (id_catedratico, id_seccion)
) engine=innodb;
