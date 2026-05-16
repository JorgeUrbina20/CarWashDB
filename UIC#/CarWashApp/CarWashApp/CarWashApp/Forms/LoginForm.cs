using CarWashApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows.Forms;

namespace CarWashApp
{
    public partial class LoginForm : Form
    {
        private TextBox txtUsuario;
        private TextBox txtPassword;
        private Button btnLogin;
        private Label lblUsuario;
        private Label lblPassword;

        public LoginForm()
        {
            CrearControles();
        }

        private void CrearControles()
        {
            this.Text = "CarWash Pro - Iniciar Sesión";
            this.Size = new System.Drawing.Size(400, 250);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            lblUsuario = new Label()
            {
                Text = "Usuario",
                Location = new System.Drawing.Point(50, 40),
                AutoSize = true
            };

            txtUsuario = new TextBox()
            {
                Location = new System.Drawing.Point(50, 60),
                Size = new System.Drawing.Size(280, 20)
            };

            lblPassword = new Label()
            {
                Text = "Contraseña",
                Location = new System.Drawing.Point(50, 100),
                AutoSize = true
            };

            txtPassword = new TextBox()
            {
                Location = new System.Drawing.Point(50, 120),
                Size = new System.Drawing.Size(280, 20),
                PasswordChar = '*'
            };

            btnLogin = new Button()
            {
                Text = "Ingresar",
                Location = new System.Drawing.Point(50, 160),
                Size = new System.Drawing.Size(280, 30)
            };

            btnLogin.Click += BtnLogin_Click;

            this.Controls.Add(lblUsuario);
            this.Controls.Add(txtUsuario);
            this.Controls.Add(lblPassword);
            this.Controls.Add(txtPassword);
            this.Controls.Add(btnLogin);
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(password))
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
                        .FirstOrDefault(u => u.NombreUsuario == usuario && u.Activo == true);

                    if (user == null)
                    {
                        MessageBox.Show("Usuario no encontrado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    bool passOk = false;
                    if (user.Contrasena == "temporal")
                    {
                        passOk = (password == "temporal");
                    }
                    else
                    {
                        passOk = BCrypt.Net.BCrypt.Verify(password, user.Contrasena);
                    }

                    if (!passOk)
                    {
                        MessageBox.Show("Contraseña incorrecta.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string rol = user.IdRolNavigation.NombreRol;
                    string nombreEmpleado = user.IdEmpleadoNavigation?.Nombre ?? usuario;

                    Form mainForm = null;
                    switch (rol)
                    {
                        case "Owner":
                            mainForm = new OwnerForm(nombreEmpleado);
                            break;
                        case "RecepcionistaCajero":
                            mainForm = new RecepcionistaForm(nombreEmpleado);
                            break;
                        case "Empleado":
                            mainForm = new EmpleadoForm(nombreEmpleado);
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