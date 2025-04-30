namespace Infrastructure.Entity;
#pragma warning disable CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
#pragma warning disable IDE1006 // Naming Styles
public partial class category {
#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
#pragma warning disable IDE1006 // Naming Styles
  public short category_id { get; set; }
#pragma warning restore IDE1006 // Naming Styles

#pragma warning disable IDE1006 // Naming Styles
  public string category_name { get; set; } = null!;
#pragma warning restore IDE1006 // Naming Styles

#pragma warning disable IDE1006 // Naming Styles
  public string? description { get; set; }
#pragma warning restore IDE1006 // Naming Styles

#pragma warning disable IDE1006 // Naming Styles
  public byte[]? picture { get; set; }
#pragma warning restore IDE1006 // Naming Styles

#pragma warning disable IDE1006 // Naming Styles
  public virtual ICollection<product> products { get; set; } = [];
#pragma warning restore IDE1006 // Naming Styles
}
