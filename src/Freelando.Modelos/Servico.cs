namespace Freelando.Modelo;
public class Servico
{
    public Guid Id { get; set; }
    public string? Titulo { get; set; }
    public string? Descricao { get; set; }
    public StatusServico Status { get; set; }

}
