using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Gregory.Clases
{
    public class Datosgetregistro
    {
        public string Empleado { get; set; }
        public string Usuario { get; set; }
        public string Nombre { get; set; }

        public DateTime Fecha_de_Ingreso = DateTime.Now;

        public Datosgetregistro() { }

        public Datosgetregistro(string pEmpleado, string pUsuario, string pnombre, DateTime pfecha)
        {
            this.Empleado = pEmpleado;
            this.Usuario = pUsuario;
            this.Nombre = pnombre;
            this.Fecha_de_Ingreso = pfecha;
        }
    }
}
