﻿using Bunifu.UI.WinForms.Helpers.Transitions;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace Proyecto_Gregory
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        int time = 200;
        int time2 = 100;

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

        async Task EsperarAsync()
        {
            await Task.Delay(time2);
        }

        private async void CollapseMenu()
        {
            if (this.panelmenu.Width > 150)
            {
                Transition.run(panelmenu, "Width", 80, new TransitionType_Linear(time));
                panel3.Visible = false;
                btnmenu.Dock = DockStyle.Top;
                foreach (Button menuButton in panelmenu.Controls.OfType<Button>())
                {
                    menuButton.Text = "";
                    menuButton.ImageAlign = ContentAlignment.MiddleCenter;
                    menuButton.Padding = new Padding(0);
                }

                await EsperarAsync();

                panel2.Width = 150;
                panelmenu.Location = new Point(50, panel3.Location.Y);

            }

            else
            {
                panel3.Visible = true;
                Transition.run(panelmenu, "Width", 160, new TransitionType_Linear(time));
                btnmenu.Dock = DockStyle.None;
                foreach (Button menuButton in panelmenu.Controls.OfType<Button>())
                {
                    menuButton.Text = "   " + menuButton.Tag.ToString();
                    menuButton.ImageAlign = ContentAlignment.MiddleLeft;
                    menuButton.Padding = new Padding(0, 0, 0, 0);
                }

                await EsperarAsync();

                panel2.Width = 255;
                panelmenu.Location = new Point(50, panel3.Location.Y);

            }
        }

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
