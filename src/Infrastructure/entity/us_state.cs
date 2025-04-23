using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class us_state
{
    public short state_id { get; set; }

    public string? state_name { get; set; }

    public string? state_abbr { get; set; }

    public string? state_region { get; set; }
}
