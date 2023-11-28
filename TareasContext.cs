using Microsoft.EntityFrameworkCore;
using projectef.Models;

namespace projectef;

public class TareasContext: DbContext
{
    public DbSet<Categoria>Categorias{get;set;}
    public DbSet<Tarea>Tareas{get;set;}

    //Método base del constructor de entity
    public TareasContext(DbContextOptions<TareasContext>options): base(options){}
}