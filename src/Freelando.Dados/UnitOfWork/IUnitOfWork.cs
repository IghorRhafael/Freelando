using Freelando.Dados.Repository;
using Freelando.Dados.Repository.Interfaces;

namespace Freelando.Dados.UnitOfWork;

public interface IUnitOfWork
{
    ICandidaturaRepository CandidaturaRepository { get; }
    IClienteRepository ClienteRepository { get; }
    IContratoRepository ContratoRepository { get; }
    IEspecialidadeRepository EspecialidadeRepository { get; }
    IProfissionalRepository ProfissionalRepository { get; }
    IProjetoRepository ProjetoRepository { get; }
    IServicoRepository ServicoRepository { get; }

    FreelandoContext contexto { get; }

    Task Commit();
}
