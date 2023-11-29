using AluraRpa.Infrastructure;

namespace AluraRpa.Application
{
    public class BuscaAluraService
    {
        private readonly AluraWebScrapingService _scrapingService;
        private readonly ResultadosService _resultadosService;

        public BuscaAluraService(AluraWebScrapingService scrapingService, ResultadosService resultadosService)
        {
            _scrapingService = scrapingService;
            _resultadosService = resultadosService;
        }

        // Implementação do serviço de busca na Alura
        public void RealizarBusca(string termoBusca)
        {
            _scrapingService.RealizarWebScraping(termoBusca);

            // Lógica para manipular os resultados e salvar no banco de dados utilizando _resultadosService
        }
    }
}
