<!-- default badges list -->
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T585190)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
[![](https://img.shields.io/badge/💬_Leave_Feedback-feecdd?style=flat-square)](#does-this-example-address-your-development-requirementsobjectives)
<!-- default badges end -->

# Chart for WinForms - Sort Stacked Bars by Total Values using QualitativeScaleComparer

The qualitative axis scale allows you to handle series whose arguments are string categories (for example, A, B, C, X, XII, etc.). Qualitative values do not have inherent numeric order, so the chart needs some way to sort or arrange them. 

To sort data with a custom comparison logic for values on a qualitative scale in WinForms Charts, use [QualitativeScaleComparer](https://docs.devexpress.com/CoreLibraries/DevExpress.XtraCharts.AxisBase.QualitativeScaleComparer). You can use it to:

- Sort categories in a custom order (not alphabetically).
- Group or prioritize values in a user-defined way.
- Ensure consistent order across multiple charts.

This example iterates through all chart points to calculate totals of the stacked bars and sort arguments based on this data:

```cs
class ArgumentByTotalComparer : IComparer {
    Dictionary<string, double> argTotalDict;

    public ArgumentByTotalComparer(Dictionary<string, double> argTotalDict) {
        this.argTotalDict = argTotalDict;
    }
    public int Compare(object x, object y) {
        return argTotalDict[(string)x].CompareTo(argTotalDict[(string)y]);
    }
}

public partial class Form1 : Form {
    // ...
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
}
```

```vb
Friend Class ArgumentByTotalComparer
    Implements IComparer

    Private argTotalDict As Dictionary(Of String, Double)

    Public Sub New(ByVal argTotalDict As Dictionary(Of String, Double))
        Me.argTotalDict = argTotalDict
    End Sub
    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare
        Return argTotalDict(DirectCast(x, String)).CompareTo(argTotalDict(DirectCast(y, String)))
    End Function
End Class

Partial Public Class Form1
    Inherits Form
    ' ...
    Private Sub ChartControl1_BoundDataChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim series As Series = chartControl1.Series(0)
        Dim argTotalDict = New Dictionary(Of String, Double)()
        For i As Integer = 0 To ArgumentNumber - 1
            Dim argument As String = series.Points(i).Argument
            Dim total As Double = GetTotalByArg(argument)
            argTotalDict.Add(argument, total)
        Next i
        Dim axisX As AxisX = CType(chartControl1.Diagram, XYDiagram).AxisX
        axisX.QualitativeScaleComparer = New ArgumentByTotalComparer(argTotalDict)
    End Sub
End Class
```

## Files to Review

* [Form1.cs](./CS/Form1.cs) (VB: [Form1.vb](./VB/Form1.vb))

## Documentation

* [QualitativeScaleComparer](https://docs.devexpress.com/CoreLibraries/DevExpress.XtraCharts.AxisBase.QualitativeScaleComparer)
* [Charts - Sorting Data](https://docs.devexpress.com/WindowsForms/6173/controls-and-libraries/chart-control/data-representation/sorting-data)

<!-- feedback -->
## Does this example address your development requirements/objectives?

[<img src="https://www.devexpress.com/support/examples/i/yes-button.svg"/>](https://www.devexpress.com/support/examples/survey.xml?utm_source=github&utm_campaign=winforms-charts-sort-stacked-bars-by-total-values-with-qualitativescalecomparer&~~~was_helpful=yes) [<img src="https://www.devexpress.com/support/examples/i/no-button.svg"/>](https://www.devexpress.com/support/examples/survey.xml?utm_source=github&utm_campaign=winforms-charts-sort-stacked-bars-by-total-values-with-qualitativescalecomparer&~~~was_helpful=no)

(you will be redirected to DevExpress.com to submit your response)
<!-- feedback end -->
