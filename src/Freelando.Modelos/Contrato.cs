using Freelando.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelando.Modelo;
public class Contrato
{
    public Guid Id { get; set; }
    public double Valor { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataEncerramento { get; set; }

}
