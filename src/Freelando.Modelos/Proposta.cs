using System.ComponentModel.DataAnnotations.Schema;

[Table("TB_Propostas")]
public class Proposta
{

    [Column("Id_Proposta")]
    public Guid Id { get; set; }
    [Column("Id_Profissional")]
    public Guid ProfissionalId { get; set; }
    [Column("Id_Projeto")]
    public Guid ProjetoId { get; set; }

    [Column("Valor_Proposta")]
    public decimal ValorProposta { get; set; }

    [Column("Data_Proposta")]
    public DateTime DataProposta { get; set; }

    [Column("Mensagem")]
    public string? Mensagem { get; set; }

    [Column("Prazo_Entrega")]
    public DateTime PrazoEntrega { get; set; }
}