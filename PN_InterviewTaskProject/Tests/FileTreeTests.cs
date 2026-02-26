using PN_InterviewTaskProject.Framework;
using PN_InterviewTaskProject.Pages;

namespace PN_InterviewTaskProject.Tests;

[TestFixture]
public class FileTreeTests : TestBase
{
    [TestCase("/home/user/projects/README.md")]
    public void ExpandGivenPathAndTakeScreenshot(string path)
    {
        var leafName = path.Split('/').Last();
        var page = new FileTreePage(Driver!).Open(TestSettings.BaseUrl).ExpandFromAbsolutePath(path);

        Assert.That(page.IsLeafVisible(leafName), Is.True, $"{leafName} should be visible after expanding given path");
        
        var screenshotDirectory = SaveScreenshotAtTestEnd();
        Assert.That(File.Exists(screenshotDirectory), $"{screenshotDirectory} does not exist");

        var fileInfo = new FileInfo(screenshotDirectory); 
        Assert.That(fileInfo.Length, Is.GreaterThan(0), $"{screenshotDirectory} is empty");
    }

    [TestCase("/home/user/projects/README.md")]
    [TestCase("/home/user/documents/taxes.pdf")]
    [TestCase("/usr/local/bin/docker")]
    [TestCase("/var/www/html/index.html")]
    public void CalculateCheckSumForVisibleFiles(string path)
    {
        var page = new FileTreePage(Driver!)
            .Open(TestSettings.BaseUrl)
            .ExpandFromAbsolutePath(path);

        var visibleLeafNames = page.GetVisibleLeafNames();
        Assert.That(visibleLeafNames, Is.Not.Empty, "No visible file names were read from the UI.");

        var checksum = FileTreePage.ComputeSha256Checksum(visibleLeafNames);

        Assert.Multiple(() =>
        {
            Assert.That(checksum, Has.Length.EqualTo(64), "Checksum should be a SHA-256 hex string.");
            Assert.That(checksum, Does.Match("^[a-f0-9]{64}$"), "Checksum format is invalid.");
        });
    }
}
