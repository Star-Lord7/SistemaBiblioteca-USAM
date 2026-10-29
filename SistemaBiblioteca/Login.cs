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
    public partial class Login : Form
    {
        public Login()
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
            // 1. Crea la instancia de tu menú principal
            MenuPrincipalForm menu = new MenuPrincipalForm();

            // 2. Si se cierra el menú principal, se cierra toda la aplicación
            menu.FormClosed += (s, args) => this.Close();

            // 3. Muestra el menú principal
            menu.Show();

            // 4. Oculta el login actual
            this.Hide();
        }

        private void Vista1_Load(object sender, EventArgs e)
        {

        }

        private void FlowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
