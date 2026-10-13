using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GreenShop.Models;

public partial class GreenShopContext : DbContext
{
    public GreenShopContext()
    {
    }

    public GreenShopContext(DbContextOptions<GreenShopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<NewDetail> NewDetails { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductCategory> ProductCategories { get; set; }

    public virtual DbSet<ProductSize> ProductSizes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<ShipmentDetail> ShipmentDetails { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserVoucher> UserVouchers { get; set; }

    public virtual DbSet<Voucher> Vouchers { get; set; }

    public DbSet<Tag> Tags { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
        {
            return;
        }

        global::GreenShop.DotEnvFile.Load();
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__MyConnect");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "Connection string 'MyConnect' is missing. Set ConnectionStrings__MyConnect in the .env file or the system environment.");
        }

        optionsBuilder.UseSqlServer(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cart>(entity =>
        {
            entity.ToTable("Cart");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Product).WithMany(p => p.Carts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.User).WithMany(p => p.Carts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.ProductSize).WithMany()
                .HasForeignKey(d => d.ProductSizeId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Category");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.ToTable("Comment", tb =>
                {
                    tb.HasTrigger("trg_UpdateProductRate_AfterDeleteComment");
                    tb.HasTrigger("trg_UpdateProductRate_AfterInsertComment");
                    tb.HasTrigger("trg_UpdateProductRate_AfterUpdateComment");
                });

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CommentedAt).HasColumnName("Commented_At");

            entity.HasOne(d => d.Product).WithMany(p => p.Comments)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<NewDetail>(entity =>
        {
            entity.ToTable("New_Details");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.New).WithMany(p => p.NewDetails)
                .HasForeignKey(d => d.NewId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasColumnName("Created_At");

            entity.HasOne(d => d.User).WithMany(p => p.News)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Order");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt).HasColumnName("Create_at");

            entity.HasOne(d => d.ShipmentDetails).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ShipmentDetailsId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.ToTable("Order_Details");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.ToTable("Product_Category");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Category).WithMany(p => p.ProductCategories)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Product).WithMany(p => p.ProductCategories)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<ShipmentDetail>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Receiver).HasColumnName("receiver");

            entity.HasOne(d => d.User).WithMany(p => p.ShipmentDetails)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt).HasColumnName("Create_at");
            entity.Property(e => e.Dob).HasColumnName("DOB");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<UserVoucher>(entity =>
        {
            entity.ToTable("User_Voucher");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.User).WithMany(p => p.UserVouchers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Voucher).WithMany(p => p.UserVouchers)
                .HasForeignKey(d => d.VoucherId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Voucher>(entity =>
        {
            entity.ToTable("Voucher");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt).HasColumnName("Create_at");
            entity.Property(e => e.ExpireAt).HasColumnName("Expire_at");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.ToTable("Tag"); 
           
        });

        modelBuilder.Entity<ProductSize>(entity =>
        {
            entity.ToTable("ProductSize");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Product).WithMany(p => p.ProductSizes)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
