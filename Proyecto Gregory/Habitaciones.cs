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
        }

        private void Habitaciones_Load(object sender, EventArgs e)
        {
            Buscar();
            CargarComboBox();
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

                // Obtén los datos de la fila seleccionada
                Int64 id = Convert.ToInt64(row.Cells[0].Value);
                string numero_habitacion = row.Cells[1].Value.ToString();
                string tipo_habitacion = Convert.ToString(row.Cells[2].Value);
                decimal tarifa_noche = Convert.ToDecimal(row.Cells[3].Value);
                Int64 capacidad_maxima = Convert.ToInt64(row.Cells[4].Value);
                Int64 camas = Convert.ToInt64(row.Cells[5].Value);
                bool servicio = Convert.ToBoolean(row.Cells[6].Value);
                string estado = row.Cells[7].Value.ToString();;

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

        private void bunifuButton22_Click(object sender, EventArgs e)
        {

        }
    }
}
