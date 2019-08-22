using ChannelAdvisorFeedSender.Classes;
using ChannelAdvisorFeedSender.Functions.Data_Feeds;
using System;
using System.Data;
using System.Data.SqlClient;

namespace ChannelAdvisorFeedSender
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Which feed would you like to send:");
            Console.WriteLine("Main Feed - 1");
            Console.WriteLine("Qty Only Feed - 2");
            Console.WriteLine("Price Ship Feed - 3");
            Console.WriteLine("CV3 Inventory Download - 4");

            string input = Console.ReadLine();
            Console.WriteLine("Processing... Please Wait...");
            ChannelAdvisorFeed ca = new ChannelAdvisorFeed();
            string Message = "You did not make a valid selection!";
            switch (input)
            {
                case "1":
                    try
                    {
                        ca.QueuePowerReviewsFullInventoryData();
                        Console.WriteLine("Main Feed Successfully sent!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    break;
                case "2":
                    try
                    {
                        ca.QueuePowerReviewsQtyOnly();
                        Console.WriteLine("Qty Only Feed Successfully sent!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    break;
                case "3":
                    try
                    {
                        ca.QueueChannelAdvisorPriceShip();
                        Console.WriteLine("Price Ship Feed Successfully sent!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    break;
                case "4":
                    try
                    {
                        using (SqlConnection con = new SqlConnection(ConnectionStrings.SalesPad))
                        {

                            SqlCommand cmd = new SqlCommand("spcpClearNDFTables", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 180;
                            con.Open();
                            cmd.ExecuteNonQuery();
                        }

                        CV3JSONDownload cjd = new CV3JSONDownload();
                        bool resultProd = cjd.ProductDownload();
                        bool resultCat = cjd.CategoryDownload();

                        if (!resultProd)
                        {
                            Console.WriteLine("Product Failed to Download");
                        }
                        if (!resultCat)
                        {
                            Console.WriteLine("Category Failed to Download");
                        }
                        if (resultProd && resultCat)
                        {
                            Console.WriteLine("CV3 Download Complete!");
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    break;
                default:
                    Console.WriteLine(Message);
                    break;
            }
            Console.ReadLine();
        }
    }

}
