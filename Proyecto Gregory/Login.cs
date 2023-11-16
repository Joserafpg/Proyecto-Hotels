using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

            txtpass.ForeColor = Color.White;

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
            txtpass.ForeColor = Color.White;
        }

        private void btnpassview_MouseUp(object sender, MouseEventArgs e)
        {
            txtpass.PasswordChar = '●';
            txtpass.ForeColor = Color.White;
        }

        private void btnpassview_MouseDown(object sender, MouseEventArgs e)
        {
            txtpass.PasswordChar = '\0';
            txtpass.ForeColor = Color.White;
        }
    }
}
