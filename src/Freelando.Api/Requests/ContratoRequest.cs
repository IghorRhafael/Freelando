using Freelando.Modelo;

namespace Freelando.Api.Requests;

public record ContratoRequest(Guid Id, double? Valor);
