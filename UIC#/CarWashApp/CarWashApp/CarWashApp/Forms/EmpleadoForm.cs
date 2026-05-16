using System;
using System.Drawing;
using System.Windows.Forms;

namespace CarWashApp
{
    public partial class EmpleadoForm : Form
    {
        private Panel panelMenu;
        private Panel panelContenido;
        private Button btnMisTrabajos;
        private Button btnSalir;
        private Label lblTitulo;

        public EmpleadoForm(string nombreEmpleado)
        {
            ConfigurarVentana(nombreEmpleado);
            InicializarComponentes();
        }

        private void ConfigurarVentana(string nombreEmpleado)
        {
            this.Text = $"CarWash Pro | Empleado - {nombreEmpleado}";
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
                BackColor = Color.FromArgb(180, 40, 40)
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

            btnMisTrabajos = CrearBotonMenu("Mis Trabajos", 0);
            btnSalir = CrearBotonMenu("Salir", 1);
            btnSalir.BackColor = Color.FromArgb(80, 20, 20);
            btnSalir.Click += (s, e) => this.Close();

            panelMenu.Controls.Add(btnMisTrabajos);
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
                Text = "Mis Trabajos Asignados",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(180, 40, 40),
                Location = new Point(30, 30),
                AutoSize = true
            };
            panelContenido.Controls.Add(lblTitulo);

            // Botón para actualizar trabajos
            Button btnActualizar = new Button()
            {
                Text = "Actualizar Lista",
                Location = new Point(30, 100),
                Size = new Size(200, 45),
                BackColor = Color.FromArgb(200, 60, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold)
            };
            btnActualizar.FlatAppearance.BorderSize = 0;
            btnActualizar.Click += (s, e) => MessageBox.Show("Cargar órdenes del empleado...");
            panelContenido.Controls.Add(btnActualizar);

            btnMisTrabajos.Click += (s, e) => MostrarContenido("Mis Trabajos Asignados");
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
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(150, 20, 20);
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