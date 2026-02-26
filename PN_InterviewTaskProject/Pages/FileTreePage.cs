using System.Security.Cryptography;
using System.Text;

namespace PN_InterviewTaskProject.Pages;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.ObjectModel;

public class FileTreePage(IWebDriver driver) : BasePage(driver)
{
    private static readonly By TreeRoot = By.CssSelector("mat-tree[role='tree']");
    private static readonly By LeafNodes = By.XPath("//mat-nested-tree-node[not(.//button)]");

    public FileTreePage Open(string url)
    {
        Driver.Navigate().GoToUrl(url);
        FindElement(TreeRoot);
        return this;
    }
    public FileTreePage ExpandFromAbsolutePath(string absolutePath)
    {
        ExpandNode("/");
        var nodes = absolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);
        ExpandPathRecursively(nodes, 0);
        return this;
    }

    public bool IsLeafVisible(string leafName)
    {
        return IsVisible(LeafByName(leafName));
    }

    public IReadOnlyList<string> GetVisibleLeafNames()
    {
        var leaves = Driver.FindElements(LeafNodes);
        return ExtractVisibleLeafNames(leaves, 0, new List<string>());
    }

    private void ExpandPathRecursively(string[] nodes, int index)
    {
        if (index >= nodes.Length - 1)
        {
            return;
        }
        ExpandNode(nodes[index]);
        ExpandPathRecursively(nodes, index + 1);
    }

    private void ExpandNode(string nodeName)
    {
        var toggle = FindElement(ToggleByNodeName(nodeName));
        if (IsCollapsed(toggle))
        {
            toggle.Click();
        }
    }

    private static By ToggleByNodeName(string nodeName)
    {
        return By.CssSelector($"button[aria-label='Toggle {nodeName}']");
    }

    private static By LeafByName(string leafName)
    {
        return By.XPath($"//mat-nested-tree-node[not(.//button) and normalize-space(.)='{leafName}']");
    }

    private static bool IsCollapsed(IWebElement toggleButton)
    {
        var iconText = toggleButton.FindElement(By.CssSelector("mat-icon")).Text.Trim();
        return iconText.Equals("chevron_right", StringComparison.OrdinalIgnoreCase);
    }

    private static IReadOnlyList<string> ExtractVisibleLeafNames(
        ReadOnlyCollection<IWebElement> leaves,
        int index,
        List<string> accumulator)
    {
        if (index >= leaves.Count)
        {
            return accumulator;
        }

        var leaf = leaves[index];
        if (leaf.Displayed)
        {
            var name = leaf.Text.Trim();
            if (!string.IsNullOrWhiteSpace(name))
            {
                accumulator.Add(name);
            }
        }

        return ExtractVisibleLeafNames(leaves, index + 1, accumulator);
    }

    public static string ComputeSha256Checksum(IEnumerable<string> values)
    {
        var normalized = string.Join("|", values.Select(x => x.Trim()).OrderBy(x => x));
        var bytes = Encoding.UTF8.GetBytes(normalized);
        var hash = SHA256.HashData(bytes);
        return Convert.ToHexString(hash).ToLowerInvariant();
    }
}
