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
            
            // Obter os resultados da classe AluraWebScrapingService
            var resultados = _scrapingService.RealizarWebScraping(termoBusca);

            // Lógica para manipular e salvar os resultados no banco de dados
            foreach (var resultado in resultados)
            {
                // Chamar o ResultadosService para salvar o resultado no banco de dados
                _resultadosService.SalvarResultado(resultado);
            }
        }
    }
}
