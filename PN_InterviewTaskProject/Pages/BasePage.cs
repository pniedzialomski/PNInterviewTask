namespace PN_InterviewTaskProject.Pages;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using PN_InterviewTaskProject.Framework;
using SeleniumExtras.WaitHelpers;

public abstract class BasePage
{
    protected readonly IWebDriver Driver;
    protected readonly WebDriverWait Wait;

    protected BasePage(IWebDriver driver)
    {
        Driver = driver;
        Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
    }

    protected IWebElement FindElement(By locator)
    {
        return Wait.Until(ExpectedConditions.ElementIsVisible(locator));
    }
    protected bool IsVisible(By locator)
    {
        try
        {
            return Wait.Until(ExpectedConditions.ElementIsVisible(locator)).Displayed;
        }
        catch (WebDriverException)
        {
            return false;
        }
    }
}
