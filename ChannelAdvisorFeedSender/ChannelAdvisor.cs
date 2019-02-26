using TPGNotificationAndDataFeeds.ChannelAdvisorFulfillmentServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TPGNotificationAndDataFeeds.Classes
{
    class ChannelAdvisor
    {
        public int ChannelAdvisorID { get; set; }
        public string TrackingNumber { get; set; }
        public string Account { get; set; }
        private string cs = "Server=TPGSQL01;Database=MailOrderManager;User ID=user_rbin;Password=un3dRB!n";

        public string UpdateChannelAdvisorFullfillment()
        {
            string resultMsg = "Channel Advisor Tracking Success!";

            string developerKey = string.Empty;
            string password = string.Empty;
            string accountID = string.Empty;


            //developerKey = "580c6f83-90a0-4d2f-a35b-06f260f5f049";
            //password = "TPGdevman6784!";
            developerKey = "83a02569-8a08-431e-8f4d-08293c08506d";
            password = "TPGdev2017";
            accountID = "";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            List<ChannelAdvisor> shippedOrders = new List<ChannelAdvisor>();

            using (SqlConnection con = new SqlConnection(cs))
            {

                SqlCommand cmd = new SqlCommand("spGetTrackingNumbersForChannelAdvisor", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    ChannelAdvisor Details = new ChannelAdvisor();
                    Details.ChannelAdvisorID = Convert.ToInt32(rdr["ChannelAdvisorID"]);
                    Details.TrackingNumber = rdr["TrackingNumber"].ToString();
                    Details.Account = rdr["Account"].ToString();

                    shippedOrders.Add(Details);
                }
            }

          
            if (shippedOrders.Count > 0)
            {

                foreach (var order in shippedOrders)
                {


                    try
                    {
                        accountID = order.Account;
                        ChannelAdvisorFulfillmentServices.FulfillmentServiceSoapClient fulfillService = new FulfillmentServiceSoapClient();
                        ChannelAdvisorFulfillmentServices.APICredentials AdminCredentials = new ChannelAdvisorFulfillmentServices.APICredentials();
                        AdminCredentials.DeveloperKey = developerKey;
                        AdminCredentials.Password = password;

                        ChannelAdvisorFulfillmentServices.APIResultOfString result = fulfillService.Ping(AdminCredentials);

                        GetOrderFulfillmentDetailListRequest ofdlr = new GetOrderFulfillmentDetailListRequest();
                        ofdlr.accountID = accountID;
                        ofdlr.orderIDList = new int[1];
                        ofdlr.orderIDList[0] = order.ChannelAdvisorID;
                        ofdlr.clientOrderIdentifierList = new string[1];
                        ofdlr.clientOrderIdentifierList[0] = "";
                        ChannelAdvisorFulfillmentServices.APIResultOfArrayOfOrderFulfillmentResponse test2 = fulfillService.GetOrderFulfillmentDetailList(AdminCredentials, ofdlr.accountID, ofdlr.orderIDList, ofdlr.clientOrderIdentifierList);

                        if (test2.Status == ChannelAdvisorFulfillmentServices.ResultStatus.Failure)
                        {
                            throw new Exception(test2.Message);
                        }


                        UpdateOrderFulfillmentsRequest update = new UpdateOrderFulfillmentsRequest();
                        update.fulfillmentUpdateList = new FulfillmentUpdateSubmit[1];
                        update.fulfillmentUpdateList[0] = new FulfillmentUpdateSubmit();
                        update.fulfillmentUpdateList[0].TrackingNumber = order.TrackingNumber;
                        update.fulfillmentUpdateList[0].FulfillmentStatus = "Complete";
                        update.fulfillmentUpdateList[0].ShippedDateGMT = DateTime.UtcNow;
                        //update.fulfillmentUpdateList[0].CarrierCode = "AMZN_US";
                        update.fulfillmentUpdateList[0].FulfillmentID = test2.ResultData[0].FulfillmentList[0].FulfillmentID;

                        ChannelAdvisorFulfillmentServices.APIResultOfArrayOfFulfillmentOperationResponse test = fulfillService.UpdateOrderFulfillments(AdminCredentials, accountID, order.ChannelAdvisorID, "", update.fulfillmentUpdateList);

                        if (test.Status == ChannelAdvisorFulfillmentServices.ResultStatus.Failure)
                        {
                            throw new Exception(test.Message);
                        }
                    }
                    catch (Exception ex)
                    {
                        resultMsg = ex.Message;
                        return resultMsg;
                    }
                }
            }
            else
            {
                resultMsg = "No orders need tracking numbers set.";
            }

            return resultMsg;
        }

    }
}
