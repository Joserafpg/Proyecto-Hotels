using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Gregory.Clases
{
    public class Permisos
    {
        public Permisos()
        {
        }

        public static bool Recepcionista { set; get; }
        public static bool Tecnico { set; get; }
        public static bool Administrador { set; get; }
        public static bool Gerencia { set; get; }
        public static bool Contable { set; get; }
        public static bool Clientes { set; get; }
        public static bool Habitaciones { set; get; }
        public static bool Reservaciones { set; get; }
        public static bool Facturacion { set; get; }
        public static bool Configuracion { set; get; }
        public static bool Agregar { set; get; }
        public static bool Editar { set; get; }
        public static bool Buscar { set; get; }
        public static bool Eliminar { set; get; }
    }
}
