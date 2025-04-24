namespace Infrastructure.Entity;
public partial class order_detail {
  public short order_id { get; set; }

  public short product_id { get; set; }

  public float unit_price { get; set; }

  public short quantity { get; set; }

  public float discount { get; set; }

  public virtual order order { get; set; } = null!;

  public virtual product product { get; set; } = null!;
}
