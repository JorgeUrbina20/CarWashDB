using System;
using System.Collections.Generic;

namespace CarWashApp.Models;

public partial class VistaOrdenesPendiente
{
    public int IdOrden { get; set; }

    public string? Estado { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public string? InstruccionesEspeciales { get; set; }

    public string Cliente { get; set; } = null!;

    public string Placa { get; set; } = null!;

    public string TipoServicio { get; set; } = null!;

    public string? EmpleadoAsignado { get; set; }
}
