using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Data.Mask;
using DevExpress.CodeParser;
using System.Data;
using ServiceCallDataElements;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace ForecastCostSheet
{
	public class ForecastSheet
	{
		public string ConnectionString;

		public string CostSheetID;
		public string ProjectManager;
		public string AddedBy;
		public string QuoteID;
		public string WorkOrderID;
		public DateTime AddedOn;
		public DateTime TargetDate;
		public DateTime EndDate;
		public int JobStatus;
		public decimal ActualHours;
		public List<ForecastCostSheetDetail> forecastCostSheetDetails;
		public List<ForecastCostSheetDiscipline> forecastCostSheetDisciplines;
		public List<Part> Parts;

		public decimal TotalLabourCost;
		public decimal TotalPartsCost;
		public decimal TotalCost;
		public decimal TotalLabour;
		public decimal TotalParts;
		public decimal TotalHours;
		public decimal TotalForecast;
		public decimal Contribution;
		public decimal ContributionPerHour;
		public decimal ContributionPercent;
		public decimal MonthsToComplete;
		public decimal HoursPerMonth;
		public decimal MenPerMonth;

		public bool Adjusted;

		public bool LoadFromSqlServer(string TargetCostSheetID, string NetworkID, string TargetWorkOrderID, int PreviousCostSheet, int Archive)
		{
			CostSheetID = TargetCostSheetID;

			SQLServer SQLServer = new SQLServer();
			SqlCommand Command = new SqlCommand();
			DataSet dsCostSheet;
			DataTable dtTarget;
			string sCommandText;
			bool bResult = true;
			ForecastCostSheetDetail forecastCostSheetDetail = null;
			ForecastCostSheetDiscipline forecastCostSheetDiscipline = null;
			Part part = null;

			try
			{
				SQLServer.ConnectionString = ConnectionString;

				sCommandText = "Reporting.proc_ForecastCostSheet";

				Command.CommandText = sCommandText;
				Command.Parameters.Add(new SqlParameter("@CostSheetID", CostSheetID));
				Command.Parameters.Add(new SqlParameter("@NetworkID", NetworkID));
				Command.Parameters.Add(new SqlParameter("@WorkOrderID", TargetWorkOrderID));
				Command.Parameters.Add(new SqlParameter("@Previous", PreviousCostSheet));
				Command.Parameters.Add(new SqlParameter("@Archive", Archive));

				dsCostSheet = SQLServer.GetStoredProcedureDataSet(Command, sCommandText);

				dtTarget = dsCostSheet.Tables[0];

				if (dtTarget.Rows != null)
				{

					AddedOn = Convert.ToDateTime(dtTarget.Rows[0]["AddedOn"]);

					if (dtTarget.Rows[0]["TargetDate"] == System.DBNull.Value)
					{
						TargetDate = DateTime.Now;
					}
					else
					{
						TargetDate = Convert.ToDateTime(dtTarget.Rows[0]["TargetDate"]);
					}

					if (dtTarget.Rows[0]["EndDate"] == System.DBNull.Value)
					{
						EndDate = DateTime.Now.AddDays(90);
					}
					else
					{
						EndDate = Convert.ToDateTime(dtTarget.Rows[0]["EndDate"]); 
					}					

					AddedBy = dtTarget.Rows[0]["AddedBy"].ToString();
					QuoteID = dtTarget.Rows[0]["QuoteID"].ToString();
					WorkOrderID = dtTarget.Rows[0]["WorkOrderID"].ToString();
					ProjectManager = dtTarget.Rows[0]["ProjectManager"].ToString();

					JobStatus = Convert.ToInt32(dtTarget.Rows[0]["Status"]);

					ActualHours = Convert.ToDecimal(dtTarget.Rows[0]["ActualHours"]);

					Adjusted = Convert.ToBoolean(dtTarget.Rows[0]["Adjusted"]);								

				}

				dtTarget = dsCostSheet.Tables[1];

				if (dtTarget.Rows != null)
				{
					forecastCostSheetDetails = new List<ForecastCostSheetDetail> { };

					foreach (DataRow row in dtTarget.Rows)
					{
						forecastCostSheetDetail = new ForecastCostSheetDetail();

						forecastCostSheetDetail.CostSheetID = row["CostSheetID"].ToString();
						forecastCostSheetDetail.CostSheetDetailID = row["CostSheetDetailID"].ToString();
						forecastCostSheetDetail.Description = row["Description"].ToString();
						forecastCostSheetDetail.DisciplineID = Convert.ToInt16(row["DisciplineID"]);
						forecastCostSheetDetail.UnitCost = Convert.ToDecimal(row["UnitCost"]);
						forecastCostSheetDetail.Quantity = Convert.ToDecimal(row["Quantity"]);
						forecastCostSheetDetail.UnitPrice = Convert.ToDecimal(row["UnitPrice"]);
						forecastCostSheetDetail.ReasonForCall = row["ReasonForCall"].ToString();

						forecastCostSheetDetail.Calculate();

						forecastCostSheetDetails.Add(forecastCostSheetDetail);
					}
				}

				dtTarget = dsCostSheet.Tables[2];

				if (dtTarget.Rows != null)
				{
					Parts = new List<Part> { };

					foreach (DataRow row in dtTarget.Rows)
					{
						part = new Part();

						part.CostSheetID = row["CostSheetID"].ToString();
						part.CostSheetDetailID = row["CostSheetDetailID"].ToString();

						part.DisciplineID =  Convert.ToInt32(row["DisciplineID"]);
						part.Description = row["Description"].ToString();
						part.PartID = row["PartID"].ToString();
						part.Quantity = Convert.ToDecimal(row["Quantity"]);
						part.UnitCost = Convert.ToDecimal(row["UnitCost"]);
						part.UnitPrice = Convert.ToDecimal(row["UnitPrice"]);
						part.ApplyPST = Convert.ToBoolean(row["IncludePST"]);
						
						part.Calculate();

						Parts.Add(part);
					}
				}

				dtTarget = dsCostSheet.Tables[3];

				if (dtTarget.Rows != null)
				{
					forecastCostSheetDisciplines = new List<ForecastCostSheetDiscipline> { };

					foreach (DataRow row in dtTarget.Rows)
					{
						forecastCostSheetDiscipline = new ForecastCostSheetDiscipline();

						forecastCostSheetDiscipline.CostSheetID = row["CostSheetID"].ToString();
						forecastCostSheetDiscipline.CostSheetDisciplineID = row["CostSheetDisciplineID"].ToString();
						forecastCostSheetDiscipline.QuoteID = row["QuoteID"].ToString();
						forecastCostSheetDiscipline.DisciplineID = Convert.ToInt16(row["DisciplineID"]);
						forecastCostSheetDiscipline.Discipline = row["Discipline"].ToString();
						forecastCostSheetDiscipline.Regular = Convert.ToDecimal(row["Hours"]);
						forecastCostSheetDiscipline.OT = Convert.ToDecimal(row["HoursOT"]);
						forecastCostSheetDiscipline.DT = Convert.ToDecimal(row["HoursDT"]);
						forecastCostSheetDiscipline.HourlyCost = Convert.ToDecimal(row["CostRate"]);
						forecastCostSheetDiscipline.HourlyRate = Convert.ToDecimal(row["HourlyRate"]);

						forecastCostSheetDiscipline.Calculate();

						forecastCostSheetDisciplines.Add(forecastCostSheetDiscipline);
					}
				}

				Calculate();
				CalculateMonths();
			}
			catch (Exception ex)
			{
				bResult = false;
				Console.WriteLine(ex.Message);
			}
			finally
			{
				SQLServer.Dispose();
			}

			return bResult;
		}
		public bool Save()
		{
			SQLServer SQLServer = new SQLServer();
			string sCommandText = "";
			bool bResult = false;
			int iAdjusted = 0;

			try
			{
				SQLServer.ConnectionString = ConnectionString;
								
				sCommandText = "SELECT COUNT(CostSheetID) FROM Reporting.ForecastCostSheets WHERE CostSheetID = '" + CostSheetID + "'";

				if (SQLServer.GetIntegerValue(sCommandText) == 0)
				{
					if (Adjusted)
					{
						iAdjusted = 1;
					}

					// New Cost Sheet
					sCommandText = "INSERT INTO Reporting.ForecastCostSheets VALUES('" + CostSheetID + "'," + JobStatus + ", '" + DateTime.Now.ToString() + "'," +
						"'" + AddedBy + "','" + QuoteID + "','" + WorkOrderID + "','" + TargetDate + "', '" + EndDate + "'," +
						MonthsToComplete + ", " + HoursPerMonth + ", " + MenPerMonth + ","  + iAdjusted +")";

					SQLServer.ExecuteNonQuery(sCommandText);


					foreach (ForecastCostSheetDetail detail in forecastCostSheetDetails)
					{
						sCommandText = "INSERT INTO Reporting.ForecastCostSheetDetail VALUES('" + detail.CostSheetDetailID + "','" + CostSheetID + "'," +
										detail.DisciplineID + ",NULL,'" + detail.Description + "','"+detail.ReasonForCall +"'," + detail.UnitPrice + "," + detail.UnitCost + "," +
										detail.Quantity + "," + 0 + ")";

						SQLServer.ExecuteNonQuery(sCommandText);
					}

					foreach (Part part in Parts)
					{
						sCommandText = "INSERT INTO Reporting.ForecastCostSheetDetail VALUES('" + part.CostSheetDetailID + "','" + CostSheetID + "'," + 
										part.DisciplineID +",'" + part.PartID + "','"+part.Description + "',NULL," + part.UnitPrice + "," + part.UnitCost + "," +
										part.Quantity + "," + part.TotalPrice + ")";

						SQLServer.ExecuteNonQuery(sCommandText);
					}

					// Save changes to CostSheetDisciplines
					foreach (ForecastCostSheetDiscipline sheetDiscipline in forecastCostSheetDisciplines)
					{
						sCommandText = "INSERT INTO Reporting.ForecastCostSheetDisciplines VALUES('" + sheetDiscipline.CostSheetDisciplineID +"','" + CostSheetID + "'," +
							sheetDiscipline.DisciplineID + "," + sheetDiscipline.Regular + "," + sheetDiscipline.OT + ", " + sheetDiscipline.DT + ")";

						SQLServer.ExecuteNonQuery(sCommandText);
					}
				}
				else
				{

					// Save changes to CostSheet
					sCommandText = "UPDATE Reporting.ForecastCostSheets SET TargetDate = '" + TargetDate + "', EndDate ='" + EndDate + "'" +
						",MonthsToComplete = " + MonthsToComplete + ", HoursPerMonth = " + HoursPerMonth + ", TechniciansPerMonth = " + MenPerMonth + ", Status = " + JobStatus + ", Adjusted = " + iAdjusted + 
						" WHERE CostSheetID = '" + CostSheetID + "'";

					SQLServer.ExecuteNonQuery(sCommandText);

					// Save changes to CostSheetDetail

					foreach (Part part in Parts)
					{
						sCommandText = "UPDATE Reporting.ForecastCostSheetDetail SET Quantity = " + part.Quantity + ", UnitCost = " + part.UnitCost + " WHERE CostSheetDetailID = '" + part.CostSheetDetailID + "'";

						SQLServer.ExecuteNonQuery(sCommandText);
					}

					foreach (ForecastCostSheetDetail detail in forecastCostSheetDetails)
					{
						sCommandText = "UPDATE Reporting.ForecastCostSheetDetail SET UnitPrice = " + detail.UnitPrice + ", UnitCost = " + detail.UnitCost + " WHERE CostSheetDetailID = '" + detail.CostSheetDetailID + "'";

						SQLServer.ExecuteNonQuery(sCommandText);
					}

					// Save changes to CostSheetDisciplines
					foreach (ForecastCostSheetDiscipline sheetDiscipline in forecastCostSheetDisciplines)
					{
						sCommandText = "UPDATE Reporting.ForecastCostSheetDisciplines SET Hours = " + sheetDiscipline.Regular +
							", HoursOT = " + sheetDiscipline.OT + ", HoursDT = " + sheetDiscipline.DT +
							 " WHERE CostSheetDisciplineID = '" + sheetDiscipline.CostSheetDisciplineID + "'";

						SQLServer.ExecuteNonQuery(sCommandText);
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			return bResult;
		}


		public string JobStatusDescription()
		{
			string sJobStatus = "";
			string sCommandText = "";
			SQLServer SQLServer = new SQLServer();

			try
			{
				sCommandText = "SELECT Status FROM Reporting.ForecastCostSheetStatus WHERE StatusID=" + JobStatus;

				SQLServer.ConnectionString = ConnectionString;

				sJobStatus = SQLServer.GetStringValue(sCommandText);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			return sJobStatus;
		}

		public void Calculate()
		{
			try
			{

				TotalLabourCost = 0;
				TotalLabour = 0;
				TotalPartsCost = 0;
				TotalParts = 0;
				TotalHours = 0;

				foreach (ForecastCostSheetDetail detail in forecastCostSheetDetails)
				{
					TotalLabourCost += detail.TotalCost;
					TotalLabour += detail.TotalPrice;
					
				}

				foreach (Part part in Parts)
				{
					TotalPartsCost += part.TotalCost;
					TotalParts += part.TotalPrice;
				}

				foreach (ForecastCostSheetDiscipline discipline in forecastCostSheetDisciplines)
				{
					TotalHours += discipline.TotalHours;
				}

				TotalCost = TotalLabourCost + TotalPartsCost;

				Contribution = (TotalParts + TotalLabour) - TotalCost;

				ContributionPerHour = Contribution / TotalHours;

				TotalForecast = TotalParts + TotalLabour;

				ContributionPercent = Contribution / TotalForecast;

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

		}

		public void CalculateMonths()
		{
			int iMonths;
			int iMonthDiff;

			try
			{
				iMonths = (EndDate.Year - TargetDate.Year) * 12;
				iMonthDiff = EndDate.Month - TargetDate.Month;

				MonthsToComplete = Convert.ToDecimal(iMonths + iMonthDiff);

				if (MonthsToComplete > 0)
				{
					HoursPerMonth = (TotalHours - ActualHours) / MonthsToComplete;
				}

				MenPerMonth = HoursPerMonth / 144;

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
	}

	public class ForecastCostSheetDetail
	{
		private string sCostSheetDetailID;
		public string CostSheetDetailID
		{
			get { return sCostSheetDetailID; }

			set { sCostSheetDetailID = value; }
		}

		private string sCostSheetID;
		public string CostSheetID
		{
			get { return sCostSheetID; }

			set { sCostSheetID = value; }
		}

		private string sReasonForCall;
		public string ReasonForCall
		{
			get { return sReasonForCall; }

			set { sReasonForCall = value; }
		}

		private int iDisciplineID;
		public int DisciplineID
		{
			get { return iDisciplineID; }
			set { iDisciplineID = value; }
		}

		private string sDescription;
		public string Description
		{
			get { return sDescription; }
			set { sDescription = value; }
		}

		private decimal dUnitCost;
		public decimal UnitCost
		{
			get { return dUnitCost; }
			set { dUnitCost = value; }
		}

		private decimal dTotalCost;
		public decimal TotalCost
		{
			get { return dTotalCost; }
			set { dTotalCost = value; }
		}

		private decimal dQuantity;
		public decimal Quantity
		{
			get { return dQuantity; }
			set { dQuantity = value; }
		}

		private decimal dUnitPrice;
		public decimal UnitPrice
		{
			get { return dUnitPrice; }
			set { dUnitPrice = value; }
		}

		private decimal dTotalPrice;
		public decimal TotalPrice
		{
			get { return dTotalPrice; }
			set { dTotalPrice = value; }
		}

		public void Calculate()
		{
			TotalCost = Quantity * UnitCost;

			TotalPrice = Quantity * UnitPrice;
		}
	}

	public class ForecastCostSheetDiscipline
	{
		private string sCostSheetDisciplineID;
		public string CostSheetDisciplineID
		{
			get { return sCostSheetDisciplineID; }
			set { sCostSheetDisciplineID = value; }
		}

		private string sCostSheetID;
		public string CostSheetID
		{
			get { return sCostSheetID; }
			set { sCostSheetID = value; }
		}

		private string sQuoteID;
		public string QuoteID
		{
			get { return sQuoteID; }
			set { sQuoteID = value; }
		}

		private int iDisciplineID;
		public int DisciplineID
		{			
			get { return iDisciplineID; }
			set { iDisciplineID = value; }
		}

		private string sDiscipline;
		public string Discipline
		{
			get { return sDiscipline; }
			set { sDiscipline = value; }
		}

		private decimal dRegular;
		public decimal Regular
		{
			get { return dRegular; }
			set { dRegular = value; }
		}

		private decimal dOT;
		public decimal OT
		{
			get { return dOT; }
			set { dOT = value; }
		}

		private decimal dDT;
		public decimal DT
		{
			get { return dDT; }
			set { dDT = value; }
		}

		private decimal dTotalHours;
		public decimal TotalHours
		{
			get { return dTotalHours; }
			set { dTotalHours = value; }
		}

		private decimal dHourlyCost;
		public decimal HourlyCost
		{
			get { return dHourlyCost; }
			set { dHourlyCost = value; }
		}

		private decimal dHourlyRate;
		public decimal HourlyRate
		{
			get { return dHourlyRate; }
			set { dHourlyRate = value; }
		}

		public void Calculate()
		{
			try
			{
				TotalHours = Regular + OT + DT;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		
		}

	}
	public class Part
	{
		private string sCostSheetDetailID;
		public string CostSheetDetailID
		{
			get { return sCostSheetDetailID; }

			set { sCostSheetDetailID = value; }
		}

		private string sCostSheetID;
		public string CostSheetID
		{
			get { return sCostSheetID; }

			set { sCostSheetID = value; }
		}

		private string sPartID;
		public string PartID
		{
            get{ return sPartID; }
			set { sPartID = value; }
		}

		private int iDisciplineID;
		public int DisciplineID
		{
			get { return iDisciplineID; }
			set { iDisciplineID = value; }
		}

		private string sDescription;
		public string Description
		{
			get { return sDescription; }
			set { sDescription = value; }
		}

		private decimal dUnitCost;
		public decimal UnitCost
		{
			get { return dUnitCost; }
			set { dUnitCost = value; }
		}

		private decimal dTotalCost;
		public decimal TotalCost
		{
			get { return dTotalCost; }
			set { dTotalCost = value; }
		}

		private decimal dQuantity;
		public decimal Quantity
		{
			get { return dQuantity; }
			set { dQuantity = value; }
		}

		private decimal dUnitPrice;
		public decimal UnitPrice
		{
			get { return dUnitPrice; }
			set { dUnitPrice = value; }
		}

		private decimal dTotalPrice;
		public decimal TotalPrice
		{
			get { return dTotalPrice; }
			set { dTotalPrice = value; }
		}

		private bool bApplyPST;
		public bool ApplyPST
		{
			get { return bApplyPST; }
			set { bApplyPST = value; }
		}

		public void Calculate()
		{
			decimal dPST = 1;

			if(ApplyPST)
            {
				dPST =(decimal) 1.08;
            }
			
			TotalCost = UnitCost * dPST * Quantity;
			TotalPrice = UnitPrice * dPST * Quantity;
		}

	}
}
