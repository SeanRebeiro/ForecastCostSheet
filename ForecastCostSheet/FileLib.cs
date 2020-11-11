using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
namespace ForecastCostSheet
{
    public class FileLib
    {
        public bool ExtractCompanyImage(string CompanyID, ref string LogoImageFile, CompanyImageTypes CompanyImageType, string ConnectionString)
        {
            System.IO.FileStream DocumentStream;
            System.Data.DataTable DataTable = null; System.Data.DataRow DataRow = null;
            string sCommandText = ""; string sImageColumn = ""; string sImageSizeColumn = "";
            int iFileSize = 0; bool bResult = true;
            ServiceCallDataElements.SQLServer SQLServer = new ServiceCallDataElements.SQLServer();

            try
            {
                switch (CompanyImageType)
                {
                    case CompanyImageTypes.InvoiceLogoImage:
                        sImageColumn = "InvoiceLogoImage";
                        sImageSizeColumn = "InvoiceLogoImageSize";
                        break;
                    case CompanyImageTypes.SmallLogoImage:
                        sImageColumn = "SmallLogoImage";
                        sImageSizeColumn = "SmallLogoImageSize";
                        break;
                }

                RemoveFile(LogoImageFile);

                sCommandText = "SELECT " + sImageColumn + "," + sImageSizeColumn + " FROM Core.Companies WHERE CompanyID='" + CompanyID + "'";

                SQLServer.ConnectionString = ConnectionString;

                DataTable = SQLServer.GetDataTable(sCommandText);

                DataTable.TableName = "Logo";

                DataRow = DataTable.Rows[0];

                iFileSize = Convert.ToInt32(DataRow[sImageSizeColumn]);

                Byte[] BLOBData = new Byte[iFileSize - 1];

                BLOBData = (Byte[])DataRow[sImageColumn];

                DocumentStream = new System.IO.FileStream(LogoImageFile, System.IO.FileMode.CreateNew);

                DocumentStream.Write(BLOBData, 0, iFileSize);
                DocumentStream.Flush();
                DocumentStream.Close();

                DocumentStream = null;

            }
            catch (Exception ex)
            {
                bResult = false;
                Console.WriteLine(ex.Message);
            }
            finally
            {
                DocumentStream = null;
                DataTable = null;
            }

            return bResult;

        }
        public bool RemoveFile(string FileName)
        {

            bool bResult = true;

            try
            {
                if (FileName == "") return true;

                if (System.IO.File.Exists(FileName)) System.IO.File.Delete(FileName);
            }
            catch (Exception ex)
            {
                bResult = false;
                Console.WriteLine(ex.Message);
            }

            return bResult;

        }

        public int FileDaysAge(string FileSource)
        {

            System.IO.FileInfo FileInfo;
            int iDays = 0;

            try
            {
                FileInfo = new System.IO.FileInfo(FileSource);

                iDays = Convert.ToInt32(Microsoft.VisualBasic.DateAndTime.DateDiff(DateInterval.Day, FileInfo.LastWriteTime, DateTime.Now));

            }
            catch (Exception ex)
            {
                iDays = 0;
                Console.WriteLine(ex.Message);
            }
            finally
            {
                FileInfo = null;
            }

            return iDays;

        }

        public string CompanyLogoImageFileName(string CompanyID, CompanyImageTypes CompanyImageType, string ConnectionString)
        {
            ServiceCallDataElements.SQLServer sqlServer = new ServiceCallDataElements.SQLServer();
            string sImageFile = ""; string sName = ""; string sTypeColumn = ""; string sExtention = ""; string sTempPath = "";

            try
            {
                sTempPath = Environment.CurrentDirectory + "\\Temp\\";

                switch (CompanyImageType)
                {
                    case CompanyImageTypes.InvoiceLogoImage:
                        sTypeColumn = "InvoiceLogoImageType";
                        sName = "InvoiceLogo";
                        break;
                    case CompanyImageTypes.SmallLogoImage:
                        sTypeColumn = "SmallLogoImageType";
                        sName = "SmallLogo";
                        break;
                }

                sqlServer.ConnectionString = ConnectionString;

                sExtention = sqlServer.GetStringValue("SELECT " + sTypeColumn + " FROM Core.Companies WHERE CompanyID='" + CompanyID + "'", "");

                if (sExtention == "") sExtention = "jpg";

                sImageFile = sTempPath + sName + "_" + CompanyID + "." + sExtention;
            }
            catch (Exception ex)
            {
                sImageFile = "";
            }

            return sImageFile;

        }

        public enum CompanyImageTypes
        {
            InvoiceLogoImage = 1,
            SmallLogoImage = 2
        }

    }
}
