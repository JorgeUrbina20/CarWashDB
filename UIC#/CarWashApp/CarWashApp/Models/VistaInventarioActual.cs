using System;
using System.Collections.Generic;

namespace CarWashApp.Models;

public partial class VistaInventarioActual
{
    public int IdProducto { get; set; }

    public string Producto { get; set; } = null!;

    public string? Descripcion { get; set; }

    public int Cantidad { get; set; }

    public decimal? CostoUnitario { get; set; }

    public decimal? ValorInventario { get; set; }
}
