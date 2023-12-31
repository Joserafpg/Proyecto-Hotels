﻿using Proyecto_Gregory.Clases.Clientes;
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
        public string connectionString = "Data source = DESKTOP-7EFN9F7; Initial Catalog=Hotel; Integrated Security=True";

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

        SqlConnection conn = new SqlConnection("Data source = DESKTOP-7EFN9F7; Initial Catalog = Hotel; Integrated Security = True");

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

        void Huesped()
        {
            if (EditMode == false)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        bunifuButton22.Visible = false;
                        label9.Visible = false;
                        txtcodigo.Visible = false;
                        bunifuButton21.Visible = false;
                        fechaentrada.Enabled = false;
                        fechasalida.Enabled = false;
                        connection.Open();

                        string habitacion = lbnumh.Text;

                        // Consulta para obtener los detalles de la reserva de la tabla Detalle_Reservas
                        string queryDetallesReservas = "SELECT DR.Id_Huesped, DR.Cedula, DR.Nombre, DR.Apellido, DR.Telefono, DR.Fecha_nacimiento " +
                                                       "FROM Detalle_Reservas AS DR " +
                                                       "LEFT JOIN Reservas AS R ON DR.Id_Reserva = R.Id_Reserva " +
                                                       "WHERE DR.Habitacion = @NombreHabitacion " +
                                                       "AND DR.Fecha_salida >= GETDATE() " +
                                                       "AND (R.Reserva_cancelada IS NULL OR R.Reserva_cancelada = 0)";


                        SqlCommand commandDetallesReservas = new SqlCommand(queryDetallesReservas, connection);
                        commandDetallesReservas.Parameters.AddWithValue("@NombreHabitacion", habitacion);

                        // Consulta para obtener las fechas de entrada y salida de la tabla Reservas
                        string queryFechasReservas = "SELECT Fecha_entrada, Fecha_salida " +
                                                     "FROM Reservas " +
                                                     "WHERE Habitacion = @NombreHabitacion " +
                                                     "AND Fecha_salida >= GETDATE() " +
                                                     "AND (Reserva_cancelada IS NULL OR Reserva_cancelada = 0)"; // Considerando que 0 significa no cancelada

                        SqlCommand commandFechasReservas = new SqlCommand(queryFechasReservas, connection);
                        commandFechasReservas.Parameters.AddWithValue("@NombreHabitacion", habitacion);

                        SqlDataAdapter adapter = new SqlDataAdapter();
                        adapter.SelectCommand = commandDetallesReservas;

                        DataTable detallesReservasTable = new DataTable();
                        adapter.Fill(detallesReservasTable);

                        DataTable fechasReservasTable = new DataTable();
                        adapter.SelectCommand = commandFechasReservas;
                        adapter.Fill(fechasReservasTable);

                        if (detallesReservasTable.Rows.Count > 0)
                        {
                            dataGridView1.Rows.Clear();

                            foreach (DataRow row in detallesReservasTable.Rows)
                            {
                                object[] rowData = row.ItemArray;
                                dataGridView1.Rows.Add(rowData[0], rowData[1], rowData[2], rowData[3], rowData[4], rowData[5]);
                            }
                        }
                        else
                        {
                            MessageBox.Show("No se encontraron detalles de reserva activa para el nombre de habitación proporcionado.");
                        }

                        if (fechasReservasTable.Rows.Count > 0)
                        {
                            // Suponiendo que tienes dos DateTimePicker llamados fechaentrada y fechasalida
                            fechaentrada.Value = Convert.ToDateTime(fechasReservasTable.Rows[0]["Fecha_entrada"]);
                            fechasalida.Value = Convert.ToDateTime(fechasReservasTable.Rows[0]["Fecha_salida"]);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al obtener los detalles de reserva: " + ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
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

        private void HabitacionAsig_Load(object sender, EventArgs e)
        {
            fechasalida_ValueChanged(sender, e);
            Huesped();            
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
                string actualizarQuery = "UPDATE Habitaciones SET Estado = 'Ocupado' WHERE Numero_habitacion = @Habitacion";


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

                this.Close();
            }
        }

        private void txtempleado_TextChanged(object sender, EventArgs e)
        {

        }

        private void bunifuButton23_Click(object sender, EventArgs e)
        {

        }

        private void lbestado_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void chservicio_CheckedChanged(object sender, Bunifu.UI.WinForms.BunifuToggleSwitch.CheckedChangedEventArgs e)
        {

        }

        private void lbtotal_Click(object sender, EventArgs e)
        {

        }

        private void label22_Click(object sender, EventArgs e)
        {

        }

        private void lbdias_Click(object sender, EventArgs e)
        {

        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void txtcodigo_TextChanged(object sender, EventArgs e)
        {

        }

        private void lbcamas_Click(object sender, EventArgs e)
        {

        }

        private void lbcapacidad_Click(object sender, EventArgs e)
        {

        }

        private void lbprecio_Click(object sender, EventArgs e)
        {

        }

        private void lbtipoh_Click(object sender, EventArgs e)
        {

        }

        private void lbnumh_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}