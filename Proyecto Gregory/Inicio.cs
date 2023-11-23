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

        private void Inicio_Load(object sender, EventArgs e)
        {
            ExecuteProcedureAndDisplayResult("HUESPEDESTOTALES", huespedestotales);
            ExecuteProcedureAndDisplayResult("HabitacionesEnUso", habitacionesenuso);
            ExecuteProcedureAndDisplayResult("HABITACIONESDISPONIBLES", habitacionesdisponibles);
        }
    }
}
