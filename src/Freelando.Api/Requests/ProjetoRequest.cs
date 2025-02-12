using Freelando.Modelo;
using Freelando.Modelos;

namespace Freelando.Api.Requests;

public record ProjetoRequest(Guid Id, string? Titulo, string? Descricao, StatusProjeto Status, ClienteRequest
    Cliente, ICollection<EspecialidadeRequest> Especialidades, Vigencia Vigencia);

