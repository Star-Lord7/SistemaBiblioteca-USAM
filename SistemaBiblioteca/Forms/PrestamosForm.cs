using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using SistemaBiblioteca.Data;

namespace SistemaBiblioteca.Forms
{
    public partial class PrestamosForm : Form
    {
        private readonly ConexionBD conexionBD = new();

        private ComboBox cmbUsuario;
        private ComboBox cmbLibro;
        private DateTimePicker dtpFechaPrestamo;
        private DateTimePicker dtpFechaDevolucion;
        private Button btnRegistrar;
        private Button btnLimpiar;

        // Controles para el buscador (Panel derecho)
        private ComboBox cmbBuscarPor;
        private TextBox txtBuscar;
        private Button btnBuscar;
        private Button btnVerTodos;

        private DataGridView dgvPrestamos;

        public PrestamosForm()
        {
            ConstruirInterfazPorCodigo();
        }

        private void ConstruirInterfazPorCodigo()
        {
            Text = "Sistema de Biblioteca - Gestión de Préstamos";
            BackColor = Color.White;
            Width = 985;
            Height = 580;
            StartPosition = FormStartPosition.CenterScreen;

            // ==========================================
            // PANEL IZQUIERDO: REGISTRAR PRÉSTAMO
            // ==========================================
            GroupBox gbRegistrar = new()
            {
                Text = "Registrar préstamo",
                Location = new Point(20, 15),
                Size = new Size(465, 165),
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold)
            };

            Label lblUsuario = new() { Text = "Usuario:", Location = new Point(15, 30), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Regular) };
            cmbUsuario = new() { Location = new Point(130, 27), Width = 315, DropDownStyle = ComboBoxStyle.DropDownList };

