using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gestion_de_tareas
{
    public partial class Form1 : Form
    {
        const int filas = 96; // 96 filas de 15 minutos tiene cada día

        public Form1()
        {
            InitializeComponent();
            //no permitimos que la el formulario cambie de tamaño ni que se maximice
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Añadimos las filas a la tabla
            for (int i = 0; i < filas; i++)
            {
                tabla.Rows.Add();
            }

            CargarFecha();
        }

        private void CargarFecha()
        {
            DateTime fechaSeleccionada = monthCalendar1.SelectionStart; //Obtenemos la fecha seleccionada en el calendario
            label1.Text = "Fecha seleccionada: " + fechaSeleccionada.ToString("dd/MM/yyyy");
            //Guardamos la fecha en un string
            string fecha = fechaSeleccionada.Day.ToString() + fechaSeleccionada.Month.ToString() + fechaSeleccionada.Year.ToString();
            //string fecha = label1.Text;

            if (!File.Exists(fecha))
            {
                StreamWriter sw = new StreamWriter(fecha);
                DateTime hoy = DateTime.Today;

                for (int i = 1; i <= filas; i++)
                {
                    sw.WriteLine(hoy.ToString("HH:mm"));
                    sw.WriteLine("");
                    hoy = hoy.AddMinutes(15);
                }
                sw.Close();
            }
            //Leemos el archivo fecha:
            StreamReader streamReader = new StreamReader(fecha);

            int cont = 0;

            while (!streamReader.EndOfStream)
            {
                string linea_1 = streamReader.ReadLine(); 
                string linea_2 = streamReader.ReadLine();

                tabla.Rows[cont].Cells[0].Value = linea_1;
                tabla.Rows[cont].Cells[1].Value = linea_2;

                cont++;
            }
            streamReader.Close();
        }

        private void tabla_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e) 
        {
            //Cuando seleccionamos una nueva fecha, la cargamos:
            CargarFecha();
        }

        private void tabla_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //Esta función se ejecuta cuando termine de editar una actividad
            //Actualizamos el archivo de texto de la fecha seleccionada

            DateTime fechaSeleccionada = monthCalendar1.SelectionStart; //Obtenemos la fecha seleccionada en el calendario
            //Guardamos la fecha en un string
            string fecha = fechaSeleccionada.Day.ToString() + fechaSeleccionada.Month.ToString() + fechaSeleccionada.Year.ToString();

            //Sobreescribimos el archivo de la fecha seleccionada para poder añadir la nueva tarea
            //Recorremos toda la tabla y la grabamos en esta fecha:
            StreamWriter sw = new StreamWriter(fecha);
            for (int i = 0; i < tabla.Rows.Count; i++)
            {
                sw.WriteLine(tabla.Rows[i].Cells[0].Value.ToString()); //Grabamos la hora
                //Si no hay una tarea a esa hora, la grabamos en la columna de actividades:
                if (tabla.Rows[i].Cells[1].Value != null)
                {
                    sw.WriteLine(tabla.Rows[i].Cells[1].Value.ToString());
                }
                else
                {
                    sw.WriteLine(""); //Si no hay una actividad, grabamos una linea en blanco.
                }
            }
            sw.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
