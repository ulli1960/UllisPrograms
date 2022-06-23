using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;
using log4net.Core;
using log4net.Repository.Hierarchy;
using UllisPrograms.FunctionLib;

namespace WinRename
{
    static class Program
    {
        private static readonly log4net.ILog lvLogger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Logger root = ((Hierarchy)LogManager.GetRepository()).Root;
            root.AddAppender(UllisPrograms.FunctionLib.Helper.GetFileAppender(UllisPrograms.FunctionLib.Helper.GetAndCreateLogFolder("WinRename"), log4net.Core.Level.All, false));
            root.Repository.Configured = true;
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            ((Hierarchy)logRepository).Root.Level = Level.All;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
