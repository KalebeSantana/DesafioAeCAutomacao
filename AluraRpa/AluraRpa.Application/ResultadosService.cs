using AluraRpa.Domain;
using AluraRpa.Infrastructure;

namespace AluraRpa.Application
{
    public class ResultadosService
    {
        private readonly BancoDeDadosRepository _repository;

        public ResultadosService(BancoDeDadosRepository repository)
        {
            _repository = repository;
        }

        public void SalvarResultado(ResultadoBusca resultado)
        {
            // Lógica para salvar o resultado no banco de dados usando o _repository
            _repository.GravarResultado(resultado);
            
        }
    }
}
