using System;
using System.Collections.Generic;

namespace CarWashApp.Models;

public partial class Auditorium
{
    public int IdAuditoria { get; set; }

    public string? TablaAfectada { get; set; }

    public string? Operacion { get; set; }

    public int? IdRegistro { get; set; }

    public string? DatosAnteriores { get; set; }

    public string? DatosNuevos { get; set; }

    public string? Usuario { get; set; }

    public DateTime? Fecha { get; set; }
}
