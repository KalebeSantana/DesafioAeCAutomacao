# AluraRpa

Este projeto é uma aplicação de console para realizar buscas no site da Alura, extrair informações e salvar os resultados em um banco de dados SQLite.

## Ferramentas Utilizadas

- **Visual Studio:** Utilizado como ambiente de desenvolvimento integrado (IDE) devido à sua rica funcionalidade, depuração avançada e suporte à linguagem C#.
- **Git e GitHub:** Gerenciamento de controle de versão para colaboração e rastreamento de alterações no código-fonte.
- **SQLite:** Banco de dados embutido escolhido devido à sua simplicidade, leveza e adequação para projetos menores.

## Bibliotecas Principais
1. **Selenium.WebDriver**: Utilizado para interagir com o navegador Edge e realizar o web scraping no site da Alura.
2. **System.Data.SQLite**: Utilizado para interagir com o banco de dados SQLite, armazenando os resultados da busca.


## Decisões Técnicas

### Selenium.WebDriver

O Selenium.WebDriver foi escolhido para interagir com o navegador Edge devido à sua flexibilidade e ampla adoção na automação de testes e scraping.

### System.Data.SQLite

O System.Data.SQLite é uma biblioteca leve e eficiente para interação com bancos de dados SQLite. A escolha foi motivada pela simplicidade e pelo suporte nativo ao SQLite.


## Estrutura do Projeto

- **AluraRpa.Application:** Contém a lógica da aplicação, incluindo a interação com o usuário e chamadas aos serviços.
- **AluraRpa.Domain:** Define as entidades do domínio, como o resultado da busca.
- **AluraRpa.Infrastructure:** Contém a implementação da automação de web scraping, interações com o banco de dados e manipulação de dados.
- **AluraRpa.Shared:** Compartilha recursos, como o logger, entre diferentes partes do projeto.

## Como Usar

1. Clone este repositório.
2. Abra a solução no Visual Studio.
3. Compile e execute o projeto `AluraRpa.ConsoleApp`.