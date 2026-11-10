using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using SistemaBiblioteca.Data;
using SistemaBiblioteca.Services;

namespace SistemaBiblioteca.Forms
{
    public partial class PrestamosForm : Form
    {
        private readonly ConexionBD conexionBD = new();
        private readonly PrestamoService prestamoService =
    new PrestamoService();

        // --- DEFINICIÓN DE CONTROLES (ESTO FALTABA) ---
        // Controles para Registrar Préstamo
        private ComboBox cmbUsuario;
        private ComboBox cmbLibro;
        private ComboBox cmbSucursal;
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
            InitializeComponent();
            // El orden es importante:
            ConstruirInterfazPorCodigo(); // 1. Crea y posiciona los controles
            CargarUsuarios();            // 2. Llena el ComboBox de usuarios
            CargarLibros();
            CargarSucursales();// 3. Llena el ComboBox de libros
            CargarTablaPrestamos();      // 4. Llena la tabla con los préstamos actuales
            LimpiarCampos();
        }

        // --- MÉTODOS QUE FALTABAN ---

        private void ConstruirInterfazPorCodigo()
        {
            Text = "Sistema de Biblioteca - Gestión de Préstamos";
            BackColor = Color.White;
            Size = new Size(907, 712);
            Dock = DockStyle.Fill;
            FormBorderStyle = FormBorderStyle.None;
            //BackColor = Color.WhiteSmoke;
            AutoScroll = true;

            // ==========================================
            // PANEL IZQUIERDO: REGISTRAR PRÉSTAMO
            // ==========================================
            GroupBox gbRegistrar = new GroupBox
            {
                Text = "Registrar préstamo",
                Location = new Point(15, 15),
                Size = new Size(425, 220),
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold)
            };

