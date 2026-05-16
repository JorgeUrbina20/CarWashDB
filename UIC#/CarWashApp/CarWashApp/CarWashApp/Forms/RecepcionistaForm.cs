using System;
using System.Drawing;
using System.Windows.Forms;

namespace CarWashApp
{
    public partial class RecepcionistaForm : Form
    {
        private Panel panelMenu;
        private Panel panelContenido;
        private Button btnNuevaOrden;
        private Button btnCobrar;
        private Button btnSalir;
        private Label lblTitulo;

        public RecepcionistaForm(string nombreEmpleado)
        {
            ConfigurarVentana(nombreEmpleado);
            InicializarComponentes();
        }

        private void ConfigurarVentana(string nombreEmpleado)
        {
            this.Text = $"CarWash Pro | Recepción - {nombreEmpleado}";
            this.Size = new Size(1100, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(245, 245, 250);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Font = new Font("Segoe UI", 10);
        }

        private void InicializarComponentes()
        {
            panelMenu = new Panel()
            {
                Width = 220,
                Height = this.ClientSize.Height,
                BackColor = Color.FromArgb(0, 120, 100)
            };
            this.Controls.Add(panelMenu);

            Label lblLogo = new Label()
            {
                Text = "CarWash Pro",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Location = new Point(20, 30),
                AutoSize = true
            };
            panelMenu.Controls.Add(lblLogo);

            btnNuevaOrden = CrearBotonMenu("Nueva Orden", 0);
            btnCobrar = CrearBotonMenu("Cobrar", 1);
            btnSalir = CrearBotonMenu("Salir", 2);
            btnSalir.BackColor = Color.FromArgb(200, 50, 50);
            btnSalir.Click += (s, e) => this.Close();

            panelMenu.Controls.Add(btnNuevaOrden);
            panelMenu.Controls.Add(btnCobrar);
            panelMenu.Controls.Add(btnSalir);

            panelContenido = new Panel()
            {
                Location = new Point(panelMenu.Width, 0),
                Size = new Size(this.ClientSize.Width - panelMenu.Width, this.ClientSize.Height),
                BackColor = Color.White
            };
            this.Controls.Add(panelContenido);

            lblTitulo = new Label()
            {
                Text = "Órdenes Pendientes",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 120, 100),
                Location = new Point(30, 30),
                AutoSize = true
            };
            panelContenido.Controls.Add(lblTitulo);

            // Acceso rápido a crear orden
            Button btnCrear = new Button()
            {
                Text = "+ Nueva Orden",
                Location = new Point(30, 100),
                Size = new Size(200, 45),
                BackColor = Color.FromArgb(0, 150, 130),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold)
            };
            btnCrear.FlatAppearance.BorderSize = 0;
            btnCrear.Click += (s, e) => MessageBox.Show("Abrir formulario de nueva orden.");
            panelContenido.Controls.Add(btnCrear);

            btnNuevaOrden.Click += (s, e) => MostrarContenido("Nueva Orden de Lavado");
            btnCobrar.Click += (s, e) => MostrarContenido("Cobrar Servicio");
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
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 100, 80);
            return btn;
        }

        private void MostrarContenido(string texto)
        {
            panelContenido.Controls.Clear();
            panelContenido.Controls.Add(lblTitulo);
            lblTitulo.Text = texto;
        }
    }
}