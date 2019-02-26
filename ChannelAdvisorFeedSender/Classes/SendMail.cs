using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Configuration;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace ChannelAdvisorFeedSender.Classes
{
    public class SendEmail
    {
        public static object ConfigurationManager { get; private set; }

        private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            // Get the unique identifier for this asynchronous operation.
            MailMessage message = e.UserState as MailMessage;

            if (!e.Cancelled && e.Error != null)
            {
                //WriteToFile.WriteTextToFile("Mail sent successfully");
            }
        }

        public static void Send(string subject, string emailbody, string to, bool attach = false, string cc = "", string cc2 = "")
        {
            string path = "C:\\ChannelAdvisorFeedSender\\Log_" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";

            if (true)
            {
                //create instance of MailMessage class and set the default from:
                MailMessage message = new MailMessage();

                //Set SMTP Connection
                SmtpClient smtp = new SmtpClient();

                //Build Email
                //message.From = Sender;
                message.To.Add(to);
                message.Bcc.Add("twilson@thepondguy.com");
                message.Bcc.Add("sbehn@thepondguy.com");
                if (cc != "")
                {
                    message.CC.Add(cc);
                }
                if (cc2 != "")
                {
                    message.CC.Add(cc2);
                }
                //Definds BCC(s)
                //message.Bcc.Add(ConfigurationManager.AppSettings["MailMsgBCC"]);
                message.Subject = subject;
                message.Body = emailbody;
                message.IsBodyHtml = true;

                if (attach)
                {
                    //attachement
                    System.Net.Mail.Attachment attachment;
                    attachment = new System.Net.Mail.Attachment(path);
                    message.Attachments.Add(attachment);
                }

                smtp.SendCompleted += (s, e) => {
                    smtp.Dispose();
                    message.Dispose();
                };
                //Send Email
                try
                {
                    smtp.SendAsync(message, null);
                }
                catch (Exception ex)
                {
                    WriteToFile.WriteTextToFile(ex.Message);
                    
                }


            }

        }
    }
}