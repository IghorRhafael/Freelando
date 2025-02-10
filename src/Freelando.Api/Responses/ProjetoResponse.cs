using Freelando.Modelo;

namespace Freelando.Api.Responses;

public record ProjetoResponse(Guid Id, string? Titulo, string? Descricao, StatusProjeto? Status);
