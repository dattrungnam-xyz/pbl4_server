using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class Commands
    {

        public static string Run(string cmdToRun)
        {
            try
            {
                string retString = "";
                Process processCmd = new Process();
                processCmd.StartInfo.FileName = "cmd.exe";
                //--use /c as a cmd argument to close cmd.exe once its finish processing your commands
                processCmd.StartInfo.Arguments = "/c " + cmdToRun;
                // Sử dụng đối số "/c" để đảm bảo rằng Command Prompt sẽ đóng sau khi hoàn thành xử lý các lệnh
                // và thêm cmdToRun là lệnh cần thực thi.
                processCmd.StartInfo.UseShellExecute = false;
                // không sử dụng giao diện người dùng mặc định.
                processCmd.StartInfo.CreateNoWindow = true;
                // không tạo ra một cửa sổ command line riêng biệt.
                processCmd.StartInfo.WorkingDirectory = Environment.CurrentDirectory;
                // Đặt thư mục làm việc cho quá trình thực thi là thư mục hiện tại của ứng dụng.
                processCmd.StartInfo.RedirectStandardOutput = true;
                processCmd.StartInfo.RedirectStandardError = true;
                // Cả hai dòng: Chuyển hướng dữ liệu đầu ra chuẩn và dữ liệu lỗi.
                processCmd.Start();

                retString += processCmd.StandardOutput.ReadToEnd();
                retString += processCmd.StandardError.ReadToEnd();
                // Đọc dữ liệu đầu ra chuẩn và lỗi từ quá trình thực thi và ghi chúng vào biến retString
                return retString;

                // Chứa kết quả của lệnh command line và thông báo lỗi(nếu có).
            }
            catch (Exception ex)
            {
                return ex.Message.ToString() + Environment.NewLine;
            }

        }
    }


}