            Label lblLibro = new() { Text = "Libro:", Location = new Point(15, 67), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Regular) };
            cmbLibro = new() { Location = new Point(130, 64), Width = 315, DropDownStyle = ComboBoxStyle.DropDownList };

            Label lblFPrestamo = new() { Text = "Fecha préstamo:", Location = new Point(15, 102), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Regular) };
            dtpFechaPrestamo = new() { Location = new Point(130, 99), Width = 180, Format = DateTimePickerFormat.Short };

            Label lblFDevolucion = new() { Text = "Fecha devolución:", Location = new Point(15, 134), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Regular) };
            dtpFechaDevolucion = new() { Location = new Point(130, 131), Width = 180, Format = DateTimePickerFormat.Short };

            gbRegistrar.Controls.Add(lblUsuario);
            gbRegistrar.Controls.Add(cmbUsuario);
            gbRegistrar.Controls.Add(lblLibro);
            gbRegistrar.Controls.Add(cmbLibro);
            gbRegistrar.Controls.Add(lblFPrestamo);
            gbRegistrar.Controls.Add(dtpFechaPrestamo);
            gbRegistrar.Controls.Add(lblFDevolucion);
            gbRegistrar.Controls.Add(dtpFechaDevolucion);


            // ==========================================
            // PANEL DERECHO: BUSCAR PRÉSTAMO
            // ==========================================
            GroupBox gbBuscar = new()
            {
                Text = "Buscar préstamo",
                Location = new Point(500, 15),
                Size = new Size(445, 165),
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold)
            };

            Label lblBuscarPor = new() { Text = "Buscar por:", Location = new Point(15, 30), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Regular) };
            cmbBuscarPor = new() { Location = new Point(110, 27), Width = 310, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbBuscarPor.Items.Add("Usuario");
            cmbBuscarPor.Items.Add("Libro");
            cmbBuscarPor.Items.Add("Estado");
            cmbBuscarPor.SelectedIndex = 0;

            Label lblIngrese = new() { Text = "Filtro:", Location = new Point(15, 70), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Regular) };
            txtBuscar = new() { Location = new Point(110, 67), Width = 310 };

            btnBuscar = new()
            {
                Text = "🔍 Buscar",
                Location = new Point(110, 112),
                Width = 120,
                Height = 35,
                BackColor = Color.FromArgb(0, 120, 215),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnBuscar.Click += btnBuscar_Click;

            btnVerTodos = new()
            {
                Text = "Ver todos",
                Location = new Point(240, 112),
                Width = 100,
                Height = 35,
                FlatStyle = FlatStyle.Standard
            };
            btnVerTodos.Click += btnVerTodos_Click;

            gbBuscar.Controls.Add(lblBuscarPor);
            gbBuscar.Controls.Add(cmbBuscarPor);
            gbBuscar.Controls.Add(lblIngrese);
            gbBuscar.Controls.Add(txtBuscar);
            gbBuscar.Controls.Add(btnBuscar);
            gbBuscar.Controls.Add(btnVerTodos);


            // ==========================================
            // BOTONES PRINCIPALES DE ACCIÓN
            // ==========================================
            btnRegistrar = new()
            {
                Text = "Registrar préstamo",
                Location = new Point(20, 192),
                Width = 150,
                Height = 35,
                BackColor = Color.FromArgb(0, 120, 215),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnRegistrar.Click += btnRegistrar_Click;

            btnLimpiar = new()
            {
                Text = "Limpiar",
                Location = new Point(180, 192),
                Width = 90,
                Height = 35,
                FlatStyle = FlatStyle.Standard
            };
            btnLimpiar.Click += btnLimpiar_Click;


            // ==========================================
            // TABLA: PRÉSTAMOS REGISTRADOS
            // ==========================================
            Label lblTituloTabla = new()
            {
                Text = "Préstamos registrados",
                Location = new Point(20, 240),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                AutoSize = true
            };

            dgvPrestamos = new()
            {
                Location = new Point(20, 275),
                Size = new Size(925, 245),
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            // Aplicar estilo visual moderno a la tabla
            EstilizarTablaPrestamos();

            // Agregar controles al formulario
            Controls.Add(gbRegistrar);
            Controls.Add(gbBuscar);
            Controls.Add(btnRegistrar);
            Controls.Add(btnLimpiar);
            Controls.Add(lblTituloTabla);
            Controls.Add(dgvPrestamos);
        }

        private void EstilizarTablaPrestamos()
        {
            dgvPrestamos.BackgroundColor = Color.White;
            dgvPrestamos.BorderStyle = BorderStyle.None;
            dgvPrestamos.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvPrestamos.RowHeadersVisible = false;
            dgvPrestamos.AllowUserToAddRows = false;

            // Encabezados en tono azul acorde a la interfaz
            dgvPrestamos.EnableHeadersVisualStyles = false;
            dgvPrestamos.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvPrestamos.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 32, 96);
            dgvPrestamos.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvPrestamos.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            dgvPrestamos.ColumnHeadersHeight = 35;

            // Estilos de filas y selección
            dgvPrestamos.DefaultCellStyle.SelectionBackColor = Color.FromArgb(220, 230, 242);
            dgvPrestamos.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvPrestamos.DefaultCellStyle.Font = new Font("Segoe UI", 9f);
            dgvPrestamos.RowTemplate.Height = 30;
        }

        private void PrestamosForm_Load(object sender, EventArgs e)
        {
            if (DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            CargarUsuarios();
            CargarLibros();
            CargarTablaPrestamos();
        }

        private void CargarUsuarios()
        {
            try
            {
                using SqlConnection conexion = conexionBD.ObtenerConexion();
                string sql = "SELECT IdUsuario, Nombre + ' ' + ISNULL(Apellido, '') AS NombreCompleto FROM Usuarios";
                SqlDataAdapter da = new(sql, conexion);
                DataTable dt = new();
                da.Fill(dt);

                cmbUsuario.DataSource = dt;
                cmbUsuario.DisplayMember = "NombreCompleto";
                cmbUsuario.ValueMember = "IdUsuario";
                cmbUsuario.SelectedIndex = -1;
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
                using SqlConnection conexion = conexionBD.ObtenerConexion();
                string sql = "SELECT IdLibro, Titulo FROM Libro";
                SqlDataAdapter da = new(sql, conexion);
                DataTable dt = new();
                da.Fill(dt);

                cmbLibro.DataSource = dt;
                cmbLibro.DisplayMember = "Titulo";
                cmbLibro.ValueMember = "IdLibro";
                cmbLibro.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar libros: " + ex.Message);
            }
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (cmbUsuario.SelectedIndex == -1 || cmbUsuario.SelectedValue == null)
            {
                MessageBox.Show("Por favor, seleccione un usuario de la lista.");
                return;
            }

            if (cmbLibro.SelectedIndex == -1 || cmbLibro.SelectedValue == null)
            {
                MessageBox.Show("Por favor, seleccione un libro de la lista.");
                return;
            }

            int idUsuario = Convert.ToInt32(cmbUsuario.SelectedValue);
            int idLibro = Convert.ToInt32(cmbLibro.SelectedValue);
            DateTime fechaPrestamo = dtpFechaPrestamo.Value;
            DateTime fechaDevolucion = dtpFechaDevolucion.Value;

            try
            {
                using SqlConnection conexion = conexionBD.ObtenerConexion();
                string sql = @"INSERT INTO Prestamos (IdUsuario, IdLibro, FechaPrestamo, FechaDevolucion, Estado) 
                               VALUES (@IdUsuario, @IdLibro, @FechaPrestamo, @FechaDevolucion, 'En curso')";

                SqlCommand cmd = new(sql, conexion);
                cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                cmd.Parameters.AddWithValue("@IdLibro", idLibro);
                cmd.Parameters.AddWithValue("@FechaPrestamo", fechaPrestamo);
                cmd.Parameters.AddWithValue("@FechaDevolucion", fechaDevolucion);

                conexion.Open();
                cmd.ExecuteNonQuery();

                MessageBox.Show("¡Préstamo registrado exitosamente!");
                LimpiarCampos();
                CargarTablaPrestamos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar préstamo: " + ex.Message);
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string criterio = cmbBuscarPor.SelectedItem?.ToString() ?? "Usuario";
            string textoBusqueda = txtBuscar.Text.Trim();

            try
            {
                using SqlConnection conexion = conexionBD.ObtenerConexion();
                string sql = @"SELECT P.IdPrestamo AS ID, 
                                     U.Nombre + ' ' + ISNULL(U.Apellido, '') AS Usuario, 
                                     L.Titulo AS Libro, 
                                     P.FechaPrestamo AS [Fecha préstamo], 
                                     P.FechaDevolucion AS [Fecha devolución], 
                                     P.Estado 
                              FROM Prestamos P
                              INNER JOIN Usuarios U ON P.IdUsuario = U.IdUsuario
                              INNER JOIN Libro L ON P.IdLibro = L.IdLibro
                              WHERE ";

                if (criterio == "Usuario")
                {
                    sql += "(U.Nombre LIKE @filtro OR U.Apellido LIKE @filtro)";
                }
                else if (criterio == "Libro")
                {
                    sql += "L.Titulo LIKE @filtro";
                }
                else
                {
                    sql += "P.Estado LIKE @filtro";
                }

                sql += " ORDER BY P.IdPrestamo DESC";

                SqlCommand cmd = new(sql, conexion);
                cmd.Parameters.AddWithValue("@filtro", "%" + textoBusqueda + "%");

                SqlDataAdapter da = new(cmd);
                DataTable dt = new();
                da.Fill(dt);
                dgvPrestamos.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar préstamos: " + ex.Message);
            }
        }

        private void btnVerTodos_Click(object sender, EventArgs e)
        {
            txtBuscar.Clear();
            CargarTablaPrestamos();
        }

        private void CargarTablaPrestamos()
        {
            try
            {
                using SqlConnection conexion = conexionBD.ObtenerConexion();
                string sql = @"SELECT P.IdPrestamo AS ID, 
                                     U.Nombre + ' ' + ISNULL(U.Apellido, '') AS Usuario, 
                                     L.Titulo AS Libro, 
                                     P.FechaPrestamo AS [Fecha préstamo], 
                                     P.FechaDevolucion AS [Fecha devolución], 
                                     P.Estado 
                              FROM Prestamos P
                              INNER JOIN Usuarios U ON P.IdUsuario = U.IdUsuario
                              INNER JOIN Libro L ON P.IdLibro = L.IdLibro
                              ORDER BY P.IdPrestamo DESC";

                SqlDataAdapter da = new(sql, conexion);
                DataTable dt = new();
                da.Fill(dt);
                dgvPrestamos.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar la lista de préstamos: " + ex.Message);
            }
        }

        private void LimpiarCampos()
        {
            if (cmbUsuario != null && cmbUsuario.Items.Count > 0) cmbUsuario.SelectedIndex = -1;
            if (cmbLibro != null && cmbLibro.Items.Count > 0) cmbLibro.SelectedIndex = -1;
            if (dtpFechaPrestamo != null) dtpFechaPrestamo.Value = DateTime.Now;
            if (dtpFechaDevolucion != null) dtpFechaDevolucion.Value = DateTime.Now.AddDays(7);
            if (txtBuscar != null) txtBuscar.Clear();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            CargarTablaPrestamos();
        }
    }
}