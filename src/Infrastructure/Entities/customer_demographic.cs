using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class customer_demographic
{
    public string customer_type_id { get; set; } = null!;

    public string? customer_desc { get; set; }

    public virtual ICollection<customer> customers { get; set; } = new List<customer>();
}
