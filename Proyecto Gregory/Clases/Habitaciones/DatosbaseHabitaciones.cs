﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Gregory.Clases.Habitaciones
{
    public class DatosbaseHabitaciones
    {
        public static List<DatosgetHabitaciones> BuscarAlumnos(string pNumero, string pTipo, string pCapacidad, string pEstado)
        {
            List<DatosgetHabitaciones> lista = new List<DatosgetHabitaciones>();
            Conexion.opencon();

            string query = "SELECT ID_Habitacion, Numero_habitacion, Tipo_habitacion, Tarifa_noche, Capacida_maxima, Camas, Servicio_habitacion, Estado FROM Habitaciones WHERE 1 = 1";

            if (!string.IsNullOrEmpty(pNumero))
            {
                query += $" AND Numero_habitacion LIKE '%{pNumero}%'";
            }
            if (!string.IsNullOrEmpty(pTipo))
            {
                query += $" AND Tipo_habitacion LIKE '%{pTipo}%'";
            }
            if (!string.IsNullOrEmpty(pCapacidad))
            {
                query += $" AND Capacida_maxima LIKE '%{pCapacidad}%'";
            }
            if (!string.IsNullOrEmpty(pEstado))
            {
                query += $" AND Estado LIKE '%{pEstado}%'";
            }

            SqlCommand comando = new SqlCommand(query, Conexion.ObtenerConexion());
            SqlDataReader reader = comando.ExecuteReader();

            while (reader.Read())
            {
                DatosgetHabitaciones pAlumnos = new DatosgetHabitaciones();
                pAlumnos.Codigo = Convert.ToInt64(reader.GetValue(0));
                pAlumnos.Numero_habitacion = reader.GetString(1);
                pAlumnos.Tipo_habitacion = reader.GetString(2);
                pAlumnos.Tarifa_noche = reader.GetDecimal(3);
                pAlumnos.Capacidad = Convert.ToInt64(reader.GetValue(4));
                pAlumnos.Camas = Convert.ToInt64(reader.GetValue(5));
                pAlumnos.Servicio_habitacion = reader.GetBoolean(6);
                pAlumnos.Estado = reader.GetString(7);

                lista.Add(pAlumnos);
            }

            Conexion.cerrarcon();
            return lista;
        }

    }
}
