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

        private void Form1_Load(object sender, EventArgs e)
        {
            borderadius();
        }

        private void bunifuGradientPanel1_MouseEnter(object sender, EventArgs e)
        {
            Transition.run(panelsuperior, "Height", 80, new TransitionType_Linear(valorsuperior));
        }

        private void panelsuperior_MouseLeave(object sender, EventArgs e)
        {
            Transition.run(panelsuperior, "Height", 50, new TransitionType_Linear(valorsuperior));
        }
    }
}
