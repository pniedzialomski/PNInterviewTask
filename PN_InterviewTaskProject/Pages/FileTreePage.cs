namespace PN_InterviewTaskProject.Pages;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

public class FileTreePage(IWebDriver driver) : BasePage(driver)
{
    private static readonly By TreeRoot = By.CssSelector("mat-tree[role='tree']");

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
}
