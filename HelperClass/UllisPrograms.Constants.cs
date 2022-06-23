using System;
using System.Drawing;

namespace UllisPrograms
{
    namespace Const
    {
        public static class BUFFER //keine Konstanten!
        {
            public static bool ShutDownRunning = false;
            public static DateTime StartTime = DateTime.Now;
        }

        public static class Constants
        {
            public static readonly char[] delimiterChars = { ';', ',', '\t' };
            public const string ACTIVITY = "ACTIVITY";
            public const string ADDGREEN = "AddGreen";
            public const string ADVNAME = "AdvCont";
            public const string AGGRERROR = "3F";
            public const string AKTENDE = "AktEnde";
            public const string APPREFMS = ".appref-ms";
            public const string ARIAL = "Arial";
            public const string ARROWRIGHT = " ► ";
            public const string ASSEMBLY_RESOURCES = "UllisPrograms.Resources.resources.dll";
            public const string ASTERISK = "*";
            public const string ASYNC = "$ASYNC$";
            public const string CAPS = "CAPS";
            public const string NUM = "NUM";
            public const string AUSGANGS_BUCHEN_BWART = "AusgangBuchenBwArt";
            public const string AUTO = "Auto";
            public const string AUTO_FILL_NEXT_MOVE = "AutoFillNextMove";
            public const string AUTOLOGON = "AutoLogon";
            public const string AUTORUN = @"Software\Microsoft\Windows\CurrentVersion\Run";
            public const string AUTOSTART = "AutoStart";
            public const string BASEPOP = "BasePopUp";
            public const string BESTLIST_GROUPMODE = "BestListGroupMode";
            public const string BIE = "Bielefeld";
            public const string BIE_RTC = "bie-rtc1.de.UllisPrograms.com";
            public const string RTCINFO = "RTCInfo";
            public const double BLUELUMINANCE = 0.072187;
            public const string BMW = "BMW";
            public const string BOLD = "Bold";
            public const string BORDER = "Border";
            public const string BTNACTION = "btnAction";
            public const string INITIALKEY = "12056024";
            public const string INITIALUSER = "ini^ppc";
            public const string INITIALPWD = "^^pwd^^";
            public const string BTNBACK = "btnBack";
            public const string BUBBLE = "Bubble";
            public static readonly string VNC = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),"UltraVNC","vncviewer.exe");
            public const string CANNOCK = "Cannock";
            public const string CARDABGEZOGEN = "$$AB$$";
            public const string CENTER = "Center";
            public const string CFGRECPT = "CFGRECPT";
            public const string CFSCRAP = "CFSCRAP";
            public const string CFWASTE = "CFWASTE";
            public const string CFYIELD = "CFYIELD";
            public static readonly char[] CHARDELIMITER = new char[] { ' ', ',', '.', ';', '\n', '\t' };
            public static readonly char[] SpecialSymbols = new char[] { '|', '{', '}' };
            public const string CLIENT = "Client";
            public const string CLIENTINI = "client.ini";
            public const string QUANTITY = "Quantity";
            public const string CMD = "cmd.exe";
            public static readonly Color DISABLED_COLOR = Color.FromArgb(215, 215, 215);
            public static readonly Color UllisPrograms_BLUE = Color.FromArgb(22, 87, 136);
            public const string CONNSECTION = "Connection";
            public const char CPUENKTCHEN = '∙';
            public const string CREA_LE_DIRECT = "CreaLEDirect";
            public const string CREDITS = "Program is created with support of Ulrich Müller, Heino Berger, Rainer Mittag, Michael Kraft, Susana Sánchez Fernández, Paul Bergen, Frank Effnert, Johann Bergen, Daniel Valentino";
            public const string CROSSCHECK_LE = "CrossCheckLE";
            public const char CSPLITTER = '°';
            public const string CULTURE = "Culture";
            public const string CURR_UICULT = "CurrentUICulture";
            public const string DATA_FROM_NOTIFY = "OnDataReceivedFromNotify";
            public const string DATA_OPEN_REQUEST = "DataOpenRequest";
            public const string DATEAMIFORMAT = "yyyy.MM.dd HH:mm:ss";
            public static readonly DateTime NULLDATE = DateTime.Parse("1960-05-12 11:11:11");
            public const string DATUM = "Datum";
            public const string DAUER = "Dauer";
            public const string DE = "de";
            public const string DEDE = "de-DE";
            public const string DELIMITER = @"$\$\$";
            public const string DEMO_LE = @"00010100924203005181";
            public const string DESCRIPTION = "Description";
            public const string DIFF_LGPLA = "DiffLgPlatz";
            public const string DIFF_LGTYP = "DiffLgTyp";
            public const string DOPPELPUNKT = @":";
            public const string DOUBLE_LE_WARN_WHILE_TRANSP = "DoubleLEWarningWhileTransporting";
            public const string DUMMY_LE = "10100000001010001";
            public const string DUMMY_MAT = "00001";
            public const string DUMY = "DUMY";
            public const string EINGANG_BUCHEN_BWART = "EingangBuchenBwArt";
            public const string EMPTY_ENTRY = SPACE + SPACE + SPACE + SPACE + SPACE + SPACE;
            public const string EMPTY_TKNUM = "0000000000";
            public const string EN = "en";
            public const string ENABLE_KEYBOARD_SCANNER = "EnableKeyboardScanner";
            public const string ENABLE_SERIALPORT = "EnableSerialPort";
            public const string ENUS = "en-US";
            public const string ERRORID = "ErrorID";
            public const string ERSALUD = "LUDErsa";
            public const string EXPERTMODE = "ExpertMode";
            public const string ExpertMode = "ExpertMode";
            public const string EXTERNAL = "external";
            public const string FACTY = "Factory1";
            public const string FEHLERKL = "(Fehler)";
            public const string FILELOGPREFIX = "Log";
            public const string FINALCOMAND = "FinalComand";
            public const string FIRSTDATE = "1900-01-01";
            public const string FLAG_GUI_TEST = "--GUI-TEST";
            public const string FLEXOWNERDRAW = "GridDrawCell";
            public const string FLEXSORT = "FlexSort";
            public const string FLEXTRUCK = "flexTruck";
            public const string FOCUSED = "Focused";
            public const string FROM_PSA_WITHOUT_QUAN = "FromPSAWithoutQuantity";
            public const string FUTURE_USE_054 = "FutureUse054";
            public const string FUTURE_USE_082 = "FutureUse082";
            public const string FUTURE_USE_084 = "FutureUse084";
            public const string GEHT = "geht";
            public const double GREENLUMINANCE = 0.715158;
            public const int GRID_FIRST_DATA_ROW = 1;
            public const int GRID_HEADER_ROW = 0;
            public const char GTLTABGS = '\x001D';
            public const string GTLTABSTART = "[)>";
            public const string HAEKCHEN = "√";
            public const string HELPFILE = "HelpFile";
            public const string HELPPAGELUD = @"\\websrv1\Web\vWebAuth_MobileBDE\Ludwigsfelde.htm";
            public const string HISTSECTION = "Historie";
            public const string HONDAPROD = "HondaProd1";
            public const string I4MM_SUBS = "SUBS";
            public const string I4MM_SUBSCRIPT_SECTION = "I4MMSubscriptions";
            public const string I4MM_USUB = "USUB";
            public const string ICONORDER = "IconOrder";
            public const int ICONSIZE = 55;
            public const string INFINITY = "∞";
            public const string INTERRUPT = "INTERRUPT";
            public const string INVENTUR = "Inventur";
            public const string INVENTUR_SICHERUNG = "InventurSicherung";
            public const string INVLFDNR = "InvLfdNr";
            public const string INVSECTION = "Stocktaking";
            public const string KEY = "Key";
            public const string KOMMT = "kommt";
            public const string LANGUAGE = "Language";
            public const string LAST_GOOD_HOST = "LastGoodHost";
            public const string LAST_GOOD_LOGIN = "LastGoodLogin";
            public const string LAST_GOOD_USERKEY = "LastGoodUserKey";
            public const string LASTFORKLIFT = "LastForkLift";
            public const string LECREADIRECT = "LE-Anlage direkt in SAP";
            public const string LECREASQL = "LE-Anlage via SQL";
            public const string LEWITHID = "LEwithID";
            public const string LGPLA = "LgPla";
            public const string LGTYP = "LgTyp";
            public const string LOADORDER = "LOAD_ORDER";
            public const string LOCALREQUERY = "LocalRequery()";
            public const string LOCALSETITEMS = "LocalSetItems()";
            public const string LOCALSTOREDATA = "LocalStoreData";
            public const string LOGLAYOUT = "%date{dd.MM. HH:mm:ss,fff} %9level [%2thread]: %message (%logger) %n";
            public const string LOGOFF_ONLY = "LogoffOnly";
            public const string LUD = "Ludwigsfelde";
            public const string ORDER = "Order";
            public const string REASON = "REASON";
            public const string LUD_RTC = "ldw-rtc1.de.UllisPrograms.com";
            public const string MAIL_4_ABGLEICH = "Mail4Abgleich";
            public const string MAINAPPS = "MainApps";
            public const string MANY_LABELS_ALLOWED = "ManyLabelsAllowed";
            public const string MARKROW = "MarkRow";
            public const string MASTERKEY = "Ulli011024!";
            public const string MATDOCALL = "11111111111111111111";
            public const string MATDOCMFBF = "11110010100011011111";
            public const string MATDOCWEAVIS = "11111111111001011111";
            public const string MATMENGETI = "<<<<<MatMengEti>>>>>";
            public const string MAXINACTTIME = "MaxInActiveTime";
            public const long MAXQUAN = 200000;
            public const string MAXRUNTIME = "MaxRunTime";
            public const string MDE = "MDE";
            public const string MDEUSER = "MDE";
            public const string MEMSIZE = "MemSize";
            public const string MESSAGE = "Message";
            public const string MESSAGEID = "MessageID";
            public const string MESSAGETYPEID = "MessageTypeID";
            public const string METRICS = @"Control Panel\Desktop\WindowMetrics";
            public const int MILLISEC100 = 100;
            public const string MONITOR_ASK_OTHER_STORE = "MonitorAskOtherLag";
            public const string MONITOR_SCAN = "MonitorScan";
            public const string MQTT_NEW_REQ_405 = "BIE/Logistics/PlantTrans/ReqNew/0010/1088/100/405/+/from/MAT";
            public const string MQTT_UKL_HYBRID = "BIE/Logistics/Signals/0010/591/0140/Box";
            public const string MUSTER = "Muster";
            public const string NA = "n/a";
            public const string NACHFOLGER = "Nachfolger";
            public const string NLA = "NLA";
            public const string NO = "no";
            public const int NOAKKU = -23062018;
            public const string NOHTML = @"http://pleasewait.co.uk/";
            public const string NONE = "none";
            public const string NOPWD = "XXXXXXXXX";
            public const string NOTEST = "NoTest4Intruders";
            public const string OBSOLET = "obsolet";
            public const string OFF = "off";
            public const string HEADERTEXT = "HeaderText";
            public const string OFFLINE = "offline.txt";
            public const string OFFLINEMODUS = "offline";
            public const string OK = "ok";
            public const string ON = "on";
            public const int ONESECOND = 1000;
            public const string ONLINEMODUS = "online";
            public const string OPC = "OPC";
            public const string OPERATION = "OPERATION";
            public const string OSRAMLISTE = "OsramListe";
            public const string PASSWORD = "230618";
            public const string PIPE = @"|";
            public const string PIPEREP = "<<<Pipe>>>";
            public const string PLANT = "Plant";
            public const string WORKCNT = "WorkCntColumn";
            public const string PLANTBRA = "0010";
            public const string PLANTLUD = "0110";
            public const string PLANZEIT = "Planzeit";
            public const float POPUPRATIO = 1.5f;
            public const int PORT1800 = 1800;
            public const int PORT1810 = 1810;
            public const string PREFIX_KEYBOARD_SCANNER = "PrefixKeyboardScanner";
            public const string PRIVSECTION = "Private";
            public const string PROFUNDLY_DEAF = "ProfundlyDeaf";
            public const string PROGDOWNLOADFOLDERNAME = "TmpProg";
            public const string PRTCLIENTKEY = "AsyncLabel|MultiLabel";
            public const string PUENKTCHEN = "∙";
            public const string PVB340 = "PVB-340";
            public const string UPDICON = "UPDICON";
            public const string MATERIAL = "Material";
            public const string QUESTION = "?";
            public const double REDLUMINANCE = 0.212655;
            public const string REGULAR = "Regular";
            public const string REMOVERED = "RemoveRed";
            public const string REQ_BY_EINLAGERN = "TrReqByEinlagern";
            public const string RESETINVNR = "neuer Wert";
            public const string RESTARTNAME = "ThisIsRestart";
            public const string RIGHTTOP = "RightTop";
            public const string ROOT = "ROOT";
            public const string RTC2 = "bie-rtc2.de.UllisPrograms.com";
            public const string RTCLOCALHOST = "LOCALHOST";
            public const string RTCMITTAG = "BIE162D025: PC Mittag";
            public const string RTCMUELLER = "BIE154D021: PC Müller";
            public const string RTCMUELLERLPT = "BIE182N047: Laptop Müller";
            public const string SAMMELMODUS = "document_spreadsheet_64x64";
            public const string SAMMELTRANSPORT = "SammelTransport";
            public const string SAP = "SAP";
            public const string SAPTRANS = @"http://websrv1/vWebAuth_SAPTrans/{0}";
            public const string SAVEALL = "SaveAll";
            public const string SBRA = "110";
            public const string SCOPEBRA = "14%";
            public const string SCOPEBRADESC = Constants.SCOPEBRA + ": Brackwede alle";
            public const string SCOPEBRAPRS = "141%";
            public const string SCOPEBRAPRSDESC = Constants.SCOPEBRAPRS + ": Brackwede Preßwerk";
            public const string SCOPEBRAZB = "142%";
            public const string SCOPEBRAZBDESC = SCOPEBRAZB + ": Brackwede ZB";
            public const string SCOPELUD = "57%, 34%";
            public const string SCOPELUDDESC = Constants.SCOPELUD + ": Ludwigsfelde alle";
            public const string SCREEN_RATIO_TRANSP = "ScreenRatioTransp";
            public const string SCREENRATIOCC = "ScreenRatioCC";
            public const string SCROLLHEIGHT = "ScrollHeight";
            public const string SCROLLWIDTH = "ScrollWidth";
            public const string SEC_DELAY_BOOKING = "SecDelayBooking";
            public const string SEC_STAY_BLINKING = "SecStayBlinking";
            public const string SEC_STAY_GREEN = "SecStayGreen";
            public const string SENDMSGCLIENT = @"SendMsgFromClient";
            public const string SERIALPORT = "SerialPort";
            public const string SERPORT = "COM1,9600,n,8,1";
            public const string SET_GRID_CELLFORMAT = "SetGridCellFormat";
            public const string SFCMM = "MDE-SFCMM";
            public const string SHARED = "UllisPrograms.mobileBDE.Shared";
            public const string SHOW_AKKU_BUTTON = "ShowAkkuButton";
            public const string SHOW_WLAN_BUTTON = "ShowWLANButton";
            public const string SHOWORIGIN = "ShowOrigin";
            public const string SHOWTECHNAMES = "ShowTechNames";
            public const string SHUTDOWN = "SHUTDOWN";
            public const string SIZE = "Size";
            public static readonly Size BTNSZ = new Size(80, 95);
            public const string SLASH = @"/";
            public const string SLDW = "LDW";
            public const string SLEEP = "SLEEP";
            public const string SLEEPPOPUP = "RuheZustandPopUp";
            public const string SLUD = "LUD";
            public const string SMALLFONT = "SmallFont";
            public const string SOFORTMODUS = "document_64x64";
            public const string SPACE = " ";
            public const string SPACE2 = SPACE+ SPACE;
            public const string SPACEDOPPELPUNKT = SPACE + DOPPELPUNKT + SPACE;
            public const string SQLDat3N = "yyyy.MM.dd HH:mm:ss.fff";
            public const string STAFF = "STAFF";
            public const string STAFFCOUNT = "STAFFCOUNT";
            public const string STANDARD = "Standard";
            public const int STANDARD_PORT_BIE = 1810;
            public const int STANDARD_PORT_LUD = 1800;
            public const string STANDARD_RES_FORM = "StandardResForm";
            public const string STANDARDPRT = "StandardPrinter";
            public const string STAPLER_COCKPIT_TRANSPORT_VIEW = "StaplerCockpitTransportView";
            public const string START = "Start";
            public const string STATUS_CHANGE_ALLOWED = "StatusChangeAllowed";
            public const float STEPOPACITY = 0.05f;
            public const string STOBIN = "StoBin";
            public const string STOLOC = "StoLoc";
            public const string STOP = "Stop";
            public const string STOREDEF = "StorageDefaults";
            public const string STORNO = "Storno";
            public const string STOTYP = "StoTyp";
            public const string STRIKEOUT = "StrikeOut";
            public static readonly string CRLF = Environment.NewLine;
            public static readonly string CRLF2 = CRLF + CRLF;
            public static readonly string CRLF4 = CRLF2 + CRLF2;
            public static readonly string SPLITTER = CSPLITTER.ToString();
            public const string STRRESTART = "ThisIsRestart";
            public const string SUBSCRIPTION1 = "Subscription1";
            public const string SUBSCRIPTION2 = "Subscription2";
            public const string SUBSCRIPTION3 = "Subscription3";
            public const string SUBSCRIPTION4 = "Subscription4";
            public const string SUBSCRIPTION5 = "Subscription5";
            public const string SUFFIX_KEYBOARD_SCANNER = "SuffixKeyboardScanner";
            public const string SUMALL = "Σ All";
            public const string SUMME = @"Σ";
            public const string SUPERUSER = @"ulrich.mueller@de.UllisPrograms.com";
            public const string SWUH = "WUH";
            public const string SYSFOLDER = "System_Folder";
            public static readonly System.Globalization.CultureInfo ENGLCULT = new System.Globalization.CultureInfo(Constants.EN, true);
            public static readonly System.Globalization.CultureInfo GERCULT = new System.Globalization.CultureInfo(Constants.DEDE, true);
            public const string SYSUSER = "SYSTEM";
            public const string TB = "TB";
            public const string TERMINALID = "TerminalID";
            public const string TEXT = "Text";
            public const string TIMEMANUAL = "TimeManual";
            public const string TMPUSER = "TMP";
            public const string TRANFER_ZTHU_LGORT = "TransferZTHULgOrt";
            public const string TRANFER_ZTHU_LGPLA = "TransferZTHULgPla";
            public const string TRANFER_ZTHU_LGTYP = "TransferZTHULgTyp";
            public const string TRANS_DEV = "TransportDevice";
            public const string TRANS_DIRECTION = "TransDirection";
            public const string TRANSFER_ZTHU = "TransferZTHU";
            public const string TREEHEADER = "TreeHeader";
            public const string TRUE = "true";
            public const string TUBHELPER = "Class TUBHelper()";
            public const string TXTOPC = "txtOPC_";
            public const string UKL_HYBRID_FERTVERSION = "UKLHybridFertVersion";
            public const string UKL_HYBRID_MATNR = "UKLHybridMatnr";
            public const string UKL_HYBRID_PRINTER = "UKLHybridPrinter";
            public const string UKLEINLAGERNFERT = "UKL einlagern Fert";
            public const string UKLEINLAGERNHALB = "UKL einlagern Halb";
            public const string UKLSWITCHER = "UKL_SWITCH_TO_PRINT";
            public const string UKLSWITCHEREXTENSION = "UKL";
            public const string UNDERLINE = "_";
            public const string UNIT = "Unit";
            public const string UNVISIBLE = "unvisible";
            public const string USE_DATALOGIC_ACOUSTICS = "UseDataLogicAcoustics";
            public const string USER_DEF_UICULT = "m_userDefaultUICulture";
            public const string USERSETTINGS = "userSettings";
            public const string VALUE = "Value";
            public const string VERSAND = "Versand";
            public const double VERSPASSWORD = 171117;
            public const string VORGÄNGER = "Vorgänger";
            public const string VORNE = "Vorne";
            public const string WAREHOUSE = "WareHouse";
            public const string WERT = "Wert";
            public const string WINDOWTEXT = "<mobileBDE>";
            public const string WINNLAKEINE = "keine";
            public const string WINNLAPLA = "Platinen";
            public const string WINNLAPROD = "Prod";
            public const int WLAN_PIC_SIZE = 128;
            public const string WM_STORARGE_LOCATION = "WMLagerort";
            public const string WORKCENTER_REWORK = "WorkCenterRework";
            public const int SCROLLBARWIDTH = 30;
            public const string X = "X";
            public const int Y1900 = 1900;
            public const int YEARS99 = 52034400;
            public const string YES = "yes";
            public const int ZERO = 0;
            public const string ZTPM = "ZTPM";

