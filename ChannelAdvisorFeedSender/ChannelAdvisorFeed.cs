using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChannelAdvisorFeedSender.Classes;
using ChannelAdvisorFeedSender.Classes.Models;

namespace ChannelAdvisorFeedSender.Functions.Data_Feeds
{
    class ChannelAdvisorFeed
    {
        //PRICE+SHIP FEED        CODE: PRICESHIP
        //INVENTORY FEED         CODE: QTYFEED
        //MAIN DATA FEEDS(ALL)   CODE: MAINDATA

        public string QueueChannelAdvisorPriceShip()
        {
            WriteToFile.WriteTextToFile("{0} QueueChannelAdvisorPriceShip Started");

            //gather data
            WriteToFile.WriteTextToFile("{0} Getting Inventory data to put in list");
            List<ChannelAdvisorModel> data = new List<ChannelAdvisorModel>();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionStrings.SalesPad))
                {

                    SqlCommand cmd = new SqlCommand("spcpChannelAdvisorShipPrice", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        ChannelAdvisorModel Details = new ChannelAdvisorModel();
                        Details.Item_Number = rdr["SKU"].ToString();
                        Details.Current_Cost = rdr["COGS"].ToString();
                        Details.lowestPrice = rdr["MINIMUMPRICE"].ToString();
                        Details.availcode = rdr["AVAILABILITYCODE"].ToString();
                        Details.Item_Shipping_Weight = rdr["WEIGHT"].ToString();
                        Details.shippingMethod = rdr["SHIPPINGMETHOD"].ToString();
                        Details.xAddtionalHandling = rdr["ADDTIONALHANDLING"].ToString();
                        Details.DropShip = Convert.ToBoolean(rdr["DROPSHIP"]);
                        Details.STRATEGYMAPP = rdr["STRATEGYMAPP"].ToString();

                        data.Add(Details);
                    }
                }
                WriteToFile.WriteTextToFile("{0} Items in List: " + data.Count);

            }
            catch (Exception ex)
            {
                WriteToFile.WriteTextToFile("Exception: " + ex.Message);
                SendEmail.Send("QueueChannelAdvisorPriceShip: gather data", ex.Message, "itdevelopment@thepondguy.com", false);
            }

            //create file
            WriteToFile.WriteTextToFile("{0} Create and load string builder");
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("\"SKU\"," + "\"COGS\"," + "\"MINIMUMPRICE\"," + "\"AVAILABILITYCODE\"," + "\"WEIGHT\"," + "\"SHIPPINGMETHOD\"," + "\"ADDTIONALHANDLING\"," + "\"STRATEGYMAPP\"," + "\"DROPSHIP\"" + "\r\n");
                foreach (ChannelAdvisorModel item in data)
                {
                    sb.Append("\"" + item.Item_Number + "\", ");
                    sb.Append(item.Current_Cost + ",");
                    sb.Append("\"" + item.lowestPrice + "\",");
                    sb.Append("\"" + item.availcode + "\",");
                    sb.Append("\"" + item.Item_Shipping_Weight + "\",");
                    sb.Append("\"" + item.shippingMethod + "\",");
                    sb.Append("\"" + item.xAddtionalHandling + "\",");
                    sb.Append("\"" + item.STRATEGYMAPP + "\",");
                    sb.Append("\"" + item.DropShip + "\"");
                    sb.Append("\r\n");
                }

                WriteToFile.WriteTextToFile("{0} Creating file");
                StreamWriter Excel = new StreamWriter("C:\\ChannelAdvisorFeedSender" + "\\PRICE+SHIP FEED.PRICESHIP.csv");
                Excel.WriteLine(sb.ToString());
                Excel.Close();
            }
            catch (Exception ex)
            {
                WriteToFile.WriteTextToFile("Exception: " + ex.Message);
                SendEmail.Send("QueueChannelAdvisorPriceShip: create file", ex.Message, "itdevelopment@thepondguy.com", false);
            }

            //ftp file
            WriteToFile.WriteTextToFile("{0} FTP File Started");
            try
            {
                FTP.SendFTP("C:\\ChannelAdvisorFeedSender\\", "PRICE+SHIP FEED.PRICESHIP.csv", "Inventory/Transform/", "ChannelAdvisor");
            }
            catch (Exception ex)
            {
                WriteToFile.WriteTextToFile("Exception: " + ex.Message);
                SendEmail.Send("QueueChannelAdvisorPriceShip: ftp file", ex.Message, "itdevelopment@thepondguy.com", false);
            }

            //archive File
            WriteToFile.WriteTextToFile("{0} Archive File Started");
            try
            {

                string newFileName = "";
                if (File.Exists("C:\\ChannelAdvisorFeedSender\\PRICE+SHIP FEED.PRICESHIP.csv"))
                {
                    if (!Directory.Exists("C:\\ChannelAdvisorFeedSender\\archiveChannelAdvisor"))
                    {
                        Directory.CreateDirectory("C:\\ChannelAdvisorFeedSender\\archiveChannelAdvisor");
                    }
                    newFileName = DateTime.Now.ToString("yyyy-MM-dd--HH-mm-ss") + "_PRICE+SHIP FEED.PRICESHIP.csv";
                    File.Move("C:\\ChannelAdvisorFeedSender\\PRICE+SHIP FEED.PRICESHIP.csv", "C:\\ChannelAdvisorFeedSender\\archiveChannelAdvisor\\" + newFileName);
                }

                WriteToFile.WriteTextToFile("{0} Moved file: " + newFileName);
            }
            catch (Exception ex)
            {
                WriteToFile.WriteTextToFile("Exception: " + ex.Message);
                SendEmail.Send("QueueChannelAdvisorPriceShip: archive File", ex.Message, "itdevelopment@thepondguy.com", false);
            }

            return "{0} QueueChannelAdvisorPriceShip Completed";
        }

        public string QueuePowerReviewsQtyOnly()
        {
            WriteToFile.WriteTextToFile("{0} QueuePowerReviewsQtyOnly Started");

            //gather data
            WriteToFile.WriteTextToFile("{0} Getting Inventory data to put in list");
            List<ChannelAdvisorModel> data = new List<ChannelAdvisorModel>();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionStrings.SalesPad))
                {

                    SqlCommand cmd = new SqlCommand("spcpChannelAdvisorQtyOnly", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        ChannelAdvisorModel Details = new ChannelAdvisorModel();
                        Details.Item_Number = rdr["SKU"].ToString();
                        Details.Available = rdr["QUANTITY"].ToString();
                        Details.WeeksOfInventory = rdr["WEEKSINSTOCK"].ToString();

                        data.Add(Details);
                    }
                }
                WriteToFile.WriteTextToFile("{0} Items in List: " + data.Count);

            }
            catch (Exception ex)
            {
                WriteToFile.WriteTextToFile("Exception: " + ex.Message);
                SendEmail.Send("QueuePowerReviewsQtyOnly: gather data", ex.Message, "itdevelopment@thepondguy.com", false);
            }

            //create file
            WriteToFile.WriteTextToFile("{0} Create and load string builder");
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("\"SKU\"," + "\"QUANTITY\"," + "\"WEEKSINSTOCK\"" + "\r\n");
                foreach (ChannelAdvisorModel item in data)
                {
                    sb.Append("\"" + item.Item_Number + "\", ");
                    sb.Append(item.Available + ",");
                    sb.Append(item.WeeksOfInventory);
                    sb.Append("\r\n");
                }

                WriteToFile.WriteTextToFile("{0} Creating file");
                StreamWriter Excel = new StreamWriter("C:\\ChannelAdvisorFeedSender" + "\\INVENTORY FEED.QTYFEED.csv");
                Excel.WriteLine(sb.ToString());
                Excel.Close();
            }
            catch (Exception ex)
            {
                WriteToFile.WriteTextToFile("Exception: " + ex.Message);
                SendEmail.Send("QueuePowerReviewsQtyOnly: create file", ex.Message, "itdevelopment@thepondguy.com", false);
            }

            //ftp file
            WriteToFile.WriteTextToFile("{0} FTP File Started");
            try
            {
                FTP.SendFTP("C:\\ChannelAdvisorFeedSender\\", "INVENTORY FEED.QTYFEED.csv", "Inventory/Transform/", "ChannelAdvisor");
            }
            catch (Exception ex)
            {
                WriteToFile.WriteTextToFile("Exception: " + ex.Message);
                SendEmail.Send("QueuePowerReviewsQtyOnly: ftp file", ex.Message, "itdevelopment@thepondguy.com", false);
            }

            //archive File
            WriteToFile.WriteTextToFile("{0} Archive File Started");
            try
            {

                string newFileName = "";
                if (File.Exists("C:\\ChannelAdvisorFeedSender\\INVENTORY FEED.QTYFEED.csv"))
                {
                    if (!Directory.Exists("C:\\ChannelAdvisorFeedSender\\archiveChannelAdvisor"))
                    {
                        Directory.CreateDirectory("C:\\ChannelAdvisorFeedSender\\archiveChannelAdvisor");
                    }
                    newFileName = DateTime.Now.ToString("yyyy-MM-dd--HH-mm-ss") + "_INVENTORY FEED.QTYFEED.csv";
                    File.Move("C:\\ChannelAdvisorFeedSender\\INVENTORY FEED.QTYFEED.csv", "C:\\ChannelAdvisorFeedSender\\archiveChannelAdvisor\\" + newFileName);
                }

                WriteToFile.WriteTextToFile("{0} Moved file: " + newFileName);
            }
            catch (Exception ex)
            {
                WriteToFile.WriteTextToFile("Exception: " + ex.Message);
                SendEmail.Send("QueuePowerReviewsQtyOnly: archive File", ex.Message, "itdevelopment@thepondguy.com", false);
            }

            return "{0} QueuePowerReviewsQtyOnly Completed";
        }

        public string QueuePowerReviewsFullInventoryData()
        {
            WriteToFile.WriteTextToFile("{0} QueuePowerReviewsFullInventoryData Started");

            //gather data
            WriteToFile.WriteTextToFile("{0} Getting Inventory data to put in list");
            List<ChannelAdvisorModel> data = new List<ChannelAdvisorModel>();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionStrings.SalesPad))
                {

                    SqlCommand cmd = new SqlCommand("spcpChannelAdvisorFullInventory", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 600;
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        ChannelAdvisorModel Details = new ChannelAdvisorModel();
                        Details.SKU = rdr["SKU"].ToString();
                        Details.BRAND = rdr["BRAND"].ToString();
                        Details.CLASSIFICATION = rdr["CLASSIFICATION"].ToString();
                        Details.DESCRIPTION = rdr["DESCRIPTION"].ToString();
                        Details.BULLET1 = rdr["BULLET1"].ToString();
                        Details.BULLET2 = rdr["BULLET2"].ToString();
                        Details.BULLET3 = rdr["BULLET3"].ToString();
                        Details.MANUFACTURER = rdr["MANUFACTURER"].ToString();
                        Details.MPN = rdr["MPN"].ToString();
                        Details.PARENTSKU = rdr["PARENTSKU"].ToString();
                        Details.RELATIONSHIPNAME = rdr["RELATIONSHIPNAME"].ToString();
                        Details.INVENTORYSUBTITLE = rdr["INVENTORYSUBTITLE"].ToString();
                        Details.TITLE = rdr["TITLE"].ToString();
                        Details.UPC = rdr["UPC"].ToString();
                        Details.PICTUREURLS = rdr["PICTUREURLS"].ToString();
                        Details.HEIGHT = rdr["HEIGHT"].ToString();
                        Details.LENGTH = rdr["LENGTH"].ToString();
                        Details.WIDTH = rdr["WIDTH"].ToString();
                        Details.SHIPPINGRESTRICTION = rdr["SHIPPINGRESTRICTION"].ToString();
                        Details.SIZE = rdr["SIZE"].ToString();
                        Details.SEARCHTERMS = rdr["SEARCHTERMS"].ToString();
                        Details.GOOGLEMERCHCATEGORY = rdr["GOOGLEMERCHCATEGORY"].ToString();
                        Details.FULLPRODURL = rdr["FULLPRODURL"].ToString();
                        Details.ISPARENT = rdr["ISPARENT"].ToString();
                        Details.PARENTCHILDDESCRIPTION = rdr["PARENTCHILDDESCRIPTION"].ToString();
                        Details.METATITLE = rdr["METATITLE"].ToString();
                        Details.METADESCRIPTION = rdr["METADESCRIPTION"].ToString();

                        Details.SDSSHEET = rdr["SDSSHEET"].ToString();
                        Details.AVERAGESTARS = rdr["AVERAGESTARS"].ToString();
                        Details.COUNTOFRATINGS = rdr["COUNTOFRATINGS"].ToString();
                        Details.ALLCATEGORIES = rdr["ALLCATEGORIES"].ToString();
                        Details.LOWCHILDPRICE = rdr["LOWCHILDPRICE"].ToString();
                        data.Add(Details);
                    }
                }
                WriteToFile.WriteTextToFile("{0} Items in List: " + data.Count);

            }
            catch (Exception ex)
            {
                WriteToFile.WriteTextToFile("Exception: " + ex.Message);
                SendEmail.Send("QueuePowerReviewsFullInventoryData: gather data", ex.Message, "itdevelopment@thepondguy.com", false);
            }

            //create file
            WriteToFile.WriteTextToFile("{0} Create and load string builder");
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("\"SKU\"," + "\"BRAND\"," + "\"CLASSIFICATION\"," + "\"DESCRIPTION\"," + "\"BULLET1\"," + "\"BULLET2\"," + "\"BULLET3\"," + "\"MANUFACTURER\"," + "\"MPN\"," +
                     "\"PARENTSKU\"," + "\"RELATIONSHIPNAME\"," + "\"INVENTORYSUBTITLE\"," + "\"TITLE\"," + "\"UPC\"," + "\"PICTUREURLS\"," +
                     "\"HEIGHT\"," + "\"LENGTH\"," + "\"WIDTH\"," + "\"SHIPPINGRESTRICTION\"," + "\"SIZE\"," + "\"SEARCHTERMS\"," +
                     "\"GOOGLEMERCHCATEGORY\"," +
                     "\"FULLPRODURL\"," + "\"ISPARENT\"," + "\"PARENTCHILDDESCRIPTION\"," + "\"METATITLE\"," + "\"METADESCRIPTION\"," + "\"AVERAGESTARS\"," + "\"COUNTOFRATINGS\"," + "\"ALLCATEGORIES\"," + "\"LOWCHILDPRICE\"," + "\"SDSSHEET\"" + "\r\n");
                foreach (ChannelAdvisorModel item in data)
                {
                    sb.Append(item.SKU.Replace(",","").Replace("\r\n", "") + ",");
                    sb.Append( item.BRAND.Replace(",", "").Replace("\r\n", "") + ",");
                    sb.Append( item.CLASSIFICATION.Replace(",", "").Replace("\r\n", "") + ",");
                    sb.Append( item.DESCRIPTION.Replace(",", "").Replace("\r\n", "") + ",");
                    sb.Append( item.BULLET1.Replace(",", "").Replace("\r\n", "") + ",");
                    sb.Append( item.BULLET2.Replace(",", "").Replace("\r\n", "") + ",");
                    sb.Append( item.BULLET3.Replace(",", "").Replace("\r\n", "") + ",");
                    sb.Append( item.MANUFACTURER.Replace(",", "").Replace("\r\n", "") + ",");
                    sb.Append( item.MPN.Replace(",", "").Replace("\r\n", "") + ",");
                    sb.Append( item.PARENTSKU.Replace(",", "").Replace("\r\n", "") + ",");
                    sb.Append( item.RELATIONSHIPNAME.Replace(",", "").Replace("\r\n", "") + ",");
                    sb.Append( item.INVENTORYSUBTITLE.Replace(",", "").Replace("\r\n", "") + ",");
                    sb.Append( item.TITLE.Replace(",", "").Replace("\r\n", "") + ",");
                    sb.Append( item.UPC.Replace(",", "").Replace("\r\n", "") + ",");
                    sb.Append( item.PICTUREURLS.Replace(",", "").Replace("\r\n", "") + ",");
                    sb.Append( item.HEIGHT.Replace(",", "").Replace("\r\n", "") + ",");
                    sb.Append( item.LENGTH.Replace(",", "").Replace("\r\n", "") + ",");
                    sb.Append( item.WIDTH.Replace(",", "").Replace("\r\n", "") + ",");
                    sb.Append( item.SHIPPINGRESTRICTION.Replace(",", "").Replace("\r\n", "") + ",");
                    sb.Append( item.SIZE.Replace(",", "").Replace("\r\n", "") + ",");
                    sb.Append( item.SEARCHTERMS.Replace(",", "").Replace("\r\n", "") + ",");
                    sb.Append( item.GOOGLEMERCHCATEGORY.Replace(",","").Replace("\r\n", "") + ",");
                    sb.Append( item.FULLPRODURL.Replace(",", "").Replace("\r\n", "") + ",");
                    sb.Append( item.ISPARENT.Replace(",", "").Replace("\r\n", "") + ",");
                    sb.Append( item.PARENTCHILDDESCRIPTION.Replace(",", "").Replace("\r\n", "") + ",");
                    sb.Append( item.METATITLE.Replace(",", "").Replace("\r\n", "") + ",");
                    sb.Append(item.METADESCRIPTION.Replace(",", "").Replace("\r\n", "") + ",");
                    sb.Append(item.AVERAGESTARS.Replace(",", "").Replace("\r\n", "") + ",");
                    sb.Append(item.COUNTOFRATINGS.Replace(",", "").Replace("\r\n", "") + ",");
                    sb.Append(item.ALLCATEGORIES.Replace(",", "").Replace("\r\n", "") + ",");
                    sb.Append(item.LOWCHILDPRICE.Replace(",", "").Replace("\r\n", "") + ",");
                    sb.Append( item.SDSSHEET.Replace(",", "").Replace("\r\n", ""));
                    sb.Append("\r\n");
                    
                }

                WriteToFile.WriteTextToFile("{0} Creating file");
                StreamWriter Excel = new StreamWriter("C:\\ChannelAdvisorFeedSender" + "\\MAIN DATA FEEDS(ALL).MAINDATA.csv");
                Excel.WriteLine(sb.ToString());
                Excel.Close();
            }
            catch (Exception ex)
            {
                WriteToFile.WriteTextToFile("Exception: " + ex.Message);
                SendEmail.Send("QueuePowerReviewsFullInventoryData: create file", ex.Message, "itdevelopment@thepondguy.com", false);
            }

            //ftp file
            WriteToFile.WriteTextToFile("{0} FTP File Started");
            try
            {
                FTP.SendFTP("C:\\ChannelAdvisorFeedSender\\", "MAIN DATA FEEDS(ALL).MAINDATA.csv", "Inventory/Transform/", "ChannelAdvisor");
            }
            catch (Exception ex)
            {
                WriteToFile.WriteTextToFile("Exception: " + ex.Message);
                SendEmail.Send("QueuePowerReviewsFullInventoryData: ftp file", ex.Message, "itdevelopment@thepondguy.com", false);
            }

            //archive File
            WriteToFile.WriteTextToFile("{0} Archive File Started");
            try
            {

                string newFileName = "";
                if (File.Exists("C:\\ChannelAdvisorFeedSender\\MAIN DATA FEEDS(ALL).MAINDATA.csv"))
                {
                    if (!Directory.Exists("C:\\ChannelAdvisorFeedSender\\archiveChannelAdvisor"))
                    {
                        Directory.CreateDirectory("C:\\ChannelAdvisorFeedSender\\archiveChannelAdvisor");
                    }
                    newFileName = DateTime.Now.ToString("yyyy-MM-dd--HH-mm-ss") + "_MAIN DATA FEEDS(ALL).MAINDATA.csv";
                    File.Move("C:\\ChannelAdvisorFeedSender\\MAIN DATA FEEDS(ALL).MAINDATA.csv", "C:\\ChannelAdvisorFeedSender\\archiveChannelAdvisor\\" + newFileName);
                }

                WriteToFile.WriteTextToFile("{0} Moved file: " + newFileName);
            }
            catch (Exception ex)
            {
                WriteToFile.WriteTextToFile("Exception: " + ex.Message);
                SendEmail.Send("QueuePowerReviewsFullInventoryData: archive File", ex.Message, "itdevelopment@thepondguy.com", false);
            }

            return "{0} QueuePowerReviewsFullInventoryData Completed";
        }

        public string IsMarketPlaceCheck()
        {
            WriteToFile.WriteTextToFile("{0} IsMarketPlaceCheck Started");
           
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionStrings.SalesPad))
                {

                    SqlCommand cmd = new SqlCommand("spcpMarkIsMarketplaces", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                
            }
            catch (Exception ex)
            {
                WriteToFile.WriteTextToFile("Exception: " + ex.Message);
                SendEmail.Send("IsMarketPlaceCheck: gather data", ex.Message, "itdevelopment@thepondguy.com", false);
            }

            return "{0} IsMarketPlaceCheck Completed";
        }
        
    }
}
