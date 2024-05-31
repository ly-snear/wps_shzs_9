using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace CalligraphyAssistantMain.Code
{
    public class HttpUtility
    {

        private static byte[] UrlEncodeBytesToBytesInternal(byte[] bytes, int offset, int count, bool alwaysCreateReturnValue)
        {
            int num = 0;
            int num2 = 0;
            for (int i = 0; i < count; i++)
            {
                char ch = (char)bytes[offset + i];
                if (ch == ' ')
                {
                    num++;
                }
                else if (!IsSafe(ch))
                {
                    num2++;
                }
            }
            if ((!alwaysCreateReturnValue && (num == 0)) && (num2 == 0))
            {
                return bytes;
            }
            byte[] buffer = new byte[count + (num2 * 2)];
            int num4 = 0;
            for (int j = 0; j < count; j++)
            {
                byte num6 = bytes[offset + j];
                char ch2 = (char)num6;
                if (IsSafe(ch2))
                {
                    buffer[num4++] = num6;
                }
                else if (ch2 == ' ')
                {
                    buffer[num4++] = 0x2b;
                }
                else
                {
                    buffer[num4++] = 0x25;
                    buffer[num4++] = (byte)IntToHex((num6 >> 4) & 15);
                    buffer[num4++] = (byte)IntToHex(num6 & 15);
                }
            }
            return buffer;
        }

        private static char IntToHex(int n)
        {
            if (n <= 9)
            {
                return (char)(n + 0x30);
            }
            return (char)((n - 10) + 0x61);
        }

        private static bool IsSafe(char ch)
        {
            if ((((ch >= 'a') && (ch <= 'z')) || ((ch >= 'A') && (ch <= 'Z'))) || ((ch >= '0') && (ch <= '9')))
            {
                return true;
            }
            switch (ch)
            {
                case '\'':
                case '(':
                case ')':
                case '*':
                case '-':
                case '.':
                case '_':
                case '!':
                    return true;
            }
            return false;
        }

        private static byte[] UrlEncodeToBytes(string str, Encoding e)
        {
            if (str == null)
            {
                return null;
            }
            byte[] bytes = e.GetBytes(str);
            return UrlEncodeBytesToBytesInternal(bytes, 0, bytes.Length, false);
        }

        public static string UrlEncode(string str, Encoding e)
        {
            if (str == null)
            {
                return null;
            }
            return Encoding.ASCII.GetString(UrlEncodeToBytes(str, e));
        }

        private static byte[] GetUploadBytes(NameValueCollection data, Encoding encoding)
        {
            string str = string.Empty;
            StringBuilder builder = new StringBuilder();
            foreach (string key in data.AllKeys)
            {
                builder.Append(str);
                builder.Append(key);
                builder.Append("=");
                builder.Append(data[key]);
                str = "&";
            }
            return encoding.GetBytes(builder.ToString());
        }

        public static long GetServerFileSize(string url)
        {
            WebRequest request = null;
            WebResponse response = null;
            long fileSize = 0;
            try
            {
                request = WebRequest.Create(url);
                response = request.GetResponse();
                fileSize = response.ContentLength;
                response.Close();
                response = null;
                return fileSize;
            }
            catch (Exception ex)
            {
                Common.Trace(ex.ToString());
                return fileSize;
            }
            finally
            {
                if (request != null)
                {
                    request.Abort();
                }
                if (response != null)
                {
                    response.Close();
                }
            }
        }
        /// <summary>
        /// 请求上传图片到阿里云
        /// </summary>
        /// <param name="url">上传地址</param>
        /// <param name="filepath">本地文件路径</param>
        /// <param name="dic">上传的数据信息</param>
        /// <returns></returns>
        public static bool UploadFilesToRemoteUrl(string url, string filepath, Dictionary<string, string> dic)
        {
            try
            {
                string boundary = DateTime.Now.Ticks.ToString("x");

                byte[] boundarybytes = System.Text.Encoding.UTF8.GetBytes("--" + boundary + "\r\n");

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                request.Method = "POST";

                request.ContentType = "multipart/form-data; boundary=" + boundary;

                Stream rs = request.GetRequestStream();

                var endBoundaryBytes = System.Text.Encoding.UTF8.GetBytes("--" + boundary + "--\r\n");

                string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n" + "\r\n" + "{1}" + "\r\n";
                if (dic != null)
                {
                    foreach (string key in dic.Keys)
                    {
                        rs.Write(boundarybytes, 0, boundarybytes.Length);

                        string formitem = string.Format(formdataTemplate, key, dic[key]);

                        byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);

                        rs.Write(formitembytes, 0, formitembytes.Length);
                    }
                }

                string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n\r\n";
                {
                    rs.Write(boundarybytes, 0, boundarybytes.Length);

                    var header = string.Format(headerTemplate, "file", Path.GetFileName(filepath));

                    var headerbytes = System.Text.Encoding.UTF8.GetBytes(header);

                    rs.Write(headerbytes, 0, headerbytes.Length);

                    using (var fileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read))
                    {
                        var buffer = new byte[1024];

                        var bytesRead = 0;

                        while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            rs.Write(buffer, 0, bytesRead);
                        }
                    }
                    var cr = Encoding.UTF8.GetBytes("\r\n");

                    rs.Write(cr, 0, cr.Length);
                }

                rs.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);

                var response = request.GetResponse() as HttpWebResponse;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return true;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("文件上传失败！");
            }
            return false;
        }

        public static string DownloadString(string url, Encoding downloadEncoding, NameValueCollection headers = null, string contentType = "application/x-www-form-urlencoded")
        {
            WebRequest request = null;
            WebResponse response = null;
            Stream stream = null;
            StreamReader reader = null;
            try
            {
                request = WebRequest.Create(url);
                request.ContentType = contentType;
                if (headers != null)
                {
                    foreach (string key in headers.AllKeys)
                    {
                        request.Headers.Add(key, headers[key]);
                    }
                }
                response = request.GetResponse();
                stream = response.GetResponseStream();
                reader = new StreamReader(stream, downloadEncoding);
                return reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                Common.Trace($"HttpUtility DownloadString Url:{url}\r\nError:" + ex.Message);
                return string.Empty;
            }
            finally
            {
                if (request != null)
                {
                    request.Abort();
                }
                if (response != null)
                {
                    response.Close();
                }
                if (stream != null)
                {
                    stream.Dispose();
                }
                if (reader != null)
                {
                    reader.Dispose();
                }
            }
        }

        public static bool DownloadFile(string url, string saveFile, CancellationTokenSource cancelToken = null)
        {

            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream stream = null;
            FileStream fileStream = null;
            int readCount;
            byte[] buffer = new byte[4096 * 100];
            try
            {
                fileStream = new FileStream(saveFile, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                request = HttpWebRequest.Create(url) as HttpWebRequest;
                if (fileStream.Length > 0)
                {
                    request.AddRange((int)fileStream.Length);
                    fileStream.Position = fileStream.Length;
                }
                response = request.GetResponse() as HttpWebResponse;
                stream = response.GetResponseStream();
                while ((readCount = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    fileStream.Write(buffer, 0, readCount);
                    if (cancelToken != null & cancelToken.IsCancellationRequested)
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Common.Trace(ex.ToString());
                return false;
            }
            finally
            {
                if (request != null)
                {
                    request.Abort();
                }
                if (response != null)
                {
                    response.Close();
                }
                if (stream != null)
                {
                    stream.Dispose();
                }
                if (fileStream != null)
                {
                    fileStream.Dispose();
                }
            }
        }

        public static string UploadValues(string url, NameValueCollection data, Encoding uploadEncoding, Encoding downloadEncoding, NameValueCollection headers = null, string contentType = "application/x-www-form-urlencoded")
        {
            WebRequest request = null;
            WebResponse response = null;
            Stream stream = null;
            StreamReader reader = null;
            try
            {
                request = WebRequest.Create(url);
                if (headers != null)
                {
                    foreach (string key in headers.AllKeys)
                    {
                        request.Headers.Add(key, headers[key]);
                    }
                }
                request.ContentType = contentType;
                byte[] dataArr = GetUploadBytes(data, uploadEncoding);
                request.ContentLength = dataArr.Length;
                request.Method = "POST";
                stream = request.GetRequestStream();
                stream.Write(dataArr, 0, dataArr.Length);
                response = request.GetResponse();
                stream = response.GetResponseStream();
                reader = new StreamReader(stream, downloadEncoding);
                return reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                Common.Trace($"HttpUtility UploadValues Url:{url}\r\nError:" + ex.Message);
                return string.Empty;
            }
            finally
            {
                if (request != null)
                {
                    request.Abort();
                }
                if (response != null)
                {
                    response.Close();
                }
                if (stream != null)
                {
                    stream.Dispose();
                }
                if (reader != null)
                {
                    reader.Dispose();
                }
            }
        }

        public static string UploadValuesJson(string url, object jsonObj, Encoding uploadEncoding, Encoding downloadEncoding, NameValueCollection headers = null)
        {
            WebRequest request = null;
            WebResponse response = null;
            Stream stream = null;
            StreamReader reader = null;
            try
            {
                request = WebRequest.Create(url);
                if (headers != null)
                {
                    foreach (string key in headers.AllKeys)
                    {
                        request.Headers.Add(key, headers[key]);
                    }
                }
                request.ContentType = "application/json";
                string jsonStr = JsonConvert.SerializeObject(jsonObj);
                byte[] dataArr = uploadEncoding.GetBytes(jsonStr);
                //byte[] dataArr = GetUploadBytes(data, uploadEncoding);
                request.ContentLength = dataArr.Length;
                request.Method = "POST";
                stream = request.GetRequestStream();
                stream.Write(dataArr, 0, dataArr.Length);
                response = request.GetResponse();
                stream = response.GetResponseStream();
                reader = new StreamReader(stream, downloadEncoding);
                return reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                Common.Trace($"HttpUtility UploadValues Url:{url}\r\nError:" + ex.Message);
                return string.Empty;
            }
            finally
            {
                if (request != null)
                {
                    request.Abort();
                }
                if (response != null)
                {
                    response.Close();
                }
                if (stream != null)
                {
                    stream.Dispose();
                }
                if (reader != null)
                {
                    reader.Dispose();
                }
            }
        }

        public static string UploadValuesJson(string url, string jsonStr, Encoding uploadEncoding, Encoding downloadEncoding)
        {
            WebRequest request = null;
            WebResponse response = null;
            Stream stream = null;
            StreamReader reader = null;
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = ValidateServerCertificate;
                request = WebRequest.Create(url);
                request.ContentType = "application/json";
                //byte[] dataArr = GetUploadBytes(data, uploadEncoding); 
                byte[] dataArr = Encoding.UTF8.GetBytes(jsonStr);
                request.ContentLength = dataArr.Length;
                request.Method = "POST";
                stream = request.GetRequestStream();
                stream.Write(dataArr, 0, dataArr.Length);
                response = request.GetResponse();
                stream = response.GetResponseStream();
                reader = new StreamReader(stream, downloadEncoding);
                return reader.ReadToEnd();
            }
            catch (Exception)
            {
                //MessageBox.Show(ex.Message);
                return string.Empty;
            }
            finally
            {
                if (request != null)
                {
                    request.Abort();
                }
                if (response != null)
                {
                    response.Close();
                }
                if (stream != null)
                {
                    stream.Dispose();
                }
                if (reader != null)
                {
                    reader.Dispose();
                }
            }
        }

        private static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        public static string UploadFiles(string[] sendfiles, string[] inputNames, NameValueCollection data, string url, Encoding encoding, int timeout = -1)
        {
            long filesSplittersLength;
            long dataSplittersLength;
            long datasLength;
            long filesLength = GetFilesLength(sendfiles);
            int bufferLength = 4096;
            byte[] buffer = new byte[bufferLength];
            byte[] breakArr = Encoding.ASCII.GetBytes("\r\n");
            string strBoundary = "----------" + DateTime.Now.Ticks.ToString("x");
            //ASP.NET下name和filename的值需要加双引号，如果Java下不需要去掉即可。
            string formatSplitter =
                "--{0}\r\n" +
                "Content-Disposition:form-data;name=\"{1}\";filename=\"{2}\"\r\n" +
                "Content-Type:application/octet-stream\r\n\r\n";
            byte[][] fileSplitterArr = GetSplitterByteArr(sendfiles, inputNames, strBoundary, formatSplitter, out filesSplittersLength, encoding);
            byte[][] dataSplitterArr = GetDataSplitterByteArr(data, strBoundary, out dataSplittersLength, encoding);
            byte[][] dataArr = GetDataByteArr(data, out datasLength, encoding);
            byte[] endboudy = Encoding.ASCII.GetBytes("--" + strBoundary + "--\r\n");

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
            request.Method = "POST";
            request.KeepAlive = true;
            //对发送的数据不使用缓存
            request.AllowWriteStreamBuffering = false;
            request.ContentType = "multipart/form-data;boundary=" + strBoundary;
            request.ContentLength = datasLength + dataSplittersLength + filesLength + filesSplittersLength + endboudy.Length + data.Count * breakArr.Length + sendfiles.Length * breakArr.Length;
            if (timeout > 0)
            {
                request.Timeout = timeout;
            }
            Stream postStream = request.GetRequestStream();
            for (int i = 0; i < data.Count; i++)
            {
                postStream.Write(dataSplitterArr[i], 0, dataSplitterArr[i].Length);
                postStream.Write(dataArr[i], 0, dataArr[i].Length);
                postStream.Write(breakArr, 0, breakArr.Length);
            }
            for (int i = 0; i < sendfiles.Length; i++)
            {
                postStream.Write(fileSplitterArr[i], 0, fileSplitterArr[i].Length);
                using (FileStream stream = new FileStream(sendfiles[i], FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    while (true)
                    {
                        int size = stream.Read(buffer, 0, bufferLength);
                        if (size > 0)
                        {
                            postStream.Write(buffer, 0, size);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                postStream.Write(breakArr, 0, breakArr.Length);
                //Application.DoEvents(); //处理当前消息队列中所有的windows消息
            }
            //添加尾部时间戳
            postStream.Write(endboudy, 0, endboudy.Length);
            postStream.Close();
            WebResponse response = request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            using (StreamReader reader = new StreamReader(responseStream))
            {
                string resultxmlstr = reader.ReadToEnd();
                response.Close();
                return resultxmlstr;
            }
        }

        /// <summary>
        /// 上传多个文件
        /// </summary>
        /// <param name="files">文件路径</param>
        /// <param name="inputNames">input的标签名称</param>
        /// <param name="url">上传的路径</param>
        /// <returns></returns>
        public static string UploadFiles(string[] sendfiles, string[] inputNames, string url)
        {
            long splittersLength;
            long filesLength = GetFilesLength(sendfiles);
            int bufferLength = 4096;
            byte[] buffer = new byte[bufferLength];
            byte[] breakArr = Encoding.ASCII.GetBytes("\r\n");
            string strBoundary = "----------" + DateTime.Now.Ticks.ToString("x");
            //ASP.NET下name和filename的值需要加双引号，如果Java下不需要去掉即可。
            string formatSplitter =
                "--{0}\r\n" +
                "Content-Disposition:form-data;name=\"{1}\";filename=\"{2}\"\r\n" +
                "Content-Type:application/octet-stream\r\n\r\n";
            byte[][] splitterArr = GetSplitterByteArr(sendfiles, inputNames, strBoundary, formatSplitter, out splittersLength, Encoding.UTF8);
            byte[] endboudy = Encoding.ASCII.GetBytes("--" + strBoundary + "--\r\n");

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
            request.Method = "POST";
            request.KeepAlive = true;
            //对发送的数据不使用缓存
            request.AllowWriteStreamBuffering = false;
            request.ContentType = "multipart/form-data;boundary=" + strBoundary;
            request.ContentLength = filesLength + splittersLength + endboudy.Length + sendfiles.Length * breakArr.Length;

            Stream postStream = request.GetRequestStream();
            for (int i = 0; i < sendfiles.Length; i++)
            {
                postStream.Write(splitterArr[i], 0, splitterArr[i].Length);
                using (FileStream stream = new FileStream(sendfiles[i], FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    while (true)
                    {
                        int size = stream.Read(buffer, 0, bufferLength);
                        if (size > 0)
                        {
                            postStream.Write(buffer, 0, size);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                postStream.Write(breakArr, 0, breakArr.Length);
                //Application.DoEvents(); //处理当前消息队列中所有的windows消息
            }
            //添加尾部时间戳
            postStream.Write(endboudy, 0, endboudy.Length);
            postStream.Close();
            WebResponse response = request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            using (StreamReader reader = new StreamReader(responseStream))
            {
                string resultxmlstr = reader.ReadToEnd();
                response.Close();
                return resultxmlstr;
            }
        }

        private static byte[][] GetSplitterByteArr(string[] files, string[] inputNames, string strBoundary, string formatSplitter, out long splittersLength, Encoding encoding)
        {
            splittersLength = 0;
            byte[][] byteArr = new byte[files.Length][];
            for (int i = 0; i < files.Length; i++)
            {
                string tempStr = string.Format(formatSplitter, strBoundary, inputNames[i], Path.GetFileName(files[i]));
                byteArr[i] = encoding.GetBytes(tempStr);
                splittersLength += byteArr[i].Length;
            }
            return byteArr;
        }

        private static byte[][] GetDataSplitterByteArr(NameValueCollection data, string strBoundary, out long splittersLength, Encoding encoding)
        {
            splittersLength = 0;
            byte[][] byteArr = new byte[data.Count][];
            for (int i = 0; i < data.Count; i++)
            {
                string tempStr = "--" + strBoundary + "\r\n" +
                    "Content-Disposition: form-data; name=\"" + data.Keys[i] + "\"\r\n\r\n";
                byteArr[i] = encoding.GetBytes(tempStr);
                splittersLength += byteArr[i].Length;
            }
            return byteArr;
        }

        private static byte[][] GetDataByteArr(NameValueCollection data, out long datasLength, Encoding encoding)
        {
            datasLength = 0;
            byte[][] byteArr = new byte[data.Count][];
            for (int i = 0; i < data.Count; i++)
            {
                byteArr[i] = encoding.GetBytes(data[i]);
                datasLength += byteArr[i].Length;
            }
            return byteArr;
        }

        /// <summary>
        /// 得到文件总长度
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        private static long GetFilesLength(string[] files)
        {
            long length = 0;
            foreach (string file in files)
            {
                length += new FileInfo(file).Length;
            }
            return length;
        }
    }
}