            public class GlobalFontSize
            {
                public static Font DisplayFont = new Font("Arial Rounded MT Bold", 30, FontStyle.Regular, GraphicsUnit.Pixel);
                public static Font VeryBigFont = new Font("Arial", 40, FontStyle.Regular, GraphicsUnit.Pixel);
                public static Font BigFont = new Font("Arial", 30, FontStyle.Regular, GraphicsUnit.Pixel);
                public static Font MediumFont = new Font("Arial", 20, FontStyle.Regular, GraphicsUnit.Pixel);
                public static Font SmallFont = new Font("Arial", 15, FontStyle.Regular, GraphicsUnit.Pixel);
                public static Font MicroFont = new Font("Arial", 9, FontStyle.Regular, GraphicsUnit.Pixel);

            }

            public static class CONTROL_RATIO
            {
                public static readonly Size BIG = new Size(180, 60);
                public static readonly Size MIDDLE = new Size(120, 40);
                public static readonly Size SMALL = new Size(72, 24);
            }

            public static class TPM
            {
                public const string LAGERORT = "TPMLagerort";
                public const string MTART = "TPMMtArt";
                public const string PLATZ_DEFAULT = "TPMPlatzDefault";
                public const string SECTION = "TPM";
                public const string TYP_DEFAULT = "TPMTypDefault";
                public const string DIFF = "TPMDiff";
            }

