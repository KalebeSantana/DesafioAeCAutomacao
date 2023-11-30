using AluraRpa.Infrastructure;
using AluraRpa.Shared;
using System;

namespace AluraRpa.Application
{
    /// <summary>
    /// Serviço responsável por realizar buscas na Alura.
    /// </summary>
    public class BuscaAluraService
    {
        private readonly AluraWebScrapingService _scrapingService;
        private readonly ResultadosService _resultadosService;
        public static bool nenhumResultado = false;

        /// <summary>
        /// Inicializa uma nova instância do serviço de busca na Alura.
        /// </summary>
        /// <param name="scrapingService">Serviço de scraping da Alura.</param>
        /// <param name="resultadosService">Serviço de manipulação de resultados.</param>
        public BuscaAluraService(AluraWebScrapingService scrapingService, ResultadosService resultadosService)
        {
            _scrapingService = scrapingService ?? throw new ArgumentNullException(nameof(scrapingService));
            _resultadosService = resultadosService ?? throw new ArgumentNullException(nameof(resultadosService));
        }

        /// <summary>
        /// Realiza uma busca na Alura com base no termo especificado.
        /// </summary>
        /// <param name="termoBusca">O termo de busca.</param>
        public void RealizarBusca(string termoBusca, Logger logger)
        {
            try
            {
                // Obter os resultados da classe AluraWebScrapingService
                var resultados = _scrapingService.RealizarWebScraping(termoBusca, logger);

                // Verificar se há resultados
                if (resultados.Count == 0)
                {
                    nenhumResultado = true;
                    logger.LogWarning($"Nenhum resultado encontrado para o termo '{termoBusca}'.");
                    Console.WriteLine($"Nenhum resultado encontrado para o termo '{termoBusca}'.");
                    return;
                }

                logger.LogInfo("Registrando resultados no banco de dados...");

                // Lógica para manipular e salvar os resultados no banco de dados
                foreach (var resultado in resultados)
                {
                    // Chamar o ResultadosService para salvar o resultado no banco de dados
                    _resultadosService.SalvarResultado(resultado, logger);
                }

                Console.WriteLine($"Foram encontrados {resultados.Count} resultados para o termo '{termoBusca}'.");
            }
            catch (Exception ex)
            {
                logger.LogError($"Erro durante a busca na Alura: {ex.Message}");
                Console.WriteLine($"Erro durante a busca na Alura: {ex.Message}");
            }
        }
    }
}
