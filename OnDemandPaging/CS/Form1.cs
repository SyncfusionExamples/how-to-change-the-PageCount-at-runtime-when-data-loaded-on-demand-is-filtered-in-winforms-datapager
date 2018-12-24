#region Copyright Syncfusion Inc. 2001-2018.
// Copyright Syncfusion Inc. 2001-2018. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Syncfusion.WinForms.Core.Utils;
using Syncfusion.WinForms.Controls;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.WinForms.DataGrid.Styles;
using Syncfusion.WinForms.DataPager.Events;
using System.Data.SqlClient;
using System.Data;
//using System.Data.SqlServerCe;
using System.IO;
using OnDemandPaging.Model;
using Syncfusion.Data;
using System.Collections.Specialized;
using System.ComponentModel;
using Syncfusion.Data.Extensions;

namespace OnDemandPaging
{
    public partial class Form1 : Form
    {
        #region Fields     

        Northwind northWind;
        List<Orders> source = new List<Orders>();
        /// <summary>
        /// Specifies the busy indicator for displaying the on loading.
        /// </summary>
        private BusyIndicator busyIndicator = new BusyIndicator();

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the Form.
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            this.Load += OnLoad;
            sfDataPager1.AllowOnDemandPaging = true;
            sfDataPager1.PageCount = 5;
            sfDataPager1.PageSize = 50;
            string connectionString = string.Format(@"Data Source = {0}", ("Northwind.sdf"));
            northWind = new Northwind(connectionString);
            source = northWind.Orders.ToList();
            this.sfDataPager1.OnDemandLoading += OnDemandLoading;            
        }



        #endregion

        #region Methods

        /// <summary>
        /// Occurs when the form load.
        /// </summary>
        /// <param name="sender">The object of the sender.</param>
        /// <param name="e">An EventArgs that contains event data.</param>
        private void OnLoad(object sender, System.EventArgs e)
        {
            sfDataGrid.DataSource = sfDataPager1.PagedSource;
        }

        private List<Orders> ApplyFilter(Northwind NorthwindSource)
        {

            // records are filtered based on CustomerID column
            return NorthwindSource.Orders.Where(item => item.CustomerID.Contains(filterTextBox.Text)).ToList();
        }

        /// <summary>
        /// Occurs when the new page is loaded in the SfDataPager.
        /// </summary>
        /// <param name="sender">The object of the sender.</param>
        /// <param name="e">An OnDemandLoadingEventArgs that contains event data.</param>
        private void OnDemandLoading(object sender, OnDemandLoadingEventArgs e)
        {
            //Show busy indicator while loading the data.
            if (sfDataGrid.TableControl.IsHandleCreated)
            {
                busyIndicator.Show(this.sfDataGrid.TableControl);
                Thread.Sleep(1000);
            }

            sfDataPager1.LoadDynamicData(e.StartRowIndex, source.Skip(e.StartRowIndex).Take(e.PageSize));
            busyIndicator.Hide();
        }

        #endregion

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
            this.sfDataPager1.PagedSource.ResetCache();
            this.sfDataPager1.MoveToPage(0);
            this.sfDataPager1.Refresh();
        }
    }
}