            public static class WM
            {
                public const string LUD = "200";
                public const string TUB = "100";
                public const string WUHAN = "800";
            }

            public static class LESTATUS
            {
                public const string KOMMISSIONIERT = "X";
                public const string ENTLEERT = "H";
                public const string ISWE = "V";
                public const string NOTWE = "U";
                public const string FREI = "F";
                public const string QBEST = "Q";
                public const string VORPLAN = Constants.UNDERLINE;
                public const string RETOURE = "R";
                public const string SPERR = "S";
                public const string VERSANDSPERR = "V";
                public const string COILPROBE = "C";
            }

            public static class SAPMSG
            {
                public const string ABORT = "A";
                public const string ERROR = "E";
                public const string WARNING = "W";
                public const string SUCCESS = "S";
                public const string INFO = "I";
                public const string DUMP = "X";
            }

            public static class UKL
            {
                public const string USER = "UKL";
            }
            public static class SPECIAL
            {
                public const string LGORT1 = "_LgOrt1";
                public const string LGORT2 = "_LgOrt1";
                public const string LGPLA1 = "_LgPla1";
                public const string LGPLA2 = "_LgPla2";
                public const string LGTYP1 = "_LgTyp1";
                public const string LGTYP2 = "_LgTyp2";
            }
            public static class BWART
            {
                public const string BWE = "101;102;301;302";
                public const string B344 = "344";
                public const string B551 = "551";
                public const string B561 = "561";
                public const string B562 = "562";
                public const string BMATUMBUCH = "311";
            }

