using MetadataExtractor;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using UllisPrograms.FunctionLib;

namespace WinRename
{
    /// <summary>
    /// ToDo:
    /// ------------------------
    /// Verkürzter Dir-Name
    /// nur eine Markierung über alle Tabs
    /// Autonummer-Format nicht aus string sondern aus Anzahl Digits ableiten
    /// 
    /// 
    /// </summary>

    /// <summary>
    /// 
    /// </summary>
    public partial class MainForm : Form
    {
        private static readonly log4net.ILog lvLogger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Fields

        readonly string csLeading = "leading";
        readonly string csTrailing = "trailing";
        readonly string csNum = "numbers";
        readonly string csBytes = "Bytes";

        string lvFolder;
        class StructCandidates
        {
            public string File { get; set; }
            public string New { get; set; }
            public string Old { get; set; }
            public string Exif { get; set; }
            public string Err { get; set; }
            public StructCandidates(string _file, string _new, string _old, string _err, string _exif)
            {
                Err = _err;
                File = _file;
                Exif = _exif;
                New = _new;
                Old = _old;
            }
        }
        #endregion

        #region Initialize

        /// <summary>
        /// 
        /// </summary>
        public MainForm()
        {
            lvLogger.DebugFormat(UllisPrograms.FunctionLib.Helper.ProgramStack);

            InitializeComponent();

            this.sel5cbFormat.Items.Clear();
            this.sel5cbFormat.Items.Add("yyyyMMdd_HHmmss");
            this.sel5cbFormat.Items.Add("yyMMdd_HHmmss");
            this.sel5cbFormat.Items.Add("yyyyMMdd_HHmm");

            txtMask.Text = String.Empty;
            lbProgress.Visible = false;

            sel3cbBytes.Items.Clear();
            sel3cbBytes.Items.Add("numbers");
            for (int i = 0; i < 1024; i++)
                sel3cbBytes.Items.Add(i.ToString() + " " + csBytes);
            sel3cbBytes.SelectedIndex = 0;

            lbResultRows.Text = string.Empty;
            lbMessage.Text = "-";
            lbMarkedRows.Text = "nothing marked";
            lbSelection.Text = GetMode();

            sel3cbPosition.Items.AddRange(new object[] {
            csLeading,
            csTrailing});
            sel3cbPosition.SelectedItem = csTrailing;

            GetStoredValues();

            SetUpdateGridCandidatesEvents();

            this.MainSplitter.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.MainSplitter_SplitterMoved);
            this.GridOrigin.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.GridOrigin_ColumnHeaderMouseClick);
            this.GridOrigin.DataSourceChanged += new System.EventHandler(this.GridOrigin_DataSourceChanged);
            this.GridOrigin.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(GridOrigin_DataBindingComplete);
            this.MainTab.SelectedIndexChanged += MainTab_SelectedIndexChanged;

            lvFolder = global::WinRename.Properties.Settings.Default.SelectedPath;
            if (string.IsNullOrEmpty(lvFolder))
                lbStartDir.Text = "please choose a Start Directory";
            else
                lbStartDir.Text = UllisPrograms.FunctionLib.Helper.EllipsisString(lvFolder, 50);