            Label lblUsuario = new Label { Text = "Usuario:", Location = new Point(15, 30), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Regular) };
            cmbUsuario = new ComboBox { Location = new Point(130, 27), Width = 315, DropDownStyle = ComboBoxStyle.DropDownList };

            Label lblLibro = new Label { Text = "Libro:", Location = new Point(15, 67), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Regular) };
            cmbLibro = new ComboBox { Location = new Point(130, 64), Width = 315, DropDownStyle = ComboBoxStyle.DropDownList };

            Label lblSucursal = new Label
            {
                Text = "Sucursal:",
                Location = new Point(15, 102),
                AutoSize = true
            };

            cmbSucursal = new ComboBox
            {
                Location = new Point(130, 99),
                Width = 270,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            Label lblFPrestamo = new Label { Text = "Fecha préstamo:", Location = new Point(15, 139), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Regular) };
            dtpFechaPrestamo = new DateTimePicker { Location = new Point(130, 136), Width = 180, Format = DateTimePickerFormat.Short };

            Label lblFDevolucion = new Label { Text = "Fecha devolución:", Location = new Point(15, 176), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Regular) };
            dtpFechaDevolucion = new DateTimePicker { Location = new Point(130, 173), Width = 180, Format = DateTimePickerFormat.Short };

            gbRegistrar.Controls.Add(lblUsuario);
            gbRegistrar.Controls.Add(cmbUsuario);
            gbRegistrar.Controls.Add(lblLibro);
            gbRegistrar.Controls.Add(cmbLibro);
            gbRegistrar.Controls.Add(lblSucursal);
            gbRegistrar.Controls.Add(cmbSucursal);
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
                Location = new Point(455, 15),
                Size = new Size(425, 220),
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
            btnRegistrar = new Button { Text = "Registrar préstamo", Location = new Point(15, 250), Width = 150, Height = 35, BackColor = Color.FromArgb(0, 120, 215), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnLimpiar = new Button { Text = "Limpiar", Location = new Point(190, 250), Width = 90, Height = 35, FlatStyle = FlatStyle.Standard };

            // Aquí conectamos los eventos para los botones principales
            btnRegistrar.Click += new EventHandler(this.btnRegistrar_Click);
            btnLimpiar.Click += new EventHandler(this.btnLimpiar_Click);


            // ==========================================
            // TABLA: PRÉSTAMOS REGISTRADOS
            // ==========================================
            Label lblTituloTabla = new Label { Text = "Préstamos registrados", Location = new Point(15, 310), Font = new Font("Segoe UI", 11, FontStyle.Bold), AutoSize = true };
            dgvPrestamos = new DataGridView { Location = new Point(15, 345), Size = new Size(865, 340), ReadOnly = true, SelectionMode = DataGridViewSelectionMode.FullRowSelect, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
            dgvPrestamos.Anchor =
    AnchorStyles.Top |
    AnchorStyles.Bottom |
    AnchorStyles.Left |
    AnchorStyles.Right;
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
            cmbUsuario.DataSource = prestamoService.ObtenerLectores();
            cmbUsuario.DisplayMember = "Nombre_Lector";
            cmbUsuario.ValueMember = "Num_Tarjeta";
            cmbUsuario.SelectedIndex = -1;
        }

        private void CargarLibros()
        {
            cmbLibro.DataSource = null;

            cmbLibro.DisplayMember = "Titulo";
            cmbLibro.ValueMember = "Id_Libro";

            cmbLibro.DataSource = prestamoService.ObtenerLibros();

            cmbLibro.SelectedIndex = -1;
        }

        private void CargarSucursales()
        {
            cmbSucursal.DataSource = null;

            cmbSucursal.DisplayMember = "Nombre_Sucursal";
            cmbSucursal.ValueMember = "Id_Sucursal";

            cmbSucursal.DataSource = prestamoService.ObtenerSucursales();

            cmbSucursal.SelectedIndex = -1;
        }

        private void CargarTablaPrestamos()
        {
            try
            {
                dgvPrestamos.DataSource =
                    prestamoService.ObtenerPrestamos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al cargar préstamos: " + ex.Message);
            }
        }

        private void LimpiarCampos()
        {
            cmbUsuario.SelectedIndex = -1;
            cmbLibro.SelectedIndex = -1;
            cmbSucursal.SelectedIndex = -1;

            dtpFechaPrestamo.Value = DateTime.Today;
            dtpFechaDevolucion.Value = DateTime.Today.AddDays(7);

            txtBuscar.Clear();
        }

        // --- MANEJADORES DE EVENTOS DE CLIC ---

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbUsuario.SelectedIndex == -1 ||
                    cmbLibro.SelectedIndex == -1 ||
                    cmbSucursal.SelectedIndex == -1)
                {
                    MessageBox.Show(
                        "Seleccione un lector, un libro y una sucursal.");
                    return;
                }

                prestamoService.RegistrarPrestamo(
                    Convert.ToInt32(cmbLibro.SelectedValue),
                    Convert.ToInt32(cmbSucursal.SelectedValue),
                    cmbUsuario.SelectedValue.ToString(),
                    dtpFechaPrestamo.Value.Date,
                    dtpFechaDevolucion.Value.Date
                );

                MessageBox.Show(
                    "Préstamo registrado correctamente.",
                    "Registro exitoso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                CargarTablaPrestamos();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Error al registrar préstamo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtBuscar.Text))
                {
                    CargarTablaPrestamos();
                    return;
                }

                string criterio = cmbBuscarPor.Text;

                if (criterio == "Estado")
                {
                    MessageBox.Show(
                        "La búsqueda por estado estará disponible cuando se implemente el control de devoluciones.");
                    return;
                }

                dgvPrestamos.DataSource =
                    prestamoService.BuscarPrestamos(
                        criterio,
                        txtBuscar.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al buscar préstamos: " + ex.Message);
            }
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // PrestamosForm
            // 
            ClientSize = new Size(282, 253);
            Name = "PrestamosForm";
            ResumeLayout(false);

        }

        private void btnVerTodos_Click(object sender, EventArgs e)
        {
            txtBuscar.Clear();
            CargarTablaPrestamos();
        }

        private void PrestamosForm_Load(object sender, EventArgs e)
        {

        }
    }
}