using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace ConsoleSelenium.Tests.Support;

/// <summary>
/// Responsável por criar o navegador usado nos testes.
/// O Selenium Manager (embutido no Selenium 4) baixa o chromedriver compatível automaticamente.
/// </summary>
public static class DriverFactory
{
    private const string VariavelModoHeadless = "SELENIUM_HEADLESS";

    public static IWebDriver CriarChrome()
    {
        var opcoes = new ChromeOptions();
        opcoes.AddArgument("--start-maximized");
        opcoes.AddArgument("--disable-notifications");

        if (ModoHeadlessHabilitado())
        {
            opcoes.AddArgument("--headless=new");
            opcoes.AddArgument("--window-size=1920,1080");
        }

        return new ChromeDriver(opcoes);
    }

    private static bool ModoHeadlessHabilitado() =>
        string.Equals(
            Environment.GetEnvironmentVariable(VariavelModoHeadless),
            "true",
            StringComparison.OrdinalIgnoreCase);
}
