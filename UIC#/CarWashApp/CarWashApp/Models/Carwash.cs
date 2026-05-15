using System;
using System.Collections.Generic;

namespace CarWashApp.Models;

public partial class Carwash
{
    public int IdCarwash { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Direccion { get; set; }

    public string? Email { get; set; }

    public string? Telefono { get; set; }

    public bool? Activo { get; set; }

    public virtual ICollection<Empleado> IdEmpleados { get; set; } = new List<Empleado>();
}
