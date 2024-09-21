using System.Dynamic;

using Microsoft.EntityFrameworkCore;

public class ExamplesContext : DbContext
{

    public ExamplesContext(DbContextOptions<ExamplesContext> options) : base(options)
    {
    }
    public DbSet<Customer> Customers { get; set; }

}
 
public class Customer
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public bool isActive { get; set; }  = true;

    public DateTime CreatedDateTime { get; set; }
}


public class DataSeeder
{
    private ExamplesContext examplesContext;

    public DataSeeder(ExamplesContext db)
    {
        examplesContext = db;
    }

    public async Task SeedData()
    {
        if (examplesContext.Customers.Any())
        {
            return; // DB has been seeded
        }

        await examplesContext.Customers.AddRangeAsync(GetDefaultCustomers());
        await examplesContext.SaveChangesAsync();
    }

    private static List<Customer> GetDefaultCustomers()
    {
        return [ 
            new() {Id = 1, Name = "Contoso", isActive = false, CreatedDateTime = DateTime.Now.AddDays(-10)},
            new() {Id = 2, Name = "Fabrikam", isActive = true, CreatedDateTime = DateTime.Now.AddDays(-5)},
            new() {Id = 3, Name = "AdventureWorks", isActive = true, CreatedDateTime = DateTime.Now.AddDays(-2)},
            new() {Id = 4, Name = "Northwind", isActive = false, CreatedDateTime = DateTime.Now.AddDays(-1)} 
        ];
    }
}
