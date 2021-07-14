using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Bit.Client.Web.BlazorUI
{
    public partial class BitSlider
    {
        private int? upperValue;
        private int? lowerValue;
        private int? value;
        private (int Lower, int Upper) rangeValue;

        private bool isReadOnly;
        private string? styleProgress;
        private string? styleContainer;
        private int inputHeight;

        private ElementReference ContainerRef { get; set; }
        private ElementReference TitleRef { get; set; }
        private ElementReference ValueLabelRef { get; set; }

        [Inject] public IJSRuntime? JSRuntime { get; set; }
        [Parameter] public int? DefaultUpperValue { get; set; }
        [Parameter] public int? DefaultLowerValue { get; set; }
        [Parameter] public int? DefaultValue { get; set; }
        [Parameter] public int Min { get; set; } = 1;
        [Parameter] public int Max { get; set; } = 10;
        [Parameter] public int Step { get; set; } = 1;

        [Parameter]
        public int? UpperValue
        {
            get => upperValue;
            set
            {
                if (value == upperValue) return;
                upperValue = value;
                ClassBuilder.Reset();
                _ = UpperValueChanged.InvokeAsync(value);
            }
        }
        [Parameter] public EventCallback<int?> UpperValueChanged { get; set; }

        [Parameter]
        public int? LowerValue
        {
            get => lowerValue;
            set
            {
                if (value == lowerValue) return;
                lowerValue = value;
                ClassBuilder.Reset();
                _ = LowerValueChanged.InvokeAsync(value);
            }
        }
        [Parameter] public EventCallback<int?> LowerValueChanged { get; set; }

        [Parameter]
        public int? Value
        {
            get => value;
            set
            {
                if (value == this.value) return;
                this.value = value;
                ClassBuilder.Reset();
                _ = ValueChanged.InvokeAsync(value);
            }
        }
        [Parameter] public EventCallback<int?> ValueChanged { get; set; }

        [Parameter]
        public (int Lower, int Upper) RangeValue
        {
            get => rangeValue;
            set
            {
                if (value == this.rangeValue) return;
                this.rangeValue = value;
                ClassBuilder.Reset();
                _ = RangeValueChanged.InvokeAsync(value);
            }
        }
        [Parameter] public EventCallback<(int Lower, int Upper)> RangeValueChanged { get; set; }

        [Parameter] public bool IsOriginFromZero { get; set; }
        [Parameter] public string? Label { get; set; }
        [Parameter] public bool IsRanged { get; set; }
        [Parameter] public bool ShowValue { get; set; } = true;
        [Parameter] public bool IsVertical { get; set; }
        [Parameter] public string? ValueFormat { get; set; }
        [Parameter] public EventCallback<ChangeEventArgs> OnChange { get; set; }
        protected override string RootElementClass => "bit-slider";

        [Parameter]
        public bool IsReadonly
        {
            get => isReadOnly;
            set
            {
                isReadOnly = value;
                ClassBuilder.Reset();
            }
        }

        protected override void RegisterComponentClasses()
        {
            ClassBuilder.Register(() => IsReadonly
                                                ? $"{RootElementClass}-readonly-{VisualClassRegistrar()}"
                                                : string.Empty);

            ClassBuilder.Register(() => IsVertical
                                                ? $"{RootElementClass}{(IsRanged ? "-ranged" : null)}-column"
                                                : $"{RootElementClass}{(IsRanged ? "-ranged" : null)}-row");
        }

        protected override void OnInitialized()
        {
            if (DefaultUpperValue.HasValue)
            {
                UpperValue = DefaultUpperValue.Value;
            }

            if (DefaultLowerValue.HasValue)
            {
                LowerValue = DefaultLowerValue.Value;
            }

            if (IsRanged)
            {
                RangeValue = (LowerValue.GetValueOrDefault(RangeValue.Lower), UpperValue.GetValueOrDefault(RangeValue.Upper));

                if (!LowerValue.HasValue)
                {
                    LowerValue = RangeValue.Lower;
                }

                if (!UpperValue.HasValue)
                {
                    UpperValue = RangeValue.Upper;
                }
            }
            else
            {
                Value = DefaultValue.GetValueOrDefault(Min);
            }

            if (!IsVertical)
            {
                FillSlider();
            }

            base.OnInitialized();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender && IsVertical)
            {
                if (IsRanged)
                {
                    inputHeight = await JSRuntime.GetClientHeight(RootElement);

                    if (Label.HasValue())
                    {
                        var titleHeight = await JSRuntime.GetClientHeight(TitleRef);
                        inputHeight -= titleHeight;
                    }

                    if (ShowValue)
                    {
                        var valueLabelHeight = await JSRuntime.GetClientHeight(ValueLabelRef);
                        inputHeight -= (valueLabelHeight * 2);
                    }
                }
                else
                {
                    inputHeight = await JSRuntime.GetClientHeight(ContainerRef);
                }
                FillSlider();
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        protected virtual async Task HandleChange(ChangeEventArgs e)
        {
            if (IsEnabled)
            {
                await OnChange.InvokeAsync(e);
            }
        }

        private void HandleInput(ChangeEventArgs e)
        {
            if (!IsRanged)
            {
                Value = Convert.ToInt32(e.Value);
                FillSlider();
            }
            else
            {
                UpperValue = null;
                LowerValue = null;
            }
        }

        private void HandleInput(ChangeEventArgs e, bool isLower)
        {
            if (IsRanged)
            {
                if (isLower)
                {
                    LowerValue = Convert.ToInt32(e.Value);
                }
                else
                {
                    UpperValue = Convert.ToInt32(e.Value);
                }

                RangeValue = (LowerValue.GetValueOrDefault(RangeValue.Lower), UpperValue.GetValueOrDefault(RangeValue.Upper));
                FillSlider();
            }
            else
            {
                Value = null;
            }
        }

        private void FillSlider()
        {
            if (IsRanged)
            {
                styleProgress = $"--l: {RangeValue.Lower}; --h: {RangeValue.Upper}; --min: {Min}; --max: {Max}";
                if (IsVertical)
                {
                    styleContainer = $"width: {inputHeight}px; height: {inputHeight}px;";
                    StateHasChanged();
                }
            }
            else
            {
                if (IsVertical)
                {
                    styleProgress = $"--value: {Value}; --min: {Min}; --max: {Max}; width: {inputHeight}px; transform: rotate(270deg) translateX(-{(inputHeight - 12)}px);";
                    StateHasChanged();
                }
                else
                {
                    styleProgress = $"--value: {Value}; --min: {Min}; --max: {Max};";
                }
            }
        }

        private string GetValueDisplay(int? val)
        {
            if (ValueFormat.HasNoValue())
            {
                return val.ToString();
            }
            else if (ValueFormat!.Contains("p", StringComparison.CurrentCultureIgnoreCase))
            {
                int digitCount = (Max - 1).ToString().Length;
                return (val.GetValueOrDefault() / Math.Pow(10, digitCount)).ToString(ValueFormat);
            }
            else
            {
                return val.GetValueOrDefault().ToString(ValueFormat);
            }
        }
    }
}
