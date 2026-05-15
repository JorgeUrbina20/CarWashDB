using System;
using System.Collections.Generic;

namespace CarWashApp.Models;

public partial class Inventario
{
    public int IdInventario { get; set; }

    public int IdProducto { get; set; }

    public int Cantidad { get; set; }

    public decimal? CostoUnitario { get; set; }

    public virtual Almacen IdProductoNavigation { get; set; } = null!;

    public virtual ICollection<Proveedore> IdProveedors { get; set; } = new List<Proveedore>();
}
