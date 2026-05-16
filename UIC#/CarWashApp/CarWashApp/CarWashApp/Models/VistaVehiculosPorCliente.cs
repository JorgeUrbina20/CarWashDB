using System;
using System.Collections.Generic;

namespace CarWashApp.Models;

public partial class VistaVehiculosPorCliente
{
    public int IdCliente { get; set; }

    public string Cliente { get; set; } = null!;

    public bool? ClienteActivo { get; set; }

    public string Placa { get; set; } = null!;

    public string? TipoVehiculo { get; set; }

    public string Marca { get; set; } = null!;

    public string Modelo { get; set; } = null!;
}
