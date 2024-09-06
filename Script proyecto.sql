create database proyectoASP;
use proyectoASP;

-- Crear tabla de Usuarios
CREATE TABLE Usuarios (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Correo VARCHAR(100) NOT NULL UNIQUE,
    Clave VARCHAR(100) NOT NULL
);

-- Crear tabla de Roles
CREATE TABLE Roles (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL UNIQUE
);

-- Crear tabla de relación entre Usuario y Rol
CREATE TABLE UsuarioRoles (
    UsuarioId INT,
    RolId INT,
    PRIMARY KEY (UsuarioId, RolId),
    FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id) ON DELETE CASCADE,
    FOREIGN KEY (RolId) REFERENCES Roles(Id) ON DELETE CASCADE
);


use proyectoasp;
-- Crear tabla de Propietarios
CREATE TABLE Propietarios (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    UsuarioId INT NOT NULL,
    FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id) ON DELETE CASCADE
);

-- Crear tabla de Propiedades
CREATE TABLE Propiedades (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(255) NOT NULL,
    Direccion VARCHAR(255) NOT NULL,
    PropietarioId INT NOT NULL,
    FOREIGN KEY (PropietarioId) REFERENCES Propietarios(Id) ON DELETE CASCADE
);

-- Crear tabla de Contratos de Alquiler
CREATE TABLE ContratosAlquiler (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    PropiedadId INT NOT NULL,
    InquilinoId INT NOT NULL,
    FechaInicio DATE NOT NULL,
    FechaFin DATE NOT NULL,
    Monto DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (PropiedadId) REFERENCES Propiedades(Id) ON DELETE CASCADE,
    FOREIGN KEY (InquilinoId) REFERENCES Usuarios(Id) ON DELETE CASCADE
);

-- Crear tabla de Pagos
CREATE TABLE Pagos (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    ContratoAlquilerId INT NOT NULL,
    FechaPago DATE NOT NULL,
    Monto DECIMAL(10, 2) NOT NULL,
    Pagado BOOLEAN NOT NULL DEFAULT FALSE,
    FOREIGN KEY (ContratoAlquilerId) REFERENCES ContratosAlquiler(Id) ON DELETE CASCADE
);


use proyectoasp;
DESCRIBE propietarios;
DESCRIBE propiedades;
DESCRIBE usuarios;
INSERT INTO Propiedades (Nombre, Direccion, PropietarioId) VALUES ('Propiedad de Prueba', 'Dirección de Prueba', 1);

CREATE TABLE FincasTerrenos (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(255) NOT NULL,
    Direccion VARCHAR(255) NOT NULL,
    Area DECIMAL(10, 2) NOT NULL, -- Campo para el tamaño del terreno o finca
    Tipo ENUM('Finca', 'Terreno') NOT NULL, -- Especificar si es finca o terreno
    PropietarioId INT NOT NULL,
    FOREIGN KEY (PropietarioId) REFERENCES Propietarios(Id) ON DELETE CASCADE
);
select * from usuarios;
select * from Propietarios;
select * from usuarioroles;
select * from roles




