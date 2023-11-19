using Proyecto_Gregory.Clases.Clientes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Description;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;

namespace Proyecto_Gregory
{
    public partial class HabitacionAsig : Form
    {
        public HabitacionAsig()
        {
            InitializeComponent();
        }
        public bool EditMode { get; set; }

        public void InitializeData(Int64 id, string numero_habitacion, string tipo_habitacion, decimal tarifa_noche, Int64 capacidad_maxima, Int64 camas, bool servicio, string estado)
        {
            lbnumh.Text = numero_habitacion;
            lbtipoh.Text = tipo_habitacion;
            lbprecio.Text = tarifa_noche.ToString();
            lbcapacidad.Text = capacidad_maxima.ToString();
            lbcamas.Text = camas.ToString();
            chservicio.Value = servicio;
            lbestado.Text = estado;
        }

        private int CalcularDiferenciaEnDias(DateTime fechaInicio, DateTime fechaFin)
        {
            if (fechaInicio.Date == fechaFin.Date)
            {
                return 1;
            }
            else
            {
                TimeSpan diferencia = fechaFin - fechaInicio;
                return (int)diferencia.TotalDays;
            }
        }

        SqlConnection conn = new SqlConnection("Data source = DESKTOP-NDDA7LS; Initial Catalog = Hotel; Integrated Security = True");

        private Huespedes ObtenerHuesped(string identificador)
        {
            Huespedes huesped = null;

            // Cadena de conexión a la base de datos
            string connectionString = "Data source = DESKTOP-NDDA7LS; Initial Catalog=Hotel; Integrated Security=True";

            // Consulta SQL para obtener el huésped por ID o cédula
            string query = "SELECT ID_Huespedes, Cedula, Nombre, Apellido, Telefono, Fecha_nacimiento FROM Huespedes WHERE ID_Huespedes = @Id OR Cedula = @Cedula";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (Int64.TryParse(identificador, out Int64 id))
                    {
                        // Es un número (ID)
                        command.Parameters.AddWithValue("@Id", id);
                        command.Parameters.AddWithValue("@Cedula", DBNull.Value); // Valor nulo para Cedula
                    }
                    else
                    {
                        // Es una cédula
                        command.Parameters.AddWithValue("@Id", DBNull.Value); // Valor nulo para ID
                        command.Parameters.AddWithValue("@Cedula", identificador);
                    }   

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Obtener los valores del huésped desde el lector de datos
                            Int32 ids = reader.GetInt32(0);
                            string cedula = reader.GetString(1);
                            string nombre = reader.GetString(2);
                            string apellio = reader.GetString(3);
                            string telefono = reader.GetString(4);
                            DateTime fecha_nacimiento = reader.GetDateTime(5);

                            // Crear un objeto Huespedes con los valores obtenidos
                            huesped = new Huespedes
                            {
                                Id = ids,
                                Cedula = cedula,
                                Nombre = nombre,
                                Apellido = apellio,
                                Telefono = telefono,
                                Fecha_nacimiento = fecha_nacimiento
                            };
                        }
                    }
                }
                connection.Close();
            }

            return huesped;
        }

        private void VerificarAgregarModificarProducto(Huespedes huesped)
        {
            int maxHuespedes = Convert.ToInt32(lbcapacidad.Text); // Obtiene el número máximo de huéspedes del Label

            if (dataGridView1.Rows.Count >= maxHuespedes)
            {
                MessageBox.Show("Se ha alcanzado el límite de huéspedes permitidos.", "Límite alcanzado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return; // Detiene la ejecución si se ha alcanzado el límite de huéspedes
            }

            bool encontrado = false;

            // Recorrer las filas del DataGridView para buscar el huésped
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                // Obtener el ID del huésped en la fila actual
                Int64 id = Convert.ToInt64(row.Cells["idhuesped"].Value);

                if (id == huesped.Id)
                {
                    // El huésped ya está en el DataGridView, mostrar un mensaje
                    MessageBox.Show("El huésped ya se encuentra en la lista.", "Huésped duplicado", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Opcionalmente, puedes resaltar la fila encontrada
                    row.Selected = true;

                    encontrado = true;
                    break;
                }
            }

            if (!encontrado)
            {
                // El huésped no está en el DataGridView, agregar una nueva fila
                dataGridView1.Rows.Add(huesped.Id, huesped.Cedula, huesped.Nombre, huesped.Apellido, huesped.Telefono, huesped.Fecha_nacimiento);
            }
        }


        private void HabitacionAsig_Load(object sender, EventArgs e)
        {
            fechasalida_ValueChanged(sender, e);
        }

        private void fechasalida_ValueChanged(object sender, EventArgs e)
        {
            DateTime fechaInicio = fechaentrada.Value;
            DateTime fechaFin = fechasalida.Value;

            int diasDeDiferencia = CalcularDiferenciaEnDias(fechaInicio, fechaFin);

            if (diasDeDiferencia <= 0)
            {
                int dia1 = 1;
                lbdias.Text = dia1 + " día";
            }
            else
            {
                lbdias.Text = diasDeDiferencia + " días";
            }

            decimal precio = Convert.ToDecimal(lbprecio.Text);
            decimal resultado = Math.Abs(precio * diasDeDiferencia);


            lbtotal.Text = Convert.ToString(resultado);
        }

        private void bunifuButton21_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtcodigo.Text))
            {
                MessageBox.Show("Debe colocar el ID o la cédula del huésped.");
                return;
            }

            // Verificar si el texto ingresado es un número (ID) o una cédula
            if (Int64.TryParse(txtcodigo.Text, out Int64 idhuesped))
            {
                // Llamar a un método para obtener el huésped completo según su ID
                Huespedes huespedPorId = ObtenerHuesped(idhuesped.ToString());

                if (huespedPorId != null)
                {
                    // Verificar si el huésped ya está en el DataGridView y realizar la acción correspondiente
                    VerificarAgregarModificarProducto(huespedPorId);
                }
                else
                {
                    // No se encontró un huésped con el ID especificado, mostrar un mensaje de error o realizar alguna otra acción apropiada
                    MessageBox.Show("No se encontró ningún huésped con el ID especificado.");
                }
            }
            else
            {
                // Intentar buscar por cédula si el texto no es un número (asumiendo que el campo txtcodigo es para cédula)
                string cedula = txtcodigo.Text;

                // Llamar a un método para obtener el huésped completo según su cédula
                Huespedes huespedPorCedula = ObtenerHuesped(cedula);

                if (huespedPorCedula != null)
                {
                    // Verificar si el huésped ya está en el DataGridView y realizar la acción correspondiente
                    VerificarAgregarModificarProducto(huespedPorCedula);
                }
                else
                {
                    // No se encontró un huésped con la cédula especificada, mostrar un mensaje de error o realizar alguna otra acción apropiada
                    MessageBox.Show("No se encontró ningún huésped con la cédula especificada.");
                }
            }

            txtcodigo.Clear();
        }
    }
}
