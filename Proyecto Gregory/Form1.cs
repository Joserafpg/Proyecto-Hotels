using Bunifu.UI.WinForms.Helpers.Transitions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto_Gregory
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        int valorsuperior = 200;

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
                Transition.run(panelmenu, "Width", 80, new TransitionType_Linear(valorsuperior));
                panel3.Visible = false;
                btnmenu.Dock = DockStyle.Top;
                foreach (Button menuButton in panelmenu.Controls.OfType<Button>())
                {
                    menuButton.Text = "";
                    menuButton.ImageAlign = ContentAlignment.MiddleCenter;
                    menuButton.Padding = new Padding(0);
                }
            }

            else
            {
                Transition.run(panelmenu, "Width", 160, new TransitionType_Linear(valorsuperior));
                panel3.Visible = true;
                btnmenu.Dock = DockStyle.None;
                foreach (Button menuButton in panelmenu.Controls.OfType<Button>())
                {
                    menuButton.Text = "   " + menuButton.Tag.ToString();
                    menuButton.ImageAlign = ContentAlignment.MiddleLeft;
                    menuButton.Padding = new Padding(0, 0, 0, 0);
                }
            }
        }

        //Transition.run(panelsuperior, "Height", 50, new TransitionType_Linear(valorsuperior));

        private void Form1_Load(object sender, EventArgs e)
        {
            borderadius();
        }

        private void btnmenu_Click(object sender, EventArgs e)
        {
            CollapseMenu();
        }
    }
}
