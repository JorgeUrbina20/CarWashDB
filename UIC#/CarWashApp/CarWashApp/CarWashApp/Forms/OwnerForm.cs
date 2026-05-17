using Microsoft.EntityFrameworkCore;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CarWashApp.Models;

namespace CarWashApp
{
    public partial class OwnerForm : Form
    {
        private Panel panelMenu;
        private Panel panelContenido;
        private Button btnEmpleados;
        private Button btnInventario;
        private Button btnUsuarios;
        private Button btnProveedores;
        private Button btnReportes;
        private Button btnSalir;
        private Label lblTitulo;

        public OwnerForm(string nombreEmpleado)
        {
            ConfigurarVentana(nombreEmpleado);
            InicializarComponentes();
            CargarEmpleados();
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
            panelMenu = new Panel()
            {
                Width = 220,
                Height = this.ClientSize.Height,
                BackColor = Color.FromArgb(30, 30, 40)
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

            btnEmpleados = CrearBotonMenu("Empleados", 0);
            btnUsuarios = CrearBotonMenu("Usuarios", 1);
            btnInventario = CrearBotonMenu("Inventario", 2);
            btnProveedores = CrearBotonMenu("Proveedores", 3);
            btnReportes = CrearBotonMenu("Reportes", 4);
            btnSalir = CrearBotonMenu("Salir", 5);
            btnSalir.BackColor = Color.FromArgb(200, 50, 50);
            btnSalir.Click += (s, e) => this.Close();

            panelMenu.Controls.Add(btnEmpleados);
            panelMenu.Controls.Add(btnUsuarios);
            panelMenu.Controls.Add(btnInventario);
            panelMenu.Controls.Add(btnProveedores);
            panelMenu.Controls.Add(btnReportes);
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
                Text = "Gestión de Empleados",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 30, 40),
                Location = new Point(30, 30),
                AutoSize = true
            };
            panelContenido.Controls.Add(lblTitulo);

            btnEmpleados.Click += (s, e) => CargarEmpleados();
            btnUsuarios.Click += (s, e) => CargarUsuarios();
            btnInventario.Click += (s, e) => CargarInventario();
            btnProveedores.Click += (s, e) => CargarProveedores();
            btnReportes.Click += (s, e) => CargarReportes();
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

