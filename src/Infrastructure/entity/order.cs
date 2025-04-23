using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class order
{
    public short order_id { get; set; }

    public string? customer_id { get; set; }

    public short? employee_id { get; set; }

    public DateOnly? order_date { get; set; }

    public DateOnly? required_date { get; set; }

    public DateOnly? shipped_date { get; set; }

    public short? ship_via { get; set; }

    public float? freight { get; set; }

    public string? ship_name { get; set; }

    public string? ship_address { get; set; }

    public string? ship_city { get; set; }

    public string? ship_region { get; set; }

    public string? ship_postal_code { get; set; }

    public string? ship_country { get; set; }

    public virtual customer? customer { get; set; }

    public virtual employee? employee { get; set; }

    public virtual ICollection<order_detail> order_details { get; set; } = new List<order_detail>();

    public virtual shipper? ship_viaNavigation { get; set; }
}
