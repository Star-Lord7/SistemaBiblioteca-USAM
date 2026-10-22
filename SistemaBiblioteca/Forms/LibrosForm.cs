
using System;
using System.Windows.Forms;
using SistemaBiblioteca.Models;
using SistemaBiblioteca.Services;

namespace SistemaBiblioteca.Forms
{
    public partial class LibrosForm : Form
    {
        private readonly LibroService libroService =
            new LibroService();

        // ID interno del libro seleccionado
        private int idLibroSeleccionado = 0;

        public LibrosForm()
        {
            InitializeComponent();
        }

        private void LibrosForm_Load(object sender, EventArgs e)
        {
            try
            {
                CargarComboBox();
                CargarLibros();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al cargar datos: " + ex.Message);
            }
        }

        // Cargar autores, editoriales y categorías
        private void CargarComboBox()
        {
            cmbAutores.DataSource = libroService.ListarAutores();
            cmbAutores.DisplayMember = "Nombre_Autor";
            cmbAutores.ValueMember = "Id_Autor";

            cmbEditoriales.DataSource = libroService.ListarEditoriales();
            cmbEditoriales.DisplayMember = "Nombre_Ed";
            cmbEditoriales.ValueMember = "Cod_Ed";

            cmbCategorias.DataSource = libroService.ListarCategorias();
            cmbCategorias.DisplayMember = "NombreCategoria";
            cmbCategorias.ValueMember = "IdCategoria";

            cmbAutores.SelectedIndex = -1;
            cmbEditoriales.SelectedIndex = -1;
            cmbCategorias.SelectedIndex = -1;
        }

        // Mostrar libros en el DataGridView
        private void CargarLibros()
        {
            dgvLibros.DataSource = null;
            dgvLibros.DataSource = libroService.ListarDetalleLibros();

            dgvLibros.ReadOnly = true;
            dgvLibros.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvLibros.MultiSelect = false;
            dgvLibros.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvLibros.AllowUserToAddRows = false;

            // Ocultar identificadores y códigos internos
            dgvLibros.Columns["Id_Libro"].Visible = false;
            dgvLibros.Columns["Cod_Editorial"].Visible = false;
            dgvLibros.Columns["IdCategoria"].Visible = false;
            dgvLibros.Columns["Id_Autor"].Visible = false;

            dgvLibros.ClearSelection();
        }

        // Obtener los datos del formulario
        private Libro ObtenerDatosFormulario()
        {
            return new Libro
            {
                Id_Libro = idLibroSeleccionado,
                Titulo = txtTitulo.Text.Trim(),

                Cod_Editorial = cmbEditoriales.SelectedValue == null
                    ? null
                    : cmbEditoriales.SelectedValue.ToString(),

                IdCategoria = cmbCategorias.SelectedValue == null
                    ? null
                    : cmbCategorias.SelectedValue.ToString()
            };
        }

        // Obtener autor seleccionado
        private int ObtenerAutorSeleccionado()
        {
            if (cmbAutores.SelectedIndex == -1)
                return 0;

            return Convert.ToInt32(cmbAutores.SelectedValue);
        }

        // GUARDAR
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                Libro libro = ObtenerDatosFormulario();
                int idAutor = ObtenerAutorSeleccionado();

                libroService.GuardarLibro(libro, idAutor);

                MessageBox.Show("Libro guardado correctamente.");

                CargarLibros();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al guardar: " + ex.Message);
            }
        }

        // ACTUALIZAR
        private void btnActualizar_Click(object sender, EventArgs e)
        {
            if (idLibroSeleccionado == 0)
            {
                MessageBox.Show(
                    "Seleccione un libro para actualizar.");
                return;
            }

            try
            {
                Libro libro = ObtenerDatosFormulario();
                int idAutor = ObtenerAutorSeleccionado();

                libroService.EditarLibro(libro, idAutor);

                MessageBox.Show("Libro actualizado correctamente.");

                CargarLibros();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al actualizar: " + ex.Message);
            }
        }

        // ELIMINAR
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (idLibroSeleccionado == 0)
            {
                MessageBox.Show(
                    "Seleccione un libro para eliminar.");
                return;
            }

            DialogResult respuesta = MessageBox.Show(
                "¿Está seguro de eliminar este libro?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (respuesta != DialogResult.Yes)
                return;

            try
            {
                libroService.BorrarLibro(idLibroSeleccionado);

                MessageBox.Show("Libro eliminado correctamente.");

                CargarLibros();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo eliminar el libro: " + ex.Message);
            }
        }

        // LIMPIAR
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        // Seleccionar un libro en la tabla
        private void dgvLibros_CellClick(
            object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewRow fila = dgvLibros.Rows[e.RowIndex];

            idLibroSeleccionado =
                Convert.ToInt32(fila.Cells["Id_Libro"].Value);

            txtTitulo.Text =
                fila.Cells["Titulo"].Value.ToString();

            cmbEditoriales.SelectedValue =
                fila.Cells["Cod_Editorial"].Value.ToString();

            cmbCategorias.SelectedValue =
                fila.Cells["IdCategoria"].Value.ToString();

            if (fila.Cells["Id_Autor"].Value != DBNull.Value)
            {
                cmbAutores.SelectedValue =
                    Convert.ToInt32(fila.Cells["Id_Autor"].Value);
            }
            else
            {
                cmbAutores.SelectedIndex = -1;
            }
        }

        // Restablecer el formulario
        private void LimpiarCampos()
        {
            idLibroSeleccionado = 0;

            txtTitulo.Clear();

            cmbAutores.SelectedIndex = -1;
            cmbEditoriales.SelectedIndex = -1;
            cmbCategorias.SelectedIndex = -1;

            dgvLibros.ClearSelection();

            txtTitulo.Focus();
        }
    }
}
