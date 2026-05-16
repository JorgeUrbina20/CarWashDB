using System;
using System.Drawing;
using System.Windows.Forms;

namespace CarWashApp
{
    public partial class OwnerForm : Form
    {
        private Panel panelMenu;
        private Panel panelContenido;
        private Button btnEmpleados;
        private Button btnInventario;
        private Button btnReportes;
        private Button btnSalir;
        private Label lblTitulo;

        public OwnerForm(string nombreEmpleado)
        {
            ConfigurarVentana(nombreEmpleado);
            InicializarComponentes();
        }

        private void ConfigurarVentana(string nombreEmpleado)
        {
            this.Text = $"CarWash Pro | Administración - {nombreEmpleado}";
            this.Size = new Size(1100, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(245, 245, 250);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Font = new Font("Segoe UI", 10);
        }

        private void InicializarComponentes()
        {
            // Menú lateral
            panelMenu = new Panel()
            {
                Width = 220,
                Height = this.ClientSize.Height,
                BackColor = Color.FromArgb(30, 30, 40)
            };
            this.Controls.Add(panelMenu);

            // Logo / título del menú
            Label lblLogo = new Label()
            {
                Text = "CarWash Pro",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Location = new Point(20, 30),
                AutoSize = true
            };
            panelMenu.Controls.Add(lblLogo);

            // Botones del menú
            btnEmpleados = CrearBotonMenu("Empleados", 0);
            btnInventario = CrearBotonMenu("Inventario", 1);
            btnReportes = CrearBotonMenu("Reportes", 2);
            btnSalir = CrearBotonMenu("Salir", 3);
            btnSalir.BackColor = Color.FromArgb(200, 50, 50);
            btnSalir.Click += (s, e) => this.Close();

            panelMenu.Controls.Add(btnEmpleados);
            panelMenu.Controls.Add(btnInventario);
            panelMenu.Controls.Add(btnReportes);
            panelMenu.Controls.Add(btnSalir);

            // Panel de contenido
            panelContenido = new Panel()
            {
                Location = new Point(panelMenu.Width, 0),
                Size = new Size(this.ClientSize.Width - panelMenu.Width, this.ClientSize.Height),
                BackColor = Color.White
            };
            this.Controls.Add(panelContenido);

            // Título dentro del contenido
            lblTitulo = new Label()
            {
                Text = "Panel de Control",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 30, 40),
                Location = new Point(30, 30),
                AutoSize = true
            };
            panelContenido.Controls.Add(lblTitulo);

            // Tarjetas de acceso rápido
            AgregarTarjeta("Empleados activos", "4", 30, 100, Color.FromArgb(52, 152, 219));
            AgregarTarjeta("Órdenes hoy", "8", 280, 100, Color.FromArgb(46, 204, 113));
            AgregarTarjeta("Ingresos hoy", "$1,200", 530, 100, Color.FromArgb(155, 89, 182));

            // Eventos de los botones del menú
            btnEmpleados.Click += (s, e) => MostrarContenido("Gestión de Empleados");
            btnInventario.Click += (s, e) => MostrarContenido("Control de Inventario");
            btnReportes.Click += (s, e) => MostrarContenido("Reportes y Estadísticas");
        }

        private Button CrearBotonMenu(string texto, int posicion)
        {
            Button btn = new Button()
            {
                Text = texto,
                Width = panelMenu.Width - 20,
                Height = 45,
                Location = new Point(10, 100 + posicion * 55),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 11)
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(50, 50, 70);
            return btn;
        }

        private void AgregarTarjeta(string titulo, string valor, int x, int y, Color color)
        {
            Panel tarjeta = new Panel()
            {
                Location = new Point(x, y),
                Size = new Size(220, 120),
                BackColor = color,
                BorderStyle = BorderStyle.None
            };
            // Sombra simulada con un panel detrás (opcional)
            tarjeta.Paint += (s, e) => ControlPaint.DrawBorder(e.Graphics, tarjeta.ClientRectangle, Color.FromArgb(50, 0, 0, 0), ButtonBorderStyle.Solid);

            Label lblTituloTarjeta = new Label()
            {
                Text = titulo,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10),
                Location = new Point(15, 15),
                AutoSize = true
            };
            Label lblValorTarjeta = new Label()
            {
                Text = valor,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                Location = new Point(15, 45),
                AutoSize = true
            };
            tarjeta.Controls.Add(lblTituloTarjeta);
            tarjeta.Controls.Add(lblValorTarjeta);
            panelContenido.Controls.Add(tarjeta);
        }

        private void MostrarContenido(string texto)
        {
            // Limpia el contenido anterior y muestra el nuevo módulo (placeholder)
            panelContenido.Controls.Clear();
            panelContenido.Controls.Add(lblTitulo);
            lblTitulo.Text = texto;
            Label lblPlaceholder = new Label()
            {
                Text = "Módulo en construcción...",
                Font = new Font("Segoe UI", 12, FontStyle.Italic),
                ForeColor = Color.Gray,
                Location = new Point(30, 80),
                AutoSize = true
            };
            panelContenido.Controls.Add(lblPlaceholder);
        }
    }
}