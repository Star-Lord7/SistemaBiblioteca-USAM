using SistemaBiblioteca.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemaBiblioteca.Forms
{
    public class PrestamosForm : Form
    {
        // Controles de Registro
        private GroupBox grpRegistrar;
        private Label lblUsuario;
        private ComboBox cmbUsuarios;
        private Label lblLibro;
        private ComboBox cmbLibros;
        private Label lblFechaPrestamo;
        private DateTimePicker dtpFechaPrestamo;
        private Label lblFechaDevolucion;
        private DateTimePicker dtpFechaDevolucion;
        private Button btnRegistrar;
        private Button btnLimpiar;

        // Controles de Tabla (Préstamos registrados)
        private Label lblTituloTabla;
        private DataGridView dgvPrestamos;

        public PrestamosForm()
        {
            // Configuración principal de la ventana
            this.Text = "Sistema de Biblioteca - Préstamos";
            this.Size = new Size(850, 650);
            this.BackColor = Color.WhiteSmoke;
            this.StartPosition = FormStartPosition.CenterScreen;

            InicializarControles();
            CargarUsuarios();
            CargarLibros();
            CargarPrestamosRegistrados();
        }

        private void InicializarControles()
        {
            // === GRUPO: REGISTRAR PRÉSTAMO ===
            grpRegistrar = new GroupBox();
            grpRegistrar.Text = "Registrar préstamo";
            grpRegistrar.Location = new Point(20, 20);
            grpRegistrar.Size = new Size(790, 220);
            this.Controls.Add(grpRegistrar);

            // Usuario
            lblUsuario = new Label() { Text = "Usuario:", Location = new Point(20, 35), AutoSize = true };
            cmbUsuarios = new ComboBox() { Location = new Point(130, 32), Width = 250, DropDownStyle = ComboBoxStyle.DropDownList };
            grpRegistrar.Controls.Add(lblUsuario);
            grpRegistrar.Controls.Add(cmbUsuarios);

            // Libro
            lblLibro = new Label() { Text = "Libro:", Location = new Point(20, 75), AutoSize = true };
            cmbLibros = new ComboBox() { Location = new Point(130, 72), Width = 250, DropDownStyle = ComboBoxStyle.DropDownList };
            grpRegistrar.Controls.Add(lblLibro);
            grpRegistrar.Controls.Add(cmbLibros);

            // Fecha Préstamo
            lblFechaPrestamo = new Label() { Text = "Fecha préstamo:", Location = new Point(420, 35), AutoSize = true };
            dtpFechaPrestamo = new DateTimePicker() { Location = new Point(540, 32), Width = 220, Format = DateTimePickerFormat.Short };
            grpRegistrar.Controls.Add(lblFechaPrestamo);
            grpRegistrar.Controls.Add(dtpFechaPrestamo);

            // Fecha Devolución
            lblFechaDevolucion = new Label() { Text = "Fecha devolución:", Location = new Point(420, 75), AutoSize = true };
            dtpFechaDevolucion = new DateTimePicker() { Location = new Point(540, 72), Width = 220, Format = DateTimePickerFormat.Short };
            grpRegistrar.Controls.Add(lblFechaDevolucion);
            grpRegistrar.Controls.Add(dtpFechaDevolucion);

            // Botón Registrar Préstamo
            btnRegistrar = new Button() { Text = "Registrar préstamo", Location = new Point(130, 135), Width = 150, Height = 35 };
            btnRegistrar.BackColor = Color.FromArgb(0, 122, 204);
            btnRegistrar.ForeColor = Color.White;
            btnRegistrar.FlatStyle = FlatStyle.Flat;
            btnRegistrar.Click += BtnRegistrar_Click;
            grpRegistrar.Controls.Add(btnRegistrar);

            // Botón Limpiar
            btnLimpiar = new Button() { Text = "Limpiar", Location = new Point(290, 135), Width = 90, Height = 35 };
            btnLimpiar.BackColor = Color.LightGray;
            btnLimpiar.FlatStyle = FlatStyle.Flat;
            btnLimpiar.Click += BtnLimpiar_Click;
            grpRegistrar.Controls.Add(btnLimpiar);

            // === SECCIÓN: PRÉSTAMOS REGISTRADOS ===
            lblTituloTabla = new Label() { Text = "Préstamos registrados", Location = new Point(20, 260), Font = new Font("Segoe UI", 11, FontStyle.Bold), AutoSize = true };
            this.Controls.Add(lblTituloTabla);

            dgvPrestamos = new DataGridView();
            dgvPrestamos.Location = new Point(20, 290);
            dgvPrestamos.Size = new Size(790, 280);
            dgvPrestamos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPrestamos.ReadOnly = true;
            dgvPrestamos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.Controls.Add(dgvPrestamos);
        }

        private void CargarUsuarios()
        {
            try
            {
                ConexionBD conexionBD = new ConexionBD();
                using (SqlConnection conexion = conexionBD.ObtenerConexion())
                {
                    string query = "SELECT Id, Nombre FROM Usuarios";
                    SqlCommand cmd = new SqlCommand(query, conexion);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cmbUsuarios.DataSource = dt;
                    cmbUsuarios.DisplayMember = "Nombre";
                    cmbUsuarios.ValueMember = "Id";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar usuarios: " + ex.Message);
            }
        }

        private void CargarLibros()
        {
            try
            {
                ConexionBD conexionBD = new ConexionBD();
                using (SqlConnection conexion = conexionBD.ObtenerConexion())
                {
                    string query = "SELECT Id, Titulo FROM Libros";
                    SqlCommand cmd = new SqlCommand(query, conexion);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cmbLibros.DataSource = dt;
                    cmbLibros.DisplayMember = "Titulo";
                    cmbLibros.ValueMember = "Id";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar libros: " + ex.Message);
            }
        }

        private void CargarPrestamosRegistrados()
        {
            // Aquí puedes conectar luego con tu tabla de préstamos en la BD para listar el historial
            DataTable dtPrueba = new DataTable();
            dtPrueba.Columns.Add("ID");
            dtPrueba.Columns.Add("Usuario");
            dtPrueba.Columns.Add("Libro");
            dtPrueba.Columns.Add("Fecha préstamo");
            dtPrueba.Columns.Add("Fecha devolución");
            dtPrueba.Columns.Add("Estado");

            // Datos de ejemplo para que veas la tabla idéntica al esquema
            dtPrueba.Rows.Add("1", "Ana López", "Cien años de soledad", "15/09/2025", "29/09/2025", "Devuelto");
            dtPrueba.Rows.Add("2", "Carlos Ramírez", "El principito", "20/09/2025", "04/10/2025", "En curso");

            dgvPrestamos.DataSource = dtPrueba;
        }

        private void BtnRegistrar_Click(object sender, EventArgs e)
        {
            if (cmbUsuarios.SelectedValue == null || cmbLibros.SelectedValue == null)
            {
                MessageBox.Show("Por favor seleccione un usuario y un libro.");
                return;
            }

            int idUsuario = Convert.ToInt32(cmbUsuarios.SelectedValue);
            int idLibro = Convert.ToInt32(cmbLibros.SelectedValue);
            string fechaP = dtpFechaPrestamo.Value.ToShortDateString();
            string fechaD = dtpFechaDevolucion.Value.ToShortDateString();

            MessageBox.Show($"¡Préstamo registrado con éxito!\nUsuario ID: {idUsuario}\nLibro ID: {idLibro}\nFecha Préstamo: {fechaP}\nFecha Devolución: {fechaD}");
        }

        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            if (cmbUsuarios.Items.Count > 0) cmbUsuarios.SelectedIndex = 0;
            if (cmbLibros.Items.Count > 0) cmbLibros.SelectedIndex = 0;
            dtpFechaPrestamo.Value = DateTime.Now;
            dtpFechaDevolucion.Value = DateTime.Now.AddDays(14);
        }
    }
}