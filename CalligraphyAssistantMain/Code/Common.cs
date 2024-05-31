using CefSharp;
using DirectShowLib;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PropertyChanged;
using Qiniu.Http;
using Qiniu.Storage;
using SharpDX;
using SharpDX.Text;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Serialization;
using Encoding = System.Text.Encoding;
using GDIP = System.Drawing;
using Path = System.IO.Path;
using Rect = System.Windows.Rect;


namespace CalligraphyAssistantMain.Code
{
    [AddINotifyPropertyChangedInterface]
    public static class Common
    {

        private static int changePresetDelay = 200;
        private static bool isDebug = false;
        private static float dpi = 0;
        public static event Action<string> ShowTipCallback = null;
        public static NavigationInfo NavigationInfo = null;
        public static string ImageWebHost = "https://images.aipingke.net/";
        public static string WebAPI = "http://" + WebSiteUrl + ":" + WebPort + "/api";
        public static string UserLogin = WebAPI + "/user/login";
        public static string GetClassRoomList = WebAPI + "/room/page?page=0&size=100&idSchool=";
        public static string GetClassList = WebAPI + "/class/list";
        public static string GetSeatList = WebAPI + "/seat/list?semester=2&idClass=";
        public static string GetSemester = WebAPI + "/year/list";
        public static string GetCurrentClassRoom = WebAPI + "/room/read";
        public static string GetImageThemes = WebAPI + "/type/list";
        public static string GetImageThemeDetails = WebAPI + "/image/list?idType=";
        public static string GetLessonList = WebAPI + "/lesson/list?idClass=";
        public static string CreateLessonList = WebAPI + "/lesson/create";
        public static string GetWorkList = WebAPI + "/stu/work/page";
        public static string GetWorkListV2 = WebAPI + "/stu/work/list";
        public static string SubmitWork = WebAPI + "/stu/work/create";
        public static string CorrectWork = WebAPI + "/stu/work/update";
        public static string GetUploadToken = WebAPI + "/qiniu/token";
        public static string SelfSubmitWork = WebAPI + "/stu/work/create/self";

        public static string GetCourseList = WebAPI + "/user/course";
        public static string GetResourceDirectory = WebAPI + "/resource/directory";
        public static string GetResourceSubDirectory = WebAPI + "/resource/course/files";
        public static string GetResourceSubDirectoryEx = WebAPI + "/resource/lesson/files";
        public static string GetClassRoomListV2 = WebAPI + "/roomv2/list";
        public static string GetSeatListV2 = WebAPI + "/roomv2/class/room";
        public static string SaveSeatListV2 = WebAPI + "/roomv2/student/seat/batch/save";

        public static string GetDynastyList = WebAPI + "/tld/dynasty/list";
        public static string GetFontList = WebAPI + "/tld/font/list";
        public static string GetAuthorList = WebAPI + "/tld/author/list";
        public static string CalligraphyImageList = WebAPI + "/tld/copybook/list";
        public static string CalligraphyWordList = WebAPI + "/tld/word/query";

        public static string GetVoteList = WebAPI + "/lesson/active/vote/page";
        public static string SaveVote = WebAPI + "/lesson/active/vote/save";
        public static string GetVoteResults = WebAPI + "/lesson/active/vote/count";

        public static string GetQuickAnswerlist = WebAPI + "/lesson/active/quick/list";
        public static string SavetQuickAnswer = WebAPI + "/lesson/active/quick/save";
        public static string GetQuickResults = WebAPI + "/lesson/active/quick/student/list";

        public static string GetSubjectivelist = WebAPI + "/lesson/active/subjective/list";
        public static string GetSubjectiveResults = WebAPI + "/lesson/active/subjective/student/list";
        //保存课堂主观题
        public static string SavetSubjective = WebAPI + "/lesson/active/subjective/save";
        //教师备课获取试卷分页
        public static string GetPaperList = WebAPI + "/lesson/active/paper/page";
        //获取答题结果
        public static string GetPaperResult = WebAPI + "/lesson/active/paper/class/submit/result";

        public static string GetActiveList = WebAPI + "/lesson/active/list";
        public static string SaveTeacherActive = WebAPI + "/lesson/active/teacher/save";
        //保存课堂分享
        public static string SaveShare = WebAPI + "/lesson/active/share/save";
        //获取课堂分享列表
        public static string GetshareList = WebAPI + "/lesson/active/share/list";
        public static string GetDiscussionList = WebAPI + "/lesson/active/discussion/list";
        public static string SaveDiscussion = WebAPI + "/lesson/active/discussion/save";
        //点评
        public static string SaveComment = WebAPI + "/lesson/active/comment/save";

        public static string SaveRecommend = WebAPI + "/lesson/active/recommend/save";
        public static string GetPushServer = WebAPI + "/server/user/push/get";
        public static string GetDocmentServer = WebAPI + "/server/user/view/get";
        public static string GetMqtServer = WebAPI + "/server/user/mq/get";
        public static string GetOssServer = WebAPI + "/server/user/oss/get";
        public static string GetOssToken = WebAPI + "user/oss/token";

        /// <summary>
        /// 快速问答评语
        /// </summary>
        public static string QuickComment = WebAPI + "/lesson/active/quick/student/comment";

        /// <summary>
        /// 快速问答得分
        /// </summary>
        public static string QuickScore = WebAPI + "/lesson/active/quick/student/score";

        /// <summary>
        /// 款速问答得星
        /// </summary>
        public static string QuickStar = WebAPI + "/lesson/active/quick/student/star";

        public static string WebSiteUrl = "shuhua.nnyun.net";
        public static string WebPort = "80";
        public static string ClientVersion = "1.0.3";
        public static string SettingsPath = string.Empty;
        public static string DocumentPath = string.Empty;
        public static string AppPath = string.Empty;
        public static Vector ClassroomLayout { get; set; }

        public static bool IsAllowStudentsOperate { get; set; }
        public static bool IsNoPhotos { get; set; } = true;
        public static int ChangePresetDelay { get { return changePresetDelay; } }
        public static int TableColumn { get; set; } = 6;
        public static float DPI
        {
            get
            {
                if (dpi == 0)
                {
                    System.Drawing.Graphics g = System.Drawing.Graphics.FromHwnd(IntPtr.Zero);
                    dpi = g.DpiX;
                    g.Dispose();
                }
                return dpi;
            }
        }
        public static string Camera1Text { get; set; } = "桌子左下角演示";
        public static string Camera2Text { get; set; } = "演示台操作";
        public static string Camera3Text { get; set; } = "教室后方挂图";
        public static string Camera1Url { get; set; } = string.Empty;
        public static string Camera2Url { get; set; } = string.Empty;
        public static string Camera3Url { get; set; } = string.Empty;
        public static Rect Student1Rect { get; set; } = new Rect(0, 0, 0.5, 1);
        public static Rect Student2Rect { get; set; } = new Rect(0, 0.5, 0.5, 1);
        public static string ScreenShareUrl { get; set; } = string.Empty;
        public static string RtmpServerUrl { get; set; } = string.Empty;
        public static string FileServerUrl { get; set; } = string.Empty;

        /// <summary>
        /// 答题倒计时时长 计量单：分钟
        /// </summary>
        public static int qTime = 1;

        public static string ShareImageText { get; set; } = "字帖";
        public static string AppCaption { get; set; } = "北京龙美讷纳渔科技有限公司";
        public static int ServicePort { get; set; } = 9100;
        public static string CurrentRecordPath { get; set; }
        public static string CurrentCourse { get; set; }
        public static bool LowConfigurationMode { get; set; }
        public static TeacherInfo CurrentUser { get; set; }
        public static LoginInfo LoginInfo { get; set; } = new LoginInfo();
        public static ClassInfo[] ClassList { get; set; }
        public static string[] CourseList { get; set; }
        public static StudentInfo[] StudentList { get; set; }
        public static ClassRoomInfo[] ClassRoomList { get; set; }
        public static ClassRoomV2Info[] ClassRoomV2List { get; set; }
        public static CameraGroupInfo CameraGroup1 { get; set; }

