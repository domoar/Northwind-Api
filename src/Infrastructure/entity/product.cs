using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class product
{
    public short product_id { get; set; }

    public string product_name { get; set; } = null!;

    public short? supplier_id { get; set; }

    public short? category_id { get; set; }

    public string? quantity_per_unit { get; set; }

    public float? unit_price { get; set; }

    public short? units_in_stock { get; set; }

    public short? units_on_order { get; set; }

    public short? reorder_level { get; set; }

    public int discontinued { get; set; }

    public virtual category? category { get; set; }

    public virtual ICollection<order_detail> order_details { get; set; } = new List<order_detail>();

    public virtual supplier? supplier { get; set; }
}
