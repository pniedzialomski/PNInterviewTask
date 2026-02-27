namespace PN_InterviewTaskProject.Tests;

using System.IO;
using Allure.Net.Commons;
using OpenQA.Selenium;
using PN_InterviewTaskProject.Driver;
using NUnit.Framework.Interfaces;
using System.Linq;

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

        try
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                var screenshotPath = SaveScreenshotAtTestEnd();
                if (!string.IsNullOrWhiteSpace(screenshotPath) && File.Exists(screenshotPath))
                {
                    AllureApi.AddAttachment("Failure screenshot", "image/png", screenshotPath);
                }
            }
        }
        finally
        {
            Driver.Quit();
            Driver.Dispose();
        }
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

            var testName = TestContext.CurrentContext.Test.Name.Split('(')[0].Trim();
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
