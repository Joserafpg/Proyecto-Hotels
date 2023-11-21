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

create table Reservas(
Id_Reserva INT IDENTITY (1,1) PRIMARY KEY NOT NULL,
Habitacion varchar (30),
Fecha_entrada datetime,
Fecha_salida datetime,
Empleado varchar (50),
Reserva_Precio decimal (38),
Reserva_cancelada bit NOT NULL,
)

create table Detalle_Reservas (
Id_Reserva INT,
Habitacion varchar (20),
Id_Huesped int,
Cedula varchar (50),
Nombre varchar (20),
Apellido varchar (20),
Telefono varchar (20),
Fecha_salida datetime,
Fecha_nacimiento datetime,
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
select * from Reservas
select * from Detalle_Reservas


DROP TABLE Habitaciones
DROP TABLE Detalle_Reservas
DROP TABLE Reservas


DROP DATABASE Hotel



/*Pruebas*/
SELECT COUNT(*) FROM Detalle_Reservas AS DR
                         INNER JOIN Reservas AS R ON DR.Id_Reserva = R.Id_Reserva
                         WHERE DR.Nombre = @Nombre AND R.Fecha_salida > @FechaActual

SELECT COUNT(*) FROM Detalle_Reservas AS DR
                         INNER JOIN Reservas AS R ON DR.Id_Reserva = R.Id_Reserva
                         WHERE DR.Nombre = 'Reynaldo Antonio' AND R.Fecha_salida > GETDATE()

/*procedure habitaciones ocupadas*/
SELECT COUNT (Id_Reserva) FROM Reservas WHERE Fecha_salida > GETDATE()

/*procedure huespedes totales*/
SELECT COUNT (Id_Huesped) FROM Detalle_Reservas WHERE Fecha_salida > GETDATE()

SELECT Id_Huesped, Cedula, Nombre, Apellido, Telefono, Fecha_nacimiento FROM Detalle_Reservas


SELECT Id_Huesped, Cedula, Nombre, Apellido, Telefono, Fecha_nacimiento
                                                       FROM Detalle_Reservas
                                                       WHERE Habitacion = 'E1-11' 
                                                       AND Fecha_salida >= GETDATE()


DECLARE @Nombre NVARCHAR(100) = 'E1-02'; -- Reemplaza 'NombreDeLaHabitacion' con el nombre real
DECLARE @FechaActual DATE = GETDATE(); -- Obtén la fecha actual

SELECT COUNT(*)
FROM Detalle_Reservas AS DR
INNER JOIN Reservas AS R ON DR.Id_Reserva = R.Id_Reserva
WHERE DR.Nombre = @Nombre 
AND R.Fecha_salida >= @FechaActual;



/*Procedures*/

CREATE PROCEDURE ActualizarEstadoHabitacion
AS
BEGIN
    UPDATE Habitaciones
    SET Estado = 'Disponible'
    FROM Habitaciones AS H
    INNER JOIN Reservas AS R ON H.Numero_habitacion = R.Habitacion
    WHERE R.Fecha_salida <= GETDATE() AND H.Estado <> 'Disponible';
END;

exec ActualizarEstadoHabitacion


CREATE PROCEDURE HabitacionesEnUso
AS
BEGIN
    SELECT COUNT(Id_Reserva) AS 'CantidadHabitacionesEnUso'
    FROM Reservas 
    WHERE Fecha_salida > GETDATE();
END;

EXEC HabitacionesEnUso


CREATE PROCEDURE HUESPEDESTOTALES
AS
BEGIN
    SELECT COUNT(Id_Huesped) AS 'HUESPEDESTOTALES'
    FROM Detalle_Reservas 
    WHERE Fecha_salida > GETDATE();
END;

EXEC HUESPEDESTOTALES


CREATE PROCEDURE HABITACIONESDISPONIBLES
AS
BEGIN
    DECLARE @TotalHabitaciones INT;
	DECLARE @HabitacionesEnUso INT;

	SELECT @TotalHabitaciones = COUNT(ID_Habitacion)
	FROM Habitaciones;

	SELECT @HabitacionesEnUso = COUNT(Id_Reserva)
	FROM Reservas 
	WHERE Fecha_salida > GETDATE();

SELECT (@TotalHabitaciones - @HabitacionesEnUso) AS 'HabitacionesDisponibles';
END;

EXEC HABITACIONESDISPONIBLES




drop procedure HABITACIONESTOTALES