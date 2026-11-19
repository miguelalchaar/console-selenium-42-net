using ConsoleSelenium.Tests.Support;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ConsoleSelenium.Tests.Pages;

/// <summary>
/// Page Object da página inicial do DuckDuckGo.
/// </summary>
public class DuckDuckGoHomePage
{
    public const string UrlBase = "https://duckduckgo.com";

    private static readonly By CampoDeBusca = By.Name("q");

    private static readonly By SugestoesDeAutocompletar =
        By.CssSelector(".search-suggestions-list li, .sbox-suggestions li, [role='option']");

    private readonly IWebDriver _driver;
    private readonly WebDriverWait _espera;

    public DuckDuckGoHomePage(IWebDriver driver)
    {
        _driver = driver;
        _espera = Esperas.Criar(driver);
    }

    public DuckDuckGoHomePage Abrir()
    {
        _driver.Navigate().GoToUrl(UrlBase);
        _espera.Until(d => d.FindElement(CampoDeBusca).Displayed);
        return this;
    }

    /// <summary>
    /// Digita o termo no campo de pesquisa sem enviar o formulário.
    /// </summary>
    public DuckDuckGoHomePage DigitarTermo(string termo)
    {
        var campo = _driver.FindElement(CampoDeBusca);
        campo.Clear();
        campo.SendKeys(termo);
        return this;
    }

    /// <summary>
    /// Digita o termo e envia a pesquisa pressionando Enter.
    /// </summary>
    public DuckDuckGoResultadosPage Pesquisar(string termo)
    {
        DigitarTermo(termo);
        _driver.FindElement(CampoDeBusca).SendKeys(Keys.Enter);

        return new DuckDuckGoResultadosPage(_driver).AguardarCarregamento();
    }

    /// <summary>
    /// Aguarda (espera explícita) a lista dinâmica de sugestões e retorna a opção da posição informada (começa em 1).
    /// </summary>
    public IWebElement AguardarSugestao(int posicao)
    {
        return _espera.Until(d =>
        {
            var visiveis = d.FindElements(SugestoesDeAutocompletar)
                .Where(sugestao => sugestao.Displayed)
                .ToList();

            return visiveis.Count >= posicao ? visiveis[posicao - 1] : null;
        })!;
    }
}
