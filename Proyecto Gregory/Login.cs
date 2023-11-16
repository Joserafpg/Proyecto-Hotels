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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void bunifuTextBox2_TextChanged(object sender, EventArgs e)
        {
            string inputText = txtpass.Text;

            bool containsLetterOrDigit = false;

            foreach (char c in inputText)
            {
                if (char.IsLetter(c) || char.IsDigit(c))
                {
                    containsLetterOrDigit = true;
                    break;
                }
            }

            if (containsLetterOrDigit)
            {
                txtpass.PasswordChar = '●';
                btnpassview.Visible = true;
            }

            else
            {
                txtpass.PasswordChar = '\0';
                btnpassview.Visible = false;
            }
        }

        private void btnpassview_Click(object sender, EventArgs e)
        {
            txtpass.PasswordChar = '\0';
        }

        private void btnpassview_MouseUp(object sender, MouseEventArgs e)
        {
            txtpass.PasswordChar = '●';
        }

        private void btnpassview_MouseDown(object sender, MouseEventArgs e)
        {
            txtpass.PasswordChar = '\0';
        }

        private void bunifuButton21_Click(object sender, EventArgs e)
        {
            if (txtuser.Text.Equals(""))
            {
                MessageBox.Show("El usuario no debe estar en blanco!...");
                txtuser.Focus();
                return;
            }

            if (txtpass.Text.Equals(""))
            {
                MessageBox.Show("La contraseña no debe estar en blanco!...");
                txtpass.Focus();
                return;
            }

            DataTable dt = new DataTable();
            string consulta;
            consulta = " select * from Usuarios where Usuario=@usuario AND Contraseña =@contrasena";
            Conexion.opencon();
            SqlDataAdapter da = new SqlDataAdapter(consulta, Conexion.ObtenerConexion());
            Conexion.cerrarcon();

            da.SelectCommand.CommandType = CommandType.Text;
            da.SelectCommand.Parameters.Add("@usuario", SqlDbType.VarChar, 10).Value = txtuser.Text;
            da.SelectCommand.Parameters.Add("@contrasena", SqlDbType.VarChar, 10).Value = txtpass.Text;

            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {

                Permisos.Recepcionista = Convert.ToBoolean(dt.Rows[0][4]);
                Permisos.Tecnico = Convert.ToBoolean(dt.Rows[0][5]);
                Permisos.Administrador = Convert.ToBoolean(dt.Rows[0][6]);
                Permisos.Gerencia = Convert.ToBoolean(dt.Rows[0][7]);
                Permisos.Contable = Convert.ToBoolean(dt.Rows[0][8]);
                Permisos.Clientes = Convert.ToBoolean(dt.Rows[0][9]);
                Permisos.Habitaciones = Convert.ToBoolean(dt.Rows[0][10]);
                Permisos.Reservaciones = Convert.ToBoolean(dt.Rows[0][11]);
                Permisos.Facturacion = Convert.ToBoolean(dt.Rows[0][12]);
                Permisos.Configuracion = Convert.ToBoolean(dt.Rows[0][13]);
                Permisos.Agregar = Convert.ToBoolean(dt.Rows[0][14]);
                Permisos.Editar = Convert.ToBoolean(dt.Rows[0][15]);
                Permisos.Buscar = Convert.ToBoolean(dt.Rows[0][16]);
                Permisos.Eliminar = Convert.ToBoolean(dt.Rows[0][17]);

                Datosgetregistro registro = new Datosgetregistro();

                registro.Usuario = txtuser.Text;

                int resultado = DatosbaseRegistro.Agregar(registro);

                Form principal = new Form1();
                principal.Show();
                principal.Visible = true;
                Visible = false;

            }

            else
            {
                MessageBox.Show(" Usuario o contraseña Incorrecto");
                txtpass.Focus();
                txtuser.BorderColorActive = Color.Red;
                txtuser.BorderColorDisabled = Color.Red;
                txtuser.BorderColorHover = Color.Red;
                txtuser.BorderColorIdle = Color.Red;

                txtpass.BorderColorActive = Color.Red;
                txtpass.BorderColorDisabled = Color.Red;
                txtpass.BorderColorHover = Color.Red;
                txtpass.BorderColorIdle = Color.Red;
            }

            Conexion.cerrarcon();
        }
    }
}
