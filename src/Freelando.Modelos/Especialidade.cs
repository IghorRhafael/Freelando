using System.ComponentModel.DataAnnotations.Schema;

namespace Freelando.Modelo;
public class Especialidade
{
    public Guid Id { get; set; }
    public string? Descricao { get; set; }

}