            public static class FLT
            {
                public static class STATUS
                {
                    public const string EMPTYRUN = "ER";
                    public const string OHNEFAHRER = "LO";
                    public const string SONDEREINSATZ = "OP";
                    public const string VERFUEGBAR = "AV";
                    public const string PAUSE = "PS";
                    public const string TRANSPORT = "TR";
                }

                public static class EVENT
                {
                    public const string WORKEND = "WEN";
                    public const string WORKSTART = "WST";
                    public const string WORKPAUSE = "WPS";
                    public const string STARTOTHEROP = "SOP";
                }
            }

            public static class LAST
            {
                public const string AVIS = "LastAvis";
                public const string COSTCENTER = "LastCostCenter";
                public const string KANBANID = "LastKanbanID";
                public const string AVIS_MTART = "LastAvisMtArt";
                public const string LE = "LastLE";
                public const string MAT = "LastMaterial";
                public const string ORDER1 = "LastOrder1";
                public const string ORDER2 = "LastOrder2";
                public const string TRANSPORT = "LastTransport";
                public const string TRANSPORT_TYPE = "LastTransportType";
                public const string WORKCENTER = "LastWorkCenter";
                public const string MESSAGEID = "LastMessageID";
                public const string TEILEFAMILIE = "LastTeilefamilie";
                public const string OSC_MODE = "LastOSCMode";
            }

