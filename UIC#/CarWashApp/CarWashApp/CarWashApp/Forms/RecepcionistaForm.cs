using Microsoft.EntityFrameworkCore;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CarWashApp.Models;

namespace CarWashApp
{
    public partial class RecepcionistaForm : Form
    {
        private Panel panelMenu;
        private Panel panelContenido;
        private Button btnClientes;
        private Button btnVehiculos;
        private Button btnOrdenes;
        private Button btnCobrar;
        private Button btnSalir;
        private Label lblTitulo;

        public RecepcionistaForm(string nombreEmpleado)
        {
            ConfigurarVentana(nombreEmpleado);
            InicializarComponentes();
            CargarClientes();
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

            btnClientes = CrearBotonMenu("Clientes", 0);
            btnVehiculos = CrearBotonMenu("Vehículos", 1);
            btnOrdenes = CrearBotonMenu("Nueva Orden", 2);
            btnCobrar = CrearBotonMenu("Cobrar", 3);
            btnSalir = CrearBotonMenu("Salir", 4);
            btnSalir.BackColor = Color.FromArgb(200, 50, 50);
            btnSalir.Click += (s, e) => this.Close();

            panelMenu.Controls.Add(btnClientes);
            panelMenu.Controls.Add(btnVehiculos);
            panelMenu.Controls.Add(btnOrdenes);
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
                Text = "Clientes",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 120, 100),
                Location = new Point(30, 30),
                AutoSize = true
            };
            panelContenido.Controls.Add(lblTitulo);

            btnClientes.Click += (s, e) => CargarClientes();
            btnVehiculos.Click += (s, e) => CargarVehiculos();
            btnOrdenes.Click += (s, e) => CrearOrden();
            btnCobrar.Click += (s, e) => CobrarOrden();
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

