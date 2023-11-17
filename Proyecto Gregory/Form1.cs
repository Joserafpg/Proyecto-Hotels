using Bunifu.UI.WinForms;
using Bunifu.UI.WinForms.BunifuButton;
using Bunifu.UI.WinForms.Helpers.Transitions;
using Bunifu.UI.WinForms.Renderers.Snackbar;
using Proyecto_Gregory.Clases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace Proyecto_Gregory
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            timer = new Timer();
            timer.Interval = delayMilliseconds;
            timer.Tick += Timer_Tick;

            timer2 = new Timer();
            timer2.Interval = 1000;
            timer2.Tick += Timer_Tick2;

            timer2.Start();
        }

        int time = 200;
        private Timer timer;
        private Timer timer2;
        private const int delayMilliseconds = 1000;

        private void Timer_Tick(object sender, EventArgs e)
        {
            paneluser.Visible = false;
            timer.Stop();
        }
        
        private void Timer_Tick2(object sender, EventArgs e)
        {
            labelhora.Text = DateTime.Now.ToString("HH:mm");
            DateTime fechaActual = DateTime.Now;
            fecha.Text = fechaActual.ToString("dd/MM/yyyy HH:mm:ss");
        }

        void borderadius()
        {
            int borderRadius = 20;
            GraphicsPath objDraw = new GraphicsPath();

            objDraw.AddArc(0, 0, borderRadius * 2, borderRadius * 2, 180, 90);
            objDraw.AddArc(this.Width - borderRadius * 2, 0, borderRadius * 2, borderRadius * 2, 270, 90);
            objDraw.AddArc(this.Width - borderRadius * 2, this.Height - borderRadius * 2, borderRadius * 2, borderRadius * 2, 0, 90);
            objDraw.AddArc(0, this.Height - borderRadius * 2, borderRadius * 2, borderRadius * 2, 90, 90);
            objDraw.CloseFigure();

            this.Region = new Region(objDraw);
        }

        private void CollapseMenu()
        {
            if (this.panelmenu.Width > 150)
            {
                Transition.run(panelmenu, "Width", 80, new TransitionType_Linear(time));
                btnmenu.Dock = DockStyle.Top;
                panel3.Visible = false;
                foreach (BunifuButton2 menuButton in panelmenu.Controls.OfType<BunifuButton2>())
                {
                    menuButton.Text = "";
                    menuButton.IconLeftAlign = ContentAlignment.MiddleCenter;
                }

                Transition.run(panel2, "Width", 150, new TransitionType_Linear(time));

            }

            else
            {
                Transition.run(panelmenu, "Width", 190, new TransitionType_Linear(time));
                panel3.Visible = true;
                btnmenu.Dock = DockStyle.None;
                foreach (BunifuButton2 menuButton in panelmenu.Controls.OfType<BunifuButton2>())
                {
                    menuButton.Text = "   " + menuButton.Tag.ToString();
                    menuButton.IconLeftAlign = ContentAlignment.MiddleLeft;
                }

                Transition.run(panel2, "Width", 255, new TransitionType_Linear(time));

            }
        }

        public void AbrirFormEnPanel(object Formhijo)
        {
            if (this.paneldesktop.Controls.Count > 0)
                this.paneldesktop.Controls.RemoveAt(0);
            Form fh = Formhijo as Form;
            fh.TopLevel = false;
            fh.Dock = DockStyle.Fill;
            this.paneldesktop.Controls.Add(fh);
            this.paneldesktop.Tag = fh;
            fh.Show();
        }

        public void text(string tittle, string description)
        {
            tittlepage.Text = tittle;
            descriptionpage.Text = description;
        }


        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            borderadius();

            btninicio.PerformClick();

            string connectionString = "Data source = DESKTOP-7EFN9F7; Initial Catalog=Hotel; Integrated Security=True";

            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                conexion.Open();

                string consultaSQL = "SELECT TOP 1 * FROM Acceso ORDER BY Fecha DESC";

                using (SqlCommand comando = new SqlCommand(consultaSQL, conexion))
                {
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            labeluser.Text = reader["Usuario"].ToString();
                            lbuser.Text = reader["Usuario"].ToString();
                        }
                    }
                }
            }

            lbpermiso.Text =    Permisos.Recepcionista ? "Recepcionista" :
                                Permisos.Tecnico ? "Tecnico" :
                                Permisos.Administrador ? "Admin" : 
                                Permisos.Gerencia ? "Gerencia" :
                                Permisos.Contable ? "Contable" : "";
        }

        private void btnmenu_Click(object sender, EventArgs e)
        {
            CollapseMenu();
        }

        private void btninicio_Click(object sender, EventArgs e)
        {
            AbrirFormEnPanel(new Inicio());
            text("Inicio", "???.");
        }

        private void labelhora_MouseEnter(object sender, EventArgs e)
        {
            panelfecha.Visible = true;
        }

        private void labelhora_MouseLeave(object sender, EventArgs e)
        {
            panelfecha.Visible = false;
        }

        private bool panelVisible = false;

        private void labeluser_Click(object sender, EventArgs e)
        {
            panelVisible = !panelVisible;
            paneluser.Visible = panelVisible;
        }

        private void paneluser_MouseLeave(object sender, EventArgs e)
        {
            timer.Start();
        }

        private void paneluser_MouseEnter(object sender, EventArgs e)
        {
            timer.Stop();
        }

        private void btnclientes_Click(object sender, EventArgs e)
        {
            AbrirFormEnPanel(new Clientes());
            text("Clientes", "???.");
        }

        private void btnhabitaciones_Click(object sender, EventArgs e)
        {
            AbrirFormEnPanel(new Habitaciones());
            text("Habitaciones", "???.");
        }

        private void btnreservaciones_Click(object sender, EventArgs e)
        {
            AbrirFormEnPanel(new Reservaciones());
            text("Reservaciones", "???.");
        }

        private void btncerrar_Click(object sender, EventArgs e)
        {
            Login frm = new Login();
            frm.Show();
            this.Close();
        }        
    }
}