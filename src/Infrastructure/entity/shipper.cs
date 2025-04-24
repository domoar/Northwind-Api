namespace Infrastructure.Entity;
public partial class shipper {
  public short shipper_id { get; set; }

  public string company_name { get; set; } = null!;

  public string? phone { get; set; }

  public virtual ICollection<order> orders { get; set; } = new List<order>();
}
