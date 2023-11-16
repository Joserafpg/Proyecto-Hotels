using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Gregory.Clases
{
    public class DatosbaseRegistro
    {
        public static int Agregar(Datosgetregistro pget)
        {
            int retorno = 0;

            Conexion.opencon();

            SqlCommand Comando = new SqlCommand(string.Format("Insert into Acceso (Usuario, Fecha) values ('{0}','{1}')",
                    pget.Usuario, pget.Fecha_de_Ingreso.ToString("yyyy-MM-dd HH:mm:ss")), Conexion.ObtenerConexion());

            retorno = Comando.ExecuteNonQuery();
            Conexion.cerrarcon();
            return retorno;

        }
    }
}
