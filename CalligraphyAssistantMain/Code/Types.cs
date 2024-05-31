using CalligraphyAssistantMain.Code;
using CefSharp;
using FFmpeg.AutoGen;
using Newtonsoft.Json;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Xml.Serialization;

namespace CalligraphyAssistantMain.Code
{
    public class NotifyClassBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
    interface ILiveItem
    {
        string GetFeatureUrl();
        string GetPanoramaUrl();
        string GetPreviewUrl();
    }

    public class NavigationInfo : NotifyClassBase
    {
        private int selectedMainMenu = 2;
        private bool shareCameraMenuHovered = false;
        private int selectedShareMenu = -1;
        private bool isClassBegin = false;

        public int SelectedMainMenu { get => selectedMainMenu; set { selectedMainMenu = value; NotifyPropertyChanged("SelectedMainMenu"); } }
        public bool ShareCameraMenuHovered { get => shareCameraMenuHovered; set { if (shareCameraMenuHovered != value) { shareCameraMenuHovered = value; NotifyPropertyChanged("ShareCameraMenuHovered"); } } }
        public int SelectedShareMenu { get => selectedShareMenu; set { selectedShareMenu = value; NotifyPropertyChanged("SelectedShareMenu"); } }
        public bool IsClassBegin { get => isClassBegin; set { isClassBegin = value; NotifyPropertyChanged("IsClassBegin"); } }
    }

    public class ImageEditInfo : NotifyClassBase
    {
        private int selectedMenu = 1;
        private int selectedImageEditMenu = 2;
        private int selectedGraphicMenu = 0;
        public int SelectedMenu { get => selectedMenu; set { selectedMenu = value; NotifyPropertyChanged("SelectedMenu"); } }
        public int SelectedImageEditMenu { get => selectedImageEditMenu; set { selectedImageEditMenu = value; NotifyPropertyChanged("SelectedImageEditMenu"); } }
        public int SelectedGraphicMenu { get => selectedGraphicMenu; set { selectedGraphicMenu = value; NotifyPropertyChanged("SelectedGraphicMenu"); } }
    }

    public class ImageTagInfo : NotifyClassBase
    {
        private string title = string.Empty;
        private int tag = 2;
        private int tagId = 0;

        public string Title { get => title; set { title = value; NotifyPropertyChanged("Title"); } }
        public int Tag { get => tag; set { tag = value; NotifyPropertyChanged("Tag"); } }
        public int TagId { get => tagId; set { tagId = value; NotifyPropertyChanged("TagId"); } }
    }

    public class AppInfo
    {
        public int Type { get; set; }
        public int Auth { get; set; }
        public string Title { get; set; }
        public string Path { get; set; }
        public bool Integrated { get; set; }
        public bool Admin { get; set; }
        public string Arguments { get; set; }
    }

    public class LoginInfo
    {
        public string Server { get; set; } = "shuhua.nnyun.net";
        public string Port { get; set; } = "80";
        public string Account { get; set; }
        public string Password { get; set; }
        public bool RememberPassword { get; set; }
        public bool AutoLogin { get; set; }
    }

