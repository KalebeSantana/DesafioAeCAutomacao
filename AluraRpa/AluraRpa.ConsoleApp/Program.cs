using AluraRpa.Application;
using AluraRpa.Infrastructure;
using System;

namespace AluraRpa.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("************************************************************");
            Console.WriteLine("*                     AluraRpa Console App                 *");
            Console.WriteLine("************************************************************");

            string resposta;

            do
            {
                Console.Write("\nDigite o termo que deseja pesquisar: ");
                string termoBusca = Console.ReadLine();

                // Configuração da injeção de dependência
                var scrapingService = new AluraWebScrapingService();
                var repository = new BancoDeDadosRepository(termoBusca); // Usando o termo da busca como nome da tabela
                var resultadosService = new ResultadosService(repository);
                var buscaService = new BuscaAluraService(scrapingService, resultadosService);

                Console.WriteLine("\nRealizando busca...\n");

                // Chamar os serviços
                buscaService.RealizarBusca(termoBusca);

                if (!BuscaAluraService.nenhumResultado)
                {
                    // Perguntar se o usuário deseja visualizar os resultados
                    Console.Write("Deseja visualizar os resultados? (S/N): ");
                    resposta = Console.ReadLine();

                    if (resposta?.ToUpper() == "S")
                    {
                        Console.Clear();
                        Console.WriteLine("************************************************************");
                        Console.WriteLine("*                      Resultados da Busca                 *");
                        Console.WriteLine("************************************************************");

                        // Chamar o método para mostrar os resultados
                        resultadosService.MostrarResultados(termoBusca);
                    }
                }

                // Perguntar se o usuário deseja pesquisar outro termo ou encerrar
                Console.Write("\nDeseja pesquisar outro termo? (S/N): ");
                resposta = Console.ReadLine();

                if (resposta?.ToUpper() != "S")
                {
                    Console.WriteLine("\nObrigado por utilizar o AluraRpa Console App. Até mais!");
                }

            } while (resposta?.ToUpper() == "S");
        }
    }
}
