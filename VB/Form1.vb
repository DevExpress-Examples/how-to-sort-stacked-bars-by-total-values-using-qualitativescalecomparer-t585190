Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Linq
Imports System.Windows.Forms
Imports DevExpress.XtraCharts

Imports System.Collections

Namespace ChartStackedSorting
	Partial Public Class Form1
		Inherits Form

		Public Sub New()
			InitializeComponent()
		End Sub

		Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
			chartControl1.Series.Clear()

			Dim s1, s2, s3 As Series

			s1 = New Series("Serie1", ViewType.StackedBar)
			s2 = New Series("Serie2", ViewType.StackedBar)
			s3 = New Series("Serie3", ViewType.StackedBar)
			s1.ArgumentScaleType = ScaleType.Qualitative
			s2.ArgumentScaleType = ScaleType.Qualitative
			s3.ArgumentScaleType = ScaleType.Qualitative


			chartControl1.Series.AddRange(New Series() { s1, s2, s3 })
			Dim axisX = CType(chartControl1.Diagram, XYDiagram).AxisX
			axisX.QualitativeScaleComparer = New TotalScaleValuesComparer(chartControl1)
			axisX.Reverse = True

			Dim r As New Random()
			For i As Integer = 0 To 9
				s1.Points.Add(New SeriesPoint(i, Math.Round(r.NextDouble() * 100)))
				s2.Points.Add(New SeriesPoint(i, Math.Round(r.NextDouble() * 100)))
				s3.Points.Add(New SeriesPoint(i, Math.Round(r.NextDouble() * 100)))
			Next i
		End Sub
	End Class

	Public Class TotalScaleValuesComparer
		Implements IComparer

		Private chart As ChartControl
		Public Sub New(ByVal chart As ChartControl)
			Me.chart = chart
		End Sub
		Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare
			Return GetTotalByArg(x).CompareTo(GetTotalByArg(y))
		End Function

		Private Function GetTotalByArg(ByVal arg As Object) As Double
			Dim total As Double = 0
			For Each series As Series In chart.Series
				For Each point As SeriesPoint In series.Points
					If Object.Equals(point.Argument, arg) Then
						total += point.Values(0)
					End If
				Next point
			Next series
			Return total
		End Function
	End Class

End Namespace