    public class SelectedItem : NotifyClassBase
    {
        private bool isSelected = false;
        private ImageSource backImage = Consts.CameraPresetImage1;
        [XmlIgnore]
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    backImage = isSelected ? Consts.CameraPresetImage2 : Consts.CameraPresetImage1;
                    NotifyPropertyChanged("BackImage");
                    NotifyPropertyChanged("IsSelected");
                }
            }
        }
        public ImageSource BackImage { get { return backImage; } }
    }

    public class CameraItemInfo : SelectedItem, ILiveItem
    {
        private int mode = 1;
        private string title = string.Empty;
        private string selectedStudent = string.Empty;
        public string Title { get { return title; } set { title = value; NotifyPropertyChanged("Title"); } }
        public int Mode
        {
            get { return mode; }
            set
            {
                mode = value; NotifyPropertyChanged("Mode");
                NotifyPropertyChanged("Current");
                if (StudentList != null)
                {
                    StudentList.ForEach(p => p.NotifyPropertyChanged("BackImage"));
                }
            }
        }
        [Newtonsoft.Json.JsonIgnore]
        public CameraItemInfo Current { get { return this; } }
        //[JsonProperty("name")]
        public string Name { get; set; }
        //[JsonProperty("cameraIp")]
        public string CameraIp { get; set; }
        //[JsonProperty("cameraAccount")]
        public string CameraAccount { get; set; }
        //[JsonProperty("cameraPassword")]
        public string CameraPassword { get; set; }
        //[JsonProperty("stuData")]
        public List<StudentInfo> StudentList { get; set; }
        [XmlIgnore]
        public string Profile { get; set; }

        public int Index { get; set; }

        public string SelectedStudent
        {
            get
            {
                return selectedStudent;
            }
            set
            {
                selectedStudent = value;
                NotifyPropertyChanged("SelectedStudent");
            }
        }

        public string GetFeatureUrl()
        {
            //return "rtmp://58.200.131.2:1935/livetv/cctv" + Index;
#if DEV
            return "http://192.168.1.200:8081/1.mp4";
#endif
            //return "rtmp://dev.lingtek.com/live/bj_s21";
            return "rtsp://" + CameraIp + "/ch1";
        }

        public string GetPanoramaUrl()
        {
            //return "rtmp://58.200.131.2:1935/livetv/cctv" + Index;
            return "rtsp://" + CameraIp + "/ch2";
        }

        public string GetPreviewUrl()
        {
#if DEV
            return "http://192.168.1.200:8081/1.mp4";
#endif
            //return "rtmp://dev.lingtek.com/live/bj_s21";
            return "rtsp://" + CameraIp + "/ch3";
        }
    }

    public class CameraItemV2Info : SelectedItem, ILiveItem
    {
        private int mode = 1;
        private string title = string.Empty;
        private string selectedStudent = string.Empty;
        private string selectedCamera = string.Empty;
        public int Index { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get { return title; } set { title = value; NotifyPropertyChanged("Title"); } }
        /// <summary>
        /// 1.特写 2.全景
        /// </summary>
        public int Mode
        {
            get { return mode; }
            set
            {
                mode = value; NotifyPropertyChanged("Mode");
                NotifyPropertyChanged("Current");
                if (StudentList != null)
                {
                    StudentList.ForEach(p => p.NotifyPropertyChanged("BackImage"));
                }
            }
        }
        public CameraItemV2Info Current { get { return this; } }
        public List<StudentV2Info> StudentList { get; set; }
        public List<CameraV2Info> CameraList { get; set; }

        public int StudentIndex { get; set; }
        public int CameraIndex { get; set; }

        public string SelectedStudent
        {
            get
            {
                return selectedStudent;
            }
            set
            {
                selectedStudent = value;
                NotifyPropertyChanged("SelectedStudent");
            }
        }

        public string SelectedCamera
        {
            get
            {
                return selectedCamera;
            }
            set
            {
                selectedCamera = value;
                NotifyPropertyChanged("SelectedCamera");
            }
        }

        public string GetFeatureUrl()
        {
#if DEV
            return "http://192.168.1.200:8081/1.mp4";
#endif           
            if (StudentList == null || StudentList.Count == 0)
            {
                return string.Empty;
            }
            return "rtsp://" + StudentList[StudentIndex].Camera.IP + "/ch1";
        }

        public string GetPanoramaUrl()
        {
#if DEV
            return "http://192.168.1.200:8081/2.mp4";
#endif
            if (CameraList == null || CameraList.Count == 0)
            {
                return string.Empty;
            }
            return "rtsp://" + CameraList[CameraIndex].IP + "/ch2";
        }

        public string GetPreviewUrl()
        {
#if DEV
            return "http://192.168.1.200:8081/1.mp4";
#endif 
            if (StudentList == null || StudentList.Count == 0)
            {
                return string.Empty;
            }
            return "rtsp://" + StudentList[StudentIndex].Camera.IP + "/ch3";
        }
    }
    [AddINotifyPropertyChangedInterface]
    public class TeacherInfo
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("created")]
        public string Created { get; set; }
        [JsonProperty("level")]
        public int Level { get; set; }
        [JsonProperty("idCity")]
        public int IdCity { get; set; }
        [JsonProperty("idZone")]
        public int IdZone { get; set; }
        [JsonProperty("idSchool")]
        public int IdSchool { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("course")]
        public string Course { get; set; }
        [JsonProperty("intro")]
        public string Intro { get; set; }
        [JsonProperty("avatar")]
        public string Avatar { get; set; }
        [JsonProperty("token")]
        public string Token { get; set; }
        [JsonProperty("role")]
        public string Role { get; set; }
    }

    public class ClassInfo
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("idTeacher")]
        public int IdTeacher { get; set; }
    }

    public class LessonInfo
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("index")]
        public int Index { get; set; }
        [JsonProperty("idClass")]
        public int IdClass { get; set; }
        [JsonProperty("created")]
        public string Created { get; set; }
    }

    public class ResultInfo<T>
    {
        [JsonProperty("ok")]
        public string Ok { get; set; }
        [JsonProperty("msg")]
        public string Msg { get; set; } = string.Empty;
        [JsonProperty("body")]
        public T Body { get; set; }
    }

    public class ResultCalligraphyListInfo<T>
    {
        [JsonProperty("page")]
        public ResultCalligraphyListPageInfo Page { get; set; }
        [JsonProperty("data")]
        public T Data { get; set; }
    }

    public class ResultClassRoomV2Info
    {
        [JsonProperty("class_data")]
        public ClassDetailsV2Info ClassData { get; set; }
        [JsonProperty("room_data")]
        public ClassRoomDetailsV2Info ClassRoomData { get; set; }
    }

    public class ResultClassRoomV2ExInfo
    {
        [JsonProperty("grade")]
        public GradeDataV2ExInfo GradeData { get; set; }
        [JsonProperty("gradeStudents")]
        public List<GradeStudentV2ExInfo> GradeStudentsData { get; set; }
        [JsonProperty("room")]
        public ClassRoomDataV2ExInfo RoomData { get; set; }
        [JsonProperty("roomSeats")]
        public List<ClassRoomSeatsV2ExInfo> RoomSeatsData { get; set; }
        [JsonProperty("classRoomGroups")]
        public List<ClassRoomGroupsV2ExInfo> RoomGroupsData { get; set; }
    }

    public class GradeDataV2ExInfo
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("id_teacher")]
        public int TeacherId { get; set; }
    }

    public class GradeStudentV2ExInfo
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("sno")]
        public string SN { get; set; }
    }

    public class ClassRoomDataV2ExInfo
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("cols")]
        public int Cols { get; set; }
        [JsonProperty("rows")]
        public int Rows { get; set; }
        [JsonProperty("seats")]
        public int Seats { get; set; }
    }

    public class ClassRoomSeatsV2ExInfo
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("row")]
        public int Row { get; set; }
        [JsonProperty("col")]
        public int Col { get; set; }
        [JsonProperty("id_group")]
        public int GroupId { get; set; }
        [JsonProperty("name_group")]
        public string GroupName { get; set; }
        [JsonProperty("state")]
        public string State { get; set; }
        [JsonProperty("ip")]
        public string Ip { get; set; }
        [JsonProperty("id_camera")]
        public int CameraId { get; set; }
        [JsonProperty("name_camera")]
        public string CameraName { get; set; }
        [JsonProperty("angle")]
        public float Angle { get; set; }
        [JsonProperty("preset")]
        public int Preset { get; set; }
    }

    public class ClassRoomGroupsV2ExInfo
    {
        [JsonProperty("id_group")]
        public int GroupId { get; set; }
        [JsonProperty("name_group")]
        public string GroupName { get; set; }
        [JsonProperty("seats")]
        public List<ClassRoomGroupsSeatV2ExInfo> Seats { get; set; }
    }

    public class ClassRoomGroupsSeatV2ExInfo
    {
        [JsonProperty("index")]
        public int Index { get; set; }
        [JsonProperty("id_seat")]
        public int SeatId { get; set; }
        [JsonProperty("name_seat")]
        public string SeatName { get; set; }
        [JsonProperty("row")]
        public int Row { get; set; }
        [JsonProperty("col")]
        public int Col { get; set; }
        [JsonProperty("state")]
        public string State { get; set; }
        [JsonProperty("ip")]
        public string Ip { get; set; }
        [JsonProperty("id_camera")]
        public int CameraId { get; set; }
        [JsonProperty("preset")]
        public int Preset { get; set; }
        [JsonProperty("angle")]
        public float Rotation { get; set; }
        [JsonProperty("user")]
        public string Account { get; set; }
        [JsonProperty("pass")]
        public string Password { get; set; }
        [JsonProperty("students")]
        public List<ClassRoomGroupsSeatStudentV2ExInfo> Students { get; set; }
    }

    public class ClassRoomGroupsSeatStudentV2ExInfo
    {
        [JsonProperty("order")]
        public int Order { get; set; }
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class ClassRoomDetailsV2Info
    {
        [JsonProperty("nameRoom")]
        public string Name { get; set; }
        [JsonProperty("idSchool")]
        public int SchoolId { get; set; }
        [JsonProperty("idRoom")]
        public int RoomId { get; set; }
        [JsonProperty("cols")]
        public int Cols { get; set; }
        [JsonProperty("rows")]
        public int Rows { get; set; }
        [JsonProperty("groups")]
        public GroupV2Info[] GroupList { get; set; }
        [JsonProperty("cameras")]
        public CameraV2Info[] CameraList { get; set; }
        [JsonProperty("seats")]
        public SeatV2Info[] SeatList { get; set; }
    }

    public class ClassDetailsV2Info
    {
        [JsonProperty("idClass")]
        public int ClassId { get; set; }
        [JsonProperty("nameClass")]
        public string ClassName { get; set; }
        [JsonProperty("idTeacher")]
        public int TeacherId { get; set; }
        [JsonProperty("nameTeacher")]
        public string TeacherName { get; set; }
        [JsonProperty("student")]
        public StudentV2Info[] StudentList { get; set; }
    }

    public class ResultCalligraphyListPageInfo
    {
        [JsonProperty("page")]
        public int Page { get; set; }
        [JsonProperty("size")]
        public int Size { get; set; }
        [JsonProperty("start")]
        public int Start { get; set; }
        [JsonProperty("end")]
        public int End { get; set; }
        [JsonProperty("page_size")]
        public int PageSize { get; set; }
        [JsonProperty("page_end")]
        public int PageEnd { get; set; }
        [JsonProperty("total")]
        public int Total { get; set; }
        [JsonProperty("pages")]
        public int Pages { get; set; }
    }

    public class ClassRoomResultInfo
    {
        [JsonProperty("content")]
        public ClassRoomInfo[] ClassRoomList { get; set; }
    }

    public class ClassRoomInfo
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("group")]
        public string Group { get; set; }
        [JsonProperty("idSchool")]
        public string IdSchool { get; set; }
        [JsonProperty("camera")]
        public List<ClassRoomItemInfo> CameraList { get; set; }
    }

    public class ClassRoomV2Info
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("created")]
        public string Created { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("seats")]
        public int Seats { get; set; }
        [JsonProperty("cols")]
        public int Cols { get; set; }
        [JsonProperty("position")]
        public string Position { get; set; }
        [JsonProperty("image")]
        public string Image { get; set; }
        [JsonProperty("idSchool")]
        public int IdSchool { get; set; }
        [JsonProperty("idUser")]
        public int IdUser { get; set; }
        [JsonProperty("describe")]
        public string Describe { get; set; }
        [JsonProperty("rows")]
        public int Rows { get; set; }
        [JsonProperty("groups")]
        public int Groups { get; set; }
        [JsonProperty("cameras")]
        public int Cameras { get; set; }
        [JsonProperty("factSeats")]
        public int FactSeats { get; set; }
        [JsonProperty("factRows")]
        public int FactRows { get; set; }
    }

    public class ClassRoomItemInfo
    {
        [JsonProperty("param1")]
        public string Param1 { get; set; }
        [JsonProperty("param2")]
        public string Param2 { get; set; }
        [JsonProperty("param3")]
        public string Param3 { get; set; }
        [JsonProperty("network")]
        public string Network { get; set; }
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
    }
    [AddINotifyPropertyChangedInterface]
    public class StudentInfo : NotifyClassBase
    {
        [XmlIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public bool IsContrast { get; set; }
        [XmlIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public bool IsTranscribe {  get; set; }
        private bool isSelected = false;
        private bool isConnected = false;
        private bool isLogined = false;
        private Brush foreground = null;
        private ImageSource backImage = Consts.CameraPresetImage1;

        [XmlIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public Brush Foreground
        {
            get { return foreground; }
            set { foreground = value; }
        }

        [XmlIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public bool IsConnected
        {
            get
            {
                return this.isConnected;
            }
            set
            {
                if (this.isConnected != value)
                {
                    this.isConnected = value;
                    this.NotifyPropertyChanged("IsConnected");
                    this.NotifyPropertyChanged("IsConnectedAndLogined");
                }
            }
        }

        [XmlIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public bool IsConnectedAndLogined
        {
            get
            {
                return (this.IsConnected && this.IsLogined);
            }
        }

        [XmlIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public bool IsLogined
        {
            get
            {
                return this.isLogined;
            }
            set
            {
                if (this.isLogined != value)
                {
                    this.isLogined = value;
                    this.NotifyPropertyChanged("IsLogined");
                    this.NotifyPropertyChanged("IsConnectedAndLogined");
                }
            }
        }

        [XmlIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                NotifyPropertyChanged("BackImage");
            }
        }
        [XmlIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public ImageSource BackImage
        {
            get
            {
                if (Owner != null)
                {
                    if (Owner.Mode == 1)
                    {
                        backImage = isSelected ? Consts.CameraPresetImage2 : Consts.CameraPresetImage1;
                    }
                    else
                    {
                        backImage = Consts.CameraPresetImage1;
                    }
                }
                return backImage;
            }
        }
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("group")]
        public int Group { get; set; }
        [JsonProperty("exStudent")]
        public string Name { get; set; }
        public string IP { get; set; }
        public int Rotation { get; set; }
        public int Preset { get; set; }
        public int Col { get; set; }
        public int Row { get; set; }
        [JsonProperty("index")]
        public int Number { get; set; }
        [XmlIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public CameraItemInfo Owner { get; set; }
        [JsonProperty("sn")]
        public string SN { get; set; }
        [XmlIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public ImageBrush PreviewImageSource { get; set; }
        [XmlIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public System.Windows.Media.Imaging.WriteableBitmap Image { get; set; }
        [XmlIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public ImageBrush FullPreviewImageSource { get; set; }
    }

    public class StudentV2Info : SeatV2Info
    {
        private bool isSelected = false;
        private ImageSource backImage = Consts.CameraPresetImage1;
        [Newtonsoft.Json.JsonIgnore]
        [System.Xml.Serialization.XmlIgnore]

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                NotifyPropertyChanged("BackImage");
            }
        }
        [Newtonsoft.Json.JsonIgnore]
        [System.Xml.Serialization.XmlIgnore]

        public ImageSource BackImage
        {
            get
            {
                if (Owner != null)
                {
                    if (Owner.Mode == 1)
                    {
                        backImage = isSelected ? Consts.CameraPresetImage2 : Consts.CameraPresetImage1;
                    }
                    else
                    {
                        backImage = Consts.CameraPresetImage1;
                    }
                }
                return backImage;
            }
        }
        [Newtonsoft.Json.JsonIgnore]
        [System.Xml.Serialization.XmlIgnore]
        public int Number { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        [System.Xml.Serialization.XmlIgnore]
        public CameraV2Info Camera { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        [System.Xml.Serialization.XmlIgnore]
        public CameraItemV2Info Owner { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        [System.Xml.Serialization.XmlIgnore]
        public string SN { get; set; }
    }
    [AddINotifyPropertyChangedInterface]

    public class SeatV2Info : NotifyClassBase
    {
        [JsonProperty("idStudent")]
        public int StudentId { get; set; }
        [JsonProperty("idSeat")]
        public int SeatId { get; set; }
        [JsonProperty("idGroup")]
        public int GroupId { get; set; }
        [JsonProperty("nameStudent")]
        public string StudentName { get; set; }
        [JsonProperty("row")]
        public int Row { get; set; }
        [JsonProperty("col")]
        public int Col { get; set; }
        [JsonProperty("sip")]
        public string IP { get; set; }
        [JsonProperty("idCamera")]
        public int CameraId { get; set; }
        [JsonProperty("angle")]
        public float Rotation { get; set; }
        [JsonProperty("nameGroup")]
        public string GroupName { get; set; }
        [JsonProperty("preset")]
        public int Preset { get; set; }
        [JsonProperty("describe")]
        public string Describe { get; set; }
    }

    public class GroupV2Info : NotifyClassBase
    {
        [JsonProperty("idGroup")]
        public int GroupId { get; set; }
        [JsonProperty("nameGroup")]
        public string GroupName { get; set; }
        [JsonProperty("camera")]
        public CameraV2Info[] CameraList { get; set; }
    }

    public class CameraV2Info : NotifyClassBase
    {
        private bool isSelected = false;
        private ImageSource backImage = Consts.CameraPresetImage1;
        [Newtonsoft.Json.JsonIgnore]
        [System.Xml.Serialization.XmlIgnore]
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                NotifyPropertyChanged("BackImage");
            }
        }
        [Newtonsoft.Json.JsonIgnore]
        [System.Xml.Serialization.XmlIgnore]
        public ImageSource BackImage
        {
            get
            {
                if (Owner != null)
                {
                    if (Owner.Mode == 2)
                    {
                        backImage = isSelected ? Consts.CameraPresetImage2 : Consts.CameraPresetImage1;
                    }
                    else
                    {
                        backImage = Consts.CameraPresetImage1;
                    }
                }
                return backImage;
            }
        }
        [JsonProperty("idCamera")]
        public int Id { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("ip")]
        public string IP { get; set; }
        [JsonProperty("name")]
        public string Account { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
        [JsonProperty("state")]
        public int State { get; set; }
        public int Number { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        [System.Xml.Serialization.XmlIgnore]
        public string Profile { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        [System.Xml.Serialization.XmlIgnore]
        public CameraItemV2Info Owner { get; set; }
    }

    public class SemesterInfo
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("semester")]
        public int Semester { get; set; }
    }

    public class ImageThemeInfo
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("count")]
        public int Count { get; set; }
    }

    public class ImageThemeItemInfo
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
    }

    public class StudentWorkInfo
    {
        [JsonProperty("content")]
        public StudentWorkDetailsInfo[] Content { get; set; }
        [JsonProperty("totalPages")]
        public int PageCount { get; set; }
        [JsonProperty("totalElements")]
        public string RecordCount { get; set; }
    }
    [AddINotifyPropertyChangedInterface]
    public class StudentWorkDetailsInfo : NotifyClassBase
    {
        private string correct = string.Empty;
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("product")]
        public string Work { get; set; }
        [JsonProperty("correct")]
        public string Correct { get { return correct; } set { correct = value; NotifyPropertyChanged("Correct"); NotifyPropertyChanged("IsCorrect"); } }
        [JsonProperty("comment")]
        public string Comment { get; set; }
        [JsonProperty("idStudent")]
        public int StudentId { get; set; }
        [JsonProperty("exStudent")]
        public string StudentName { get; set; }
        [JsonProperty("group")]
        public string Group { get; set; }
        [JsonProperty("created")]
        public string Created { get; set; }
        public DateTime time { get; set; }
        public int Number { get; set; }
        public string Date
        {
            get
            {
                try
                {
                    return time.ToString("MM/dd");
                }
                catch (Exception ex)
                {
                    Common.Trace("Types StudentWorkDetailsInfo Error:" + ex.Message);
                    return string.Empty;
                }
            }
        }
        public string ClassName { get; set; }
        public string GroupName { get; set; }
        public string LocalPath { get; set; }
        public Visibility IsCorrect { get { return string.IsNullOrEmpty(Correct) ? Visibility.Collapsed : Visibility.Visible; } }
        public int id_teacher { get; set; }
        public long id_student { get;set; }
        public string name_teacher { get; set; }
        public int review {  get; set; }
        public bool IsSelected { get; set; }
        public string suffix { get; set; }
        public string name_student { get; set; }
        public int type { get; set; }
    }
    [AddINotifyPropertyChangedInterface]
    public class MembersItem
    {
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int cid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int member_type { get; set; }
        /// <summary>
        /// 学生
        /// </summary>
        public string name_type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int id_member { get; set; }
        /// <summary>
        /// 张三
        /// </summary>
        public string name_member { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int review { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime time { get; set; }
    }
    [AddINotifyPropertyChangedInterface]
    public class ShareInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int id_lesson { get; set; }
        /// <summary>
        /// 美术
        /// </summary>
        public string name_lesson { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int id_class { get; set; }
        /// <summary>
        /// 初中2020级展会班级
        /// </summary>
        public string name_class { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 作品
        /// </summary>
        public string name_type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int sid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string name_sid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 作品分享
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int audit { get; set; }
        /// <summary>
        /// 初中2020级展会班级
        /// </summary>
        public string name_audit { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string audit_time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int previous { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string name_previous { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int user_type { get; set; }
        /// <summary>
        /// 教师
        /// </summary>
        public string name_user_type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int user { get; set; }
        /// <summary>
        /// 讷纳渔科技
        /// </summary>
        public string name_user { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<MembersItem> members { get; set; }=new List<MembersItem>();
        public string memberNames{ get =>string.Join("、", members.Select(p => p.name_member).ToArray());}
    }
    public class DownloadInfo : NotifyClassBase
    {
        private int currentProgress = 0;

        public int Id { get; set; }
        public string FileName { get; set; }
        public string SavePath { get; set; }
        public int TotalProgress { get; set; }
        public int CurrentProgress
        {
            get { return currentProgress; }
            set
            {
                if (currentProgress != value)
                {
                    currentProgress = value;
                    NotifyPropertyChanged("CurrentProgress");
                }
            }
        }
        public bool IsComplete { get; set; }
        public IDownloadItemCallback Callback { get; set; }
    }

    public class CalligraphyImageInfo : NotifyClassBase
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
    }


    public class DynastyInfo : NotifyClassBase
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        public bool IsSelect { get; set; }
    }

    public class FontInfo : NotifyClassBase
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        public bool IsSelect { get; set; }
    }

    public class AuthorInfoInfo : NotifyClassBase
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("dynasty_id")]
        public int DynastyId { get; set; }
        [JsonProperty("dynasty")]
        public string Dynasty { get; set; }
        public bool IsSelect { get; set; }
    }

    public class CalligraphyImageSearchInfo
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("author")]
        public string Author { get; set; }
        [JsonProperty("font")]
        public string Font { get; set; }
        [JsonProperty("dynasty")]
        public string Dynasty { get; set; }
        [JsonProperty("size")]
        public int Size { get; set; }
        [JsonProperty("page")]
        public int Page { get; set; }
    }

    public class CalligraphyWordSearchInfo
    {
        [JsonProperty("word")]
        public string Word { get; set; }
        [JsonProperty("author")]
        public string Author { get; set; }
        [JsonProperty("font")]
        public string Font { get; set; }
        [JsonProperty("dynasty")]
        public string Dynasty { get; set; }
        [JsonProperty("size")]
        public int Size { get; set; }
        [JsonProperty("page")]
        public int Page { get; set; }
    }

    public class CalligraphyImageDetailsInfo : NotifyClassBase
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("author_id")]
        public int AuthorId { get; set; }
        [JsonProperty("author")]
        public string Author { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("font_id")]
        public int FontId { get; set; }
        [JsonProperty("font")]
        public string Font { get; set; }
        [JsonProperty("dynasty_id")]
        public int DynastyId { get; set; }
        [JsonProperty("dynasty")]
        public string Dynasty { get; set; }
        [JsonProperty("cover")]
        public CalligraphyImageDetailsCoverInfo[] CoverArr { get; set; }
        public int ImageCount { get { return CoverArr == null ? 0 : CoverArr.Length; } }
    }

    public class CalligraphyWordDetailsInfo : NotifyClassBase
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("author_id")]
        public int AuthorId { get; set; }
        [JsonProperty("author")]
        public string Author { get; set; }
        [JsonProperty("font_id")]
        public int FontId { get; set; }
        [JsonProperty("font")]
        public string Font { get; set; }
        [JsonProperty("dynasty_id")]
        public int DynastyId { get; set; }
        [JsonProperty("dynasty")]
        public string Dynasty { get; set; }
        [JsonProperty("copybook_id")]
        public string CopybookId { get; set; }
        [JsonProperty("copybook")]
        public string Copybook { get; set; }
        [JsonProperty("word_id")]
        public string WordId { get; set; }
        [JsonProperty("word")]
        public string Word { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        public string LocalPath { get; set; }
        public string FullTitle { get; set; }
    }

    public class CalligraphyImageDetailsCoverInfo : NotifyClassBase
    {
        [JsonProperty("order")]
        public int Order { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        public string LocalPath { get; set; }
        public string FullTitle { get; set; }
    }

    public class CalligraphyPickImageInfo
    {
        public string Title { get; set; }
        public string LocalPath { get; set; }
        public CalligraphyMode Mode { get; set; }
        public string Url { get; set; }
        public int ClipCount { get; set; } = 1;
    }

    public class ResultResourceFolderInfo
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public class ResultResourceItemInfo
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("time")]
        public DateTime Time { get; set; }
    }
    [AddINotifyPropertyChangedInterface]
    public class ResourceItemInfo : NotifyClassBase
    {
        private DownloadState state = DownloadState.Downloading;
        private List<ResourceDispensedUser> dispensedUsers=new List<ResourceDispensedUser>();
        private int noCompleteCount;

        public int ServerId { get; set; }
        public string ServerUrl { get; set; }
        public string FileName { get; set; }
        public string LocalPath { get; set; }
        public long FileSize { get; set; }
        public DateTime ModifyTime { get; set; }
        public ImageSource FileIcon { get; set; }
        public CancellationTokenSource CancelToken { get; set; }
        public object Owner { get; set; }
        public string FileSizeStr { get { return Common.GetFileSizeStr(FileSize); } }

        public DownloadState State
        {
            get { return state; }
            set
            {
                if (state != value)
                {
                    state = value;
                    NotifyPropertyChanged("State");
                    NotifyPropertyChanged("StateImage");
                }
            }
        }

        public ImageSource StateImage
        {
            get { return state == DownloadState.Downloading ? Consts.DownloadingIcon : Consts.DownloadedIcon; }
        }
        public List<ResourceDispensedUser> DispensedUsers { get => dispensedUsers; set { dispensedUsers = value; CompleteCount = value.Count(p => p.IsComplete); UserCount = value.Count; } }
        public int CompleteCount { get;set; }
        public int UserCount { get; set; }

    }
    [AddINotifyPropertyChangedInterface]
    public class ResourceDispensedUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public bool IsComplete { get; set; }
        public string ip { get; set; }

    }

    public class CameraGroupInfo
    {
        public string Title { get; set; }
        public List<CameraGroupItemInfo> CameraGroupItemList { get; set; }
    }

    public class CameraGroupItemInfo
    {
        public string Title { get; set; }
        public string PreviewUrl { get; set; }
        public string Url { get; set; }
    }

    public class QuickAnswerInfo : NotifyClassBase
    {
        public int id { get; set; }
        public int question { get; set; }

        public string title { get; set; }

        public string content { get; set; }

        public string audio { get; set; }

        public string video { get; set; }

        public int user { get; set; }

        public string name_user { get; set; }

        public DateTime time { get; set; }

        public int id_class { get; set; }

        public string name_class { get; set; }

        public int id_lesson { get; set; }

        public string name_lesson { get; set; }

        /// <summary>
        /// 问题类型
        /// 0:选择题 1:主观题
        /// </summary>
        //[Newtonsoft.Json.JsonIgnore]
        public int answer_type { get; set; }

        /// <summary>
        /// 答题类型
        /// 0:抢答 1:随机抽人 2:选择答题学生
        /// </summary>
        //[Newtonsoft.Json.JsonIgnore]
        public int category { get; set; }

        /// <summary>
        /// 选择的答题学生
        /// </summary>
        //[Newtonsoft.Json.JsonIgnore]
        public List<StudentInfo> answer_students { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        [System.Xml.Serialization.XmlIgnore]
        public string status { get; set; } = "0";
        [Newtonsoft.Json.JsonIgnore]
        [System.Xml.Serialization.XmlIgnore]
        public string type { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        [System.Xml.Serialization.XmlIgnore]
        public List<Answer> students {  get; set; }
        public Object questions {  get; set; }
    }
    public class Answer
    {
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int id_student { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int value { get; set; }
        /// <summary>
        /// 张三
        /// </summary>
        public string name_student { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string audio { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string video { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double score { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int grade { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double star { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int order { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string time { get; set; }
    }
    public class VoteInfo : NotifyClassBase
    {
        public int id { get; set; }
        public string title { get; set; }
        public int content { get; set; }
        public int id_lesson { get; set; }
        public DateTime time { get; set; }
        public string status { get; set; } = "0";
        [Newtonsoft.Json.JsonIgnore]
        [System.Xml.Serialization.XmlIgnore]
        public VoteResult voteResult { get; set; }
    }
    public class VoteResult : NotifyClassBase
    {
        /// <summary>
        /// 
        /// </summary>
        public int result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string letter { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int count { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<StudentItem> student { get; set; }
    }
    public class StudentItem
    {
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 张三
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string letter { get; set; }
    }
    public class PaperInfo : NotifyClassBase
    {
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool shared { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int id_user { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string user { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string remarks { get; set; }
        public List<PaperTopicInfo> topics {  get; set; }
        public string answers { get; set; }
        public string status { get; set; }="0";
        private List<ResourceDispensedUser> dispensedUsers = new List<ResourceDispensedUser>();
        public List<ResourceDispensedUser> DispensedUsers { get => dispensedUsers; set { dispensedUsers = value; CompleteCount = value.Count(p => p.IsComplete); UserCount = value.Count; } }
        public int CompleteCount { get; set; }
        public int UserCount { get; set; }
        //public List<StudentInfo> StudentInfos { get; set; }
        public PaperResult paperResult { get; set; }
    }
    public class PaperTopicInfo : NotifyClassBase
    {
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int id_type { get; set; }
        /// <summary>
        /// 单选题
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int id_plate { get; set; }
        /// <summary>
        /// 09.15测试
        /// </summary>
        public string plate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int id_resource { get; set; }
        /// <summary>
        /// 美术
        /// </summary>
        public string resource { get; set; }
        public string name_science { get; set; }
        public string name_period { get; set; }
        public string name_grade { get; set; }
        public string name_chapters { get; set; }
        public string name_material { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool shared { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double score { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int state { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int order { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int id_user { get; set; }
        /// <summary>
        /// 讷纳渔科技
        /// </summary>
        public string user { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string remarks { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string tags { get; set; }
        public List<PaperTopicAnswer> answers
        {
            get; set;
        }

        public List<PaperTopicOption> options {  get; set; }

        public string GetAnswerStr
        {
            get
            {
                string str = "";
                try {
                    if (id_type == 1 || id_type == 2)
                    {
                        string[] Answers = new string[] { "无效", "A", "B", "C", "D", "E", "F", "G", "K" };
                        options.ForEach(p =>
                        {
                            if (p.answer == 1)
                            {
                                if (p.order < Answers.Length)
                                    str += Answers[p.order];
                            }
                        });
                    }
                    if (id_type == 3)
                    {
                        if (answers != null && answers.Count > 0)
                        {
                            str = answers[0].answer == "1" ? "正确" : "错误";
                        }
                    }
                    if (id_type == 5)
                    {
                        if (answers != null && answers.Count > 0)
                        {
                            str = answers[0].answer;
                        }
                    }
                    if (id_type == 4)
                    {
                        if (options != null && options.Count > 0)
                        {
                            foreach (var item in options)
                            {
                                var item1 = options.FirstOrDefault(p => p.order == item.answer);
                                string title=item.title + "=";
                                if(item1 != null)
                                {
                                    title += item1.caption + "  ";
                                }
                                str += title;
                            }

                        }
                    }
                }
                catch
                {
                    str = "答案解析错误";
                }
                return str;
            }
        }
    }
    public class PaperTopicAnswer
    {
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int order { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string answer { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double score { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string remarks { get; set; }
    }
    public class PaperTopicOption:NotifyClassBase
    {
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int order { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string caption { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int answer { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double score { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string remarks { get; set; }
    }
    public class ActiveInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int id_class { get; set; }
        /// <summary>
        /// 测试专用2
        /// </summary>
        public string name_class { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int id_lesson { get; set; }
        /// <summary>
        /// 书法
        /// </summary>
        public string name_lesson { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int member_type { get; set; }
        /// <summary>
        /// 教师
        /// </summary>
        public string name_member_type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int id_member { get; set; }
        /// <summary>
        /// 讷纳渔科技
        /// </summary>
        public string name_member { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 主观题
        /// </summary>
        public string name_type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 教师:讷纳渔科技，主观问题
        /// </summary>
        public string describe { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int user_type { get; set; }
        /// <summary>
        /// 教师
        /// </summary>
        public string name_user_type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int user { get; set; }
        /// <summary>
        /// 讷纳渔科技
        /// </summary>
        public string name_user { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string time { get; set; }
    }
    [AddINotifyPropertyChangedInterface]
    public class DiscussionItem
    {
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int order { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 教师
        /// </summary>
        public string name_type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int id_member { get; set; }
        /// <summary>
        /// 讷纳渔科技
        /// </summary>
        public string name_member { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int id_class { get; set; }
        /// <summary>
        /// 初中2020级展会班级
        /// </summary>
        public string name_class { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int id_lesson { get; set; }
        /// <summary>
        /// 美术
        /// </summary>
        public string name_lesson { get; set; }
        /// <summary>
        /// 张三的评议
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 画法1112312
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double score { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int grade { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double star { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime time { get; set; }
    }
    [AddINotifyPropertyChangedInterface]
    public class DiscussionInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 作品
        /// </summary>
        public string name_type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int id_resource { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string name_resource { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int id_share { get; set; }
        /// <summary>
        /// 作品分享
        /// </summary>
        public string name_share { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 张三的评议
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 画法OK
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int member_score { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double score { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int member_grade { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int grade { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int member_star { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double star { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int member { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int type_member { get; set; }
        /// <summary>
        /// 教师
        /// </summary>
        public string name_type_member { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int id_member { get; set; }
        /// <summary>
        /// 讷纳渔科技
        /// </summary>
        public string name_member { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double average_score { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double average_grade { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double average_star { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int min_order { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int max_order { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int member_sum { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DiscussionItem first { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DiscussionItem last { get; set; }
        public bool IsSender { get; set; }
    }
    public class ServerInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string host { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int port { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string account { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string password { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ssl { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int location { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int own { get; set; }
        /// <summary>
        /// 公司测试推流服务器
        /// </summary>
        public string remarks { get; set; }
    }
    public class SDLVideoInfo
    {
        public IntPtr ControlHandle { get; set; }
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }
        public IntPtr SDLWindow { get; set; }
        public IntPtr SDLRenderer { get; set; }
        public IntPtr SDLTexture { get; set; }
        public SDL2.SDL_Rect SDLRect;
    }

    public class SDLAudioInfo
    {
        public bool IsPlaying { get; set; }
        public uint AudioDeviceId { get; set; }
        public SDL2.SDL_AudioSpec AudioSpec;
    }

    public class ImageConverterInfo
    {
        public AVPixelFormat SourceFormat;
        public AVPixelFormat TargetFormat;
        public byte_ptrArray4 Data;
        public int_array4 LineSize;
        public unsafe SwsContext* Context;
        public int SourceWidth;
        public int SourceHeight;
        public int TargetWidth;
        public int TargetHeight;
    }

    public class AVFrameEx
    {
        public byte[] data;
        public int linesize;
        public int width;
        public int height;
        public int sample_rate;
        public int channels;
        public int channel_layout;
        public AVSampleFormat format;
        public int nb_samples;
    }

    public struct SFCommand
    {
        public static int STRUCT_LEN = 24;
        public byte[] Header;//学生命令头:0x150x6B
        public byte[] Rev;//保留
        public byte[] Len;//长度0x10 默认值0x10，根据后续需求可变
        public byte[] Signature;//0x4C 0x4F 0x47 0x05  固定值，登陆命令特征码
        public byte[] EJ;//EJ 0X01表示耳机正常。0x00表示未插入或故障
        public byte Mic;//mic 0X01表示MIC正常。0x00表示未插入或故障
        public byte Vol;//音量 当前音量值范围：0x00-0x0F
        public byte Hand;//举手 0x01表示举手状态；0X00表示正常状态；可通过网络发命令清除举手状态
        public byte Photo;//照相 0X01表示拍照按钮按下；0X00表示拍照按钮放开；(终端单次发送后自动清零。)210401新增
        public byte Video;//录像 x01表示开始录像；0X00终止录像(默认状态)；(第一次按下开始录像，第二次按下终止录像210401新增
        public byte[] Reserved_filling; //预留

        public SFCommand(byte[] In)
        {
            int point = 0;
            Header = new byte[2];
            Buffer.BlockCopy(In, point, Header, 0, Header.Length);
            point += Header.Length;
            Rev = new byte[4];
            Buffer.BlockCopy(In, point, Rev, 0, Rev.Length);
            point += Rev.Length;
            Len = new byte[2];
            Buffer.BlockCopy(In, point, Len, 0, Len.Length);
            point += Len.Length;
            Signature = new byte[4];
            Buffer.BlockCopy(In, point, Signature, 0, Signature.Length);
            point += Signature.Length;
            EJ = new byte[2];
            Buffer.BlockCopy(In, point, EJ, 0, EJ.Length);
            point += EJ.Length;
            Mic = In[point];
            point += 1;
            Vol = In[point];
            point += 1;
            Hand = In[point];
            point += 1;

            Photo = In[point];
            point += 1;
            Video = In[point];
            point += 1;

            Reserved_filling = new byte[5];
            Buffer.BlockCopy(In, point, Reserved_filling, 0, Reserved_filling.Length);
            point += Reserved_filling.Length;
        }
    }
    public class OssItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string accessid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string policy { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string signature { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string dir { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string host { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string expire { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string callback { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string domain { get; set; }
    }
    public class AnswersItem
    {
        /// <summary>
        /// 
        /// </summary>
        public int order { get; set; }
        /// <summary>
        /// 以线条勾勒为主，色彩较为简单
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string caption { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string answer { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double score { get; set; }
    }

    public class AnswerItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string values { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string time { get; set; }
    }

    public class StudentsItem
    {
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 张三
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<AnswerItem> answer { get; set; }
    }

    public class TopicsItem
    {
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 以下哪个选项最能概括版画的特点?
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<AnswersItem> answers { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<StudentsItem> students { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double score { get; set; }
    }
    public class PaperResult
    {
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 20231218考卷
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<TopicsItem> topics { get; set; }
    }
    public class StudentPrompts
    {
        public int id { get; set; }
        public string name { get; set; }
        public MessageType type { get; set; }
    }
}
