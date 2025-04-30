namespace Infrastructure.Entity;
#pragma warning disable IDE1006 // Naming Styles
public partial class customer_demographic {
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE1006 // Naming Styles
  public string customer_type_id { get; set; } = null!;
#pragma warning restore IDE1006 // Naming Styles

#pragma warning disable IDE1006 // Naming Styles
  public string? customer_desc { get; set; }
#pragma warning restore IDE1006 // Naming Styles

#pragma warning disable IDE1006 // Naming Styles
  public virtual ICollection<customer> customers { get; set; } = [];
#pragma warning restore IDE1006 // Naming Styles
}
