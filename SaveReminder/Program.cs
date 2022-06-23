using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HWND = System.IntPtr;

namespace SaveReminder
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (!Wait4OpenProcess("SaveReminder", 10))
            {
                return;
            }

            // Ereignis-Handler für UI-Threads:
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            // Alle unbehandelten WinForms-Fehler durch diesen Ereignis-Handler 
            // zwingen (unabhängig von config-Einstellungen):
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            // Ereignis-Hanlder für nicht UI-Threads:
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new SaveReminder.MainForm());
            }
            catch { Application.Restart(); }

            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            Application.Restart();
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            CurrentDomain_UnhandledException(sender, new UnhandledExceptionEventArgs(e.Exception, true));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="MainTitleShort"></param>
        /// <param name="WaitSeconds"></param>
        /// <returns></returns>
        public static bool Wait4OpenProcess(string MainTitleShort, int WaitSeconds)
        {
            int cnt = 0; double SleepingTime = 1000 / 4f; double MaxCounter = WaitSeconds * 1000 / SleepingTime;
            while (OpenWindowGetter.IsProcessOpen(MainTitleShort) != null && cnt < MaxCounter) //max 10 sec warten
            {
                cnt++;
                System.Threading.Thread.Sleep((int)SleepingTime);
                if (cnt == (int)(MaxCounter / 2f))
                {
                    OpenWindowGetter.KillProcess(MainTitleShort);
                    OpenWindowGetter.CloseForm(MainTitleShort);
                }
            }
            if (OpenWindowGetter.IsProcessOpen(MainTitleShort) != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>Contains functionality to get all the open windows.</summary>
        public static class OpenWindowGetter
        {
            public static bool IsFormOpen(string f)
            {
                // too many calls ----> lvLogger.Verbose(Helper.ProgramStack);

                try
                {
                    foreach (KeyValuePair<IntPtr, string> window in OpenWindowGetter.GetOpenWindows())
                    {
                        IntPtr handle = window.Key;
                        if (window.Value.ToLower() == f.ToLower())
                        {
                            return true;
                        }
                    }

                    return false;
                }
                catch { return false; }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="f"></param>
            public static void CloseForm(string f)
            {
                try
                {
                    foreach (KeyValuePair<IntPtr, string> window in OpenWindowGetter.GetOpenWindows())
                    {
                        IntPtr handle = window.Key;
                        if (window.Value.ToLower() == f.ToLower())
                        {
                            SendMessage(handle, WM_NCDESTROY, (uint)IntPtr.Zero, (uint)IntPtr.Zero);
                        }
                    }
                }
                catch { }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="f"></param>
            /// <returns></returns>
            public static Process IsProcessOpen(string f)
            {
                try
                {
                    Process currentProcess = Process.GetCurrentProcess();
                    IEnumerable<Process> RunningProcesses = Process.GetProcesses().
                       Where(pr => pr.ProcessName.ToLower() == f.ToLower() && pr.Id != currentProcess.Id);

                    foreach (Process process in RunningProcesses)
                    {
                        return process;
                    }

                    return null;
                }
                catch { return null; }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="f"></param>
            public static void KillProcess(string f)
            {
                try
                {
                    Process currentProcess = Process.GetCurrentProcess();
                    IEnumerable<Process> RunningProcesses = Process.GetProcesses().
                        Where(pr => pr.ProcessName.ToLower() == f.ToLower() && pr.Id != currentProcess.Id);

                    foreach (Process process in RunningProcesses)
                    {
                        process.Kill();
                    }
                }
                catch { }
            }
            /// <summary>Returns a dictionary that contains the handle and title of all the open windows.</summary>
            /// <returns>A dictionary that contains the handle and title of all the open windows.</returns>
            public static IDictionary<HWND, string> GetOpenWindows()
            {
                // too many calls ---->  lvLogger.Verbose(Helper.ProgramStack);

                HWND shellWindow = GetShellWindow();
                Dictionary<HWND, string> windows = new Dictionary<HWND, string>();

                EnumWindows(delegate (HWND hWnd, int lParam)
                {
                    if (hWnd == shellWindow)
                    {
                        return true;
                    }

                    if (!IsWindowVisible(hWnd))
                    {
                        return true;
                    }

                    int length = GetWindowTextLength(hWnd);
                    if (length == 0)
                    {
                        return true;
                    }

                    StringBuilder builder = new StringBuilder(length);
                    GetWindowText(hWnd, builder, length + 1);

                    windows[hWnd] = builder.ToString();
                    return true;

                }, 0);

                return windows;
            }

            private delegate bool EnumWindowsProc(HWND hWnd, int lParam);

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, uint wParam, uint lParam);

            private const uint WM_CLOSE = 0x0010;
            private const uint WM_IME_NOTIFY = 0x0282;
            private const uint WM_DESTROY = 0x0002;
            private const uint WM_NCDESTROY = 0x0082;
            private const uint IMN_CLOSESTATUSWINDOW = 0x0001;
            private const uint WM_KILLFOCUS = 0x0008;
            private const uint WM_COMMAND = 0x0011;
            public const uint WM_SYSCOMMAND = 0x0112;
            public const uint SC_CLOSE = 0xF060;

            public static void CloseWindow(IntPtr hwnd)
            {
                SendMessage(hwnd, WM_SYSCOMMAND, SC_CLOSE, 0);
            }

            [DllImport("kernel32.dll", SetLastError = true, EntryPoint = "GetCurrentProcessId")]
            private static extern int GetCurrentProcessIdWin();

            [DllImport("USER32.DLL")]
            private static extern bool EnumWindows(EnumWindowsProc enumFunc, int lParam);

            [DllImport("USER32.DLL")]
            private static extern int GetWindowText(HWND hWnd, StringBuilder lpString, int nMaxCount);

            [DllImport("USER32.DLL")]
            private static extern int GetWindowTextLength(HWND hWnd);

            [DllImport("USER32.DLL")]
            private static extern bool IsWindowVisible(HWND hWnd);

            [DllImport("USER32.DLL")]
            private static extern IntPtr GetShellWindow();
        }
    }
}
