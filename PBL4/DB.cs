//-- for further improvement: put all webclient in try-catch

using System;
using System.Collections.Generic;
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

        public static void insertDB(string id, string type, string detail, string content) // idBot, bảng, tên lệnh, nội dung trả về
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
        public static string getIdByIpAndPort(String ip, String port)
        {
            WebClient wc = new WebClient();
            wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            wc.UploadString("http://localhost:7777/PBL4/php/getIdBotByIpAndPort.php", "&ip=" + ip + "&port=" + port );
            return wc.DownloadString("http://localhost:7777/PBL4/php/getIdBotByIpAndPort.php");
            
        }
    }
}
