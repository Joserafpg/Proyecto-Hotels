using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Gregory.Clases.Habitaciones
{
    public class DatosgetReserva
    {

        public Int64 Codigo { get; set; }
        public string Habitacion { get; set; }
        public Int64 Id_Huesped { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public DateTime fecha_nacimiento { get; set; }
        public DateTime fecha_entrada { get; set; }
        public DateTime fecha_salida { get; set; }
        public string Empleado { get; set; }
        public decimal Reserva_precio { get; set; }

        public DatosgetReserva() { }

        public DatosgetReserva(Int64 pCodigo, string pHabitacion, Int64 pId_huesped, string pNombre, string pApellido, string pTelefono, DateTime pFecha_nacimiento, DateTime pFecha_entrada, DateTime pFecha_salida, string pEmpleado, decimal pPrecio)
        {
            this.Codigo = pCodigo;
            this.Habitacion = pHabitacion;
            this.Id_Huesped = pId_huesped;
            this.Nombre = pNombre;
            this.Apellido = pApellido;
            this.Telefono = pTelefono;
            this.fecha_nacimiento = pFecha_nacimiento;
            this.fecha_entrada = pFecha_entrada;
            this.fecha_salida = pFecha_salida;
            this.Empleado = pEmpleado;
            this.Reserva_precio = pPrecio;
        }
    }
}
