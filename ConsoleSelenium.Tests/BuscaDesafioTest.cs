using System.Text.RegularExpressions;
using ConsoleSelenium.Tests.Pages;
using ConsoleSelenium.Tests.Support;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace ConsoleSelenium.Tests;

[TestClass]
[DoNotParallelize]
public class BuscaDesafioTest
{
    private IWebDriver _driver = null!;

    [TestInitialize]
    public void Preparar()
    {
        _driver = DriverFactory.CriarChrome();
    }

    [TestCleanup]
    public void Finalizar()
    {
        _driver?.Quit();
    }

    private static string Normalizar(string texto) =>
        Regex.Replace(texto.Trim(), @"\s+", " ");

    [TestMethod]
    public void Exercicio1_PesquisaBasica_ValidaTituloOuUrlDaPaginaDeResultados()
    {
        const string termo = "Selenium WebDriver C#";

        var resultados = new DuckDuckGoHomePage(_driver)
            .Abrir()
            .Pesquisar(termo);

        var termoNaUrl = resultados.Consulta.Contains(termo, StringComparison.OrdinalIgnoreCase);
        var termoNoTitulo = resultados.Titulo.Contains(termo, StringComparison.OrdinalIgnoreCase);

        Assert.IsTrue(
            termoNaUrl || termoNoTitulo,
            $"O termo '{termo}' não foi encontrado na URL ({resultados.Url}) nem no título ({resultados.Titulo}).");
    }

    [TestMethod]
    public void Exercicio2_Autocompletar_SugestaoSelecionadaRedirecionaParaResultados()
    {
        const string termo = "C# unit testing";
        const int posicaoDaSugestao = 3;

        var home = new DuckDuckGoHomePage(_driver)
            .Abrir()
            .DigitarTermo(termo);

        var sugestao = home.AguardarSugestao(posicaoDaSugestao);
        var textoDaSugestao = Normalizar(sugestao.Text);
        Assert.IsFalse(string.IsNullOrWhiteSpace(textoDaSugestao), "A sugestão escolhida está sem texto.");

        sugestao.Click();

        var resultados = new DuckDuckGoResultadosPage(_driver).AguardarCarregamento();

        Assert.IsTrue(
            Normalizar(resultados.Consulta).Contains(textoDaSugestao, StringComparison.OrdinalIgnoreCase),
            $"A pesquisa realizada ('{resultados.Consulta}') não corresponde à sugestão escolhida ('{textoDaSugestao}').");
    }

    [TestMethod]
    public void Exercicio3_NavegacaoHistorico_VoltarRestauraAPaginaDeResultados()
    {
        const string termo = "Automated Testing";

        var resultados = new DuckDuckGoHomePage(_driver)
            .Abrir()
            .Pesquisar(termo);

        resultados.AbrirPrimeiroResultadoOrganico();
        var urlExterna = resultados.AguardarPaginaExterna();

        Assert.IsTrue(
            Uri.TryCreate(urlExterna, UriKind.Absolute, out var uri) &&
            (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps),
            $"A página externa não foi carregada corretamente. URL atual: {_driver.Url}");
        Assert.IsFalse(
            _driver.Url.Contains("duckduckgo.com", StringComparison.OrdinalIgnoreCase),
            "O teste deveria ter saído da página de resultados do DuckDuckGo.");

        _driver.Navigation.Back();
        resultados.AguardarRetornoAosResultados();

        Assert.IsTrue(
            _driver.Url.Contains("duckduckgo.com", StringComparison.OrdinalIgnoreCase),
            $"A página de resultados não foi restaurada. URL atual: {_driver.Url}");
        Assert.IsTrue(
            resultados.Consulta.Contains(termo, StringComparison.OrdinalIgnoreCase),
            $"A busca restaurada ('{resultados.Consulta}') não corresponde ao termo '{termo}'.");
    }
}
