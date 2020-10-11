using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pax.blazor.chartjs
{
    public class Chart
    {
        public List<ChartData> Data { get; set; } = new List<ChartData>();
        public ChartType ChartType { get; set; }
    }

    [Serializable]
    public class ChartData
    {
        public string Label { get; set; }
        public double Result { get; set; }
        public string Color { get; set; }
    }


    public class ChartColors
    {
        public static List<string> Colors = new List<string>()
        {
            "#FFFF00",
            "#0000FF",
            "#008000",
            "#FF0000",
            "#800080",
            "#191970",
            "#FFC0CB",
            "#4B0082",
            "#FF00FF",
            "#000080",
            "#9400D3",
            "#FF8C00",
            "#00FFFF",
            "#DC143C",
            "#FF8C00",
        };

        public static string GetColor(int i, ChartType chartType)
        {
            if (chartType == ChartType.Bar)
                return "rgba(34, 38, 255, 0.45)";

            string mycolor = String.Empty;
            if (i >= 0 && i < Colors.Count)
                mycolor = Colors[i];
            else
                mycolor = Colors[i % Colors.Count];
            Color color = ColorTranslator.FromHtml(mycolor);
            string temp_col = color.R + ", " + color.G + ", " + color.B;
            if (chartType == ChartType.Radar)
                return "rgba(" + temp_col + ", 0.5)";

            return "rgba(" + temp_col + ", 1)";
        }
    }

    public enum ChartType
    {
        Pie,
        Radar,
        Bar
    }
}
