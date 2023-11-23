using Proyecto_Gregory.Clases.Habitaciones;
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
    public partial class Habitaciones : Form
    {
        public Habitaciones()
        {
            InitializeComponent();
        }

        public static SqlConnection Conn = new SqlConnection("Server = DESKTOP-7EFN9F7; database=Hotel; Integrated Security=True");
        public string connectionString = "Data source = DESKTOP-7EFN9F7; Initial Catalog=Hotel; Integrated Security=True";

        void CargarComboBox()
        {
            Conn.Open();
            string consulta = "SELECT DISTINCT Tipo_habitacion FROM Habitaciones";
            SqlCommand comando = new SqlCommand(consulta, Conn);
            SqlDataReader lector = comando.ExecuteReader();

            while (lector.Read())
            {
                cClase.Items.Add(lector.GetString(0));
            }

            Conn.Close();



            Conn.Open();
            string consulta2 = "SELECT DISTINCT Capacida_maxima FROM Habitaciones";
            SqlCommand comando2 = new SqlCommand(consulta2, Conn);
            SqlDataReader lector2 = comando2.ExecuteReader();

            while (lector2.Read())
            {
                cCapacidad.Items.Add(lector2.GetSqlInt32(0));
            }

            Conn.Close();



            Conn.Open();
            string consulta3 = "SELECT DISTINCT Estado FROM Habitaciones";
            SqlCommand comando3 = new SqlCommand(consulta3, Conn);
            SqlDataReader lector3 = comando3.ExecuteReader();

            while (lector3.Read())
            {
                cDisponibilidad.Items.Add(lector3.GetString(0));
            }

            Conn.Close();
        }

        void Buscar()
        {
            dataGridView1.DataSource = DatosbaseHabitaciones.BuscarAlumnos(txtBuscar.Text, cClase.Text, cCapacidad.Text, cDisponibilidad.Text);
            UPDATE();
        }

        void UPDATE()
        {
            string procedureName = "ActualizarEstadoHabitacion";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(procedureName, connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.ExecuteNonQuery();
                    Console.WriteLine("Procedimiento ejecutado exitosamente");
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error al ejecutar el procedimiento almacenado: " + ex.Message);
            }


            string procedureName2 = "ActualizarEstadoHabitacionPorReservaCancelada";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(procedureName2, connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.ExecuteNonQuery();
                    Console.WriteLine("Procedimiento ejecutado exitosamente2");
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error al ejecutar el procedimiento almacenado: " + ex.Message);
            }

            string procedureName3 = "ActualizarEstadoHabitacionPorReservaNoCancelada";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(procedureName3, connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.ExecuteNonQuery();
                    Console.WriteLine("Procedimiento ejecutado exitosamente3");
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error al ejecutar el procedimiento almacenado: " + ex.Message);
            }
        }

        private void Habitaciones_Load(object sender, EventArgs e)
        {
            Buscar();
            CargarComboBox();
            UPDATE();
        }

        private void bunifuButton24_Click(object sender, EventArgs e)
        {
            Buscar();
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            Buscar();
        }

        private void cClase_SelectedIndexChanged(object sender, EventArgs e)
        {
            Buscar();
        }

        private void bunifuButton21_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dataGridView1.SelectedRows[0];

                string estado = row.Cells[7].Value.ToString();

                // Verifica si el estado es igual a 'Reservado', 'Ocupado' o 'Limpieza'
                if (estado == "Reservado" || estado == "Ocupado" || estado == "Limpieza")
                {
                    MessageBox.Show("No se puede asignar esta habitación, está en estado Reservado, Ocupado o Limpieza.", "Habitación no disponible", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {

                    // Obtén los datos de la fila seleccionada
                    Int64 id = Convert.ToInt64(row.Cells[0].Value);
                    string numero_habitacion = row.Cells[1].Value.ToString();
                    string tipo_habitacion = Convert.ToString(row.Cells[2].Value);
                    decimal tarifa_noche = Convert.ToDecimal(row.Cells[3].Value);
                    Int64 capacidad_maxima = Convert.ToInt64(row.Cells[4].Value);
                    Int64 camas = Convert.ToInt64(row.Cells[5].Value);
                    bool servicio = Convert.ToBoolean(row.Cells[6].Value);

                    // Abre el formulario para editar el producto
                    HabitacionAsig formEditar = new HabitacionAsig();
                    formEditar.EditMode = true; // Estás en modo editar
                    formEditar.InitializeData(id, numero_habitacion, tipo_habitacion, tarifa_noche, capacidad_maxima, camas, servicio, estado);
                    if (formEditar.ShowDialog() == DialogResult.OK)
                    {
                        // Actualiza el DataGridView después de la edición
                        Buscar();
                    }
                }
            }
        }

        private void bunifuButton22_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dataGridView1.SelectedRows[0];

                string estado = row.Cells[7].Value.ToString();

                // Obtén los datos de la fila seleccionada
                Int64 id = Convert.ToInt64(row.Cells[0].Value);
                string numero_habitacion = row.Cells[1].Value.ToString();
                string tipo_habitacion = Convert.ToString(row.Cells[2].Value);
                decimal tarifa_noche = Convert.ToDecimal(row.Cells[3].Value);
                Int64 capacidad_maxima = Convert.ToInt64(row.Cells[4].Value);
                Int64 camas = Convert.ToInt64(row.Cells[5].Value);
                bool servicio = Convert.ToBoolean(row.Cells[6].Value);

                // Abre el formulario para editar el producto
                HabitacionAsig formEditar = new HabitacionAsig();
                formEditar.EditMode = false; // Estás en modo editar
                formEditar.InitializeData(id, numero_habitacion, tipo_habitacion, tarifa_noche, capacidad_maxima, camas, servicio, estado);
                if (formEditar.ShowDialog() == DialogResult.OK)
                {
                    // Actualiza el DataGridView después de la edición
                    Buscar();
                }
            }
        }
    }
}