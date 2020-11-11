using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.Utils.Drawing;
using DevExpress.XtraGrid.Views.Grid.Drawing;
using System.ComponentModel.DataAnnotations;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.DataAccess.Native.ObjectBinding;
using DevExpress.XtraGrid.Views.Grid;
using ServiceCallDataElements;
using DevExpress.XtraPrinting.Export.Pdf;
using ForecastCostSheet;
using ServiceCallAssembly.Core;
using DevExpress.Xpo.Helpers;
using DevExpress.Utils.MVVM.Services;

namespace ForecastCostSheet
{
    public partial class ForecastCostSheet: UserControl
    {
        private bool bIsLoading;
        private ForecastSheet myForecast;
        private ServiceCallDataElements.SQLServer sqlServer;
        private string sCompanyID;
        private string sNetworkID;
        private string sConnectionString;
        public ServiceCallAssembly.Core.AddInCom AddInCom;

        public ForecastCostSheet(bool HideWorkOrderButton = false, bool IsPrevious = false, string CompanyID = "", string NetworkID = "")
        {                                   
            InitializeComponent();

            try
            {
                sCompanyID = CompanyID;
                sNetworkID = NetworkID;

                sqlServer = new ServiceCallDataElements.SQLServer();


                bIsLoading = true;                      
                                
                if (HideWorkOrderButton)
                {
                    WorkOrder_barButtonItem.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                }

                if (IsPrevious)
                {
                    PrevRevision_barButtonItem.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                }

                myForecast = new ForecastSheet();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                bIsLoading = false;
            }
        }