        public static LessonInfo[] LessonList { get; set; }
        public static DynastyInfo[] DynastyList { get; set; }
        public static FontInfo[] FontList { get; set; }
        public static AuthorInfoInfo[] AuthorList { get; set; }
        public static SemesterInfo SemesterInfo { get; set; }
        public static ServerInfo PushServer { get; set; }
        public static ServerInfo DocmentServer { get; set; }
        public static ServerInfo MqServer { get; set; }
        public static ServerInfo OssServer { get; set; }

        /// <summary>
        /// 快速答题列表
        /// </summary>
        public static List<QuickAnswerInfo> QuickAnswerList { get; set; } = new List<QuickAnswerInfo>();
        public static List<VoteInfo> VoteList { get; set; } = new List<VoteInfo>();
        public static List<PaperInfo> PaperList { get; set; } = new List<PaperInfo>();
        public static List<ActiveInfo> ActiveList { get; set; } = new List<ActiveInfo>();
        public static List<StudentWorkDetailsInfo> StudentWorkList { get; set; } = new List<StudentWorkDetailsInfo>();
        public static List<StudentWorkDetailsInfo> TeacherWorkList { get; set; } = new List<StudentWorkDetailsInfo>();
        public static ClassRoomInfo CurrentClassRoom { get; set; }
        public static ClassDetailsV2Info CurrentClassV2 { get; set; }
        public static ClassRoomDetailsV2Info CurrentClassRoomV2 { get; set; }
        public static ClassInfo CurrentClass { get; set; }
        public static LessonInfo CurrentLesson { get; set; }
        public static List<CameraItemInfo> CameraList { get; set; }
        public static List<ResourceItemInfo> resourceItemInfos { get; set; }
        public static SocketServer SocketServer { get; private set; } = new SocketServer();
        public static Window window = null;
        public static void Init()
        {
            DocumentPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            DocumentPath = DocumentPath.TrimEnd('\\') + "\\";
            SettingsPath = DocumentPath + "CalligraphyAssistant\\";
            AppPath = System.Windows.Forms.Application.StartupPath.TrimEnd('\\') + "\\";
            if (!Directory.Exists(SettingsPath))
            {
                Directory.CreateDirectory(SettingsPath);
            }
            string logPath = SettingsPath + "log.txt";
            if (File.Exists(logPath))
            {
                try
                {
                    File.Delete(logPath);
                }
                catch
                {
                }
            }
            string debugPath = SettingsPath + "debug.txt";
            if (File.Exists(debugPath))
            {
                try
                {
                    File.Delete(debugPath);
                }
                catch
                {
                }
            }
            if (ConfigurationManager.AppSettings["Debug"] != null)
            {
                isDebug = ConfigurationManager.AppSettings["Debug"] == "1";
            }
            if (ConfigurationManager.AppSettings["ChangePresetDelay"] != null)
            {
                try
                {
                    changePresetDelay = int.Parse(ConfigurationManager.AppSettings["ChangePresetDelay"]);
                }
                catch
                {
                }
            }
            if (ConfigurationManager.AppSettings["TableColumn"] != null)
            {
                try
                {
                    TableColumn = int.Parse(ConfigurationManager.AppSettings["TableColumn"]);
                }
                catch
                {
                }
            }
            if (ConfigurationManager.AppSettings["Camera1Text"] != null)
            {
                Camera1Text = ConfigurationManager.AppSettings["Camera1Text"];
            }
            if (ConfigurationManager.AppSettings["Camera2Text"] != null)
            {
                Camera2Text = ConfigurationManager.AppSettings["Camera2Text"];
            }
            if (ConfigurationManager.AppSettings["Camera3Text"] != null)
            {
                Camera3Text = ConfigurationManager.AppSettings["Camera3Text"];
            }
            if (ConfigurationManager.AppSettings["Camera1Url"] != null)
            {
                Camera1Url = ConfigurationManager.AppSettings["Camera1Url"];
            }
            if (ConfigurationManager.AppSettings["Camera2Url"] != null)
            {
                Camera2Url = ConfigurationManager.AppSettings["Camera2Url"];
            }
            if (ConfigurationManager.AppSettings["Camera3Url"] != null)
            {
                Camera3Url = ConfigurationManager.AppSettings["Camera3Url"];
            }
            if (ConfigurationManager.AppSettings["Student1Rect"] != null)
            {
                Student1Rect = Rect.Parse(ConfigurationManager.AppSettings["Student1Rect"]);
            }
            if (ConfigurationManager.AppSettings["Student2Rect"] != null)
            {
                Student2Rect = Rect.Parse(ConfigurationManager.AppSettings["Student2Rect"]);
            }
            if (ConfigurationManager.AppSettings["CameraGroup1"] != null)
            {
                string path = ConfigurationManager.AppSettings["CameraGroup1"];
                if (File.Exists(path))
                {
                    CameraGroup1 = Common.XmlDeserializeFromFile<CameraGroupInfo>(path);
                }
            }
            if (ConfigurationManager.AppSettings["RtmpServerUrl"] != null)
            {
                RtmpServerUrl = ConfigurationManager.AppSettings["RtmpServerUrl"];
            }
            if (ConfigurationManager.AppSettings["FileServerUrl"] != null)
            {
                FileServerUrl = ConfigurationManager.AppSettings["FileServerUrl"];
            }
            if (ConfigurationManager.AppSettings["ScreenShareUrl"] != null)
            {
                ScreenShareUrl = ConfigurationManager.AppSettings["ScreenShareUrl"];
            }
            if (ConfigurationManager.AppSettings["ShareImageText"] != null)
            {
                ShareImageText = ConfigurationManager.AppSettings["ShareImageText"];
            }
            if (ConfigurationManager.AppSettings["LowConfigurationMode"] != null)
            {
                LowConfigurationMode = Convert.ToBoolean(ConfigurationManager.AppSettings["LowConfigurationMode"]);
            }
            if (ConfigurationManager.AppSettings["ServicePort"] != null)
            {
                ServicePort = Convert.ToInt32(ConfigurationManager.AppSettings["ServicePort"]);
            }

            Common.LoadLoginSettings();
            UpdateWebAPI();
            Common.ClearFolder(SettingsPath + "Temp\\", DateTime.Now.AddDays(-7), "*.png");
            Common.ClearFolder(SettingsPath + "Temp\\", DateTime.Now.AddDays(-7), "*.jpg");
            Common.ClearFolder(SettingsPath + "Work\\", DateTime.Now.AddDays(-7), "*.png");
            Common.ClearFolder(SettingsPath + "Work\\", DateTime.Now.AddDays(-7), "*.jpg");
        }

