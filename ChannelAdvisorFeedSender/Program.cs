using ChannelAdvisorFeedSender.Functions.Data_Feeds;
using System;

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
                default:
                    Console.WriteLine(Message);
                    break;
            }
            Console.ReadLine();
        }
    }

}
