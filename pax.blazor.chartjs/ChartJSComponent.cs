using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pax.blazor.chartjs
{
    public partial class ChartJSComponent
    {
        [Inject]
        IJSRuntime _js { get; set; }

        [Inject] public ILogger<ChartJSComponent> logger { get; set; }

        [Parameter]
        public Chart chart { get; set; }

        [Parameter]
        public int chartId { get; set; }

        bool isLoading = true;
        public string canvasId => "paxchartjscanvas" + chartId;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _js.InvokeVoidAsync("initiateChartJS", chartId);
                DrawChart();
            }
        }

        protected override Task OnParametersSetAsync()
        {
            //DrawChart();
            return base.OnParametersSetAsync();
        }

        public async Task DrawChart()
        {
            if (chart == null)
                return;
            bool done = false;
            if (chart.ChartType == ChartType.Radar)
            {
                done = await _js.InvokeAsync<bool>("RadarChart", chartId);
                logger.LogInformation("radar init " + chartId);
            } else if (chart.ChartType == ChartType.Pie)
            {
                done = await _js.InvokeAsync<bool>("PieChart", chartId);
                logger.LogInformation("pie init " + chartId);
            }

            if (done)
            {
                int i = 0;
                foreach (ChartData data in chart.Data.OrderBy(o => o.Result))
                {
                    i++;
                    await _js.InvokeVoidAsync("AddData", chartId, data.Label, data.Result, data.Color == null ? ChartColors.GetColor(i, chart.ChartType) : data.Color, "");
                }
            }
            else
                logger.LogError("failed init chart " + chartId);

            isLoading = false;
            StateHasChanged();
        }
    }
}
