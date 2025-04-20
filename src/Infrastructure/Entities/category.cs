using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class category
{
    public short category_id { get; set; }

    public string category_name { get; set; } = null!;

    public string? description { get; set; }

    public byte[]? picture { get; set; }

    public virtual ICollection<product> products { get; set; } = new List<product>();
}
