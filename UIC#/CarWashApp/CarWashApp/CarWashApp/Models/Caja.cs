using System;
using System.Collections.Generic;

namespace CarWashApp.Models;

public partial class Caja
{
    public int IdCaja { get; set; }

    public int IdServicio { get; set; }

    public decimal Precio { get; set; }

    public string? TipoPago { get; set; }

    public DateOnly? Fecha { get; set; }

    public virtual Servicio IdServicioNavigation { get; set; } = null!;
}
