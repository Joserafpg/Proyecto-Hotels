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


select * from Acceso

DROP TABLE Habitaciones

DROP DATABASE Hotel