            GridCandidates.RowHeadersVisible = false;
        }

        #endregion

        #region Events

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainTab_SelectedIndexChanged(object sender, EventArgs e)
        {
            lvLogger.DebugFormat(UllisPrograms.FunctionLib.Helper.ProgramStack);

            try
            {
                lbSelection.Text = GetMode(); ;
            }
            catch (Exception ex) { lvLogger.Error(ex); }
            finally { }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetRootDir(object sender, EventArgs e)
        {
            lvLogger.DebugFormat(UllisPrograms.FunctionLib.Helper.ProgramStack);

            try
            {
                FolderDialog.RootFolder = Environment.SpecialFolder.MyComputer;
                FolderDialog.SelectedPath = global::WinRename.Properties.Settings.Default.SelectedPath;
                FolderDialog.ShowDialog();
                if (!string.IsNullOrEmpty(FolderDialog.SelectedPath))
                {
                    lvFolder = FolderDialog.SelectedPath;
                    lbStartDir.Text = UllisPrograms.FunctionLib.Helper.EllipsisString(lvFolder, 50);
                    global::WinRename.Properties.Settings.Default.SelectedPath = lvFolder;
                }

                GridClear(GridOrigin);
            }
            catch (Exception ex) { lvLogger.Error(ex); }
            finally { }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridOrigin_ColumnHeaderMouseClick(object sender, System.Windows.Forms.DataGridViewCellMouseEventArgs e)
        {
            lvLogger.DebugFormat(UllisPrograms.FunctionLib.Helper.ProgramStack);

            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

                SearchOption s = cbRootOnly.Checked ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories;

                //get the current column details
                string strColumnName = GridOrigin.Columns[e.ColumnIndex].Name;
                SortOrder strSortOrder = getSortOrder(e.ColumnIndex);

                if (strColumnName == "Name")
                    if (strSortOrder == SortOrder.Descending)
                        GridOrigin.DataSource = new List<FileInfo>(new DirectoryInfo(lvFolder).GetFiles(txtMask.Text, s).OrderByDescending(f => f.Name));
                    else
                        GridOrigin.DataSource = new List<FileInfo>(new DirectoryInfo(lvFolder).GetFiles(txtMask.Text, s).OrderBy(f => f.Name));
                if (strColumnName == "Attributes")
                    if (strSortOrder == SortOrder.Descending)
                        GridOrigin.DataSource = new List<FileInfo>(new DirectoryInfo(lvFolder).GetFiles(txtMask.Text, s).OrderByDescending(f => f.Attributes));
                    else
                        GridOrigin.DataSource = new List<FileInfo>(new DirectoryInfo(lvFolder).GetFiles(txtMask.Text, s).OrderBy(f => f.Attributes));
                if (strColumnName == "CreationTime")
                    if (strSortOrder == SortOrder.Descending)
                        GridOrigin.DataSource = new List<FileInfo>(new DirectoryInfo(lvFolder).GetFiles(txtMask.Text, s).OrderByDescending(f => f.CreationTime));
                    else
                        GridOrigin.DataSource = new List<FileInfo>(new DirectoryInfo(lvFolder).GetFiles(txtMask.Text, s).OrderBy(f => f.CreationTime));
                if (strColumnName == "CreationTimeUtc")
                    if (strSortOrder == SortOrder.Descending)
                        GridOrigin.DataSource = new List<FileInfo>(new DirectoryInfo(lvFolder).GetFiles(txtMask.Text, s).OrderByDescending(f => f.CreationTimeUtc));
                    else
                        GridOrigin.DataSource = new List<FileInfo>(new DirectoryInfo(lvFolder).GetFiles(txtMask.Text, s).OrderBy(f => f.CreationTimeUtc));
                if (strColumnName == "Directory")
                    if (strSortOrder == SortOrder.Descending)
                        GridOrigin.DataSource = new List<FileInfo>(new DirectoryInfo(lvFolder).GetFiles(txtMask.Text, s).OrderByDescending(f => f.Directory));
                    else
                        GridOrigin.DataSource = new List<FileInfo>(new DirectoryInfo(lvFolder).GetFiles(txtMask.Text, s).OrderBy(f => f.Directory));
                if (strColumnName == "DirectoryName")
                    if (strSortOrder == SortOrder.Descending)
                        GridOrigin.DataSource = new List<FileInfo>(new DirectoryInfo(lvFolder).GetFiles(txtMask.Text, s).OrderByDescending(f => f.DirectoryName));
                    else
                        GridOrigin.DataSource = new List<FileInfo>(new DirectoryInfo(lvFolder).GetFiles(txtMask.Text, s).OrderBy(f => f.DirectoryName));
                if (strColumnName == "Exists")
                    if (strSortOrder == SortOrder.Descending)
                        GridOrigin.DataSource = new List<FileInfo>(new DirectoryInfo(lvFolder).GetFiles(txtMask.Text, s).OrderByDescending(f => f.Exists));
                    else
                        GridOrigin.DataSource = new List<FileInfo>(new DirectoryInfo(lvFolder).GetFiles(txtMask.Text, s).OrderBy(f => f.Exists));
                if (strColumnName == "Extension")
                    if (strSortOrder == SortOrder.Descending)
                        GridOrigin.DataSource = new List<FileInfo>(new DirectoryInfo(lvFolder).GetFiles(txtMask.Text, s).OrderByDescending(f => f.Extension));
                    else
                        GridOrigin.DataSource = new List<FileInfo>(new DirectoryInfo(lvFolder).GetFiles(txtMask.Text, s).OrderBy(f => f.Extension));
                if (strColumnName == "FullName")
                    if (strSortOrder == SortOrder.Descending)
                        GridOrigin.DataSource = new List<FileInfo>(new DirectoryInfo(lvFolder).GetFiles(txtMask.Text, s).OrderByDescending(f => f.FullName));
                    else
                        GridOrigin.DataSource = new List<FileInfo>(new DirectoryInfo(lvFolder).GetFiles(txtMask.Text, s).OrderBy(f => f.FullName));
                if (strColumnName == "IsReadOnly")
                    if (strSortOrder == SortOrder.Descending)
                        GridOrigin.DataSource = new List<FileInfo>(new DirectoryInfo(lvFolder).GetFiles(txtMask.Text, s).OrderByDescending(f => f.IsReadOnly));
                    else
                        GridOrigin.DataSource = new List<FileInfo>(new DirectoryInfo(lvFolder).GetFiles(txtMask.Text, s).OrderBy(f => f.IsReadOnly));
                if (strColumnName == "LastAccessTime")
                    if (strSortOrder == SortOrder.Descending)
                        GridOrigin.DataSource = new List<FileInfo>(new DirectoryInfo(lvFolder).GetFiles(txtMask.Text, s).OrderByDescending(f => f.LastAccessTime));
                    else
                        GridOrigin.DataSource = new List<FileInfo>(new DirectoryInfo(lvFolder).GetFiles(txtMask.Text, s).OrderBy(f => f.LastAccessTime));
                if (strColumnName == "LastAccessTimeUtc")
                    if (strSortOrder == SortOrder.Descending)
                        GridOrigin.DataSource = new List<FileInfo>(new DirectoryInfo(lvFolder).GetFiles(txtMask.Text, s).OrderByDescending(f => f.LastAccessTimeUtc));
                    else
                        GridOrigin.DataSource = new List<FileInfo>(new DirectoryInfo(lvFolder).GetFiles(txtMask.Text, s).OrderBy(f => f.LastAccessTimeUtc));
                if (strColumnName == "Length")
                    if (strSortOrder == SortOrder.Descending)
                        GridOrigin.DataSource = new List<FileInfo>(new DirectoryInfo(lvFolder).GetFiles(txtMask.Text, s).OrderByDescending(f => f.Length));
                    else
                        GridOrigin.DataSource = new List<FileInfo>(new DirectoryInfo(lvFolder).GetFiles(txtMask.Text, s).OrderBy(f => f.Length));
                if (strColumnName == "LastWriteTime")
                    if (strSortOrder == SortOrder.Descending)
                        GridOrigin.DataSource = new List<FileInfo>(new DirectoryInfo(lvFolder).GetFiles(txtMask.Text, s).OrderByDescending(f => f.LastWriteTime));
                    else
                        GridOrigin.DataSource = new List<FileInfo>(new DirectoryInfo(lvFolder).GetFiles(txtMask.Text, s).OrderBy(f => f.LastWriteTime));
                if (strColumnName == "LastWriteTimeUtc")
                    if (strSortOrder == SortOrder.Descending)
                        GridOrigin.DataSource = new List<FileInfo>(new DirectoryInfo(lvFolder).GetFiles(txtMask.Text, s).OrderByDescending(f => f.LastWriteTimeUtc));
                    else
                        GridOrigin.DataSource = new List<FileInfo>(new DirectoryInfo(lvFolder).GetFiles(txtMask.Text, s).OrderBy(f => f.LastWriteTimeUtc));

                GridOrigin.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = strSortOrder;
            }
            catch (Exception ex) { lvLogger.Error(ex); lbMessage.Text = ex.Message; }
            finally
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStartSearch(object sender, EventArgs e)
        {
            lvLogger.DebugFormat(UllisPrograms.FunctionLib.Helper.ProgramStack);

            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

                GridClear(GridOrigin);

                GridOrigin.DataSource =
                    new List<FileInfo>(
                        new System.IO.DirectoryInfo(lvFolder).GetFiles(txtMask.Text, cbRootOnly.Checked ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories).OrderBy(f => f.DirectoryName));

                GridOrigin.Columns[0].HeaderCell.SortGlyphDirection = SortOrder.None;

                Application.DoEvents();

                if (GridOrigin.Rows.Count != 0)
                    GridOrigin.Rows[0].Selected = true;
            }
            catch (Exception ex) { lvLogger.Error(ex); lbMessage.Text = ex.Message; }
            finally
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMask_Click(object sender, EventArgs e)
        {
            lvLogger.DebugFormat(UllisPrograms.FunctionLib.Helper.ProgramStack);

            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

                if (string.IsNullOrEmpty(txtMask.Text))
                    txtMask.Text = "*.*";

                GridClear(GridOrigin);
            }
            catch (Exception ex) { lvLogger.Error(ex); }
            finally { System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridOriginSelectionChanged(object sender, EventArgs e)
        {
            lvLogger.DebugFormat(UllisPrograms.FunctionLib.Helper.ProgramStack);

            try
            {
                UpdateGridCandidates(sender, e);
            }
            catch (Exception ex) { lvLogger.Error(ex); MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            finally
            {
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateGridCandidates(object sender, EventArgs e)
        {
            lvLogger.DebugFormat(UllisPrograms.FunctionLib.Helper.ProgramStack);

            List<StructCandidates> list = new List<StructCandidates>();
            string NewFi = string.Empty;
            string NewNam = string.Empty;
            string NewExt = string.Empty;
            DateTime NewTi = DateTime.MinValue;
            System.IO.FileInfo fi = new FileInfo("dummy.txt");

            try
            {
                lbMarkedRows.Text = GridOrigin.SelectedRows.Count + " of " + GridOrigin.Rows.Count + " Rows marked";

                // Set cursor as hourglass
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                lbProgress.Visible = true;

                GridClear(GridCandidates);

                lbMessage.Text = string.Empty;
                lbSelection.Text = GetMode();

                decimal AutoNumberC = sel8nmStartNum.Value + (GridOrigin.SelectedRows.Count * sel8nmInc.Value) - sel8nmInc.Value;

                lbProgress.Maximum = GridOrigin.SelectedRows.Count;
                lbProgress.Minimum = 0;
                lbProgress.Value = 0;
                foreach (DataGridViewRow row in GridOrigin.SelectedRows)
                {
                    lbProgress.Value++;

                    fi = row.DataBoundItem as System.IO.FileInfo;
                    if (fi != null)
                    {
                        lbMessage.Text = UllisPrograms.FunctionLib.Helper.EllipsisString(fi.FullName, 20);
                        Application.DoEvents();

                        try
                        {
                            NewNam = Path.GetFileNameWithoutExtension(fi.FullName);
                            NewFi = NewNam + Path.GetExtension(fi.FullName);

                            if (MainTab.SelectedTab == tbString)
                            {
                                if (sel1rbActiv.Value !=0)
                                {
                                    NewNam = Path.GetFileNameWithoutExtension(fi.FullName).Insert((int)sel1nmInsertAt.Value, sel1txtInsert.Text);
                                    NewFi = NewNam + Path.GetExtension(fi.FullName);
                                }
                                else if (sel2rbActiv.Value != 0)
                                {
                                    NewNam = Path.GetFileNameWithoutExtension(fi.FullName).Substring((int)sel2nmExtractFrom.Value, (int)sel2nmExtractLength.Value);
                                    NewFi = NewNam + Path.GetExtension(fi.FullName);
                                }
                                else if (sel3rbActiv.Value != 0)
                                {
                                    NewNam = Path.GetFileNameWithoutExtension(fi.FullName).Trim();

                                    if (sel3cbBytes.SelectedItem.ToString() == csNum)
                                    {
                                        if (sel3cbPosition.SelectedItem.ToSaveString() == csTrailing)
                                        {
                                            NewNam = NewNam.NoTrailingNumbers();
                                            NewFi = NewNam + Path.GetExtension(fi.FullName);
                                        }
                                        else
                                            throw new System.ArgumentException("deleting leading numbers is not supported");
                                    }
                                    else
                                    {
                                        if (int.TryParse(sel3cbBytes.SelectedItem.ToSaveString().Replace(csBytes, "").Trim(), out int n))
                                        {
                                            if (sel3cbPosition.SelectedItem.ToSaveString() == csTrailing)
                                            {
                                                NewNam = NewNam.Substring(0, NewNam.Length - n);
                                                NewFi = NewNam + Path.GetExtension(fi.FullName);
                                            }
                                            else
                                            {
                                                NewNam = NewNam.Substring(n);
                                                NewFi = NewNam + Path.GetExtension(fi.FullName);
                                            }
                                        }
                                        else
                                            throw new System.ArgumentException("Parse numeric don't work");
                                    }
                                }
                                else if (sel4rbActiv.Value != 0)
                                {
                                    NewNam = Path.GetFileNameWithoutExtension(fi.FullName).Replace(sel4txtReplFrom.Text, sel4txtReplTo.Text);
                                    NewFi = NewNam + Path.GetExtension(fi.FullName);
                                }
                                else
                                    throw new System.ArgumentException("no Marker active");
                            }
                            else if (MainTab.SelectedTab == tbAutoNumber)
                            {
                                if (sel8rbActive.Value != 0)
                                {
                                    string tStr = Path.GetFileNameWithoutExtension(fi.FullName);
                                    if (sel8cbDelTrailingNum.Checked)
                                    {
                                        tStr = tStr.NoTrailingNumbers();
                                    }
                                    if (sel8cbPosition.SelectedIndex == 0)
                                        NewNam = AutoNumberC.ToString(sel8nmFormat.Text) + tStr;
                                    else if (sel8cbPosition.SelectedIndex == 1)
                                        NewNam = tStr + AutoNumberC.ToString(sel8nmFormat.Text);
                                    else if (sel8cbPosition.SelectedIndex == 2)
                                    {
                                        NewNam = AutoNumberC.ToString(sel8nmFormat.Text);
                                    }

                                    NewFi = NewNam + Path.GetExtension(fi.FullName);
                                    AutoNumberC -= sel8nmInc.Value;
                                }
                                else
                                    throw new System.ArgumentException("no Marker active");
                            }
                            else if (MainTab.SelectedTab == tbRegex)
                            {
                                if (sel6rbActiv.Value != 0)
                                {
                                    NewNam = Regex.Replace(Path.GetFileNameWithoutExtension(fi.FullName), sel6txtRegex.Text, sel6txtReplFrom.Text);
                                    NewFi = NewNam + Path.GetExtension(fi.FullName);
                                }
                                else
                                    throw new System.ArgumentException("no Marker active");
                            }
                            else if (MainTab.SelectedTab == tbExtensions)
                            {
                                if (sel7rbActive.Value != 0)
                                {
                                    NewExt = Path.GetExtension(fi.FullName).Replace(sel7txtRepl.Text, sel7txtReplWith.Text);
                                    NewFi = Path.GetFileNameWithoutExtension(fi.FullName) + NewExt;
                                }
                                else
                                    throw new System.ArgumentException("no Marker active");
                            }
                            else if (MainTab.SelectedTab == tbExif)
                            {
                                if (sel5rbActiv.Value != 0)
                                {
                                    lbSelection.Text = "Exif-Operation selected";
                                    IEnumerable<MetadataExtractor.Directory> directories = ImageMetadataReader.ReadMetadata(fi.FullName);

                                    //DumpAllTags(directories);

                                    NewTi = ParseTag(directories, "Exif IFD0", "Date/Time");
                                    if (NewTi == DateTime.MinValue)
                                    {
                                        NewTi = ParseTag(directories, "ICC Profile", "Profile Date/Time");
                                        if (NewTi == DateTime.MinValue)
                                        {
                                            NewTi = ParseTag(directories, "QuickTime Movie Header", "Created");
                                            if (NewTi == DateTime.MinValue)
                                            {
                                                NewTi = ParseTag(directories, "File", "File Modified Date");
                                                if (NewTi == DateTime.MinValue)
                                                {
                                                    NewTi = ParseTag(directories, "QuickTime Movie Header", "Created");
                                                    if (NewTi == DateTime.MinValue)
                                                    {
                                                        NewTi = ParseTag(directories, "Exif SubIFD", "Date/Time Original");
                                                        if (NewTi == DateTime.MinValue)
                                                        {
                                                            throw new System.ArgumentException("no valid EXIF-Data found");
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    NewFi = NewTi.ToString(sel5cbFormat.SelectedItem.ToSaveString()) + Path.GetExtension(fi.FullName);
                                    if (File.Exists(NewFi))
                                        throw new System.ArgumentException("new File already exists in FS");
                                    else
                                        foreach (DataGridViewRow testrow in GridCandidates.Rows)
                                        {
                                            StructCandidates s = testrow.DataBoundItem as StructCandidates;
                                            if (s.New.ToLower() == NewFi.ToLower())
                                                throw new System.ArgumentException("new File already exists in Grid");
                                        }
                                }
                                else
                                    throw new System.ArgumentException("no Marker active");
                            }
                            else
                                throw new System.ArgumentException("no Tab active????");
                        }
                        catch (Exception ex)
                        {
                            list.Insert(0, new StructCandidates(ex.Message, fi.FullName, fi.FullName, "!", " "));
                            continue;
                        }

                        list.Insert(0, new StructCandidates(NewFi, fi.DirectoryName + Path.DirectorySeparatorChar + NewFi, fi.FullName, " ", NewTi.ToString(sel5cbFormat.SelectedItem.ToSaveString())));
                    }
                }

                GridCandidates.DataSource = list;

                DataGridViewImageColumn dgv_pic = new DataGridViewImageColumn(false);
                dgv_pic.ImageLayout = DataGridViewImageCellLayout.Stretch;
                dgv_pic.DefaultCellStyle.BackColor = Color.Transparent;

                //Füge die Colums hinzu
                GridCandidates.Columns.Insert(0, dgv_pic);

                GridCandidates.AutoResizeColumns();
                GridCandidates.AutoResizeRows();

                int i = 0; int c = 0;
                foreach (DataGridViewRow row in GridCandidates.Rows)
                {
                    GridCandidates[0, i].Value = global::WinRename.Properties.Resources.LED_Gray_Off_128;

                    StructCandidates s = row.DataBoundItem as StructCandidates;
                    if (s != null)
                        if (s.Err == "!")
                        {
                            GridCandidates.Rows[i].Cells[1].Style.BackColor = Color.Red;
                            GridCandidates.Rows[i].Cells[1].Style.ForeColor = Color.White;
                            GridCandidates[0, i].Value = global::WinRename.Properties.Resources.LED_Red_On_128;
                        }
                        else
                        {
                            if (s.New != s.Old)
                            {
                                c++;
                                GridCandidates[0, i].Value = global::WinRename.Properties.Resources.LED_Green_On_128;
                            }
                        }

                    i++;
                }

                lbResultRows.Text = c + " Candidates";
            }
            catch (Exception ex) { lvLogger.Error(ex); MessageBox.Show(ex.Message); }
            finally
            {
                lbMessage.Text = string.Empty;
                lbProgress.Visible = false;
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;

                SaveValues();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRename_Click(object sender, EventArgs e)
        {
            lvLogger.DebugFormat(UllisPrograms.FunctionLib.Helper.ProgramStack);

            try
            {
                int i = 0;

                lbProgress.Visible = true;
                lbProgress.Maximum = GridCandidates.Rows.Count;
                lbProgress.Minimum = 0;
                lbProgress.Value = 0;
                foreach (DataGridViewRow row in GridCandidates.Rows)
                {
                    lbProgress.Value++;

                    StructCandidates s = row.DataBoundItem as StructCandidates;
                    lbMessage.Text = UllisPrograms.FunctionLib.Helper.EllipsisString(s.Old, 20);
                    Application.DoEvents();

                    if (s != null)
                        if (s.Err != "!")
                        {
                            if (s.Old.ToLower() != s.New.ToLower())
                                if (!File.Exists(s.New))
                                    System.IO.File.Move(s.Old, s.New);
                        }

                    i++;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message,"Error", MessageBoxButtons.OK,MessageBoxIcon.Error); }
            finally { lbProgress.Visible = false; lbMessage.Text = String.Empty; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridOrigin_DataSourceChanged(object sender, EventArgs e)
        {
            lvLogger.DebugFormat(UllisPrograms.FunctionLib.Helper.ProgramStack);

            try
            {
                // System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

                UpdateGridCandidates(sender, e);

                GridOrigin.AutoResizeColumns();
                GridOrigin.AutoResizeRows();
            }
            catch (Exception ex) { lvLogger.Error(ex); }
            finally { }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void GridOrigin_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            lvLogger.DebugFormat(UllisPrograms.FunctionLib.Helper.ProgramStack);

            try
            {
            }
            catch (Exception ex) { lvLogger.Error(ex); }
            finally { }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainSplitter_SplitterMoved(object sender, SplitterEventArgs e)
        {
            try
            {
                global::WinRename.Properties.Settings.Default.SplitterDistance = MainSplitter.SplitterDistance;
            }
            catch (Exception ex) { lvLogger.Error(ex); }
            finally { }
        }
        #endregion

        #region Private

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dir"></param>
        private void DumpAllTags(IEnumerable<MetadataExtractor.Directory> dir)
        {
#if (DEBUG)//************************************************** D E B U G - V E R S I O N ***********************************************************

            lvLogger.DebugFormat(UllisPrograms.FunctionLib.Helper.ProgramStack);

            try
            {
                foreach (var directory in dir)
                    foreach (var tag in directory.Tags)
                        Debug.Print($"{directory.Name} - {tag.Name} = {tag.Description}");
            }
            catch (Exception ex) { lvLogger.Error(ex); }
            finally { }

#endif
        }
        /// <summary>
        /// >
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="DirNam"></param>
        /// <param name="TagNam"></param>
        /// <returns></returns>
        private DateTime ParseTag(IEnumerable<MetadataExtractor.Directory> DirList, string DirNam, string TagNam)
        {
            lvLogger.DebugFormat(UllisPrograms.FunctionLib.Helper.ProgramStack);

            try
            {
                foreach (var directory in DirList)
                    if (directory.Name == DirNam)
                        foreach (var tag in directory.Tags)
                            if (tag.Name == TagNam)
                            {
                                CultureInfo provider = CultureInfo.CurrentCulture;
                                if (DateTime.TryParse(tag.Description, provider, DateTimeStyles.AssumeLocal, out var ti))
                                    return ti;
                                else if (DateTime.TryParseExact(tag.Description, "yyyy:MM:dd HH:mm:ss", provider, DateTimeStyles.AssumeLocal, out var ti1))
                                    return ti1;
                                else if (DateTime.TryParseExact(tag.Description.Substring(3), "MMM dd HH:mm:ss yyyy", provider, DateTimeStyles.AssumeLocal, out var ti2))
                                    //QuickTime Movie Header - Created = So Feb 06 15:16:00 2022
                                    return ti2;
                                else if (DateTimeOffset.TryParseExact(tag.Description.Substring(3), "MMM dd HH:mm:ss zzz yyyy", provider, DateTimeStyles.AssumeLocal, out var tiOffset))
                                    //Do Okt 23 18:15:13 +02:00 2014
                                    return tiOffset.LocalDateTime;
                                else { }
                            }

                return DateTime.MinValue;
            }
            catch (Exception ex) { lvLogger.Error(ex); return DateTime.MinValue; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private string GetMode()
        {
            lvLogger.DebugFormat(UllisPrograms.FunctionLib.Helper.ProgramStack);

            try
            {
                if (MainTab.SelectedTab == tbString)
                {
                    if (sel1rbActiv.Value != 0)
                        return "Insert-Operation selected";
                    else if (sel2rbActiv.Value != 0)
                        return "Substring-Operation selected";
                    else if (sel3rbActiv.Value != 0)
                        return "Leading/Trailing-Operation selected";
                    else if (sel4rbActiv.Value != 0)
                        return "Replace-Operation selected";
                    else
                        return "no operations";
                }
                else if (MainTab.SelectedTab == tbAutoNumber)
                {
                    if (sel8rbActive.Value != 0)
                        return "AutoNumber selected";
                    else
                        return "no operations";
                }
                else if (MainTab.SelectedTab == tbRegex)
                {
                    if (sel6rbActiv.Value != 0)
                        return "Regex-Operation selected";
                    else
                        return "no operations";
                }
                else if (MainTab.SelectedTab == tbExtensions)
                {
                    if (sel7rbActive.Value != 0)
                        return "Replace-Operation for extensions selected";
                    else
                        return "no operations";
                }
                else if (MainTab.SelectedTab == tbExif)
                {
                    if (sel5rbActiv.Value != 0)
                        return "Exif-Operation selected";
                    else
                        return "no operations";
                }
                else
                    return "no operations";
            }
            catch (Exception ex) { lvLogger.Error(ex); return "no operations"; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridClear(DataGridView Grid)
        {
            lvLogger.DebugFormat(UllisPrograms.FunctionLib.Helper.ProgramStack);

            try
            {
                Grid.DataSource = null;
                Grid.Rows.Clear(); // löscht alle Zeilen
                Grid.Columns.Clear(); // löscht alle Spalten
                Grid.ClearSelection();
                if (Grid.Name == GridOrigin.Name)
                    GridClear(GridCandidates);
            }
            catch (Exception ex) { lvLogger.Error(ex); }
            finally { }
        }
        /// <summary>
        /// Get the current sort order of the column and return it
        /// set the new SortOrder to the columns.
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <returns>SortOrder of the current column</returns>
        private SortOrder getSortOrder(int columnIndex)
        {
            lvLogger.DebugFormat(UllisPrograms.FunctionLib.Helper.ProgramStack);

            try
            {
                if (GridOrigin.Columns[columnIndex].HeaderCell.SortGlyphDirection == SortOrder.None ||
                    GridOrigin.Columns[columnIndex].HeaderCell.SortGlyphDirection == SortOrder.Descending)
                {
                    GridOrigin.Columns[columnIndex].HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                    return SortOrder.Ascending;
                }
                else
                {
                    GridOrigin.Columns[columnIndex].HeaderCell.SortGlyphDirection = SortOrder.Descending;
                    return SortOrder.Descending;
                }
            }
            catch (Exception ex) { lvLogger.Error(ex); return SortOrder.Ascending; }
            finally { }
        }
        /// <summary>
        /// 
        /// </summary>
        void GetStoredValues()
        {
            lvLogger.DebugFormat(UllisPrograms.FunctionLib.Helper.ProgramStack);

            try
            {
                MainSplitter.SplitterDistance = global::WinRename.Properties.Settings.Default.SplitterDistance;
                cbRootOnly.Checked = global::WinRename.Properties.Settings.Default.cbRootOnly;
                sel1nmInsertAt.Value = global::WinRename.Properties.Settings.Default.sel1nmInsertAt;
                sel1rbActiv.Value  = global::WinRename.Properties.Settings.Default.sel1rbActiv;
                sel1txtInsert.Text = global::WinRename.Properties.Settings.Default.sel1txtInsert;
                sel2nmExtractFrom.Value = global::WinRename.Properties.Settings.Default.sel2nmExtractFrom;
                sel2nmExtractLength.Value = global::WinRename.Properties.Settings.Default.sel2nmExtractLength;
                sel2rbActiv.Value = global::WinRename.Properties.Settings.Default.sel2rbActiv;
                sel3cbPosition.SelectedIndex = global::WinRename.Properties.Settings.Default.sel3cbPosition;
                sel3cbBytes.SelectedIndex = global::WinRename.Properties.Settings.Default.sel3cbBytes;
                sel3rbActiv.Value = global::WinRename.Properties.Settings.Default.sel3rbActiv;
                sel4rbActiv.Value = global::WinRename.Properties.Settings.Default.sel4rbActiv;
                sel4txtReplFrom.Text = global::WinRename.Properties.Settings.Default.sel4txtReplFrom;
                sel4txtReplTo.Text = global::WinRename.Properties.Settings.Default.sel4txtReplTo;
                sel5rbActiv.Value = global::WinRename.Properties.Settings.Default.sel5rbActiv;
                sel5cbFormat.SelectedIndex = global::WinRename.Properties.Settings.Default.sel5cbFormat;
                sel6rbActiv.Value = global::WinRename.Properties.Settings.Default.sel6rbActiv;
                sel6txtRegex.Text = global::WinRename.Properties.Settings.Default.sel6txtRegex;
                sel6txtReplFrom.Text = global::WinRename.Properties.Settings.Default.sel6txtReplFrom;
                sel7rbActive.Value = global::WinRename.Properties.Settings.Default.sel7rbActive;
                sel7txtRepl.Text = global::WinRename.Properties.Settings.Default.sel7txtRepl;
                sel7txtReplWith.Text = global::WinRename.Properties.Settings.Default.sel7txtReplWith;
                sel8rbActive.Value = global::WinRename.Properties.Settings.Default.sel8rbActive;
                sel8cbPosition.SelectedIndex = global::WinRename.Properties.Settings.Default.sel8cbPosition;
                sel8cbDelTrailingNum.Checked = global::WinRename.Properties.Settings.Default.sel8cbDelTrailingNum;
                sel8nmFormat.Text = global::WinRename.Properties.Settings.Default.sel8nmFormat;
                sel8nmInc.Value = global::WinRename.Properties.Settings.Default.sel8nmInc;
                sel8nmStartNum.Value = global::WinRename.Properties.Settings.Default.sel8nmStartNum;
                MainTab.SelectedIndex = global::WinRename.Properties.Settings.Default.MainTab;
                cbRootOnly.Checked = global::WinRename.Properties.Settings.Default.cbRootOnly;
                txtMask.Text = global::WinRename.Properties.Settings.Default.txtMask;
                this.Bounds = global::WinRename.Properties.Settings.Default.Bounds;
            }
            catch (Exception ex) { lvLogger.Error(ex);  }
            finally
            {
                if (string.IsNullOrEmpty(sel8nmFormat.Text))
                    sel8nmFormat.Text = "00000";
                if (sel8cbPosition.SelectedIndex == -1)
                    sel8cbPosition.SelectedIndex = 1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void SaveValues()
        {
            lvLogger.DebugFormat(UllisPrograms.FunctionLib.Helper.ProgramStack);

            try
            {
                global::WinRename.Properties.Settings.Default.Bounds = this.Bounds;
                global::WinRename.Properties.Settings.Default.cbRootOnly = cbRootOnly.Checked;
                global::WinRename.Properties.Settings.Default.sel1nmInsertAt = sel1nmInsertAt.Value;
                global::WinRename.Properties.Settings.Default.sel1rbActiv = sel1rbActiv.Value;
                global::WinRename.Properties.Settings.Default.sel1txtInsert = sel1txtInsert.Text;
                global::WinRename.Properties.Settings.Default.sel2nmExtractFrom = sel2nmExtractFrom.Value;
                global::WinRename.Properties.Settings.Default.sel2nmExtractLength = sel2nmExtractLength.Value;
                global::WinRename.Properties.Settings.Default.sel2rbActiv = sel2rbActiv.Value;
                global::WinRename.Properties.Settings.Default.sel3cbPosition = sel3cbPosition.SelectedIndex;
                global::WinRename.Properties.Settings.Default.sel3cbBytes = sel3cbBytes.SelectedIndex;
                global::WinRename.Properties.Settings.Default.sel3rbActiv = sel3rbActiv.Value;
                global::WinRename.Properties.Settings.Default.sel4rbActiv = sel4rbActiv.Value;
                global::WinRename.Properties.Settings.Default.sel4txtReplFrom = sel4txtReplFrom.Text;
                global::WinRename.Properties.Settings.Default.sel4txtReplTo = sel4txtReplTo.Text;
                global::WinRename.Properties.Settings.Default.sel5rbActiv = sel5rbActiv.Value;
                global::WinRename.Properties.Settings.Default.sel5cbFormat = sel5cbFormat.SelectedIndex;
                global::WinRename.Properties.Settings.Default.sel6rbActiv = sel6rbActiv.Value;
                global::WinRename.Properties.Settings.Default.sel6txtRegex = sel6txtRegex.Text;
                global::WinRename.Properties.Settings.Default.sel6txtReplFrom = sel6txtReplFrom.Text;
                global::WinRename.Properties.Settings.Default.sel7rbActive = sel7rbActive.Value;
                global::WinRename.Properties.Settings.Default.sel7txtRepl = sel7txtRepl.Text;
                global::WinRename.Properties.Settings.Default.sel7txtReplWith = sel7txtReplWith.Text;
                global::WinRename.Properties.Settings.Default.sel8rbActive = sel8rbActive.Value;
                global::WinRename.Properties.Settings.Default.sel8cbPosition = sel8cbPosition.SelectedIndex;
                global::WinRename.Properties.Settings.Default.sel8cbDelTrailingNum = sel8cbDelTrailingNum.Checked;
                global::WinRename.Properties.Settings.Default.sel8nmFormat = sel8nmFormat.Text;
                global::WinRename.Properties.Settings.Default.sel8nmInc = sel8nmInc.Value;
                global::WinRename.Properties.Settings.Default.sel8nmStartNum = sel8nmStartNum.Value;
                global::WinRename.Properties.Settings.Default.MainTab = MainTab.SelectedIndex;
                global::WinRename.Properties.Settings.Default.txtMask = txtMask.Text;
                global::WinRename.Properties.Settings.Default.Save();
            }
            catch (Exception ex) { lvLogger.Error(ex); }
            finally { }
        }

        /// <summary>
        /// 
        /// </summary>
        void SetUpdateGridCandidatesEvents()
        {
            lvLogger.DebugFormat(UllisPrograms.FunctionLib.Helper.ProgramStack);

            try
            {
                this.sel1rbActiv.ValueChanged += new System.EventHandler(this.UpdateGridCandidates);
                this.sel5cbFormat.SelectedIndexChanged += new System.EventHandler(this.UpdateGridCandidates);
                this.sel6rbActiv.ValueChanged += new System.EventHandler(this.UpdateGridCandidates);
                this.sel6txtReplFrom.TextChanged += new System.EventHandler(this.UpdateGridCandidates);
                this.sel6txtRegex.TextChanged += new System.EventHandler(this.UpdateGridCandidates);
                this.sel1txtInsert.TextChanged += new System.EventHandler(this.UpdateGridCandidates);
                this.sel1nmInsertAt.ValueChanged += new System.EventHandler(this.UpdateGridCandidates);
                this.sel2nmExtractLength.ValueChanged += new System.EventHandler(this.UpdateGridCandidates);
                this.sel2nmExtractFrom.ValueChanged += new System.EventHandler(this.UpdateGridCandidates);
                this.sel2rbActiv.ValueChanged += new System.EventHandler(this.UpdateGridCandidates);
                this.sel3cbBytes.SelectedIndexChanged += new System.EventHandler(this.UpdateGridCandidates);
                this.sel3rbActiv.ValueChanged += new System.EventHandler(this.UpdateGridCandidates);
                this.sel5rbActiv.ValueChanged += new System.EventHandler(this.UpdateGridCandidates);
                this.sel7txtRepl.TextChanged += new System.EventHandler(this.UpdateGridCandidates);
                this.sel7rbActive.ValueChanged += new System.EventHandler(this.UpdateGridCandidates);
                this.sel3cbPosition.SelectedIndexChanged += new System.EventHandler(this.UpdateGridCandidates);
                this.sel4rbActiv.ValueChanged += new System.EventHandler(this.UpdateGridCandidates);
                this.sel4txtReplFrom.TextChanged += new System.EventHandler(this.UpdateGridCandidates);
                this.sel4txtReplTo.TextChanged += new System.EventHandler(this.UpdateGridCandidates);
                this.sel8rbActive.ValueChanged += new System.EventHandler(this.UpdateGridCandidates);
                this.sel8cbPosition.SelectedIndexChanged += new System.EventHandler(this.UpdateGridCandidates);
                this.sel8nmFormat.TextChanged += new System.EventHandler(this.UpdateGridCandidates);
                this.sel8cbDelTrailingNum.CheckedChanged += new System.EventHandler(this.UpdateGridCandidates);
                this.sel8nmInc.ValueChanged += new System.EventHandler(this.UpdateGridCandidates);
                this.sel8nmStartNum.ValueChanged += new System.EventHandler(this.UpdateGridCandidates);
                this.MainTab.SelectedIndexChanged += new System.EventHandler(this.UpdateGridCandidates);
                this.GridOrigin.SelectionChanged += new System.EventHandler(this.GridOriginSelectionChanged);
                this.cbRootOnly.CheckedChanged += new System.EventHandler(this.UpdateGridCandidates);
                this.sel7txtReplWith.TextChanged += new System.EventHandler(this.UpdateGridCandidates);
            }
            catch (Exception ex) { lvLogger.Error(ex); }
            finally { }
        }

        #endregion
    }
}

