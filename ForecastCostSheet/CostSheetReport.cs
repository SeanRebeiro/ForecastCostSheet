using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Security.Cryptography.X509Certificates;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.Design;
using DevExpress.Xpo.DB;
using Microsoft.VisualBasic;

namespace ForecastCostSheet
{
    public partial class CostSheetReport : DevExpress.XtraReports.UI.XtraReport
    {
        private ForecastSheet myCostSheet;
        private string sCompanyID;
        private string sConnectionString;

        public CostSheetReport(ForecastSheet CostSheet, string CompanyID, string ConnectionString)
        {
            InitializeComponent();

            sCompanyID = CompanyID;
            sConnectionString = ConnectionString;
            myCostSheet = CostSheet;

            LoadHeader();

            BindReport();
        }

        private void LoadHeader()
        {
            string sImageFile = "";
            bool bExtractLogoFile = false;
            FileLib fileLibrary = new FileLib();

            try
            {
                Logo_xrPictureBox.Image = null;
            }
            catch{}
        
            try
            {
                sImageFile = fileLibrary.CompanyLogoImageFileName(sCompanyID, FileLib.CompanyImageTypes.InvoiceLogoImage, sConnectionString);

                if (!System.IO.File.Exists(sImageFile))
                {
                    bExtractLogoFile = true;
                }
                else
                {
                    if (fileLibrary.FileDaysAge(sImageFile) > 0)
                    {
                        bExtractLogoFile = true;
                    }
                }

                if (bExtractLogoFile)
                {
                    fileLibrary.ExtractCompanyImage(sCompanyID, ref sImageFile, FileLib.CompanyImageTypes.InvoiceLogoImage, sConnectionString);
                }

                Logo_xrPictureBox.Image = new System.Drawing.Bitmap(sImageFile);
                Logo_xrPictureBox.Visible = true;
            
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        
        }


        private void BindReport()
        {
            XRTableRow row;
            XRTableCell cell;

            try
            {

                AddedOn_xrLabel.Text = myCostSheet.AddedOn.ToString("MMMM dd, yyyy");
                TargetDate_xrLabel.Text = myCostSheet.TargetDate.ToString("MMMM dd, yyyy");
                EndDate_xrLabel.Text = myCostSheet.EndDate.ToString("MMMM dd, yyyy");
                JobStatus_xrLabel.Text = myCostSheet.JobStatusDescription();
                ProjectManager_xrLabel.Text = myCostSheet.ProjectManager;
                Quote_xrLabel.Text = myCostSheet.QuoteID;
                WorkOrderID_xrLabel.Text = myCostSheet.WorkOrderID;
                MonthsToComplete_xrLabel.Text = myCostSheet.MonthsToComplete.ToString();
                MenPerMonth_xrLabel.Text = myCostSheet.MenPerMonth.ToString("N2");
                HoursPerMonth_xrLabel.Text = myCostSheet.HoursPerMonth.ToString("N2");


                foreach (ForecastCostSheetDetail detail in myCostSheet.forecastCostSheetDetails)
                {
                    row = new XRTableRow();

                    cell = SetNewXRTableCell(detail.Description,TextAlignment.TopLeft, 260);
                    row.Cells.Add(cell);

                    cell = SetNewXRTableCell(detail.UnitCost.ToString("C2"), TextAlignment.TopRight, 90);
                    row.Cells.Add(cell);

                    cell = SetNewXRTableCell(detail.TotalCost.ToString("C2"),  TextAlignment.TopRight,90);
                    row.Cells.Add(cell);

                    cell = SetNewXRTableCell(detail.Quantity.ToString("N2"), TextAlignment.TopCenter,110);
                    row.Cells.Add(cell);

                    cell = SetNewXRTableCell(detail.UnitPrice.ToString("C2"),  TextAlignment.TopRight,100);
                    row.Cells.Add(cell);

                    cell = SetNewXRTableCell(detail.TotalPrice.ToString("C2"),  TextAlignment.TopRight,100);
                    row.Cells.Add(cell);

                    Disciplines_xrTable.Rows.Add(row);

                    row = new XRTableRow();

                    row.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 10, 10, 96F);

                    cell = SetNewXRTableCell(detail.ReasonForCall, TextAlignment.TopLeft,750);
                    row.Cells.Add(cell);

                    Disciplines_xrTable.Rows.Add(row);

                }

                foreach (Part part in myCostSheet.Parts)
                {
                    row = new XRTableRow();

                    cell = SetNewXRTableCell(part.Description, TextAlignment.TopLeft, 260);
                    row.Cells.Add(cell);

                    cell = SetNewXRTableCell(part.UnitCost.ToString("C2"), TextAlignment.TopRight, 90);
                    row.Cells.Add(cell);

                    cell = SetNewXRTableCell(part.TotalCost.ToString("C2"), TextAlignment.TopRight,90);
                    row.Cells.Add(cell);

                    cell = SetNewXRTableCell(part.Quantity.ToString("N2"), TextAlignment.TopCenter,110);
                    row.Cells.Add(cell);

                    cell = SetNewXRTableCell(part.UnitPrice.ToString("C2"), TextAlignment.TopRight, 100);
                    row.Cells.Add(cell);

                    cell = SetNewXRTableCell(part.TotalPrice.ToString("C2"), TextAlignment.TopRight,100);
                    row.Cells.Add(cell);

                    Parts_xrTable.Rows.Add(row);

                }

                foreach (ForecastCostSheetDiscipline discipline in myCostSheet.forecastCostSheetDisciplines)
                {
                    row = new XRTableRow();

                    cell = SetNewXRTableCell(discipline.QuoteID, TextAlignment.TopLeft,80 );
                    row.Cells.Add(cell);

                    cell = SetNewXRTableCell(discipline.Discipline,  TextAlignment.TopLeft,180);
                    row.Cells.Add(cell);

                    cell = SetNewXRTableCell(discipline.Regular.ToString("N2"),TextAlignment.TopCenter,75);
                    row.Cells.Add(cell);

                    cell = SetNewXRTableCell(discipline.OT.ToString("N2"), TextAlignment.TopCenter,75);
                    row.Cells.Add(cell);

                    cell = SetNewXRTableCell(discipline.DT.ToString("N2"), TextAlignment.TopCenter,75);
                    row.Cells.Add(cell);

                    cell = SetNewXRTableCell(discipline.TotalHours.ToString("N2"), TextAlignment.TopCenter,85);
                    row.Cells.Add(cell);

                    cell = SetNewXRTableCell(discipline.HourlyCost.ToString("C2"),  TextAlignment.TopRight,90);
                    row.Cells.Add(cell);

                    cell = SetNewXRTableCell(discipline.HourlyRate.ToString("C2"),  TextAlignment.TopRight,90);
                    row.Cells.Add(cell);

                    Hours_xrTable.Rows.Add(row);

                }

                LabourCosts_xrLabel.Text = myCostSheet.TotalLabourCost.ToString("C2");
                PartsCosts_xrLabel.Text = myCostSheet.TotalPartsCost.ToString("C2");
                TotalCost_xrLabel.Text = (myCostSheet.TotalPartsCost + myCostSheet.TotalLabourCost).ToString("C2");
                Contribution_xrLabel.Text = myCostSheet.Contribution.ToString("C2");
                TotalLabour_xrLabel.Text = myCostSheet.TotalLabour.ToString("C2");
                TotalParts_xrLabel.Text = myCostSheet.TotalParts.ToString("C2");
                TotalHours_xrLabel.Text = myCostSheet.TotalHours.ToString("C2");
                ContPerHour_xrLabel.Text = myCostSheet.ContributionPerHour.ToString("C2");
                PercentCont_xrLabel.Text = myCostSheet.ContributionPercent.ToString("P2");

                TotalForecast_xrLabel.Text = myCostSheet.TotalForecast.ToString("C2");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (myCostSheet != null)
                {
                    myCostSheet = null;
                }
            }
            
        }
        private XRTableCell SetNewXRTableCell(string CellText, TextAlignment TextAlign, double CellWidth)
        {
            XRTableCell TargetCell = new XRTableCell();

            TargetCell.Text = CellText.ToString();

            TargetCell.TextAlignment = TextAlign;
            TargetCell.WidthF = (float)CellWidth;

            return TargetCell;
        }
    }
}
