using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pax.blazor.chartjs
{
    public class GoogleChartData
    {
        public List<string> Labels { get; set; }
        public List<float> Data { get; set; }

        public void GetData()
        {
            List<string> label = new List<string>();
            label.Add("label");
            label.AddRange(Labels);

            List<object> data = new List<object>();


        }
    }

    public class GoogleChartRow
    {
        public List<string> Labels { get; set; }
        public List<float> Data { get; set; }
    }
}
