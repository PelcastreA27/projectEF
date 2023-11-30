using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using projectef;

var builder = WebApplication.CreateBuilder(args);

//Esta configuración solo es para una bd en memoria
//builder.Services.AddDbContext<TareasContext>(p => p.UseInMemoryDatabase("TareasDB"));

//Aquí  se crea a la bd de SqlServer
builder.Services.AddSqlServer<TareasContext>(builder.Configuration.GetConnectionString("conexionTareas"));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/dbconexion", async ([FromServices] TareasContext dbContext) =>
{
    //El método EnsureCreated verifica que la bd este creada sino la crea.
    dbContext.Database.EnsureCreated();
    return Results.Ok("Bd en memoria:" + dbContext.Database.IsInMemory());
});

app.Run();
