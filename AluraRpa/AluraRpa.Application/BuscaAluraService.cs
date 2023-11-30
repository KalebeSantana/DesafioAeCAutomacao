using AluraRpa.Infrastructure;
using System;
using System.Collections.Generic;

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
        public void RealizarBusca(string termoBusca)
        {
            try
            {
                // Obter os resultados da classe AluraWebScrapingService
                var resultados = _scrapingService.RealizarWebScraping(termoBusca);

                // Verificar se há resultados
                if (resultados.Count == 0)
                {
                    nenhumResultado = true;
                    Console.WriteLine($"Nenhum resultado encontrado para o termo '{termoBusca}'.");
                    return;
                }

                // Lógica para manipular e salvar os resultados no banco de dados
                foreach (var resultado in resultados)
                {
                    // Chamar o ResultadosService para salvar o resultado no banco de dados
                    _resultadosService.SalvarResultado(resultado);
                }

                Console.WriteLine($"Foram encontrados {resultados.Count} resultados para o termo '{termoBusca}'.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro durante a busca na Alura: {ex.Message}");
            }
        }
    }
}
