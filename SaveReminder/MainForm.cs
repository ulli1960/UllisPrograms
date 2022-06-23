using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SaveReminder
{
    public partial class MainForm : Form
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private static System.Windows.Forms.Timer OneSecondTimer = new System.Windows.Forms.Timer();
        private static decimal mSec = 160;

        public MainForm()
        {
            InitializeComponent();

            this.notifyIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon.BalloonTipTitle = "SaveReminder";

            OneSecondTimer.Interval = 400;
            OneSecondTimer.Enabled = true;
            OneSecondTimer.Tick += OneSecondTimer_Tick;

            this.MaximumSize = this.MinimumSize = this.Size;

            txtProgress.BackColor = SystemColors.ScrollBar;
            txtProgress.ForeColor = SystemColors.ControlText;

            this.Text = "SaveReminder";

            MyHint.Text =
               string.Format("Dies Programm löst bei {0} Sekunden Inaktivität die F12-Taste aus. "
            + "Dies wird in den üblichen Microsoft-Programmen als 'Sichern' interpretiert. "
            + "Daher kann man dann kaum vergessen, zu sichern. "
            + "Das Programm kann verkleinert werden und verschwindet dann im Sys-Tray, "
            + "rechts unten in der Taskleiste...", mSec);

            Debug.Print("running...");
        }
        /// <summary>
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OneSecondTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                decimal t = GetIdleTime();
                txtProgress.Text = (t / 1000).ToString() + " sec. Inaktivität";
                UInt32 u = Convert.ToUInt32(t / 1000);
                if (u > 1)
                    if (u % mSec == 0)
                    {
                        keybd_event(0x7b, 0x58, 0, 0); // Press F12
                        keybd_event(0x7b, 0xd8, 2, 0); // Release F12
                        txtProgress.BackColor = Color.Red;
                        txtProgress.ForeColor = Color.Yellow;
                        txtProgress.Text = "drücke F12";
                        txtProgress.Refresh();
                        Thread.Sleep(1000);
                        txtProgress.BackColor = SystemColors.ScrollBar;
                        txtProgress.ForeColor = SystemColors.ControlText;

                    }
            }
            catch (Exception ex) { Debug.Print(ex.ToString()); }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainFormClosed(object sender, FormClosingEventArgs e)
        {
            try
            {
                //if (e.CloseReason != CloseReason.WindowsShutDown)
                //    e.Cancel = true;
            }
            catch (Exception ex) { Debug.Print(ex.ToString()); }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Resize(object sender, EventArgs e)
        {
            try
            {
                //if the form is minimized
                //hide it from the task bar
                //and show the system tray icon (represented by the NotifyIcon control)
                if (this.WindowState == FormWindowState.Minimized)
                {
                    this.notifyIcon.Text = "eingeschaltet";
                    this.notifyIcon.BalloonTipText = "SaveReminder ist jetzt minimiert" + Environment.NewLine + this.notifyIcon.Text + Environment.NewLine;
                    this.notifyIcon.Text = "SaveReminder" + Environment.NewLine + this.notifyIcon.Text;

                    Hide();
                    notifyIcon.Visible = true;
                    notifyIcon.ShowBalloonTip(1000);
                }
            }
            catch (Exception ex) { Debug.Print(ex.ToString()); }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IconClick(object sender, MouseEventArgs e)
        {
            try
            {
                Show();
                this.WindowState = FormWindowState.Normal;
                notifyIcon.Visible = false;
            }
            catch (Exception ex) { Debug.Print(ex.ToString()); }
        }

        //--------------------------------------------------------------------------------------------------
        //
        //
        //--------------------------------------------------------------------------------------------------
        internal struct LASTINPUTINFO
        {
            public uint cbSize;
            public uint dwTime;
        }

        [DllImport("Kernel32.dll")]
        private static extern uint GetLastError();

        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        [DllImport("User32.dll")]
        private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        public static uint GetIdleTime()
        {
            LASTINPUTINFO lastInPut = new LASTINPUTINFO();
            lastInPut.cbSize = (uint)System.Runtime.InteropServices.Marshal.SizeOf(lastInPut);
            GetLastInputInfo(ref lastInPut);

            return ((uint)Environment.TickCount - lastInPut.dwTime);
        }

        public static long GetLastInputTime()
        {
            LASTINPUTINFO lastInPut = new LASTINPUTINFO();
            lastInPut.cbSize = (uint)System.Runtime.InteropServices.Marshal.SizeOf(lastInPut);
            if (!GetLastInputInfo(ref lastInPut))
            {
                throw new Exception(GetLastError().ToString());
            }

            return lastInPut.dwTime;
        }
    }
}