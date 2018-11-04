using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace eSPD.Core
{
    public static class ESPDBudget
    {
        public static string CheckBudgetSAP(ESPDJson ROW)
        {
            string Request = string.Empty;
            string Response = string.Empty;
            try
            {
                string Uri = ConfigurationManager.AppSettings["APIUrl"].ToString();
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(Uri);
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                httpWebRequest.Method = "POST";
                var username = ConfigurationManager.AppSettings["SAPUserName"].ToString();
                var password = ConfigurationManager.AppSettings["SAPPassword"].ToString(); ;
                var bytes = Encoding.UTF8.GetBytes($"{username}:{password}");
                httpWebRequest.Headers.Add("Authorization", $"Basic {Convert.ToBase64String(bytes)}");
                
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    Request = "{\"ROW\": {\"FISCAL_YEAR\": \"" + ROW.FISCAL_YEAR + "\",\"FUNDS_CTR\": \"" + ROW.FUNDS_CTR + "\",\"NO_ESPD\": \"" + ROW.NO_ESPD + "\",\"TRANSPORT\": \"" + ROW.TRANSPORT + "\",\"AKOMODASI\": \"" + ROW.AKOMODASI + "\",\"UANG_MAKAN\": \"" + ROW.UANG_MAKAN + "\",\"UANG_SAKU\": \"" + ROW.UANG_SAKU + "\"}}";
                    streamWriter.Write(Request);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();               
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    Response = streamReader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                return Response;
            }
            return Response;
        }
    }
}