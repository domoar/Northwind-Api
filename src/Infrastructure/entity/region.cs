using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class region
{
    public short region_id { get; set; }

    public string region_description { get; set; } = null!;

    public virtual ICollection<territory> territories { get; set; } = new List<territory>();
}
