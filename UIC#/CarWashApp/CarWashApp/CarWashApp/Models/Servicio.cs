using System;
using System.Collections.Generic;

namespace CarWashApp.Models;

public partial class Servicio
{
    public int IdServicio { get; set; }

    public int IdCliente { get; set; }

    public int? IdVehiculo { get; set; }

    public int IdTipoServicio { get; set; }

    public decimal Costo { get; set; }

    public DateOnly? Fecha { get; set; }

    public virtual ICollection<Caja> Cajas { get; set; } = new List<Caja>();

    public virtual Cliente IdClienteNavigation { get; set; } = null!;

    public virtual TiposServicio IdTipoServicioNavigation { get; set; } = null!;

    public virtual Vehiculo? IdVehiculoNavigation { get; set; }

    public virtual ICollection<Ordene> Ordenes { get; set; } = new List<Ordene>();
}
