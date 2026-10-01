using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Windows.Forms;
using mRemoteNG.Tools;

namespace mRemoteNG.UI.Controls
{
    [SupportedOSPlatform("windows")]
    internal static class ExternalToolsMenuBuilder
    {
        internal static ToolStripItem[] CreateMenuItems(IEnumerable<ExternalTool> externalTools, EventHandler clickHandler)
        {
            List<ExternalTool> tools = externalTools.ToList();
            List<ToolStripItem> menuItems = [];

            foreach (ExternalTool tool in tools.Where(tool => string.IsNullOrWhiteSpace(tool.Category)))
                menuItems.Add(CreateToolMenuItem(tool, clickHandler));

            foreach (IGrouping<string, ExternalTool> category in tools
                         .Where(tool => !string.IsNullOrWhiteSpace(tool.Category))
                         .GroupBy(tool => tool.Category.Trim(), StringComparer.OrdinalIgnoreCase)
                         .OrderBy(group => group.Key, StringComparer.OrdinalIgnoreCase))
            {
                ToolStripMenuItem categoryMenuItem = new() { Text = category.Key };
                foreach (ExternalTool tool in category)
                    categoryMenuItem.DropDownItems.Add(CreateToolMenuItem(tool, clickHandler));

                menuItems.Add(categoryMenuItem);
            }

            return menuItems.ToArray();
        }

        private static ToolStripMenuItem CreateToolMenuItem(ExternalTool tool, EventHandler clickHandler)
        {
            ToolStripMenuItem menuItem = new()
            {
                Text = tool.DisplayName,
                Tag = tool,
                Image = tool.Image
            };
            menuItem.Click += clickHandler;
            return menuItem;
        }
    }
}
