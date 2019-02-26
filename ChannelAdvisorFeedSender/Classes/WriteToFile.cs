using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannelAdvisorFeedSender.Classes
{
    class WriteToFile
    {
        public static void WriteTextToFile(string text)
        {
            System.IO.Directory.CreateDirectory("C:\\ChannelAdvisorFeedSender");
            string path = "C:\\ChannelAdvisorFeedSender\\Log_" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(string.Format(text, (DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt")) + " | "));
                writer.Close();
            }
        }
    }
}
