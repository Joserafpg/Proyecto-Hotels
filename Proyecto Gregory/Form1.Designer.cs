namespace Proyecto_Gregory
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panelmenu = new Bunifu.UI.WinForms.BunifuGradientPanel();
            this.button1 = new System.Windows.Forms.Button();
            this.btnmenu = new System.Windows.Forms.Button();
            this.panel3 = new Bunifu.UI.WinForms.BunifuPanel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panelmenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1200, 39);
            this.panel1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.label1.Location = new System.Drawing.Point(20, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 16);
            this.label1.TabIndex = 4;
            this.label1.Text = "GestFinance";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.Controls.Add(this.panelmenu);
            this.panel2.Location = new System.Drawing.Point(0, 39);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(255, 516);
            this.panel2.TabIndex = 2;
            // 
            // panelmenu
            // 
            this.panelmenu.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panelmenu.BackColor = System.Drawing.Color.Transparent;
            this.panelmenu.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panelmenu.BackgroundImage")));
            this.panelmenu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelmenu.BorderRadius = 35;
            this.panelmenu.Controls.Add(this.button1);
            this.panelmenu.Controls.Add(this.btnmenu);
            this.panelmenu.Controls.Add(this.panel3);
            this.panelmenu.GradientBottomLeft = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(43)))), ((int)(((byte)(54)))));
            this.panelmenu.GradientBottomRight = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(43)))), ((int)(((byte)(54)))));
            this.panelmenu.GradientTopLeft = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(43)))), ((int)(((byte)(54)))));
            this.panelmenu.GradientTopRight = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(43)))), ((int)(((byte)(54)))));
            this.panelmenu.Location = new System.Drawing.Point(50, 25);
            this.panelmenu.Name = "panelmenu";
            this.panelmenu.Padding = new System.Windows.Forms.Padding(0, 20, 0, 0);
            this.panelmenu.Quality = 10;
            this.panelmenu.Size = new System.Drawing.Size(160, 450);
            this.panelmenu.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Top;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(0, 99);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(160, 45);
            this.button1.TabIndex = 2;
            this.button1.Tag = "Clientes";
            this.button1.Text = "Clientes";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // btnmenu
            // 
            this.btnmenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(43)))), ((int)(((byte)(54)))));
            this.btnmenu.FlatAppearance.BorderSize = 0;
            this.btnmenu.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnmenu.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnmenu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnmenu.Image = ((System.Drawing.Image)(resources.GetObject("btnmenu.Image")));
            this.btnmenu.Location = new System.Drawing.Point(109, 10);
            this.btnmenu.Name = "btnmenu";
            this.btnmenu.Size = new System.Drawing.Size(48, 54);
            this.btnmenu.TabIndex = 0;
            this.btnmenu.Tag = "";
            this.btnmenu.UseVisualStyleBackColor = false;
            this.btnmenu.Click += new System.EventHandler(this.btnmenu_Click);
            // 
            // panel3
            // 
            this.panel3.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(43)))), ((int)(((byte)(54)))));
            this.panel3.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel3.BackgroundImage")));
            this.panel3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel3.BorderColor = System.Drawing.Color.Transparent;
            this.panel3.BorderRadius = 35;
            this.panel3.BorderThickness = 1;
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 20);
            this.panel3.Name = "panel3";
            this.panel3.ShowBorders = true;
            this.panel3.Size = new System.Drawing.Size(160, 79);
            this.panel3.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 555);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panelmenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private Bunifu.UI.WinForms.BunifuGradientPanel panelmenu;
        private System.Windows.Forms.Button btnmenu;
        private Bunifu.UI.WinForms.BunifuPanel panel3;
        private System.Windows.Forms.Button button1;
    }
}