        // -------------------------------------------------------
        // EMPLEADOS
        // -------------------------------------------------------
        private void CargarEmpleados()
        {
            panelContenido.Controls.Clear();
            panelContenido.Controls.Add(lblTitulo);
            lblTitulo.Text = "Gestión de Empleados";

            DataGridView dgv = new DataGridView()
            {
                Location = new Point(30, 80),
                Size = new Size(800, 400),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            using (var db = new AppDbContext())
            {
                var empleados = db.Empleados
                    .FromSqlRaw("EXEC sp_LeerEmpleados")
                    .ToList();

                dgv.DataSource = empleados.Select(e => new
                {
                    e.IdEmpleado,
                    e.Nombre,
                    e.ApellidoPaterno,
                    e.ApellidoMaterno,
                    e.Cargo,
                    e.Telefono,
                    e.Activo
                }).ToList();
            }

            Button btnNuevo = new Button()
            {
                Text = "+ Nuevo Empleado",
                Location = new Point(30, 500),
                Size = new Size(150, 35),
                BackColor = Color.FromArgb(0, 150, 130),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnNuevo.FlatAppearance.BorderSize = 0;
            btnNuevo.Click += (s, e) => EditarEmpleado(null);

            Button btnEditar = new Button()
            {
                Text = "Editar Seleccionado",
                Location = new Point(200, 500),
                Size = new Size(150, 35),
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
                        var emp = db.Empleados.Find(id);
                        if (emp != null) EditarEmpleado(emp);
                    }
                }
                else MessageBox.Show("Seleccione un empleado.");
            };

            Button btnEliminar = new Button()
            {
                Text = "Desactivar",
                Location = new Point(370, 500),
                Size = new Size(150, 35),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnEliminar.FlatAppearance.BorderSize = 0;
            btnEliminar.Click += (s, e) =>
            {
                if (dgv.SelectedRows.Count > 0)
                {
                    int id = (int)dgv.SelectedRows[0].Cells[0].Value;
                    using (var db = new AppDbContext())
                    {
                        db.Database.ExecuteSqlRaw("EXEC sp_EliminarEmpleado @IdEmpleado = {0}", id);
                    }
                    CargarEmpleados();
                }
            };

            panelContenido.Controls.Add(dgv);
            panelContenido.Controls.Add(btnNuevo);
            panelContenido.Controls.Add(btnEditar);
            panelContenido.Controls.Add(btnEliminar);
        }

        private void EditarEmpleado(Empleado empleado)
        {
            bool esNuevo = empleado == null;
            if (esNuevo) empleado = new Empleado() { Activo = true, FechaIngreso = DateOnly.FromDateTime(DateTime.Now) };

            Form frm = new Form()
            {
                Text = esNuevo ? "Nuevo Empleado" : "Editar Empleado",
                Size = new Size(450, 420),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false
            };

            int y = 20;
            TextBox txtNombre = AgregarCampo(frm, "Nombre", empleado.Nombre, ref y);
            TextBox txtApPat = AgregarCampo(frm, "Apellido Paterno", empleado.ApellidoPaterno, ref y);
            TextBox txtApMat = AgregarCampo(frm, "Apellido Materno", empleado.ApellidoMaterno, ref y);
            TextBox txtCargo = AgregarCampo(frm, "Cargo", empleado.Cargo, ref y);
            TextBox txtTelefono = AgregarCampo(frm, "Teléfono", empleado.Telefono, ref y);
            NumericUpDown numSueldo = new NumericUpDown()
            {
                Location = new Point(20, y),
                Width = 200,
                Value = empleado.Sueldo ?? 0,
                Minimum = 0,
                Maximum = 100000
            };
            frm.Controls.Add(new Label() { Text = "Sueldo:", Location = new Point(20, y - 18), AutoSize = true });
            frm.Controls.Add(numSueldo);
            y += 50;

            ComboBox cmbTurno = new ComboBox()
            {
                Location = new Point(20, y),
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbTurno.Items.AddRange(new string[] { "Matutino", "Vespertino", "Nocturno", "Completo" });
            cmbTurno.SelectedItem = empleado.Turno ?? "Matutino";
            frm.Controls.Add(new Label() { Text = "Turno:", Location = new Point(20, y - 18), AutoSize = true });
            frm.Controls.Add(cmbTurno);
            y += 50;

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
                                "EXEC sp_InsertarEmpleado @Nombre={0}, @ApellidoPaterno={1}, @ApellidoMaterno={2}, @Cargo={3}, @Telefono={4}, @Sueldo={5}, @Turno={6}",
                                txtNombre.Text, txtApPat.Text, txtApMat.Text, txtCargo.Text, txtTelefono.Text, numSueldo.Value, cmbTurno.SelectedItem.ToString());
                        }
                        else
                        {
                            db.Database.ExecuteSqlRaw(
                                "EXEC sp_ActualizarEmpleado @IdEmpleado={0}, @Nombre={1}, @ApellidoPaterno={2}, @ApellidoMaterno={3}, @Cargo={4}, @Telefono={5}, @Sueldo={6}, @Turno={7}",
                                empleado.IdEmpleado, txtNombre.Text, txtApPat.Text, txtApMat.Text, txtCargo.Text, txtTelefono.Text, numSueldo.Value, cmbTurno.SelectedItem.ToString());
                        }
                    }
                    frm.Close();
                    CargarEmpleados();
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
        // USUARIOS
        // -------------------------------------------------------
        private void CargarUsuarios()
        {
            panelContenido.Controls.Clear();
            panelContenido.Controls.Add(lblTitulo);
            lblTitulo.Text = "Gestión de Usuarios";

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
                var usuarios = db.Usuarios
                    .Include(u => u.IdRolNavigation)
                    .Include(u => u.IdEmpleadoNavigation)
                    .Select(u => new
                    {
                        u.IdUsuario,
                        u.NombreUsuario,
                        Rol = u.IdRolNavigation.NombreRol,
                        Empleado = u.IdEmpleadoNavigation.Nombre + " " + u.IdEmpleadoNavigation.ApellidoPaterno,
                        u.Activo
                    }).ToList();
                dgv.DataSource = usuarios;
            }

