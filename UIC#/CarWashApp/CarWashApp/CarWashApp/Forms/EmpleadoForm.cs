using CarWashApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
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
        private int _idEmpleado;

        public EmpleadoForm(string nombreEmpleado, int idEmpleado)
        {
            _idEmpleado = idEmpleado;
            ConfigurarVentana(nombreEmpleado);
            InicializarComponentes();
            CargarMisTrabajos();
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

            btnMisTrabajos.Click += (s, e) => CargarMisTrabajos();
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

        private void CargarMisTrabajos()
        {
            panelContenido.Controls.Clear();
            panelContenido.Controls.Add(lblTitulo);
            lblTitulo.Text = "Mis Trabajos Asignados";

            DataGridView dgv = new DataGridView()
            {
                Location = new Point(30, 80),
                Size = new Size(800, 450),
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            using (var db = new AppDbContext())
            {
                var trabajos = db.Ordenes
                    .Where(o => o.IdEmpleado == _idEmpleado && (o.Estado == "pendiente" || o.Estado == "lavando"))
                    .Select(o => new
                    {
                        o.IdOrden,
                        Cliente = o.IdClienteNavigation.Nombre + " " + o.IdClienteNavigation.ApellidoPaterno,
                        Placa = o.IdVehiculoNavigation.Placa,
                        Servicio = o.IdTipoServicioNavigation.Nombre,
                        o.Estado,
                        o.InstruccionesEspeciales
                    }).ToList();

                dgv.DataSource = trabajos;
            }

            Button btnIniciar = new Button()
            {
                Text = "Iniciar Lavado",
                Location = new Point(30, 550),
                Size = new Size(150, 35),
                BackColor = Color.FromArgb(230, 126, 34),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnIniciar.FlatAppearance.BorderSize = 0;
            btnIniciar.Click += (s, e) => CambiarEstadoOrden("lavando", dgv);

            Button btnTerminar = new Button()
            {
                Text = "Marcar Terminado",
                Location = new Point(200, 550),
                Size = new Size(150, 35),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnTerminar.FlatAppearance.BorderSize = 0;
            btnTerminar.Click += (s, e) => CambiarEstadoOrden("terminado", dgv);

            panelContenido.Controls.Add(dgv);
            panelContenido.Controls.Add(btnIniciar);
            panelContenido.Controls.Add(btnTerminar);
        }

        private void InitializeComponent()
        {

        }

        private void CambiarEstadoOrden(string nuevoEstado, DataGridView dgv)
        {
            if (dgv.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione una orden.");
                return;
            }
            int idOrden = (int)dgv.SelectedRows[0].Cells[0].Value;
            try
            {
                using (var db = new AppDbContext())
                {
                    db.Database.ExecuteSqlRaw("EXEC sp_CambiarEstadoOrden @IdOrden={0}, @NuevoEstado={1}", idOrden, nuevoEstado);
                }
                CargarMisTrabajos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}