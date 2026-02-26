using OpenQA.Selenium;
using System.IO;
using PN_InterviewTaskProject.Framework;
using PN_InterviewTaskProject.Pages;

namespace PN_InterviewTaskProject.Tests;

[TestFixture]
public class FileTreeTests : TestBase
{
    [TestCase]
    public void ExpandGivenPathAndTakeScreenshot(string path = "/home/user/projects/README.md")
    {
        var leafName = path.Split('/').Last();
        var page = new FileTreePage(Driver!).Open(TestSettings.BaseUrl).ExpandFromAbsolutePath(path);

        Assert.That(page.IsLeafVisible(leafName), Is.True, $"{leafName} should be visible after expanding given path");
        
        var screenshotDirectory = SaveScreenshotAtTestEnd();
        Assert.That(File.Exists(screenshotDirectory), $"{screenshotDirectory} does not exist");

        var fileInfo = new FileInfo(screenshotDirectory);
        Assert.That(fileInfo.Length, Is.GreaterThan(0), $"{screenshotDirectory} is empty");
    }
}
