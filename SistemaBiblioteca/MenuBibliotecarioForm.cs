using SistemaBiblioteca.Forms;
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
    public partial class MenuBibliotecarioForm : Form
    {
        public MenuBibliotecarioForm()
        {
            InitializeComponent();
        }

        private void AbrirFormulario(Form formularioHijo)
        {
            // Eliminar el formulario que se muestra actualmente
            panelContenido.Controls.Clear();

            // Configurar el nuevo formulario
            formularioHijo.TopLevel = false;
            formularioHijo.FormBorderStyle = FormBorderStyle.None;
            formularioHijo.Dock = DockStyle.Fill;

            // Mostrarlo dentro del panel
            panelContenido.Controls.Add(formularioHijo);
            formularioHijo.Show();
        }


        // Evento para el botón Inicio
        private void Button1_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new InicioForm());
        }

        // Evento para el botón Libros (Abre Vista1)
        private void Button2_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new LibrosForm());
        }

        // Evento para el botón Usuarios (Abre Vista2)
        private void Button3_Click(object sender, EventArgs e)
        {
            //AbrirFormulario(new UsuariosForm());
        }

        // Evento para el botón Préstamos
        private void Button4_Click(object sender, EventArgs e)
        {
            //  AbrirFormulario(new PrestamosForm());
        }

        // Evento para el botón Reportes
        private void Button5_Click(object sender, EventArgs e)
        {
            // AbrirFormulario(new ReportesForm());
        }

        // Evento para el botón Configuración
        private void Button7_Click(object sender, EventArgs e)
        {
            // Evento para Configuración
        }

        // Evento para el botón Cerrar sesión con ventana de confirmación
        private void Button6_Click(object sender, EventArgs e)
        {
            DialogResult resultado = MessageBox.Show(
                "¿Está seguro de que desea cerrar la sesión?",
                "Confirmar Cierre de Sesión",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (resultado == DialogResult.Yes)
            {
                this.Close();
            }
        }

        // Métodos vacíos para resolver los vínculos del Designer y evitar errores CS1061
        private void Panel1_Paint(object sender, PaintEventArgs e) { }
        private void Panel4_Paint(object sender, PaintEventArgs e) { }
        private void Label5_Click(object sender, EventArgs e) { }
        private void Label1_Click(object sender, EventArgs e) { }
        private void Label10_Click(object sender, EventArgs e) { }

        private void MenuPrincipalForm_Load(object sender, EventArgs e)
        {
            AbrirFormulario(new InicioForm());
        }
    }
}
// Cambios de navegación listos
