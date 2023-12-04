namespace projectef.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class Categoria
{
    //[Key]
    public Guid CategoriaId {get;set;}

    //[Required]
    //[MaxLength(150)]
    public string Nombre{get;set;}
    public string Descripcion{get;set;}

    public int Peso{get;set;}

    [JsonIgnore] //se agrega por motivos del registro que se quiere obtener
    public virtual ICollection<Tarea> Tareas{get;set;}
}