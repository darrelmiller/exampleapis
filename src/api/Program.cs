using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ExamplesContext>(options => options.UseInMemoryDatabase("Examples"));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ExamplesContext>();
    var seeder = new DataSeeder(context);
    await seeder.SeedData();
}


app.UseHttpsRedirection();

app.MapGet("/customers",Operations.GetCustomers);
app.MapPost("/customers",Operations.CreateCustomer);
app.MapPatch("/customers",Operations.UpdateCustomer);



app.Run();



public static class Operations {

    public static IResult GetCustomers(ExamplesContext dataContext, [FromQuery] string? name = null) {
        List<Customer> customers = []; 

        if (name != null) {
            customers = dataContext.Customers.Where(c => c.Name.Contains(name)).ToList();
        } else {
            customers = dataContext.Customers.ToList();
        }
        return Results.Ok(customers);
    }

    public static IResult CreateCustomer(ExamplesContext dataContext, [FromBody]Customer customer) {

        customer.Id = dataContext.Customers.Max(c => c.Id) + 1;
        customer.CreatedDateTime = DateTime.Now;
        
        dataContext.Customers.Add(customer);
        dataContext.SaveChanges();
        return Results.Created($"/customers/{customer.Id}", customer);
    }

    public static IResult UpdateCustomer(ExamplesContext dataContext, [FromBody]Customer customer) {
        dataContext.Customers.Update(customer);
        dataContext.SaveChanges();
        return Results.Ok(customer);
    }
}




