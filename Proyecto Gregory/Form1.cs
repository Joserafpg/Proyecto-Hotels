﻿using Bunifu.UI.WinForms;
using Bunifu.UI.WinForms.BunifuButton;
using Bunifu.UI.WinForms.Helpers.Transitions;
using Bunifu.UI.WinForms.Renderers.Snackbar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        int time2 = 50;
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
        }

        private void btnmenu_Click(object sender, EventArgs e)
        {
            CollapseMenu();
        }

        private void btninicio_Click(object sender, EventArgs e)
        {
            AbrirFormEnPanel(new Inicio());
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
    }
}