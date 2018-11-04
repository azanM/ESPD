using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace eSPD.Core
{
    public class LogError
    {
        public static void Log_Error(Exception ex, string modul, string id)
        {
            string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
            message += Environment.NewLine;
            message += "-----------------------------------------------------------Modul:" + modul + " ID: " + id;
            message += Environment.NewLine;
            message += string.Format("Message: {0}", ex.Message);
            message += Environment.NewLine;
            message += string.Format("StackTrace: {0}", ex.StackTrace);
            message += Environment.NewLine;
            message += string.Format("Source: {0}", ex.Source);
            message += Environment.NewLine;
            message += string.Format("TargetSite: {0}", ex.TargetSite.ToString());
            message += Environment.NewLine;
            message += "-----------------------------------------------------------";
            message += Environment.NewLine;

            string path = "E:\\SPD\\LogESPD\\ErrorLog.txt";
            if (Directory.Exists("E:\\SPD\\LogESPD\\") == false)
            {
                Directory.CreateDirectory("E:\\SPD\\LogESPD\\");
            }

            //string path = "D:\\SPD\\LogESPD\\ErrorLog.txt";
            //if (Directory.Exists("D:\\SPD\\LogESPD\\") == false)
            //{
            //    Directory.CreateDirectory("D:\\SPD\\LogESPD\\");
            //}

            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(message);
                writer.Close();
            }
        }
    }
}