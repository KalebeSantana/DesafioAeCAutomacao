using AluraRpa.Domain;
using AluraRpa.Shared;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace AluraRpa.Infrastructure
{
    public class BancoDeDadosRepository
    {
        private const string NomeBanco = "AluraRpa.sqlite3";
        private string NomeTabela { get; }

        public BancoDeDadosRepository(string nomeTabela)
        {
            NomeTabela = nomeTabela.ToUpper();
        }

        public void CriarBancoETabelas(Logger logger)
        {
            try
            {
                using (var connection = new SQLiteConnection($"Data Source={NomeBanco}"))
                {
                    if (!File.Exists($"./{NomeBanco}"))
                    {
                        SQLiteConnection.CreateFile(NomeBanco);
                    }

                    connection.Open();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = $@"
                            CREATE TABLE IF NOT EXISTS {NomeTabela} (
                                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                Titulo TEXT,
                                Professor TEXT,
                                CargaHoraria TEXT,
                                Descricao TEXT
                            );
                        ";

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Erro durante a criação do banco de dados ou tabela: {ex.Message}");
                Console.WriteLine($"Erro durante a criação do banco de dados ou tabela: {ex.Message}");
            }
        }

        public void GravarResultadoNoBanco(ResultadoBusca resultado, Logger logger)
        {
            try
            {
                logger.LogInfo($"Verificando se registro com o título '{resultado.Titulo}' já existe na tabela");

                // Verifica se o registro já existe na tabela
                if (!RegistroExiste(resultado, logger))
                {
                    using (var connection = new SQLiteConnection($"Data Source={NomeBanco}"))
                    {
                        connection.Open();

                        using (var command = connection.CreateCommand())
                        {
                            command.CommandText = $@"
                                INSERT INTO {NomeTabela} (Titulo, Professor, CargaHoraria, Descricao)
                                VALUES (@Titulo, @Professor, @CargaHoraria, @Descricao);
                            ";

                            command.Parameters.AddWithValue("@Titulo", resultado.Titulo);
                            command.Parameters.AddWithValue("@Professor", resultado.Professor);
                            command.Parameters.AddWithValue("@CargaHoraria", resultado.CargaHoraria);
                            command.Parameters.AddWithValue("@Descricao", resultado.Descricao);

                            command.ExecuteNonQuery();
                        }
                    }
                }
                else
                {
                    logger.LogWarning("O registro já existe na tabela. Não será inserido novamente.");
                    Console.WriteLine("O registro já existe na tabela. Não será inserido novamente.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Erro durante a gravação no banco de dados: {ex.Message}");
                Console.WriteLine($"Erro durante a gravação no banco de dados: {ex.Message}");
            }
        }

        private bool RegistroExiste(ResultadoBusca resultado, Logger logger)
        {
            using (var connection = new SQLiteConnection($"Data Source={NomeBanco}"))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $@"
                        SELECT COUNT(*)
                        FROM {NomeTabela}
                        WHERE Titulo = @Titulo
                        AND Professor = @Professor
                        AND CargaHoraria = @CargaHoraria
                        AND Descricao = @Descricao;
                    ";

                    command.Parameters.AddWithValue("@Titulo", resultado.Titulo);
                    command.Parameters.AddWithValue("@Professor", resultado.Professor);
                    command.Parameters.AddWithValue("@CargaHoraria", resultado.CargaHoraria);
                    command.Parameters.AddWithValue("@Descricao", resultado.Descricao);

                    int count = Convert.ToInt32(command.ExecuteScalar());

                    return count > 0;
                }
            }
        }

        public List<ResultadoBusca> ObterResultadosPorTermo(string termoBusca, Logger logger)
        {
            try
            {
                logger.LogInfo($"Criando conexão com o banco {NomeBanco}");

                using (var connection = new SQLiteConnection($"Data Source={NomeBanco}"))
                {
                    connection.Open();

                    logger.LogInfo($"Obtendo dados relacionados ao temo {termoBusca}");

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = $@"
                            SELECT Titulo, Professor, CargaHoraria, Descricao
                            FROM {NomeTabela}
                            WHERE Titulo LIKE @TermoBusca OR Professor LIKE @TermoBusca OR CargaHoraria LIKE @TermoBusca OR Descricao LIKE @TermoBusca;
                        ";

                        command.Parameters.AddWithValue("@TermoBusca", $"%{termoBusca}%");

                        using (var reader = command.ExecuteReader())
                        {
                            var resultados = new List<ResultadoBusca>();

                            while (reader.Read())
                            {
                                var resultado = new ResultadoBusca
                                {
                                    Titulo = reader["Titulo"].ToString(),
                                    Professor = reader["Professor"].ToString(),
                                    CargaHoraria = reader["CargaHoraria"].ToString(),
                                    Descricao = reader["Descricao"].ToString()
                                };

                                resultados.Add(resultado);
                            }

                            return resultados;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Erro durante a consulta no banco de dados: {ex.Message}");
                Console.WriteLine($"Erro durante a consulta no banco de dados: {ex.Message}");
                return new List<ResultadoBusca>();
            }
        }

    }
}