        private void IsAdjusted(string WorkOrderID)
        {
            string sCommandText = "";
            string sMessage = "";
            NotificationFrm notification = null;

            try
            {
                sCommandText = "SELECT Top 1 Adjusted FROM Reporting.forecastcostsheets WHERE WorkOrderID = '" + WorkOrderID + "' ORDER BY AddedOn DESC";

                if (sqlServer.GetBooleanValue(sCommandText))
                {
                    // Display notification if the work order has been adjusted
                    sMessage = "This work order has been adjusted! The Cost sheet will display work order data even if there are past cost sheets!";

                    notification = new NotificationFrm();

                    notification.Display(sMessage, "Work Order Adjusted", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);

                    if (notification.ShowDialog() == DialogResult.OK)
                    {
                        notification.Hide();
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (notification != null)
                {
                    notification.Dispose();
                }
            }
        }

        private bool IsLatest()
        {
            bool bResult = false;
            string sCommandText;
            string sLatestCostSheetID;

            try
            {                
                sCommandText = "SELECT TOP 1 CostSheetID FROM Reporting.ForecastCostSheets WHERE workorderid='" + myForecast.WorkOrderID + "' AND Adjusted = 0 ORDER BY AddedOn DESC";

                sLatestCostSheetID = sqlServer.GetStringValue(sCommandText);

                if (myForecast.CostSheetID == sLatestCostSheetID)
                {
                    bResult = true;
                }
                else
                {
                    // check if sheet exists if not allow editing
                    sCommandText = "SELECT TOP 1 CostSheetID FROM Reporting.ForecastCostSheets WHERE CostSheetID='" + myForecast.CostSheetID + "'";

                    sLatestCostSheetID = sqlServer.GetStringValue(sCommandText);

                    if (sLatestCostSheetID == "")
                    {
                        bResult = true;
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return bResult;
        }

        public void PopulateJobStatus(string ConnectionString)
        {
            ListBoxItem JobStatusItem;
            string sCommandText;
            DataTable dtJobStatus;            

            try
            {
                sConnectionString = ConnectionString;

                myForecast.ConnectionString = sConnectionString;

                sqlServer.ConnectionString = sConnectionString;

                sCommandText = "SELECT StatusId, Status FROM Reporting.ForecastCostSheetStatus ORDER BY Statusid";

                dtJobStatus = sqlServer.GetDataTable(sCommandText);

                if (dtJobStatus != null)
                {

                    foreach (DataRow jobStatus in dtJobStatus.Rows)
                    {
                        JobStatusItem = new ListBoxItem();

                        JobStatusItem.ID = (int)jobStatus["StatusID"];

                        JobStatusItem.Name = jobStatus["Status"].ToString();

                        JobStatus_ComboBox.Items.Add(JobStatusItem);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void BindCostSheet(string CostSheetID, string NetworkID, string WorkOrderID, bool IsNew = false, int PreviousCostSheet = 0, int Archive = 1)
        {

            try
            {

                bIsLoading = true;

                if (PreviousCostSheet == 0)
                {
                    IsAdjusted(WorkOrderID);
                }

                myForecast.LoadFromSqlServer(CostSheetID, NetworkID, WorkOrderID, PreviousCostSheet, Archive);

                ForecastDetail_GridControl.DataSource = DataTableFromList(myForecast.forecastCostSheetDetails, null, null);

                Parts_GridControl.DataSource = DataTableFromList(null, myForecast.Parts, null);

                Disciplines_GridControl.DataSource = DataTableFromList(null, null, myForecast.forecastCostSheetDisciplines);

                Revision_barStaticItem.Caption = "Revision: " + myForecast.AddedOn.ToString("MM/dd/yyyy") + " added by " + GetFullName(myForecast.AddedBy);

                QuoteWorkOrder_barListItem.Caption = "Q-" + myForecast.QuoteID + "\r\n" + "W-" + myForecast.WorkOrderID;

                ProjectManager_Label.Text = myForecast.ProjectManager;

                MonthsToComplete_Label.Text = String.Format("{0:N}", myForecast.MonthsToComplete);
                HoursPerMonth_Label.Text = String.Format("{0:N}", myForecast.HoursPerMonth);
                MenPerMonth_Label.Text = String.Format("{0:N}", myForecast.MenPerMonth);

                Target_DateEdit.EditValue = myForecast.TargetDate;
                EndDate_DateEdit.EditValue = myForecast.EndDate;

                LabourCost_Label.Text = String.Format("{0:C}", myForecast.TotalLabourCost);
                PartsCost_Label.Text = String.Format("{0:C}", myForecast.TotalPartsCost);
                TotalCost_Label.Text = String.Format("{0:C}", myForecast.TotalCost);
                Contribution_Label.Text = String.Format("{0:C}", myForecast.Contribution);
                TotalLabour_Label.Text = String.Format("{0:C}", myForecast.TotalLabour);
                TotalParts_Label.Text = String.Format("{0:C}", myForecast.TotalParts);
                TotalHours_Label.Text = myForecast.TotalHours.ToString();
                ContPerHour_Label.Text = String.Format("{0:C}", myForecast.ContributionPerHour);
                ContPercent_Label.Text = String.Format("{0:P}", myForecast.ContributionPercent);
                TotalForecast_Label.Text = String.Format("{0:C}", myForecast.TotalForecast);

                foreach (ListBoxItem item in JobStatus_ComboBox.Items)
                {
                    if (item.ID == myForecast.JobStatus)
                    {
                        JobStatus_ComboBox.SelectedItem = item;
                        break;
                    }
                }

                FormatForecastDetail();

                if (IsNew)
                {
                    myForecast.Save();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                bIsLoading = false;
            }

        }

        private string GetFullName(string NetworkID)
        {
            string sFullName = "";
            string sCommandText = "";

            try
            {
                sCommandText = "SELECT FullName FROM dbo.TeamMembers WHERE NetworkID='" + NetworkID + "'";

                sFullName = sqlServer.GetStringValue(sCommandText);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return sFullName;
        }

        private DataTable DataTableFromList(List<ForecastCostSheetDetail> Details = null, List<Part> Parts = null, List<ForecastCostSheetDiscipline> Disciplines = null)
        {
            PropertyDescriptorCollection properties;
            DataTable table = null;

            try
            {
                table = new DataTable();

                if (Details != null)
                {
                    properties = TypeDescriptor.GetProperties(typeof(ForecastCostSheetDetail));

                    foreach (PropertyDescriptor prop in properties)
                        table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

                    foreach (ForecastCostSheetDetail item in Details)
                    {
                        DataRow row = table.NewRow();
                        foreach (PropertyDescriptor prop in properties)
                            row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                        table.Rows.Add(row);
                    }
                }
                else if (Parts != null)
                {
                    properties = TypeDescriptor.GetProperties(typeof(Part));

                    foreach (PropertyDescriptor prop in properties)
                        table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

                    foreach (Part item in Parts)
                    {
                        DataRow row = table.NewRow();
                        foreach (PropertyDescriptor prop in properties)
                            row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                        table.Rows.Add(row);
                    }
                }
                else
                {
                    properties = TypeDescriptor.GetProperties(typeof(ForecastCostSheetDiscipline));

                    foreach (PropertyDescriptor prop in properties)
                        table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

                    foreach (ForecastCostSheetDiscipline item in Disciplines)
                    {
                        DataRow row = table.NewRow();
                        foreach (PropertyDescriptor prop in properties)
                            row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;

                        table.Rows.Add(row);
                    }
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return table;
        }

        private void FormatForecastDetail()
        {
            try
            {

                foreach (DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn column in advBandedGridView1.Columns)
                {
                        column.AppearanceHeader.BackColor = Color.Goldenrod;
                        column.AppearanceHeader.ForeColor = Color.Black;
                        column.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        column.AppearanceHeader.FontStyleDelta = FontStyle.Bold;

                        column.OptionsColumn.ReadOnly = true;
                        column.OptionsColumn.AllowEdit = false;
                }

                foreach (DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn column in advBandedGridView2.Columns)
                {
                    column.OptionsColumn.ReadOnly = true;
                    column.OptionsColumn.AllowEdit = false;


                    if(column.FieldName == "Quantity" || column.FieldName == "UnitCost")       
                    {

                        column.OptionsColumn.ReadOnly = false;
                        column.OptionsColumn.AllowEdit = true;

                    }

                }

                advBandedGridView1.Columns["CostSheetDetailID"].Visible = false;
                advBandedGridView1.Columns["CostSheetID"].Visible = false;
                advBandedGridView1.Columns["DisciplineID"].Visible = false;

                advBandedGridView1.Columns["Description"].Width = 375;
                advBandedGridView1.Columns["UnitPrice"].Width = 150;
                advBandedGridView1.Columns["UnitCost"].Width = 150;
                advBandedGridView1.Columns["Quantity"].Width = 150;
                advBandedGridView1.Columns["TotalCost"].Width = 150;
                advBandedGridView1.Columns["TotalPrice"].MinWidth = 150;

                advBandedGridView1.Columns["Quantity"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

                advBandedGridView1.Columns["UnitPrice"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                advBandedGridView1.Columns["UnitPrice"].DisplayFormat.FormatString = "c2";

                advBandedGridView1.Columns["UnitCost"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                advBandedGridView1.Columns["UnitCost"].DisplayFormat.FormatString = "c2";

                advBandedGridView1.Columns["Quantity"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                advBandedGridView1.Columns["Quantity"].DisplayFormat.FormatString = "n2";

                advBandedGridView1.Columns["TotalCost"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                advBandedGridView1.Columns["TotalCost"].DisplayFormat.FormatString = "c2";

                advBandedGridView1.Columns["TotalPrice"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                advBandedGridView1.Columns["TotalPrice"].DisplayFormat.FormatString = "c2";

                advBandedGridView1.Columns["ReasonForCall"].RowIndex = 1;

                advBandedGridView2.Columns["CostSheetDetailID"].Visible = false;
                advBandedGridView2.Columns["CostSheetID"].Visible = false;
                advBandedGridView2.Columns["DisciplineID"].Visible = false;
                advBandedGridView2.Columns["PartID"].Visible = false;
                advBandedGridView2.Columns["ApplyPST"].Visible = false;

                advBandedGridView2.Columns["Quantity"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

                advBandedGridView2.Columns["Description"].Width = 375;
                advBandedGridView2.Columns["UnitPrice"].Width = 150;
                advBandedGridView2.Columns["UnitCost"].Width = 150;
                advBandedGridView2.Columns["Quantity"].Width = 150;
                advBandedGridView2.Columns["TotalCost"].Width = 150;
                advBandedGridView2.Columns["TotalPrice"].MinWidth = 150;

                advBandedGridView2.Columns["UnitPrice"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                advBandedGridView2.Columns["UnitPrice"].DisplayFormat.FormatString = "c2";

                advBandedGridView2.Columns["UnitCost"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                advBandedGridView2.Columns["UnitCost"].DisplayFormat.FormatString = "c2";

                advBandedGridView2.Columns["Quantity"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                advBandedGridView2.Columns["Quantity"].DisplayFormat.FormatString = "n2";

                advBandedGridView2.Columns["TotalCost"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                advBandedGridView2.Columns["TotalCost"].DisplayFormat.FormatString = "c2";

                advBandedGridView2.Columns["TotalPrice"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                advBandedGridView2.Columns["TotalPrice"].DisplayFormat.FormatString = "c2";


                foreach (DevExpress.XtraGrid.Columns.GridColumn column in Disciplines_GridView.Columns)
                {
                    column.AppearanceHeader.BackColor = Color.Purple;
                    column.AppearanceHeader.FontStyleDelta = FontStyle.Bold;
                    column.AppearanceHeader.ForeColor = Color.Black;

                    column.OptionsColumn.ReadOnly = true;
                    column.OptionsColumn.AllowEdit = false;

                    switch (column.FieldName)
                    {
                        case "HourlyCost":
                        case "HourlyRate":
                            {
                                column.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                                column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

                                break;
                            }
                        case "Regular":
                        case "OT":
                        case "DT":
                        case "TotalHours":
                            {
                                column.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                                column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

                                if (column.FieldName != "TotalHours")
                                {
                                    column.OptionsColumn.ReadOnly = false;
                                    column.OptionsColumn.AllowEdit = true;
                                }

                                break;
                            }

                    }

                }

                Disciplines_GridView.Columns["CostSheetDisciplineID"].Visible = false;
                Disciplines_GridView.Columns["CostSheetID"].Visible = false;
                Disciplines_GridView.Columns["DisciplineID"].Visible = false;

                Disciplines_GridView.Columns["Discipline"].Width = 250;

                Disciplines_GridView.Columns["HourlyCost"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                Disciplines_GridView.Columns["HourlyCost"].DisplayFormat.FormatString = "c2";

                Disciplines_GridView.Columns["HourlyRate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                Disciplines_GridView.Columns["HourlyRate"].DisplayFormat.FormatString = "c2";

                if (!IsLatest())
                {
                    Target_DateEdit.ReadOnly = true;
                    EndDate_DateEdit.ReadOnly = true;
                    JobStatus_ComboBox.Enabled = false;
                    Disciplines_GridView.OptionsBehavior.ReadOnly = true;
                    advBandedGridView1.OptionsBehavior.ReadOnly = true;
                    advBandedGridView2.OptionsBehavior.ReadOnly = true;
                   
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
             
        private void EditMonthDetails()
        {
            try
            {

                if (bIsLoading) return;

                myForecast.TargetDate = Convert.ToDateTime(Target_DateEdit.EditValue);
                myForecast.EndDate = Convert.ToDateTime( EndDate_DateEdit.EditValue);

                myForecast.CalculateMonths();

                MonthsToComplete_Label.Text = String.Format("{0:N}", myForecast.MonthsToComplete);
                HoursPerMonth_Label.Text = String.Format("{0:N}",myForecast.HoursPerMonth);
                MenPerMonth_Label.Text = String.Format("{0:N}", myForecast.MenPerMonth);

                myForecast.Save();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void Target_DateEdit_EditValueChanged(object sender, EventArgs e)
        {
            EditMonthDetails();
        }

        private void EndDate_DateEdit_EditValueChanged(object sender, EventArgs e)
        {
            EditMonthDetails();
        }

        private void advBandedGridView2_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            string sCostSheetDetailID;

            try
            {
                BandedGridView view = sender as BandedGridView;
                if (view == null) return;

                if (e.Column.FieldName == "Quantity" || e.Column.FieldName == "UnitCost")
                {
                    sCostSheetDetailID = advBandedGridView2.GetRowCellValue(e.RowHandle, advBandedGridView2.Columns["CostSheetDetailID"]).ToString();

                    foreach (Part part in myForecast.Parts)
                    {
                        if (part.CostSheetDetailID == sCostSheetDetailID)
                        {
                            if (e.Column.FieldName == "Quantity")
                            { 
                                part.Quantity = Convert.ToDecimal(e.Value);
                            }
                            else
                            {
                                part.UnitCost = Convert.ToDecimal(e.Value);
                            }

                            part.Calculate();

                            break;
                        }
                    }

                    myForecast.Save();

                                                            
                    BindCostSheet(myForecast.CostSheetID, sNetworkID ,myForecast.WorkOrderID,false,0,0);
                }    
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void Disciplines_GridView_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            string sCostSheetDisciplineID;
            decimal dOrigTotalHours;
            decimal dAdjustedTotal;

            try
            {
                GridView view = sender as GridView;
                if (view == null) return;

                if (e.Column.FieldName == "Regular" || e.Column.FieldName == "OT" || e.Column.FieldName == "DT")
                {
                    sCostSheetDisciplineID = Disciplines_GridView.GetRowCellValue(e.RowHandle, Disciplines_GridView.Columns["CostSheetDisciplineID"]).ToString();

                    foreach (ForecastCostSheetDiscipline discipline in myForecast.forecastCostSheetDisciplines)
                    {
                        if (discipline.CostSheetDisciplineID == sCostSheetDisciplineID)
                        {
                            dOrigTotalHours = discipline.Regular + discipline.OT + discipline.DT;

                            if (e.Column.FieldName == "Regular")
                            {
                                discipline.Regular = Convert.ToDecimal(e.Value);
                            }
                            else if (e.Column.FieldName == "OT")
                            {
                                discipline.OT = Convert.ToDecimal(e.Value);
                            }
                            else
                            {
                                discipline.DT = Convert.ToDecimal(e.Value);
                            }
                            
                            foreach(ForecastCostSheetDetail detail in myForecast.forecastCostSheetDetails)
                            {
                                if (detail.DisciplineID == discipline.DisciplineID)
                                {
                                    dAdjustedTotal = detail.UnitPrice - (dOrigTotalHours * discipline.HourlyRate);

                                    detail.UnitPrice = ((discipline.Regular + discipline.OT + discipline.DT) * discipline.HourlyRate) + dAdjustedTotal;

                                    dAdjustedTotal = detail.UnitCost - (dOrigTotalHours * discipline.HourlyCost);

                                    detail.UnitCost = (discipline.Regular + discipline.OT + discipline.DT) * discipline.HourlyCost + dAdjustedTotal;

                                    detail.Calculate();
                                }
                            }

                            discipline.Calculate();

                            break;
                        }
                    }

                    myForecast.Save();

                    BindCostSheet(myForecast.CostSheetID, myForecast.AddedBy,myForecast.WorkOrderID,false,0,0);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void JobStatus_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBoxItem SelectedStatus;

            try
            {

                if (bIsLoading) return;

                SelectedStatus = (ListBoxItem)JobStatus_ComboBox.SelectedItem;

                myForecast.JobStatus = SelectedStatus.ID;

                myForecast.Save();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        private void WorkOrder_barButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                 
                AddInCom.ShowWorkOrderForm(myForecast.WorkOrderID);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        private void PrevRevision_barButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PreviousRevision_Frm frm = new PreviousRevision_Frm();
            ServiceCallDataElements.SQLServer sqlServer = null;
            string sCommandText = "";
            string sPrevCostSheetID = "";
            string sMessage = "";
            NotificationFrm notification = null;

            try
            {
                sCommandText = "SELECT TOP 1 CostSheetID from Reporting.ForecastCostsheets WHERE WorkorderID = '" + myForecast.WorkOrderID + "' AND AddedOn < '" + myForecast.AddedOn + "' Order By Addedon DESC";

                sqlServer = new ServiceCallDataElements.SQLServer();

                sqlServer.ConnectionString = sConnectionString;

                sPrevCostSheetID = sqlServer.GetStringValue(sCommandText);

                if (sPrevCostSheetID != "")
                {
                    frm.OpenPrevCostSheet(myForecast.WorkOrderID, myForecast.ConnectionString, myForecast.AddedBy, sPrevCostSheetID);
                    frm.Show();
                }
                else
                {
                    // Display Notification there are no previous cost sheets to show!
                    sMessage = "There are no previous cost sheets to show!";
                                         
                    notification = new NotificationFrm();

                    notification.Display(sMessage, "Work Order Adjusted", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);

                    if (notification.ShowDialog() == DialogResult.OK)
                    {
                        notification.Hide();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (notification != null)
                {
                    notification.Dispose();
                }
            }
        }

        private void Print_barButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PrintPreviewFrm printPreview = new PrintPreviewFrm(myForecast, sCompanyID, sConnectionString);

        }
    }

    public class ListBoxItem
    {
        public int ID;
        public string Name;

        public override string ToString()
        {
            return Name;
        }
    }

}
