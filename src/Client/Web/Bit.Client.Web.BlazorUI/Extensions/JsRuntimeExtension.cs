using System.Threading.Tasks;
using Bit.Client.Web.BlazorUI;
using Microsoft.AspNetCore.Components;

namespace Microsoft.JSInterop
{
    public static class JsRuntimeExtension
    {
        public static async Task SetProperty(this IJSRuntime jsRuntime, ElementReference element, string property, object value)
        {
            await jsRuntime.InvokeVoidAsync("Bit.setProperty", element, property, value);
        }

        public static async Task<string> GetProperty(this IJSRuntime jsRuntime, ElementReference element, string property)
        {
            return await jsRuntime.InvokeAsync<string>("Bit.getProperty", element, property);
        }

        public static async Task<BoundingClientRect> GetBoundingClientRect(this IJSRuntime jsRuntime, ElementReference element)
        {
            return await jsRuntime.InvokeAsync<BoundingClientRect>("Bit.getBoundingClientRect", element);
        }

        public static async Task<string> OnWindowMouseUp(this IJSRuntime jsRuntime, BitComponentBase dontetHelper, string callbackName)
        {

            return await jsRuntime.InvokeAsync<string>("Bit.onWindowMouseUp", DotNetObjectReference.Create(dontetHelper),
                callbackName);
        }
        public static async Task<string> OnWindowMouseMove(this IJSRuntime jsRuntime, BitComponentBase dontetHelper, string callbackName)
        {
            return await jsRuntime.InvokeAsync<string>("Bit.onWindowMouseMove", DotNetObjectReference.Create(dontetHelper),
                 callbackName);
        }

        public static async Task AbortProcedure(this IJSRuntime jSRuntime, string abortControllerId)
        {
            await jSRuntime.InvokeVoidAsync("Bit.abortProcedure", abortControllerId);
        }
    }

    public class BoundingClientRect
    {
        public double Bottom { get; set; }
        public double Height { get; set; }
        public double Left { get; set; }
        public double Right { get; set; }
        public double Top { get; set; }
        public double Width { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
    }
}
