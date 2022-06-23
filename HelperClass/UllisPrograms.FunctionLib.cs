using log4net;
using log4net.Appender;
using log4net.Core;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using HWND = System.IntPtr;


namespace UllisPrograms
{
    namespace FunctionLib
    {
        public static class ObjectExtension
        {
            private static readonly ILog lvLogger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

            public static string ToSaveString(this Object obj)
            {
                if (obj == null)
                    return string.Empty;

                return obj.ToString().Trim();
            }
            public static double ToSaveDouble(this Object obj)
            {
                return Helper.doubleParse(obj);
            }
            public static double ToSaveInt(this Object obj)
            {
                return Helper.int32Parse(obj);
            }
        }
        // Extension methods must be defined in a static class.
        public static class StringExtension
        {
            // This is the extension method.
            // The first parameter takes the "this" modifier
            // and specifies the type for which the method is defined.
            public static IEnumerable<string> GraphemeClusters(this string s)
            {
                var enumerator = StringInfo.GetTextElementEnumerator(s);
                while (enumerator.MoveNext())
                {
                    yield return (string)enumerator.Current;
                }
            }
            public static string ReverseGraphemeClusters(this string s)
            {
                return string.Join("", s.GraphemeClusters().Reverse().ToArray());
            }
            public static string ConvertUmlaute(this string s)
            {
                string old = s;
                old = old.Replace("ä", "ae");
                old = old.Replace("ö", "oe");
                old = old.Replace("ü", "ue");
                old = old.Replace("ß", "ss");
                return (old);
            }
            public static int WordCount(this string str)
            {
                return str.Split(new char[] { ' ', '.', '?', ',', ';', ':' }, StringSplitOptions.RemoveEmptyEntries).Length;
            }
            public static string NoLeadingZeros(this string str)
            {
                return str.TrimStart('0').Trim(); ;
            }
            public static string NoTrailingNumbers(this string str)
            {
                string NewNam = str;
                while (Int32.TryParse(NewNam.LastBytes(1), out var n))
                {
                    NewNam = NewNam.Substring(0, NewNam.Length - 1);
                }

                return NewNam;
            }
            public static string LeadingZeros(this string str, int cnt)
            {
                return str.Trim().PadLeft(cnt, '0');
            }
            public static string ReplaceAll(this string str, string v, string n)
            {
                string w = str;
                while (w.IndexOf(v) > -1)
                {
                    w = w.Replace(v, n);
                }

                return w;
            }
            public static string ReplaceAll(this string str, char[] v, string n)
            {
                string w = str;
                for (int i = 0; i < v.Length; i++)
                {
                    w = ReplaceAll(w, v[i].ToString(), n);
                }
                return w;
            }
            public static string CondenseGaps(this string str)
            {
                return str.ReplaceAll(Const.Constants.SPACE2, Const.Constants.SPACE);
            }
            public static string Condense(this string str)
            {
                return str.ReplaceAll(Const.Constants.SPACE, string.Empty);
            }
            public static string Proper(this string str)
            {
                string t = str.ToSaveString();
                t = t.TrimStart('+');
                t.CondenseGaps();
                t.ReplaceAll("träger", "tr.");
                t.ReplaceAll("Hybrid", "Hyb.");
                t.ReplaceAll("Transferpresse", "Transferpr.");
                t.ReplaceAll("Führungsgelenk", "Fhrgsgelenk");
                t.ReplaceAll("Komplett fertigen", "kompl.fert.");
                return t;
            }
            public static string RegexContent(this string str)
            {
                return str.Replace('#', '%').Trim().ToUpper();
            }
            public static string LastBytes(this string str, int n)
            {
                if (string.IsNullOrWhiteSpace(str))
                    return string.Empty;

                if (str.Length <= n)
                    n = str.Length;

                return str.Substring(str.Length - n, n);
            }
            public static string ReplaceAllCtrl(this string str)
            {
                string tmpStr = str;

                tmpStr = tmpStr.Replace("\t", string.Empty);
                tmpStr = tmpStr.Replace("\n", string.Empty);
                tmpStr = tmpStr.Replace("\r", string.Empty);
                tmpStr = tmpStr.Replace(Environment.NewLine, string.Empty);
                tmpStr = tmpStr.Trim();

                return tmpStr;
            }
            public static string SplitAtColon(this string str, int pos)
            {
                if (string.IsNullOrWhiteSpace(str))
                    return string.Empty;
                if (str.IndexOf(':') < 0)
                    return str;

                return str.Split(':')[pos].Trim();
            }
            public static string SplitAtCRLF(this string str)
            {
                if (string.IsNullOrWhiteSpace(str))
                    return string.Empty;

                int cPos = str.IndexOf(Environment.NewLine);

                if (cPos == -1)
                    return str.Trim();
                else
                    return str.Substring(0, cPos).Trim();
            }
            public static bool IsNumeric(this string str)
            {
                if (string.IsNullOrWhiteSpace(str))
                    return false;
                else
                    return int.TryParse(str, out int n);
            }
            public static string ProperScan(this string str)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(str))
                        return string.Empty;

                    string message = str;

                    if (message.IndexOfAny(new char[] { '\r', '\n' }) > 0)
                    {
                        if (message[message.Length - 1] == '\r')
                        {
                            message = message.Substring(0, message.Length - 1);
                        }
                        if (message[message.Length - 1] == '\n')
                        {
                            message = message.Substring(0, message.Length - 1);
                        }
                    }

