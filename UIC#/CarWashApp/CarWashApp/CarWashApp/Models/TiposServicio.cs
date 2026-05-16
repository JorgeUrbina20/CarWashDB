using System;
using System.Collections.Generic;

namespace CarWashApp.Models;

public partial class TiposServicio
{
    public int IdTipoServicio { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public decimal PrecioBase { get; set; }

    public virtual ICollection<Ordene> Ordenes { get; set; } = new List<Ordene>();

    public virtual ICollection<Servicio> Servicios { get; set; } = new List<Servicio>();
}
