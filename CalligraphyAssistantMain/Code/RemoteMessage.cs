using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CalligraphyAssistantMain.Code
{
    /// <summary>
    /// 远程消息格式
    /// 0-6     固定格式 nena@yu 
    /// 7       客户端类型 0：教师端 1：学生端
    /// 8       消息编号
    /// 9-12   自定义数据长度
    /// 13-     自定义数据
    /// </summary>
    public class RemoteMessage
    {
        private const int MessagePrefixLength = 7;
        public const int MessageHeaderLength = 13;
        private static readonly byte[] MessagePrefix = Encoding.UTF8.GetBytes("nena@yu");
        public byte MessageId { get; private set; }
        public RemoteClientType Type { get; private set; }
        public byte[] Data { get; private set; }

        public string DataStr
        {
            get
            {
                if (Data == null || Data.Length == 0)
                {
                    return string.Empty;
                }
                return Encoding.UTF8.GetString(Data);
            }
        }

        private RemoteMessage()
        {
        }

        public static RemoteMessage AnalyzeRemoteMessage(byte[] data)
        {
            if (data == null || data.Length < MessageHeaderLength ||
                !Common.CheckArrayEquals<byte>(data, MessagePrefix, MessagePrefixLength))
            {
                return null;
            }
            RemoteMessage remoteMessage = new RemoteMessage()
            {
                Type = (RemoteClientType)data[7],
                MessageId = data[8],
                Data = data.Skip(MessageHeaderLength).ToArray()
            };
            return remoteMessage;
        }

        public static int GetRemoteMessageLength(byte[] data)
        {
            if (data == null || data.Length < MessageHeaderLength ||
                  !Common.CheckArrayEquals<byte>(data, MessagePrefix, MessagePrefixLength))
            {
                return -1;
            }
            return MessageHeaderLength + BitConverter.ToInt32(data, 9);
        }

        public static byte[] CreateRemoteMessage(int messageId, RemoteClientType type, byte[] data)
        {
            int bodyLength = data == null ? 0 : data.Length;
            byte[] headerBuffer = new byte[MessageHeaderLength];
            byte[] messageBuffer = new byte[MessageHeaderLength + bodyLength];

            Array.Copy(MessagePrefix, headerBuffer, MessagePrefixLength);
            headerBuffer[MessagePrefixLength] = (byte)type;
            headerBuffer[MessagePrefixLength + 1] = (byte)messageId;
            Array.Copy(BitConverter.GetBytes(bodyLength), 0, headerBuffer, MessagePrefixLength + 2, 4);
            Array.Copy(headerBuffer, messageBuffer, MessageHeaderLength);
            if (bodyLength > 0)
            {
                Array.Copy(data, 0, messageBuffer, MessageHeaderLength, bodyLength);
            }
            return messageBuffer;
        }

        public static byte[] CreateRemoteMessage(int messageId, RemoteClientType type, string data = "")
        {
            byte[] dataBuffer = null;
            if (!string.IsNullOrEmpty(data))
            {
                dataBuffer = Encoding.UTF8.GetBytes(data);
            }
            return CreateRemoteMessage(messageId, type, dataBuffer);
        }

        //public static byte[] CreateRMServerMessage(RMServerMessage messageId, string data = "")
        //{
        //    return CreateRemoteMessage((int)messageId, RemoteClientType.RMServer, data);
        //}

        //public static byte[] CreateRMServerMessage(RMServerMessage messageId, byte[] data)
        //{
        //    return CreateRemoteMessage((int)messageId, RemoteClientType.RMServer, data);
        //}

        //public static byte[] CreateRMClientMessage(RMClientMessage messageId, string data = "")
        //{
        //    return CreateRemoteMessage((int)messageId, RemoteClientType.RMClient, data);
        //}

        //public static byte[] CreateRMClientMessage(RMClientMessage messageId, byte[] data)
        //{
        //    return CreateRemoteMessage((int)messageId, RemoteClientType.RMClient, data);
        //}

        //public static byte[] CreateRMServerFileMessage(RMServerFileMessage messageId, string data = "")
        //{
        //    return CreateRemoteMessage((int)messageId, RemoteClientType.RMServerFile, data);
        //}

        //public static byte[] CreateRMServerFileMessage(RMServerFileMessage messageId, byte[] data)
        //{
        //    return CreateRemoteMessage((int)messageId, RemoteClientType.RMServerFile, data);
        //}

        //public static byte[] CreateRMClientFileMessage(RMClientFileMessage messageId, string data = "")
        //{
        //    return CreateRemoteMessage((int)messageId, RemoteClientType.RMClientFile, data);
        //}
        //public static byte[] CreateRMClientFileMessage(RMClientFileMessage messageId, byte[] data)
        //{
        //    return CreateRemoteMessage((int)messageId, RemoteClientType.RMClientFile, data);
        //}

        //public static byte[] CreateRMClientToRMServerForwardMessage(string rMServerClientId, RMServerMessage messageId, string data = "")
        //{
        //    byte[] body = RemoteMessage.CreateRMServerMessage(messageId, data);
        //    return CreateRMClientToRMServerForwardMessage(rMServerClientId, body);
        //}

        //public static byte[] CreateRMClientToRMServerForwardMessage(string rMServerClientId, RMServerMessage messageId, byte[] data)
        //{
        //    byte[] body = RemoteMessage.CreateRMServerMessage(messageId, data);
        //    return CreateRMClientToRMServerForwardMessage(rMServerClientId, body);
        //}

        //private static byte[] CreateRMClientToRMServerForwardMessage(string rMServerClientId, byte[] body)
        //{
        //    byte[] header;
        //    if (string.IsNullOrEmpty(rMServerClientId))
        //    {
        //        header = Guid.Empty.ToByteArray();
        //    }
        //    else
        //    {
        //        header = Guid.Parse(rMServerClientId).ToByteArray();
        //    }
        //    byte[] buffer = new byte[header.Length + body.Length];
        //    Array.Copy(header, buffer, header.Length);
        //    Array.Copy(body, 0, buffer, header.Length, body.Length);
        //    return RemoteMessage.CreateRMClientMessage(RMClientMessage.Forward, buffer);
        //}

        //public static byte[] CreateRMServerToRMClientForwardMessage(string rMClientClientId, RMClientMessage messageId, byte[] data)
        //{
        //    byte[] body = RemoteMessage.CreateRMClientMessage(messageId, data);
        //    return CreateRMServerToRMClientForwardMessage(rMClientClientId, body);
        //}

        //private static byte[] CreateRMServerToRMClientForwardMessage(string rMClientClientId, byte[] body)
        //{
        //    byte[] header;
        //    if (string.IsNullOrEmpty(rMClientClientId))
        //    {
        //        header = Guid.Empty.ToByteArray();
        //    }
        //    else
        //    {
        //        header = Guid.Parse(rMClientClientId).ToByteArray();
        //    }
        //    byte[] buffer = new byte[header.Length + body.Length];
        //    Array.Copy(header, buffer, header.Length);
        //    Array.Copy(body, 0, buffer, header.Length, body.Length);
        //    return RemoteMessage.CreateRMServerMessage(RMServerMessage.Forward, buffer);
        //}

        //private static byte[] CreateRMClientFileToRMServerFileForwardMessage(string rMServerClientId, byte[] body)
        //{
        //    byte[] header = Guid.Parse(rMServerClientId).ToByteArray();
        //    byte[] buffer = new byte[header.Length + body.Length];
        //    Array.Copy(header, buffer, header.Length);
        //    Array.Copy(body, 0, buffer, header.Length, body.Length);
        //    return RemoteMessage.CreateRMClientFileMessage(RMClientFileMessage.Forward, buffer);
        //}

        ///// <summary>
        ///// 使用RMServer的ClientId
        ///// </summary>
        ///// <param name="rMServerClientId"></param>
        ///// <param name="body"></param>
        ///// <returns></returns>
        //public static byte[] CreateRMClientFileToRMServerFileForwardMessage(string rMServerClientId, RMServerFileMessage messageId, byte[] data)
        //{
        //    byte[] body = RemoteMessage.CreateRMServerFileMessage(messageId, data);
        //    return CreateRMClientFileToRMServerFileForwardMessage(rMServerClientId, body);
        //}

        //public static byte[] CreateRMClientFileToRMServerFileForwardMessage(string rMServerClientId, RMServerFileMessage messageId, string data = "")
        //{
        //    byte[] body = RemoteMessage.CreateRMServerFileMessage(messageId, data);
        //    return CreateRMClientFileToRMServerFileForwardMessage(rMServerClientId, body);
        //}

        //private static byte[] CreateRMServerFileToRMClientFileForwardMessage(string rMClientClientId, byte[] body)
        //{
        //    byte[] header = Guid.Parse(rMClientClientId).ToByteArray();
        //    byte[] buffer = new byte[header.Length + body.Length];
        //    Array.Copy(header, buffer, header.Length);
        //    Array.Copy(body, 0, buffer, header.Length, body.Length);
        //    return RemoteMessage.CreateRMServerFileMessage(RMServerFileMessage.Forward, buffer);
        //}

        ///// <summary>
        ///// 使用RMClient的ClientId
        ///// </summary>
        ///// <param name="rMClientClientId"></param>
        ///// <param name="body"></param>
        ///// <returns></returns>
        //public static byte[] CreateRMServerFileToRMClientFileForwardMessage(string rMClientClientId, RMClientFileMessage messageId, byte[] data)
        //{
        //    byte[] body = RemoteMessage.CreateRMClientFileMessage(messageId, data);
        //    return CreateRMServerFileToRMClientFileForwardMessage(rMClientClientId, body);
        //}

        //public static byte[] CreateRMServerFileToRMClientFileForwardMessage(string rMClientClientId, RMClientFileMessage messageId, string data = "")
        //{
        //    byte[] body = RemoteMessage.CreateRMClientFileMessage(messageId, data);
        //    return CreateRMServerFileToRMClientFileForwardMessage(rMClientClientId, body);
        //}
    }
}