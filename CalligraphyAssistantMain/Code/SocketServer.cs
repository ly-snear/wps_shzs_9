using Calligraphyassistant.Message;
using CommonServiceLocator;
using FFmpeg.AutoGen;
using Google.Protobuf;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CalligraphyAssistantMain.Code
{
    public class SocketServer
    {
        private Socket listener = null;
        private List<StudentSocketClient> clientList = null;
        private object lockObj = new object();
        private Dictionary<string, string> imageDict = null;
        private ScreenCapture screenCapture = null;
        private LivePacketParser livePacketParser = null;
        private IEventAggregator eventAggregator;
        private int index = 1;
        private int lastLockScreenMode = 0;
        private int lastViewMode = 0;
        private string lastRtmpUrl = string.Empty;
        private List<ImageMessage> lastImageList = new List<ImageMessage>();
        public bool IsStarted { get; private set; } = false;

        public SocketServer()
        {
            eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            eventAggregator.GetEvent<GetLastImageListEvent>().Subscribe(SendLastImageList);
        }
        public void Start(int port)
        {
            if (IsStarted)
            {
                return;
            }
            try
            {
                listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                clientList = new List<StudentSocketClient>();
                imageDict = new Dictionary<string, string>();
                listener.Bind(new IPEndPoint(IPAddress.Any, port));
                listener.Listen(100);
                listener.BeginAccept(AcceptClient, null);
                IsStarted = true;
                StartProcessThread();
            }
            catch (Exception ex)
            {
                Common.Trace("SocketServer Start Error:" + ex.ToString());
            }
        }

        public void Stop()
        {
            lock (lockObj)
            {
                try
                {
                    listener.Dispose();
                    listener = null;
                    StudentSocketClient[] socketClients = clientList.ToArray();
                    Array.ForEach(socketClients, p => p.Dispose());
                    clientList.Clear();
                    clientList = null;
                    IsStarted = false;
                }
                catch (Exception ex)
                {
                    Common.Trace("SocketServer Stop Error:" + ex.ToString());
                }
            }
        }

        public void SendToAll(byte[] data)
        {
            try
            {
                if (clientList == null)
                {
                    return;
                }
                StudentSocketClient[] socketClients;
                lock (lockObj)
                {
                    socketClients = clientList.ToArray();
                }
                foreach (StudentSocketClient item in socketClients)
                {
                    item.Send(data);
                }
            }
            catch (Exception ex)
            {
                Common.Trace("SocketServer SendToAll Error:" + ex.ToString());
            }
        }

        public void TakePhoto(StudentInfo studentInfo)
        {
            try
            {
                StudentSocketClient client = null;
                lock (lockObj)
                {
                    client = clientList.FirstOrDefault(p => p.StudentInfo == studentInfo);
                }
                if (client != null)
                {
                    byte[] data = RemoteMessage.CreateRemoteMessage((int)ServerMessageType.TakePhoto, RemoteClientType.Server);
                    client.Send(data);
                }
            }
            catch (Exception ex)
            {
                Common.Trace("SocketServer TakePhoto Error:" + ex.ToString());
            }
        }

        public void ClearImageList()
        {
            lastImageList.Clear();
        }

        public void SendImage(string path, int index, int type, bool isSelected)
        {
            if (File.Exists(path))
            {
                string key = Common.ComputeMD5(path);
                if (!imageDict.ContainsKey(key))
                {
                    imageDict.Add(key, path);
                }
                ImageMessage imageMessage = new ImageMessage()
                {
                    Id = key,
                    Index = index,
                    Type = type,
                    IsSelected = isSelected,
                    Data = ByteString.CopyFrom(File.ReadAllBytes(path))
                };
                if (lastImageList.Any(p => p.Index == index))
                {
                    lastImageList[index] = imageMessage;
                }
                else
                {
                    lastImageList.Add(imageMessage);
                    lastImageList.Sort((x, y) => x.Index.CompareTo(y.Index));
                }
                imageMessage.Data = ByteString.CopyFrom(File.ReadAllBytes(path));
                byte[] data = new byte[imageMessage.CalculateSize()];
                CodedOutputStream stream = new CodedOutputStream(data);
                imageMessage.WriteTo(stream);
                stream.Dispose();
                data = RemoteMessage.CreateRemoteMessage((int)ServerMessageType.SendImage, RemoteClientType.Server, data);
                SendToAll(data);
            }
        }

        public void LockScreen(bool isWhiteScreen)
        {
            CommandMessage message;
            if ((lastLockScreenMode == 1 && isWhiteScreen) || (lastLockScreenMode == 2 && !isWhiteScreen))
            {
                message = new CommandMessage() { Command = lastLockScreenMode = 0 };
            }
            else
            {
                message = new CommandMessage() { Command = lastLockScreenMode = isWhiteScreen ? 1 : 2 };
            }
            byte[] data = new byte[message.CalculateSize()];
            CodedOutputStream stream = new CodedOutputStream(data);
            message.WriteTo(stream);
            stream.Dispose();
            data = RemoteMessage.CreateRemoteMessage((int)ServerMessageType.LockScreen, RemoteClientType.Server, data);
            SendToAll(data);
        }


        public void ShowImageMode()
        {
            lastViewMode = 1;
            lastRtmpUrl = string.Empty;
            byte[] data = RemoteMessage.CreateRemoteMessage((int)ServerMessageType.ImageMode, RemoteClientType.Server);
            SendToAll(data);
        }

        public void ShowVideoMode(string rtmpUrl)
        {
            Common.Trace("发送到学生：" + rtmpUrl);
            lastViewMode = 2;
            lastRtmpUrl = rtmpUrl;
            byte[] data = RemoteMessage.CreateRemoteMessage((int)ServerMessageType.VideoMode, RemoteClientType.Server, rtmpUrl);
            SendToAll(data);
        }

        public void StartCameraLive(StudentInfo studentInfo)
        {
            try
            {
                StudentSocketClient client = null;
                lock (lockObj)
                {
                    client = clientList.FirstOrDefault(p => p.StudentInfo == studentInfo);
                }
                if (client != null)
                {
                    byte[] data = RemoteMessage.CreateRemoteMessage((int)ServerMessageType.StartCameraLive, RemoteClientType.Server);
                    client.Send(data);
                }
            }
            catch (Exception ex)
            {
                Common.Trace("SocketServer StartCameraLive Error:" + ex.ToString());
            }
        }

        public void StopCameraLive(StudentInfo studentInfo)
        {
            try
            {
                StudentSocketClient client = null;
                lock (lockObj)
                {
                    client = clientList.FirstOrDefault(p => p.StudentInfo == studentInfo);
                }
                if (client != null)
                {
                    byte[] data = RemoteMessage.CreateRemoteMessage((int)ServerMessageType.StopCameraLive, RemoteClientType.Server);
                    client.Send(data);
                }
            }
            catch (Exception ex)
            {
                Common.Trace("SocketServer StartCameraLive Error:" + ex.ToString());
            }
        }

        public void StopAllCameraLive()
        {
            byte[] data = RemoteMessage.CreateRemoteMessage((int)ServerMessageType.StopCameraLive, RemoteClientType.Server);
            SendToAll(data);
            //ReleaseAllDecoders();
        }

        public void StartShareScreen()
        {
            if (screenCapture != null)
            {
                return;
            }
            screenCapture = new ScreenCapture(GetCaptureScreenRect(), 1280, 720, 15, Common.ScreenShareUrl);
            if (string.IsNullOrEmpty(Common.ScreenShareUrl))
            {
                screenCapture.NewAVPacket += AVPacketProvider_NewAVPacket;
            }
            screenCapture.Start();
        }

        public void StopShareScreen()
        {
            if (screenCapture != null)
            {
                screenCapture.Stop();
                screenCapture.Dispose();
                screenCapture = null;
            }
        }

        public void StartLiveTransfer(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return;
            }
            if (livePacketParser != null)
            {
                if (livePacketParser.Url == url)
                {
                    return;
                }
                else
                {
                    livePacketParser.Stop();
                    livePacketParser.Dispose();
                    livePacketParser = null;
                }
            }
            livePacketParser = new LivePacketParser();
            livePacketParser.Init(url, 10000);
            livePacketParser.NewAVPacket += AVPacketProvider_NewAVPacket;
            livePacketParser.Start();
        }

        public void StopLiveTransfer()
        {
            if (livePacketParser != null)
            {
                livePacketParser.Stop();
                livePacketParser.Dispose();
                livePacketParser = null;
            }
        }

        public void LogoutAll()
        {
            byte[] data = RemoteMessage.CreateRemoteMessage((int)ServerMessageType.Logout, RemoteClientType.Server);
            SendToAll(data);
        }

        private void StartProcessThread()
        {
            Task.Run(() =>
            {
                while (IsStarted)
                {
                    if (index % 5 == 0)
                    {
                        SendHeartbeat();
                    }
                    Thread.Sleep(1000);
                    index++;
                }
            });
        }

        private void SendLastImageList(CommonModel model)
        {
            try
            {
                if (model.Student != null && lastImageList.Count > 0 && clientList != null)
                {
                    ImageMessage[] images = null;
                    StudentSocketClient[] students = null;
                    lock (lockObj)
                    {
                        images = lastImageList.ToArray();
                        students = clientList.ToArray();
                    }
                    StudentSocketClient client = students.FirstOrDefault(p => p.StudentInfo == model.Student);
                    if (client != null)
                    {
                        foreach (ImageMessage imageMessage in images)
                        {
                            byte[] data = new byte[imageMessage.CalculateSize()];
                            CodedOutputStream stream = new CodedOutputStream(data);
                            imageMessage.WriteTo(stream);
                            stream.Dispose();
                            data = RemoteMessage.CreateRemoteMessage((int)ServerMessageType.SendImage, RemoteClientType.Server, data);
                            client.Send(data);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.Trace("SocketServer GetLastImageList Error:" + ex.ToString());
            }
        }

        private void SendHeartbeat()
        {
            HeartbeatMessage heartbeatMessage = new HeartbeatMessage() { CurrentCommands = $"{lastLockScreenMode}[$]{lastViewMode}[$]{lastRtmpUrl}" };
            byte[] data = new byte[heartbeatMessage.CalculateSize()];
            CodedOutputStream stream = new CodedOutputStream(data);
            heartbeatMessage.WriteTo(stream);
            stream.Dispose();
            data = RemoteMessage.CreateRemoteMessage((int)ServerMessageType.Heartbeat, RemoteClientType.Server, data);
            SendToAll(data);
        }

        private void ReleaseAllDecoders()
        {
            StudentSocketClient[] socketClients;
            lock (lockObj)
            {
                socketClients = clientList.ToArray();
            }
            foreach (StudentSocketClient item in socketClients)
            {
                item.ReleaseVideoDecoder();
            }
        }

        private unsafe void SendVideoFrame(AVPacket packet, int frame, int width, int height)
        {
            if (packet.size == 0)
            {
                return;
            }
            try
            {
                byte[] buffer = new byte[packet.size];
                lock (lockObj)
                {
                    Marshal.Copy((IntPtr)packet.data, buffer, 0, buffer.Length);
                }
                VideoFrameMessage message = new VideoFrameMessage()
                {
                    Frame = frame,
                    Width = width,
                    Height = height,
                    Packet = ByteString.CopyFrom(buffer)
                };
                byte[] data = new byte[message.CalculateSize()];
                CodedOutputStream stream = new CodedOutputStream(data);
                message.WriteTo(stream);
                stream.Dispose();
                data = RemoteMessage.CreateRemoteMessage((int)ServerMessageType.SendVideoFrame, RemoteClientType.Server, data);
                SendToAll(data);
            }
            catch (Exception ex)
            {
                Common.Trace("SocketServer SendVideoFrame Error:" + ex.ToString());
            }
        }

        private static Rectangle GetCaptureScreenRect()
        {
            Rectangle sourceRect = new Rectangle(System.Drawing.Point.Empty, Common.RealScreenSize);
            Rectangle rect = sourceRect;
            while (rect.Width % 4 != 0)
            {
                rect.Width--;
            }
            while (rect.Height % 4 != 0)
            {
                rect.Height--;
            }
            if (rect.Width != sourceRect.Width || rect.Height != sourceRect.Height)
            {
                //输出分辨率与屏幕分辨率相同且屏幕分辨率宽高不是4的倍数
                if ((sourceRect.Width - rect.Width > 0 && sourceRect.Width - rect.Width < 4) ||
                    (sourceRect.Height - rect.Height > 0 && sourceRect.Height - rect.Height < 4))
                {
                    sourceRect.X = (sourceRect.Width - rect.Width) / 2;
                    sourceRect.Y = (sourceRect.Height - rect.Height) / 2;
                    sourceRect.Width = rect.Width;
                    sourceRect.Height = rect.Height;
                }
            }
            return sourceRect;
        }

        private void AcceptClient(IAsyncResult ar)
        {
            try
            {
                if (listener == null)
                {
                    return;
                }
                Socket socket = listener.EndAccept(ar);
                if (socket != null && socket.Connected)
                {
                    lock (lockObj)
                    {
                        StudentSocketClient socketClient = new StudentSocketClient(socket);
                        socketClient.OnDisconnected += SocketClient_OnDisconnected;
                        clientList.Add(socketClient);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.Trace("SocketServer AcceptClient Error1:" + ex.ToString());
            }
            try
            {
                if (listener != null)
                {
                    listener.BeginAccept(AcceptClient, null);
                }
            }
            catch (Exception ex)
            {
                Common.Trace("SocketServer AcceptClient Error2:" + ex.ToString());
            }
        }

        private void SocketClient_OnDisconnected(object sender, EventArgs e)
        {
            lock (lockObj)
            {
                try
                {
                    if (clientList != null)
                    {
                        clientList.Remove(sender as StudentSocketClient);
                    }
                }
                catch (Exception ex)
                {
                    Common.Trace("SocketServer SocketClient_OnDisconnected Error:" + ex.ToString());
                }
            }
        }

        private void AVPacketProvider_NewAVPacket(object sender, NewAVPacketEventArgs e)
        {
            SendVideoFrame(e.Packet, e.Frame, (int)e.Size.Width, (int)e.Size.Height);
        }
    }
}
