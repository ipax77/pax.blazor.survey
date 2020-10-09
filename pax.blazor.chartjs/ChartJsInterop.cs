using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pax.blazor.chartjs
{
    public class ChartJsInterop
    {
        public static async Task Init(IJSRuntime jsRuntime, string canvasId)
        {
            await jsRuntime.InvokeVoidAsync("DrawChart", canvasId);
        }


    }
}