        private static void UpdateWebAPI()
        {
            if (Common.LoginInfo != null)
            {
                WebSiteUrl = Common.LoginInfo.Server;
                WebPort = Common.LoginInfo.Port;
            }
            WebAPI = "http://" + WebSiteUrl + ":" + WebPort + "/api";
            UserLogin = WebAPI + "/user/login";
            GetClassRoomList = WebAPI + "/room/page?page=0&size=100&idSchool=";
            GetClassList = WebAPI + "/class/list";
            GetSeatList = WebAPI + "/seat/list?semester=2&idClass=";
            GetSemester = WebAPI + "/year/list";
            GetCurrentClassRoom = WebAPI + "/room/read";
            GetImageThemes = WebAPI + "/type/list";
            GetImageThemeDetails = WebAPI + "/image/list?idType=";
            GetLessonList = WebAPI + "/lesson/list?idClass=";
            CreateLessonList = WebAPI + "/lesson/create";
            GetWorkList = WebAPI + "/stu/work/page";
            GetWorkListV2 = WebAPI + "/stu/work/list";
            SubmitWork = WebAPI + "/stu/work/create";
            SelfSubmitWork = WebAPI + "/stu/work/create/self";
            CorrectWork = WebAPI + "/stu/work/update";
            GetUploadToken = WebAPI + "/qiniu/token";
            GetCourseList = WebAPI + "/user/course";
            GetResourceDirectory = WebAPI + "/resource/directory";
            GetResourceSubDirectory = WebAPI + "/resource/course/files";
            GetResourceSubDirectoryEx = WebAPI + "/resource/lesson/files";
            GetClassRoomListV2 = WebAPI + "/roomv2/list?page=0&size=100&idSchool=";
            //GetSeatListV2 = WebAPI + "/roomv2/class/room"; 
            GetSeatListV2 = WebAPI + "/roomv2/class/room/v2";
            SaveSeatListV2 = WebAPI + "/roomv2/student/seat/batch/save";
            GetDynastyList = WebAPI + "/tld/dynasty/list";
            GetFontList = WebAPI + "/tld/font/list";
            GetAuthorList = WebAPI + "/tld/author/list";
            CalligraphyImageList = WebAPI + "/tld/copybook/list";
            CalligraphyWordList = WebAPI + "/tld/word/query";

            GetQuickAnswerlist = WebAPI + "/lesson/active/quick/list";
            SavetQuickAnswer = WebAPI + "/lesson/active/quick/save";
            GetSubjectivelist = WebAPI + "/lesson/active/subjective/list";
            SavetSubjective = WebAPI + "/lesson/active/subjective/save";
            GetVoteList = WebAPI + "/lesson/active/vote/page";
            SaveVote = WebAPI + "/lesson/active/vote/save";
            GetVoteResults = WebAPI + "/lesson/active/vote/count";
            GetPaperList = WebAPI + "/lesson/active/paper/page";
            GetPaperResult = WebAPI + "/lesson/active/paper/class/submit/result";
            //获取快速答题结果
            GetQuickResults = WebAPI + "/lesson/active/quick/student/page";
            GetSubjectiveResults = WebAPI + "/lesson/active/subjective/student/page";
            GetActiveList = WebAPI + "/lesson/active/list";
            SaveTeacherActive = WebAPI + "/lesson/active/teacher/save";
            SaveShare = WebAPI + "/lesson/active/share/save";
            GetshareList = WebAPI + "/lesson/active/share/list";
            GetDiscussionList = WebAPI + "/lesson/active/discussion/list";
            SaveDiscussion = WebAPI + "/lesson/active/discussion/save";
            SaveComment = WebAPI + "/lesson/active/comment/save";
            SaveRecommend = WebAPI + "/lesson/active/recommend/save";
            GetPushServer = WebAPI + "/server/user/push/get";
            GetDocmentServer = WebAPI + "/server/user/view/get";
            GetMqtServer = WebAPI + "/server/user/mq/get";
            GetOssServer = WebAPI + "/server/user/oss/get";
            GetOssToken = WebAPI + "/user/oss/token";
        }

