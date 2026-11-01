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
    public partial class UsuariosForm : Form
    {
        private int idUsuarioSeleccionado = 0;
        private readonly UsuarioService usuarioService = new UsuarioService();

        public UsuariosForm()
        {
            InitializeComponent();
        }


        private void UsuariosForm_Load(object sender, EventArgs e)
        {
            CargarRoles();
            CargarUsuarios();
            LimpiarCampos();
        }

        private void CargarRoles()
        {
            cmbRoles.DataSource = usuarioService.ListarRoles();
            cmbRoles.DisplayMember = "NombreRol";
            cmbRoles.ValueMember = "IdRol";
            cmbRoles.SelectedIndex = -1;
        }

        private void CargarUsuarios()
        {
            dgvUsuarios.DataSource = usuarioService.ListarUsuarios();

            dgvUsuarios.Columns["IdUsuario"].Visible = false;

            dgvUsuarios.ReadOnly = true;
            dgvUsuarios.AllowUserToAddRows = false;
            dgvUsuarios.MultiSelect = false;
            dgvUsuarios.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvUsuarios.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
        }


        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                int idRol = cmbRoles.SelectedIndex == -1
                    ? 0
                    : Convert.ToInt32(cmbRoles.SelectedValue);

                usuarioService.GuardarUsuario(
                    txtNombre.Text,
                    txtApellido.Text,
                    txtCorreo.Text,
                    txtPassword.Text,
                    idRol
                );

                MessageBox.Show(
                    "Usuario registrado correctamente.",
                    "Registro exitoso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                CargarUsuarios();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Error al registrar usuario",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private void dgvUsuarios_CellClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewRow fila = dgvUsuarios.Rows[e.RowIndex];

            idUsuarioSeleccionado =
                Convert.ToInt32(fila.Cells["IdUsuario"].Value);

            txtNombre.Text =
                fila.Cells["Nombre"].Value.ToString();

            txtApellido.Text =
                fila.Cells["Apellido"].Value.ToString();

            txtCorreo.Text =
                fila.Cells["Correo"].Value.ToString();

            string nombreRol =
                fila.Cells["Rol"].Value.ToString();

            cmbRoles.Text = nombreRol;

            // No mostramos ni modificamos el hash de la contraseña
            txtPassword.Clear();
        }


        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void LimpiarCampos()
        {
            idUsuarioSeleccionado = 0;

            txtNombre.Clear();
            txtApellido.Clear();
            txtCorreo.Clear();
            txtPassword.Clear();

            cmbRoles.SelectedIndex = -1;
            chkMostrarPassword.Checked = false;

            dgvUsuarios.ClearSelection();

            txtNombre.Focus();
        }

        private void chkMostrarPassword_CheckedChanged(
            object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar =
                !chkMostrarPassword.Checked;
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                if (idUsuarioSeleccionado == 0)
                {
                    MessageBox.Show("Seleccione un usuario para actualizar.");
                    return;
                }

                int idRol = Convert.ToInt32(cmbRoles.SelectedValue);

                usuarioService.ActualizarUsuario(
                    idUsuarioSeleccionado,
                    txtNombre.Text,
                    txtApellido.Text,
                    txtCorreo.Text,
                    idRol
                );

                MessageBox.Show(
                    "Usuario actualizado correctamente.",
                    "Actualización exitosa",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                CargarUsuarios();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Error al actualizar",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (idUsuarioSeleccionado == 0)
            {
                MessageBox.Show("Seleccione un usuario para eliminar.");
                return;
            }

            DialogResult resultado = MessageBox.Show(
                "¿Está seguro de que desea eliminar este usuario?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (resultado != DialogResult.Yes)
                return;

            try
            {
                usuarioService.EliminarUsuario(idUsuarioSeleccionado);

                MessageBox.Show(
                    "Usuario eliminado correctamente.",
                    "Eliminación exitosa",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                CargarUsuarios();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Error al eliminar",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

    }
}
