using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CalligraphyAssistantMain.Code
{
    public class SocketClient : IDisposable
    {
        protected Socket socket;
        private SocketError socketError;
        private byte[] buffer = new byte[8192 * 10];
        private object lockObj = new object();
        private List<byte> bufferList = new List<byte>();
        private List<byte> sendList = new List<byte>();
        private ManualResetEventSlim eventSlim = null;
        public bool IsDisposed { get; private set; } = false;
        public bool IsConnected
        {
            get
            {
                if (socket == null)
                {
                    return false;
                }
                return socket.Connected;
            }
        }
        public DateTime LastReceiveTime { get; set; } = DateTime.MaxValue;
        public event EventHandler OnDisconnected = null;
        public event EventHandler OnError = null;
        public SocketClient(Socket socket)
        {
            if (socket == null)
            {
                return;
            }
            this.socket = socket;
            try
            {
                socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, out socketError, Receive, null);
                StartSendThread();
            }
            catch (Exception ex)
            {
                if (socketError == SocketError.SocketError)
                {
                    RaiseError();
                }
                RaiseDisconnected();
                Common.Trace("SocketClient SocketClient Error:" + ex.ToString());
                Dispose();
            }
        }

        public void Send(byte[] data)
        {
            if (IsDisposed || sendList == null || eventSlim == null)
            {
                return;
            }
            try
            {
                lock (lockObj)
                {
                    sendList.AddRange(data);
                }
                eventSlim.Set();
            }
            catch (Exception ex)
            {
                Common.Trace("SocketClient Send Error:" + ex.ToString());
            }
        }

        public void Dispose()
        {
            if (!IsDisposed)
            {
                IsDisposed = true;
                try
                {
                    lock (lockObj)
                    {
                        if (socket != null)
                        {
                            socket.Close();
                        }
                        if (bufferList != null)
                        {
                            bufferList.Clear();
                        }
                        if (sendList != null)
                        {
                            sendList.Clear();
                        }
                        socket = null;
                        buffer = null;
                        bufferList = null;
                        sendList = null;
                    }
                }
                catch (Exception ex)
                {
                    Common.Trace("SocketClient Dispose Error:" + ex.ToString());
                }
            }
        }

        private void StartSendThread()
        {
            Task.Run(() =>
            {
                eventSlim = new ManualResetEventSlim(false);
                eventSlim.Reset();
                while (!IsDisposed)
                {
                    while (sendList != null && sendList.Count > 0)
                    {
                        try
                        {
                            byte[] data = null;
                            lock (lockObj)
                            {
                                if (sendList == null)
                                {
                                    continue;
                                }
                                data = sendList.Take(Math.Min(8192 * 10, sendList.Count)).ToArray();
                            }
                            int start = 0;
                            int last = data.Length;
                            while ((start = socket.Send(data, start, last, SocketFlags.None)) > 0)
                            {
                                last += start;
                                if (start >= data.Length)
                                {
                                    break;
                                }
                                else
                                {
                                    //Console.WriteLine("123");
                                }
                            }
                            if (sendList != null)
                            {
                                sendList.RemoveRange(0, data.Length);
                            }
                        }
                        catch (Exception ex)
                        {
                            Common.Trace("SocketClient StartSendThread Error:" + ex.ToString());
                            Close();
                        }
                    }
                    eventSlim.Wait(500);
                    eventSlim.Reset();
                }
                eventSlim.Dispose();
                eventSlim = null;
            });
        }

        private void RaiseError()
        {
            if (OnError != null)
            {
                OnError(this, null);
            }
        }

        private void RaiseDisconnected()
        {
            if (OnDisconnected != null)
            {
                OnDisconnected(this, null);
            }
        }

        private void Close()
        {
            RaiseDisconnected();
            Dispose();
        }

        private void Process()
        {
            RemoteMessage remoteMessage = null;
            while (true)
            {
                try
                {
                    remoteMessage = null;
                    if (bufferList.Count >= RemoteMessage.MessageHeaderLength)
                    {
                        int count = RemoteMessage.GetRemoteMessageLength(bufferList.Take(RemoteMessage.MessageHeaderLength).ToArray());
                        if (count == -1)
                        {
                            Close();
                            return;
                        }
                        if (count <= bufferList.Count)
                        {
                            remoteMessage = RemoteMessage.AnalyzeRemoteMessage(bufferList.Take(count).ToArray());
                            bufferList.RemoveRange(0, count);
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        return;
                    }
                    if (remoteMessage != null)
                    {
                        ProcessMessage(remoteMessage);
                    }
                    else
                    {
                        Close();
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Common.Trace("SocketClient Process Error:" + ex.ToString());
                    Close();
                    return;
                } 
            }
        }

        public virtual void ProcessMessage(RemoteMessage remoteMessage)
        {

        }

        private void Receive(IAsyncResult asyncResult)
        {
            int count = 0;
            try
            {
                if (socket == null)
                {
                    return;
                }
                count = socket.EndReceive(asyncResult);
            }
            catch (Exception ex)
            {
                Common.Trace("SocketClient Receive Error1:" + ex.ToString());
                RaiseDisconnected();
                Dispose();
                return;
            }
            try
            {
                if (count > 0)
                {
                    LastReceiveTime = DateTime.Now;
                    bufferList.AddRange(buffer.Take(count));
                    Process();
                    socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, out socketError, Receive, null);
                }
                else
                {
                    RaiseDisconnected();
                    Dispose();
                }
            }
            catch (Exception ex)
            {
                Common.Trace("SocketClient Receive Error2:" + ex.ToString());
                RaiseDisconnected();
                Dispose();
                return;
            } 
        }
    }
}
