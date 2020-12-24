using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;


namespace 码垛线测试临时客户端
{
    static class Program
    {
        [DllImport("User32.dll")]
        //This function puts the thread that created the specified window into the foreground and activates the window.
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [STAThread]
        static void Main()
        {
            bool createdNew;
            string appName;
            appName = System.Reflection.Assembly.GetExecutingAssembly().Location;
            appName = appName.Replace(Path.DirectorySeparatorChar, '_'); using (Mutex mutex = new Mutex(true, appName, out createdNew))
            {
                if (createdNew)
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new 码垛线测试临时客户端());
                }
                else
                {
                    Process current = Process.GetCurrentProcess();
                    foreach (Process process in Process.GetProcessesByName(current.ProcessName))
                    {
                        if (process.Id != current.Id)
                        {
                            SetForegroundWindow(process.MainWindowHandle);
                            break;
                        }
                    }
                }
            }
        }
    }
}
