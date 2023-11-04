//-- for further improvement: put all webclient in try-catch

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class AddFile
    {


        public static void addNewFile(string id, string type, string detail, string content) // idBot, bảng, tên lệnh, nội dung trả về
        {
            WebClient wc = new WebClient();
            wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            wc.UploadString("http://localhost/PBL4_v2/php/addFile.php","table=" + type + "&idbot=" + id + "&detail=" + detail + "&content=" + content);
        }
    }
}
