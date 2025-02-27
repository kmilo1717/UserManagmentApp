CREATE DATABASE user_managment_db;
go
USE user_managment_db;
go
CREATE TABLE persona (
    id INT IDENTITY(1,1) PRIMARY KEY,
    documento_identidad VARCHAR(50) UNIQUE NOT NULL,
    nombres VARCHAR(100) NOT NULL,
    apellidos VARCHAR(100) NOT NULL,
    fecha_nacimiento DATE NOT NULL
);
go
CREATE TABLE telefono (
    id INT IDENTITY(1,1) PRIMARY KEY,
    persona_id INT NOT NULL REFERENCES persona(id) ON DELETE CASCADE,
    numero VARCHAR(20) NOT NULL,
    CONSTRAINT unique_telefono UNIQUE (persona_id, numero)
);
go
CREATE TABLE correo_electronico (
    id INT IDENTITY(1,1) PRIMARY KEY,
    persona_id INT NOT NULL REFERENCES persona(id) ON DELETE CASCADE,
    email VARCHAR(100) NOT NULL,
    CONSTRAINT unique_email UNIQUE (persona_id, email)
);
go
CREATE TABLE direccion (
    id INT IDENTITY(1,1) PRIMARY KEY,
    persona_id INT NOT NULL REFERENCES persona(id) ON DELETE CASCADE,
    direccion VARCHAR(255) NOT NULL,
    CONSTRAINT unique_direccion UNIQUE (persona_id, direccion)
);
