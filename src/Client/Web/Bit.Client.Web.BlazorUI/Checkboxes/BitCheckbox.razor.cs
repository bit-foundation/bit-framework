﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Threading.Tasks;
using System;

namespace Bit.Client.Web.BlazorUI.Checkboxes
{
    public partial class BitCheckbox
    {
        [Parameter] public string Label { get; set; }
        [Parameter] public string CustomLabel { get; set; }
        [Parameter] public bool IsChecked { get; set; }
        [Parameter] public bool IsIndeterminate { get; set; }
        [Parameter] public string BoxSide { get; set; }
        [Parameter] public EventCallback<MouseEventArgs> OnCheckboxClick { get; set; }
        [Parameter] public Action<bool> OnCheckboxChange { get; set; }


        public string CheckedClass => IsChecked ? "checked" : string.Empty;
        public string IndeterminateClass => IsIndeterminate ? "indeterminate" : string.Empty;
        public bool HasCustomLabel => CustomLabel.HasValue() ? true : false;

        public string InputUniqueIdentifier = Guid.NewGuid().ToString("n");

        protected void HandleCheckboxClick()
        {
            if (IsEnabled)
            {
                if (IsChecked)
                {
                    IsChecked = false;
                }
                else {
                    IsChecked = true;
                }
                OnCheckboxChange?.Invoke(IsChecked);
            }
        }
        public override Task SetParametersAsync(ParameterView parameters)
        {
            foreach (ParameterValue parameter in parameters)
            {
                switch (parameter.Name)
                {
                    case nameof(Label):
                        Label = (string)parameter.Value;
                        break;
                    case nameof(CustomLabel):
                        CustomLabel = (string)parameter.Value;
                        break;
                    case nameof(IsChecked):
                        IsChecked = (bool)parameter.Value;
                        break;
                    case nameof(IsIndeterminate):
                        IsIndeterminate = (bool)parameter.Value;
                        break;
                    case nameof(BoxSide):
                        BoxSide = (string)parameter.Value;
                        break;
                    case nameof(OnCheckboxChange):
                        OnCheckboxChange = (Action<bool>)parameter.Value;
                        break;
                }
            }

            return base.SetParametersAsync(parameters);
        }
    }
}