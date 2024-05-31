using FFmpeg.AutoGen;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace CalligraphyAssistantMain.Code
{
    public class AVPacketSender
    {
        public event EventHandler SendFailed = null;
        private VideoEncoder videoEncoder = null;
        private DateTime lastSendTime = DateTime.MaxValue;
        private Stopwatch stopwatch = new Stopwatch();
        private List<AVPacket> packetList = new List<AVPacket>();
        private ManualResetEvent sendPacketResetEvent = new ManualResetEvent(false);
        private bool isStarted = false;
        private bool isStop = false;
        private long lasttime = -1;
        private object lockObj = new object();

        public AVPacketSender(VideoEncoder videoEncoder)
        {
            this.videoEncoder = videoEncoder;
        }

        public void Start()
        {
            if (isStarted)
            {
                return;
            }
            isStarted = true;
            isStop = false;
            lasttime = -1;
            lastSendTime = DateTime.Now;
            stopwatch.Start();
            SendPackets();
        }

        public void Stop()
        {
            if (!isStarted)
            {
                return;
            }
            isStarted = false;
            isStop = true;
            lasttime = -1;
            stopwatch.Stop();
            lock (lockObj)
            {
                sendPacketResetEvent.Set();
            }
        }

        public long GetTimestamp()
        {
            if (stopwatch == null)//断开重连时Stopwatch可能为null
            {
                if (lasttime > 0)
                {
                    return ++lasttime;
                }
                return 0;
            }
            if (stopwatch.ElapsedMilliseconds == 0)
            {
                return 0;
            }
            if (stopwatch.ElapsedMilliseconds <= lasttime)
            {
                lasttime = stopwatch.ElapsedMilliseconds + 1;
            }
            else
            {
                lasttime = stopwatch.ElapsedMilliseconds;
            }
            return lasttime;
        }

        public bool AddPacket(AVPacket avPacket)
        {
            if (!isStarted)
            {
                return false;
            }
            packetList.Add(avPacket);
            sendPacketResetEvent.Set();
            return true;
        }

        private unsafe void SendPackets()
        {
            ThreadPool.QueueUserWorkItem(p =>
            {
                long lastTimeStamp = 0;
                bool sendResult = false;
                bool reconnect = false;
                while (isStarted)
                {
                    while (packetList.Count > 0)
                    {
                        try
                        {
                            while (packetList.Count > 20)//超过20个未发送的清理
                            {
                                AVPacket temp = packetList[0];
                                ffmpeg.av_packet_unref(&temp);
                                packetList.RemoveAt(0);
                            }
                            AVPacket info = packetList[0];
                            if (lastTimeStamp == info.pts)//时间戳与上次相同 时间戳加1
                            {
                                info.pts = info.pts + 1;
                            }
                            else if (lastTimeStamp == info.pts + 1)//时间戳加1与上次相同 如果是视频再加1 如果是音频丢弃
                            {
                                if (info.stream_index == videoEncoder.VideoStreamIndex)
                                {
                                    info.pts = lastTimeStamp + 1;
                                }
                            }
                            if (info.pts < lastTimeStamp)
                            {
                                ffmpeg.av_packet_unref(&info);
                                packetList.RemoveAt(0);
                                //Common.Debug("AVPacketSender SandPackage Test:" + packetList.Count);
                                continue;
                            }
                            lastTimeStamp = info.pts;
                            lock (lockObj)
                            {
                                try
                                {
                                    sendResult = videoEncoder.WriteFrame(info);
                                }
                                catch (Exception ex)
                                {
                                    Common.Trace("AVPacketSender SandPackage Error2:" + ex.ToString());
                                }
                            }

                            packetList.RemoveAt(0);
                            if (!sendResult)
                            {
                                goto End;
                            }
                        }
                        catch (Exception ex)
                        {
                            Common.Trace("AVPacketSender SandPackage Error3:" + ex.ToString());
                        }
                        lastSendTime = DateTime.Now;
                    }
                    if (!isStarted)
                    {
                        goto End;
                    }
                    //超过10秒没有发送过数据（可能出现过断网）
                    if ((DateTime.Now - lastSendTime).TotalSeconds > 10)
                    {
                        reconnect = true;
                        goto End;
                    }
                    sendPacketResetEvent.WaitOne(100);
                    sendPacketResetEvent.Reset();
                }
            End:
                lock (lockObj)
                {
                    try
                    {
                        isStarted = false;
                        for (int i = 0; i < packetList.Count; i++)
                        {
                            AVPacket packet = packetList[i];
                            ffmpeg.av_packet_unref(&packet);
                        }
                        packetList.Clear();
                        sendPacketResetEvent.Set();
                        sendPacketResetEvent.Dispose();
                        sendPacketResetEvent = null;
                        stopwatch.Stop();
                        stopwatch = null;
                    }
                    catch (Exception ex)
                    {
                        Common.Trace("AVPacketSender SandPackage Error4:" + ex.ToString());
                    }
                }
                if (!isStop)
                {
                    if (SendFailed != null)
                    {
                        SendFailed(this, null);
                    }
                    if (videoEncoder.IsInited || reconnect)//发送失败或强制重新连接 重新初始化AVCodec
                    {
                        Thread.Sleep(500);
                        Common.Debug("AVPacketSender SendPackets 数据发送失败重新初始化:" + videoEncoder.Url);
                        bool result = videoEncoder.ReinitAVCodec();
                        sendPacketResetEvent = new ManualResetEvent(false);
                        stopwatch = new Stopwatch();
                        Start();
                        Common.Debug("AVPacketSender SendPackets 数据发送失败初始化" + (result ? "成功" : "失败") + ":" + videoEncoder.Url);
                        return;
                    }
                }
            });
        }
    }
}