            public static class Format
            {
                public const string DMYYHMMSSFFFFFF = "d.M.yy H:mm:ss,fffff";
                public const string DMHMMSSFFFFFF = "dd.MM HH:mm:ss,fffff";
                public const string DMYHMMSSFFFFFF = "dd.MM.yy HH:mm:ss,fffff";
                public const string DMYHMMSS = "dd.MM.yy HH:mm:ss";
                public const string DMHHMMSSF = "d.M H:mm:ss,f";
                public const string DMHHMMSS = "d.M H:mm:ss";
                public const string DMYYHMM = "d.M.y H:mm";
                public const string DDMMYYYYHHMM = "dd.MM.yyyy HH:mm";
                public const string DDMMYYYYHHMMSS = "dd.MM.yyyy HH:mm:ss";
                public const string DMHMM = "d.M H:mm";
                public const string DMYHMMwithCR = "d.M.y\r\nH:mm";
                public const string DDMMYYYYHHMMSSwithCR = "d.M.yyyy\r\nH:mm:ss";
                public const string DMHMMwithCR = "d.M\r\nH:mm";
                public const string DMYY = "d.M.yy";
                public const string HMM = "H.mm";
                public const string YYYYMMDD = "yyyyMMdd";
                public const string HHMMSS = "HHmmss";
                public const string DDMMYY = "dd.MM.yy";
                public const string N0 = "#,##0;-#,##0;' '";
                public const string N0ODZ = "#0;-#0;' '";
                public const string N1 = "#,##0.0;-#,##0.0;' '";
                public const string N2 = "#,##0.00;-#,##0.00;' '";
                public const string N3 = "#,##0.000;-#,##0.000;' '";
                public const string N6 = "#,##0.000000;-#,##0.000000;' '";
                public const string KILO = "#,##0 kg;-#,##0 kg;' '";
                public const string TG = "#,##0 Tg;-#,##0 Tg;' '";
                public const string STD = "#,##0.00 Std;-#,##0.00 Std;' '";
                public const string SEC = "#,##0.000 sec;-#,##0.000 sec;' '";
                public const string MIN = "#,##0.000 min;-#,##0.000 min;' '";
            }

