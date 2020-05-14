using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.XtraCharts;

namespace SortStackedBarsByTotalValue {
    public partial class Form1 : Form {
        const int SeriesNumber = 3;
        const int ArgumentNumber = 10;

        public Form1() {
            InitializeComponent();
            chartControl1.DataSource = CreateDataSource();
            chartControl1.SeriesDataMember = "Series";
            SeriesTemplate seriesTemplate = chartControl1.SeriesTemplate;
            seriesTemplate.ArgumentDataMember = "Argument";
            seriesTemplate.ValueDataMembers.AddRange("Value");
            seriesTemplate.View = new StackedBarSeriesView();
            chartControl1.BoundDataChanged += ChartControl1_BoundDataChanged;
        }

        List<DataPoint> CreateDataSource() {
            var dataSource = new List<DataPoint>();
            Random random = new Random(1);
            for (int seriesIndex = 0; seriesIndex < SeriesNumber; seriesIndex++) {
                for (int argumentIndex = 0; argumentIndex < ArgumentNumber; argumentIndex++) {
                    DataPoint dataPoint = new DataPoint() {
                        Series = "Series " + seriesIndex,
                        Argument = "Argument " + argumentIndex,
                        Value = random.Next(1, 10)
                    };
                    dataSource.Add(dataPoint);
                }
            }
            return dataSource;
        }
        void ChartControl1_BoundDataChanged(object sender, EventArgs e) {
            Series series = chartControl1.Series[0];
            var argTotalDict = new Dictionary<string, double>();
            for (int i = 0; i < ArgumentNumber; i++) {
                string argument = series.Points[i].Argument;
                double total = GetTotalByArg(argument);
                argTotalDict.Add(argument, total);
            }
            AxisX axisX = ((XYDiagram)chartControl1.Diagram).AxisX;
            axisX.QualitativeScaleComparer = new ArgumentByTotalComparer(argTotalDict);
        }
        double GetTotalByArg(object arg) {
            double total = 0;
            foreach (Series series in chartControl1.Series)
                foreach (SeriesPoint point in series.Points)
                    if (Equals(point.Argument, arg))
                        total += point.Values[0];
            return total;
        }
    }

    public class DataPoint {
        public string Series { get; set; }
        public string Argument { get; set; }
        public double Value { get; set; }
    }

    class ArgumentByTotalComparer : IComparer {
        Dictionary<string, double> argTotalDict;

        public ArgumentByTotalComparer(Dictionary<string, double> argTotalDict) {
            this.argTotalDict = argTotalDict;
        }
        public int Compare(object x, object y) {
            return argTotalDict[(string)x].CompareTo(argTotalDict[(string)y]);
        }
    }
}
