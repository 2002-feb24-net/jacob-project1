using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Project1.Domain.Model
{
    public partial class Project1Context : DbContext
    {
        public Project1Context()
        {
        }

        public Project1Context(DbContextOptions<Project1Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<StoreLocation> StoreLocation { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer", "P0");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(128);
            });

            modelBuilder.Entity<Orders>(entity =>
            {
                entity.ToTable("Orders", "P0");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.OrderTime)
                    .IsRequired()
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.StoreLocationId).HasColumnName("StoreLocationID");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Customer_ID");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_ID");

                entity.HasOne(d => d.StoreLocation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.StoreLocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StoreLocation_ID");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product", "P0");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Price).HasColumnType("money");

                entity.Property(e => e.StoreLocationId).HasColumnName("StoreLocationID");

                entity.HasOne(d => d.StoreLocation)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.StoreLocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StoreProduct_ID");
            });

            modelBuilder.Entity<StoreLocation>(entity =>
            {
                entity.ToTable("StoreLocation", "P0");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.LocationName)
                    .IsRequired()
                    .HasMaxLength(128);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
