using SistemaBiblioteca.Models;
using SistemaBiblioteca.Services;
using SistemaBiblioteca.Services;
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
        private readonly AuthService authService = new AuthService();

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
            try
            {
                string correo = txtCorreo.Text.Trim();
                string password = txtPassword.Text;

                Usuario usuario = authService.IniciarSesion(
                    correo,
                    password);

                if (usuario == null)
                {
                    MessageBox.Show(
                        "Correo o contraseña incorrectos.",
                        "Error de autenticación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                Form menu = null;

                switch (usuario.NombreRol)
                {
                    case "Administrador":
                        menu = new MenuPrincipalForm();
                        break;

                    case "Bibliotecario":
                        menu = new MenuBibliotecarioForm();
                        break;

                    case "Usuario":
                        menu = new MenuUsuarioForm();
                        break;

                    default:
                        MessageBox.Show(
                            "El usuario no tiene un rol válido.",
                            "Acceso denegado",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        return;
                }

                /*MessageBox.Show(
                    "Bienvenido, " + usuario.Nombre,
                    "Inicio de sesión exitoso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);*/

                this.Hide();

                menu.ShowDialog();

                txtCorreo.Clear();

                txtPassword.Clear();

                this.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void chkMostrarPassword_CheckedChanged(
    object sender,
    EventArgs e)
        {
            txtPassword.UseSystemPasswordChar =
                !chkMostrarPassword.Checked;
        }

        private void Vista1_Load(object sender, EventArgs e)
        {

        }

        private void FlowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
