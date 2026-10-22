
using System.Drawing;
using System.Windows.Forms;

namespace SistemaBiblioteca.Styles
{
    public static class Estilos
    {
        public static readonly Color AzulPrincipal =
            ColorTranslator.FromHtml("#14213D");

        public static readonly Color AzulMenu =
            ColorTranslator.FromHtml("#1E293B");

        public static readonly Color AzulAccion =
            ColorTranslator.FromHtml("#2563EB");

        public static readonly Color Verde =
            ColorTranslator.FromHtml("#16A34A");

        public static readonly Color Rojo =
            ColorTranslator.FromHtml("#DC2626");

        public static readonly Color Gris =
            ColorTranslator.FromHtml("#64748B");

        public static readonly Color Fondo =
            ColorTranslator.FromHtml("#F1F5F9");

        public static void EstilizarBoton(
            Button boton, Color color)
        {
            boton.BackColor = color;
            boton.ForeColor = Color.White;

            boton.FlatStyle = FlatStyle.Flat;
            boton.FlatAppearance.BorderSize = 0;

            boton.Font = new Font(
                "Segoe UI", 10, FontStyle.Bold);

            boton.Cursor = Cursors.Hand;
            boton.Height = 38;

            boton.UseVisualStyleBackColor = false;
        }

        public static void EstilizarTabla(DataGridView tabla)
        {
            tabla.BackgroundColor = Color.White;
            tabla.BorderStyle = BorderStyle.None;

            tabla.EnableHeadersVisualStyles = false;

            tabla.ColumnHeadersDefaultCellStyle.BackColor =
                AzulPrincipal;

            tabla.ColumnHeadersDefaultCellStyle.ForeColor =
                Color.White;

            tabla.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 10, FontStyle.Bold);

            tabla.ColumnHeadersHeight = 40;

            tabla.DefaultCellStyle.Font =
                new Font("Segoe UI", 10);

            tabla.DefaultCellStyle.SelectionBackColor =
                AzulAccion;

            tabla.DefaultCellStyle.SelectionForeColor =
                Color.White;

            tabla.AlternatingRowsDefaultCellStyle.BackColor =
                ColorTranslator.FromHtml("#F8FAFC");

            tabla.RowTemplate.Height = 32;

            tabla.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            tabla.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            tabla.ReadOnly = true;
            tabla.MultiSelect = false;
            tabla.AllowUserToAddRows = false;
        }
    }
}
