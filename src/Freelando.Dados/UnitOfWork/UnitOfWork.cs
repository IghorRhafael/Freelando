using Freelando.Dados.Repository;
using Freelando.Dados.Repository.Interfaces;

namespace Freelando.Dados.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private CandidaturaRepository? _candidaturaRepository;
    private ClienteRepository? _clienteRepository;
    private ContratoRepository? _contratoRepository;
    private EspecialidadeRepository? _especialidadeRepository;
    private ProfissionalRepository? _profissionalRepository;
    private ProjetoRepository? _projetoRepository;
    private ServicoRepository? _servicoRepository;

    public ICandidaturaRepository CandidaturaRepository
    {
        get
        {
            if (_candidaturaRepository == null)
            {
                _candidaturaRepository = new CandidaturaRepository(_context ?? throw new ArgumentNullException(nameof(_context)));
            }
            return _candidaturaRepository;
        }
    }

    public IClienteRepository ClienteRepository
    {
        get
        {
            if (_clienteRepository == null)
            {
                _clienteRepository = new ClienteRepository(_context ?? throw new ArgumentNullException(nameof(_context)));
            }
            return _clienteRepository;
        }
    }

    public IContratoRepository ContratoRepository
    {
        get
        {
            if (_contratoRepository == null)
            {
                _contratoRepository = new ContratoRepository(_context ?? throw new ArgumentNullException(nameof(_context)));
            }
            return _contratoRepository;
        }
    }

    public IEspecialidadeRepository EspecialidadeRepository
    {
        get
        {
            if (_especialidadeRepository == null)
            {
                _especialidadeRepository = new EspecialidadeRepository(_context ?? throw new ArgumentNullException(nameof(_context)));
            }
            return _especialidadeRepository;
        }
    }

    public IProfissionalRepository ProfissionalRepository
    {
        get
        {
            if (_profissionalRepository == null)
            {
                _profissionalRepository = new ProfissionalRepository(_context ?? throw new ArgumentNullException(nameof(_context)));
            }
            return _profissionalRepository;
        }
    }

    public IProjetoRepository ProjetoRepository
    {
        get
        {
            if (_projetoRepository == null)
            {
                _projetoRepository = new ProjetoRepository(_context ?? throw new ArgumentNullException(nameof(_context)));
            }
            return _projetoRepository;
        }
    }

    public IServicoRepository ServicoRepository
    {
        get
        {
            if (_servicoRepository == null)
            {
                _servicoRepository = new ServicoRepository(_context ?? throw new ArgumentNullException(nameof(_context)));
            }
            return _servicoRepository;
        }
    }

    public FreelandoContext contexto => _context!;

    public FreelandoContext? _context;

    public UnitOfWork(FreelandoContext? context)
    {
        this._context = context;
    }

    public async Task Commit()
    {
        if (_context != null)
        {
            await _context.SaveChangesAsync();
        }
        else
        {
            throw new ArgumentNullException(nameof(_context));
        }
    }

    public void Dispose()
    {
        if (_context != null)
        {
            _context.Dispose();
        }
    }

}
