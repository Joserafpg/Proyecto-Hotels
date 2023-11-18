create database Hotel

use Hotel

create table Usuarios(
Id_Usuario int IDENTITY (1,1) NOT NULL,
Empleado varchar (50),
Usuario varchar (10),
Contraseña varchar(30),

Recepcionista bit,
Tecnico bit,
Administrador bit,
Gerencia bit,
Contable bit,


Clientes bit,
Habitaciones bit,
Reservaciones bit,
Facturacion bit,
Configuracion bit,
Agregar bit,
Editar bit,
Buscar bit,
Eliminar bit,
)

create table Acceso(
Usuario varchar (10),
Fecha datetime,
)

create table Habitaciones (
ID_Habitacion INT IDENTITY (1,1) NOT NULL,
Numero_habitacion varchar (30) PRIMARY KEY,
Tipo_habitacion varchar (30),
Tarifa_noche decimal (38),
Capacida_maxima int,
Camas int,
Servicio_habitacion bit,
Estado varchar (20)
)

create table Detalle_Reservas (
Id_Reserva INT IDENTITY (1,1) PRIMARY KEY NOT NULL,
Habitacion varchar (30),
Id_Huesped int,
Huespede varchar (50),
Fecha_entrada datetime,
Fecha_salida datetime,
Empleado varchar (50),
Reserva_Precio decimal (38),
)

create table Huespedes(
ID_Huespedes INT IDENTITY (1,1) PRIMARY KEY NOT NULL,
Cedula varchar (20),
Nombre varchar (20),
Apellido varchar (20),
Telefono varchar (20),
Fecha_nacimiento datetime,
)


select * from Acceso
select * from Huespedes

select * from Habitaciones
select * from Detalle_Reservas


DROP TABLE Habitaciones
DROP TABLE Detalle_Reservas

DROP DATABASE Hotel