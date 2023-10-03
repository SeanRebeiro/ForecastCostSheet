using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForecastCostSheet
{
   public class DateChangedDelegateEventArgs : EventArgs
    {
        public DateTime StartDate;
        public DateTime EndDate;
    }
}
