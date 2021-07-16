using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bunit;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bit.Client.Web.BlazorUI.Tests.Inputs
{
    [TestClass]
    public class BitDropDownTests : BunitTestContext
    {
        [DataTestMethod,
           DataRow(Visual.Fluent, true, 1, true, "enabled"),
           DataRow(Visual.Fluent, false, 0, false, "disabled"),

           DataRow(Visual.Cupertino, true, 1, true, "enabled"),
           DataRow(Visual.Cupertino, false, 0, false, "disabled"),

           DataRow(Visual.Material, true, 1, true, "enabled"),
           DataRow(Visual.Material, false, 0, false, "disabled")
        ]
        public async Task BitDropDownShouldRespectIsEnabled(Visual visual, bool isEnabled, int count, bool isCalloutOpen, string className)
        {
            var items = new List<DropDownItem>
            {
                new() {ItemType = DropDownItemType.Header, IsEnabled = isEnabled},
                new() {ItemType = DropDownItemType.Normal, IsEnabled = isEnabled},
                new() {ItemType = DropDownItemType.Normal, IsEnabled = isEnabled},
                new() {ItemType = DropDownItemType.Divider, IsEnabled = isEnabled}
            };
            var com = RenderComponent<BitDropDownTest>(
                parameters =>
                {
                    parameters.Add(p => p.Visual, visual);
                    parameters.Add(p => p.Items, items);
                    parameters.Add(p => p.IsEnabled, isEnabled);
                    parameters.Add(p => p.Value, count.ToString());
                });
            var bitChoiceGroup = com.Find(".bit-drp");
            bitChoiceGroup.Children.First().Click();
            var visualClass = visual == Visual.Cupertino ? "cupertino" : visual == Visual.Material ? "material" : "fluent";
            Assert.AreEqual(count, com.Instance.CurrentCount);
            Assert.IsTrue(bitChoiceGroup.ClassList.Contains($"bit-drp-{className}-{visualClass}"));
            Assert.IsTrue(!isCalloutOpen || bitChoiceGroup.Children[2].Attributes.Any(p => p.Name.Equals("style")));
        }

        [DataTestMethod,
           DataRow(Visual.Fluent, true, 2),
           DataRow(Visual.Fluent, false, 1),

           DataRow(Visual.Cupertino, true, 2),
           DataRow(Visual.Cupertino, false, 1),

           DataRow(Visual.Material, true, 2),
           DataRow(Visual.Material, false, 1)
        ]
        public async Task BitDropDownItemsShouldRespectIsEnabled(Visual visual, bool isEnabled, int count)
        {
            var items = new List<DropDownItem>
            {
                new() {ItemType = DropDownItemType.Header, IsEnabled = isEnabled},
                new() {ItemType = DropDownItemType.Normal, IsEnabled = isEnabled, Text = "Test1", Value = "Test value1"},
                new() {ItemType = DropDownItemType.Normal, IsEnabled = isEnabled, Text = "Test2", Value = "Test value2"},
                new() {ItemType = DropDownItemType.Divider, IsEnabled = isEnabled}
            };
            var com = RenderComponent<BitDropDownTest>(
                parameters =>
                {
                    parameters.Add(p => p.Visual, visual);
                    parameters.Add(p => p.Items, items);
                    parameters.Add(p => p.ItemIsEnabled, isEnabled);
                    parameters.Add(p => p.Value, count.ToString());
                });
            var bitChoiceGroup = com.Find(".bit-drp");
            bitChoiceGroup.Children.First().Click();
            bitChoiceGroup.Children[2].Children[1].Click();
            Assert.AreEqual(count, com.Instance.CurrentCount);
        }

        [DataTestMethod,
           DataRow(Visual.Fluent, true, 3),
           DataRow(Visual.Fluent, false, 1),

           DataRow(Visual.Cupertino, true, 3),
           DataRow(Visual.Cupertino, false, 1),

           DataRow(Visual.Material, true, 3),
           DataRow(Visual.Material, false, 1)
        ]
        public async Task BitDropDownMultiSelectShouldRespectIsEnabled(Visual visual, bool isEnabled, int count)
        {
            Context.JSInterop.Mode = JSRuntimeMode.Loose;
            var items = new List<DropDownItem>
            {
                new() {ItemType = DropDownItemType.Header, IsEnabled = isEnabled},
                new() {ItemType = DropDownItemType.Normal, IsEnabled = isEnabled},
                new() {ItemType = DropDownItemType.Normal, IsEnabled = isEnabled},
                new() {ItemType = DropDownItemType.Divider, IsEnabled = isEnabled}
            };
            var com = RenderComponent<BitDropDownTest>(
                parameters =>
                {
                    parameters.Add(p => p.Visual, visual);
                    parameters.Add(p => p.Items, items);
                    parameters.Add(p => p.IsMultiSelect, true);
                    parameters.Add(p => p.ItemIsEnabled, isEnabled);
                    parameters.Add(p => p.Value, count.ToString());
                });
            var bitChoiceGroup = com.Find(".bit-drp");
            bitChoiceGroup.Children.First().Click();
            var visualClass = visual == Visual.Cupertino ? "cupertino" : visual == Visual.Material ? "material" : "fluent";
            var checkboxItems = com.FindAll($".bit-chb-{visualClass} > div > div");
            for (int index = 0; index < checkboxItems.Count; index++)
            {
                checkboxItems[index].Click();
            }
            Assert.AreEqual(count, com.Instance.CurrentCount);
        }
    }
}
