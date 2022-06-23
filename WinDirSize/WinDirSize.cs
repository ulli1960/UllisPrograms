using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace WinDirSize
{
    public partial class WinDirSize : Form
    {
        #region Fields

        private static Dictionary<string, long> files = new Dictionary<string, long>(1024 * 1024 * 35);
        private static bool StopAll = false;

        int mRows = 100;

        private int csListSize = 0;
        private int csListDir = 1;
        private int cslistFiles = 2;
        private int csListSizeHidden = 3;

        private ColumnHeader c0 = new ColumnHeader();
        private ColumnHeader c1 = new ColumnHeader();
        private ColumnHeader c2 = new ColumnHeader();
        private ColumnHeader c3 = new ColumnHeader();

        #endregion

        #region Initialize

        /// <summary>
        /// 
        /// </summary>
        public WinDirSize()
        {
            InitializeComponent();
            this.Text = "WinDirSize V1.1";
            this.tsLabelDirectories.Text = "";
            this.tsLabelThreads.Text = "";
            this.tsLabeDir.Text = "";
            this.tsLabelError.Text = "";
            InitializeGrid();

            lvResult.DrawSubItem += new DrawListViewSubItemEventHandler(LvResult_DrawItem);
            lvResult.DrawColumnHeader += new DrawListViewColumnHeaderEventHandler(LvResult_DrawHeader);
        }

        #endregion

        #region Events

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        private void LvResult_DrawHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        private void LvResult_DrawItem(object sender, DrawListViewSubItemEventArgs e)
        {
            try
            {
                if (e.ColumnIndex != csListDir)
                {
                    e.DrawDefault = true;
                    return;
                }

                long AllSize = 0;
                for (int i = 0; i < mRows; i++)
                {
                    AllSize += long.Parse(lvResult.Items[i].SubItems[csListSizeHidden].Text, CultureInfo.CurrentCulture);
                }

                if (AllSize == 0)
                {
                    e.DrawDefault = true;
                    return;
                }

                e.DrawBackground();

                //calculate bar extent 
                Rectangle rect1 = e.Item.SubItems[csListDir].Bounds;
                double vr = rect1.Width * long.Parse(e.Item.SubItems[csListSizeHidden].Text, CultureInfo.CurrentCulture) / AllSize;
                rect1.Width = vr < rect1.Width ? Convert.ToInt32(vr) : rect1.Width;

                //draw bar
                rect1.Inflate(-2, -2);
                e.Graphics.FillRectangle(new SolidBrush(Color.Gold), rect1);
                rect1.Inflate(-1, -1);
                e.Graphics.FillRectangle(new SolidBrush(Color.LightGoldenrodYellow), rect1);

                TextFormatFlags flags = TextFormatFlags.Left;

                using (StringFormat sf = new StringFormat())
                {
                    // Store the column text alignment, letting it default
                    // to Left if it has not been set to Center or Right.
                    switch (e.Header.TextAlign)
                    {
                        case HorizontalAlignment.Center:
                            sf.Alignment = StringAlignment.Center;
                            flags = TextFormatFlags.HorizontalCenter;
                            break;
                        case HorizontalAlignment.Right:
                            sf.Alignment = StringAlignment.Far;
                            flags = TextFormatFlags.Right;
                            break;
                    }

                    e.Graphics.DrawString(e.SubItem.Text, lvResult.Font, Brushes.Black, e.Bounds, sf);
                }
            }
            catch (Exception ex)
            {
                tsLabelError.Text = ex.Message;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemStopClicked(object sender, EventArgs e)
        {
            StopAll = true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            StopAll = true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            StopAll = true;

            files.Clear();

            DelAllItems();

            if (folderBrowserDialog1.ShowDialog() != DialogResult.OK)
                return;

            tsLabeDir.Text = folderBrowserDialog1.SelectedPath;

            StopAll = false;

            Thread myNewThread = new Thread(() => GetAllFilesFromFolder(folderBrowserDialog1.SelectedPath, true));
            myNewThread.Start();

            while (!StopAll)
            {
                try
                {
                    var items = from pair in files
                                orderby pair.Value descending
                                select pair;

                    int i = 0;

                    foreach (KeyValuePair<string, long> pair in items)
                    {
                        if (i >= mRows)
                            continue;

                        ListViewItem t = new ListViewItem(new[] {
                                  (pair.Value / 1024 / 1024).ToString("#,##0.00 MB"),
                                   pair.Key,
                                   "0",
                                  (pair.Value / 1024 / 1024).ToString("00000000000"),
                                   "",
                                   ""});

                        if (!lvResult.Items[i].SubItems[csListDir].Text.Equals(pair.Key.ToString()))
                        {
                            lvResult.Items[i] = t;
                        }

                        i++;
                    }

                    int AvailableThreads = 0;
                    int PortsThreads = 0;
                    int completionPortsThreads = 0;
                    int MaxThreads = 0;
                    ThreadPool.GetAvailableThreads(out AvailableThreads, out PortsThreads);
                    ThreadPool.GetMaxThreads(out MaxThreads, out completionPortsThreads);

                    tsLabelDirectories.Text = files.Count.ToString("#,##0 directories");
                    tsLabelThreads.Text = (MaxThreads - AvailableThreads).ToString("#,##0 threads");
                    ///tsLabelGesSize.Text = (GesSize / 1024 / 1024).ToString("#,##0 MB Sum");
                    tsLabelError.Text = "";

                    Application.DoEvents();
                    Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    tsLabelError.Text = ex.Message;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvResult_ItemMouseHover(object sender, ListViewItemMouseHoverEventArgs e)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvResult_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", lvResult.FocusedItem.SubItems[csListDir].Text);
        }

        #endregion

        #region Misc

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentDirectory"></param>
        /// <returns></returns>
        private static long DirectorySize(string parentDirectory)
        {
            return new DirectoryInfo(parentDirectory).GetFiles(searchPattern: "*", searchOption: SearchOption.TopDirectoryOnly).Sum(file => file.Length);
        }
        /// <summary>
        /// 
        /// </summary>
        private void InitializeGrid()
        {
            try
            {
                lvResult.Items.Clear();

                // Set the view to show details.
                lvResult.View = View.Details;
                // Allow the user to edit item text.
                lvResult.LabelEdit = false;
                // Allow the user to rearrange columns.
                lvResult.AllowColumnReorder = false;
                // Display check boxes.
                lvResult.CheckBoxes = false;
                // Select the item and subitems when selection is made.
                lvResult.FullRowSelect = true;
                // Display grid lines.
                lvResult.GridLines = true;
                // Sort the items in the list in ascending order.
                lvResult.Sorting = SortOrder.None;
                //
                lvResult.HeaderStyle = ColumnHeaderStyle.Nonclickable;
                //
                lvResult.Scrollable = true;

                c0.Text = "Size";
                c1.Text = "Top 100 Directories sorted by FileSize descending";
                c2.Text = "Files";
                c3.Text = "...";

                // Add columns
                lvResult.Columns.Clear();
                lvResult.Columns.Add(c0);
                lvResult.Columns.Add(c1);
                lvResult.Columns.Add(c2);
                lvResult.Columns.Add(c3);

                lvResult.BackColor = System.Drawing.SystemColors.Info;
                lvResult.ForeColor = System.Drawing.SystemColors.InfoText;

                c0.Width = Convert.ToInt32(this.CreateGraphics().MeasureString("000.000.000,00 MB", lvResult.Font).Width);
                c1.Width = lvResult.DisplayRectangle.Width - c0.Width - System.Windows.Forms.SystemInformation.VerticalScrollBarWidth - 10;
                c3.Width = 0;
                c2.Width = 0;

                c0.TextAlign = HorizontalAlignment.Right;
                c1.TextAlign = HorizontalAlignment.Left;
                c2.TextAlign = HorizontalAlignment.Right;
                c3.TextAlign = HorizontalAlignment.Right;

                DelAllItems();

                lvResult.OwnerDraw = true;
            }
            catch (Exception ex)
            {
                tsLabelError.Text = ex.Message;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void DelAllItems()
        {
            lvResult.Items.Clear();
            for (int i = 0; i < mRows; i++)
                lvResult.Items.Add(
                    new ListViewItem(new[] {
                          "",
                          "" ,
                          "0",
                          "0"}));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="root"></param>
        /// <param name="searchSubfolders"></param>
        private void GetAllFilesFromFolder(string root, bool searchSubfolders)
        {
            Queue<string> folders = new Queue<string>();
            folders.Enqueue(root);
            while (folders.Count != 0 && !StopAll)
            {
                string currentFolder = folders.Dequeue();

                try
                {
                    if (searchSubfolders)
                    {
                        string[] foldersInCurrent = System.IO.Directory.GetDirectories(currentFolder, "*.*", System.IO.SearchOption.TopDirectoryOnly);
                        foreach (string _current in foldersInCurrent)
                        {
                            folders.Enqueue(_current);
                            ThreadPool.QueueUserWorkItem(ThreadPoolOperation, _current);
                            Application.DoEvents();
                        }
                    }
                }
                catch // Do Nothing
                { }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private static void ThreadPoolOperation(object obj)
        {
            try
            {
                long s = DirectorySize(obj as string);
                files.Add(obj as string, s);
            }
            catch { }
        }

        #endregion
    }
}
