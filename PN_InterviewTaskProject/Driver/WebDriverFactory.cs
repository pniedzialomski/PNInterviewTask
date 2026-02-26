namespace PN_InterviewTaskProject.Driver;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

public static class WebDriverFactory
{
    public static IWebDriver CreateChromeDriver()
    {
        var browser = Environment.GetEnvironmentVariable("BROWSER")?.Trim().ToLowerInvariant() ?? "chrome";

        if (browser != "chrome")
        {
            throw new NotSupportedException($"Browser '{browser}' is not supported. Set BROWSER=chrome.");
        }

        var options = new ChromeOptions();
        options.AddArgument("--start-maximized");
        options.AddArgument("--disable-gpu");
        options.AddArgument("--no-sandbox");

        return new ChromeDriver(options);
    }
}
