using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.Text.Json;

namespace Bit.Client.Web.BlazorUI
{
    public partial class BitColorPicker : IDisposable
    {
        private string onWindowMouseUpAbortControllerId;
        private string onWindowMouseMoveAbortControllerId;

        public bool ColorHasBeenSet { get; set; }

        private RGB mainColor = new RGB() { Red = 255 };

        protected override string RootElementClass => "bit-clr-pkr";

        [Inject] public IJSRuntime JSRuntime { get; set; }

        public ElementReference SaturationPickerRef { get; set; }

        public Position? SaturationPickerThumbPosition { get; set; }
        public bool SaturationPickerMouseDown { get; set; }

        public HSV SaturationPickerBackground { get; set; } = new HSV() { Hue = 0, Saturation = 1, Value = 1 };

        public double SelectedSaturation { get; set; } = 1;
        public double SelectedValue { get; set; } = 1;
        public double Hue { get; set; }
        public double Alpha { get; set; } = 100;

        public RGB MainColor
        {
            get => mainColor; set
            {
                mainColor = value;
            }
        }

        [Parameter]
        public string Color
        {
            get => MainColor.ToCss(Alpha);
            set
            {
                if (value == MainColor.ToCss(Alpha)) return;
                MainColor = StringToRGBConverter(value);
                Hue = MainColor.toHSV().Hue;
                SaturationPickerBackground = new HSV { Hue = Hue, Value = 1, Saturation = 1 };

                ColorChanged.InvokeAsync(value);
            }
        }

        [Parameter]
        public EventCallback<string> ColorChanged { get; set; }

        [Parameter]
        public bool ShowPreview { get; set; }

        [Parameter]
        public AlphaType AlphaType { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                onWindowMouseUpAbortControllerId = await JSRuntime.OnWindowMouseUp(this, "OnWindowMouseUp");
                onWindowMouseMoveAbortControllerId = await JSRuntime.OnWindowMouseMove(this, "OnWindowMouseMove");

                var saturationPickerRect = await JSRuntime.GetBoundingClientRect(SaturationPickerRef);

                SaturationPickerThumbPosition = new Position
                {
                    Left = Convert.ToInt32(saturationPickerRect.Width),
                    Top = 0
                };
            }

            await base.OnAfterRenderAsync(firstRender);

        }

        [JSInvokable]
        public async Task OnWindowMouseUp(MouseEventArgs e)
        {
            SaturationPickerMouseDown = false;
        }

        [JSInvokable]
        public async Task OnWindowMouseMove(MouseEventArgs e)
        {
            await OnSaturationPickerMouseMove(e);
        }

        private async Task PickColorTune(MouseEventArgs e)
        {
            var parent = await JSRuntime.GetBoundingClientRect(SaturationPickerRef);

            SaturationPickerThumbPosition = new Position
            {
                Left = e.ClientX < parent.Left ? 0 : e.ClientX > parent.Left + parent.Width ? Convert.ToInt32(parent.Width) : Convert.ToInt32(e.ClientX - parent.Left),
                Top = e.ClientY < parent.Top ? 0 : e.ClientY > parent.Top + parent.Height ? Convert.ToInt32(parent.Height) : Convert.ToInt32(e.ClientY - parent.Top)
            };


            SelectedSaturation = Math.Clamp(ToValidSpanValue(0, parent.Width, 0, 1, Convert.ToInt32(e.ClientX - parent.Left)), 0, 1);
            SelectedValue = Math.Clamp(ToValidSpanValue(0, parent.Height, 0, 1, parent.Height - Convert.ToInt32(e.ClientY - parent.Top)), 0, 1);

            MainColor = new HSV() { Hue = Hue, Saturation = SelectedSaturation, Value = SelectedValue }.ToRGB();
            SaturationPickerBackground = new HSV { Hue = Hue, Value = 1, Saturation = 1 };

            await ColorChanged.InvokeAsync(MainColor.ToCss(Alpha));

            StateHasChanged();
        }

        private async Task PickMainColor(ChangeEventArgs args)
        {
            Hue = Convert.ToInt32(args.Value);
            MainColor = new HSV() { Hue = Hue, Saturation = SelectedSaturation, Value = SelectedValue }.ToRGB();
            SaturationPickerBackground = new HSV() { Hue = Hue, Saturation = 1, Value = 1 };

            await ColorChanged.InvokeAsync(MainColor.ToCss(Alpha));

        }

