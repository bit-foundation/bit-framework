using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Bit.Client.Web.BlazorUI
{
    public partial class BitDropDown
    {
        private string focusClass = "";
        private string expandClass = "";
        private bool isMultiSelect = false;

        [Parameter]
        public bool IsMultiSelect
        {
            get => isMultiSelect;
            set
            {
                isMultiSelect = value;
                ClassBuilder.Reset();
            }
        }
        
        [Parameter] public List<DropDownItem> Items { get; set; } = new List<DropDownItem>();
        [Parameter] public string? Placeholder { get; set; }

        [Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }
        [Parameter] public EventCallback<DropDownItem> OnSelectItem { get; set; }

        public string? Text { get; set; }

        public string FocusClass
        {
            get => focusClass;
            set
            {
                focusClass = value;
                ClassBuilder.Reset();
            }
        }
        public string ExpandClass
        {
            get => expandClass;
            set
            {
                expandClass = value;
                ClassBuilder.Reset();
            }
        }

        protected override string RootElementClass => "bit-drp";
        protected override void RegisterComponentClasses()
        {
            ClassBuilder.Register(() => FocusClass.HasNoValue()
                ? string.Empty
                : $"{RootElementClass}-{FocusClass}-{VisualClassRegistrar()}");

            ClassBuilder.Register(() => ExpandClass.HasNoValue()
                ? string.Empty
                : $"{RootElementClass}-{ExpandClass}-{VisualClassRegistrar()}");

            ClassBuilder.Register(() => Items.Any(prop => prop.IsSelected)
                ? string.Empty
                : $"{RootElementClass}-{"hasValue"}-{VisualClassRegistrar()}");

            ClassBuilder.Register(() => IsMultiSelect is false
                ? string.Empty
                : $"{RootElementClass}-{"multi"}-{VisualClassRegistrar()}");

            ClassBuilder.Register(() => "bit-cal-com");
        }

        protected virtual async Task HandleClick(MouseEventArgs e)
        {
            if (IsEnabled)
            {
                if (FocusClass.HasNoValue())
                {
                    FocusClass = "focused";
                }
                else
                {
                    FocusClass = "";
                }
                await OnClick.InvokeAsync(e);
            }
        }

        protected virtual async Task HandleItemClick(DropDownItem? selectedItem)
        {
            if (selectedItem is not null)
            {
                if (selectedItem.IsEnabled)
                {
                    if (IsMultiSelect)
                    {
                        if (Text.HasValue())
                        {
                            Text += ", ";
                        }
                        Text += selectedItem.Text;
                    }
                    else
                    {
                        ChangeAllItemsIsSelected(false);
                        Text = selectedItem.Text;
                        selectedItem.IsSelected = true;
                    }
                    await OnSelectItem.InvokeAsync(selectedItem);
                }
            }
            else
            {
                if (IsMultiSelect)
                {
                    Text = string.Empty;
                    foreach (var item in Items)
                    {
                        if (item.IsSelected)
                        {
                            if (Text.HasValue())
                            {
                                Text += ", ";
                            }
                            Text += item.Text;
                        }
                    }
                }
            }
        }

        internal void ChangeAllItemsIsSelected(bool value)
        {
            foreach (var item in Items)
            {
                item.IsSelected = value;
            }
        }
    }
}
