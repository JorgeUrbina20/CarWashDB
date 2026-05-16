using System;
using System.Collections.Generic;

namespace CarWashApp.Models;

public partial class VistaServiciosHoy
{
    public int IdServicio { get; set; }

    public int IdCliente { get; set; }

    public string Cliente { get; set; } = null!;

    public string? Placa { get; set; }

    public string TipoServicio { get; set; } = null!;

    public decimal Costo { get; set; }

    public DateOnly? Fecha { get; set; }
}
