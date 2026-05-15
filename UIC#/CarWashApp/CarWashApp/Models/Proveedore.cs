using System;
using System.Collections.Generic;

namespace CarWashApp.Models;

public partial class Proveedore
{
    public int IdProveedor { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Direccion { get; set; }

    public string? Telefono { get; set; }

    public string? Email { get; set; }

    public virtual ICollection<Inventario> IdInventarios { get; set; } = new List<Inventario>();
}
