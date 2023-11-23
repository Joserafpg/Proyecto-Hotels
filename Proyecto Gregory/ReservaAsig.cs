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
using System.Windows.Forms;

namespace Proyecto_Gregory
{
    public partial class ReservaAsig : Form
    {
        public ReservaAsig()
        {
            InitializeComponent();
        }

        SqlConnection conn = new SqlConnection("Data source = DESKTOP-7EFN9F7; Initial Catalog = Hotel; Integrated Security = True");
        public string connectionString = "Data source = DESKTOP-7EFN9F7; Initial Catalog=Hotel; Integrated Security=True";

        public void InitializeData(Int64 id, string numero_habitacion, string tipo_habitacion, decimal tarifa_noche, Int64 capacidad_maxima, Int64 camas, bool servicio, string estado)
        {
            lbnumh.Text = numero_habitacion;
            lbtipoh.Text = tipo_habitacion;
            lbprecio.Text = tarifa_noche.ToString();
            lbcapacidad.Text = capacidad_maxima.ToString();
            lbcamas.Text = camas.ToString();
            chservicio.Value = servicio;
        }

        void CargarComboBox()
        {
            conn.Open();
            string consulta = "SELECT DISTINCT Tipo_habitacion FROM Habitaciones";
            SqlCommand comando = new SqlCommand(consulta, conn);
            SqlDataReader lector = comando.ExecuteReader();

            while (lector.Read())
            {
                cClase.Items.Add(lector.GetString(0));
            }

            conn.Close();



            conn.Open();
            string consulta2 = "SELECT DISTINCT Capacida_maxima FROM Habitaciones";
            SqlCommand comando2 = new SqlCommand(consulta2, conn);
            SqlDataReader lector2 = comando2.ExecuteReader();

            while (lector2.Read())
            {
                cCapacidad.Items.Add(lector2.GetSqlInt32(0));
            }

            conn.Close();



            conn.Open();
            string consulta3 = "SELECT DISTINCT Estado FROM Habitaciones";
            SqlCommand comando3 = new SqlCommand(consulta3, conn);
            SqlDataReader lector3 = comando3.ExecuteReader();

            while (lector3.Read())
            {
                cDisponibilidad.Items.Add(lector3.GetString(0));
            }

            conn.Close();
        }

        private Huespedes ObtenerHuesped(string identificador)
        {
            Huespedes huesped = null;

            string queryCedula = "SELECT ID_Huespedes, Cedula, Nombre, Apellido, Telefono, Fecha_nacimiento FROM Huespedes WHERE Cedula = @Cedula";
            string queryId = "SELECT ID_Huespedes, Cedula, Nombre, Apellido, Telefono, Fecha_nacimiento FROM Huespedes WHERE ID_Huespedes = @Id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(queryCedula, connection))
                {
                    command.Parameters.AddWithValue("@Cedula", identificador);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Obtener los valores del huésped desde el lector de datos
                            Int32 ids = reader.GetInt32(0);
                            string cedula = reader.GetString(1);
                            string nombre = reader.GetString(2);
                            string apellido = reader.GetString(3);
                            string telefono = reader.GetString(4);
                            DateTime fecha_nacimiento = reader.GetDateTime(5);

                            // Crear un objeto Huespedes con los valores obtenidos
                            huesped = new Huespedes
                            {
                                Id = ids,
                                Cedula = cedula,
                                Nombre = nombre,
                                Apellido = apellido,
                                Telefono = telefono,
                                Fecha_nacimiento = fecha_nacimiento
                            };
                        }
                    }
                }

