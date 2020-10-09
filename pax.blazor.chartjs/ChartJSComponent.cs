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
        public ChartData chartData { get; set; }

        [Parameter]
        public int chartId { get; set; }

        public string canvasId => "paxchartjscanvas" + chartId;

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _js.InvokeVoidAsync("initiateChartJS", chartId);
                DrawChart();
            }
            return base.OnAfterRenderAsync(firstRender);
        }

        protected override Task OnParametersSetAsync()
        {
            //DrawChart();
            return base.OnParametersSetAsync();
        }

        public async void DrawChart()
        {
            bool done = false;
            if (chartData.ChartType == ChartType.Radar)
            {
                done = await _js.InvokeAsync<bool>("RadarChart", chartId);
                logger.LogInformation("radar init " + chartId);
            } else if (chartData.ChartType == ChartType.Pie)
            {
                done = await _js.InvokeAsync<bool>("PieChart", chartId);
                logger.LogInformation("pie init " + chartId);
            }

            if (done)
            {
                for (int i = 0; i < chartData.Data.Count; i++)
                {
                    if (chartData.Colors == null || chartData.Colors.Count != chartData.Data.Count)
                        await _js.InvokeVoidAsync("AddData", chartId, chartData.Labels[i], chartData.Data[i], ChartColors.GetColor(i, chartData.ChartType), "");
                    else
                        await _js.InvokeVoidAsync("AddData", chartId, chartData.Labels[i], chartData.Data[i], chartData.Colors[i], "");
                }
            }
            else
                logger.LogError("failed init chart " + chartId);
        }

        public async void Test()
        {
            PieChart pieChart = new PieChart();
            pieChart.piedata = new List<double>() { 0.2, 0.4, 0.3, 0.1 };
            pieChart.pielabels = new List<string>() { "red", "green", "blue", "black" };
            // "rgba(246, 246, 115,1)"
            pieChart.piecolors = new List<string>() { "rgba(255, 0, 0,1)", "rgba(0, 255, 0,1)", "rgba(0, 0, 255,1)", "rgba(0, 0, 0,1)" };
            //_js.InvokeVoidAsync("PieChart", "paxchartjscanvas", pieChart);
            await _js.InvokeVoidAsync("RadarChart", chartId, pieChart);

            for (int i = 0; i < pieChart.piedata.Count; i++)
            {
                await _js.InvokeVoidAsync("AddData", chartId, pieChart.pielabels[i], pieChart.piedata[i], "rgba(0, 0, 255, 0.6)", "");
            }
            
        }
    }
}
