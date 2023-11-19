using Proyecto_Gregory.Clases.Clientes;
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
using System.Web.Services.Description;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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

        private void bunifuButton22_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                conn.Open();
                string connectionString = "Data source = DESKTOP-NDDA7LS; Initial Catalog = Hotel; Integrated Security = True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Tu consulta SQL
                    string consulta = "SELECT Empleado FROM Usuarios WHERE Usuario = (SELECT TOP 1 Usuario FROM Acceso ORDER BY Fecha DESC)";

                    using (SqlCommand command = new SqlCommand(consulta, connection))
                    {
                        // Ejecutar la consulta y recuperar el valor
                        string resultadoConsulta = command.ExecuteScalar() as string;

                        // Asignar el resultado al TextBox
                        txtempleado.Text = resultadoConsulta;
                    }
                }

                DatosgetReserva pFactura = new DatosgetReserva();
                pFactura.Habitacion = lbnumh.Text;
                pFactura.fecha_entrada = fechaentrada.Value;
                pFactura.fecha_salida = fechasalida.Value;
                pFactura.Empleado = txtempleado.Text;
                pFactura.Reserva_precio = Convert.ToDecimal(lbtotal.Text);

                DatosbaseHabitaciones.Agregar(pFactura);

                txtid.Visible = true;

                // Consultar el último registro de Id_Factura en FacturaTittle
                string query = "SELECT TOP 1 Id_Reserva FROM Reservas ORDER BY Id_Reserva DESC";
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    // Obtener el resultado de la consulta
                    object result = command.ExecuteScalar();

                    // Verificar si se obtuvo un resultado válido
                    if (result != null && result != DBNull.Value)
                    {
                        // Convertir el resultado en un entero
                        int ultimoIdFactura = Convert.ToInt32(result);

                        // Mostrar el último Id_Factura en un TextBox
                        txtid.Text = ultimoIdFactura.ToString();
                    }
                }

                conn.Close();
                
                SqlCommand agregar = new SqlCommand("INSERT INTO Detalle_Reservas VALUES (@Id_Reserva, @Id_Huesped, @Cedula, @Nombre, @Apellido, @Telefono, @Fecha_nacimiento)", conn);
                //string verificarQuery = "SELECT Cantidad FROM Productos WHERE Nombre = @Producto";
                string actualizarQuery = "UPDATE Habitaciones SET Estado = 'Ocupado' WHERE Numero_habitacion = @Habitacion";


                conn.Open();

                try
                {
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        // Obtener los valores de la fila actual del DataGridView
                        Int64 idfactura = Convert.ToInt64(txtid.Text);
                        int id_huesped = Convert.ToInt32(row.Cells["idhuesped"].Value);
                        string cedula = Convert.ToString(row.Cells["cedula"].Value);
                        string nombre = Convert.ToString(row.Cells["nombre"].Value);
                        string apellido = Convert.ToString(row.Cells["apellido"].Value);
                        string telefono = Convert.ToString(row.Cells["telefono"].Value);
                        DateTime fecha = Convert.ToDateTime(row.Cells["fecha_nacimiento"].Value); 

                        // Agregar los parámetros al comando
                        agregar.Parameters.Clear();
                        agregar.Parameters.AddWithValue("@Id_Reserva", idfactura);
                        agregar.Parameters.AddWithValue("@Id_Huesped", id_huesped);
                        agregar.Parameters.AddWithValue("@Cedula", cedula);
                        agregar.Parameters.AddWithValue("@Nombre", nombre);
                        agregar.Parameters.AddWithValue("@Apellido", apellido);;
                        agregar.Parameters.AddWithValue("@Telefono", telefono);
                        agregar.Parameters.AddWithValue("@Fecha_nacimiento", fecha);

                        // Ejecutar el comando para agregar la factura
                        agregar.ExecuteNonQuery();

                        // Actualizar los datos de la tabla productos
                        using (SqlCommand actualizarCmd = new SqlCommand(actualizarQuery, conn))
                        {
                            actualizarCmd.Parameters.AddWithValue("@Habitacion", lbnumh.Text);
                            actualizarCmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Facturado con exito");
                    dataGridView1.Rows.Clear();
                    //Limpiar();
                }

                catch (Exception ex)
                {
                    MessageBox.Show("Error al guardar: " + ex.Message);
                }

                finally
                {
                    conn.Close();
                }
            }
        }

        private void txtempleado_TextChanged(object sender, EventArgs e)
        {

        }
    }
}