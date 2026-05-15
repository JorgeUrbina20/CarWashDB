using System;
using System.Collections.Generic;

namespace CarWashApp.Models;

public partial class Ordene
{
    public int IdOrden { get; set; }

    public int IdCliente { get; set; }

    public int IdVehiculo { get; set; }

    public int IdTipoServicio { get; set; }

    public int? IdEmpleado { get; set; }

    public int? IdServicio { get; set; }

    public string? Estado { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public string? InstruccionesEspeciales { get; set; }

    public virtual Cliente IdClienteNavigation { get; set; } = null!;

    public virtual Empleado? IdEmpleadoNavigation { get; set; }

    public virtual Servicio? IdServicioNavigation { get; set; }

    public virtual TiposServicio IdTipoServicioNavigation { get; set; } = null!;

    public virtual Vehiculo IdVehiculoNavigation { get; set; } = null!;
}
