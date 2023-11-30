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
        modelBuilder.Entity<Categoria>(categoria =>
        {
            categoria.ToTable("Categoria");
            categoria.HasKey(p=> p.CategoriaId);

            categoria.Property(p=> p.Nombre).IsRequired().HasMaxLength(150);

            categoria.Property(p=> p.Descripcion);
        });

        modelBuilder.Entity<Tarea>(tarea =>
        {
            tarea.ToTable("Tarea");
            tarea.HasKey(t => t.TareaId);
            tarea.HasOne(t => t.Categoria).WithMany(t => t.Tareas).HasForeignKey(t => t.CategoriaId);
            tarea.Property(t => t.Titulo).IsRequired().HasMaxLength(200);
            tarea.Property(t => t.Descripcion);
            tarea.Property(t => t.PrioridadTarea);
            tarea.Property(t => t.FechaCreacion);

            //Ignorar una propiedad para no crearla en la bd
            tarea.Ignore(t => t.Resumen);
        });
    }
}