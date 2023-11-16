using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Gregory.Clases.Habitaciones
{
    public class DatosgetHabitaciones
    {
        public Int64 Codigo { get; set; }
        public string Numero_habitacion { get; set; }
        public string Tipo_habitacion { get; set; }
        public decimal Tarifa_noche { get; set; }
        public Int64 Capacida_maxima { get; set; }
        public Int64 Camas { get; set; }
        public bool Servicio_habitacion { get; set; }
        public string Estado { get; set; }

        public DatosgetHabitaciones() { }

        public DatosgetHabitaciones(Int64 pCodigo, string pNumero, string pTipo, decimal pTarifa, Int64 pCapacidad, Int64 pCamas, bool pServicio, string pEstado)
        {
            this.Codigo = pCodigo;
            this.Numero_habitacion = pNumero;
            this.Tipo_habitacion = pTipo;
            this.Tarifa_noche = pTarifa;
            this.Capacida_maxima = pCapacidad;
            this.Camas = pCamas;
            this.Servicio_habitacion = pServicio;
            this.Estado = pEstado;
        }
    }
}
