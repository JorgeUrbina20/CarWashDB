using System;
using System.Collections.Generic;

namespace CarWashApp.Models;

public partial class Vehiculo
{
    public int IdVehiculo { get; set; }

    public int IdCliente { get; set; }

    public int IdModelo { get; set; }

    public string? Tipo { get; set; }

    public string Placa { get; set; } = null!;

    public virtual Cliente IdClienteNavigation { get; set; } = null!;

    public virtual Modelo IdModeloNavigation { get; set; } = null!;

    public virtual ICollection<Ordene> Ordenes { get; set; } = new List<Ordene>();

    public virtual ICollection<Servicio> Servicios { get; set; } = new List<Servicio>();
}
