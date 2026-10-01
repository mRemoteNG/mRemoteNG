using System;
using System.Linq;
using System.Windows.Forms;
using mRemoteNG.Tools;
using mRemoteNG.UI.Controls;
using NUnit.Framework;

namespace mRemoteNGTests.UI.Controls
{
    [TestFixture]
    public class ExternalToolsMenuBuilderTests
    {
        [Test]
        public void CreateMenuItems_GroupsToolsByCategoryAndLeavesUncategorizedToolsAtRoot()
        {
            ExternalTool uncategorizedTool = new("Uncategorized");
            ExternalTool firstCategorizedTool = new("PowerShell script") { Category = " PowerShell " };
            ExternalTool secondCategorizedTool = new("PowerShell action") { Category = "powershell" };
            ExternalTool otherCategorizedTool = new("Windows action") { Category = "Windows" };

            ToolStripItem[] menuItems = ExternalToolsMenuBuilder.CreateMenuItems(
                [uncategorizedTool, firstCategorizedTool, secondCategorizedTool, otherCategorizedTool],
                (_, _) => { });

            try
            {
                Assert.That(menuItems, Has.Length.EqualTo(3));
                Assert.That(menuItems[0], Is.TypeOf<ToolStripMenuItem>());
                Assert.That(menuItems[0].Text, Is.EqualTo("Uncategorized"));

                ToolStripMenuItem powershellMenu = (ToolStripMenuItem)menuItems[1];
                Assert.That(powershellMenu.Text, Is.EqualTo("PowerShell"));
                Assert.That(powershellMenu.DropDownItems.Cast<ToolStripMenuItem>()
                    .Select(item => item.Tag), Is.EqualTo(new[] { firstCategorizedTool, secondCategorizedTool }));

                ToolStripMenuItem windowsMenu = (ToolStripMenuItem)menuItems[2];
                Assert.That(windowsMenu.Text, Is.EqualTo("Windows"));
                Assert.That(windowsMenu.DropDownItems[0].Tag, Is.SameAs(otherCategorizedTool));
            }
            finally
            {
                foreach (ToolStripItem menuItem in menuItems)
                    menuItem.Dispose();
            }
        }
    }
}