            public static class TYPE
            {
                public const string SU = "SU";
                public const string MAT = "MAT";
                public const string TPM = "EMP";
                public const string WKZ = "TOL";
                public const string COIL = "COIL";
                public const string KAUF = "KAUF";
            }

            public static class DATA
            {
                public const string ALREADYBOOKED = "DataAlreadyBooked";
                public const string HASERROR = "DataHasError";
                public const string MARKFORDEL = "MarkForDel";
                public const string MARKFORACTION = "MarkForAction";
                public const string UNMARKED = "UnMarked";
                public const string MARKED = "Marked";
                public const string OPENREQUEST = "OpenRequest";
                public const string EMPTY = "Empty";
                public const string OCCUPIED = "Occupied";
                public const string CHECKED_AND_OK = "DataCheckedAndOK";
                public const string AVAILABLE = "Available";
                public const string DEFAULT_VALUE = "DefaultValue";
                public const string LINK = "TheLink";
                public const string PRECHECKHASERROR = "DataPreCheckHasError";
                public const string CHECKEDBUTNOTSTORED = "DataCheckedButNotStored";

                public static class STATUS
                {
                    public const string Q = "Status_Q";
                    public const string R = "Status_R";
                    public const string S = "Status_S";
                    public const string F = "Status_F";
                    public const string X = "Status_X";
                    public const string K = "Status_K";
                }
            }

