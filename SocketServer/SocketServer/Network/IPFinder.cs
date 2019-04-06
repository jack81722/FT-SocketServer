﻿
using System.IO;
using System.Net;

namespace FTServer.Network
{
    /// <summary>
    /// 搜尋IP位址之靜態功能類別
    /// </summary>
    public static class IPFinder
    {
        /// <summary>
        /// 取得本機端IP，例如192.168.xxx.xxx
        /// </summary>
        /// <returns>本機端ip address字串</returns>
        public static string GetLocalhostIP()
        {
            string localhostIPAddress = "";
            System.Net.IPAddress[] ips = System.Net.Dns.GetHostAddresses(System.Net.Dns.GetHostName());
            foreach (System.Net.IPAddress ip in ips)
            {
                if (ip.ToString().Substring(0, 3) == "192")
                    localhostIPAddress = ip.ToString();
            }
            return localhostIPAddress;
        }

        public static string GetExternalIP()
        {
            string result;
            WebRequest request = WebRequest.Create("http://checkip.dyndns.org/");
            WebResponse response = request.GetResponse();                                   // 取得web回應 (是否有等待過程?)
            StreamReader stream = new StreamReader(response.GetResponseStream());           // 將web回應轉成資料流
            result = stream.ReadToEnd();                                                    // 讀取資料流(轉換成字串)
            stream.Close();
            response.Close();
            //Search for the ip in the html
            //response html 
            //<html><head><title>Current IP Check</title></head><body>Current IP Address: 1.163.114.234</body></html>
            int first = result.IndexOf("Address: ") + 9;
            int last = result.LastIndexOf("</body>");
            result = result.Substring(first, last - first);
            return result;
        }
         
        public static string GetExternalIPAsync()
        {
            string result = "";
            WebRequest request = WebRequest.Create("http://checkip.dyndns.org/");
            WebResponse response = request.GetResponse();                                   // 取得web回應 (是否有等待過程?)
            var r = request.BeginGetResponse(ResponseCallBack, response);

            StreamReader stream = new StreamReader(response.GetResponseStream());           // 將web回應轉成資料流
            result = stream.ReadToEnd();                                                    // 讀取資料流(轉換成字串)
            stream.Close();
            response.Close();
            //Search for the ip in the html
            //response html 
            //<html><head><title>Current IP Check</title></head><body>Current IP Address: 1.163.114.234</body></html>
            int first = result.IndexOf("Address: ") + 9;
            int last = result.LastIndexOf("</body>");
            result = result.Substring(first, last - first);
            return result;
        }

        private static void ResponseCallBack(System.IAsyncResult result)
        {

        }
    }
}
