using CarWashApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace CarWashApp
{
    public partial class LoginForm : Form
    {
        private Panel panelTarjeta;
        private TextBox txtUsuario;
        private TextBox txtPassword;
        private Button btnLogin;

        public LoginForm()
        {
            ConfigurarVentana();
            CrearControles();
        }

        private void ConfigurarVentana()
        {
            this.Text = "";
            this.Size = new Size(450, 560);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.FromArgb(30, 30, 40);
            this.MaximizeBox = false;
            this.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    ReleaseCapture();
                    SendMessage(Handle, 0xA1, 0x2, 0);
                }
            };
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool ReleaseCapture();
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        private void CrearControles()
        {
            panelTarjeta = new Panel()
            {
                Size = new Size(350, 430),
                Location = new Point((this.ClientSize.Width - 350) / 2, 45),
                BackColor = Color.White
            };
            panelTarjeta.Paint += (s, e) => DibujarTarjeta(e.Graphics, panelTarjeta.ClientRectangle);
            this.Controls.Add(panelTarjeta);

            PictureBox picLogo = new PictureBox()
            {
                Size = new Size(90, 90),
                Location = new Point((panelTarjeta.Width - 90) / 2, 25),
                SizeMode = PictureBoxSizeMode.Zoom,
                Image = null
            };

            // Comprobación segura: muestra la ruta buscada y evita excepción si falta el archivo
            string rutaLogo = System.IO.Path.Combine(Application.StartupPath, "Resources", "garaje-coche.png");
            if (!System.IO.File.Exists(rutaLogo))
            {
                MessageBox.Show("No se encontró la imagen en: " + rutaLogo + "\n\nAsegúrate de reconstruir la solución para copiar los recursos al directorio de salida.", "Recurso faltante", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    picLogo.Image = Image.FromFile(rutaLogo);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar la imagen: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            panelTarjeta.Controls.Add(picLogo);

            Label lblTitulo = new Label()
            {
                Text = "CarWash Pro",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 30, 40),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(panelTarjeta.Width, 30),
                Location = new Point(0, 125)
            };
            panelTarjeta.Controls.Add(lblTitulo);

            Label lblSubtitulo = new Label()
            {
                Text = "Inicia sesión con tu cuenta",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.Gray,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(panelTarjeta.Width, 20),
                Location = new Point(0, 155)
            };
            panelTarjeta.Controls.Add(lblSubtitulo);

            int xCampo = 40;
            int yCampo = 190;
            int anchoCampo = panelTarjeta.Width - 80;

            txtUsuario = CrearCampo("Usuario", xCampo, yCampo, anchoCampo, false);
            panelTarjeta.Controls.Add(txtUsuario);

            yCampo += 60;
            txtPassword = CrearCampo("Contraseña", xCampo, yCampo, anchoCampo, true);
            panelTarjeta.Controls.Add(txtPassword);

            btnLogin = new Button()
            {
                Text = "INGRESAR",
                Size = new Size(anchoCampo, 40),
                Location = new Point(xCampo, yCampo + 50),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(0, 150, 130),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Region = new Region(ObtenerRectanguloRedondeado(new Rectangle(0, 0, btnLogin.Width, btnLogin.Height), 10));
            btnLogin.Click += BtnLogin_Click;
            panelTarjeta.Controls.Add(btnLogin);

            Button btnCerrar = new Button()
            {
                Text = "X",
                Size = new Size(30, 30),
                Location = new Point(this.ClientSize.Width - 35, 5),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnCerrar.FlatAppearance.BorderSize = 0;
            btnCerrar.Click += (s, e) => Application.Exit();
            this.Controls.Add(btnCerrar);
        }

        private TextBox CrearCampo(string placeholder, int x, int y, int ancho, bool esPassword)
        {
            TextBox txt = new TextBox()
            {
                Text = placeholder,
                Location = new Point(x, y),
                Size = new Size(ancho, 35),
                BorderStyle = BorderStyle.None,
                ForeColor = Color.Gray,
                BackColor = Color.FromArgb(245, 245, 245),
                Font = new Font("Segoe UI", 11)
            };
            if (esPassword) txt.PasswordChar = '•';

            Panel borde = new Panel()
            {
                Size = new Size(ancho, 2),
                Location = new Point(x, y + 35),
                BackColor = Color.FromArgb(0, 150, 130)
            };
            panelTarjeta.Controls.Add(borde);

            txt.GotFocus += (s, e) =>
            {
                if (txt.Text == placeholder)
                {
                    txt.Text = "";
                    txt.ForeColor = Color.Black;
                }
            };
            txt.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txt.Text))
                {
                    txt.Text = placeholder;
                    txt.ForeColor = Color.Gray;
                }
            };
            return txt;
        }

        private void DibujarTarjeta(Graphics g, Rectangle rect)
        {
            using (GraphicsPath path = ObtenerRectanguloRedondeado(rect, 20))
            using (Brush brush = new SolidBrush(panelTarjeta.BackColor))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.FillPath(brush, path);
                using (Pen pen = new Pen(Color.FromArgb(200, 200, 200), 1))
                    g.DrawPath(pen, path);
            }
        }

        private GraphicsPath ObtenerRectanguloRedondeado(Rectangle rect, int radio)
        {
            GraphicsPath path = new GraphicsPath();
            int d = radio * 2;
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.X + rect.Width - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.X + rect.Width - d, rect.Y + rect.Height - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Y + rect.Height - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        private void InitializeComponent()
        {

        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(password)
                || usuario == "Usuario" || password == "Contraseña")
            {
                MessageBox.Show("Ingrese usuario y contraseña.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var db = new AppDbContext())
                {
                    var user = db.Usuarios
                        .Include(u => u.IdRolNavigation)
                        .Include(u => u.IdEmpleadoNavigation)
                        .FirstOrDefault(u => u.NombreUsuario == usuario && u.Activo == true);

                    if (user == null)
                    {
                        MessageBox.Show("Usuario no encontrado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    bool passOk;
                    if (user.Contrasena == "temporal")
                        passOk = (password == "temporal");
                    else
                        passOk = BCrypt.Net.BCrypt.Verify(password, user.Contrasena);

                    if (!passOk)
                    {
                        MessageBox.Show("Contraseña incorrecta.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string rol = user.IdRolNavigation.NombreRol;
                    string nombreEmpleado = (user.IdEmpleadoNavigation != null)
                        ? user.IdEmpleadoNavigation.Nombre + " " + user.IdEmpleadoNavigation.ApellidoPaterno
                        : usuario;
                    int idEmpleado = user.IdEmpleado ?? 0;

                    Form mainForm;
                    switch (rol)
                    {
                        case "Owner":
                            mainForm = new OwnerForm(nombreEmpleado);
                            break;
                        case "RecepcionistaCajero":
                            mainForm = new RecepcionistaForm(nombreEmpleado);
                            break;
                        case "Empleado":
                            mainForm = new EmpleadoForm(nombreEmpleado, idEmpleado);
                            break;
                        default:
                            MessageBox.Show("Rol no reconocido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                    }

                    this.Hide();
                    mainForm.FormClosed += (s, args) => this.Close();
                    mainForm.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}