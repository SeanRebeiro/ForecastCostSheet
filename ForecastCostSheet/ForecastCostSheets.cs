using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ServiceCallDataElements;
using DevExpress.Utils.Zip;

namespace ForecastCostSheet
{
    public partial class ForecastCostSheets : UserControl
    {
        public string ConnectionString;
        public string NetworkID;
        public string WorkOrderID;
        public bool IsFromWorkOrder;

        private string sCompanyID;

        public ForecastCostSheets(string CompanyID)
        {
            InitializeComponent();

            sCompanyID = CompanyID;

            //Testing only!
            //ConnectionString = "Data Source=sqltopro1;Initial Catalog=ServiceCall;Integrated Security=True";
            //NetworkID = "sean.rebeiro";

            //WorkOrderID = "20180725-055";
         //   WorkOrderID = "20190801-861";
         //   WorkOrderID = "19000101-207";

          //  BindCostSheets();

            //end testing code


        }
        public void BindCostSheets()
        {
            SQLServer sqlServer = new SQLServer();
            DataTable dtCostSheets = null;
            string sCommandText = "";

            try
            {
                CostSheets_GridControl.DataSource = null;

                sCommandText = "SELECT CostSheetID, Addedon as 'Revision Date', " +
                    "(SELECT FullName FROM dbo.TeamMembers WHERE NetworkID = FCS.AddedBy) As AddedBy" + 
                    " FROM Reporting.ForecastCostSheets FCS WHERE WorkOrderID = '" + WorkOrderID + "' ORDER BY AddedOn DESC";

                sqlServer.ConnectionString = ConnectionString;
                dtCostSheets = sqlServer.GetDataTable(sCommandText);

                CostSheets_GridControl.DataSource = dtCostSheets;

                CostSheets_GridView.Columns["CostSheetID"].Visible = false;


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void NewCostSheet()
        { 
            string CostSheetID;

            try
            {
                CostSheetID = Guid.NewGuid().ToString().ToUpper();

                OpenCostSheet(CostSheetID, true);

                BindCostSheets();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void OpenCostSheet(string CostSheetID,bool IsNew = false)
        {
            ForecastCostSheet costSheet;
            int iArchive = 0;

            try
            {
                if (!IsNew) iArchive = 1; 

                Content_Panel.Controls.Clear();

                costSheet = new ForecastCostSheet(true, false,sCompanyID, NetworkID);

                costSheet.PopulateJobStatus(ConnectionString);

                costSheet.BindCostSheet(CostSheetID, NetworkID, WorkOrderID, IsNew,0,iArchive);

                Content_Panel.Controls.Add(costSheet);

                costSheet.Dock = DockStyle.Fill; 

                costSheet.BringToFront();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void CostSheets_GridView_DoubleClick(object sender, EventArgs e)
        {
            string CostSheetID;
            int iRowIndex;

            iRowIndex = (int)CostSheets_GridView.GetSelectedRows()[0];

            CostSheetID = CostSheets_GridView.GetRowCellValue(iRowIndex, CostSheets_GridView.Columns["CostSheetID"]).ToString();
            

            OpenCostSheet(CostSheetID);
        }

        private void New_barButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            NewCostSheet();
        }
    }
}