                // Si no se encontró por cédula, intentamos buscar por ID
                if (huesped == null && Int64.TryParse(identificador, out Int64 id))
                {
                    using (SqlCommand command = new SqlCommand(queryId, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Obtener los valores del huésped desde el lector de datos
                                Int32 ids = reader.GetInt32(0);
                                string cedula = reader.GetString(1);
                                string nombre = reader.GetString(2);
                                string apellido = reader.GetString(3);
                                string telefono = reader.GetString(4);
                                DateTime fecha_nacimiento = reader.GetDateTime(5);

                                // Crear un objeto Huespedes con los valores obtenidos
                                huesped = new Huespedes
                                {
                                    Id = ids,
                                    Cedula = cedula,
                                    Nombre = nombre,
                                    Apellido = apellido,
                                    Telefono = telefono,
                                    Fecha_nacimiento = fecha_nacimiento
                                };
                            }
                        }
                    }
                }

                connection.Close();

            }

            return huesped;

        }

        public bool VerificarReservaActiva(string nombreHuesped, DateTime fechaActual)
        {
            bool tieneReservaActiva = false;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Consulta SQL para verificar la reserva activa del huésped por su nombre y fecha de salida superior a la fecha actual
                string query = @"SELECT COUNT(*)
                                FROM Detalle_Reservas AS DR
                                INNER JOIN Reservas AS R ON DR.Id_Reserva = R.Id_Reserva
                                WHERE DR.Nombre = @Nombre 
                                AND R.Fecha_salida >= @FechaActual";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", nombreHuesped);
                    command.Parameters.AddWithValue("@FechaActual", fechaActual);

                    // Ejecutar la consulta y obtener el recuento de reservas activas
                    int count = Convert.ToInt32(command.ExecuteScalar());

                    // Si el recuento es mayor que cero, el huésped tiene una reserva activa
                    if (count > 0)
                    {
                        tieneReservaActiva = true;
                    }
                }
            }

            return tieneReservaActiva;
        }

        int ObtenerIdReservaActiva(string nombreHuesped)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT Id_Reserva FROM Reservas WHERE Habitacion = @NombreHuesped AND Fecha_salida >= GETDATE()";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@NombreHuesped", nombreHuesped);

                object result = command.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    return Convert.ToInt32(result);
                }
                return -1; // Retorna un valor predeterminado en caso de no encontrar una reserva activa
            }
        }

        bool HanPasadoDosHorasDesdeSalida(int idReserva)
        {
            DateTime fechaSalidaReserva;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Fecha_salida FROM Reservas WHERE Id_Reserva = @IdReserva";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@IdReserva", idReserva);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        fechaSalidaReserva = Convert.ToDateTime(result);
                        TimeSpan diferencia = DateTime.Now - fechaSalidaReserva;

                        return diferencia.TotalHours >= 2;
                    }
                    else
                    {
                        // La consulta no devolvió una fecha válida
                        return false;
                    }
                }
                catch (Exception)
                {
                    // Manejo de excepciones
                    return false;
                }
            }
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

        private void bunifuButton22_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                conn.Open();
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
                pFactura.Reserva_cancelada = false;

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

                SqlCommand agregar = new SqlCommand("INSERT INTO Detalle_Reservas VALUES (@Id_Reserva, @Habitaciones, @Id_Huesped, @Cedula, @Nombre, @Apellido, @Telefono, @fecha, @Fecha_nacimiento)", conn);
                //string verificarQuery = "SELECT Cantidad FROM Productos WHERE Nombre = @Producto";
                string actualizarQuery = "UPDATE Habitaciones SET Estado = 'Reservado' WHERE Numero_habitacion = @Habitacion";


                conn.Open();

                try
                {
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        // Obtener los valores de la fila actual del DataGridView
                        Int64 idfactura = Convert.ToInt64(txtid.Text);
                        string habitacion = Convert.ToString(lbnumh.Text);
                        int id_huesped = Convert.ToInt32(row.Cells["idhuesped"].Value);
                        string cedula = Convert.ToString(row.Cells["cedula"].Value);
                        string nombre = Convert.ToString(row.Cells["nombre"].Value);
                        string apellido = Convert.ToString(row.Cells["apellido"].Value);
                        string telefono = Convert.ToString(row.Cells["telefono"].Value);
                        DateTime fecha = Convert.ToDateTime(row.Cells["fecha_nacimiento"].Value);
                        DateTime fechasa = fechasalida.Value;

                        // Agregar los parámetros al comando
                        agregar.Parameters.Clear();
                        agregar.Parameters.AddWithValue("@fecha", fechasa);
                        agregar.Parameters.AddWithValue("@Habitaciones", habitacion);
                        agregar.Parameters.AddWithValue("@Id_Reserva", idfactura);
                        agregar.Parameters.AddWithValue("@Id_Huesped", id_huesped);
                        agregar.Parameters.AddWithValue("@Cedula", cedula);
                        agregar.Parameters.AddWithValue("@Nombre", nombre);
                        agregar.Parameters.AddWithValue("@Apellido", apellido); ;
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

                this.Close();
            }
        }

        void Buscar()
        {
            dataGridView2.DataSource = DatosbaseHabitaciones.BuscarAlumno3(txtbuscar.Text, cClase.Text, cCapacidad.Text, cDisponibilidad.Text);
        }

        private void bunifuButton21_Click(object sender, EventArgs e)
        {
            DateTime fechaHoraActual = DateTime.Now;

            if (Int64.TryParse(txtcodigo.Text, out Int64 idHuesped))
            {
                Huespedes huespedPorId = ObtenerHuesped(idHuesped.ToString());

                if (huespedPorId != null)
                {
                    bool tieneReserva = VerificarReservaActiva(huespedPorId.Nombre, fechaHoraActual);

                    if (tieneReserva)
                    {
                        // Si tiene reserva activa, verificamos si ha pasado más de 2 horas desde la salida
                        int idReserva = ObtenerIdReservaActiva(huespedPorId.Nombre);
                        if (idReserva != -1 && HanPasadoDosHorasDesdeSalida(idReserva))
                        {
                            VerificarAgregarModificarProducto(huespedPorId);
                        }
                        else
                        {
                            MessageBox.Show("El huésped ya tiene una reserva activa o su reserva anterior no ha finalizado.");
                        }
                    }
                    else
                    {
                        VerificarAgregarModificarProducto(huespedPorId);
                    }
                }
                else
                {
                    MessageBox.Show("No se encontró ningún huésped con el ID especificado.");
                }
            }

            txtcodigo.Clear();
        }

        private void fechaentrada_ValueChanged(object sender, EventArgs e)
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

        private void bunifuButton23_Click(object sender, EventArgs e)
        {
            Buscar();
        }

        private void ReservaAsig_Load(object sender, EventArgs e)
        {
            CargarComboBox();
            Buscar();
        }

        private void cClase_SelectedIndexChanged(object sender, EventArgs e)
        {
            Buscar();
        }

        private void txtbuscar_TextChanged(object sender, EventArgs e)
        {
            Buscar();
        }

        private void bunifuButton24_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dataGridView2.SelectedRows[0];

                string estado = row.Cells[7].Value.ToString();

                // Obtén los datos de la fila seleccionada
                Int64 id = Convert.ToInt64(row.Cells[0].Value);
                string numero_habitacion = row.Cells[1].Value.ToString();
                string tipo_habitacion = Convert.ToString(row.Cells[2].Value);
                decimal tarifa_noche = Convert.ToDecimal(row.Cells[3].Value);
                Int64 capacidad_maxima = Convert.ToInt64(row.Cells[4].Value);
                Int64 camas = Convert.ToInt64(row.Cells[5].Value);
                bool servicio = Convert.ToBoolean(row.Cells[6].Value);

                lbnumh.Text = numero_habitacion; 
                lbtipoh.Text = tipo_habitacion; 
                lbprecio.Text = Convert.ToString(tarifa_noche); 
                lbcapacidad.Text = Convert.ToString(capacidad_maxima); 
                lbcamas.Text = Convert.ToString(camas); 
                chservicio.Checked = servicio;

                panel1.Visible = false;
            }
        }
    }
}