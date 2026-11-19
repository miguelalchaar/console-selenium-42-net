using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ConsoleSelenium.Tests.Support;

/// <summary>
/// Cria esperas explícitas (WebDriverWait) com tempo limite e tratamento de exceções padronizados.
/// </summary>
public static class Esperas
{
    private static readonly TimeSpan TempoLimitePadrao = TimeSpan.FromSeconds(20);
    private static readonly TimeSpan IntervaloDeVerificacao = TimeSpan.FromMilliseconds(250);

    public static WebDriverWait Criar(IWebDriver driver)
    {
        var espera = new WebDriverWait(driver, TempoLimitePadrao)
        {
            PollingInterval = IntervaloDeVerificacao
        };

        espera.IgnoreExceptionTypes(
            typeof(NoSuchElementException),
            typeof(StaleElementReferenceException));

        return espera;
    }
}
