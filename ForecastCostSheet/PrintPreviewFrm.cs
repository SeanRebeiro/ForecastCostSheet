using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.LookAndFeel;
using DevExpress.XtraReports.UI;

namespace ForecastCostSheet
{
    public partial class PrintPreviewFrm : Form
    {
        public PrintPreviewFrm(ForecastSheet CostSheet, string CompanyID, string ConnectionString)
        {
            InitializeComponent();

            ReportPrintTool printTool = null;

            try
            {
                printTool = new ReportPrintTool(new CostSheetReport(CostSheet, CompanyID, ConnectionString));

                // Invoke the Print Preview form  
                // and load the report document into it. 
                printTool.ShowPreview();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
