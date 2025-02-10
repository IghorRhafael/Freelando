using Freelando.Api.Responses;
using Freelando.Modelo;

namespace Freelando.Api.Requests;

public record CandidaturaRequest(Guid Id, double? ValorProposto, string? DescricaoProposta, ProfissionalRequest? Profissional, ServicoRequest? Servico, string? DuracaoProposta, string? Status);
