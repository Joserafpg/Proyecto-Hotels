create database Hotel

use Hotel

create table Habitaciones (
Numero_habitacion varchar (30) PRIMARY KEY,
Tipo_habitacion varchar (30),
Tarifa_noche decimal (38),
Capacida_naxima int,
Camas int,
Servicio_habitacion bit,
Estado varchar (20)
)