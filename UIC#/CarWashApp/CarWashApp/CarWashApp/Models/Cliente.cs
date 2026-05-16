using System;
using System.Collections.Generic;

namespace CarWashApp.Models;

public partial class Cliente
{
    public int IdCliente { get; set; }

    public string Nombre { get; set; } = null!;

    public string? ApellidoPaterno { get; set; }

    public string? ApellidoMaterno { get; set; }

    public string? Direccion { get; set; }

    public string? Telefono { get; set; }

    public string? Email { get; set; }

    public bool? Activo { get; set; }

    public virtual ICollection<Ordene> Ordenes { get; set; } = new List<Ordene>();

    public virtual ICollection<Servicio> Servicios { get; set; } = new List<Servicio>();

    public virtual ICollection<Vehiculo> Vehiculos { get; set; } = new List<Vehiculo>();
}
