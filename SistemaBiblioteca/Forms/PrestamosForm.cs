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

        // --- DEFINICIÓN DE CONTROLES (ESTO FALTABA) ---
        // Controles para Registrar Préstamo
        private ComboBox cmbUsuario;
        private ComboBox cmbLibro;
        private DateTimePicker dtpFechaPrestamo;
        private DateTimePicker dtpFechaDevolucion;
        private Button btnRegistrar;
        private Button btnLimpiar;

        // Controles para el Buscador (Panel derecho)
        private ComboBox cmbBuscarPor;
        private TextBox txtBuscar;
        private Button btnBuscar;
        private Button btnVerTodos;

        // La Tabla (DataGridView)
        private DataGridView dgvPrestamos;

        public PrestamosForm()
        {
            // El orden es importante:
            ConstruirInterfazPorCodigo(); // 1. Crea y posiciona los controles
            CargarUsuarios();            // 2. Llena el ComboBox de usuarios
            CargarLibros();              // 3. Llena el ComboBox de libros
            CargarTablaPrestamos();      // 4. Llena la tabla con los préstamos actuales
        }

        // --- MÉTODOS QUE FALTABAN ---

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
            GroupBox gbRegistrar = new GroupBox
            {
                Text = "Registrar préstamo",
                Location = new Point(20, 15),
                Size = new Size(465, 165),
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold)
            };

            Label lblUsuario = new Label { Text = "Usuario:", Location = new Point(15, 30), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Regular) };
            cmbUsuario = new ComboBox { Location = new Point(130, 27), Width = 315, DropDownStyle = ComboBoxStyle.DropDownList };

            Label lblLibro = new Label { Text = "Libro:", Location = new Point(15, 67), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Regular) };
            cmbLibro = new ComboBox { Location = new Point(130, 64), Width = 315, DropDownStyle = ComboBoxStyle.DropDownList };

            Label lblFPrestamo = new Label { Text = "Fecha préstamo:", Location = new Point(15, 102), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Regular) };
            dtpFechaPrestamo = new DateTimePicker { Location = new Point(130, 99), Width = 180, Format = DateTimePickerFormat.Short };

            Label lblFDevolucion = new Label { Text = "Fecha devolución:", Location = new Point(15, 134), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Regular) };
            dtpFechaDevolucion = new DateTimePicker { Location = new Point(130, 131), Width = 180, Format = DateTimePickerFormat.Short };

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
            GroupBox gbBuscar = new GroupBox
            {
                Text = "Buscar préstamo",
                Location = new Point(500, 15),
                Size = new Size(445, 165),
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold)
            };

            Label lblBuscarPor = new Label { Text = "Buscar por:", Location = new Point(15, 30), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Regular) };
            cmbBuscarPor = new ComboBox { Location = new Point(110, 27), Width = 310, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbBuscarPor.Items.Add("Usuario");
            cmbBuscarPor.Items.Add("Libro");
            cmbBuscarPor.Items.Add("Estado");
            cmbBuscarPor.SelectedIndex = 0;

            Label lblIngrese = new Label { Text = "Filtro:", Location = new Point(15, 70), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Regular) };
            txtBuscar = new TextBox { Location = new Point(110, 67), Width = 310 };

            btnBuscar = new Button { Text = "🔍 Buscar", Location = new Point(110, 112), Width = 120, Height = 35, BackColor = Color.FromArgb(0, 120, 215), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnVerTodos = new Button { Text = "Ver todos", Location = new Point(240, 112), Width = 100, Height = 35, FlatStyle = FlatStyle.Standard };

            // Aquí conectamos los eventos para los botones del buscador
            btnBuscar.Click += new EventHandler(this.btnBuscar_Click);
            btnVerTodos.Click += new EventHandler(this.btnVerTodos_Click);

            gbBuscar.Controls.Add(lblBuscarPor);
            gbBuscar.Controls.Add(cmbBuscarPor);
            gbBuscar.Controls.Add(lblIngrese);
            gbBuscar.Controls.Add(txtBuscar);
            gbBuscar.Controls.Add(btnBuscar);
            gbBuscar.Controls.Add(btnVerTodos);


            // ==========================================
            // BOTONES PRINCIPALES DE ACCIÓN
            // ==========================================
            btnRegistrar = new Button { Text = "Registrar préstamo", Location = new Point(20, 192), Width = 150, Height = 35, BackColor = Color.FromArgb(0, 120, 215), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnLimpiar = new Button { Text = "Limpiar", Location = new Point(180, 192), Width = 90, Height = 35, FlatStyle = FlatStyle.Standard };

            // Aquí conectamos los eventos para los botones principales
            btnRegistrar.Click += new EventHandler(this.btnRegistrar_Click);
            btnLimpiar.Click += new EventHandler(this.btnLimpiar_Click);


            // ==========================================
            // TABLA: PRÉSTAMOS REGISTRADOS
            // ==========================================
            Label lblTituloTabla = new Label { Text = "Préstamos registrados", Location = new Point(20, 240), Font = new Font("Segoe UI", 11, FontStyle.Bold), AutoSize = true };
            dgvPrestamos = new DataGridView { Location = new Point(20, 275), Size = new Size(925, 245), ReadOnly = true, SelectionMode = DataGridViewSelectionMode.FullRowSelect, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
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

        private void CargarUsuarios()
        {
            // Lógica para cargar usuarios desde la base de datos a cmbUsuario
            // (Similar a CargarTablaPrestamos, pero para el ComboBox)
        }

        private void CargarLibros()
        {
            // Lógica para cargar libros desde la base de datos a cmbLibro
            // (Similar a CargarTablaPrestamos, pero para el ComboBox)
        }

        private void CargarTablaPrestamos()
        {
            try
            {
                using SqlConnection conexion = conexionBD.ObtenerConexion();
                conexion.Open();
                string sql = "SELECT * FROM Prestamos"; // Reemplaza por tu consulta real con JOINS
                SqlDataAdapter da = new SqlDataAdapter(sql, conexion);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvPrestamos.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar préstamos: " + ex.Message);
            }
        }

        private void LimpiarCampos()
        {
            cmbUsuario.SelectedIndex = -1;
            cmbLibro.SelectedIndex = -1;
            dtpFechaPrestamo.Value = DateTime.Now;
            dtpFechaDevolucion.Value = DateTime.Now.AddDays(7); // Fecha de devolución por defecto a una semana
            if (txtBuscar != null) txtBuscar.Clear();
        }

        // --- MANEJADORES DE EVENTOS DE CLIC ---

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            // Implementar lógica para registrar el préstamo en la base de datos
            MessageBox.Show("Lógica para registrar préstamo no implementada aún.");
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            // Implementar lógica de búsqueda y filtrado de préstamos
            MessageBox.Show("Lógica para buscar préstamos no implementada aún.");
        }

        private void btnVerTodos_Click(object sender, EventArgs e)
        {
            txtBuscar.Clear();
            CargarTablaPrestamos();
        }
    }
}