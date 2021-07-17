using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Bit.Client.Web.BlazorUI
{
    public partial class BitTextField
    {
        private bool isMultiLine;
        private bool isReadonly;
        private bool isRequired;
        private string focusClass = "";
        private TextFieldType type = TextFieldType.Text;
        private Guid _inputId = Guid.NewGuid();

        [Parameter] public int MaxLength { get; set; } = -1;

        [Parameter] public string? IconName { get; set; }

        [Parameter] public string? Value { get; set; }

        [Parameter] public string? Placeholder { get; set; }

        [Parameter] public string? Label { get; set; }

        [Parameter]
        public bool CanRevealPassword { get; set; }

        [Parameter]
        public bool IsReadonly
        {
            get => isReadonly;
            set
            {
                isReadonly = value;
                ClassBuilder.Reset();
            }
        }

        [Parameter]
        public bool IsRequired
        {
            get => isRequired;
            set
            {
                isRequired = value;
                ClassBuilder.Reset();
            }
        }

        [Parameter]
        public TextFieldType Type
        {
            get => type;
            set
            {
                type = value;
                ElementType = value;
                ClassBuilder.Reset();
            }
        }

        public TextFieldType ElementType { get; set; }

        [Parameter]
        public bool IsMultiLine
        {
            get => isMultiLine;
            set
            {
                isMultiLine = value;
                ClassBuilder.Reset();
            }
        }

        [Parameter] public EventCallback<FocusEventArgs> OnFocusIn { get; set; }

        [Parameter] public EventCallback<FocusEventArgs> OnFocusOut { get; set; }

        [Parameter] public EventCallback<FocusEventArgs> OnFocus { get; set; }

        [Parameter] public EventCallback<ChangeEventArgs> OnChange { get; set; }

        [Parameter] public EventCallback<KeyboardEventArgs> OnKeyDown { get; set; }

        [Parameter] public EventCallback<KeyboardEventArgs> OnKeyUp { get; set; }

        [Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }

        public string FocusClass
        {
            get => focusClass;
            set
            {
                focusClass = value;
                ClassBuilder.Reset();
            }
        }

        protected override string RootElementClass => "bit-txt";

        protected override void RegisterComponentClasses()
        {
            ClassBuilder.Register(() => IsMultiLine && Type == TextFieldType.Text
                                        ? $"{RootElementClass}-multiline-{VisualClassRegistrar()}" : string.Empty);

            ClassBuilder.Register(() => IsEnabled is false
                                        ? $"{RootElementClass}-disabled-{VisualClassRegistrar()}" : string.Empty);

            ClassBuilder.Register(() => IsEnabled && IsReadonly
                                        ? $"{RootElementClass}-readonly-{VisualClassRegistrar()}" : string.Empty);

            ClassBuilder.Register(() => IsEnabled && IsRequired
                                        ? $"{RootElementClass}-required-{VisualClassRegistrar()}" : string.Empty);

            ClassBuilder.Register(() => FocusClass.HasValue()
                                        ? $"{RootElementClass}-{FocusClass}-{VisualClassRegistrar()}" : string.Empty);
        }

        protected virtual async Task HandleFocusIn(FocusEventArgs e)
        {
            if (IsEnabled)
            {
                FocusClass = "focused";
                await OnFocusIn.InvokeAsync(e);
            }
        }

        protected virtual async Task HandleFocusOut(FocusEventArgs e)
        {
            if (IsEnabled)
            {
                FocusClass = "";
                await OnFocusOut.InvokeAsync(e);
            }
        }

        protected virtual async Task HandleFocus(FocusEventArgs e)
        {
            if (IsEnabled)
            {
                FocusClass = "focused";
                await OnFocus.InvokeAsync(e);
            }
        }

        protected virtual async Task HandleChange(ChangeEventArgs e)
        {
            if (IsEnabled)
            {
                await OnChange.InvokeAsync(e);
            }
        }

        protected virtual async Task HandleKeyDown(KeyboardEventArgs e)
        {
            if (IsEnabled)
            {
                await OnKeyDown.InvokeAsync(e);
            }
        }

        protected virtual async Task HandleKeyUp(KeyboardEventArgs e)
        {
            if (IsEnabled)
            {
                await OnKeyUp.InvokeAsync(e);
            }
        }

        protected virtual async Task HandleClick(MouseEventArgs e)
        {
            if (IsEnabled)
            {
                await OnClick.InvokeAsync(e);
            }
        }

        public void TogglePasswordRevealIcon()
        {
            ElementType = ElementType == TextFieldType.Text ? TextFieldType.Password : TextFieldType.Text;
        }
    }
}
