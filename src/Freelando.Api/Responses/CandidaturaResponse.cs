using Freelando.Modelo;

namespace Freelando.Api.Responses;

public record CandidaturaResponse(Guid Id, double? ValorProposto, string? DescricaoProposta, ProfissionalResponse? Profissional, ServicoResponse? Servico, string? DuracaoProposta, string? Status);
