using System.ComponentModel.DataAnnotations.Schema;

namespace Freelando.Modelo;
[Table("TB_Especialidades")]
public class Especialidade
{
    [Column("ID_Especialidade")]
    public Guid Id { get; set; }
    [Column("DS_Especialidade")]
    public string? Descricao { get; set; }

}
