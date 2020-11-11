using ServiceCallAssembly.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ForecastCostSheet
{
    public partial class PreviousRevision_Frm : Form
    {
        
        public PreviousRevision_Frm()
        {
            InitializeComponent();
        }

        public void OpenPrevCostSheet(string WorkOrderID, string ConnectionString, string NetworkID, string CurrentCostSheetID)
        {
            ForecastCostSheet myPrevCostSheet = null;
            SQLServer sqlServer = new SQLServer();

            try
            {

                myPrevCostSheet = new ForecastCostSheet(true,true);

                myPrevCostSheet.PopulateJobStatus(ConnectionString);

                myPrevCostSheet.BindCostSheet(CurrentCostSheetID, NetworkID, WorkOrderID, false, 1,0);

                this.Controls.Add(myPrevCostSheet);

                myPrevCostSheet.Dock = DockStyle.Fill;
                  
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
