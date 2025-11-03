# How to Change the PageCount at Runtime When Data Loaded on Demand is Filtered in WinForms DataPager?

This example illustrates how to change the **PageCount** at runtime when data loaded on demand is filtered in [WinForms DataPager](https://help.syncfusion.com/cr/windowsforms/Syncfusion.WinForms.DataPager.SfDataPager.html).

You can change the [SfDataPager.PageCount](https://help.syncfusion.com/cr/windowsforms/Syncfusion.WinForms.DataPager.SfDataPager.html#Syncfusion_WinForms_DataPager_SfDataPager_PageCount) at runtime based on the records count in on-demand paging. Here, **PageCount** are modified by filtering the records in run time.

#### C#

```c#
public partial class Form1 : Form
{
    Northwind northWind;
    List<Orders> source = new List<Orders>();

    public Form1()
    {
        InitializeComponent();
        string connectionString = string.Format(@"Data Source = {0}", ("Northwind.sdf"));
        //northWind dataProvider connectivity.
        northWind = new Northwind(connectionString);
        source = northWind.Orders.ToList();
        this.sfDataPager1.OnDemandLoading += OnDemandLoading;
    }

    private void OnDemandLoading(object sender, OnDemandLoadingEventArgs e)
    { 
        sfDataPager1.LoadDynamicData(e.StartRowIndex, source.Skip(e.StartRowIndex).Take(e.PageSize));
    }

    private List<Orders> ApplyFilter(Northwind NorthwindSource)
    {
        // records are filtered based on CustomerID column
        return NorthwindSource.Orders.Where(item => item.CustomerID.Contains(filterTextBox.Text)).ToList();
    }

    private void FilterBtn_Click(object sender, System.EventArgs e)
    {
        source = ApplyFilter(northWind);
        //page count resets based on filtered records.
        if (source.Count() < sfDataPager1.PageSize)
            this.sfDataPager1.PageCount = 1;
        else
        {
            var count = source.Count() / sfDataPager1.PageSize;
            if (source.Count() % sfDataPager1.PageSize == 0)
                this.sfDataPager1.PageCount = count;
            else
                this.sfDataPager1.PageCount = count + 1;
        }
        this.sfDataPager1.MoveToPage(0);
        this.sfDataPager1.Refresh();
    }
```

#### VB

``` vb
Partial Public Class Form1
	Inherits Form
Private northWind As Northwind
Private source As New List(Of Orders)()

Public Sub New()
	InitializeComponent()
	Dim connectionString As String = String.Format("Data Source = {0}", ("Northwind.sdf"))
	'northWind dataProvider connectivity.
	northWind = New Northwind(connectionString)
	source = northWind.Orders.ToList()
	AddHandler Me.sfDataPager1.OnDemandLoading, AddressOf OnDemandLoading
End Sub

Private Sub OnDemandLoading(ByVal sender As Object, ByVal e As OnDemandLoadingEventArgs)
	sfDataPager1.LoadDynamicData(e.StartRowIndex, source.Skip(e.StartRowIndex).Take(e.PageSize))
End Sub

Private Function ApplyFilter(ByVal NorthwindSource As Northwind) As List(Of Orders)
	'records are filtered based on CustomerID column
	Return NorthwindSource.Orders.Where(Function(item) item.CustomerID.Contains(filterTextBox.Text)).ToList()
End Function

Private Sub FilterBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs)
	source = ApplyFilter(northWind)
	'page count resets based on filtered records.
	If source.Count() < sfDataPager1.PageSize Then
		Me.sfDataPager1.PageCount = 1
	Else
		Dim count = source.Count() / sfDataPager1.PageSize
		If source.Count() Mod sfDataPager1.PageSize = 0 Then
			Me.sfDataPager1.PageCount = count
		Else
			Me.sfDataPager1.PageCount = count + 1
		End If
	End If
	Me.sfDataPager1.MoveToPage(0)
	Me.sfDataPager1.Refresh()
End Sub
```

Here, records are filtered based on the textbox text in clicking event of Filter button. Initially PageCount is 5 and it is changed as 1 once the records are filtered.
