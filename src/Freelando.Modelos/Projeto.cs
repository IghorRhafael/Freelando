using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Freelando.Modelo;
public class Projeto
{
    public Guid Id { get; set; }
    public string? Titulo { get; set; }
    public  string? Descricao { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public StatusProjeto Status { get; set; }
}
