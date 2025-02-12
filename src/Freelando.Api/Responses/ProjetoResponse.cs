using Freelando.Modelo;
using Freelando.Modelos;

namespace Freelando.Api.Responses;

public record ProjetoResponse(Guid Id, string? Titulo, string? Descricao, string? Status, ClienteResponse Cliente, ICollection<EspecialidadeResponse> Especialidades, Vigencia Vigencia);
