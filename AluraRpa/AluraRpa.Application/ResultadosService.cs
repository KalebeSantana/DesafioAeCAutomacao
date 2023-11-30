using AluraRpa.Domain;
using AluraRpa.Infrastructure;
using AluraRpa.Shared;
using System;

namespace AluraRpa.Application
{
    public class ResultadosService
    {
        private readonly BancoDeDadosRepository _repository;

        public ResultadosService(BancoDeDadosRepository repository)
        {
            _repository = repository;
        }

        public void SalvarResultado(ResultadoBusca resultado, Logger logger)
        {
            // Lógica para salvar o resultado no banco de dados usando o _repository
            _repository.CriarBancoETabelas(logger);
            _repository.GravarResultadoNoBanco(resultado, logger);
        }

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
