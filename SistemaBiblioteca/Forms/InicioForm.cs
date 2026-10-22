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

namespace SistemaBiblioteca.Forms
{
    public partial class InicioForm : Form
    {
        private readonly LibroService libroService = new LibroService();

        public InicioForm()
        {
            InitializeComponent();
        }

        private void InicioForm_Load(object sender, EventArgs e)
        {
            CargarEstadisticas();
        }

        private void lblTotalLibros_Click(object sender, EventArgs e)
        {

        }

        private void CargarEstadisticas()
        {
            try
            {
                int totalLibros = libroService.ObtenerTotalLibros();

                lblTotalLibros.Text = totalLibros.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al cargar las estadísticas: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
