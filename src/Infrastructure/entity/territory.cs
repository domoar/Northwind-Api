namespace Infrastructure.Entity;
#pragma warning disable CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
#pragma warning disable IDE1006 // Naming Styles
public partial class territory {
#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
#pragma warning disable IDE1006 // Naming Styles
  public string territory_id { get; set; } = null!;
#pragma warning restore IDE1006 // Naming Styles

#pragma warning disable IDE1006 // Naming Styles
  public string territory_description { get; set; } = null!;
#pragma warning restore IDE1006 // Naming Styles

#pragma warning disable IDE1006 // Naming Styles
  public short region_id { get; set; }
#pragma warning restore IDE1006 // Naming Styles

#pragma warning disable IDE1006 // Naming Styles
  public virtual region region { get; set; } = null!;
#pragma warning restore IDE1006 // Naming Styles

#pragma warning disable IDE1006 // Naming Styles
  public virtual ICollection<employee> employees { get; set; } = [];
#pragma warning restore IDE1006 // Naming Styles
}
