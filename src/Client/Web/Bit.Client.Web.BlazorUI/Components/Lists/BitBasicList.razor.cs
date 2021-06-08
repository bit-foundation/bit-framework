﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Bit.Client.Web.BlazorUI
{
    public partial class BitBasicList<TItem>
    {
        [Parameter] public ICollection<TItem> Items { get; set; } = Array.Empty<TItem>();
        [Parameter] public string Role { get; set; } = "list";
        [Parameter] public bool Virtualize { get; set; } = true;
        [Parameter] public string? LoadingText { get; set; }
        [Parameter] public int OverscanCount { get; set; } = 3;
        [Parameter] public int ItemSize { get; set; } = 50;
        [Parameter] public RenderFragment<TItem>? RowTemplate { get; set; }

        protected override string RootElementClass => "bit-bsc-lst";
        public override Task SetParametersAsync(ParameterView parameters)
        {
            foreach (ParameterValue parameter in parameters)
            {
                switch (parameter.Name)
                {
                    case nameof(Items):
                        Items = (System.Collections.Generic.ICollection<TItem>)parameter.Value;
                        break;
                    case nameof(Role):
                        Role = (string)parameter.Value;
                        break;
                    case nameof(Virtualize):
                        Virtualize = (bool)parameter.Value;
                        break;
                    case nameof(LoadingText):
                        LoadingText = (string)parameter.Value;
                        break;
                    case nameof(OverscanCount):
                        OverscanCount = (int)parameter.Value;
                        break;
                    case nameof(ItemSize):
                        ItemSize = (int)parameter.Value;
                        break;
                    case nameof(RowTemplate):
                        RowTemplate = (Microsoft.AspNetCore.Components.RenderFragment<TItem>)parameter.Value;
                        break;
                }
            }
            return base.SetParametersAsync(parameters);
        }
    }
}
