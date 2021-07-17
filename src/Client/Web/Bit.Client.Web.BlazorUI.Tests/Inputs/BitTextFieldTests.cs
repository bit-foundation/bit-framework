using System.Linq;
using AngleSharp;
using Bunit;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bit.Client.Web.BlazorUI.Tests.Inputs
{
    [TestClass]
    public class BitTextFieldTests : BunitTestContext
    {
        [DataTestMethod,
            DataRow(Visual.Fluent, true, true, true),
            DataRow(Visual.Fluent, false, true, false),
            DataRow(Visual.Fluent, true, false, true),
            DataRow(Visual.Fluent, false, false, false),

            DataRow(Visual.Cupertino, true, true, true),
            DataRow(Visual.Cupertino, false, true, false),
            DataRow(Visual.Cupertino, true, false, true),
            DataRow(Visual.Cupertino, false, false, false),

            DataRow(Visual.Material, true, true, true),
            DataRow(Visual.Material, false, true, false),
            DataRow(Visual.Material, true, false, true),
            DataRow(Visual.Material, false, false, false)
        ]
        public void BitTextFieldShouldTakeCorrectTypeAndVisual(Visual visual, bool isEnabled, bool IsMultiline, bool isRequired)
        {
            var component = RenderComponent<BitTextFieldTest>(
                parameters =>
                {
                    parameters.Add(p => p.Visual, visual);
                    parameters.Add(p => p.IsEnabled, isEnabled);
                    parameters.Add(p => p.IsMultiLine, IsMultiline);
                    parameters.Add(p => p.IsRequired, isRequired);
                });

            var bitTextField = component.Find(".bit-txt");
            var bitTextFieldFieldGroup = component.Find(".bit-txt > div");

            var isEnabledClass = isEnabled ? "enabled" : "disabled";
            var visualClass = visual == Visual.Cupertino ? "cupertino" : visual == Visual.Material ? "material" : "fluent";

            Assert.IsTrue(bitTextField.ClassList.Contains($"bit-txt-{isEnabledClass}-{visualClass}"));

            Assert.AreEqual(IsMultiline ? "TEXTAREA" : "INPUT", bitTextFieldFieldGroup.FirstElementChild.TagName);

            Assert.AreEqual(bitTextField.ClassList.Contains($"bit-txt-required-{visualClass}"), isRequired);
        }

        [DataTestMethod,
           DataRow(true, "hello world"),
           DataRow(false, "hello world")
       ]
        public void BitTextFieldShouldTakeValue(bool IsMultiline, string value)
        {
            var component = RenderComponent<BitTextFieldTest>(
                parameters =>
                {
                    parameters.Add(p => p.Value, value);
                    parameters.Add(p => p.IsMultiLine, IsMultiline);
                });

            var bitTextField = IsMultiline ? component.Find(".bit-txt textarea") : component.Find(".bit-txt input");

            if (IsMultiline)
            {
                Assert.AreEqual(bitTextField.TextContent, value);
            }
            else
            {
                Assert.AreEqual(bitTextField.GetAttribute("value"), value);
            }
        }

        [DataTestMethod, DataRow("this is label")]
        public void BitTextFieldLabel(string label)
        {
            var com = RenderComponent<BitTextFieldTest>(parameters =>
            {
                parameters.Add(p => p.Label, label);
            });

            var bitTextFieldLabel = com.Find(".bit-txt label").TextContent;

            Assert.AreEqual(label, bitTextFieldLabel);
        }

        [DataTestMethod,
            DataRow(15, true, "this is placeholder", true),
            DataRow(15, true, "this is placeholder", false),
            DataRow(15, false, "this is placeholder", true),
            DataRow(15, false, "this is placeholder", false),
        ]
        public void BitTextFieldShouldTakeBaseParameters(int maxLength, bool IsMultiline, string placeholder, bool isReadOnly)
        {
            var component = RenderComponent<BitTextFieldTest>(
                parameters =>
                {
                    parameters.Add(p => p.MaxLength, maxLength);
                    parameters.Add(p => p.IsMultiLine, IsMultiline);
                    parameters.Add(p => p.Placeholder, placeholder);
                    parameters.Add(p => p.IsReadOnly, isReadOnly);
                });

            var bitTextField = IsMultiline ? component.Find(".bit-txt textarea") : component.Find(".bit-txt input");

            Assert.IsTrue(bitTextField.HasAttribute("maxlength"));
            Assert.AreEqual(bitTextField.GetAttribute("maxlength"), maxLength.ToString());

            Assert.IsTrue(bitTextField.HasAttribute("placeholder"));
            Assert.AreEqual(bitTextField.GetAttribute("placeholder"), placeholder);

            Assert.AreEqual(isReadOnly, bitTextField.HasAttribute("readonly"));
        }

        [DataTestMethod, DataRow("Emoji2")]
        public void BitTextFieldShouldAcceptIcon(string iconName)
        {
            var component = RenderComponent<BitTextFieldTest>(
                parameters =>
                {
                    parameters.Add(p => p.IconName, iconName);
                });

            var bitTextFieldIcon = component.Find(".bit-txt span i");

            Assert.IsTrue(bitTextFieldIcon.ClassList.Contains($"bit-icon--{iconName}"));
        }

        [DataTestMethod,
            DataRow(true),
            DataRow(false),
        ]
        public void BitTextFieldCanRevealPassword(bool isEnabled)
        {
            var component = RenderComponent<BitTextFieldTest>(
                parameters =>
                {
                    parameters.Add(p => p.IsEnabled, isEnabled);
                    parameters.Add(p => p.Type, TextFieldType.Password);
                    parameters.Add(p => p.CanRevealPassword, true);
                });

            var bitTextField = component.Find(".bit-txt");
            var bitTextFieldFieldGroup = component.Find(".bit-txt > div");
            var bitTextFieldRevealPassword = component.Find(".bit-txt > div > span");

            Assert.AreEqual("Password", bitTextFieldFieldGroup.FirstElementChild.GetAttribute("type"));
            Assert.IsTrue(bitTextFieldRevealPassword.FirstElementChild.ClassList.Contains($"bit-icon--RedEye"));

            bitTextFieldRevealPassword.Click();

            Assert.AreEqual("Text", bitTextFieldFieldGroup.FirstElementChild.GetAttribute("type"));
            Assert.IsTrue(bitTextFieldRevealPassword.FirstElementChild.ClassList.Contains($"bit-icon--Hide"));
        }

        [DataTestMethod,
            DataRow(true, false),
            DataRow(true, true),
            DataRow(false, false),
            DataRow(false, true),
        ]
        public void BitTextFieldMustRespondToTheClickEvent(bool isEnabled, bool IsMultiline)
        {
            var component = RenderComponent<BitTextFieldTest>(
                parameters =>
                {
                    parameters.Add(p => p.IsEnabled, isEnabled);
                    parameters.Add(p => p.IsMultiLine, IsMultiline);
                });

            var bitTextField = IsMultiline ? component.Find(".bit-txt textarea") : component.Find(".bit-txt input");

            bitTextField.Click();

            Assert.AreEqual(isEnabled ? 1 : 0, component.Instance.CurrentCount);
        }

        [DataTestMethod,
            DataRow(true, false),
            DataRow(true, true),
            DataRow(false, false),
            DataRow(false, true),
        ]
        public void BitTextFieldMustRespondToTheFocusEvent(bool isEnabled, bool IsMultiline)
        {
            var component = RenderComponent<BitTextFieldTest>(
                parameters =>
                {
                    parameters.Add(p => p.IsEnabled, isEnabled);
                    parameters.Add(p => p.IsMultiLine, IsMultiline);
                });

            var bitTextField = IsMultiline ? component.Find(".bit-txt textarea") : component.Find(".bit-txt input");

            bitTextField.Focus();
            Assert.AreEqual(isEnabled ? 1 : 0, component.Instance.FocusedValue);

            bitTextField.FocusIn();
            Assert.AreEqual(isEnabled ? 1 : 0, component.Instance.FocusedInValue);

            bitTextField.FocusOut();
            Assert.AreEqual(isEnabled ? 1 : 0, component.Instance.FocusedOutValue);
        }

        [DataTestMethod,
            DataRow(true, false, "u", "d"),
            DataRow(true, true, "u", "d"),
            DataRow(false, false, "u", "d"),
            DataRow(false, true, "u", "d"),
        ]
        public void BitTextFieldMustRespondToTheKeyEvent(bool isEnabled, bool IsMultiline, string keyUpValue, string keyDownValue)
        {
            var component = RenderComponent<BitTextFieldTest>(
                parameters =>
                {
                    parameters.Add(p => p.IsEnabled, isEnabled);
                    parameters.Add(p => p.IsMultiLine, IsMultiline);
                });

            var bitTextField = IsMultiline ? component.Find(".bit-txt textarea") : component.Find(".bit-txt input");

            bitTextField.KeyDown(keyDownValue);
            Assert.AreEqual(isEnabled ? keyDownValue : null, component.Instance.KeyDownedValue);

            bitTextField.KeyUp(keyUpValue);
            Assert.AreEqual(isEnabled ? keyUpValue : null, component.Instance.KeyUpedValue);
        }

        [DataTestMethod,
           DataRow(true, false),
           DataRow(true, true),
           DataRow(false, false),
           DataRow(false, true),
       ]
        public void BitTextFieldMustRespondToTheChangeEvent(bool isEnabled, bool IsMultiline)
        {
            var component = RenderComponent<BitTextFieldTest>(
                parameters =>
                {
                    parameters.Add(p => p.IsEnabled, isEnabled);
                    parameters.Add(p => p.IsMultiLine, IsMultiline);
                });

            var bitTextField = IsMultiline ? component.Find(".bit-txt textarea") : component.Find(".bit-txt input");

            bitTextField.KeyDown("a");

            //TODO: bypassed - BUnit oninput event issue
            //Assert.AreEqual(isEnabled ? 1 : 0, component.Instance.CurrentCount);
        }
    }
}
