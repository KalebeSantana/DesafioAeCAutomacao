using AluraRpa.Domain;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Collections.ObjectModel;

namespace AluraRpa.Infrastructure
{
    public class AluraWebScrapingService
    {
        /// <summary>
        /// Realiza web scraping no site da Alura para obter resultados relacionados ao termo de busca.
        /// </summary>
        /// <param name="termoBusca">O termo a ser pesquisado.</param>
        /// <returns>Uma lista de resultados de busca.</returns>
        public List<ResultadoBusca> RealizarWebScraping(string termoBusca)
        {
            var resultados = new List<ResultadoBusca>();

            try
            {
                var service = EdgeDriverService.CreateDefaultService();
                service.HideCommandPromptWindow = true;

                using (IWebDriver driver = new EdgeDriver(service))
                {
                    // Navegar até a página da Alura
                    driver.Navigate().GoToUrl("https://www.alura.com.br/");

                    // Localizar o campo de pesquisa e o botão de pesquisa
                    var campoPesquisa = driver.FindElement(By.XPath("//input[@placeholder='O que você quer aprender?']"));
                    var botaoPesquisa = driver.FindElement(By.XPath("//input[@placeholder='O que você quer aprender?']//parent::form//button"));

                    // Inserir o termo de busca no campo
                    campoPesquisa.SendKeys(termoBusca);

                    // Clicar no botão de pesquisa
                    botaoPesquisa.Click();

                    // Aguardar alguns segundos para a página de resultados carregar 
                    Thread.Sleep(5000);

                    // Encontrar todos os elementos dentro da seção que contém "Resultado da sua Busca:"
                    var resultadosPesquisa = driver.FindElements(By.XPath("//h2[text()='Resultado da sua Busca:']//parent::section//ul//li"));

                    // Iniciar o índice
                    int index = 0;

                    // Criar uma instância da classe Actions
                    Actions actions = new Actions(driver);

                    // Iterar sobre os elementos encontrados
                    while (index < resultadosPesquisa.Count)
                    {
                        var item = resultadosPesquisa[index];
                        var titulo = item.FindElement(By.XPath(".//h4"));
                        string textoTitulo = titulo.Text;
                        string descricaoResumida = item.FindElement(By.XPath(".//p")).Text;

                        if ((titulo.Text.Contains("Curso") && titulo.Text.ToLower().Contains(termoBusca.ToLower())) || (titulo.Text.Contains("Curso") && descricaoResumida.ToLower().Contains(termoBusca.ToLower())))
                        {
                            // Pressionar a tecla Ctrl e clicar no link
                            actions.KeyDown(Keys.Control).Click(titulo).KeyUp(Keys.Control).Build().Perform();

                            // Alternar para a nova aba
                            SwitchTab(driver, 1);

                            // Pegar dados
                            string cargaHoraria = driver.FindElement(By.XPath("//div//p[text()='Para conclusão']//parent::div//p[1]")).Text;
                            var professores = driver.FindElements(By.XPath("//h3[@class='instructor-title--name']"));

                            string resultadoProfessores = ObterNomesDosProfessores(professores);

                            // Criar um objeto ResultadoBusca
                            var resultado = new ResultadoBusca
                            {
                                Titulo = textoTitulo,
                                Professor = resultadoProfessores,
                                CargaHoraria = cargaHoraria,
                                Descricao = descricaoResumida
                            };

                            resultados.Add(resultado);

                            // Fechar segunda aba
                            driver.Close();

                            // Alternar de volta para a aba principal
                            SwitchTab(driver, 0);

                            // Incrementar o índice
                            index++;
                        }
                        else if ((titulo.Text.Contains("Formação") && titulo.Text.ToLower().Contains(termoBusca.ToLower())) || (titulo.Text.Contains("Formação") && descricaoResumida.ToLower().Contains(termoBusca.ToLower())))
                        {
                            // Pressionar a tecla Ctrl e clicar no link
                            actions.KeyDown(Keys.Control).Click(titulo).KeyUp(Keys.Control).Build().Perform();

                            // Alternar para a nova aba
                            SwitchTab(driver, 1);

                            // Pegar dados
                            string cargaHoraria = driver.FindElement(By.XPath("//div//p[text()='Para conclusão']//parent::div//div")).Text;
                            string descricaoCompleta = driver.FindElement(By.XPath("//div[@class='formacao-descricao-texto']//p")).Text;

                            var professores = driver.FindElements(By.XPath("//*[@id='instrutores']//li[@class='formacao-instrutores-instrutor --hidden-mobile']//h3"));

                            string resultadoProfessores = ObterNomesDosProfessores(professores);

                            // Criar um objeto ResultadoBusca
                            var resultado = new ResultadoBusca
                            {
                                Titulo = textoTitulo,
                                Professor = resultadoProfessores,
                                CargaHoraria = cargaHoraria,
                                Descricao = descricaoCompleta
                            };

                            resultados.Add(resultado);

                            // Fechar segunda aba
                            driver.Close();

                            // Alternar de volta para a aba principal
                            SwitchTab(driver, 0);

                            // Incrementar o índice
                            index++;
                        }
                        else
                        {
                            // Se o item não corresponder ao termo de busca, passe para o próximo
                            index++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro durante o web scraping: {ex.Message}");
            }

            return resultados;
        }

        /// <summary>
        /// Obtém os nomes dos professores formatados a partir da lista de elementos de professores.
        /// </summary>
        /// <param name="professores">A lista de elementos de professores.</param>
        /// <returns>Uma string contendo os nomes dos professores formatados.</returns>
        private string ObterNomesDosProfessores(ReadOnlyCollection<IWebElement> professores)
        {
            string resultadoProfessores = "Nenhum professor disponível";

            if (professores.Count > 1)
            {
                // Mapeia os nomes dos professores para uma lista de strings
                var nomesDosProfessores = professores.Select(professor => professor.Text).ToList();

                // Usa string.Join para unir os nomes com vírgula
                resultadoProfessores = string.Join(", ", nomesDosProfessores);
            }
            else if (professores.Count == 1)
            {
                resultadoProfessores = professores[0].Text;
            }

            return resultadoProfessores;
        }

        /// <summary>
        /// Alterna para a aba especificada pelo índice.
        /// </summary>
        /// <param name="driver">O driver do Selenium.</param>
        /// <param name="indiceAba">O índice da aba.</param>
        private static void SwitchTab(IWebDriver driver, int indiceAba)
        {
            try
            {
                // Obter todas as janelas abertas
                var windowHandles = driver.WindowHandles;

                if (windowHandles.Count > indiceAba)
                {
                    var window = windowHandles[indiceAba];
                    driver.SwitchTo().Window(window);
                }
                else
                {
                    Console.WriteLine($"Índice da aba {indiceAba} fora do intervalo.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro durante a troca de aba: {ex.Message}");
            }
        }
    }
}
