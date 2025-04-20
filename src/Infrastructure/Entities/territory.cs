using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class territory
{
    public string territory_id { get; set; } = null!;

    public string territory_description { get; set; } = null!;

    public short region_id { get; set; }

    public virtual region region { get; set; } = null!;

    public virtual ICollection<employee> employees { get; set; } = new List<employee>();
}