            public static class FARBE
            {
                public static readonly Color Status_Q = Color.Orange;
                public static readonly Color OpenRequest = Color.Aqua;
                public static readonly Color CheckedAndOK = Color.LightGreen;
                public static readonly Color HasError = Color.DarkOrange;
                public static readonly Color AlreadyBooked = Color.Pink;
                public static readonly Color PreCheckHasError = Color.PeachPuff;
                public static readonly Color CheckedButNotStored = Color.Yellow;
                public static readonly Color Link = Color.DarkBlue;
            }

            public static class DIN
            {
                public const string A4 = "A4";
                public const string A5 = "A5";
            }

            public static class GSSUPPORT
            {
                public const string ERR_IN_STRUCT = "Wrong structure in RTC-Answer";
                public const string INIEXE = "INIHandler executes '{0}'";
            }

            public static class ONEVENT
            {
                public const string SAVEFIELDVALUES = "SaveFieldValues()";
                public const string GRIDCLICK = "GridClick()";
                public const string SENDLOGGING = "SendLogging()";
                public const string OWNERDRAWCELL = "GridDrawCell()";
                public const string ONDATASCANNEDWHENACTIVE = "OnDatacannedWhenActive()";
                public const string ONDESELECTED = "OnDeselected()";
                public const string ONDESELECTING = "OnDeselecting()";
                public const string ONSELECTED = "OnSelected()";
                public const string ONSELECTING = "OnSelecting()";
                public const string GRIDCLICKED = "GridClickEvent()";
                public const string BTNCHECK_CLICK = "btnCheck_Click()";
                public const string ACTION = "Action()";
                public const string RESIZE_HANDLER = "ResizeHandler()";
                public const string GRID_DRAW_CELL = "GridDawCell()";
                public const string GRID_AFTER_EDIT = "GridAfterEdit";
            }
        }
    }
}
