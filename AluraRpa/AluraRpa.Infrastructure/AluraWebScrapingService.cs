using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Interactions;
using System;
using System.Threading;

namespace AluraRpa.Infrastructure
{
    public class AluraWebScrapingService
    {
        // Implementação do serviço de web scraping
        public void RealizarWebScraping(string termoBusca)
        {
            using (IWebDriver driver = new EdgeDriver())
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
                var resultados = driver.FindElements(By.XPath("//h2[text()='Resultado da sua Busca:']//parent::section//ul//li"));

                // Iniciar o índice
                int index = 0;

                // Iterar sobre os elementos encontrados
                while (index < resultados.Count)
                {
                    var item = resultados[index];
                    var titulo = item.FindElement(By.XPath(".//h4"));
                    string descricao = item.FindElement(By.XPath(".//p")).Text;

                    if (titulo.Text.Contains(termoBusca) || descricao.Contains(termoBusca))
                    {
                        // Criar uma instância da classe Actions
                        Actions actions = new Actions(driver);

                        // Pressionar a tecla Ctrl e clicar no link
                        actions.KeyDown(Keys.Control).Click(titulo).KeyUp(Keys.Control).Build().Perform();

                        // Alternar para a nova aba
                        SwitchTab(driver, 1);

                        // Pegar dados
                        string cargaHoraria = driver.FindElement(By.XPath("//div//p[text()='Para conclusão']//parent::div//p[1]")).Text;
                        var professores = driver.FindElements(By.XPath("//h3[@class='instructor-title--name']"));

                        foreach (var prof in professores)
                        {
                            Console.WriteLine(prof.Text + "\n");
                        }

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

        private static void SwitchTab(IWebDriver driver, int indiceAba)
        {
            // Obter todas as janelas abertas
            var window = driver.WindowHandles[indiceAba];
            driver.SwitchTo().Window(window);
        }
    }
}
