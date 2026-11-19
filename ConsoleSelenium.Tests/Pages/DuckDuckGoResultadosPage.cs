using System.Web;
using ConsoleSelenium.Tests.Support;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ConsoleSelenium.Tests.Pages;

/// <summary>
/// Page Object da página de resultados de busca do DuckDuckGo.
/// </summary>
public class DuckDuckGoResultadosPage
{
    private const string DominioDuckDuckGo = "duckduckgo.com";

    private static readonly By LinksDosResultados =
        By.CssSelector("a[data-testid='result-title-a'], article h2 a");

    private readonly IWebDriver _driver;
    private readonly WebDriverWait _espera;

    public DuckDuckGoResultadosPage(IWebDriver driver)
    {
        _driver = driver;
        _espera = Esperas.Criar(driver);
    }

    public string Url => _driver.Url;

    public string Titulo => _driver.Title;

    /// <summary>
    /// Termo pesquisado, lido do parâmetro "q" da URL já decodificado (ex.: "C%23" vira "C#").
    /// </summary>
    public string Consulta
    {
        get
        {
            var queryString = new Uri(_driver.Url).Query;
            return HttpUtility.ParseQueryString(queryString)["q"] ?? string.Empty;
        }
    }

    public DuckDuckGoResultadosPage AguardarCarregamento()
    {
        _espera.Until(d => d.Url.Contains("q=", StringComparison.OrdinalIgnoreCase));
        return this;
    }

    public DuckDuckGoResultadosPage AguardarRetornoAosResultados()
    {
        _espera.Until(d =>
            d.Url.Contains(DominioDuckDuckGo, StringComparison.OrdinalIgnoreCase) &&
            d.Url.Contains("q=", StringComparison.OrdinalIgnoreCase));

        return this;
    }

    /// <summary>
    /// Clica no primeiro link orgânico (que não aponta para o próprio DuckDuckGo) e retorna o destino do link.
    /// </summary>
    public string AbrirPrimeiroResultadoOrganico()
    {
        var link = _espera.Until(d => d.FindElements(LinksDosResultados)
            .FirstOrDefault(resultado => resultado.Displayed && EhLinkExterno(resultado)))!;

        var destino = link.GetAttribute("href") ?? string.Empty;
        var javascript = (IJavaScriptExecutor)_driver;

        // Garante que o link abra na mesma aba, para que o botão "Voltar" funcione no teste.
        javascript.ExecuteScript("arguments[0].removeAttribute('target');", link);
        javascript.ExecuteScript("arguments[0].scrollIntoView({ block: 'center' });", link);

        link.Click();
        return destino;
    }

    /// <summary>
    /// Aguarda a navegação para fora do DuckDuckGo e retorna a URL da página externa.
    /// </summary>
    public string AguardarPaginaExterna()
    {
        _espera.Until(d => !d.Url.Contains(DominioDuckDuckGo, StringComparison.OrdinalIgnoreCase));
        return _driver.Url;
    }

    private static bool EhLinkExterno(IWebElement link)
    {
        var href = link.GetAttribute("href");

        return !string.IsNullOrEmpty(href) &&
               !href.Contains(DominioDuckDuckGo, StringComparison.OrdinalIgnoreCase);
    }
}
