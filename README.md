# ConsoleSelenium42

![C#](https://img.shields.io/badge/C%23-.NET%2010-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Selenium](https://img.shields.io/badge/Selenium-WebDriver-43B02A?style=for-the-badge&logo=selenium&logoColor=white)
![MSTest](https://img.shields.io/badge/MSTest-Testes%20Automatizados-5E2D91?style=for-the-badge&logo=dotnet&logoColor=white)
![Git](https://img.shields.io/badge/Git-Versionamento-orange?style=for-the-badge&logo=git&logoColor=white)
![Status](https://img.shields.io/badge/Status-Conclu%C3%ADda-green?style=for-the-badge)

Projeto desenvolvido para a disciplina de **Garantia da Qualidade de Software**, com foco na **automação de testes de interface com Selenium WebDriver** e **MSTest**, utilizando uma solution **.NET 10** organizada e modularizada.

A atividade consiste em automatizar três cenários reais de uso do buscador **DuckDuckGo** (https://duckduckgo.com): pesquisa básica, autocompletar e navegação por histórico.

## ▸ Conceito da atividade

A solution `ConsoleSelenium42` é composta por dois projetos:

- `ConsoleSelenium`: aplicação de console (projeto principal da solution).
- `ConsoleSelenium.Tests`: projeto de testes MSTest com os cenários automatizados, os Page Objects e os utilitários do Selenium.

## ▸ Estrutura do projeto

```text
console-selenium-42-net/
├── ConsoleSelenium42.slnx
├── ConsoleSelenium/
│   └── Program.cs
└── ConsoleSelenium.Tests/
    ├── BuscaDesafioTest.cs
    ├── Pages/
    │   ├── DuckDuckGoHomePage.cs
    │   └── DuckDuckGoResultadosPage.cs
    └── Support/
        ├── DriverFactory.cs
        └── Esperas.cs
```

| Componente                  | Responsabilidade                                                                            |
| --------------------------- | ------------------------------------------------------------------------------------------- |
| `BuscaDesafioTest`          | Contém os três cenários `[TestMethod]`, com abertura e fechamento do navegador por teste.   |
| `DuckDuckGoHomePage`        | Page Object da página inicial: abrir, digitar, pesquisar e aguardar sugestões.              |
| `DuckDuckGoResultadosPage`  | Page Object dos resultados: ler a consulta, abrir o primeiro resultado e aguardar a página. |
| `DriverFactory`             | Cria o `ChromeDriver` (modo headless opcional pela variável `SELENIUM_HEADLESS=true`).      |
| `Esperas`                   | Cria o `WebDriverWait` padrão (20 s) ignorando exceções transitórias.                       |

## ▸ Cenários automatizados

| Teste                                                                    | O que valida                                                                                                                                   |
| ------------------------------------------------------------------------ | ---------------------------------------------------------------------------------------------------------------------------------------------- |
| `Exercicio1_PesquisaBasica_ValidaTituloOuUrlDaPaginaDeResultados`        | Digita "Selenium WebDriver C#" no campo `name="q"`, envia com Enter e valida que a URL ou o título contém o termo pesquisado.                  |
| `Exercicio2_Autocompletar_SugestaoSelecionadaRedirecionaParaResultados`  | Digita "C# unit testing", aguarda a lista de sugestões com `WebDriverWait`, clica na terceira opção e valida que a busca corresponde a ela.    |
| `Exercicio3_NavegacaoHistorico_VoltarRestauraAPaginaDeResultados`        | Pesquisa "Automated Testing", abre o primeiro resultado orgânico, valida a página externa, usa `Navigation.Back()` e valida os resultados.     |

## ▸ Boas práticas aplicadas

- **Page Object Model:** separa a interação com a página da lógica dos testes.
- **Esperas explícitas:** `WebDriverWait` no lugar de `Thread.Sleep`.
- **Isolamento:** cada teste abre e fecha o seu próprio navegador (`[TestInitialize]` e `[TestCleanup]`).
- **Commits semânticos:** histórico organizado por tipo (`feat`, `chore`, `test`, `docs`).

## ▸ Como executar

**Pré-requisitos:** [SDK do .NET 10](https://dotnet.microsoft.com/download), Google Chrome instalado e acesso à internet (o Selenium Manager baixa o chromedriver compatível automaticamente).

```bash
git clone https://github.com/miguelalchaar/console-selenium-42-net
cd console-selenium-42-net
dotnet restore
dotnet test
```

Para executar sem abrir a janela do navegador:

```bash
# Git Bash / Linux / macOS
SELENIUM_HEADLESS=true dotnet test

# PowerShell
$env:SELENIUM_HEADLESS="true"; dotnet test
```

**Observação:** os testes dependem do site real do DuckDuckGo. Se o site apresentar uma verificação anti-robô ou mudar sua estrutura, algum cenário pode falhar; nesse caso, execute novamente sem o modo headless.

## ▸ Padrão de commits

```bash
git commit -m "feat(setup): cria estrutura inicial da solution console-selenium-42"
git commit -m "chore(deps): adiciona pacotes Selenium.WebDriver e Selenium.Support"
git commit -m "feat(support): adiciona fabrica do ChromeDriver e esperas explicitas"
git commit -m "feat(pages): adiciona page objects da home e dos resultados do DuckDuckGo"
git commit -m "test(google): implementa cenario de pesquisa basica e validacao de titulo"
git commit -m "test(google): implementa cenario de autocompletar"
git commit -m "test(google): implementa cenario de navegacao por historico"
git commit -m "docs: adiciona README com instrucoes de execucao"
```

## ▸ Autor

**Miguel Alchaar**

Desenvolvido como atividade acadêmica da disciplina de **Garantia da Qualidade de Software**, com foco em automação de testes e versionamento de código.
