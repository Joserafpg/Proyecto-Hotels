using Proyecto_Gregory.Clases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto_Gregory
{
    public partial class Inicio : Form
    {
        public Inicio()
        {
            InitializeComponent();
        }

        public string connectionString = "Data source = DESKTOP-7EFN9F7; Initial Catalog=Hotel; Integrated Security=True";

        private void ExecuteProcedureAndDisplayResult(string procedureName, Label textBox)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            using (SqlCommand command = new SqlCommand(procedureName, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                connection.Open();
                var result = command.ExecuteScalar();
                connection.Close();

                if (result != null)
                {
                    textBox.Text = result.ToString();
                }
            }
        }

        void Buscar()
        {
            String query = "SELECT habitacion, Fecha_entrada, Fecha_salida FROM Reservas ORDER BY ABS(DATEDIFF(day, GETDATE(), Fecha_salida)) ";
            
            Conexion.opencon();
            SqlCommand cmd = new SqlCommand(query, Conexion.ObtenerConexion());
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            Conexion.cerrarcon();
        }

        private void Inicio_Load(object sender, EventArgs e)
        {
            ExecuteProcedureAndDisplayResult("HUESPEDESTOTALES", huespedestotales);
            ExecuteProcedureAndDisplayResult("HabitacionesEnUso", habitacionesenuso);
            ExecuteProcedureAndDisplayResult("HABITACIONESDISPONIBLES", habitacionesdisponibles);
            Buscar();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
