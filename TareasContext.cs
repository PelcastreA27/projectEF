using Microsoft.EntityFrameworkCore;
using projectef.Models;

namespace projectef;

public class TareasContext: DbContext
{
    public DbSet<Categoria>Categorias{get;set;}
    public DbSet<Tarea>Tareas{get;set;}

    //Método base del constructor de entity
    public TareasContext(DbContextOptions<TareasContext>options): base(options){}

    //Si se va a sobreescribir un metodo se tiene que colocar como protected
    //se sobrescribe un metodo unsandoo override con esto podemos cambiar el funcionamiento de este.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        List<Categoria> categoriasInit = new List<Categoria>();
        categoriasInit.Add(new Categoria(){CategoriaId = Guid.Parse("6c469833-5e9d-407d-b9ee-e76c4a981320"), Nombre="Actividades pendientes", Peso = 20});
        categoriasInit.Add(new Categoria(){CategoriaId = Guid.Parse("6c469833-5e9d-407d-b9ee-e76c4a981321"), Nombre="Actividades personales", Peso = 50});
       
        modelBuilder.Entity<Categoria>(categoria =>
        {
            categoria.ToTable("Categoria");
            categoria.HasKey(p=> p.CategoriaId);

            categoria.Property(p=> p.Nombre).IsRequired().HasMaxLength(150);

            categoria.Property(p=> p.Descripcion).IsRequired(false);

            categoria.Property(p => p.Peso);

            categoria.HasData(categoriasInit);
        });

        List<Tarea> tareasInit = new List<Tarea>();

        tareasInit.Add(new Tarea(){ TareaId = Guid.Parse("6c469833-5e9d-407d-b9ee-e76c4a981310"), CategoriaId = Guid.Parse("6c469833-5e9d-407d-b9ee-e76c4a981320"), PrioridadTarea = Prioridad.Media, Titulo = "Pago de servicios publicos", FechaCreacion = DateTime.Now });
        tareasInit.Add(new Tarea(){ TareaId = Guid.Parse("6c469833-5e9d-407d-b9ee-e76c4a981311"), CategoriaId = Guid.Parse("6c469833-5e9d-407d-b9ee-e76c4a981321"), PrioridadTarea = Prioridad.Baja, Titulo = "Terminar de ver pelicula en Netflix", FechaCreacion = DateTime.Now });
        
        modelBuilder.Entity<Tarea>(tarea =>
        {
            tarea.ToTable("Tarea");
            tarea.HasKey(t => t.TareaId);
            tarea.HasOne(t => t.Categoria).WithMany(t => t.Tareas).HasForeignKey(t => t.CategoriaId);
            tarea.Property(t => t.Titulo).IsRequired().HasMaxLength(200);
            tarea.Property(t => t.Descripcion).IsRequired(false);
            tarea.Property(t => t.PrioridadTarea);
            tarea.Property(t => t.FechaCreacion);

            //Ignorar una propiedad para no crearla en la bd
            tarea.Ignore(t => t.Resumen);

            tarea.HasData(tareasInit);
        });
    }
}