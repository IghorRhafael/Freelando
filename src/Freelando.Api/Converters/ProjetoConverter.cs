using Freelando.Api.Requests;
using Freelando.Api.Responses;
using Freelando.Modelo;
using Freelando.Modelos;

namespace Freelando.Api.Converters;

public class ProjetoConverter
{
    private ClienteConverter? _clienteConverter;
    private EspecialidadeConverter? _especialidadeConverter;
    private ServicoConverter? _servicoConverter;
    public ProjetoResponse EntityToResponse(Projeto projeto)
    {
        _clienteConverter = new ClienteConverter();
        _especialidadeConverter = new EspecialidadeConverter();
        

        return (projeto is null)
            ? new ProjetoResponse(Guid.Empty, "", "", StatusProjeto.Disponivel.ToString(), null, new List<EspecialidadeResponse>(), new Vigencia())
            : new ProjetoResponse(projeto.Id, projeto.Titulo, projeto.Descricao, projeto.Status.ToString(), _clienteConverter.EntityToResponse(projeto.Cliente), _especialidadeConverter.EntityListToResponseList(projeto.Especialidades!), projeto.Vigencia);
    }

    public Projeto RequestToEntity(ProjetoRequest projetoRequest)
    {
        _especialidadeConverter = new EspecialidadeConverter();
        _servicoConverter = new ServicoConverter();

        return (projetoRequest is null)
            ? new Projeto(Guid.Empty, "", "", StatusProjeto.Disponivel, new Cliente(Guid.Empty, "", "", "", "", new List<Projeto>()), new List<Especialidade>(), new List<Servico>(), new Vigencia())
            : new Projeto(projetoRequest.Id, projetoRequest.Titulo!, projetoRequest.Descricao!, projetoRequest.Status, new Cliente(), _especialidadeConverter.RequestListToEntityList(projetoRequest.Especialidades), null, projetoRequest.Vigencia);
    }

    public ICollection<ProjetoResponse> EntityListToResponseList(IEnumerable<Projeto>? projetos)
    {
        return (projetos is null)
            ? new List<ProjetoResponse>()
            : projetos.Select(p => EntityToResponse(p)).ToList();
    }

    public ICollection<Projeto> RequestListToEntityList(IEnumerable<ProjetoRequest>? projetos)
    {
        return (projetos is null)
            ? new List<Projeto>()
            : projetos.Select(a => RequestToEntity(a)).ToList();
    }
}