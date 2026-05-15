using System;
using System.Collections.Generic;

namespace CarWashApp.Models;

public partial class VistaCajaDiarium
{
    public DateOnly? Fecha { get; set; }

    public decimal? Total { get; set; }

    public int? NumeroTransacciones { get; set; }

    public decimal? PromedioPorTransaccion { get; set; }

    public string? TipoPago { get; set; }
}
