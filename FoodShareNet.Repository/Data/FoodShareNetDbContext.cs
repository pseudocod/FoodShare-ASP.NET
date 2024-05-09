using Microsoft.EntityFrameworkCore;
using FoodShareNet.Domain.Entities;
using FoodShareNet.Application.Interfaces;

namespace FoodShareNet.Repository.Data;

public class FoodShareNetDbContext : DbContext, IFoodShareDbContext
{
    public FoodShareNetDbContext(DbContextOptions<FoodShareNetDbContext> options)
        : base(options)
    {
    }

    public DbSet<Courier> Couriers { get; set; }
    public DbSet<Beneficiary> Beneficiaries { get; set; }
    public DbSet<Donor> Donors { get; set; }
    public DbSet<Donation> Donations { get; set; }
    public DbSet<DonationStatus> DonationStatuses { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderStatus> OrderStatuses { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<Product> Products { get; set; }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>().HasData(
                new City { Id = 1, Name = "Bucuresti" },
                new City { Id = 2, Name = "Cluj-Napoca" },
                new City { Id = 3, Name = "Timisoara" },
                new City { Id = 4, Name = "Sibiu" },
                new City { Id = 5, Name = "Iasi" }
            );

        modelBuilder.Entity<Courier>().HasData(
            new Courier { Id = 1, Name = "DPD", Price=20 },
            new Courier { Id = 2, Name = "DHL", Price = 15 },
            new Courier { Id = 3, Name = "GLS", Price = 17.5M }
        );

        modelBuilder.Entity<DonationStatus>().HasData(
            new DonationStatus { Id = 1, Name = "Pending" },
            new DonationStatus { Id = 2, Name = "Approved" },
            new DonationStatus { Id = 3, Name = "Rejected" }
        );

        modelBuilder.Entity<OrderStatus>().HasData(
            new OrderStatus { Id = 1, Name = "Unconfirmed" },
            new OrderStatus { Id = 2, Name = "Confirmed" },
            new OrderStatus { Id = 3, Name = "InDelivery" },
            new OrderStatus { Id = 4, Name = "Delivered" }
        );

        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Tomatoes", Image = "tomato.jpg" },
            new Product { Id = 2, Name = "Potatoes" , Image = "potatoes.jpg"},
            new Product { Id = 3, Name = "Meat", Image = "meat.jpg" }
        );

        modelBuilder.Entity<Courier>()
            .Property(c => c.Price)
            .HasColumnType("decimal(18, 2)");

        modelBuilder.Entity<Order>()
         .HasOne(o => o.Donation)
         .WithMany()
         .HasForeignKey(o => o.DonationId)
         .OnDelete(DeleteBehavior.Restrict);


        base.OnModelCreating(modelBuilder);
    }
}