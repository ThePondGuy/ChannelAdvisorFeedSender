using System;
using System.IO;
using TPGNotificationAndDataFeeds.Properties;
using WinSCP;

namespace TPGNotificationAndDataFeeds.Classes
{
    class FTP
    {
        public string host { get; set; }
        public int port { get; set; }
        public string username { get; set; }
        public string password { get; set; }


        public static void SendFTP(string filepath, string filename, string destination, string account)
        {
            WriteToFile.WriteTextToFile("{0} Sending file via SFTP");
            WriteToFile.WriteTextToFile("{0} Getting account Credentials for: " + account);
            FTP f = new FTP();
            f = GetFTPCreds(account);

            WriteToFile.WriteTextToFile("{0} Attempting to connect");
            WriteToFile.WriteTextToFile("{0} f.host: " + f.host.ToString());
            WriteToFile.WriteTextToFile("{0} f.username: " + f.username.ToString());
            WriteToFile.WriteTextToFile("{0} f.password: " + f.password.ToString());

            try
            {
                SessionOptions sessionOptions = new SessionOptions();
                sessionOptions.Protocol = Protocol.Ftp;
                sessionOptions.HostName = f.host;
                sessionOptions.UserName = f.username;
                sessionOptions.Password = f.password;
                sessionOptions.TimeoutInMilliseconds = 20000;



                using (Session session = new Session())
                {
                    // Connect

                    session.Open(sessionOptions);

                    // Upload files
                    TransferOptions transferOptions = new TransferOptions();
                    transferOptions.TransferMode = TransferMode.Binary;

                    TransferOperationResult transferResult = default(TransferOperationResult);
                    transferResult = session.PutFiles(filepath + filename, "/" + destination + filename, false, transferOptions);

                    // Throw on any error
                    transferResult.Check();

                    // Print results
                    foreach (var transfer in transferResult.Transfers)
                    {
                        WriteToFile.WriteTextToFile("{0} Connected to the client.  Successfully sent: " + filename);
                    }
                }
            }
            catch (Exception ex)
            {
                WriteToFile.WriteTextToFile("{0} Could not connect to the client");
                WriteToFile.WriteTextToFile("Exception: " + ex.Message);
                SendEmail.Send("SendFTP", ex.Message, "itdevelopment@thepondguy.com", false);
            }




        }
        public static void SendSFTP(string filepath, string filename, string destination ,string account)
        {
            WriteToFile.WriteTextToFile("{0} Sending file via SFTP");
            WriteToFile.WriteTextToFile("{0} Getting account Credentials for: " + account);
            FTP f = new FTP();
            f = GetFTPCreds(account);

            WriteToFile.WriteTextToFile("{0} Attempting to connect");
            WriteToFile.WriteTextToFile("{0} f.host: "+ f.host.ToString());
            WriteToFile.WriteTextToFile("{0} f.username: "+ f.username.ToString());
            WriteToFile.WriteTextToFile("{0} f.password: "+ f.password.ToString());

            try
            {
                SessionOptions sessionOptions = new SessionOptions();
                sessionOptions.Protocol = Protocol.Sftp;
                sessionOptions.HostName = f.host;
                sessionOptions.UserName = f.username;
                sessionOptions.Password = f.password;
                sessionOptions.PortNumber = f.port;
                sessionOptions.GiveUpSecurityAndAcceptAnySshHostKey = true;

                using (Session session = new Session())
                {
                    // Connect

                    session.Open(sessionOptions);

                    // Upload files
                    TransferOptions transferOptions = new TransferOptions();
                    transferOptions.TransferMode = TransferMode.Binary;

                    TransferOperationResult transferResult = default(TransferOperationResult);
                    transferResult = session.PutFiles(filepath + filename, "/"+ destination + filename, false, transferOptions);

                    // Throw on any error
                    transferResult.Check();

                    // Print results
                    foreach (var transfer in transferResult.Transfers)
                    {
                        WriteToFile.WriteTextToFile("{0} Connected to the client.  Successfully sent: "+ filename);
                    }
                }
            }catch(Exception ex)
            {
                WriteToFile.WriteTextToFile("{0} Could not connect to the client");
                WriteToFile.WriteTextToFile("Exception: " + ex.Message);
                SendEmail.Send("SendSFTP", ex.Message, "itdevelopment@thepondguy.com", false);
            }
        


        }

        public static FTP GetFTPCreds(string account)
        {
            FTP f = new FTP();
            string[] creds = new string[3];
            switch (account)
            {
                case "PowerReviews":
                    creds = Settings.Default.PowerReviews.Split(';');
                    break;
                case "ROI":
                    creds = Settings.Default.ROI.Split(';');
                    break;
                case "SLI":
                    creds = Settings.Default.SLICredentials.Split(';');
                    break;
                case "ChannelAdvisor":
                    creds = Settings.Default.ChannelAdvisorCredentials.Split(';');
                    break;
            }

            f.host = creds[0].ToString();
            f.port = 22;
            f.username = creds[1].ToString();
            f.password = creds[2].ToString();

            return f;
        }
    }
}
