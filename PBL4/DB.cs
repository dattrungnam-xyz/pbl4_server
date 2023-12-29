//-- for further improvement: put all webclient in try-catch

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Server
{
    internal class DB
    {
        public static void addNewBot(TcpClient client)
        {
            IPEndPoint remoteEndPoint = (IPEndPoint)client.Client.RemoteEndPoint;
            string ip = remoteEndPoint.Address.ToString();
            int port = remoteEndPoint.Port;
            string post = "ip=" + ip + "&port=" + port;
            WebClient wc = new WebClient();
            wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            Console.WriteLine(ip);
            Console.WriteLine(port);
            wc.UploadString("http://localhost:7777/PBL4/php/addBot.php", post);
            //  Gửi yêu cầu POST đến URL với dữ liệu là nội dung chuỗi post
        }
        public static void insertDB(string id, string type, string detail, string content) 
        {
            WebClient wc = new WebClient();
            wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            wc.UploadString("http://localhost:7777/PBL4/php/addFile.php","table=" + type + "&idbot=" + id + "&detail=" + detail + "&content=" + content);
        }
        public static void resetBotStatus()
        {
            WebClient wc = new WebClient();
            string url = "http://localhost:7777/PBL4/php/resetBotStatus.php";
            wc.DownloadData(url);
        }
        public static void resetCommandBot()
        {
            WebClient wc = new WebClient();
            string url = "http://localhost:7777/PBL4/php/resetCommandBot.php";
            wc.DownloadData(url);
        }
        public static string getIdByIpAndPort(String ip, String port)
        {
            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                NameValueCollection data = new NameValueCollection();
                data["ip"] = ip;
                data["port"] = port;

                byte[] responseBytes = wc.UploadValues("http://localhost:7777/PBL4/php/getIdBotByIpAndPort.php", "POST", data);

                string response = Encoding.UTF8.GetString(responseBytes);

                return response;
            }
        }
        public static void resetStatusBotByIpAndPort(string IpAndPort)
        {
            string Ip = IpAndPort.Split(':')[0];
            string Port = IpAndPort.Split(':')[1];
            string Id = getIdByIpAndPort(Ip,Port);

            WebClient wc = new WebClient();
            string url = "http://localhost:7777/PBL4/php/resetBotStatus.php?ID="+Id;
            wc.DownloadData(url);
        }
    }
}
