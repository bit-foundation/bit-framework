using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bunit;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bit.Client.Web.BlazorUI.Tests.Sliders
{
    [TestClass]
    public class BitSliderTests : BunitTestContext
    {
        [DataTestMethod,
            DataRow(Visual.Fluent, false, false, false, null, null),
            //DataRow(Visual.Fluent, true, false, false, null, null),
            DataRow(Visual.Fluent, false, true, false, null, null),
            //DataRow(Visual.Fluent, true, true, false, null, null),
            DataRow(Visual.Fluent, false, false, true, null, null),
            DataRow(Visual.Fluent, false, true, true, null, null),
            //DataRow(Visual.Fluent, true, true, true, null, null),
            DataRow(Visual.Fluent, false, false, true, 2, 6),
            DataRow(Visual.Fluent, false, true, true, 2, 6),
            //DataRow(Visual.Fluent, true, true, true, 2, 6),

            DataRow(Visual.Cupertino, false, false, false, null, null),
            //DataRow(Visual.Cupertino, true, false, false, null, null),
            DataRow(Visual.Cupertino, false, true, false, null, null),
            //DataRow(Visual.Cupertino, true, true, false, null, null),
            DataRow(Visual.Cupertino, false, false, true, null, null),
            DataRow(Visual.Cupertino, false, true, true, null, null),
            //DataRow(Visual.Cupertino, true, true, true, null, null),
            DataRow(Visual.Cupertino, false, false, true, 2, 6),
            DataRow(Visual.Cupertino, false, true, true, 2, 6),
            //DataRow(Visual.Cupertino, true, true, true, 2, 6),

            DataRow(Visual.Material, false, false, false, null, null),
            //DataRow(Visual.Material, true, false, false, null, null),
            DataRow(Visual.Material, false, true, false, null, null),
            //DataRow(Visual.Material, true, true, false, null, null),
            DataRow(Visual.Material, false, false, true, null, null),
            DataRow(Visual.Material, false, true, true, null, null),
            //DataRow(Visual.Material, true, true, true, null, null),
            DataRow(Visual.Material, false, false, true, 2, 6),
            DataRow(Visual.Material, false, true, true, 2, 6),
        //DataRow(Visual.Material, true, true, true, 2, 6)
        ]
        public Task BitSliderTest(Visual visual, bool isEnabled, bool vertical, bool ranged, int? defaultHigherValue, int? defaultLowerValue)
        {
            Context.JSInterop.Mode = JSRuntimeMode.Loose;
            var com = RenderComponent<BitSliderTest>(parameters =>
            {
                parameters.Add(p => p.Visual, visual);
                parameters.Add(p => p.IsEnabled, isEnabled);
                parameters.Add(p => p.Vertical, vertical);
                parameters.Add(p => p.Ranged, ranged);
                parameters.Add(p => p.DefaultHigherValue, defaultHigherValue);
                parameters.Add(p => p.DefaultLowerValue, defaultLowerValue);
            });


            var bitSlider = com.Find(".bit-slider");

            var isEnabledClass = isEnabled ? "enabled" : "disabled";
            var visualClass = visual == Visual.Cupertino ? "cupertino" : visual == Visual.Material ? "material" : "fluent";

            Assert.IsTrue(bitSlider.ClassList.Contains($"bit-slider-{isEnabledClass}-{visualClass}"));

            if (ranged)
            {
                Assert.IsTrue(bitSlider.ClassList.Contains($"bit-slider-ranged-{(vertical ? "column" : "row")}"));
                Assert.IsTrue(isEnabled ? com.Instance.LowerValue.HasValue : !com.Instance.LowerValue.HasValue);
                Assert.IsTrue(isEnabled ? com.Instance.HigherValue.HasValue : !com.Instance.HigherValue.HasValue);
                Assert.AreEqual(isEnabled ? defaultLowerValue.GetValueOrDefault() : null, com.Instance.LowerValue);
                Assert.AreEqual(isEnabled ? defaultHigherValue.GetValueOrDefault() : null, com.Instance.HigherValue);
            }
            else
            {
                Assert.IsTrue(bitSlider.ClassList.Contains($"bit-slider-{(vertical ? "column" : "row")}"));
                Assert.IsTrue(isEnabled ? com.Instance.Value.HasValue : !com.Instance.Value.HasValue);
                Assert.AreEqual(isEnabled ? 0 : null, com.Instance.Value);
            }

            return Task.CompletedTask;
        }

        [DataTestMethod,
            DataRow(Visual.Fluent, false, null),
            DataRow(Visual.Fluent, true, null),
            DataRow(Visual.Fluent, false, 2),
            DataRow(Visual.Fluent, true, 2),

            DataRow(Visual.Cupertino, false, null),
            DataRow(Visual.Cupertino, true, null),
            DataRow(Visual.Cupertino, false, 2),
            DataRow(Visual.Cupertino, true, 2),

            DataRow(Visual.Material, false, null),
            DataRow(Visual.Material, true, null),
            DataRow(Visual.Material, false, 2),
            DataRow(Visual.Material, true, 2),
        ]
        public Task BitSliderStepTest(Visual visual, bool ranged, int? step)
        {
            Context.JSInterop.Mode = JSRuntimeMode.Loose;
            var com = RenderComponent<BitSliderTest>(parameters =>
            {
                parameters.Add(p => p.Visual, visual);
                parameters.Add(p => p.Ranged, ranged);
                if (step.HasValue)
                {
                    parameters.Add(p => p.Step, step.Value);
                }
            });

            var bitSlider = com.Find(".bit-slider");
            foreach (var item in bitSlider.GetElementsByTagName("input"))
            {
                Assert.AreEqual(item.GetAttribute("step"), (step.HasValue ? step.Value : com.Instance.Step).ToString());
            }

            return Task.CompletedTask;
        }

        [DataTestMethod,
            DataRow(Visual.Fluent, false, null, null),
            DataRow(Visual.Fluent, false, 4, null),
            DataRow(Visual.Fluent, false, null, 8),
            DataRow(Visual.Fluent, true, null, null),
            DataRow(Visual.Fluent, true, 4, null),
            DataRow(Visual.Fluent, true, null, 8),
            DataRow(Visual.Fluent, false, 4, 8),
            DataRow(Visual.Fluent, true, 4, 8),

            DataRow(Visual.Cupertino, false, null, null),
            DataRow(Visual.Cupertino, false, 4, null),
            DataRow(Visual.Cupertino, false, null, 8),
            DataRow(Visual.Cupertino, true, null, null),
            DataRow(Visual.Cupertino, true, 4, null),
            DataRow(Visual.Cupertino, true, null, 8),
            DataRow(Visual.Cupertino, false, 4, 8),
            DataRow(Visual.Cupertino, true, 4, 8),

            DataRow(Visual.Material, false, null, null),
            DataRow(Visual.Material, false, 4, null),
            DataRow(Visual.Material, false, null, 8),
            DataRow(Visual.Material, true, null, null),
            DataRow(Visual.Material, true, 4, null),
            DataRow(Visual.Material, true, null, 8),
            DataRow(Visual.Material, false, 4, 8),
            DataRow(Visual.Material, true, 4, 8),
        ]
        public Task BitSliderMinMaxTest(Visual visual, bool ranged, int? min, int? max)
        {
            Context.JSInterop.Mode = JSRuntimeMode.Loose;
            var com = RenderComponent<BitSliderTest>(parameters =>
            {
                parameters.Add(p => p.Visual, visual);
                parameters.Add(p => p.Ranged, ranged);
                if (min.HasValue)
                {
                    parameters.Add(p => p.Min, min.Value);
                }

                if (max.HasValue)
                {
                    parameters.Add(p => p.Max, max.Value);
                }
            });

            var bitSlider = com.Find(".bit-slider");
            foreach (var item in bitSlider.GetElementsByTagName("input"))
            {
                Assert.AreEqual(item.GetAttribute("min"), (min.HasValue ? min.Value : com.Instance.Min).ToString());
                Assert.AreEqual(item.GetAttribute("max"), (max.HasValue ? max.Value : com.Instance.Max).ToString());
            }

            return Task.CompletedTask;
        }

        [DataTestMethod,
            DataRow(Visual.Fluent, false, null),
            DataRow(Visual.Fluent, true, null),
            DataRow(Visual.Fluent, false, "Bit Title"),
            DataRow(Visual.Fluent, true, "Bit Title"),

            DataRow(Visual.Cupertino, false, null),
            DataRow(Visual.Cupertino, true, null),
            DataRow(Visual.Cupertino, false, "Bit Title"),
            DataRow(Visual.Cupertino, true, "Bit Title"),

            DataRow(Visual.Material, false, null),
            DataRow(Visual.Material, true, null),
            DataRow(Visual.Material, false, "Bit Title"),
            DataRow(Visual.Material, true, "Bit Title"),
        ]
        public Task BitSliderLabelTest(Visual visual, bool ranged, string label)
        {
            Context.JSInterop.Mode = JSRuntimeMode.Loose;
            var com = RenderComponent<BitSliderTest>(parameters =>
            {
                parameters.Add(p => p.Visual, visual);
                parameters.Add(p => p.Ranged, ranged);
                parameters.Add(p => p.Label, label);
            });

            var bitSlider = com.Find(".bit-slider");
            var labels = bitSlider.GetElementsByTagName("label");

            if (string.IsNullOrEmpty(label))
            {
                Assert.IsFalse(labels.Any(l => l.ClassList.Contains("title")));
            }
            else
            {
                Assert.IsTrue(labels.Any(l => l.ClassList.Contains("title") && l.TextContent.Equals(label)));
            }

            return Task.CompletedTask;
        }

        [DataTestMethod,
            DataRow(Visual.Fluent, false, false),
            DataRow(Visual.Fluent, true, false),
            DataRow(Visual.Fluent, false, true),
            DataRow(Visual.Fluent, true, true),

            DataRow(Visual.Cupertino, false, false),
            DataRow(Visual.Cupertino, true, false),
            DataRow(Visual.Cupertino, false, true),
            DataRow(Visual.Cupertino, true, true),

            DataRow(Visual.Material, false, false),
            DataRow(Visual.Material, true, false),
            DataRow(Visual.Material, false, true),
            DataRow(Visual.Material, true, true),
        ]
        public Task BitSliderShowValueTest(Visual visual, bool ranged, bool showValue)
        {
            Context.JSInterop.Mode = JSRuntimeMode.Loose;
            var com = RenderComponent<BitSliderTest>(parameters =>
            {
                parameters.Add(p => p.Visual, visual);
                parameters.Add(p => p.Ranged, ranged);
                parameters.Add(p => p.ShowValue, showValue);
            });

            var bitSlider = com.Find(".bit-slider");
            var labels = bitSlider.GetElementsByTagName("label");

            if (showValue)
            {
                Assert.IsTrue(labels.Any(l => l.ClassList.Contains("valueLabel")));
                Assert.AreEqual(labels.Count(l => l.ClassList.Contains("valueLabel")), ranged ? 2 : 1);
            }
            else
            {
                Assert.IsFalse(labels?.Any(l => l.ClassList.Contains("valueLabel")) ?? false);
            }

            return Task.CompletedTask;
        }

        [DataTestMethod,
            DataRow(Visual.Fluent, false, false),
            DataRow(Visual.Fluent, true, false),
            DataRow(Visual.Fluent, false, true),
            DataRow(Visual.Fluent, true, true),

            DataRow(Visual.Cupertino, false, false),
            DataRow(Visual.Cupertino, true, false),
            DataRow(Visual.Cupertino, false, true),
            DataRow(Visual.Cupertino, true, true),

            DataRow(Visual.Material, false, false),
            DataRow(Visual.Material, true, false),
            DataRow(Visual.Material, false, true),
            DataRow(Visual.Material, true, true),
        ]
        public Task BitSliderOriginFromZeroTest(Visual visual, bool ranged, bool originFromZero)
        {
            Context.JSInterop.Mode = JSRuntimeMode.Loose;
            var com = RenderComponent<BitSliderTest>(parameters =>
            {
                parameters.Add(p => p.Visual, visual);
                parameters.Add(p => p.Ranged, ranged);
                parameters.Add(p => p.OriginFromZero, originFromZero);
            });

            var bitSlider = com.Find(".bit-slider");
            var labels = bitSlider.GetElementsByTagName("span");

            if (originFromZero)
            {
                Assert.IsTrue(labels.Any(l => l.ClassList.Contains("zeroTick")));
            }
            else
            {
                Assert.IsFalse(labels?.Any(l => l.ClassList.Contains("zeroTick")) ?? false);
            }

            return Task.CompletedTask;
        }

        [DataTestMethod,
            DataRow(Visual.Fluent, false, null),
            DataRow(Visual.Fluent, true, null),
            DataRow(Visual.Fluent, false, "P01"),
            DataRow(Visual.Fluent, true, "P01"),

            DataRow(Visual.Cupertino, false, null),
            DataRow(Visual.Cupertino, true, null),
            DataRow(Visual.Cupertino, false, "P01"),
            DataRow(Visual.Cupertino, true, "P01"),

            DataRow(Visual.Material, false, null),
            DataRow(Visual.Material, true, null),
            DataRow(Visual.Material, false, "P01"),
            DataRow(Visual.Material, true, "P01"),
        ]
        public Task BitSliderValueFormatTest(Visual visual, bool ranged, string valueFormat)
        {
            Context.JSInterop.Mode = JSRuntimeMode.Loose;
            var com = RenderComponent<BitSliderTest>(parameters =>
            {
                parameters.Add(p => p.Visual, visual);
                parameters.Add(p => p.Ranged, ranged);
                parameters.Add(p => p.ValueFormat, valueFormat);
            });

            var bitSlider = com.Find(".bit-slider");
            var labels = bitSlider.GetElementsByTagName("label");

            if (string.IsNullOrEmpty(valueFormat))
            {
                Assert.AreEqual(labels.Count(l => l.ClassList.Contains("valueLabel") && !l.TextContent.Contains("%")), ranged ? 2 : 1);
            }
            else
            {
                Assert.AreEqual(labels.Count(l => l.ClassList.Contains("valueLabel") && l.TextContent.Contains("%")), ranged ? 2 : 1);
            }

            return Task.CompletedTask;
        }
    }
}
