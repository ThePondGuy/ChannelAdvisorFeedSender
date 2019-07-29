using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ChannelAdvisorFeedSender.Classes
{
    class CV3Creds
    {
        public string user { get; set; }
        public string pass { get; set; }
        public string serviceID { get; set; }


        public IEnumerable<CV3Creds> GetCV3Creds()
        {
            List<CV3Creds> creds = new List<CV3Creds>();
            
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionStrings.SalesPad))
                {

                    SqlCommand cmd = new SqlCommand("SELECT [UserName] ,[Password] ,[ServiceID] FROM [dbo].[_NDFCV3Creds]", con);
                    cmd.CommandTimeout = 180;
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        CV3Creds Details = new CV3Creds();
                        Details.user = rdr["UserName"].ToString();
                        Details.pass = rdr["Password"].ToString();
                        Details.serviceID = rdr["ServiceID"].ToString();
                        creds.Add(Details);
                    }
                }


                //WriteToFile.WriteTextToFile("{0} UserName: " + creds[0].user);
                //WriteToFile.WriteTextToFile("{0} Password: " + creds[0].pass);
                //WriteToFile.WriteTextToFile("{0} ServiceID: " + creds[0].serviceID);

            }
            catch (Exception ex)
            {
                WriteToFile.WriteTextToFile(ex.Message);
                SendEmail.Send("GetCV3Creds", ex.Message, "itdevelopment@thepondguy.com", false);
            }

            return creds;
        }


    }
}
