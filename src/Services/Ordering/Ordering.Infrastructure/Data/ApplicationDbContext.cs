
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Data;
using Ordering.Domain.Models;
using System.Reflection;

namespace Ordering.Infrastructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options)
    {
    }
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();


    // we can configure EF core entities on model creating method. 
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        // this method scans the assembly for all classes that
        // implement IEntity type configuration and apply state 
        // configurations, and this method is called within the all 
        // model creating. 

        base.OnModelCreating(builder);
    }

}
