using System;
using System.Collections.Generic;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.context;

public partial class NorthwindContext : DbContext
{
    public NorthwindContext(DbContextOptions<NorthwindContext> options)
        : base(options)
    {
    }

    public virtual DbSet<category> categories { get; set; }

    public virtual DbSet<customer> customers { get; set; }

    public virtual DbSet<customer_demographic> customer_demographics { get; set; }

    public virtual DbSet<employee> employees { get; set; }

    public virtual DbSet<order> orders { get; set; }

    public virtual DbSet<order_detail> order_details { get; set; }

    public virtual DbSet<product> products { get; set; }

    public virtual DbSet<region> regions { get; set; }

    public virtual DbSet<shipper> shippers { get; set; }

    public virtual DbSet<supplier> suppliers { get; set; }

    public virtual DbSet<territory> territories { get; set; }

    public virtual DbSet<us_state> us_states { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<category>(entity =>
        {
            entity.HasKey(e => e.category_id).HasName("pk_categories");

            entity.ToTable("categories", "northwind");

            entity.Property(e => e.category_id).ValueGeneratedNever();
            entity.Property(e => e.category_name).HasMaxLength(15);
        });

        modelBuilder.Entity<customer>(entity =>
        {
            entity.HasKey(e => e.customer_id).HasName("pk_customers");

            entity.ToTable("customers", "northwind");

            entity.Property(e => e.customer_id).HasMaxLength(5);
            entity.Property(e => e.address).HasMaxLength(60);
            entity.Property(e => e.city).HasMaxLength(15);
            entity.Property(e => e.company_name).HasMaxLength(40);
            entity.Property(e => e.contact_name).HasMaxLength(30);
            entity.Property(e => e.contact_title).HasMaxLength(30);
            entity.Property(e => e.country).HasMaxLength(15);
            entity.Property(e => e.fax).HasMaxLength(24);
            entity.Property(e => e.phone).HasMaxLength(24);
            entity.Property(e => e.postal_code).HasMaxLength(10);
            entity.Property(e => e.region).HasMaxLength(15);

            entity.HasMany(d => d.customer_types).WithMany(p => p.customers)
                .UsingEntity<Dictionary<string, object>>(
                    "customer_customer_demo",
                    r => r.HasOne<customer_demographic>().WithMany()
                        .HasForeignKey("customer_type_id")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_customer_customer_demo_customer_demographics"),
                    l => l.HasOne<customer>().WithMany()
                        .HasForeignKey("customer_id")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_customer_customer_demo_customers"),
                    j =>
                    {
                        j.HasKey("customer_id", "customer_type_id").HasName("pk_customer_customer_demo");
                        j.ToTable("customer_customer_demo", "northwind");
                        j.IndexerProperty<string>("customer_id").HasMaxLength(5);
                        j.IndexerProperty<string>("customer_type_id").HasMaxLength(5);
                    });
        });

        modelBuilder.Entity<customer_demographic>(entity =>
        {
            entity.HasKey(e => e.customer_type_id).HasName("pk_customer_demographics");

            entity.ToTable("customer_demographics", "northwind");

            entity.Property(e => e.customer_type_id).HasMaxLength(5);
        });

        modelBuilder.Entity<employee>(entity =>
        {
            entity.HasKey(e => e.employee_id).HasName("pk_employees");

            entity.ToTable("employees", "northwind");

            entity.Property(e => e.employee_id).ValueGeneratedNever();
            entity.Property(e => e.address).HasMaxLength(60);
            entity.Property(e => e.city).HasMaxLength(15);
            entity.Property(e => e.country).HasMaxLength(15);
            entity.Property(e => e.extension).HasMaxLength(4);
            entity.Property(e => e.first_name).HasMaxLength(10);
            entity.Property(e => e.home_phone).HasMaxLength(24);
            entity.Property(e => e.last_name).HasMaxLength(20);
            entity.Property(e => e.photo_path).HasMaxLength(255);
            entity.Property(e => e.postal_code).HasMaxLength(10);
            entity.Property(e => e.region).HasMaxLength(15);
            entity.Property(e => e.title).HasMaxLength(30);
            entity.Property(e => e.title_of_courtesy).HasMaxLength(25);

            entity.HasOne(d => d.reports_toNavigation).WithMany(p => p.Inversereports_toNavigation)
                .HasForeignKey(d => d.reports_to)
                .HasConstraintName("fk_employees_employees");

            entity.HasMany(d => d.territories).WithMany(p => p.employees)
                .UsingEntity<Dictionary<string, object>>(
                    "employee_territory",
                    r => r.HasOne<territory>().WithMany()
                        .HasForeignKey("territory_id")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_employee_territories_territories"),
                    l => l.HasOne<employee>().WithMany()
                        .HasForeignKey("employee_id")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_employee_territories_employees"),
                    j =>
                    {
                        j.HasKey("employee_id", "territory_id").HasName("pk_employee_territories");
                        j.ToTable("employee_territories", "northwind");
                        j.IndexerProperty<string>("territory_id").HasMaxLength(20);
                    });
        });

        modelBuilder.Entity<order>(entity =>
        {
            entity.HasKey(e => e.order_id).HasName("pk_orders");

            entity.ToTable("orders", "northwind");

            entity.Property(e => e.order_id).ValueGeneratedNever();
            entity.Property(e => e.customer_id).HasMaxLength(5);
            entity.Property(e => e.ship_address).HasMaxLength(60);
            entity.Property(e => e.ship_city).HasMaxLength(15);
            entity.Property(e => e.ship_country).HasMaxLength(15);
            entity.Property(e => e.ship_name).HasMaxLength(40);
            entity.Property(e => e.ship_postal_code).HasMaxLength(10);
            entity.Property(e => e.ship_region).HasMaxLength(15);

            entity.HasOne(d => d.customer).WithMany(p => p.orders)
                .HasForeignKey(d => d.customer_id)
                .HasConstraintName("fk_orders_customers");

            entity.HasOne(d => d.employee).WithMany(p => p.orders)
                .HasForeignKey(d => d.employee_id)
                .HasConstraintName("fk_orders_employees");

            entity.HasOne(d => d.ship_viaNavigation).WithMany(p => p.orders)
                .HasForeignKey(d => d.ship_via)
                .HasConstraintName("fk_orders_shippers");
        });

        modelBuilder.Entity<order_detail>(entity =>
        {
            entity.HasKey(e => new { e.order_id, e.product_id }).HasName("pk_order_details");

            entity.ToTable("order_details", "northwind");

            entity.HasOne(d => d.order).WithMany(p => p.order_details)
                .HasForeignKey(d => d.order_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_order_details_orders");

            entity.HasOne(d => d.product).WithMany(p => p.order_details)
                .HasForeignKey(d => d.product_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_order_details_products");
        });

        modelBuilder.Entity<product>(entity =>
        {
            entity.HasKey(e => e.product_id).HasName("pk_products");

            entity.ToTable("products", "northwind");

            entity.Property(e => e.product_id).ValueGeneratedNever();
            entity.Property(e => e.product_name).HasMaxLength(40);
            entity.Property(e => e.quantity_per_unit).HasMaxLength(20);

            entity.HasOne(d => d.category).WithMany(p => p.products)
                .HasForeignKey(d => d.category_id)
                .HasConstraintName("fk_products_categories");

            entity.HasOne(d => d.supplier).WithMany(p => p.products)
                .HasForeignKey(d => d.supplier_id)
                .HasConstraintName("fk_products_suppliers");
        });

        modelBuilder.Entity<region>(entity =>
        {
            entity.HasKey(e => e.region_id).HasName("pk_region");

            entity.ToTable("region", "northwind");

            entity.Property(e => e.region_id).ValueGeneratedNever();
            entity.Property(e => e.region_description).HasMaxLength(60);
        });

        modelBuilder.Entity<shipper>(entity =>
        {
            entity.HasKey(e => e.shipper_id).HasName("pk_shippers");

            entity.ToTable("shippers", "northwind");

            entity.Property(e => e.shipper_id).ValueGeneratedNever();
            entity.Property(e => e.company_name).HasMaxLength(40);
            entity.Property(e => e.phone).HasMaxLength(24);
        });

        modelBuilder.Entity<supplier>(entity =>
        {
            entity.HasKey(e => e.supplier_id).HasName("pk_suppliers");

            entity.ToTable("suppliers", "northwind");

            entity.Property(e => e.supplier_id).ValueGeneratedNever();
            entity.Property(e => e.address).HasMaxLength(60);
            entity.Property(e => e.city).HasMaxLength(15);
            entity.Property(e => e.company_name).HasMaxLength(40);
            entity.Property(e => e.contact_name).HasMaxLength(30);
            entity.Property(e => e.contact_title).HasMaxLength(30);
            entity.Property(e => e.country).HasMaxLength(15);
            entity.Property(e => e.fax).HasMaxLength(24);
            entity.Property(e => e.phone).HasMaxLength(24);
            entity.Property(e => e.postal_code).HasMaxLength(10);
            entity.Property(e => e.region).HasMaxLength(15);
        });

        modelBuilder.Entity<territory>(entity =>
        {
            entity.HasKey(e => e.territory_id).HasName("pk_territories");

            entity.ToTable("territories", "northwind");

            entity.Property(e => e.territory_id).HasMaxLength(20);
            entity.Property(e => e.territory_description).HasMaxLength(60);

            entity.HasOne(d => d.region).WithMany(p => p.territories)
                .HasForeignKey(d => d.region_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_territories_region");
        });

        modelBuilder.Entity<us_state>(entity =>
        {
            entity.HasKey(e => e.state_id).HasName("pk_usstates");

            entity.ToTable("us_states", "northwind");

            entity.Property(e => e.state_id).ValueGeneratedNever();
            entity.Property(e => e.state_abbr).HasMaxLength(2);
            entity.Property(e => e.state_name).HasMaxLength(100);
            entity.Property(e => e.state_region).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
