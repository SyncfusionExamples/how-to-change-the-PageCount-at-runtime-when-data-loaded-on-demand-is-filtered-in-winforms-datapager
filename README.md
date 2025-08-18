# How to change the PageCount at runtime when data loaded on demand is filtered in winforms datapager

This example illustrates how to change the `PageCount` at runtime when data loaded on demand is filtered in [WinForms DataPager](https://help.syncfusion.com/cr/windowsforms/Syncfusion.WinForms.DataPager.SfDataPager.html).

You can change the [SfDataPager.PageCount](https://help.syncfusion.com/cr/windowsforms/Syncfusion.WinForms.DataPager.SfDataPager.html#Syncfusion_WinForms_DataPager_SfDataPager_PageCount) at runtime based on the records count in on-demand paging. Here, `PageCount` are modified by filtering the records in run time.

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
