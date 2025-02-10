using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace Freelando.Modelo;

public class Candidatura
{
    public Guid Id { get; set; }
    public double ValorProposto { get; set; }
    public string? DescricaoProposta { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public DuracaoEmDias DuracaoProposta { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public StatusCandidatura Status { get; set; }
}