using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class employee
{
    public short employee_id { get; set; }

    public string last_name { get; set; } = null!;

    public string first_name { get; set; } = null!;

    public string? title { get; set; }

    public string? title_of_courtesy { get; set; }

    public DateOnly? birth_date { get; set; }

    public DateOnly? hire_date { get; set; }

    public string? address { get; set; }

    public string? city { get; set; }

    public string? region { get; set; }

    public string? postal_code { get; set; }

    public string? country { get; set; }

    public string? home_phone { get; set; }

    public string? extension { get; set; }

    public byte[]? photo { get; set; }

    public string? notes { get; set; }

    public short? reports_to { get; set; }

    public string? photo_path { get; set; }

    public virtual ICollection<employee> Inversereports_toNavigation { get; set; } = new List<employee>();

    public virtual ICollection<order> orders { get; set; } = new List<order>();

    public virtual employee? reports_toNavigation { get; set; }

    public virtual ICollection<territory> territories { get; set; } = new List<territory>();
}
