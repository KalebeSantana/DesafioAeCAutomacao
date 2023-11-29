using AluraRpa.Application;
using AluraRpa.Infrastructure;

namespace AluraRpa.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Configuração da injeção de dependência
            var scrapingService = new AluraWebScrapingService();
            var repository = new BancoDeDadosRepository();
            var resultadosService = new ResultadosService(repository);
            var buscaService = new BuscaAluraService(scrapingService, resultadosService);

            // Lógica para interagir com o usuário e chamar os serviços
            buscaService.RealizarBusca("Jogos");
        }
    }
}