        private async Task PickAlphaColor(ChangeEventArgs args)
        {
            Alpha = Convert.ToDouble(args.Value);
            await ColorChanged.InvokeAsync(MainColor.ToCss(Alpha));

        }

        private async Task RGBColorChanged(string? red = null, string? green = null, string? blue = null)
        {

            MainColor.Red = string.IsNullOrWhiteSpace(red) ? MainColor.Red : Convert.ToDouble(red);
            MainColor.Green = string.IsNullOrWhiteSpace(green) ? MainColor.Green : Convert.ToDouble(green);
            MainColor.Blue = string.IsNullOrWhiteSpace(blue) ? MainColor.Blue : Convert.ToDouble(blue);

            var saturationPickerRect = await JSRuntime.GetBoundingClientRect(SaturationPickerRef);

            var hsv = MainColor.toHSV();
            Hue = hsv.Hue;
            SaturationPickerBackground = new HSV() { Hue = Hue, Saturation = 1, Value = 1 };

            SaturationPickerThumbPosition = new Position
            {
                Left = Convert.ToInt32(saturationPickerRect.Width * hsv.Saturation / 100),
                Top = Convert.ToInt32(saturationPickerRect.Height - saturationPickerRect.Height * hsv.Value / 100)
            };

            await ColorChanged.InvokeAsync(MainColor.ToCss(Alpha));

            StateHasChanged();
        }

        private async Task HexCHanged(ChangeEventArgs args)
        {
            var color = args.Value.ToString();

            if (color.Length is 6 && Regex.IsMatch(color, @"\A\b[0-9a-fA-F]+\b\Z"))
            {
                var hex = new Hex() { ColorCode = color };

                var saturationPickerRect = await JSRuntime.GetBoundingClientRect(SaturationPickerRef);

                MainColor = hex.ToRGB();
                var hsv = MainColor.toHSV();
                Hue = hsv.Hue;
                SaturationPickerBackground = new HSV() { Hue = Hue, Saturation = 1, Value = 1 };

                SaturationPickerThumbPosition = new Position
                {
                    Left = Convert.ToInt32(saturationPickerRect.Width * hsv.Saturation / 100),
                    Top = Convert.ToInt32(saturationPickerRect.Height - saturationPickerRect.Height * hsv.Value / 100)
                };

                StateHasChanged();
            }
        }

        private void setSaturationPickerThumbPosition(int top, int left)
        {

        }

        private double ToValidSpanValue(double min, double max, double newMin, double newMax, double value)
        {
            return (value - min) * (newMax - newMin) / (max - min);
        }


        private async Task OnSaturationPickerMouseDown(MouseEventArgs e)
        {
            SaturationPickerMouseDown = true;

            await PickColorTune(e);

        }
        private async Task OnSaturationPickerMouseMove(MouseEventArgs e)
        {
            if (SaturationPickerMouseDown is false) return;

            await PickColorTune(e);
        }
        private RGB StringToRGBConverter(string value)
        {
            RGB color = new RGB { Red = 0, Green = 0, Blue = 0 };
            try
            {
                Regex rx = new Regex(@"\(([^)]+)\)");
                var mathedColor = rx.Match(value).Value;

                mathedColor = mathedColor.Trim('(');
                mathedColor = mathedColor.Trim(')');

                var colorString = mathedColor.Split(",");
                if (colorString.Length >= 3)
                {
                    color.Red = int.Parse(colorString[0]);
                    color.Green = int.Parse(colorString[1]);
                    color.Blue = int.Parse(colorString[2]);
                    Alpha = AlphaType is AlphaType.Transparency ? (1 - Convert.ToDouble(colorString[3])) * 100 : Convert.ToDouble(colorString[3]) * 100;
                }
            }
            catch (Exception exp) { }

            return color;
        }

        public void Dispose()
        {
            JSRuntime.AbortProcedure(onWindowMouseUpAbortControllerId);
            JSRuntime.AbortProcedure(onWindowMouseMoveAbortControllerId);
        }
    }

    public class Position
    {
        public int Top { get; set; }
        public int Left { get; set; }
    }

}