                    if (message.Length > 0)
                        return message;
                    else
                        return string.Empty;
                }
                catch { return string.Empty; }
            }
            public static string AddOneLetter(this string str, string z)
            {
                string tmp = str.Trim();

                if (!tmp.EndsWith(z))
                {
                    tmp += z;
                }

                return tmp;
            }
            public static string ReplaceSpecChar(this string str)
            {
                string Fullstring = str;
                Fullstring.ReplaceAll("&#23", "#");
                Fullstring.ReplaceAll("&#7C", "|");
                Fullstring.ReplaceAll("&#7B", "{");
                Fullstring.ReplaceAll("&#7D", "}");
                Fullstring.ReplaceAll("&#7E", "~");
                return Fullstring;
            }
            public static string Reverse(this string str)
            {
                string nest;
                char[] c = new char[str.Length];
                for (int i = 0; i < str.Length; i++)
                {
                    c[i] = str[str.Length - 1 - i];
                }

                nest = new string(c);
                return nest;
            }
            public static string CutAtBackSlash(this string str)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(str))
                    {
                        return string.Empty;
                    }

                    int Pos = str.IndexOf("\r\n");
                    if (Pos < 0)
                    {
                        return str;
                    }

                    return str.Substring(0, Pos);
                }
                catch { return string.Empty; }
            }
            public static string InsertHTTPChars(this string str)
            {
                if (string.IsNullOrWhiteSpace(str))
                {
                    return str;
                }

                string st = str;
                st = st.ReplaceAll("#", "&#23");
                st = st.ReplaceAll("|", "&#7C");
                st = st.ReplaceAll("{", "&#7B");
                st = st.ReplaceAll("}", "&#7D");
                st = st.ReplaceAll("~", "&#7E");
                return st;
            }
            public static string RemoveHTTPChars(this string str)
            {
                if (string.IsNullOrWhiteSpace(str))
                {
                    return str;
                }

                string st = str;
                st = st.ReplaceAll("&#23", "#");
                st = st.ReplaceAll("&#7C", "|");
                st = st.ReplaceAll("&#7B", "{");
                st = st.ReplaceAll("&#7D", "}");
                st = st.ReplaceAll("&#7E", "~");
                return st;
            }
            public static string ReplaceSemicolonByPipe(this string str)
            {
                return str.ReplaceAll(";", "|");
            }
            public static string CapitalizeWords(this string str)
            {
                if (string.IsNullOrWhiteSpace(str))
                {
                    return string.Empty;
                }

                string nStr = string.Empty;
                string[] StrArray = str.Split(' ');
                foreach (string s in StrArray)
                {
                    if (s.ToUpper().StartsWith("NCV3") ||
                        s.ToUpper().StartsWith("NLA") ||
                        s.ToUpper().StartsWith("EUCD") ||
                        s.ToUpper().StartsWith("BMW") ||
                        s.ToUpper().StartsWith("UKL") ||
                        s.ToUpper().StartsWith("PQ") ||
                        s.ToUpper().StartsWith("VW") ||
                        s.ToUpper().StartsWith("LAD-"))
                    {
                        nStr += s.ToUpper() + Const.Constants.SPACE;
                    }
                    else
                    {
                        nStr += CapitalizeString(s) + Const.Constants.SPACE;
                    }
                }

                return nStr.Trim();
            }
            public static string CapitalizeString(this string str)
            {
                string h = str.Trim();

                if (string.IsNullOrWhiteSpace(h))
                {
                    return string.Empty;
                }

                if (h.Length == 1)
                {
                    return str.ToUpper();
                }

                return h.Substring(0, 1).ToUpper() + h.Substring(1).ToLower();
            }
            public static string RemoveIllegalChars(this string str)
            {
                var tmp = new StringBuilder();

                for (int v = 0; v < str.Length; v++)
                    for (int i = 0; i < 128; i++)
                        if (char.IsLetterOrDigit((char)i))
                            if (str.Substring(v, 1) == Convert.ToString((char)i))
                                tmp.Append((char)i);

                return tmp.ToString();
            }
            public static string[] SplitAtFirst(this string str, string First)
            {
                string[] TmpArray = new string[2];
                TmpArray[0] = "";
                TmpArray[1] = "";
                int i = str.IndexOf(First);
                if (i > -1)
                {
                    try { TmpArray[0] = str.Substring(0, i); }
                    catch { }
                    try { TmpArray[1] = str.Substring(i + 1); }
                    catch { }
                }

                return TmpArray;
            }
            public static string ToFuzzyByteString(this string str, double bytes)
            {
                double s = Convert.ToDouble(bytes);
                string[] format = new string[]
                  {
                      "{0} bytes", "{0} KB",
                      "{0} MB", "{0} GB", "{0} TB", "{0} PB", "{0} EB"
                  };

                int i = 0;

                while (i < format.Length && s >= 1024)
                {
                    s = (long)(100 * s / 1024) / 100.0;
                    i++;
                }
                return string.Format(format[i], s);
            }
        }

        public static class ILogExtentions
        {
            private static readonly ILog lvLogger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

            public static void Trace(this ILog log, string message, Exception exception)
            {
                log.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, log4net.Core.Level.Trace, message, exception);
            }
            public static void Trace(this ILog log, string message)
            {
                log.Trace(message, null);
            }
            public static void Verbose(this ILog log, string message, Exception exception)
            {
                log.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, log4net.Core.Level.Verbose, message, exception);
            }
            public static void Verbose(this ILog log, string message)
            {
                log.Verbose(message, null);
            }
            public static void Notice(this ILog log, string message, Exception exception)
            {
                log.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, log4net.Core.Level.Notice, message, exception);
            }
            public static void Notice(this ILog log, string message)
            {
                log.Notice(message, null);
            }
            public static void Emergency(this ILog log, string message, Exception exception)
            {
                log.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, log4net.Core.Level.Emergency, message, exception);
            }
            public static void Emergency(this ILog log, string message)
            {
                log.Emergency(message, null);
            }
        }

        /// <summary>
        /// *************************************************************************************************************************
        /// *************************************************************************************************************************
        /// *************************************************************************************************************************
        /// *************************************************************************************************************************
        /// </summary>
        public static class Helper
        {
            private static readonly byte[] IV = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
            private const int BlockSize = 128;
            private static readonly log4net.ILog lvLogger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            #region Files

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public static string ExePath
            {
                get
                {
                    lvLogger.Verbose(Helper.ProgramStack);

                    try
                    {
                        Uri ExeUri = new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase));
                        string ExeFolderPath = ExeUri.LocalPath;
                        if (!ExeFolderPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
                        {
                            ExeFolderPath += Path.DirectorySeparatorChar;
                        }

                        return ExeFolderPath;
                    }
                    catch (Exception ex)
                    {
                        lvLogger.Error(ex.Message, ex);
                        return null;
                    }
                }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="lvFile"></param>
            /// <param name="lvDir"></param>
            public static void DelFile(string lvFile, string lvDir)
            {
                lvLogger.Verbose(Helper.ProgramStack);

                try
                {
                    if (lvDir.EndsWith(@"\"))
                    {
                        lvDir = lvDir.Substring(0, lvDir.Length - 1);
                    }

                    foreach (string f in Directory.GetFiles(lvDir, lvFile))
                    {
                        File.Delete(f);
                    }
                }
                catch (Exception ex)
                {
                    lvLogger.Error(ex.Message, ex);
                }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="File"></param>
            /// <param name="Level"></param>
            /// <param name="cAppendToFile"></param>
            /// <returns></returns>
            public static FileAppender GetFileAppender(string File, log4net.Core.Level Level, bool cAppendToFile)
            {
                RollingFileAppender lAppender = new RollingFileAppender
                {
                    File = File,
                    AppendToFile = cAppendToFile,
                    MaxSizeRollBackups = 99,
                    MaximumFileSize = "16MB",
                    RollingStyle = RollingFileAppender.RollingMode.Once | RollingFileAppender.RollingMode.Size,
                    Threshold = Level,
                    Name = "File",
                    Layout = new log4net.Layout.PatternLayout(Const.Constants.LOGLAYOUT)
                };
                lAppender.ActivateOptions();

                return lAppender;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="ExecutingAssembly"></param>
            public static void CreateAutoStartEntry(Assembly ExecutingAssembly)
            {
                lvLogger.Verbose(Helper.ProgramStack);

                try
                {
                    string StartMenueEntry = Helper.StartMenuEntry(ExecutingAssembly, out bool MenuExists);
                    if (!MenuExists)
                        return;

                    string AutoStartEntry = GetAutoStartEntry(ExecutingAssembly);
                    if (string.IsNullOrWhiteSpace(AutoStartEntry))
                        return;

                    File.Copy(StartMenueEntry, AutoStartEntry);
                }
                catch (Exception ex)
                {
                    lvLogger.Error(ex.Message, ex);
                }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="ExecutingAssembly"></param>
            /// <returns></returns>
            public static string GetAutoStartEntry(Assembly ExecutingAssembly)
            {
                lvLogger.Verbose(Helper.ProgramStack);

                try
                {
                    string StartMenueEntry = Helper.StartMenuEntry(ExecutingAssembly, out bool MenuExists);
                    if (!MenuExists)
                        return string.Empty;

                    string AutoStartFolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
                    if (!Directory.Exists(AutoStartFolder))
                        return string.Empty;
                    string AutoCommonStartFolder = Environment.GetFolderPath(Environment.SpecialFolder.CommonStartup);
                    if (!Directory.Exists(AutoCommonStartFolder))
                        return string.Empty;

                    string AutoStartEntry = Path.Combine(new string[2] { Environment.GetFolderPath(Environment.SpecialFolder.Startup), Path.GetFileNameWithoutExtension(ExecutingAssembly.Location) + Const.Constants.APPREFMS });
                    AutoStartEntry = AutoStartEntry.TrimEnd(Path.PathSeparator);
                    if (File.Exists(AutoStartEntry))
                        return AutoStartEntry;

                    string AutoCommonStartEntry = Path.Combine(new string[2] { Environment.GetFolderPath(Environment.SpecialFolder.CommonStartup), Path.GetFileNameWithoutExtension(ExecutingAssembly.Location) + Const.Constants.APPREFMS });
                    AutoStartEntry = AutoCommonStartEntry.TrimEnd(Path.PathSeparator);
                    if (File.Exists(AutoCommonStartEntry))
                        return AutoCommonStartEntry;

                    return string.Empty;
                }
                catch (Exception ex)
                {
                    lvLogger.Error(ex.Message, ex);
                    return string.Empty;
                }
            }
            /// <summary>
            /// This method will test that two dictionaries contain the same keys with the same values
            /// (assuming that both dictionaries use the same IEqualityComparer<TKey> implementation).
            /// </summary>
            /// <typeparam name="TKey"></typeparam>
            /// <typeparam name="TValue"></typeparam>
            /// <param name="dict1"></param>
            /// <param name="dict2"></param>
            /// <returns></returns>
            public static bool CompareX<TKey, TValue>(
                SortedDictionary<TKey, TValue> dict1, SortedDictionary<TKey, TValue> dict2)
            {
                if (dict1 == dict2) return true;
                if ((dict1 == null) || (dict2 == null)) return false;
                if (dict1.Count != dict2.Count) return false;

                var valueComparer = EqualityComparer<TValue>.Default;

                foreach (var kvp in dict1)
                {
                    if (!dict2.TryGetValue(kvp.Key, out TValue value2)) return false;
                    if (!valueComparer.Equals(kvp.Value, value2)) return false;
                }
                return true;
            }
            public static bool CompareX<TKey, TValue>(
                Dictionary<TKey, TValue> dict1, Dictionary<TKey, TValue> dict2)
            {
                if (dict1 == dict2) return true;
                if ((dict1 == null) || (dict2 == null)) return false;
                if (dict1.Count != dict2.Count) return false;

                var valueComparer = EqualityComparer<TValue>.Default;

                foreach (var kvp in dict1)
                {
                    if (!dict2.TryGetValue(kvp.Key, out TValue value2)) return false;
                    if (!valueComparer.Equals(kvp.Value, value2)) return false;
                }
                return true;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="sourceDirName"></param>
            /// <param name="destDirName"></param>
            /// <param name="copySubDirs"></param>
            public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
            { DirectoryCopy(sourceDirName, destDirName, copySubDirs, "^^^°°°^^^^"); }
            public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs, string Exclude)
            {
                lvLogger.Verbose(Helper.ProgramStack);

                try
                {
                    // Get the subdirectories for the specified directory.
                    DirectoryInfo dir = new DirectoryInfo(sourceDirName);
                    DirectoryInfo[] dirs = dir.GetDirectories();

                    if (!dir.Exists)
                    {
                        throw new DirectoryNotFoundException(
                            "Source directory does not exist or could not be found: "
                            + sourceDirName);
                    }

                    if (dir.ToString().IndexOf(Exclude) > -1)
                    {
                        lvLogger.InfoFormat("skip \"{0}\"", dir.ToString());
                        return;
                    }

                    // If the destination directory doesn't exist, create it.
                    if (!Directory.Exists(destDirName))
                    {
                        Directory.CreateDirectory(destDirName);
                    }

                    // Get the files in the directory and copy them to the new location.
                    FileInfo[] files = dir.GetFiles();
                    foreach (FileInfo file in files)
                    {
                        string destpath = Path.Combine(destDirName, file.Name);
                        lvLogger.Info(string.Format("deleting \"{0}\"", destpath));
                        try { File.Delete(destpath); }
                        catch (Exception exp) { lvLogger.Error(exp.ToString(), exp); }
                        System.Threading.Thread.Sleep(500);
                        lvLogger.Info(string.Format("copying \"{0}\" from \"{1}\" to \"{2}\"", file.Name, file.DirectoryName, destpath));
                        try { file.CopyTo(destpath, true); }
                        catch (Exception exp) { lvLogger.Error(exp.ToString(), exp); }
                    }

                    // If copying subdirectories, copy them and their contents to new location.
                    if (copySubDirs)
                    {
                        foreach (DirectoryInfo subdir in dirs)
                        {
                            string destpath = Path.Combine(destDirName, subdir.Name);
                            DirectoryCopy(subdir.FullName, destpath, copySubDirs, Exclude);
                        }
                    }
                }
                catch { }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="path"></param>
            /// <param name="newName"></param>
            public static void RenameFile(string path, string newName)
            {
                lvLogger.Verbose(Helper.ProgramStack);

                FileInfo var = new FileInfo(path);
                File.Move(path, var.Directory + newName);
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public static string GetAndCreateLogFolder(string MainTitleShort)
            {
                lvLogger.Verbose(Helper.ProgramStack);

                try
                {
                   string iPath = Path.Combine( Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Temp", MainTitleShort, "Logging");

                    if (System.IO.Directory.Exists(iPath))
                    {
                      return  Path.Combine(iPath , MainTitleShort + ".log");
                    }
                    else
                    {
                        System.IO.Directory.CreateDirectory(iPath);
                        if (System.IO.Directory.Exists(iPath))
                        {
                          return  Path.Combine(iPath, MainTitleShort + ".log");
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                catch (Exception ex) { lvLogger.Error(ex.Message, ex); return null; }
            }

            #endregion

            #region Pictures

            /// <summary>
            /// 
            /// </summary>
            /// <param name="bmp"></param>
            /// <returns></returns>
            public static Bitmap RemoveBorder(Bitmap bmp)
            {
                lvLogger.Verbose(Helper.ProgramStack);

                int w = bmp.Width;
                int h = bmp.Height;

                Func<int, bool> allWhiteRow = row =>
                {
                    for (int i = 0; i < w; ++i)
                        if (bmp.GetPixel(i, row).R != 255)
                            return false;
                    return true;
                };

                Func<int, bool> allWhiteColumn = col =>
                {
                    for (int i = 0; i < h; ++i)
                        if (bmp.GetPixel(col, i).R != 255)
                            return false;
                    return true;
                };

                int topmost = 0;
                for (int row = 0; row < h; ++row)
                {
                    if (allWhiteRow(row))
                        topmost = row;
                    else break;
                }

                int bottommost = 0;
                for (int row = h - 1; row >= 0; --row)
                {
                    if (allWhiteRow(row))
                        bottommost = row;
                    else break;
                }

                int leftmost = 0, rightmost = 0;
                for (int col = 0; col < w; ++col)
                {
                    if (allWhiteColumn(col))
                        leftmost = col;
                    else
                        break;
                }

                for (int col = w - 1; col >= 0; --col)
                {
                    if (allWhiteColumn(col))
                        rightmost = col;
                    else
                        break;
                }

                if (rightmost == 0) rightmost = w; // As reached left
                if (bottommost == 0) bottommost = h; // As reached top.

                int croppedWidth = rightmost - leftmost;
                int croppedHeight = bottommost - topmost;

                if (croppedWidth == 0) // No border on left or right
                {
                    leftmost = 0;
                    croppedWidth = w;
                }

                if (croppedHeight == 0) // No border on top or bottom
                {
                    topmost = 0;
                    croppedHeight = h;
                }

                try
                {
                    var target = new Bitmap(croppedWidth, croppedHeight);
                    using (Graphics g = Graphics.FromImage(target))
                    {
                        g.DrawImage(bmp,
                          new RectangleF(0, 0, croppedWidth, croppedHeight),
                          new RectangleF(leftmost, topmost, croppedWidth, croppedHeight),
                          GraphicsUnit.Pixel);
                    }
                    return target;
                }
                catch (Exception ex)
                {
                    throw new Exception(
                      string.Format("Values are topmost={0} btm={1} left={2} right={3} croppedWidth={4} croppedHeight={5}", topmost, bottommost, leftmost, rightmost, croppedWidth, croppedHeight),
                      ex);
                }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="original"></param>
            /// <returns></returns>
            public static Bitmap MakeGray(Image original)
            {
                lvLogger.Verbose(Helper.ProgramStack);

                //create a blank bitmap the same size as original
                Bitmap newBitmap = new Bitmap(original.Width, original.Height);

                //get a graphics object from the new image
                Graphics g = Graphics.FromImage(newBitmap);

                //create the grayscale ColorMatrix
                ColorMatrix colorMatrix = new ColorMatrix(
                new float[][]
                  {
                     new float[] {.3f, .3f, .3f, 0, 0},
                     new float[] {.59f, .59f, .59f, 0, 0},
                     new float[] {.11f, .11f, .11f, 0, 0},
                     new float[] {0, 0, 0, 1, 0},
                     new float[] {0, 0, 0, 0, 1}
                  });

                //create some image attributes
                ImageAttributes attributes = new ImageAttributes();

                //set the color matrix attribute
                attributes.SetColorMatrix(colorMatrix);

                //draw the original image on the new image
                //using the grayscale color matrix
                g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
                   0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

                //dispose the Graphics object
                g.Dispose();
                return newBitmap;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="OrgImage"></param>
            /// <param name="brightness"></param>
            /// <param name="contrast"></param>
            /// <param name="gamma"></param>
            /// <param name="Gray"></param>
            /// <returns></returns>
            public static Bitmap AdjustPicture(this Image OrgImage, float brightness, float contrast, float gamma, bool Gray)
            {
                lvLogger.Verbose(Helper.ProgramStack);

                Bitmap originalImage = new Bitmap(OrgImage);
                Bitmap adjustedImage = new Bitmap(originalImage.Width, originalImage.Height);
                //float brightness = 1.0f; // no change in brightness
                //float contrast = 2.0f; // twice the contrast
                //float gamma = 1.0f; // no change in gamma

                float adjustedBrightness = brightness - 1.0f;
                // create matrix that will brighten and contrast the image
                float[][] ptsArray ={
                new float[] {contrast, 0, 0, 0, 0}, // scale red
                new float[] {0, contrast, 0, 0, 0}, // scale green
                new float[] {0, 0, contrast, 0, 0}, // scale blue
                new float[] {0, 0, 0, 1.0f, 0}, // don't scale alpha
                new float[] {adjustedBrightness, adjustedBrightness, adjustedBrightness, 0, 1}};

                ImageAttributes imageAttributes = new ImageAttributes();
                imageAttributes.ClearColorMatrix();
                imageAttributes.SetColorMatrix(new ColorMatrix(ptsArray), ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                imageAttributes.SetGamma(gamma, ColorAdjustType.Bitmap);
                Graphics g = Graphics.FromImage(adjustedImage);
                g.DrawImage(Gray ? MakeGray(originalImage) : originalImage, new Rectangle(0, 0, adjustedImage.Width, adjustedImage.Height)
                    , 0, 0, originalImage.Width, originalImage.Height,
                    GraphicsUnit.Pixel, imageAttributes);

                //dispose the Graphics object
                g.Dispose();

                return adjustedImage;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="img"></param>
            /// <param name="targetWidth"></param>
            /// <param name="targetHeight"></param>
            /// <param name="x1"></param>
            /// <param name="y1"></param>
            /// <param name="x2"></param>
            /// <param name="y2"></param>
            /// <param name="imageFormat"></param>
            /// <returns></returns>
            public static Image CropAndResizeImage(Image img, int targetWidth, int targetHeight)
            {
                lvLogger.Verbose(Helper.ProgramStack);

                Bitmap bmp = new Bitmap(targetWidth, targetHeight);
                Graphics g = Graphics.FromImage(bmp);

                g.DrawImage(img, new Rectangle(0, 0, targetWidth, targetHeight), new Rectangle(0, 0, img.Width, img.Height), GraphicsUnit.Pixel);
                g.Dispose();

                return bmp;
            }

            #endregion

            #region Misc

            /// <summary>
            /// 
            /// </summary>
            /// <param name="rawString"></param>
            /// <param name="maxLength"></param>
            /// <param name="delimiter"></param>
            /// <returns></returns>
            public static string EllipsisString(this string rawString, int maxLength = 30, char delimiter = '\\')
            {
                maxLength -= 3; //account for delimiter spacing

                if (rawString.Length <= maxLength)
                {
                    return rawString;
                }

                string final = rawString;
                List<string> parts;

                int loops = 0;
                while (loops++ < 100)
                {
                    parts = rawString.Split(delimiter).ToList();
                    parts.RemoveRange(parts.Count - 1 - loops, loops);
                    if (parts.Count == 1)
                    {
                        return parts.Last();
                    }

                    parts.Insert(parts.Count - 1, "...");
                    final = string.Join(delimiter.ToString(), parts);
                    if (final.Length < maxLength)
                    {
                        return final;
                    }
                }

                return rawString.Split(delimiter).ToList().Last();
            }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Decrypt(string s)
            {
                //Decrypt
                byte[] bytes = Convert.FromBase64String(s);
                SymmetricAlgorithm crypt = Aes.Create();
                HashAlgorithm hash = MD5.Create();
                crypt.Key = hash.ComputeHash(Encoding.Unicode.GetBytes(Const.Constants.MASTERKEY));
                crypt.IV = IV;

                using (MemoryStream memoryStream = new MemoryStream(bytes))
                {
                    using (CryptoStream cryptoStream =
                       new CryptoStream(memoryStream, crypt.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        byte[] decryptedBytes = new byte[bytes.Length];
                        cryptoStream.Read(decryptedBytes, 0, decryptedBytes.Length);
                        string t = Encoding.Unicode.GetString(decryptedBytes);
                        return t.ReplaceAll("\0", string.Empty);
                    }
                }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="s"></param>
            /// <returns></returns>
            public static string Encrypt(string s)
            {
                byte[] bytes = Encoding.Unicode.GetBytes(s);
                //Encrypt
                SymmetricAlgorithm crypt = Aes.Create();
                HashAlgorithm hash = MD5.Create();
                crypt.BlockSize = BlockSize;
                crypt.Key = hash.ComputeHash(Encoding.Unicode.GetBytes(Const.Constants.MASTERKEY));
                crypt.IV = IV;

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream =
                       new CryptoStream(memoryStream, crypt.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(bytes, 0, bytes.Length);
                    }

                    return Convert.ToBase64String(memoryStream.ToArray()).ToString();
                }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="adr"></param>
            /// <returns></returns>
            public static bool TestPing(string adr)
            {
                lvLogger.Verbose(Helper.ProgramStack);

                try
                {
                    return TestPing(adr, out var Msg);
                }
                catch (Exception ex) { lvLogger.Error(ex.Message, ex); return false; }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="adr"></param>
            /// <param name="Msg"></param>
            /// <returns></returns>
            public static bool TestPing(string adr, out string Msg)
            {
                lvLogger.Verbose(Helper.ProgramStack);

                try
                {
                    Msg = string.Empty;
                    Ping pingSender = new Ping();
                    PingOptions options = new PingOptions();

                    // Use the default Ttl value which is 128,
                    // but change the fragmentation behavior.
                    options.DontFragment = true;

                    // Create a buffer of 32 bytes of data to be transmitted.
                    byte[] buffer = Encoding.ASCII.GetBytes(new string('a', 32));
                    int timeout = 120;
                    PingReply reply = pingSender.Send(adr, timeout, buffer, options);
                    if (reply.Status == IPStatus.Success)
                    {
                        lvLogger.InfoFormat("Address: {0}", reply.Address.ToString());
                        lvLogger.InfoFormat("RoundTrip time: {0}", reply.RoundtripTime);
                        lvLogger.InfoFormat("Time to live: {0}", reply.Options.Ttl);
                        lvLogger.InfoFormat("Don't fragment: {0}", reply.Options.DontFragment);
                        lvLogger.InfoFormat("Buffer size: {0}", reply.Buffer.Length);
                        return true;
                    }
                    return false;
                }
                catch (Exception ex) { lvLogger.Error(ex.Message, ex); Msg = ex.Message; return false; }
            }

            /// <summary>
            /// Returns all ticks, milliseconds or seconds since 1970.
            /// 
            /// 1 tick = 100 nanoseconds
            /// 
            /// Samples:
            /// 
            /// Return unit     value decimal           length      value hex       length
            /// --------------------------------------------------------------------------
            /// ticks           14094017407993061       17          3212786FA068F0  14
            /// milliseconds    1409397614940           13          148271D0BC5     11
            /// seconds         1409397492              10          5401D2AE        8
            ///
            /// </summary>
            public static string TickIdGet(bool getSecondsNotTicks, bool getMillisecondsNotTicks, bool getHexValue)
            {
                string id = string.Empty;

                DateTime historicalDate = new DateTime(1970, 1, 1, 0, 0, 0);

                if (getSecondsNotTicks || getMillisecondsNotTicks)
                {
                    TimeSpan spanTillNow = DateTime.UtcNow.Subtract(historicalDate);

                    if (getSecondsNotTicks)
                        id = String.Format("{0:0}", spanTillNow.TotalSeconds);
                    else
                        id = String.Format("{0:0}", spanTillNow.TotalMilliseconds);
                }
                else
                {
                    long ticksTillNow = DateTime.UtcNow.Ticks - historicalDate.Ticks;
                    id = ticksTillNow.ToString();
                }

                if (getHexValue)
                    id = long.Parse(id).ToString("X");

                return id;
            }
            /// <summary>
            /// 
            /// </summary>
            public static class Find
            {
                // Apparently the regex below works for non-ASCII uppercase
                // characters (so, better than A-Z).
                static readonly Regex CapitalLetter = new Regex(@"\p{Lu}");

                public static int FirstCapitalLetter(string input)
                {
                    Match match = CapitalLetter.Match(input);

                    return match.Success ? match.Index : -1;
                }
                public static int SecondCapitalLetter(string input)
                {
                    if (string.IsNullOrEmpty(input))
                        return -1;
                    if (input.Length < 2)
                        return -1;

                    return FirstCapitalLetter(input.Substring(1));
                }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="s"></param>
            /// <returns></returns>
            public static string ProperHeadCell(string s)
            {
                try
                {
                    int n = -1;

                    if (string.IsNullOrEmpty(s))
                        return s;
                    if (s.Length < 5)
                        return s;

                    n = s.IndexOf(" ");
                    if (n > 0)
                        return s.Remove(n, 1).Insert(n, Const.Constants.CRLF);

                    n = s.IndexOf("-");
                    if (n > 0)
                        return s.Remove(n, 1).Insert(n, Const.Constants.CRLF);

                    n = Find.SecondCapitalLetter(s);
                    if (n > 0)
                        return s.Insert(n + 1, Const.Constants.CRLF);
                    return s;
                }
                catch (Exception ex) { lvLogger.Error(ex.Message, ex); return s; }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="s"></param>
            /// <returns></returns>
            public static string stringParse(string s)
            {
                return s.ToSaveString();
            }
            public static string stringParse(object s)
            {
                return s.ToSaveString();
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="s"></param>
            /// <returns></returns>
            public static string ProperHeadCell2Lines(string s)
            {
                try
                {
                    if (string.IsNullOrEmpty(s))
                        return s;

                    string p = ProperHeadCell(s);
                    if (p.IndexOf(Const.Constants.CRLF) < 0)
                        p += Const.Constants.CRLF;
                    return p;
                }
                catch (Exception ex) { lvLogger.Error(ex.Message, ex); return s; }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="xml"></param>
            /// <returns></returns>
            public static string PrintXML(string xml)
            {
                lvLogger.Verbose(Helper.ProgramStack);

                string result = "";

                MemoryStream mStream = new MemoryStream();
                XmlTextWriter writer = new XmlTextWriter(mStream, Encoding.Unicode);
                XmlDocument document = new XmlDocument();

                try
                {
                    // Load the XmlDocument with the XML.
                    document.LoadXml(xml);

                    writer.Formatting = Formatting.Indented;

                    // Write the XML into a formatting XmlTextWriter
                    document.WriteContentTo(writer);
                    writer.Flush();
                    mStream.Flush();

                    // Have to rewind the MemoryStream in order to read
                    // its contents.
                    mStream.Position = 0;

                    // Read MemoryStream contents into a StreamReader.
                    StreamReader sReader = new StreamReader(mStream);

                    // Extract the text from the StreamReader.
                    string formattedXml = sReader.ReadToEnd();

                    result = formattedXml;
                }
                catch (XmlException)
                {
                    // Handle the exception
                }

                mStream.Close();
                writer.Close();

                return result;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="ExeAssembly"></param>
            /// <param name="MenuExists"></param>
            /// <returns></returns>
            public static string StartMenuEntry(Assembly ExeAssembly, out bool MenuExists)
            {
                lvLogger.Verbose(Helper.ProgramStack);

                MenuExists = false;

                try
                {
                    string AppRef = Const.Constants.APPREFMS;
                    string StartMenuePath = Path.GetFileNameWithoutExtension(ExeAssembly.Location) + AppRef;

                    string[] StartMenuePathArr1 = new string[5]
                    { Environment.GetFolderPath(Environment.SpecialFolder.StartMenu),
                      "Programs",
                      "UllisPrograms Umformtechnik",
                      Path.GetFileNameWithoutExtension(ExeAssembly.Location),
                      Path.GetFileNameWithoutExtension(ExeAssembly.Location) + AppRef};
                    StartMenuePath = Path.Combine(StartMenuePathArr1);
                    MenuExists = (File.Exists(StartMenuePath) ? true : false);
                    if (MenuExists)
                        return StartMenuePath;

                    string[] StartMenuePathArr2 = new string[4]
                    { Environment.GetFolderPath(Environment.SpecialFolder.StartMenu),
                      "Programs",
                      "UllisPrograms Umformtechnik",
                      Path.GetFileNameWithoutExtension(ExeAssembly.Location) + AppRef};
                    StartMenuePath = Path.Combine(StartMenuePathArr2);
                    MenuExists = (File.Exists(StartMenuePath) ? true : false);
                    if (MenuExists)
                        return StartMenuePath;

                    string[] StartMenuePathArr3 = new string[2]
                    { Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                      Path.GetFileNameWithoutExtension(ExeAssembly.Location) + AppRef};
                    StartMenuePath = Path.Combine(StartMenuePathArr3);
                    MenuExists = (File.Exists(StartMenuePath) ? true : false);
                    if (MenuExists)
                        return StartMenuePath;

                    return StartMenuePath;
                }
                catch (Exception ex) { lvLogger.Error(ex.Message, ex); return null; }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="ExeAssembly"></param>
            /// <returns></returns>
            public static string GetExeName(Assembly ExeAssembly)
            {
                lvLogger.Verbose(Helper.ProgramStack);

                try
                {
                    return ExeAssembly.GetName().Name;
                }
                catch (Exception ex) { lvLogger.Error(ex.Message, ex); return null; }
            }
            /// <summary>
            /// 
            /// </summary>
            public static string ComFolder
            {
                get
                {
                    lvLogger.Verbose(Helper.ProgramStack);

                    try
                    {
                        string iPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + Path.DirectorySeparatorChar + @"ProgCom\";

                        if (System.IO.Directory.Exists(iPath))
                        {
                            return iPath;
                        }
                        else
                        {
                            System.IO.Directory.CreateDirectory(iPath);
                            if (System.IO.Directory.Exists(iPath))
                            {
                                return iPath;
                            }
                            else
                            {
                                return iPath;
                            }
                        }
                    }
                    catch (Exception ex) { lvLogger.Error(ex.Message, ex); return null; }
                }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="LocalPath"></param>
            public static void DelGarbage(string LocalPath)
            {
                lvLogger.Verbose(Helper.ProgramStack);

                DelFile("*Membrain*", LocalPath);
                DelFile("admin.ini", LocalPath);
                DelFile("Psion*", LocalPath);
                DelFile("Symbol*", LocalPath);
                DelFile("Intermec*", LocalPath);
                DelFile("InventurBackup*", LocalPath);
            }
            /// <summary>
            /// Gets the drive type of the given path.
            /// </summary>
            /// <param name="path">The path.</param>
            /// <returns>DriveType of path</returns>
            public static DriveType GetPathDriveType(string path)
            {
                lvLogger.Verbose(Helper.ProgramStack);

                //OK, so UNC paths aren't 'drives', but this is still handy
                if (path.StartsWith(@"\\")) return DriveType.Network;
                var info = DriveInfo.GetDrives();

                foreach (var i in info)
                {
                    if (path.StartsWith(i.Name, StringComparison.OrdinalIgnoreCase))
                        return i.DriveType;
                }

                return DriveType.Unknown;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="firstImage"></param>
            /// <param name="secondImage"></param>
            /// <returns></returns>
            public static bool ImageCompareString(Image firstImage, Image secondImage)
            {
                lvLogger.Verbose(Helper.ProgramStack);

                System.Diagnostics.StackTrace sTrace = new System.Diagnostics.StackTrace();

                MemoryStream ms = new MemoryStream();
                firstImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                string firstBitmap = Convert.ToBase64String(ms.ToArray());

                ms.Position = 0;

                secondImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

                string secondBitmap = Convert.ToBase64String(ms.ToArray());

                if (firstBitmap.Equals(secondBitmap))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="ex"></param>
            /// <returns></returns>
            public static string BuildMessageString(Exception ex)
            {
                lvLogger.Verbose(Helper.ProgramStack);

                if (ex == null)
                {
                    return string.Empty;
                }

                string tmp = ex.Message;
                if (ex.InnerException != null)
                {
                    tmp += Environment.NewLine + ex.InnerException.Message;
                }

                return tmp;
            }
            /// <summary>
            /// 
            /// </summary>
            public static string ProgramStack
            {
                get
                {
                    System.Diagnostics.StackTrace sTrace = new System.Diagnostics.StackTrace();

                    string className1 = string.Empty; string methodName1 = string.Empty;

                    try
                    {
                        MethodBase method1 = sTrace.GetFrame(1).GetMethod();
                        methodName1 = method1.Name;
                        className1 = method1.ReflectedType.Name;
                    }
                    catch { className1 = methodName1 = string.Empty; }

                    return string.Format("entering {0}.{1} by {2}", className1, methodName1 + "()", CurrStep);
                }
            }
            /// <summary>
            /// 
            /// </summary>
            public static string CurrStep
            {
                get
                {
                    System.Diagnostics.StackTrace sTrace = new System.Diagnostics.StackTrace();

                    string className1 = string.Empty; string methodName1 = string.Empty;

                    try
                    {
                        MethodBase method1 = sTrace.GetFrame(2).GetMethod();
                        methodName1 = method1.Name;
                        className1 = method1.ReflectedType.Name;
                    }
                    catch { className1 = methodName1 = string.Empty; }

                    return string.Format("{0}.{1}()", className1, methodName1);
                }
            }
            /// <summary>
            /// 
            /// </summary>
            public static string ProgramStackLeave
            {
                get
                {
                    try
                    {
                        string tmp = ProgramStack;

                        return tmp.Replace("entering", "now leaving");
                    }
                    catch { return string.Empty; }
                }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="s"></param>
            /// <returns></returns>
            public static void ExtractContentFromNotify(string s, out string c1, out string c2)
            {
                c1 = s;
                c2 = string.Empty;

                int p = s.IndexOf('!');
                if (p > -1)
                {
                    c1 = s.Substring(0, p);
                    c2 = s.Substring(p + 1);
                    return;
                }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="t"></param>
            /// <returns></returns>
            public static void SetRegScrollbarWidth()
            {
                lvLogger.Verbose(Helper.ProgramStack);

                try
                {
                    if (KernelFunctions.IsTouchDevice)
                    {
                        lvLogger.InfoFormat("This device is a Touchscreen-Device");
                        //create a reference to a valid key, which was created earlier
                        //remember that the key name is case-insensitive
                        using (RegistryKey rk = Registry.CurrentUser.OpenSubKey(Const.Constants.METRICS, true))
                        {
                            rk.SetValue(Const.Constants.SCROLLHEIGHT, "40");
                            rk.SetValue(Const.Constants.SCROLLWIDTH, "40");
                        }
                    }
                    else
                    {
                        lvLogger.InfoFormat("This device is not a Touchscreen-Device");
                    }
                }
                catch (Exception ex) { lvLogger.Error(ex.Message, ex); }
            }
            /// <summary>
            /// 
            /// </summary>
            private static readonly string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
            public static string SizeSuffix(long value, int decimalPlaces = 1)
            {
                if (value < 0) { return "-" + SizeSuffix(-value); }

                int i = 0;
                decimal dValue = value;
                while (Math.Round(dValue, decimalPlaces) >= 1000)
                {
                    dValue /= 1024;
                    i++;
                }

                return string.Format("{0:n" + decimalPlaces + "} {1}", dValue, SizeSuffixes[i]);
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="Origin"></param>
            /// <param name="Msg"></param>
            /// <param name="ParentBounds"></param>
            public static bool CompareTrimmed(object c1, object c2)
            {
                try
                {
                    return CompareTrimmed(c1.ToSaveString(), c2.ToSaveString());
                }
                catch (Exception ex) { lvLogger.Error(ex.Message, ex); return false; }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="Origin"></param>
            /// <param name="Msg"></param>
            /// <param name="ParentBounds"></param>
            public static bool CompareTrimmed(string c1, string c2)
            {
                try
                {
                    if (string.Compare(c1.TrimStart('0'), c2.TrimStart('0'), true) == 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex) { lvLogger.Error(ex.Message, ex); return false; }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="form"></param>
            public static Rectangle BigToSmall(Rectangle ParentBounds, float Ratio)
            {
                try
                {
                    Rectangle objBig = ParentBounds;
                    Rectangle objSmall = ParentBounds;
                    objSmall.Size = new SizeF(objBig.Width / Ratio, objBig.Height / Ratio).ToSize();
                    objSmall.Location = CenterSmallOnBig(objBig, objSmall);
                    return objSmall;
                }
                catch (Exception ex) { lvLogger.Error(ex.Message, ex); return ParentBounds; }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="objBig"></param>
            /// <param name="objSmall"></param>
            public static Point CenterSmallOnBig(Rectangle objBig, Rectangle objSmall)
            {
                try
                {
                    objSmall.X = (int)(objBig.X + ((((objBig.Width / 2) - (objSmall.Width / 2)))));
                    objSmall.Y = (int)(objBig.Y + ((((objBig.Height / 2) - (objSmall.Height / 2)))));
                    return objSmall.Location;
                }
                catch (Exception ex) { lvLogger.Error(ex.Message, ex); return new Point(0, 0); }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public static bool IsLUD
            {
                get
                {
                    if (System.Net.Dns.GetHostName().ToUpper().StartsWith("LDW"))
                    {
                        return true;
                    }

                    if (System.Net.Dns.GetHostName().ToUpper().StartsWith("LUD"))
                    {
                        return true;
                    }

                    return false;
                }
            }
            /// <summary>
            /// 
            /// </summary>
            public static string DeviceName => System.Net.Dns.GetHostName().ToLower();
            /// <summary>
            /// 
            /// 
            /// </summary>
            public static bool CompareWithPattern(string Content, string ContentWithPattern)
            {
                ContentWithPattern = ContentWithPattern.Replace("/", string.Empty);
                ContentWithPattern = ContentWithPattern.Replace("+", "%");
                ContentWithPattern = ContentWithPattern.Replace("%%%%%", "%");
                ContentWithPattern = ContentWithPattern.Replace("%%%%", "%");
                ContentWithPattern = ContentWithPattern.Replace("%%%", "%");
                ContentWithPattern = ContentWithPattern.Replace("%%", "%");
                Content = Content.Replace("/", string.Empty);
                return new Regex(@"\A" + new Regex(@"\.|\$|\^|\{|\[|\(|\||\)|\*|\+|\?|\\").Replace(ContentWithPattern, ch => @"\" + ch).Replace('_', '.').Replace("%", ".*") + @"\z", RegexOptions.Singleline).IsMatch(Content);
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public static string GetUTC => DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd") + "T" + DateTime.Now.ToUniversalTime().ToString("HH:mm:ss").Replace('.', ':');
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public static bool IsRunningOnMyComputer
            {
                get
                {
                    string csMyPC1 = "bie154d021";
                    string csMyPC2 = "biews1877";
                    string csZotac = "zotac2";
                    string csMyLaptop = "bie182n047";

                    return
                       (System.Net.Dns.GetHostName().ToLower() == csZotac.ToLower() ||
                        System.Net.Dns.GetHostName().ToLower() == csMyPC1.ToLower() ||
                        System.Net.Dns.GetHostName().ToLower() == csMyPC2.ToLower() ||
                        System.Net.Dns.GetHostName().ToLower() == csMyLaptop.ToLower());
                }
            }
            public static bool IsRunningOnSusanasComputer
            {
                get
                {
                    string csSSPC1 = "bie153D031";
                    string csSSPC2 = "bie162d033";
                    return
                       (System.Net.Dns.GetHostName().ToLower() == csSSPC1.ToLower() ||
                        System.Net.Dns.GetHostName().ToLower() == csSSPC2.ToLower());
                }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="lvFile"></param>
            /// <returns></returns>
            public static Assembly LoadLib(string lvFile)
            {
                try
                {
                    return Assembly.LoadFrom(lvFile);
                }
                catch (Exception ex)
                {
                    lvLogger.Error(ex.Message, ex);
                    return null;
                }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="value"></param>
            /// <param name="Percentage"></param>
            /// <returns></returns>
            public static int CalcPercentage(int value, double Percentage)
            {
                double z = Convert.ToDouble(value) / 100 * Percentage;
                return (int)z;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="value"></param>
            /// <param name="Percentage"></param>
            /// <returns></returns>
            public static int CalcPercentage(double value, double Percentage)
            {
                double z = value / 100 * Percentage;
                return (int)z;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="locArray"></param>
            /// <param name="locSearch"></param>
            /// <returns></returns>
            public static bool IsInArray(string[] locArray, string locSearch)
            {
                for (int i = 0; i <= locArray.Length - 1; i++)
                {
                    if (locArray[i] == locSearch)
                    {
                        return true;
                    }
                }

                return false;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="str"></param>
            /// <returns></returns>
            public static byte[] StringToByteArray(string str)
            {
                ASCIIEncoding enc = new System.Text.ASCIIEncoding();
                return enc.GetBytes(str);
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="arr"></param>
            /// <returns></returns>
            public static string ByteArrayToString(byte[] arr)
            {
                ASCIIEncoding enc = new System.Text.ASCIIEncoding();
                return enc.GetString(arr, arr.GetLowerBound(0), arr.GetUpperBound(0));
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public static bool QbjEquals(object o1, object o2)
            {
                try
                {
                    if (o1 == null)
                    {
                        return false;
                    }

                    if (o2 == null)
                    {
                        return false;
                    }

                    if (o1.Equals(o2))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    lvLogger.Error(ex.Message, ex);

                    return false;
                }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="type"></param>
            /// <returns></returns>
            public static bool IsValueType(Type type)
            {
                return type.IsValueType || type == typeof(string);
            }

            #endregion

            #region Grafics

            /// <summary>
            /// Returns the EXIF Image Data of the Date Taken.
            /// </summary>
            /// <param name="getImage">Image (If based on a file use Image.FromFile(f);)</param>
            /// <returns>Date Taken or Null if Unavailable</returns>
            public static DateTime DateTaken(Image getImage)
            {
                int DateTakenValue = 0x9003; //36867;
                int DateCreateValue = 306;
                int Exif = 0;

                if (getImage.PropertyIdList.Contains(DateTakenValue))
                    Exif = DateTakenValue;
                else if (getImage.PropertyIdList.Contains(DateCreateValue))
                    Exif = DateCreateValue;
                else
                    return DateTime.MinValue;

                string dateTakenTag = System.Text.Encoding.ASCII.GetString(getImage.GetPropertyItem(Exif).Value);
                string[] parts = dateTakenTag.Split(':', ' ');
                int year = int.Parse(parts[0]);
                int month = int.Parse(parts[1]);
                int day = int.Parse(parts[2]);
                int hour = int.Parse(parts[3]);
                int minute = int.Parse(parts[4]);
                int second = int.Parse(parts[5]);

                return new DateTime(year, month, day, hour, minute, second);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="graphics"></param>
            /// <param name="text"></param>
            /// <param name="font"></param>
            /// <param name="maxWidth"></param>
            /// <returns></returns>        
            public static SizeF MeasureString(Graphics graphics, ref string text, Font font, float maxWidth)
            {
                int lineStart = 0;
                while (true)
                {
                    SizeF size = graphics.MeasureString(text, font);
                    if (size.Width <= maxWidth)
                    {
                        return size;
                    }

                    int lineEnd = text.IndexOf('\n', lineStart);
                    if (lineEnd == -1)
                    {
                        lineEnd = text.Length;
                    }

                    string line = text.Substring(lineStart, lineEnd - lineStart).TrimEnd(' ');

                    size = graphics.MeasureString(line, font);
                    if (size.Width > maxWidth)
                    {
                        int wordStart = 0;
                        while (true)
                        {
                            int wordEnd = line.IndexOf(' ', wordStart);
                            if (wordEnd == -1)
                            {
                                wordEnd = line.Length;
                            }

                            string tmpLine = line.Substring(0, wordEnd);
                            size = graphics.MeasureString(tmpLine, font);
                            if (size.Width <= maxWidth)
                            {
                                Debug.Assert(line[wordEnd] == ' ');
                                wordStart = wordEnd;

                                // Skip spaces
                                do
                                {
                                    wordStart++;
                                }
                                while (line[wordStart] == ' ');
                            }
                            else
                            {
                                if (wordStart == 0)
                                {
                                    for (wordStart = 2; ; wordStart++)
                                    {
                                        string tmpWord = line.Substring(0, wordStart);
                                        if (graphics.MeasureString(tmpWord, font).Width > maxWidth)
                                        {
                                            wordStart--;
                                            break;
                                        }
                                    }
                                }

                                text = text.Substring(0, lineStart + wordStart) + '\n' + text.Substring(lineStart + wordStart);
                                lineEnd = lineStart + wordStart;
                                break;
                            }
                        }
                    }
                    lineStart = lineEnd + 1;
                }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="ic"></param>
            /// <returns></returns>
            public static double GetInverseGamma(int ic)
            {
                double result;

                // Inverse of sRGB "gamma" function. (approx 2.2)

                double c = ic / 255.0;
                if (c <= 0.04045)
                {
                    result = c / 12.92;
                }
                else
                {
                    result = Math.Pow((c + 0.055) / 1.055, 2.4);
                }

                return result;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="color"></param>
            /// <returns></returns>
            public static int GetBrightness(Color color)
            {
                //http://stackoverflow.com/a/13558570/148962

                // GRAY VALUE ("brightness")

                return GetGamma(Const.Constants.REDLUMINANCE * GetInverseGamma(color.R) + Const.Constants.GREENLUMINANCE * GetInverseGamma(color.G) + Const.Constants.BLUELUMINANCE * GetInverseGamma(color.B));
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="v"></param>
            /// <returns></returns>
            public static int GetGamma(double v)
            {
                // sRGB "gamma" function (approx 2.2)

                if (v <= 0.0031308)
                {
                    v *= 12.92;
                }
                else
                {
                    v = 1.055 * Math.Pow(v, 1.0 / 2.4) - 0.055;
                }

                return (int)(v * 255 + .5);
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="ColourToInvert"></param>
            /// <returns></returns>
            public static Color InvertMeAColour(Color fromColor)
            {
                try
                {
                    Color invertedColor = Color.FromArgb(fromColor.ToArgb() ^ 0xffffff);

                    if (invertedColor.R > 110 && invertedColor.R < 150 &&
                        invertedColor.G > 110 && invertedColor.G < 150 &&
                        invertedColor.B > 110 && invertedColor.B < 150)
                    {
                        int avg = (invertedColor.R + invertedColor.G + invertedColor.B) / 3;
                        avg = avg > 128 ? 200 : 60;
                        invertedColor = Color.FromArgb(avg, avg, avg);
                    }
                    return invertedColor;
                }
                catch { return Color.White; }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="bitmap"></param>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <param name="cx"></param>
            /// <param name="cy"></param>
            public static void ConvertToGrayscale(Bitmap bitmap, int x, int y, int cx, int cy)
            {
                for (int i = 0, yy = y; i < cy; i++, yy++)
                {
                    for (int j = 0, xx = x; j < cx; j++, xx++)
                    {
                        Color c = bitmap.GetPixel(xx, yy);
                        int luma = (int)(c.R * 0.3 + c.G * 0.59 + c.B * 0.11);
                        if (luma != 0)
                        {
                            bitmap.SetPixel(xx, yy, Color.FromArgb(luma, luma, luma));
                        }
                    }
                }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="original"></param>
            /// <param name="newX"></param>
            /// <param name="newY"></param>
            /// <returns></returns>
            public static Bitmap ResizePic(Bitmap original, int newX, int newY)
            {
                if (original.Equals(new Size(newX, newY)))
                {
                    return original;
                }

                Rectangle recSrc = new Rectangle(0, 0, original.Width, original.Height);
                Rectangle recDest = new Rectangle(0, 0, newX, newY);
                Bitmap target = new Bitmap(newX, newY);
                Graphics g = Graphics.FromImage(target);
                g.DrawImage(original, recDest, recSrc, GraphicsUnit.Pixel);
                return target;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="original"></param>
            /// <param name="newS"></param>
            /// <returns></returns>
            public static Image ResizePic(Image original, int newX, int newY)
            {
                try
                {
                    return ResizePic(original, new Size(newX, newY));
                }
                catch { return new Bitmap(56, 56); }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="original"></param>
            /// <param name="newS"></param>
            /// <returns></returns>
            public static Image ResizePic(Image original, Size newS)
            {
                try
                {
                    if (original.Equals(newS))
                    {
                        return original;
                    }

                    Rectangle recSrc = new Rectangle(0, 0, original.Width, original.Height);
                    Rectangle recDest = new Rectangle(0, 0, newS.Width, newS.Height);
                    Bitmap target = new Bitmap(newS.Width, newS.Height);
                    Graphics g = Graphics.FromImage(target);
                    g.DrawImage(original, recDest, recSrc, GraphicsUnit.Pixel);
                    return target;
                }
                catch { return new Bitmap(56, 56); }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="originalIcon"></param>
            /// <param name="newX"></param>
            /// <param name="newY"></param>
            /// <returns></returns>
            public static Bitmap ResizePic(Icon originalIcon, int newX, int newY)
            {
                Rectangle recSrc = new Rectangle(0, 0, originalIcon.Width, originalIcon.Height);
                Rectangle recDest = new Rectangle(0, 0, newX, newY);
                Bitmap originalBitmap = new Bitmap(originalIcon.Width, originalIcon.Height);
                Bitmap targetBitmap = new Bitmap(newX, newY);
                ImageAttributes transparencyAttribute = new ImageAttributes();
                transparencyAttribute.SetColorKey(originalBitmap.GetPixel(0, 0), originalBitmap.GetPixel(0, 0));
                Graphics g = Graphics.FromImage(originalBitmap);
                g.DrawIcon(originalIcon, 0, 0);
                g = Graphics.FromImage(targetBitmap);
                g.DrawImage(originalBitmap, recDest, 0, 0, originalBitmap.Width,
                    originalBitmap.Height, GraphicsUnit.Pixel, transparencyAttribute);
                return targetBitmap;
            }

            #endregion

            #region Version

            /// <summary>
            /// 
            /// </summary>
            /// <param name="AssVers"></param>
            /// <returns></returns>
            public static string BuildVersionString(Version AssVers)
            { return BuildVersionString(AssVers, 2); }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="AssVers"></param>
            /// <param name="Type"></param>
            /// <returns></returns>
            public static string BuildVersionString(Version AssVers, int Type)
            {
                DateTime AssDat = BuildVersionDateTime(AssVers);

                if (Type == 1)
                {
                    return "V" + BuildBasicVersion(AssVers) + Environment.NewLine +
                      AssDat.ToString("dd.MM.yy HH:mm") + Const.Constants.SPACE + BuildZone(AssVers);
                }
                else if (Type == 2)
                {
                    return "V" + BuildBasicVersion(AssVers) + " vom " + AssDat.ToString("dd.MM.yy HH:mm") + Const.Constants.SPACE + BuildZone(AssVers);
                }
                else if (Type == 3)
                {
                    return "V" + BuildBasicVersion(AssVers) + " vom " + AssDat.ToString("dd.MM.yy");
                }
                else if (Type == 4)
                {
                    return "V" + BuildBasicVersion(AssVers);
                }
                else if (Type == 5)
                {
                    return "V" + BuildBasicVersion(AssVers) + " vom " + AssDat.ToString("d.M. H:mm");
                }
                else
                {
                    return "V" + BuildBasicVersion(AssVers);
                }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public static DateTime BuildVersionDateTime(Version AssVers)
            {
                DateTime dt = Convert.ToDateTime("2000-01-01");
                dt = dt.AddDays(AssVers.Build);
                dt = dt.AddSeconds(AssVers.Revision * 2);
                //if (System.TimeZone.IsDaylightSavingTime(dt, System.TimeZone.CurrentTimeZone.GetDaylightChanges(dt.Year)))
                //{
                //    dt = dt.AddHours(1);
                //}

                return dt;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="VersStr"></param>
            /// <returns></returns>
            private static string BuildBasicVersion(Version AssVers)
            {
                return AssVers.Major.ToString() + "." + AssVers.Minor.ToString();
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            private static string BuildZone(Version AssVers)
            {
                DateTime dt = Convert.ToDateTime("2000-01-01");
                dt = dt.AddDays(AssVers.Build);
                dt = dt.AddSeconds(AssVers.Revision * 2);
                if (System.TimeZone.IsDaylightSavingTime(dt, System.TimeZone.CurrentTimeZone.GetDaylightChanges(dt.Year)))
                {
                    return "(CEST)";
                }
                else
                {
                    return "(CET)";
                }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public static string GetCompileMode()
            {
#if debug
				return "Debug";
#else
                return "Release";
#endif
            }

            #endregion

            #region Parser

            /// <summary>
            /// 
            /// </summary>
            /// <param name="cObj"></param>
            /// <returns></returns>
            public static Int32 SQLInt32Parse(object cObj)
            {
                return (cObj.Equals(DBNull.Value) ? 0 : int32Parse(cObj));
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cObj"></param>
            /// <returns></returns>
            public static string SQLStringParse(object cObj)
            {
                return (cObj.Equals(DBNull.Value) ? string.Empty : stringParse(cObj));
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cObj"></param>
            /// <returns></returns>
            public static DateTime SQLDateTimeParse(object cObj)
            {
                return (cObj.Equals(DBNull.Value) ? DateTime.MinValue : DateTimeParse(cObj));
            }
            ///	<summary>
            /// convert and parse can't handle empty string and NULL. this sub 
            /// handles trailing '-', empty strings, NULLs and '.'-colons
            ///	</summary>
            public static int int32Parse(string ctext)
            {
                if (string.IsNullOrWhiteSpace(ctext))
                {
                    return 0;
                }

                return Convert.ToInt32(doubleParse(ctext));
            }
            public static int int32Parse(object cobject)
            {
                if (cobject == null)
                {
                    return 0;
                }

                return int32Parse(cobject.ToString());
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cobject"></param>
            /// <returns></returns>
            public static bool SQLBoolParse(object cobject)
            {
                try
                {
                    if (cobject.Equals(System.DBNull.Value))
                    {
                        return false;
                    }

                    if (cobject == null)
                    {
                        return false;
                    }

                    return stringComparer(cobject, "true") ? true : false;
                }
                catch { return false; }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="s1"></param>
            /// <param name="s2"></param>
            /// <returns></returns>
            public static bool stringComparer(string s1, string s2)
            {
                return (string.Compare(s1, s2, true) == 0);
            }
            public static bool stringComparer(object s1, string s2)
            {
                return (string.Compare(s1.ToSaveString(), s2, true) == 0);
            }
            public static bool stringContains(string s1, string s2)
            {
                return s1.ToLower().Contains(s2.ToLower());
            }
            ///	<summary>
            /// convert and parse can't handle empty string and NULL. this sub 
            /// handles trailing '-', empty strings, NULLs and '.'-colons
            ///	</summary>
            public static long longParse(string ctext)
            {
                if (string.IsNullOrWhiteSpace(ctext))
                {
                    return 0;
                }

                return Convert.ToInt32(doubleParse(ctext));
            }
            public static long longParse(object cobject)
            {
                if (cobject.Equals(System.DBNull.Value))
                {
                    return 0;
                }

                if (cobject == null)
                {
                    return 0;
                }

                return longParse(cobject.ToString());
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="ctext"></param>
            /// <returns></returns>
            public static float singleParse(string ctext)
            {
                if (string.IsNullOrWhiteSpace(ctext))
                {
                    return 0;
                }

                return Convert.ToInt32(doubleParse(ctext));
            }
            public static float singleParse(object cobject)
            {
                if (cobject.Equals(System.DBNull.Value))
                {
                    return 0;
                }

                if (cobject == null)
                {
                    return 0;
                }

                return singleParse(cobject.ToString());
            }
            ///	<summary>
            /// convert and parse can't handle empty string and NULL. this sub 
            /// handles trailing '-', empty strings, NULLs and '.'-colons
            ///	</summary>
            public static int intParse(string ctext)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(ctext))
                    {
                        return 0;
                    }

                    return (int)(doubleParse(ctext));
                }
                catch { return 0; }
            }
            public static uint uintParse(string ctext)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(ctext))
                    {
                        return 0;
                    }

                    return (uint)(doubleParse(ctext));
                }
                catch { return 0; }
            }
            public static int intParse(object cobject)
            {
                if (cobject.Equals(System.DBNull.Value))
                {
                    return 0;
                }

                if (cobject == null)
                {
                    return 0;
                }

                return intParse(cobject.ToString());
            }
            ///	<summary>
            /// convert and parse can't handle empty string and NULL. this sub 
            /// handles trailing '-', empty strings, NULLs and '.'-colons
            ///	</summary>
            public static double doubleParse(object cobject)
            {
                if (cobject == null)
                {
                    return 0.0;
                }
                if (cobject.Equals(System.DBNull.Value))
                {
                    return 0.0;
                }

                return doubleParse(cobject.ToString());
            }
            public static double doubleParseAMI(object cobject)
            {
                if (cobject == null)
                {
                    return 0.0;
                }
                if (cobject.Equals(System.DBNull.Value))
                {
                    return 0.0;
                }

                string tmp = cobject.ToSaveString();
                tmp = tmp.Replace(",", "");
                tmp = tmp.Replace(".", ",");

                return doubleParse(tmp.ToString());
            }
            public static double doubleParse(string ctext)
            {
                bool minusflag = false; double d;

                if (string.IsNullOrWhiteSpace(ctext))
                {
                    return 0.0;
                }

                string s = ctext.Trim();
                if (string.IsNullOrWhiteSpace(s))
                {
                    return 0.0;
                }

                if (s.IndexOf('-') > -1)
                {
                    s = s.Replace("-", string.Empty);
                    minusflag = true;
                }
                try
                {
                    d = double.Parse(s);
                }
                catch (Exception ex)
                {
                    lvLogger.Error(ex.Message + " Eingabe war:'" + ctext + "'", ex);

                    d = 0.0;
                }

                if (minusflag)
                {
                    d = -d;
                }

                return d;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cobject"></param>
            /// <returns></returns>
            public static Size SizeParse(string myString)
            {
                try
                {
                    // convert a string to a Size using a type converter
                    myString.Condense();
                    myString = myString.Replace("Height=", Const.Constants.SPACE);
                    myString = myString.Replace("Width=", Const.Constants.SPACE);
                    myString = myString.Replace("{", Const.Constants.SPACE);
                    myString = myString.Replace("}", Const.Constants.SPACE);
                    myString.Condense();
                    string[] StrArray = myString.Trim().Split(',');
                    return new Size(Helper.intParse(StrArray[0]), Helper.intParse(StrArray[1]));
                }
                catch { return Size.Empty; }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="d"></param>
            /// <param name="t"></param>
            /// <returns></returns>
            public static DateTime DateTimeParse(string cDateString, string cTimeString)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(cDateString))
                    {
                        return DateTime.MinValue;
                    }

                    if (string.IsNullOrWhiteSpace(cTimeString))
                    {
                        return DateTime.MinValue;
                    }

                    return DateTimeParse(cDateString + Const.Constants.SPACE + cTimeString);
                }
                catch { return DateTime.MinValue; }
            }
            public static DateTime DateTimeParse(string cDateString, string cTimeString, bool Ret19)
            {
                DateTime dt = DateTimeParse(cDateString, cTimeString);
                if (dt == DateTime.MinValue)
                {
                    return DateTime.Parse("1900-01-01 00:00:01");
                }
                else
                {
                    return dt;
                }
            }
            public static DateTime DateTimeParse(object cDateTimeObject)
            {
                try
                {
                    if (cDateTimeObject == null)
                    {
                        return DateTime.MinValue;
                    }

                    return DateTimeParse(cDateTimeObject.ToString());
                }
                catch { return DateTime.MinValue; }
            }
            public static DateTime DateTimeParse(string cDateTimeString)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(cDateTimeString))
                    {
                        return DateTime.MinValue;
                    }

                    string[] formats = {"d.M.yyyy H:mm:ss tt", "d/M/yyyy H:mm tt",
                         "dd.MM.yyyy HH:mm:ss", "M/d/yyyy H:mm:ss",
                         "d.M.yyyy HH:mm tt", "M/d/yyyy HH tt",
                         "d.M.yyyy H:mm", "M/d/yyyy H:mm",
                         "dd.MM.yy HH:mm:ss.fff",
                         "dd.MM.yyyy HH:mm:ss.fff",
                         "dd/MM/yyyy",
                         "yyyy.MM.dd HH:mm:ss.fff",
                         "dd.MM.yyyy HH:mm", "M/dd/yyyy HH:mm",
                         "dd.MM.yyyy HH:mm:ss",
                         "d.MM.yyyy HH:mm:ss.ffffff",
                         "yyyyMMddHHmmss",
                         "yyyyMMdd",
                         "yyyyMMdd HHmmss"};

                    return (DateTime.ParseExact(cDateTimeString, formats,
                                            new CultureInfo("de-DE"),
                                            DateTimeStyles.None));
                }
                catch { return DateTime.MinValue; }
            }
            ///	<summary>
            /// convert and parse can't handle empty string and NULL. this sub 
            /// handles trailing '-', empty strings, NULLs and '.'-colons
            ///	</summary>
            public static decimal decimalParse(string ctext)
            {
                try
                {
                    if (ctext.Equals(System.DBNull.Value))
                    {
                        return 0;
                    }

                    if (string.IsNullOrWhiteSpace(ctext))
                    {
                        return 0;
                    }

                    return decimal.Parse(ctext);
                }
                catch { return 0; }
            }
            public static decimal decimalParse(object cobject)
            {
                try
                {
                    if (cobject.Equals(System.DBNull.Value))
                    {
                        return 0;
                    }

                    if (cobject == null)
                    {
                        return 0;
                    }

                    return decimalParse(cobject.ToSaveString());
                }
                catch { return 0; }
            }

            #endregion

            #region DateTime

            public enum Bundesland
            {
                BadenWürttemberg,
                Bayern,
                Berlin,
                Brandenburg,
                Bremen,
                Hamburg,
                Hessen,
                MecklenburgVorpommern,
                Niedersachsen,
                NordrheinWestfalen,
                RheinlandPfalz,
                Saarland,
                Sachsen,
                SachsenAnhalt,
                SchleswigHolstein,
                Thüringen
            }
            /// <summary>
            /// Representiert den Feiertag mit
            /// allen wichtigen informationen
            /// </summary>
            public class Feiertag : IComparable<Feiertag>
            {
                private bool isFix;
                private DateTime datum;
                private string name;

                public Feiertag(bool isFix, DateTime datum, string name)
                {
                    IsFix = isFix;
                    Datum = datum;
                    Name = name;
                }

                /// <summary>
                /// Der Name des Feiertages
                /// </summary>
                public string Name
                {
                    get => name;
                    set => name = value;
                }

                /// <summary>
                /// Das Datum an dem dieser Feiertag stattfindet
                /// </summary>
                public DateTime Datum
                {
                    get => datum;
                    set => datum = value;
                }

                /// <summary>
                /// Zeigt an ob es sich um einen Datums spezifischer
                /// oder zyklisch Feiertag handelt
                /// </summary>
                public bool IsFix
                {
                    get => isFix;
                    set => isFix = value;
                }

                public int CompareTo(Feiertag other)
                {
                    return datum.Date.CompareTo(other.datum.Date);
                }
            }
            /// <summary>
            /// Hält eine Liste von Feiertagen für die Jahr Monat Kombination
            /// </summary>
            public class MyFeiertagLogic
            {
                private static MyFeiertagLogic Instance;
                private List<Feiertag> feiertage;
                private int year;

                /// <summary>
                /// Das Jahr für welches die Feiertage berechnet werden
                /// </summary>
                public int CurrentYear
                {
                    get => year;
                    set => year = value;
                }

                /// <summary>
                /// Erzeugt eine neue Instanz der Feiertage für das Übergebene Jahr außerdem wird davon ausgegangen das es sich bei dem Bundesland um Sachsen handelt
                /// </summary>
                /// <param name="year">Das Jahr für welches Die Feiertaglogic Initialisiert werden soll</param>
                /// <returns></returns>
                public static MyFeiertagLogic GetInstance(int year)
                {
                    if (Instance == null || year != Instance.CurrentYear)
                    {
                        Instance = new MyFeiertagLogic(year, Bundesland.Sachsen);
                        return Instance;
                    }

                    return Instance;
                }

                /// <summary>
                /// Erzeugt eine neue Instanz der Feiertage für das Übergebene Jahr und Bundesland
                /// </summary>
                /// <param name="year">Das Jahr für welches Die Feiertaglogic Initialisiert werden soll</param>
                /// <param name="bl">Das Bundesland welches zur Ermittlung der Feiertage betrachtet werden soll</param>
                /// <returns></returns>
                public static MyFeiertagLogic GetInstance(int year, Bundesland bl)
                {
                    if (Instance == null || year != Instance.CurrentYear)
                    {
                        Instance = new MyFeiertagLogic(year, bl);
                        return Instance;
                    }

                    return Instance;
                }

                /// <summary>
                /// Beschreibung: Gibt variable Feiertage zurueck
                /// </summary>
                public List<Feiertag> Feiertagliste => feiertage;

                /// <summary>
                ///  prüft ob das übermittelte Datum ein Feiertag ist
                /// </summary>
                /// <param name="value">zu prüfendes Datum</param>
                /// <returns>True wenn ja</returns>
                public bool isFeiertag(DateTime value)
                {
                    return (feiertage.Find(delegate (Feiertag f) { return f.Datum.Date == value.Date; }) != null);
                }

                /// <summary>
                /// gibt den Names des Feirtages zurück wenn das
                /// übergebene Datum ein Feiertag ist
                /// </summary>
                /// <param name="value">Feiertagsdatum</param>
                /// <returns>Name des Feiertages</returns>
                public Feiertag GetFeiertagName(DateTime value)
                {
                    return (feiertage.Find(delegate (Feiertag f) { return f.Datum.Date == value.Date; }));
                }

                /// <summary>
                ///  var feiertage = MyFeiertagLogic.GetInstance(date.Year);
                ///  var isfeiertag = feiertage.isFeiertag(date);
                /// </summary>
                public List<Feiertag> FesteFeiertage => feiertage.FindAll(delegate (Feiertag f) { return f.IsFix; });

                private MyFeiertagLogic(int year, Bundesland BL)
                {
                    CurrentYear = year;

                    #region fillList

                    DateTime osterSonntag = GetOsterSonntag();
                    DateTime bußuBettag = GetBußuBetTag();

                    feiertage = new List<Feiertag>
                    {

                        //alle Bundesländer
                        new Feiertag(true, new DateTime(year, 1, 1), "Neujahr")
                    };

                    if (BL == Bundesland.BadenWürttemberg || BL == Bundesland.Bayern || BL == Bundesland.SachsenAnhalt)
                    {
                        feiertage.Add(new Feiertag(true, new DateTime(year, 1, 6), "Heilige Drei Könige"));
                    }

                    //if(BL == Bundesland.BadenWürttemberg)
                    //this.feiertage.Add(new Feiertag(false, osterSonntag.AddDays(-3), "Gründonnerstag"));

                    //alle Bundesländer
                    feiertage.Add(new Feiertag(false, osterSonntag.AddDays(-2), "Karfreitag"));

                    if (BL == Bundesland.Brandenburg)
                    {
                        feiertage.Add(new Feiertag(false, osterSonntag, "Ostersonntag"));
                    }

                    //alle Bundesländer
                    feiertage.Add(new Feiertag(false, osterSonntag.AddDays(1), "Ostermontag"));

                    //alle Bundesländer
                    feiertage.Add(new Feiertag(true, new DateTime(year, 5, 1), "Tag der Arbeit"));

                    //alle Bundesländer
                    feiertage.Add(new Feiertag(false, osterSonntag.AddDays(39), "Christi Himmelfahrt"));

                    if (BL == Bundesland.Brandenburg)
                    {
                        feiertage.Add(new Feiertag(false, osterSonntag.AddDays(49), "Pfingstsonntag"));
                    }

                    //alle Bundesländer
                    feiertage.Add(new Feiertag(false, osterSonntag.AddDays(50), "Pfingstmontag"));

                    if (BL == Bundesland.BadenWürttemberg || BL == Bundesland.Bayern || BL == Bundesland.Hessen || BL == Bundesland.NordrheinWestfalen || BL == Bundesland.RheinlandPfalz || BL == Bundesland.Saarland)
                    {
                        feiertage.Add(new Feiertag(false, osterSonntag.AddDays(60), "Fronleichnam"));
                    }

                    if (/*BL == Bundesland.Bayern || */ BL == Bundesland.Saarland)
                    {
                        feiertage.Add(new Feiertag(true, new DateTime(year, 8, 15), "Mariä Himmelfahrt"));
                    }

                    //alle Bundesländer
                    feiertage.Add(new Feiertag(true, new DateTime(year, 10, 3), "Tag der dt. Einheit"));

                    if (BL == Bundesland.Brandenburg || BL == Bundesland.MecklenburgVorpommern || BL == Bundesland.Sachsen || BL == Bundesland.SachsenAnhalt || BL == Bundesland.Thüringen)
                    {
                        feiertage.Add(new Feiertag(true, new DateTime(year, 10, 31), "Reformationstag"));
                    }

                    if (BL == Bundesland.BadenWürttemberg || BL == Bundesland.Bayern || BL == Bundesland.NordrheinWestfalen || BL == Bundesland.RheinlandPfalz || BL == Bundesland.Saarland)
                    {
                        feiertage.Add(new Feiertag(true, new DateTime(year, 11, 1), "Allerheiligen "));
                    }

                    if (BL == Bundesland.Sachsen)
                    {
                        feiertage.Add(new Feiertag(false, bußuBettag, "Buß- u. Bettag"));
                    }

                    //alle Bundesländer
                    feiertage.Add(new Feiertag(true, new DateTime(year, 12, 25), "1. Weihnachtstag"));

                    //alle Bundesländer
                    feiertage.Add(new Feiertag(true, new DateTime(year, 12, 26), "2. Weihnachtstag"));

                    #endregion fillList
                }

                /// <summary>
                /// Berechnet für das CurrentYear den Ostersonntag
                /// </summary>
                /// <returns>Datum für Ostersonntag</returns>
                private DateTime GetOsterSonntag()
                {
                    int g, h, c, j, l, i;

                    g = year % 19;
                    c = year / 100;
                    h = ((c - (c / 4)) - (((8 * c) + 13) / 25) + (19 * g) + 15) % 30;
                    i = h - (h / 28) * (1 - (29 / (h + 1)) * ((21 - g) / 11));
                    j = (year + (year / 4) + i + 2 - c + (c / 4)) % 7;

                    l = i - j;
                    int month = 3 + ((l + 40) / 44);
                    int day = l + 28 - 31 * (month / 4);

                    return new DateTime(year, month, day);
                }

                /// <summary>
                /// Berechnet für das CurrentYear den Buß- und Bettag
                /// </summary>
                /// <returns>Datum für Buß- und Bettag</returns>
                public DateTime GetBußuBetTag()
                {
                    /// Buß- und Bettag ist immer der
                    /// Mittwoch vor dem 23. November
                    DateTime nov = new DateTime(year, 11, 23);

                    for (int i = -1; i < 10; i--)
                    {
                        DateTime d = nov.AddDays(i);
                        if (d.DayOfWeek == DayOfWeek.Wednesday)
                        {
                            return d;
                        }
                    }

                    throw new IndexOutOfRangeException("BußuBetTag konnte nicht gefunden werden");
                }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="dt"></param>
            /// <returns></returns>
            public static DateTime RndTi(DateTime dt)
            {
                return dt.AddMinutes((60 - dt.Minute) % 30);
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="input"></param>
            /// <returns></returns>
            public static DateTime TimeStripSeconds(DateTime dt)
            {
                DateTime d1 = dt.AddSeconds(-dt.Second);
                return TimeStripMilliSeconds(d1);
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="input"></param>
            /// <returns></returns>
            public static DateTime TimeStripMilliSeconds(DateTime dt)
            {
                return dt.AddMilliseconds(-dt.Millisecond);
            }
            public static DateTime TimeAdd1Min(DateTime dt)
            {
                DateTime d1 = dt.AddSeconds(-dt.Second);
                DateTime d2 = d1.AddMilliseconds(-d1.Millisecond);
                return TimeStripSeconds(dt).AddMinutes(1);
            }
            /// <summary>
            /// var date = new DateTime(2010, 02, 05, 10, 35, 25, 450); // 2010/02/05 10:35:25
            /// var roundedUp = date.RoundUp(TimeSpan.FromMinutes(15)); // 2010/02/05 10:45:00
            /// var roundedDown = date.RoundDown(TimeSpan.FromMinutes(15)); // 2010/02/05 10:30:00
            /// var roundedToNearest = date.RoundToNearest(TimeSpan.FromMinutes(15)); // 2010/02/05 10:30:00
            /// </summary>
            /// <param name="dt"></param>
            /// <returns></returns>
            public static DateTime TiRoundUp(DateTime dt, TimeSpan d)
            {
                long modTicks = dt.Ticks % d.Ticks;
                long delta = modTicks != 0 ? d.Ticks - modTicks : 0;
                return new DateTime(dt.Ticks + delta, dt.Kind);
            }
            /// <summary>
            /// var date = new DateTime(2010, 02, 05, 10, 35, 25, 450); // 2010/02/05 10:35:25
            /// var roundedUp = date.RoundUp(TimeSpan.FromMinutes(15)); // 2010/02/05 10:45:00
            /// var roundedDown = date.RoundDown(TimeSpan.FromMinutes(15)); // 2010/02/05 10:30:00
            /// var roundedToNearest = date.RoundToNearest(TimeSpan.FromMinutes(15)); // 2010/02/05 10:30:00
            /// </summary>
            /// <param name="dt"></param>
            /// <returns></returns>
            public static DateTime TiRoundDown(DateTime dt, TimeSpan d)
            {
                long delta = dt.Ticks % d.Ticks;
                return new DateTime(dt.Ticks - delta, dt.Kind);
            }
            /// <summary>
            /// var date = new DateTime(2010, 02, 05, 10, 35, 25, 450); // 2010/02/05 10:35:25
            /// var roundedUp = date.RoundUp(TimeSpan.FromMinutes(15)); // 2010/02/05 10:45:00
            /// var roundedDown = date.RoundDown(TimeSpan.FromMinutes(15)); // 2010/02/05 10:30:00
            /// var roundedToNearest = date.RoundToNearest(TimeSpan.FromMinutes(15)); // 2010/02/05 10:30:00
            /// </summary>
            /// <param name="dt"></param>
            /// <returns></returns>
            public static DateTime TiRoundToNearest(DateTime dt, TimeSpan d)
            {
                long delta = dt.Ticks % d.Ticks;
                bool roundUp = delta > d.Ticks / 2;
                long offset = roundUp ? d.Ticks : 0;

                return new DateTime(dt.Ticks + offset - delta, dt.Kind);
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="t"></param>
            /// <returns></returns>
            public static bool IsDateGreaterMin(DateTime t)
            {
                if (t > Helper.DateTimeParse("19000101 010000"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="lvText"></param>
            /// <returns></returns>
            public static bool IsValidTime(string lvTime)
            {
                return (DateTimeParse("19600512 " + lvTime) != DateTime.MinValue);
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="lvText"></param>
            /// <returns></returns>
            public static bool IsValidDate(string lvDate)
            {
                return (DateTimeParse(lvDate + " 000001") != DateTime.MinValue);
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="fInput"></param>
            /// <returns></returns>
            /// <summary>
            /// 
            /// </summary>
            /// <param name="dt"></param>
            public static DateTime RoundTime(DateTime ti)
            {
                DateTime t = ti;

                if (t.Minute > 45)
                {
                    t = t.AddMinutes(60 - t.Minute);
                }
                else if (t.Minute > 30)
                {
                    t = t.AddMinutes(45 - t.Minute);
                }
                else if (t.Minute > 15)
                {
                    t = t.AddMinutes(30 - t.Minute);
                }
                else if (t.Minute > 0)
                {
                    t = t.AddMinutes(15 - t.Minute);
                }

                return t;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="lvText"></param>
            /// <returns></returns>
            public static string UnformatTimeString(string lvText)
            {
                string s1, s2, s3;
                int i1, i2;

                string pat = @"^\d{1,2}:\d{1,2}:\d{1,2}$";
                Regex r = new Regex(pat, RegexOptions.IgnoreCase);
                if (r.IsMatch(lvText))
                {
                    i1 = lvText.IndexOf(':');
                    i2 = lvText.IndexOf(':', i1 + 1);
                    s1 = lvText.Substring(0, i1);
                    s2 = lvText.Substring(i1 + 1, i2 - i1 - 1);
                    s3 = lvText.Substring(i2 + 1);

                    if (s1.Length < 2)
                    {
                        s1 = "0" + s1;
                    }

                    if (s2.Length < 2)
                    {
                        s2 = "0" + s2;
                    }

                    if (s3.Length < 2)
                    {
                        s3 = "0" + s3;
                    }

                    return s1 + s2 + s3;
                }
                else
                {
                    return string.Empty;
                }
            }

            #endregion

            #region log4net

            /// <summary>
            /// "%date{dd.MM. HH:mm:ss,fff} %5level [%2thread]: %message (%logger) %n"
            /// </summary>
            /// <returns></returns>
            public static ColoredConsoleAppender GetConsoleAppender()
            {
                var lAppender = new ColoredConsoleAppender
                {
                    Name = "Console",
                    Layout = new log4net.Layout.PatternLayout(Const.Constants.LOGLAYOUT),
                    Threshold = log4net.Core.Level.All
                };
                lAppender.ActivateOptions();

                return lAppender;
            }
            public static ColoredConsoleAppender GetConsoleAppender(string Layout, Level l1)
            {
                var lAppender = new ColoredConsoleAppender
                {
                    Name = "Console",
                    Layout = new log4net.Layout.PatternLayout(Layout),
                    Threshold = log4net.Core.Level.Debug
                };
                lAppender.AddMapping(new ColoredConsoleAppender.LevelColors
                {
                    Level = l1,
                    ForeColor = ColoredConsoleAppender.Colors.Yellow | ColoredConsoleAppender.Colors.HighIntensity,
                    BackColor = ColoredConsoleAppender.Colors.Red | ColoredConsoleAppender.Colors.HighIntensity
                });
                //lAppender.AddMapping(new ColoredConsoleAppender.LevelColors
                //{
                //    Level = l2,
                //    ForeColor = ColoredConsoleAppender.Colors.Purple | ColoredConsoleAppender.Colors.HighIntensity,
                //    BackColor = ColoredConsoleAppender.Colors.Cyan| ColoredConsoleAppender.Colors.HighIntensity
                //});
                //lAppender.AddMapping(new ColoredConsoleAppender.LevelColors
                //{
                //    Level = l3,
                //    ForeColor = ColoredConsoleAppender.Colors.White | ColoredConsoleAppender.Colors.HighIntensity,
                //    BackColor = ColoredConsoleAppender.Colors.Blue | ColoredConsoleAppender.Colors.HighIntensity
                //});

                lAppender.ActivateOptions();

                return lAppender;
            }

            #endregion
        }

        /// <summary>
        /// *************************************************************************************************************************
        /// *************************************************************************************************************************
        /// *************************************************************************************************************************
        /// *************************************************************************************************************************
        /// </summary>
        public class Crypto
        {
            private log4net.ILog lvLogger = log4net.LogManager.GetLogger(typeof(Crypto));

            public DESCryptoServiceProvider cryptoBase = new DESCryptoServiceProvider();
            public byte[] key, iv;

            /// <summary>
            /// Generates a new crypto class without a key and ini vektor
            /// </summary>
            public Crypto()
            {
                lvLogger.Verbose(Helper.ProgramStack);

                // 8 Chars!
                key = System.Text.Encoding.ASCII.GetBytes("RTCCrKey");
                iv = System.Text.Encoding.ASCII.GetBytes("RTCCrVct");
            }

            public Crypto(string sKey)
            {
                lvLogger.Verbose(Helper.ProgramStack);

                // 8 Chars!
                string s;
                if (sKey.Length >= 8)
                {
                    s = sKey.Substring(0, 8);
                }
                else
                {
                    s = sKey + "12345678".Substring(0, 8 - sKey.Length);
                }

                key = System.Text.Encoding.ASCII.GetBytes(s);
                iv = System.Text.Encoding.ASCII.GetBytes("InVektor");
            }

            //========================================================================================

            /// <summary>
            /// returns a base64 encrypted string
            /// </summary>
            /// <param name="data">string to be encryted</param>
            /// <returns>base64 encrypted string</returns>
            public string DecryptString(string data)
            {
                lvLogger.Verbose(Helper.ProgramStack);

                byte[] decr = GetFromBase64(data);
                return DecryptData(decr);
            }

            //----------------------------------------------------------------------------------------

            /// <summary>
            /// returns string, where '#'-delimited substrings are decrypted
            /// '#' must occur in pairs at beginning and end of encrypted elements
            /// </summary>
            /// <param name="data">string with partially encrypted data</param>
            /// <returns>decrypted string</returns>
            public string DecryptSubstring(string data)
            {
                lvLogger.Verbose(Helper.ProgramStack);

                string[] sa = data.Split('#');
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < sa.Length; i++)
                {
                    if (i % 2 == 0)
                    {
                        sb.Append(sa[i]);
                    }
                    else
                    {
                        sb.Append(DecryptString(sa[i]));
                    }
                }
                return sb.ToString();
            }

            //----------------------------------------------------------------------------------------

            /// <summary>
            /// returns e decrypted string
            /// </summary>
            /// <param name="data">base 64 encrypted string</param>
            /// <returns>decrypted string</returns>
            public string EncryptString(string data)
            {
                lvLogger.Verbose(Helper.ProgramStack);

                byte[] encr = Encrypt(data);
                return GetToBase64(encr);
            }

            //========================================================================================

            /// <summary>
            /// Converts a string into a BASE64 encoded byte array
            /// </summary>
            public byte[] GetFromBase64(string data)
            {
                lvLogger.Verbose(Helper.ProgramStack);

                try
                {
                    return Convert.FromBase64String(data);
                }
                catch (Exception ex)
                {
                    lvLogger.Error(ex.Message, ex);
                    return null;
                }
            }

            /// <summary>
            /// Converts a BASE64 encoded byte array back to a string
            /// </summary>
            public string GetToBase64(byte[] data)
            {
                lvLogger.Verbose(Helper.ProgramStack);

                return Convert.ToBase64String(data);
            }

            //----------------------------------------------------------------------------------------

            /// <summary>
            /// Encrypts the string
            /// </summary>
            public byte[] Encrypt(string dataToEncrypt)
            {
                lvLogger.Verbose(Helper.ProgramStack);

                ICryptoTransform encryptor = cryptoBase.CreateEncryptor(key, iv);

                //Encrypt the data.
                byte[] data = System.Text.Encoding.Default.GetBytes(dataToEncrypt);
                MemoryStream msEncrypt = new MemoryStream();
                CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);

                //Write all data to the crypto stream and flush it.
                csEncrypt.Write(data, 0, data.Length);
                csEncrypt.FlushFinalBlock();

                //Get encrypted array of bytes.
                return msEncrypt.ToArray();
            }

            //----------------------------------------------------------------------------------------

            /// <summary>
            /// Decrypts a string
            /// </summary>
            public string DecryptData(string dataToDecrypt)
            {
                lvLogger.Verbose(Helper.ProgramStack);

                return DecryptData(System.Text.Encoding.Default.GetBytes(dataToDecrypt));
            }

            /// <summary>
            /// Decrypts a byte array
            /// </summary>
            public string DecryptData(byte[] data)
            {
                lvLogger.Verbose(Helper.ProgramStack);

                ICryptoTransform decryptor = cryptoBase.CreateDecryptor(key, iv);

                //Decrypt the data
                try
                {
                    MemoryStream msDecrypt = new MemoryStream(data);
                    CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);

                    byte[] fromEncrypt = new byte[data.Length];

                    //Read the data out of the crypto stream.
                    int len = csDecrypt.Read(fromEncrypt, 0, fromEncrypt.Length);

                    return System.Text.Encoding.Default.GetString(fromEncrypt, 0, len);
                }
                catch (Exception ex)
                {
                    lvLogger.Error(ex.Message, ex);

                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// *************************************************************************************************************************
        /// *************************************************************************************************************************
        /// *************************************************************************************************************************
        /// *************************************************************************************************************************
        /// </summary>
        public static class KernelFunctions
        {
            private static readonly log4net.ILog lvLogger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            internal struct LASTINPUTINFO
            {
                public uint cbSize;
                public uint dwTime;
            }

            private delegate IntPtr LowLevelKeyboardProc(
                    int nCode, IntPtr wParam, IntPtr lParam);

            [DllImport("user32.dll")]
            public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

            [DllImport("User32.dll")]
            public static extern bool LockWorkStation();

            [DllImport("User32.dll")]
            private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

            [DllImport("Kernel32.dll")]
            private static extern uint GetLastError();

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

            [DllImport("kernel32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            private static extern bool GetPhysicallyInstalledSystemMemory(out long TotalMemoryInKilobytes);

            public static long GetMemSize()
            {
                GetPhysicallyInstalledSystemMemory(out long memKb);
                return memKb / 1024;
            }

            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
            internal struct RECT
            {
                public int Left;
                public int Top;
                public int Right;
                public int Bottom;
                public int Width => Right - Left;
                public int Height => Bottom - Top;
            }
            /// <summary>Returns true if the current application has focus, false otherwise</summary>
            public static bool ApplicationIsActivated
            {
                get
                {
                    //to many calls ---->   lvLogger.Verbose(Helper.ProgramStack);

                    HWND activatedHandle = GetForegroundWindow();
                    if (activatedHandle == IntPtr.Zero)
                    {
                        return false;       // No window is currently activated
                    }

                    int procId = Process.GetCurrentProcess().Id;
                    GetWindowThreadProcessId(activatedHandle, out int activeProcId);

                    return activeProcId == procId;
                }
            }

            [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
            private static extern IntPtr GetForegroundWindow();

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            private static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);
            [System.Runtime.InteropServices.DllImport("user32.dll")]
            internal static extern bool GetUpdateRect(IntPtr hWnd, ref RECT rect, bool bErase);

            [System.Runtime.InteropServices.DllImport("user32.dll")]
            public static extern int GetSystemMetrics(int nIndex);

            public static bool IsTouchDevice
            {
                get
                {
                    lvLogger.Verbose(Helper.ProgramStack);

                    const int SM_MAXIMUMTOUCHES = 95;

                    int v = GetSystemMetrics(SM_MAXIMUMTOUCHES);
                    if (v < 1)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }

            public static bool Wait4OpenProcess(string MainTitleShort, int WaitSeconds)
            {
                lvLogger.Verbose(Helper.ProgramStack);

                int cnt = 0; double SleepingTime = Const.Constants.ONESECOND / 4f; double MaxCounter = WaitSeconds * Const.Constants.ONESECOND / SleepingTime;
                while (KernelFunctions.OpenWindowGetter.IsProcessOpen(MainTitleShort) != null && cnt < MaxCounter) //max 10 sec warten
                {
                    cnt++;
                    lvLogger.WarnFormat("'{0}' ist nach {1} Durchläufen noch offen", MainTitleShort, cnt);
                    System.Threading.Thread.Sleep((int)SleepingTime);
                    if (cnt == (int)(MaxCounter / 2f))
                    {
                        lvLogger.WarnFormat("will try to kill '{0}' now", MainTitleShort);
                        KernelFunctions.OpenWindowGetter.KillProcess(MainTitleShort);
                        KernelFunctions.OpenWindowGetter.CloseForm(MainTitleShort);
                    }
                }
                if (KernelFunctions.OpenWindowGetter.IsProcessOpen(MainTitleShort) != null)
                {
                    lvLogger.FatalFormat("'{0}' ist nach Ablauf der Warte-Frist noch offen", MainTitleShort);
                    return false;
                }
                else
                {
                    lvLogger.FatalFormat("'{0}' ist jetzt geschlossen", MainTitleShort);
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
                        foreach (KeyValuePair<IntPtr, string> window in KernelFunctions.OpenWindowGetter.GetOpenWindows())
                        {
                            IntPtr handle = window.Key;
                            if (window.Value.ToLower().Contains(f.ToLower()))
                            {
                                return true;
                            }
                        }

                        return false;
                    }
                    catch (Exception ex) { lvLogger.Error(ex.Message, ex); return false; }
                }
                public static void CloseForm(string f)
                {
                    lvLogger.Verbose(Helper.ProgramStack);

                    try
                    {
                        foreach (KeyValuePair<IntPtr, string> window in KernelFunctions.OpenWindowGetter.GetOpenWindows())
                        {
                            IntPtr handle = window.Key;
                            if (window.Value.ToLower() == f.ToLower())
                            {
                                SendMessage(handle, WM_NCDESTROY, (uint)IntPtr.Zero, (uint)IntPtr.Zero);
                            }
                        }
                    }
                    catch (Exception ex) { lvLogger.Error(ex.Message, ex); }
                }
                public static Process IsProcessOpen(string f)
                {
                    lvLogger.Verbose(Helper.ProgramStack);

                    try
                    {
                        lvLogger.InfoFormat("searching for Process with name='{0}'", f);
                        Process currentProcess = Process.GetCurrentProcess();
                        IEnumerable<Process> RunningProcesses = Process.GetProcesses().
                           Where(pr => pr.ProcessName.ToLower() == f.ToLower() && pr.Id != currentProcess.Id);

                        foreach (Process process in RunningProcesses)
                        {
                            lvLogger.InfoFormat("found '{0}' open ProcessID '{1}' with '{2}'", process.ProcessName, process.Id, process.MainWindowTitle);
                            return process;
                        }

                        return null;
                    }
                    catch (Exception ex) { lvLogger.Error(ex.Message, ex); return null; }
                }
                public static void KillProcess(string f)
                {
                    lvLogger.Verbose(Helper.ProgramStack);

                    try
                    {
                        Process currentProcess = Process.GetCurrentProcess();
                        IEnumerable<Process> RunningProcesses = Process.GetProcesses().
                            Where(pr => pr.ProcessName.ToLower() == f.ToLower() && pr.Id != currentProcess.Id);

                        foreach (Process process in RunningProcesses)
                        {
                            lvLogger.InfoFormat("will kill ProcessID '{0}' with '{0}' now", process.Id, process.MainWindowTitle);
                            process.Kill();
                            lvLogger.InfoFormat("ProcessID '{0}' with '{1}' is killed", process.Id, process.MainWindowTitle);
                        }
                    }
                    catch (Exception ex) { lvLogger.Error(ex.Message, ex); }
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
}