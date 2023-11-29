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

        // Implementação do serviço para manipular os resultados
    }
}
