using System.Collections.Generic;

namespace Bit.Client.Web.BlazorUI.Playground.Web.Pages.Components
{
    public partial class NavMenu
    {
        private readonly List<BitNavLinkItem> NavLinks = new()
        {
            new BitNavLinkItem
            {
                Name = "Basic Inputs",
                Key = "Inputs",
                IsExpanded = true,
                Links = new List<BitNavLinkItem>
                {
                     new BitNavLinkItem { Name= "Button", Key = "Button", Url = "/components/buttons"},
                     new BitNavLinkItem { Name= "Checkbox", Key = "Checkbox", Url = "/components/checkboxes" },
                     new BitNavLinkItem { Name= "ChoiceGroup", Key = "Choice", Url = "/components/choicegroup" },
                     new BitNavLinkItem { Name= "Label", Key = "Label",  Url = "/components/labels" },
                     new BitNavLinkItem { Name= "Link", Key = "Link", Url = "/components/links" },
                     new BitNavLinkItem { Name= "Rating", Key = "Rating", Url = "/components/rating" },
                     new BitNavLinkItem { Name= "SearchBox", Key = "SearchBox", Url = "/components/searchbox" },
                     new BitNavLinkItem { Name= "TextField", Key = "TextField", Url = "/components/textfield" },
                     new BitNavLinkItem { Name= "Toggle", Key = "Toggle", Url = "#" }
                }
            },
            new BitNavLinkItem
            {
                Name = "Commands, Menus & Navs",
                Key = "Navigations",
                Links = new List<BitNavLinkItem>
                {
                    new BitNavLinkItem { Name = "Nav", Key = "Nav", Url = "#" },
                    new BitNavLinkItem { Name = "Pivot", Key = "Pivot", Url = "#" },
                }
            },
            new BitNavLinkItem
            {
                Name = "Notification & Engagement",
                Key = "Notifications",
                Links = new List<BitNavLinkItem>
                {
                    new BitNavLinkItem { Name = "MessageBar", Key = "MessageBar", Url="#" }
                }
            },
            new BitNavLinkItem
            {
                Name = "Progress",
                Key = "Progress",
                Links = new List<BitNavLinkItem>
                {
                    new BitNavLinkItem{ Name = "Spinner", Key = "Spinner", Url = "/components/spinner" }
                }
            },
        };
    }
}