        // -------------------------------------------------------
        // CLIENTES
        // -------------------------------------------------------
        private void CargarClientes()
        {
            panelContenido.Controls.Clear();
            panelContenido.Controls.Add(lblTitulo);
            lblTitulo.Text = "Listado de Clientes";

            DataGridView dgv = new DataGridView()
            {
                Location = new Point(30, 80),
                Size = new Size(800, 400),
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            using (var db = new AppDbContext())
            {
                var clientes = db.Clientes.Where(c => c.Activo == true).ToList();
                dgv.DataSource = clientes.Select(c => new
                {
                    c.IdCliente,
                    NombreCompleto = $"{c.Nombre} {c.ApellidoPaterno} {c.ApellidoMaterno}",
                    c.Telefono,
                    c.Email
                }).ToList();
            }

            Button btnNuevo = new Button()
            {
                Text = "Nuevo Cliente",
                Location = new Point(30, 500),
                Size = new Size(150, 35),
                BackColor = Color.FromArgb(0, 150, 130),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnNuevo.FlatAppearance.BorderSize = 0;
            btnNuevo.Click += (s, e) => EditarCliente(null);

            Button btnEditar = new Button()
            {
                Text = "Editar",
                Location = new Point(200, 500),
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnEditar.FlatAppearance.BorderSize = 0;
            btnEditar.Click += (s, e) =>
            {
                if (dgv.SelectedRows.Count > 0)
                {
                    int id = (int)dgv.SelectedRows[0].Cells[0].Value;
                    using (var db = new AppDbContext())
                    {
                        var cliente = db.Clientes.Find(id);
                        if (cliente != null) EditarCliente(cliente);
                    }
                }
            };

            panelContenido.Controls.Add(dgv);
            panelContenido.Controls.Add(btnNuevo);
            panelContenido.Controls.Add(btnEditar);
        }

        private void EditarCliente(Cliente cliente)
        {
            bool esNuevo = cliente == null;
            if (esNuevo) cliente = new Cliente() { Activo = true };

            Form frm = new Form()
            {
                Text = esNuevo ? "Nuevo Cliente" : "Editar Cliente",
                Size = new Size(400, 300),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false
            };

            int y = 20;
            TextBox txtNombre = AgregarCampo(frm, "Nombre", cliente.Nombre, ref y);
            TextBox txtApPat = AgregarCampo(frm, "Apellido Paterno", cliente.ApellidoPaterno, ref y);
            TextBox txtApMat = AgregarCampo(frm, "Apellido Materno", cliente.ApellidoMaterno, ref y);
            TextBox txtTelefono = AgregarCampo(frm, "Teléfono", cliente.Telefono, ref y);
            TextBox txtEmail = AgregarCampo(frm, "Email", cliente.Email, ref y);

            Button btnGuardar = new Button()
            {
                Text = "Guardar",
                Location = new Point(20, y),
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(0, 150, 130),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.Click += (s, e) =>
            {
                try
                {
                    using (var db = new AppDbContext())
                    {
                        if (esNuevo)
                        {
                            db.Database.ExecuteSqlRaw(
                                "EXEC sp_InsertarCliente @Nombre={0}, @ApellidoPaterno={1}, @ApellidoMaterno={2}, @Telefono={3}, @Email={4}",
                                txtNombre.Text, txtApPat.Text, txtApMat.Text, txtTelefono.Text, txtEmail.Text);
                        }
                        else
                        {
                            db.Database.ExecuteSqlRaw(
                                "EXEC sp_ActualizarCliente @IdCliente={0}, @Nombre={1}, @ApellidoPaterno={2}, @ApellidoMaterno={3}, @Telefono={4}, @Email={5}",
                                cliente.IdCliente, txtNombre.Text, txtApPat.Text, txtApMat.Text, txtTelefono.Text, txtEmail.Text);
                        }
                    }
                    frm.Close();
                    CargarClientes();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            };
            frm.Controls.Add(btnGuardar);
            frm.ShowDialog(this);
        }

        private TextBox AgregarCampo(Form frm, string label, string valor, ref int y)
        {
            frm.Controls.Add(new Label() { Text = label + ":", Location = new Point(20, y), AutoSize = true });
            TextBox txt = new TextBox() { Location = new Point(20, y + 20), Width = 250, Text = valor ?? "" };
            frm.Controls.Add(txt);
            y += 50;
            return txt;
        }

        // -------------------------------------------------------
        // VEHÍCULOS
        // -------------------------------------------------------
        private void CargarVehiculos()
        {
            panelContenido.Controls.Clear();
            panelContenido.Controls.Add(lblTitulo);
            lblTitulo.Text = "Gestión de Vehículos";

            DataGridView dgv = new DataGridView()
            {
                Location = new Point(30, 80),
                Size = new Size(800, 400),
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            using (var db = new AppDbContext())
            {
                var vehiculos = db.Vehiculos
                    .Include(v => v.IdClienteNavigation)
                    .Include(v => v.IdModeloNavigation)
                    .ThenInclude(m => m.IdMarcaNavigation)
                    .Select(v => new
                    {
                        v.IdVehiculo,
                        Cliente = v.IdClienteNavigation.Nombre + " " + v.IdClienteNavigation.ApellidoPaterno,
                        v.Placa,
                        Marca = v.IdModeloNavigation.IdMarcaNavigation.NombreMarca,
                        Modelo = v.IdModeloNavigation.NombreModelo,
                        v.Tipo
                    }).ToList();
                dgv.DataSource = vehiculos;
            }

            Button btnNuevo = new Button()
            {
                Text = "+ Nuevo Vehículo",
                Location = new Point(30, 500),
                Size = new Size(150, 35),
                BackColor = Color.FromArgb(0, 150, 130),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnNuevo.FlatAppearance.BorderSize = 0;
            btnNuevo.Click += (s, e) => EditarVehiculo(null);

            Button btnEditar = new Button()
            {
                Text = "Editar",
                Location = new Point(200, 500),
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnEditar.FlatAppearance.BorderSize = 0;
            btnEditar.Click += (s, e) =>
            {
                if (dgv.SelectedRows.Count > 0)
                {
                    int id = (int)dgv.SelectedRows[0].Cells[0].Value;
                    using (var db = new AppDbContext())
                    {
                        var veh = db.Vehiculos.Find(id);
                        if (veh != null) EditarVehiculo(veh);
                    }
                }
            };

            panelContenido.Controls.Add(dgv);
            panelContenido.Controls.Add(btnNuevo);
            panelContenido.Controls.Add(btnEditar);
        }

        private void EditarVehiculo(Vehiculo vehiculo)
        {
            bool esNuevo = vehiculo == null;
            if (esNuevo) vehiculo = new Vehiculo();

            Form frm = new Form()
            {
                Text = esNuevo ? "Nuevo Vehículo" : "Editar Vehículo",
                Size = new Size(400, 300),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false
            };

            int y = 20;

            ComboBox cmbCliente = new ComboBox()
            {
                Location = new Point(20, y),
                Width = 250,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            using (var db = new AppDbContext()) { cmbCliente.DataSource = db.Clientes.ToList(); cmbCliente.DisplayMember = "Nombre"; cmbCliente.ValueMember = "IdCliente"; }
            if (!esNuevo) cmbCliente.SelectedValue = vehiculo.IdCliente;
            frm.Controls.Add(new Label() { Text = "Cliente:", Location = new Point(20, y - 18), AutoSize = true });
            frm.Controls.Add(cmbCliente);
            y += 50;

            ComboBox cmbModelo = new ComboBox()
            {
                Location = new Point(20, y),
                Width = 250,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            using (var db = new AppDbContext())
            {
                var modelos = db.Modelos.Include(m => m.IdMarcaNavigation).ToList();
                cmbModelo.DataSource = modelos;
                cmbModelo.DisplayMember = "NombreModelo";
                cmbModelo.ValueMember = "IdModelo";
            }
            if (!esNuevo) cmbModelo.SelectedValue = vehiculo.IdModelo;
            frm.Controls.Add(new Label() { Text = "Modelo:", Location = new Point(20, y - 18), AutoSize = true });
            frm.Controls.Add(cmbModelo);
            y += 50;

            TextBox txtTipo = AgregarCampo(frm, "Tipo", vehiculo.Tipo, ref y);
            TextBox txtPlaca = AgregarCampo(frm, "Placa", vehiculo.Placa, ref y);

            Button btnGuardar = new Button()
            {
                Text = "Guardar",
                Location = new Point(20, y),
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(0, 150, 130),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.Click += (s, e) =>
            {
                using (var db = new AppDbContext())
                {
                    if (esNuevo)
                    {
                        db.Database.ExecuteSqlRaw(
                            "EXEC sp_InsertarVehiculo @IdCliente={0}, @IdModelo={1}, @Tipo={2}, @Placa={3}",
                            cmbCliente.SelectedValue, cmbModelo.SelectedValue, txtTipo.Text, txtPlaca.Text);
                    }
                    else
                    {
                        db.Database.ExecuteSqlRaw(
                            "EXEC sp_ActualizarVehiculo @IdVehiculo={0}, @IdCliente={1}, @IdModelo={2}, @Tipo={3}, @Placa={4}",
                            vehiculo.IdVehiculo, cmbCliente.SelectedValue, cmbModelo.SelectedValue, txtTipo.Text, txtPlaca.Text);
                    }
                }
                frm.Close();
                CargarVehiculos();
            };
            frm.Controls.Add(btnGuardar);
            frm.ShowDialog(this);
        }

        // -------------------------------------------------------
        // ORDEN
        // -------------------------------------------------------
        private void CrearOrden()
        {
            panelContenido.Controls.Clear();
            panelContenido.Controls.Add(lblTitulo);
            lblTitulo.Text = "Nueva Orden de Lavado";

            ComboBox cmbClientes = new ComboBox() { Location = new Point(30, 80), Width = 300, DropDownStyle = ComboBoxStyle.DropDownList };
            using (var db = new AppDbContext())
            {
                var clientes = db.Clientes.Where(c => c.Activo == true).ToList();
                cmbClientes.DataSource = clientes;
                cmbClientes.DisplayMember = "Nombre";
                cmbClientes.ValueMember = "IdCliente";
            }
            panelContenido.Controls.Add(new Label() { Text = "Cliente:", Location = new Point(30, 60), AutoSize = true });
            panelContenido.Controls.Add(cmbClientes);

            ComboBox cmbVehiculos = new ComboBox() { Location = new Point(30, 140), Width = 300, DropDownStyle = ComboBoxStyle.DropDownList };
            panelContenido.Controls.Add(new Label() { Text = "Vehículo:", Location = new Point(30, 120), AutoSize = true });
            panelContenido.Controls.Add(cmbVehiculos);

            cmbClientes.SelectedIndexChanged += (s, e) =>
            {
                if (cmbClientes.SelectedValue is int idCliente)
                {
                    using (var db = new AppDbContext())
                    {
                        var vehiculos = db.Vehiculos.Where(v => v.IdCliente == idCliente).ToList();
                        cmbVehiculos.DataSource = vehiculos;
                        cmbVehiculos.DisplayMember = "Placa";
                        cmbVehiculos.ValueMember = "IdVehiculo";
                    }
                }
            };

            ComboBox cmbServicios = new ComboBox() { Location = new Point(30, 200), Width = 300, DropDownStyle = ComboBoxStyle.DropDownList };
            using (var db = new AppDbContext())
            {
                var tipos = db.TiposServicios.ToList();
                cmbServicios.DataSource = tipos;
                cmbServicios.DisplayMember = "Nombre";
                cmbServicios.ValueMember = "IdTipoServicio";
            }
            panelContenido.Controls.Add(new Label() { Text = "Tipo de Servicio:", Location = new Point(30, 180), AutoSize = true });
            panelContenido.Controls.Add(cmbServicios);

            TextBox txtInstrucciones = new TextBox() { Location = new Point(30, 260), Width = 300, Height = 60, Multiline = true };
            panelContenido.Controls.Add(new Label() { Text = "Instrucciones:", Location = new Point(30, 240), AutoSize = true });
            panelContenido.Controls.Add(txtInstrucciones);

            Button btnCrear = new Button()
            {
                Text = "Crear Orden",
                Location = new Point(30, 340),
                Size = new Size(150, 40),
                BackColor = Color.FromArgb(0, 150, 130),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCrear.FlatAppearance.BorderSize = 0;
            btnCrear.Click += (s, e) =>
            {
                if (cmbClientes.SelectedValue == null || cmbVehiculos.SelectedValue == null || cmbServicios.SelectedValue == null)
                {
                    MessageBox.Show("Complete todos los campos.");
                    return;
                }
                try
                {
                    using (var db = new AppDbContext())
                    {
                        db.Database.ExecuteSqlRaw(
                            "EXEC sp_CrearOrden @IdCliente={0}, @IdVehiculo={1}, @IdTipoServicio={2}, @Instrucciones={3}",
                            (int)cmbClientes.SelectedValue, (int)cmbVehiculos.SelectedValue, (int)cmbServicios.SelectedValue, txtInstrucciones.Text);
                    }
                    MessageBox.Show("Orden creada exitosamente.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al crear orden: " + ex.Message);
                }
            };
            panelContenido.Controls.Add(btnCrear);
        }

        private void InitializeComponent()
        {

        }

        // -------------------------------------------------------
        // COBRAR
        // -------------------------------------------------------
        private void CobrarOrden()
        {
            panelContenido.Controls.Clear();
            panelContenido.Controls.Add(lblTitulo);
            lblTitulo.Text = "Cobrar Orden Terminada";

            DataGridView dgv = new DataGridView()
            {
                Location = new Point(30, 80),
                Size = new Size(800, 400),
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            using (var db = new AppDbContext())
            {
                var ordenes = db.Ordenes
                    .Where(o => o.Estado == "terminado" && o.IdServicio == null)
                    .Select(o => new
                    {
                        o.IdOrden,
                        Cliente = o.IdClienteNavigation.Nombre + " " + o.IdClienteNavigation.ApellidoPaterno,
                        Placa = o.IdVehiculoNavigation.Placa,
                        Servicio = o.IdTipoServicioNavigation.Nombre,
                        o.Estado
                    }).ToList();

                dgv.DataSource = ordenes;
            }

            ComboBox cmbTipoPago = new ComboBox()
            {
                Location = new Point(30, 500),
                Width = 150,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbTipoPago.Items.AddRange(new string[] { "Efectivo", "Tarjeta" });
            cmbTipoPago.SelectedIndex = 0;
            panelContenido.Controls.Add(new Label() { Text = "Tipo de Pago:", Location = new Point(30, 480), AutoSize = true });
            panelContenido.Controls.Add(cmbTipoPago);

            Button btnCobrar = new Button()
            {
                Text = "Registrar Pago",
                Location = new Point(200, 500),
                Size = new Size(150, 35),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCobrar.FlatAppearance.BorderSize = 0;
            btnCobrar.Click += (s, e) =>
            {
                if (dgv.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Seleccione una orden.");
                    return;
                }
                int idOrden = (int)dgv.SelectedRows[0].Cells[0].Value;
                using (var db = new AppDbContext())
                {
                    var orden = db.Ordenes.Find(idOrden);
                    if (orden != null)
                    {
                        try
                        {
                            db.Database.ExecuteSqlRaw(
                                "EXEC sp_CrearServicioDesdeOrden @IdOrden={0}, @Costo={1}, @TipoPago={2}",
                                idOrden, orden.IdTipoServicioNavigation.PrecioBase, cmbTipoPago.SelectedItem.ToString());
                            MessageBox.Show("Pago registrado correctamente.");
                            CobrarOrden();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error: " + ex.Message);
                        }
                    }
                }
            };

            panelContenido.Controls.Add(dgv);
            panelContenido.Controls.Add(btnCobrar);
        }
    }
}