create database Hotel

use Hotel

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


DROP TABLE Habitaciones