            Button btnNuevo = new Button()
            {
                Text = "+ Nuevo Usuario",
                Location = new Point(30, 500),
                Size = new Size(150, 35),
                BackColor = Color.FromArgb(0, 150, 130),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnNuevo.FlatAppearance.BorderSize = 0;
            btnNuevo.Click += (s, e) => EditarUsuario(null);

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
                        var user = db.Usuarios.Find(id);
                        if (user != null) EditarUsuario(user);
                    }
                }
            };

            Button btnEliminar = new Button()
            {
                Text = "Desactivar",
                Location = new Point(320, 500),
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnEliminar.FlatAppearance.BorderSize = 0;
            btnEliminar.Click += (s, e) =>
            {
                if (dgv.SelectedRows.Count > 0)
                {
                    int id = (int)dgv.SelectedRows[0].Cells[0].Value;
                    using (var db = new AppDbContext())
                    {
                        db.Database.ExecuteSqlRaw("UPDATE USUARIOS SET Activo = 0 WHERE IdUsuario = {0}", id);
                    }
                    CargarUsuarios();
                }
            };

            panelContenido.Controls.Add(dgv);
            panelContenido.Controls.Add(btnNuevo);
            panelContenido.Controls.Add(btnEditar);
            panelContenido.Controls.Add(btnEliminar);
        }

        private void EditarUsuario(Usuario usuario)
        {
            bool esNuevo = usuario == null;
            if (esNuevo) usuario = new Usuario() { Activo = true };

            Form frm = new Form()
            {
                Text = esNuevo ? "Nuevo Usuario" : "Editar Usuario",
                Size = new Size(400, 350),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false
            };

            int y = 20;
            TextBox txtUsuario = AgregarCampo(frm, "Nombre Usuario", usuario.NombreUsuario, ref y);
            TextBox txtPassword = AgregarCampo(frm, "Contraseña", "", ref y);

            ComboBox cmbRol = new ComboBox()
            {
                Location = new Point(20, y),
                Width = 250,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            using (var db = new AppDbContext()) { cmbRol.DataSource = db.Roles.ToList(); cmbRol.DisplayMember = "NombreRol"; cmbRol.ValueMember = "IdRol"; }
            if (!esNuevo) cmbRol.SelectedValue = usuario.IdRol;
            frm.Controls.Add(new Label() { Text = "Rol:", Location = new Point(20, y - 18), AutoSize = true });
            frm.Controls.Add(cmbRol);
            y += 50;

            ComboBox cmbEmpleado = new ComboBox()
            {
                Location = new Point(20, y),
                Width = 250,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            using (var db = new AppDbContext()) { cmbEmpleado.DataSource = db.Empleados.ToList(); cmbEmpleado.DisplayMember = "Nombre"; cmbEmpleado.ValueMember = "IdEmpleado"; }
            if (!esNuevo && usuario.IdEmpleado.HasValue) cmbEmpleado.SelectedValue = usuario.IdEmpleado.Value;
            frm.Controls.Add(new Label() { Text = "Empleado:", Location = new Point(20, y - 18), AutoSize = true });
            frm.Controls.Add(cmbEmpleado);
            y += 50;

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
                            string hash = BCrypt.Net.BCrypt.HashPassword(txtPassword.Text);
                            db.Database.ExecuteSqlRaw(
                                "INSERT INTO USUARIOS (NombreUsuario, Contrasena, IdRol, IdEmpleado) VALUES (@p0, @p1, @p2, @p3)",
                                txtUsuario.Text, hash, cmbRol.SelectedValue, cmbEmpleado.SelectedValue);
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(txtPassword.Text))
                            {
                                string hash = BCrypt.Net.BCrypt.HashPassword(txtPassword.Text);
                                db.Database.ExecuteSqlRaw("UPDATE USUARIOS SET Contrasena = @p0 WHERE IdUsuario = @p1", hash, usuario.IdUsuario);
                            }
                            db.Database.ExecuteSqlRaw(
                                "UPDATE USUARIOS SET NombreUsuario = @p0, IdRol = @p1, IdEmpleado = @p2 WHERE IdUsuario = @p3",
                                txtUsuario.Text, cmbRol.SelectedValue, cmbEmpleado.SelectedValue, usuario.IdUsuario);
                        }
                    }
                    frm.Close();
                    CargarUsuarios();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            };
            frm.Controls.Add(btnGuardar);
            frm.ShowDialog(this);
        }

        // -------------------------------------------------------
        // INVENTARIO
        // -------------------------------------------------------
        private void CargarInventario()
        {
            panelContenido.Controls.Clear();
            panelContenido.Controls.Add(lblTitulo);
            lblTitulo.Text = "Control de Inventario";

            DataGridView dgv = new DataGridView()
            {
                Location = new Point(30, 80),
                Size = new Size(800, 350),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            using (var db = new AppDbContext())
            {
                var inventario = db.Almacens
                    .Join(db.Inventarios, a => a.IdProducto, i => i.IdProducto, (a, i) => new
                    {
                        a.IdProducto,
                        a.Nombre,
                        a.Descripcion,
                        i.Cantidad,
                        i.CostoUnitario
                    }).ToList();

                dgv.DataSource = inventario;
            }

            panelContenido.Controls.Add(dgv);
        }

        // -------------------------------------------------------
        // PROVEEDORES
        // -------------------------------------------------------
        private void CargarProveedores()
        {
            panelContenido.Controls.Clear();
            panelContenido.Controls.Add(lblTitulo);
            lblTitulo.Text = "Proveedores";

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
                dgv.DataSource = db.Proveedores.ToList();
            }

            Button btnNuevo = new Button()
            {
                Text = "+ Nuevo Proveedor",
                Location = new Point(30, 500),
                Size = new Size(150, 35),
                BackColor = Color.FromArgb(0, 150, 130),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnNuevo.FlatAppearance.BorderSize = 0;
            btnNuevo.Click += (s, e) => EditarProveedor(null);

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
                        var prov = db.Proveedores.Find(id);
                        if (prov != null) EditarProveedor(prov);
                    }
                }
            };

            Button btnEliminar = new Button()
            {
                Text = "Eliminar",
                Location = new Point(320, 500),
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnEliminar.FlatAppearance.BorderSize = 0;
            btnEliminar.Click += (s, e) =>
            {
                if (dgv.SelectedRows.Count > 0)
                {
                    int id = (int)dgv.SelectedRows[0].Cells[0].Value;
                    using (var db = new AppDbContext())
                    {
                        var prov = db.Proveedores.Find(id);
                        if (prov != null)
                        {
                            db.Proveedores.Remove(prov);
                            db.SaveChanges();
                        }
                    }
                    CargarProveedores();
                }
            };

            panelContenido.Controls.Add(dgv);
            panelContenido.Controls.Add(btnNuevo);
            panelContenido.Controls.Add(btnEditar);
            panelContenido.Controls.Add(btnEliminar);
        }

        private void EditarProveedor(Proveedore proveedor)
        {
            bool esNuevo = proveedor == null;
            if (esNuevo) proveedor = new Proveedore();

            Form frm = new Form()
            {
                Text = esNuevo ? "Nuevo Proveedor" : "Editar Proveedor",
                Size = new Size(400, 250),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false
            };

            int y = 20;
            TextBox txtNombre = AgregarCampo(frm, "Nombre", proveedor.Nombre, ref y);
            TextBox txtDireccion = AgregarCampo(frm, "Dirección", proveedor.Direccion, ref y);
            TextBox txtTelefono = AgregarCampo(frm, "Teléfono", proveedor.Telefono, ref y);
            TextBox txtEmail = AgregarCampo(frm, "Email", proveedor.Email, ref y);

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
                        var nuevo = new Proveedore
                        {
                            Nombre = txtNombre.Text,
                            Direccion = txtDireccion.Text,
                            Telefono = txtTelefono.Text,
                            Email = txtEmail.Text
                        };
                        db.Proveedores.Add(nuevo);
                    }
                    else
                    {
                        proveedor.Nombre = txtNombre.Text;
                        proveedor.Direccion = txtDireccion.Text;
                        proveedor.Telefono = txtTelefono.Text;
                        proveedor.Email = txtEmail.Text;
                    }
                    db.SaveChanges();
                }
                frm.Close();
                CargarProveedores();
            };
            frm.Controls.Add(btnGuardar);
            frm.ShowDialog(this);
        }

        private void InitializeComponent()
        {

        }

        // -------------------------------------------------------
        // REPORTES
        // -------------------------------------------------------
        private void CargarReportes()
        {
            panelContenido.Controls.Clear();
            panelContenido.Controls.Add(lblTitulo);
            lblTitulo.Text = "Reporte de Caja (Hoy)";

            DataGridView dgv = new DataGridView()
            {
                Location = new Point(30, 80),
                Size = new Size(400, 200),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            using (var db = new AppDbContext())
            {
                var reporte = db.VistaCajaDiaria.ToList();
                dgv.DataSource = reporte;
            }

            Label lblTotal = new Label()
            {
                Text = "Total del día: " + dgv.Rows.Cast<DataGridViewRow>().Sum(r => Convert.ToDecimal(r.Cells["Total"].Value)).ToString("C"),
                Location = new Point(30, 300),
                AutoSize = true,
                Font = new Font("Segoe UI", 12, FontStyle.Bold)
            };

            panelContenido.Controls.Add(dgv);
            panelContenido.Controls.Add(lblTotal);
        }
    }
}