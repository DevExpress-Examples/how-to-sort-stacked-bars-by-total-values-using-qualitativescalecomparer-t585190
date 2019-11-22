using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraCharts;

using System.Collections;

namespace ChartStackedSorting
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            chartControl1.Series.Clear();

            Series s1, s2, s3;

            s1 = new Series("Serie1", ViewType.StackedBar);
            s2 = new Series("Serie2", ViewType.StackedBar);
            s3 = new Series("Serie3", ViewType.StackedBar);
            s1.ArgumentScaleType = ScaleType.Qualitative;
            s2.ArgumentScaleType = ScaleType.Qualitative;
            s3.ArgumentScaleType = ScaleType.Qualitative;
            
            
            chartControl1.Series.AddRange(new Series[] { s1, s2, s3 });
            var axisX = ((XYDiagram)chartControl1.Diagram).AxisX;
            axisX.QualitativeScaleComparer = new TotalScaleValuesComparer(chartControl1);
            axisX.Reverse = true;

            Random r = new Random();
            for (int i = 0; i < 10; i++) {
                s1.Points.Add(new SeriesPoint(i, Math.Round(r.NextDouble() * 100)));
                s2.Points.Add(new SeriesPoint(i, Math.Round(r.NextDouble() * 100)));
                s3.Points.Add(new SeriesPoint(i, Math.Round(r.NextDouble() * 100)));
            }
        }
    }

    public class TotalScaleValuesComparer : IComparer {

        ChartControl chart;
        public TotalScaleValuesComparer(ChartControl chart) {
            this.chart = chart;
        }
        public int Compare(object x, object y) {
            return GetTotalByArg(x).CompareTo(GetTotalByArg(y));
        }

        double GetTotalByArg( object arg) {
            double total = 0;
            foreach(Series series in chart.Series)
                foreach(SeriesPoint point in series.Points)
                    if(Object.Equals(point.Argument, arg))
                        total += point.Values[0];
            return total;
        }
    }

}