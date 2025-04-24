namespace Infrastructure.Entity;
public partial class customer {
  public string customer_id { get; set; } = null!;

  public string company_name { get; set; } = null!;

  public string? contact_name { get; set; }

  public string? contact_title { get; set; }

  public string? address { get; set; }

  public string? city { get; set; }

  public string? region { get; set; }

  public string? postal_code { get; set; }

  public string? country { get; set; }

  public string? phone { get; set; }

  public string? fax { get; set; }

  public virtual ICollection<order> orders { get; set; } = new List<order>();

  public virtual ICollection<customer_demographic> customer_types { get; set; } = new List<customer_demographic>();
}
