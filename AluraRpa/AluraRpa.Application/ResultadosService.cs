using AluraRpa.Domain;
using AluraRpa.Infrastructure;
using AluraRpa.Shared;
using System;

namespace AluraRpa.Application
{
    /// <summary>
    /// Serviço para manipulação e exibição de resultados.
    /// </summary>
    public class ResultadosService
    {
        private readonly BancoDeDadosRepository _repository;

        /// <summary>
        /// Construtor da classe ResultadosService.
        /// </summary>
        /// <param name="repository">Instância do BancoDeDadosRepository.</param>
        public ResultadosService(BancoDeDadosRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Salva um resultado no banco de dados.
        /// </summary>
        /// <param name="resultado">Resultado a ser salvo.</param>
        /// <param name="logger">Instância do Logger para registro de logs.</param>
        public void SalvarResultado(ResultadoBusca resultado, Logger logger)
        {
            // Lógica para salvar o resultado no banco de dados usando o _repository
            _repository.CriarBancoETabelas(logger);
            _repository.GravarResultadoNoBanco(resultado, logger);
        }

        /// <summary>
        /// Obtém e mostra os resultados do banco de dados para um termo específico.
        /// </summary>
        /// <param name="termo">Termo de busca.</param>
        /// <param name="logger">Instância do Logger para registro de logs.</param>
        public void MostrarResultados(string termo, Logger logger)
        {
            try
            {
                // Obter resultados do banco de dados
                var resultados = _repository.ObterResultadosPorTermo(termo, logger);

                if (resultados.Count == 0)
                {
                    Console.WriteLine($"Nenhum resultado encontrado para o termo '{termo}'.");
                }
                else
                {
                    Console.WriteLine($"Resultados encontrados para o termo '{termo}':");
                    foreach (var resultado in resultados)
                    {
                        Console.WriteLine($"Título: {resultado.Titulo}");
                        Console.WriteLine($"Professor(es): {resultado.Professor}");
                        Console.WriteLine($"Carga Horária: {resultado.CargaHoraria}");
                        Console.WriteLine($"Descrição: {resultado.Descricao}");
                        Console.WriteLine("------");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Erro ao obter resultados do banco de dados: {ex.Message}");
                Console.WriteLine($"Erro ao obter resultados do banco de dados: {ex.Message}");
            }
        }
    }
}