        public static string UploadImage(string imagePath)
        {
            try
            {
                WebClient webClient = new WebClient();
                string jsonStr = webClient.DownloadString(GetUploadToken);
                JObject obj = JsonConvert.DeserializeObject(jsonStr) as JObject;
                string upToken = obj.Value<string>("uptoken");
                Config config = new Config();
                config.Zone = Qiniu.Storage.Zone.ZONE_CN_North;
                config.UseHttps = true;
                config.UseCdnDomains = true;
                config.ChunkSize = ChunkUnit.U512K;
                UploadManager uploadManager = new UploadManager(config);
                string key = Guid.NewGuid() + Path.GetExtension(imagePath);
                HttpResult result = uploadManager.UploadFile(imagePath, key, upToken, null);
                webClient.Dispose();
                if (result != null)
                {
                    if (result.Code == 200)
                    {
                        obj = JsonConvert.DeserializeObject(result.Text) as JObject;
                        return Common.ImageWebHost + obj.Value<string>("key");
                    }
                    else
                    {
                        Common.Trace("Common UploadImage Error1:" + result.ToString());
                        Console.WriteLine("【------------------------】");
                        Console.WriteLine("【Common.cs Error 001】" + result.ToString());
                        Console.WriteLine("【------------------------】");
                    }
                }
            }
            catch (Exception ex)
            {
                Common.Trace("Common UploadImage Error2:" + ex.Message);
                Console.WriteLine("【------------------------】");
                Console.WriteLine("【Common.cs Error 002】" + ex.ToString());
                Console.WriteLine("【------------------------】");
            }
            return string.Empty;
        }
        public static string UploadImageV2(string imagePath, int id)
        {
            try
            {
                NameValueCollection dict = new NameValueCollection();
                dict.Add("token", Common.CurrentUser.Token);
                string jsonResult = HttpUtility.DownloadString(GetOssToken, Encoding.UTF8, dict);
                ResultInfo<OssItem> result = JsonConvert.DeserializeObject<ResultInfo<OssItem>>(jsonResult);
                if (result != null)
                {
                    if (result.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                    {

                        var oss = result.Body;
                        string keyName = oss.dir + "/" + id + "/" + Path.GetFileName(imagePath);
                        Dictionary<string, string> dictParam = new Dictionary<string, string>();
                        dictParam.Add("policy", oss.policy);
                        dictParam.Add("ossAccesskeyId", oss.accessid);
                        dictParam.Add("signature", oss.signature);
                        dictParam.Add("key", keyName);
                        dictParam.Add("success_action_status", "200");
                        if (HttpUtility.UploadFilesToRemoteUrl(oss.host, imagePath, dictParam))
                        {
                            return string.Format("http://{0}/{1}", oss.domain, keyName);
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                Common.Trace("Common UploadImageV2 Error2:" + ex.Message);
                Console.WriteLine("【------------------------】");
                Console.WriteLine("【Common.cs Error 003】" + ex.ToString());
                Console.WriteLine("【------------------------】");
            }
            return string.Empty;
        }

        public static bool SubmitStudentWork(string imagePath, StudentInfo studentInfo, string token = null)
        {
            string imageUrl = Common.UploadImageV2(imagePath, studentInfo.Id);
            if (string.IsNullOrEmpty(imageUrl))
            {
                return false;
            }
            if (string.IsNullOrEmpty(token))
            {
                token = Common.CurrentUser.Token;
            }
            NameValueCollection dict = new NameValueCollection();
            NameValueCollection data = new NameValueCollection();
            dict.Add("token", token);
            data.Add("semester", Common.SemesterInfo.Semester.ToString());
            data.Add("product", imageUrl);
            data.Add("idStudent", studentInfo.Id.ToString());
            data.Add("exStudent", studentInfo.Name);
            data.Add("idClass", Common.CurrentClass.Id.ToString());
            data.Add("idWork", "0");
            data.Add("exWork", "书画教室");
            data.Add("group", studentInfo.Group.ToString());
            data.Add("idLesson", Common.CurrentLesson.Id.ToString());
            data.Add("course", Common.CurrentCourse);
            string jsonResult = HttpUtility.UploadValues(Common.SubmitWork, data, Encoding.UTF8, Encoding.UTF8, dict);
            //object jsonObj = new
            //{
            //    semester = Common.SemesterInfo.Semester.ToString(),
            //    product = imageUrl,
            //    idStudent = studentInfo.StudentId.ToString(),
            //    exStudent = studentInfo.StudentName,
            //    idClass = Common.CurrentClass.Id.ToString(),
            //    idWork = "0",
            //    exWork = "书画教室",
            //    group = studentInfo.GroupId.ToString(),
            //    idLesson = Common.CurrentLesson.Id.ToString()
            //};
            //string jsonResult = HttpUtility.UploadValuesJson(Common.SubmitWork, jsonObj, Encoding.UTF8, Encoding.UTF8, dict);
            ResultInfo<object> result = JsonConvert.DeserializeObject<ResultInfo<object>>(jsonResult);
            if (result != null && result.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }

        public static bool SubmitVideo(string videoPath, StudentInfo studentInfo)
        {
            string videoUrl = Common.UploadImageV2(videoPath, studentInfo.Id);
            if (string.IsNullOrEmpty(videoUrl))
            {
                return false;
            }
            NameValueCollection dict = new NameValueCollection();
            NameValueCollection data = new NameValueCollection();
            dict.Add("token", Common.CurrentUser.Token);
            data.Add("semester", Common.SemesterInfo.Semester.ToString());
            data.Add("product", videoUrl);
            data.Add("idStudent", studentInfo.Id.ToString());
            data.Add("exStudent", string.Empty);
            data.Add("idClass", Common.CurrentClass.Id.ToString());
            data.Add("idWork", "0");
            data.Add("exWork", Common.Camera1Text);
            data.Add("group", studentInfo.Group.ToString());
            data.Add("idLesson", Common.CurrentLesson.Id.ToString());
            data.Add("Shared", "0");
            data.Add("Type", "1");
            data.Add("course", Common.CurrentCourse);
            string jsonResult = HttpUtility.UploadValues(Common.SubmitWork, data, Encoding.UTF8, Encoding.UTF8, dict);
            ResultInfo<object> result = JsonConvert.DeserializeObject<ResultInfo<object>>(jsonResult);
            if (result != null && result.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            else
            {
                Common.Trace("Common SubmitVideo Error2:" + jsonResult);
            }
            return false;
        }
        public static bool TeacherSubmitWork(string imagePath)
        {
            string imageUrl = Common.UploadImageV2(imagePath, Common.CurrentLesson.Id);
            if (string.IsNullOrEmpty(imageUrl))
            {
                return false;
            }
            NameValueCollection dict = new NameValueCollection();
            NameValueCollection data = new NameValueCollection();
            dict.Add("token", Common.CurrentUser.Token);
            data.Add("semester", Common.SemesterInfo.Semester.ToString());
            data.Add("product", imageUrl);
            data.Add("idClass", Common.CurrentClass.Id.ToString());
            data.Add("shared", "0");
            data.Add("type", "1");
            data.Add("idLesson", Common.CurrentLesson.Id.ToString());
            data.Add("style", "2");
            string jsonResult = HttpUtility.UploadValues(Common.SelfSubmitWork, data, Encoding.UTF8, Encoding.UTF8, dict);

            ResultInfo<object> result = JsonConvert.DeserializeObject<ResultInfo<object>>(jsonResult);
            if (result != null && result.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }
        public static bool MergeImage(string sourcePath, string targetPath, int targetWidth, int targetHeight, int clipCount, string backImagePath = null)
        {
            if (File.Exists(targetPath))
            {
                return true;
            }
            GDIP.Bitmap sourceImage = GDIP.Image.FromFile(sourcePath) as GDIP.Bitmap;
            GDIP.Bitmap targetImage = new GDIP.Bitmap(targetWidth, targetHeight);
            GDIP.Graphics g = GDIP.Graphics.FromImage(targetImage);
            if (!string.IsNullOrEmpty(backImagePath) && File.Exists(backImagePath))
            {
                GDIP.Bitmap backImage = GDIP.Image.FromFile(backImagePath) as GDIP.Bitmap;
                g.DrawImage(backImage, new GDIP.RectangleF(0, 0, targetWidth, targetHeight), new GDIP.RectangleF(0, 0, backImage.Width, backImage.Height), GDIP.GraphicsUnit.Pixel);
                backImage.Dispose();
            }
            else
            {
                g.Clear(GDIP.Color.Black);
            }
            float newWidth;
            float newHeight;
            float x;
            float y;
            float sourceRatio;
            float targetRatio = targetWidth / (float)targetHeight;
            if (clipCount == 1)
            {
                sourceRatio = sourceImage.Width / (float)sourceImage.Height;
                if (sourceRatio < targetRatio)
                {
                    newWidth = sourceImage.Width / (float)sourceImage.Height * targetHeight;
                    newHeight = targetHeight;
                    x = (targetWidth - newWidth) / 2;
                    y = 0;
                }
                else
                {
                    newWidth = targetWidth;
                    newHeight = sourceImage.Height / (float)sourceImage.Width * targetWidth;
                    x = 0;
                    y = (targetHeight - newHeight) / 2;
                }
                g.DrawImage(sourceImage, new GDIP.RectangleF(x, y, newWidth, newHeight));
            }
            else if (clipCount == 4)
            {
                sourceRatio = sourceImage.Width * 4 / (float)sourceImage.Height;
                if (sourceRatio < targetRatio)
                {

                }
                else
                {
                    newWidth = targetWidth / 4;
                    newHeight = sourceImage.Height / (float)sourceImage.Width * targetWidth / 4;
                    x = (targetWidth - newWidth * 4) / 2;
                    y = (targetHeight - newHeight) / 2;
                    for (int i = 0; i < 4; i++)
                    {
                        g.DrawImage(sourceImage, new GDIP.RectangleF(x, y, newWidth, newHeight));
                        x += newWidth;
                    }
                }
            }
            else if (clipCount == 8)
            {
                sourceRatio = sourceImage.Width * 4 / (float)sourceImage.Height * 2;
                if (sourceRatio < targetRatio)
                {

                }
                else
                {
                    newWidth = targetWidth / 4;
                    newHeight = sourceImage.Height / (float)sourceImage.Width * targetWidth / 4;
                    x = (targetWidth - newWidth * 4) / 2;
                    y = (targetHeight - newHeight * 2) / 2;
                    for (int j = 0; j < 2; j++)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            g.DrawImage(sourceImage, new GDIP.RectangleF(x, y, newWidth, newHeight));
                            x += newWidth;
                        }
                        y += newHeight;
                        x = (targetWidth - newWidth * 4) / 2;
                    }
                }
            }
            else
            {
                return false;
            }
            g.Dispose();
            targetImage.Save(targetPath, GDIP.Imaging.ImageFormat.Jpeg);
            targetImage.Dispose();
            sourceImage.Dispose();
            return true;
        }

        public static ResultClassRoomV2Info GetClassRoomV2Info(string jsonStr)
        {
            ResultInfo<ResultClassRoomV2ExInfo> resultInfo = JsonConvert.DeserializeObject<ResultInfo<ResultClassRoomV2ExInfo>>(jsonStr);
            ResultClassRoomV2ExInfo body = resultInfo.Body;
            ResultClassRoomV2Info resultClassRoomV2Info = new ResultClassRoomV2Info();
            ClassDetailsV2Info classDetailsV2Info = new ClassDetailsV2Info()
            {
                ClassId = body.GradeData.Id,
                ClassName = body.GradeData.Name,
                TeacherId = body.GradeData.TeacherId,
                TeacherName = body.GradeData.Name,
            };
            ClassRoomDetailsV2Info classRoomDetailsV2Info = new ClassRoomDetailsV2Info()
            {
                Cols = body.RoomData.Cols,
                Name = body.RoomData.Name,
                RoomId = body.RoomData.Id,
                Rows = body.RoomData.Rows,
                SchoolId = 0,
            };
            List<SeatV2Info> seatV2InfoList = new List<SeatV2Info>();
            List<StudentV2Info> studentV2InfoList = new List<StudentV2Info>();
            List<CameraV2Info> cameraV2InfoList = new List<CameraV2Info>();
            List<GroupV2Info> groupV2InfoList = new List<GroupV2Info>();
            foreach (var item in body.RoomSeatsData)
            {
                SeatV2Info seatV2Info = new SeatV2Info()
                {
                    CameraId = item.CameraId,
                    Col = item.Col,
                    Describe = string.Empty,
                    IP = item.Ip,
                    Row = item.Row,
                    SeatId = item.Id,
                };

                StudentV2Info studentV2Info = new StudentV2Info()
                {
                    Col = item.Col,
                    Describe = string.Empty,
                    IP = item.Ip,
                    Number = item.Id,
                    Row = item.Row,
                    SeatId = item.Id,
                };
                ClassRoomGroupsV2ExInfo groupsV2ExInfo = body.RoomGroupsData.FirstOrDefault(p => p.Seats.Any(q => q.Col == item.Col && q.Row == item.Row));
                if (groupsV2ExInfo != null)
                {
                    ClassRoomGroupsSeatV2ExInfo groupsSeatV2ExInfo = groupsV2ExInfo.Seats.FirstOrDefault(p => p.SeatId == item.Id);
                    if (groupsSeatV2ExInfo != null && groupsSeatV2ExInfo.Students != null && groupsSeatV2ExInfo.Students.Count > 0)
                    {
                        ClassRoomGroupsSeatStudentV2ExInfo groupsSeatStudentV2ExInfo = groupsSeatV2ExInfo.Students.FirstOrDefault();
                        if (groupsSeatStudentV2ExInfo != null)
                        {
                            studentV2Info.CameraId = seatV2Info.CameraId;
                            studentV2Info.Rotation = seatV2Info.Rotation = groupsSeatV2ExInfo.Rotation;
                            studentV2Info.Preset = seatV2Info.Preset = groupsSeatV2ExInfo.Preset;
                            studentV2Info.GroupId = seatV2Info.GroupId = groupsV2ExInfo.GroupId;
                            studentV2Info.GroupName = seatV2Info.GroupName = groupsV2ExInfo.GroupName;
                            studentV2Info.StudentId = seatV2Info.StudentId = groupsSeatStudentV2ExInfo.Id;
                            studentV2Info.StudentName = seatV2Info.StudentName = groupsSeatStudentV2ExInfo.Name;

                        }
                    }
                }
                GradeStudentV2ExInfo exInfo = body.GradeStudentsData.FirstOrDefault(p => p.Id == studentV2Info.StudentId);
                if (exInfo != null)
                {
                    studentV2Info.SN = exInfo.SN;
                }
                seatV2InfoList.Add(seatV2Info);
                studentV2InfoList.Add(studentV2Info);
            }

            foreach (var item in body.RoomGroupsData)
            {
                List<CameraV2Info> tempList = new List<CameraV2Info>();
                foreach (var camera in item.Seats)
                {
                    CameraV2Info cameraV2Info = new CameraV2Info()
                    {
                        Account = camera.Account,
                        Id = camera.CameraId,
                        IP = camera.Ip,
                        Number = camera.Index,
                        Password = camera.Password,
                    };
                    tempList.Add(cameraV2Info);
                }
                GroupV2Info groupV2Info = new GroupV2Info()
                {
                    GroupId = item.GroupId,
                    GroupName = item.GroupName,
                    CameraList = tempList.ToArray()
                };
                groupV2InfoList.Add(groupV2Info);
            }
            classDetailsV2Info.StudentList = studentV2InfoList.ToArray();
            classRoomDetailsV2Info.SeatList = seatV2InfoList.ToArray();
            classRoomDetailsV2Info.GroupList = groupV2InfoList.ToArray();
            classRoomDetailsV2Info.CameraList = groupV2InfoList.SelectMany(p => p.CameraList).ToArray();
            resultClassRoomV2Info.ClassData = classDetailsV2Info;
            resultClassRoomV2Info.ClassRoomData = classRoomDetailsV2Info;
            return resultClassRoomV2Info;
        }

        /// <summary>  
        /// 获取宽度缩放百分比  
        /// </summary>  
        public static float ScaleX
        {
            get
            {
                IntPtr hdc = WinAPI.GetDC(IntPtr.Zero);
                float ScaleX = (float)WinAPI.GetDeviceCaps(hdc, WinAPI.DESKTOPHORZRES) / (float)WinAPI.GetDeviceCaps(hdc, WinAPI.HORZRES);
                WinAPI.ReleaseDC(IntPtr.Zero, hdc);
                return ScaleX;
            }
        }

        /// <summary>  
        /// 获取高度缩放百分比  
        /// </summary>  
        public static float ScaleY
        {
            get
            {
                IntPtr hdc = WinAPI.GetDC(IntPtr.Zero);
                float ScaleY = (float)(float)WinAPI.GetDeviceCaps(hdc, WinAPI.DESKTOPVERTRES) / (float)WinAPI.GetDeviceCaps(hdc, WinAPI.VERTRES);
                WinAPI.ReleaseDC(IntPtr.Zero, hdc);
                return ScaleY;
            }
        }

        /// <summary>  
        /// 获取真实设置的桌面分辨率大小  
        /// </summary>  
        public static System.Drawing.Size RealScreenSize
        {
            get
            {
                IntPtr hdc = WinAPI.GetDC(IntPtr.Zero);
                System.Drawing.Size size = new System.Drawing.Size();
                size.Width = WinAPI.GetDeviceCaps(hdc, WinAPI.DESKTOPHORZRES);
                size.Height = WinAPI.GetDeviceCaps(hdc, WinAPI.DESKTOPVERTRES);
                WinAPI.ReleaseDC(IntPtr.Zero, hdc);
                return size;
            }
        }

        public static bool SaveImage(ImageSource image, string savePath)
        {
            try
            {
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create((BitmapSource)image));
                using (FileStream stream = new FileStream(savePath, FileMode.Create))
                {
                    encoder.Save(stream);
                }
            }
            catch (Exception ex)
            {
                Common.Trace("Common SaveImage Error:" + ex.Message); 
                Console.WriteLine("【------------------------】");
                Console.WriteLine("【Common.cs Error 004】" + ex.ToString());
                Console.WriteLine("【------------------------】");
                return false;
            }

            return true;
        }

        public static void ShowTip(string tip)
        {
            if (ShowTipCallback != null)
            {
                ShowTipCallback(tip);
            }
        }

        public static void Debug(string str)
        {
            if (isDebug)
            {
                string logPath = Path.Combine(SettingsPath, "debug.txt");
                try
                {
                    StreamWriter sw = new StreamWriter(logPath, true);
                    sw.WriteLine("--" + DateTime.Now.ToString("HH:mm:ss") + ":" + str);
                    sw.Close();
                }
                catch     (Exception ex)
                {
                    Console.WriteLine("【------------------------】");
                    Console.WriteLine("【Common.cs Error 005】" + ex.ToString());
                    Console.WriteLine("【------------------------】");
                }
            }
        }

        public static void Trace(string str)
        {
            string logPath = Path.Combine(SettingsPath, "log.txt");
            try
            {
                StreamWriter sw = new StreamWriter(logPath, true);
                sw.WriteLine("--" + DateTime.Now.ToString("HH:mm:ss") + ":" + str);
                sw.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("【------------------------】");
                Console.WriteLine("【Common.cs Error 006】" + ex.ToString());
                Console.WriteLine("【------------------------】");
            }
        }

        public static string GetSystemInstallDate()
        {
            try
            {
                RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\");
                if (registryKey != null)
                {
                    object result = registryKey.GetValue("InstallDate", string.Empty);
                    if (result != null)
                    {
                        return result.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Common.Trace("Common GetSystemInstallDate Error:" + ex.Message);
                Console.WriteLine("【------------------------】");
                Console.WriteLine("【Common.cs Error 007】" + ex.ToString());
                Console.WriteLine("【------------------------】");
            }
            return string.Empty;
        }

        public static void ClearFolder(string folderPath, DateTime belowDate, string type)
        {
            try
            {
                if (Directory.Exists(folderPath))
                {
                    string[] strArr = Directory.GetFiles(folderPath, type);
                    foreach (string item in strArr)
                    {
                        System.IO.FileInfo fileInfo = new System.IO.FileInfo(item);
                        if (fileInfo.LastWriteTime < belowDate)
                        {
                            System.IO.File.Delete(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.Trace("Common ClearFolder Error:" + ex.ToString());
                Console.WriteLine("【------------------------】");
                Console.WriteLine("【Common.cs Error 008】" + ex.ToString());
                Console.WriteLine("【------------------------】");
            }
        }

        public static string Encrypt(string stringToEncrypt, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.UTF8.GetBytes(stringToEncrypt);
            des.Key = Encoding.UTF8.GetBytes(sKey);
            des.IV = Encoding.UTF8.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            ret.ToString();
            return ret.ToString();
        }

        public static string Decrypt(string stringToDecrypt, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = new byte[stringToDecrypt.Length / 2];
            for (int x = 0; x < stringToDecrypt.Length / 2; x++)
            {
                int i = (Convert.ToInt32(stringToDecrypt.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }
            des.Key = Encoding.UTF8.GetBytes(sKey);
            des.IV = Encoding.UTF8.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Encoding.UTF8.GetString(ms.ToArray());
        }

        public static string ComputeMD5(string input)
        {
            try
            {
                if (string.IsNullOrEmpty(input))
                {
                    return null;
                }
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                byte[] byteArr = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
                md5.Dispose();
                string resultStr = "";
                for (int i = 0; i < byteArr.Length; i++)
                {
                    resultStr += byteArr[i].ToString("X2");
                }
                return resultStr;
            }
            catch
            {
            }
            return null;
        }

        public static void SaveLoginSettings(LoginInfo setting)
        {
            Common.WebSiteUrl = setting.Server;
            Common.WebPort = setting.Port;
            UpdateWebAPI();
            string path = SettingsPath + "Login.xml";
            Common.XmlSerializeToFile(setting, path);
        }

        public static void LoadLoginSettings()
        {
            string path = SettingsPath + "Login.xml";
            if (File.Exists(path))
            {
                LoginInfo setting = Common.XmlDeserializeFromFile<LoginInfo>(path);
                if (setting != null)
                {
                    Common.LoginInfo = setting;
                    UpdateWebAPI();
                }
            }
        }

        public static System.Windows.Media.Imaging.BitmapSource Screenshot()
        {
            float scale = DPI / 96f;
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap((int)(SystemParameters.PrimaryScreenWidth * scale), (int)(SystemParameters.PrimaryScreenHeight * scale), System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);
            g.CopyFromScreen(0, 0, 0, 0, bitmap.Size);
            g.Dispose();
            System.Windows.Media.Imaging.BitmapSource image = BitmapToBitmapSource(bitmap, System.Windows.Media.PixelFormats.Pbgra32);
            bitmap.Dispose();
            return image;
        }

        public static System.Windows.Media.Imaging.BitmapSource BitmapToBitmapSource(System.Drawing.Bitmap bitmap, System.Windows.Media.PixelFormat pixelFormat)
        {
            System.Drawing.Imaging.BitmapData data = bitmap.LockBits(
              new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
               System.Drawing.Imaging.ImageLockMode.WriteOnly,
               bitmap.PixelFormat);
            System.Windows.Media.Imaging.BitmapSource source = System.Windows.Media.Imaging.BitmapSource.Create(bitmap.Width, bitmap.Height, 96, 96, pixelFormat, null, data.Scan0, data.Stride * bitmap.Height, data.Stride);
            bitmap.UnlockBits(data);
            return source;
        }

        /// <summary>
        /// 返回三个点之间的夹角
        /// </summary>
        /// <param name="center"></param>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static double GetAngle(Point center, Point first, Point second)
        {
            double dx1, dx2, dy1, dy2;
            double angle;

            dx1 = first.X - center.X;
            dy1 = first.Y - center.Y;

            dx2 = second.X - center.X;

            dy2 = second.Y - center.Y;

            double c = Math.Sqrt(dx1 * dx1 + dy1 * dy1) * Math.Sqrt(dx2 * dx2 + dy2 * dy2);

            if (c == 0) return -1;

            angle = (Math.Acos((dx1 * dx2 + dy1 * dy2) / c) / Math.PI * 180);
            return angle;
        }

        public static Point[] GetPointsFromLine(Point start, Point end)
        {
            Point a = start.X < end.X ? start : end;
            Point b = start.X < end.X ? end : start;
            List<Point> list = new List<Point>();
            list.Add(end);
            for (int i = (int)a.X + 1; i < (int)b.X; i++)
            {
                // 计算斜率
                double k = ((double)(a.Y - b.Y)) / (a.X - b.X);
                // 根据斜率，计算y坐标
                double y = k * (i - a.X) + a.Y;
                list.Add(new Point(i, y));
            }
            return list.ToArray();
        }

        public static string GetFileSizeStr(long fileSize)
        {
            if (fileSize < Consts.KB)
            {
                return fileSize.ToString("N") + " B";
            }
            else if (fileSize >= Consts.KB && fileSize < Consts.MB)
            {
                return (fileSize / Consts.KB).ToString("N2") + " KB";
            }
            else if (fileSize >= Consts.MB && fileSize < Consts.GB)
            {
                return (fileSize / Consts.MB).ToString("N2") + " MB";
            }
            else if (fileSize >= Consts.GB)
            {
                return (fileSize / Consts.GB).ToString("N2") + " GB";
            }
            else
            {
                return "Unknown";
            }
        }

        public static void EndClassOnClosing()
        {
            if (Common.CurrentClass != null)
            {
                Common.SocketServer.ShowImageMode();
                Common.SocketServer.StopLiveTransfer();
                Common.SocketServer.StopShareScreen();
                Common.SocketServer.LogoutAll();
                Common.SocketServer.Stop();
                Thread.Sleep(2000);
            }
        }

        public static void DownloadImage(string imageUrl, string savePath)
        {
            WebClient webClient = new WebClient();
            try
            {
                string tempPath = savePath + ".temp";
                if (File.Exists(savePath))
                {
                    return;
                }
                if (File.Exists(tempPath))
                {
                    File.Delete(tempPath);
                }
                if (imageUrl.StartsWith("//"))
                {
                    imageUrl = "http://" + imageUrl.TrimStart(new char[] { '/' });
                }
                else
                {
                    imageUrl = imageUrl.Replace("\\", string.Empty);
                }
                webClient.DownloadFileAsync(new Uri(imageUrl), tempPath);
                webClient.DownloadFileCompleted += (x, y) =>
                {
                    File.Move(tempPath, savePath);
                    webClient.Dispose();
                };
            }
            catch (Exception ex)
            {
                Common.Trace("Common DownloadImage Error:" + ex.Message);
                Console.WriteLine("【------------------------】");
                Console.WriteLine("【Common.cs Error 009】" + ex.ToString());
                Console.WriteLine("【------------------------】");
                webClient.Dispose();
            }
        }

        public static void FlipX(ref byte[] byteArr, int width, int height)
        {
            byte[] tempArr = new byte[3];
            int start = 0;
            for (int i = 0; i < height; i++)
            {
                start = width * i * 3;
                for (int j = 0; j < width / 2; j++)
                {
                    tempArr[0] = byteArr[start + j * 3];
                    tempArr[1] = byteArr[start + j * 3 + 1];
                    tempArr[2] = byteArr[start + j * 3 + 2];
                    byteArr[start + j * 3] = byteArr[start + (width - j - 1) * 3];
                    byteArr[start + j * 3 + 1] = byteArr[start + (width - j - 1) * 3 + 1];
                    byteArr[start + j * 3 + 2] = byteArr[start + (width - j - 1) * 3 + 2];
                    byteArr[start + (width - j - 1) * 3] = tempArr[0];
                    byteArr[start + (width - j - 1) * 3 + 1] = tempArr[1];
                    byteArr[start + (width - j - 1) * 3 + 2] = tempArr[2];
                }
            }
        }

        public static void FlipY(ref byte[] byteArr, int width, int height)
        {
            byte[] tempArr = new byte[width * 3];
            int top = 0;
            int bottom = 0;
            for (int i = 0; i < height / 2; i++)
            {
                top = width * i * 3;
                bottom = width * (height - i - 1) * 3;
                Array.Copy(byteArr, top, tempArr, 0, tempArr.Length);
                Array.Copy(byteArr, bottom, byteArr, top, tempArr.Length);
                Array.Copy(tempArr, 0, byteArr, bottom, tempArr.Length);
            }
        }

        public static int GetDeviceIndex(string deviceName, bool isAudio)
        {
            DsDevice[] devices = null;
            if (isAudio)
            {
                devices = DsDevice.GetDevicesOfCat(FilterCategory.AudioInputDevice);
            }
            else
            {
                devices = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
            }
            if (devices.Length > 0)
            {
                int index = -1;
                foreach (DsDevice item in devices)
                {
                    index++;
                    if (item.Name == deviceName)
                    {
                        return index;
                    }
                }
            }
            return -1;
        }


        public static void XmlSerializeToFile(object obj, string path)
        {
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                serializer.Serialize(stream, obj);
            }
        }

        public static T XmlDeserializeFromFile<T>(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            if (!File.Exists(path))
            {
                return default(T);
            }
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                try
                {
                    return (T)serializer.Deserialize(stream);
                }
                catch (Exception ex)
                {
                    Common.Trace("Common XmlDeserializeFromFile Error:" + ex.Message);
                    Console.WriteLine("【------------------------】");
                    Console.WriteLine("【Common.cs Error 010】" + ex.ToString());
                    Console.WriteLine("【------------------------】");
                    return default;
                }
            }
        }

        public static bool CheckArrayEquals<T>(T[] array1, T[] array2, int length)
        {
            if (array1 == null || array2 == null)
            {
                return false;
            }
            if (array1.Length < length || array2.Length < length)
            {
                return false;
            }
            for (int i = 0; i < length; i++)
            {
                if (!array1[i].Equals(array2[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public static List<T> GetChildObjects<T>(DependencyObject obj, Type typename) where T : FrameworkElement
        {
            DependencyObject child = null;
            List<T> childList = new List<T>();
            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);

                if (child is T && (((T)child).GetType() == typename))
                {
                    childList.Add((T)child);
                }
                childList.AddRange(GetChildObjects<T>(child, typename));
            }
            return childList;
        }
        public static void GetQuickAnswer()
        {
            try
            {
                NameValueCollection dict = new NameValueCollection();
                dict.Add("token", Common.CurrentUser.Token);
                var data = new
                {

                };
                string jsonResult = HttpUtility.UploadValuesJson(Common.GetQuickAnswerlist, data, Encoding.UTF8, Encoding.UTF8, dict);
                ResultInfo<List<QuickAnswerInfo>> result3 = JsonConvert.DeserializeObject<ResultInfo<List<QuickAnswerInfo>>>(jsonResult);
                if (result3 != null)
                {
                    if (result3.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                    {
                        List<QuickAnswerInfo> info = result3.Body;
                        info.ForEach(item =>
                        {
                            if (Common.QuickAnswerList.FirstOrDefault(p => p.id == item.id) == null && item.id_lesson == Common.CurrentLesson.Id && item.id_class == Common.CurrentClassV2.ClassId)
                            {
                                item.type = "quick";
                                Common.QuickAnswerList.Add(item);
                                Common.QuickAnswerList = Common.QuickAnswerList.OrderByDescending(v => v.id).ToList();
                            }
                        });
                    }
                }
                jsonResult = HttpUtility.UploadValuesJson(Common.GetSubjectivelist, data, Encoding.UTF8, Encoding.UTF8, dict);
                ResultInfo<List<QuickAnswerInfo>> result4 = JsonConvert.DeserializeObject<ResultInfo<List<QuickAnswerInfo>>>(jsonResult);
                if (result4 != null)
                {
                    if (result4.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                    {
                        List<QuickAnswerInfo> info = result4.Body;
                        info.ForEach(item =>
                        {
                            if (Common.QuickAnswerList.FirstOrDefault(p => p.id == item.id) == null && item.id_lesson == Common.CurrentLesson.Id && item.id_class == Common.CurrentClassV2.ClassId)
                            {
                                item.type = "subjective";
                                Common.QuickAnswerList.Add(item);
                                Common.QuickAnswerList = Common.QuickAnswerList.OrderByDescending(v => v.id).ToList();
                            }
                        });
                    }
                }
                //QuickAnswerList= Common.QuickAnswerList.OrderBy(p => p.id).ToList();
            }
            catch (Exception ex)
            {
                Common.Trace("GetQuickAnswer Error:" + ex.Message);
                Console.WriteLine("【------------------------】");
                Console.WriteLine("【Common.cs Error 011】" + ex.ToString());
                Console.WriteLine("【------------------------】");
            }
        }
        /// <summary>
        /// 获取投票
        /// </summary>
        public static void GetVoteInfoList()
        {
            try
            {
                NameValueCollection dict = new NameValueCollection();
                dict.Add("token", Common.CurrentUser.Token);
                var data = new
                {
                    id_class = Common.CurrentClassV2.ClassId,
                    id_lesson = Common.CurrentLesson.Id,
                };
                string jsonResult = HttpUtility.UploadValuesJson(Common.GetVoteList, data, Encoding.UTF8, Encoding.UTF8, dict);
                ResultInfo<ResultCalligraphyListInfo<List<VoteInfo>>> result = JsonConvert.DeserializeObject<ResultInfo<ResultCalligraphyListInfo<List<VoteInfo>>>>(jsonResult);
                if (result != null)
                {
                    if (result.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                    {
                        ResultCalligraphyListInfo<List<VoteInfo>> resultCalligraphyListInfo = result.Body;
                        List<VoteInfo> info = resultCalligraphyListInfo.Data;
                        info.ForEach(item =>
                        {
                            if (Common.VoteList.FirstOrDefault(p => p.id == item.id) == null)
                            {
                                Common.VoteList.Add(item);
                                Common.VoteList = Common.VoteList.OrderByDescending(v => v.id).ToList();
                            }
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Common.Trace("GetVoteInfoList Error:" + ex.Message);
                Console.WriteLine("【------------------------】");
                Console.WriteLine("【Common.cs Error 012】" + ex.ToString());
                Console.WriteLine("【------------------------】");
            }

        }
        public static void GetPaperInfoList()
        {
            try
            {
                NameValueCollection dict = new NameValueCollection();
                dict.Add("token", Common.CurrentUser.Token);
                var data = new
                {
                    not_self = 1,
                    //id_resource = Common.CurrentLesson.Id
                };
                string jsonResult = HttpUtility.UploadValuesJson(Common.GetPaperList, data, Encoding.UTF8, Encoding.UTF8, dict);
                ResultInfo<ResultCalligraphyListInfo<List<PaperInfo>>> result = JsonConvert.DeserializeObject<ResultInfo<ResultCalligraphyListInfo<List<PaperInfo>>>>(jsonResult);
                if (result != null)
                {
                    if (result.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                    {
                        ResultCalligraphyListInfo<List<PaperInfo>> resultCalligraphyListInfo = result.Body;
                        Common.PaperList = resultCalligraphyListInfo.Data;
                        Common.PaperList.ForEach(p => p.paperResult = GetPaperResultInfo(p.id));
                    }
                }
            }
            catch (Exception ex)
            {
                Common.Trace("GetQuickAnswer Error:" + ex.Message);
                Console.WriteLine("【------------------------】");
                Console.WriteLine("【Common.cs Error 013】" + ex.ToString());
                Console.WriteLine("【------------------------】");
            }

        }
        public static PaperResult GetPaperResultInfo(int id)
        {
            PaperResult paperResult = null;
            try
            {

                NameValueCollection dict = new NameValueCollection();
                dict.Add("token", Common.CurrentUser.Token);
                string jsonResult = HttpUtility.DownloadString(Common.GetPaperResult + $"?paper={id}", Encoding.UTF8, dict);
                ResultInfo<PaperResult> result = JsonConvert.DeserializeObject<ResultInfo<PaperResult>>(jsonResult);
                if (result.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    paperResult = result.Body;

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("【------------------------】");
                Console.WriteLine("【Common.cs Error 014】" + ex.ToString());
                Console.WriteLine("【------------------------】");
                Common.Trace("GetPaperResultInfo Error:" + ex.Message);
            }

            return paperResult;
        }
        public static void GetActiveInfoList()
        {
            try
            {
                NameValueCollection dict = new NameValueCollection();
                dict.Add("token", Common.CurrentUser.Token);
                var data = new
                {
                    id_class = Common.CurrentClassV2.ClassId,
                    id_lesson = Common.CurrentLesson.Id,
                };
                string jsonResult = HttpUtility.UploadValuesJson(Common.GetActiveList, data, Encoding.UTF8, Encoding.UTF8, dict);
                ResultInfo<List<ActiveInfo>> result = JsonConvert.DeserializeObject<ResultInfo<List<ActiveInfo>>>(jsonResult);
                if (result != null)
                {
                    if (result.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                    {
                        Common.ActiveList = result.Body;

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("【------------------------】");
                Console.WriteLine("【Common.cs Error 015】" + ex.ToString());
                Console.WriteLine("【------------------------】");
                Common.Trace("GetActiveInfoList Error:" + ex.Message);
            }

        }
        /// <summary>
        /// 获取当前课程学生作品
        /// </summary>
        public static void GetStudentWorkInfoList()
        {
            try
            {
                StudentWorkList.Clear();
                NameValueCollection dict = new NameValueCollection();
                dict.Add("token", Common.CurrentUser.Token);
                var data = new
                {
                    id_class = Common.CurrentClassV2.ClassId,
                    id_lesson = Common.CurrentLesson.Id,
                    //id_class = 1,
                    //id_lesson = 4,
                    sort = 2,
                };
                string worksPath = Common.SettingsPath + "Work\\";
                if (!Directory.Exists(worksPath))
                {
                    Directory.CreateDirectory(worksPath);
                }
                string jsonResult = HttpUtility.UploadValuesJson(Common.GetWorkListV2, data, Encoding.UTF8, Encoding.UTF8, dict);
                //Console.WriteLine("学生作品：" + jsonResult);
                ResultInfo<List<StudentWorkDetailsInfo>> result = JsonConvert.DeserializeObject<ResultInfo<List<StudentWorkDetailsInfo>>>(jsonResult);
                if (result != null)
                {
                    if (result.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                    {
                        foreach (StudentWorkDetailsInfo item in result.Body)
                        {
                            item.ClassName = Common.CurrentClass.Name;
                            GroupV2Info groupInfo = Common.CurrentClassRoomV2.GroupList.FirstOrDefault(p => p.GroupId.ToString() == item.Group);
                            if (groupInfo != null)
                            {
                                item.GroupName = groupInfo.GroupName;
                            }
                            StudentInfo studentInfo = Common.StudentList.FirstOrDefault(p => p.Id == item.StudentId);
                            if (studentInfo != null)
                            {
                                item.Number = studentInfo.Number;
                            }
                            string savePath;
                            if (!string.IsNullOrEmpty(item.Correct))
                            {
                                savePath = worksPath + System.IO.Path.GetFileName(item.Correct);
                            }
                            else
                            {
                                savePath = worksPath + System.IO.Path.GetFileName(item.Work);
                            }
                            item.LocalPath = savePath;
                            //Console.WriteLine("-->" + item.ToJson());
                            StudentWorkList.Add(item);

                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("【------------------------】");
                Console.WriteLine("【Common.cs Error 016】" + ex.ToString());
                Console.WriteLine("【------------------------】");
                Common.Trace("GetStudentWorkInfoList Error:" + ex.Message);
            }

        }
        /// <summary>
        ///  获取当前课程教师作品
        /// </summary>
        public static void GetTeacherWorkInfoList()
        {
            try
            {
                TeacherWorkList.Clear();
                NameValueCollection dict = new NameValueCollection();
                dict.Add("token", Common.CurrentUser.Token);
                var data = new
                {
                    id_class = Common.CurrentClassV2.ClassId,
                    id_lesson = Common.CurrentLesson.Id,
                    //id_class = 1,
                    //id_lesson = 4,
                    sort = 1
                };
                string worksPath = Common.SettingsPath + "Work\\";
                if (!Directory.Exists(worksPath))
                {
                    Directory.CreateDirectory(worksPath);
                }
                string jsonResult = HttpUtility.UploadValuesJson(Common.GetWorkListV2, data, Encoding.UTF8, Encoding.UTF8, dict);
                ResultInfo<List<StudentWorkDetailsInfo>> result = JsonConvert.DeserializeObject<ResultInfo<List<StudentWorkDetailsInfo>>>(jsonResult);
                if (result != null)
                {
                    if (result.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                    {
                        foreach (StudentWorkDetailsInfo item in result.Body)
                        {
                            item.ClassName = Common.CurrentClass.Name;
                            GroupV2Info groupInfo = Common.CurrentClassRoomV2.GroupList.FirstOrDefault(p => p.GroupId.ToString() == item.Group);
                            if (groupInfo != null)
                            {
                                item.GroupName = groupInfo.GroupName;
                            }
                            StudentInfo studentInfo = Common.StudentList.FirstOrDefault(p => p.Id == item.StudentId);
                            if (studentInfo != null)
                            {
                                item.Number = studentInfo.Number;
                            }
                            string savePath;
                            if (!string.IsNullOrEmpty(item.Correct))
                            {
                                savePath = worksPath + item.Id + System.IO.Path.GetFileName(item.Correct);
                            }
                            else
                            {
                                savePath = worksPath + item.Id + System.IO.Path.GetFileName(item.Work);
                            }
                            item.LocalPath = savePath;
                            TeacherWorkList.Add(item);

                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("【------------------------】");
                Console.WriteLine("【Common.cs Error 017】" + ex.ToString());
                Console.WriteLine("【------------------------】");
                Common.Trace("GetTeacherWorkInfoList Error:" + ex.Message);
            }

        }
        public static bool SaveWorkShare(List<StudentInfo> studentInfos, int workId, string url)
        {
            try
            {
                NameValueCollection dict = new NameValueCollection();
                dict.Add("token", Common.CurrentUser.Token);
                List<object> members = new List<object>();
                studentInfos.ForEach(p => {
                    members.Add(new { member_type = 1, id_member = p.Id, });
                });

                var data = new
                {
                    id_class = Common.CurrentClassV2.ClassId,
                    id_lesson = Common.CurrentLesson.Id,
                    type = 1,
                    sid = workId,
                    url,
                    title = "作品分享",
                    members
                };

                string jsonResult = HttpUtility.UploadValuesJson(Common.SaveShare, data, Encoding.UTF8, Encoding.UTF8, dict);
                ResultInfo<int> result = JsonConvert.DeserializeObject<ResultInfo<int>>(jsonResult);
                if (result != null)
                {
                    if (result.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                    {

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("【------------------------】");
                Console.WriteLine("【Common.cs Error 018】" + ex.ToString());
                Console.WriteLine("【------------------------】");
                Common.Trace("SaveWorkShare Error:" + ex.Message);
            }

            return false;
        }
        public static bool GetPushServerInfo()
        {
            try
            {
                NameValueCollection dict = new NameValueCollection();
                dict.Add("token", Common.CurrentUser.Token);
                string jsonResult = HttpUtility.DownloadString(Common.GetPushServer, Encoding.UTF8, dict);
                ResultInfo<ServerInfo> result = JsonConvert.DeserializeObject<ResultInfo<ServerInfo>>(jsonResult);
                if (result != null)
                {
                    if (result.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                    {
                        PushServer = result.Body;
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("【------------------------】");
                Console.WriteLine("【Common.cs Error 019】" + ex.ToString());
                Console.WriteLine("【------------------------】");
                Common.Trace("GetPushServerInfo Error:" + ex.Message);
            }

            return false;
        }
        public static bool GetDocmentServerInfo()
        {
            try
            {
                NameValueCollection dict = new NameValueCollection();
                dict.Add("token", Common.CurrentUser.Token);
                string jsonResult = HttpUtility.DownloadString(Common.GetDocmentServer, Encoding.UTF8, dict);
                Console.WriteLine("GetDocmentServerInfo 获取文件服务器信息" + jsonResult);
                ResultInfo<ServerInfo> result = JsonConvert.DeserializeObject<ResultInfo<ServerInfo>>(jsonResult);
                if (result != null)
                {
                    if (result.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                    {
                        DocmentServer = result.Body;
                        return true;
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("【------------------------】");
                Console.WriteLine("【Common.cs Error 020】" + ex.ToString());
                Console.WriteLine("【------------------------】");
                Common.Trace("GetDocmentServerInfo Error:" + ex.Message);
            }

            return false;
        }
        public static bool GetMQServerInfo()
        {
            try
            {
                NameValueCollection dict = new NameValueCollection();
                dict.Add("token", Common.CurrentUser.Token);
                string jsonResult = HttpUtility.DownloadString(Common.GetMqtServer, Encoding.UTF8, dict);
                Console.WriteLine("GetMQServerInfo 获取mq信息" + jsonResult);
                ResultInfo<ServerInfo> result = JsonConvert.DeserializeObject<ResultInfo<ServerInfo>>(jsonResult);
                if (result != null)
                {
                    if (result.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                    {
                        MqServer = result.Body;
                        //MqServer.host = "49.232.229.202";
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("【------------------------】");
                Console.WriteLine("【Common.cs Error 021】" + ex.ToString());
                Console.WriteLine("【------------------------】");
                Common.Trace("GetMQServerInfo Error:" + ex.Message);
            }
            return false;
        }
        public static bool GetOssServerInfo()
        {

            try
            {
                NameValueCollection dict = new NameValueCollection();
                dict.Add("token", Common.CurrentUser.Token);
                string jsonResult = HttpUtility.DownloadString(Common.GetOssServer, Encoding.UTF8, dict);
                ResultInfo<ServerInfo> result = JsonConvert.DeserializeObject<ResultInfo<ServerInfo>>(jsonResult);
                if (result != null)
                {
                    if (result.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                    {
                        OssServer = result.Body;
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("【------------------------】");
                Console.WriteLine("【Common.cs Error 022】" + ex.ToString());
                Console.WriteLine("【------------------------】");
                Common.Trace("GetOssServerInfo Error:" + ex.Message);
            }

            return false;
        }

    }
}
