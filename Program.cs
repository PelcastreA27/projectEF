using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using projectef;
using projectef.Models;
using System.Xml.XPath;

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


app.MapGet("/api/tareas", async ([FromServices] TareasContext dbContext) =>
{
    return Results.Ok(dbContext.Tareas.Include(p => p.Categoria));
});

app.MapPost("/api/tareas", async ([FromServices] TareasContext dbContext, [FromBody] Tarea tarea) =>
{
    tarea.TareaId = Guid.NewGuid();
    tarea.FechaCreacion = DateTime.Now;
    await dbContext.AddAsync(tarea);
    // otra forma de agregar
   // await dbContext.Tareas.AddAsync(tarea);

   //Invocar el método para guardar los cambios en la bd
   await dbContext.SaveChangesAsync();

   return Results.Ok();

});

//Actualizar datos ya existentes
app.MapPut("/api/tareas/{id}", async ([FromServices] TareasContext dbContext, [FromBody] Tarea tarea,[FromRoute] Guid id) =>
{
    var tareaActual = dbContext.Tareas.Find(id);

    if(tareaActual != null)
    {
        tareaActual.CategoriaId = tarea.CategoriaId;
        tareaActual.Titulo = tarea.Titulo;
        tareaActual.PrioridadTarea = tarea.PrioridadTarea;
        tareaActual.Descripcion = tarea.Descripcion;

        await dbContext.SaveChangesAsync();

        return Results.Ok();
    }

   

   return Results.NotFound();

});

app.MapDelete("/api/tareas/{id}", async ([FromServices] TareasContext dbContext, [FromRoute] Guid id)=>
{
    var tareaActual = dbContext.Tareas.Find(id);

    if(tareaActual != null)
    {
        dbContext.Remove(tareaActual);
        await dbContext.SaveChangesAsync();

        return Results.Ok();
    }

    return Results.NotFound();
});

app.Run();
