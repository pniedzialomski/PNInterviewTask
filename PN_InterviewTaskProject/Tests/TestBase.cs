namespace PN_InterviewTaskProject.Tests;

using System.IO;
using OpenQA.Selenium;
using PN_InterviewTaskProject.Driver;

public abstract class TestBase
{
    protected IWebDriver? Driver;

    [SetUp]
    public void SetUp()
    {
        Driver = WebDriverFactory.CreateChromeDriver();
    }

    [TearDown]
    public void TearDown()
    {
        if (Driver is null)
        {
            return;
        }
        
        Driver.Quit();
        Driver.Dispose();
    }

    protected string? SaveScreenshotAtTestEnd()
    {
        if (Driver is not ITakesScreenshot screenshotDriver)
        {
            return null;
        }
        try
        {
            var outputDir = Path.Combine(TestContext.CurrentContext.WorkDirectory, "TestArtifacts");
            Directory.CreateDirectory(outputDir);

            var testName = TestContext.CurrentContext.Test.Name;
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var outputPath = Path.Combine(outputDir, $"{testName}_{timestamp}.png");

            screenshotDriver.GetScreenshot().SaveAsFile(outputPath);
            TestContext.WriteLine($"Screenshot saved: {outputPath}");

            return outputPath;
        }
        catch (Exception ex)
        {
            TestContext.WriteLine($"Screenshot not saved: {ex.Message}");
        }
        return null;
    }
}
