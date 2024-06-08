using Kolokwium_s24242.Configurations;
using Kolokwium_s24242.Models;
using Microsoft.EntityFrameworkCore;

namespace Kolokwium_s24242.Context;

public class ApbdContext :DbContext
{
 public ApbdContext()
 {
  
 }

 public ApbdContext(DbContextOptions<ApbdContext> options) : base(options)
 {
  
 }
 
 public DbSet<Client> Clients { get; set; }
 public DbSet<Sale> Sales { get; set; }
 public DbSet<Subscription> Subscriptions { get; set; }
 public DbSet<Discount> Discounts { get; set; }
 public DbSet<Payment> Payments { get; set; }
 protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
 {
  optionsBuilder.UseSqlServer("Server=tcp:s24242.database.windows.net,1433;Initial Catalog=kolokwium ;Persist Security Info=False;User ID={xxx};Password={xxx};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"); 
 }

 protected override void OnModelCreating(ModelBuilder modelBuilder)
 {
  base.OnModelCreating(modelBuilder);

  modelBuilder.ApplyConfiguration(new ClientConfiguration());
  modelBuilder.ApplyConfiguration(new SalesConfiguration());
  modelBuilder.ApplyConfiguration(new SubscriptionConfiguration());
  modelBuilder.ApplyConfiguration(new DiscountConfiguration());
  modelBuilder.ApplyConfiguration(new PaymentConfiguration());
 }
}
