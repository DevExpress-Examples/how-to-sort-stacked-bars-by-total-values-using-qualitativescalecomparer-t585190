Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Windows.Forms
Imports DevExpress.XtraCharts

Namespace SortStackedBarsByTotalValue
	Partial Public Class Form1
		Inherits Form

		Private Const SeriesNumber As Integer = 3
		Private Const ArgumentNumber As Integer = 10

		Public Sub New()
			InitializeComponent()
			chartControl1.DataSource = CreateDataSource()
			chartControl1.SeriesDataMember = "Series"
			Dim seriesTemplate As SeriesTemplate = chartControl1.SeriesTemplate
			seriesTemplate.ArgumentDataMember = "Argument"
			seriesTemplate.ValueDataMembers.AddRange("Value")
			seriesTemplate.View = New StackedBarSeriesView()
			AddHandler chartControl1.BoundDataChanged, AddressOf ChartControl1_BoundDataChanged
		End Sub

		Private Function CreateDataSource() As List(Of DataPoint)
			Dim dataSource = New List(Of DataPoint)()
			Dim random As New Random(1)
			For seriesIndex As Integer = 0 To SeriesNumber - 1
				For argumentIndex As Integer = 0 To ArgumentNumber - 1
					Dim dataPoint As New DataPoint() With {
						.Series = "Series " & seriesIndex,
						.Argument = "Argument " & argumentIndex,
						.Value = random.Next(1, 10)
					}
					dataSource.Add(dataPoint)
				Next argumentIndex
			Next seriesIndex
			Return dataSource
		End Function
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
		Private Function GetTotalByArg(ByVal arg As Object) As Double
			Dim total As Double = 0
			For Each series As Series In chartControl1.Series
				For Each point As SeriesPoint In series.Points
					If Equals(point.Argument, arg) Then
						total += point.Values(0)
					End If
				Next point
			Next series
			Return total
		End Function
	End Class

	Public Class DataPoint
		Public Property Series() As String
		Public Property Argument() As String
		Public Property Value() As Double
	End Class

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
End Namespace
