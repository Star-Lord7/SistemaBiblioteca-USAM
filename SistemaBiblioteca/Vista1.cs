using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemaBiblioteca
{
    public partial class Vista1 : Form
    {
        public Vista1()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 1. Crea la instancia de tu nuevo formulario (reemplaza "Form2" con el nombre real de tu form)
            Vista2 nuevaPantalla = new Vista2();

            // 2. Configura que al cerrar la nueva pantalla, se cierre toda la aplicación
            // (Si no agregas esto, tu app quedará ejecutándose invisible en segundo plano)
            nuevaPantalla.FormClosed += (s, args) => this.Close();

            // 3. Muestra la nueva pantalla
            nuevaPantalla.Show();

            // 4. Oculta la pantalla actual (Form1)
            this.Hide();
        }
    }
}
