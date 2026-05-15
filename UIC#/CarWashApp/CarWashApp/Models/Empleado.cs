using System;
using System.Collections.Generic;

namespace CarWashApp.Models;

public partial class Empleado
{
    public int IdEmpleado { get; set; }

    public string Nombre { get; set; } = null!;

    public string ApellidoPaterno { get; set; } = null!;

    public string? ApellidoMaterno { get; set; }

    public string? Direccion { get; set; }

    public string? Telefono { get; set; }

    public decimal? Sueldo { get; set; }

    public string? Turno { get; set; }

    public string Cargo { get; set; } = null!;

    public DateOnly? FechaIngreso { get; set; }

    public bool? Activo { get; set; }

    public virtual ICollection<Ordene> Ordenes { get; set; } = new List<Ordene>();

    public virtual Usuario? Usuario { get; set; }

    public virtual ICollection<Carwash> IdCarwashes { get; set; } = new List<Carwash>();
}
