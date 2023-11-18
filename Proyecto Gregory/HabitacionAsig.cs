﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Description;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;

namespace Proyecto_Gregory
{
    public partial class HabitacionAsig : Form
    {
        public HabitacionAsig()
        {
            InitializeComponent();
        }
        public bool EditMode { get; set; }

        public void InitializeData(Int64 id, string numero_habitacion, string tipo_habitacion, decimal tarifa_noche, Int64 capacidad_maxima, Int64 camas, bool servicio, string estado)
        {
            /*txtid.Text = id.ToString();
            txtnombre.Text = nombre;
            txtpreciocompra.Text = precioCompra.ToString();
            txtprecio.Text = precio.ToString();
            txtcantidad.Text = cantidad.ToString();
            cdepartamento.Text = departamento;
            dtp.Value = fecha;*/
        }

        private void HabitacionAsig_Load(object sender, EventArgs e)
        {
            fechasalida_ValueChanged(sender, e);
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
        }
    }
}
