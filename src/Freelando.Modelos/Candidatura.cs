using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Freelando.Modelo;

public class Candidatura
{
    public Guid Id { get; set; }
    public double ValorProposto { get; set; }
    public string? DescricaoProposta { get; set; }
    public DuracaoEmDias DuracaoProposta { get; set; }
    public StatusCandidatura Status { get; set; }
}