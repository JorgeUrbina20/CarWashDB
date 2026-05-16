using System;
using System.Collections.Generic;

namespace CarWashApp.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string NombreUsuario { get; set; } = null!;

    public string Contrasena { get; set; } = null!;

    public int IdRol { get; set; }

    public int? IdEmpleado { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public bool? Activo { get; set; }

    public virtual Empleado? IdEmpleadoNavigation { get; set; }

    public virtual Role IdRolNavigation { get; set; } = null!;
}
