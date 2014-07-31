using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Net.Security;
using System.Security.Cryptography;
using System.Net;

namespace YnoteEx
{
    public static class WebTool
    {
        public static string Md5(string instr)
        {



            string pwd = "";
            MD5 md5 = MD5.Create();
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(instr));
            for (int i = 0; i < s.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 

                pwd = pwd + s[i].ToString("X");

            }
            return pwd;

        }

        public static string To64String(long a)
        {
            string f = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890_";
            int b = 0;
            string c = "";
            while (a > 0)
            {
                b = (int)(a % 63);
                c = f[b].ToString() + c;
                a = Int64.Parse((a / 63).ToString(), System.Globalization.NumberStyles.Integer);
                //a = Math.Truncate(a / 63);
                //a = parseInt(a / 63, 10);
            }
            return c;

        }

        public static string GetTimestamp()
        {
            return DateTime.UtcNow.Ticks.ToString();
        }


        private static MemoryStream GetMemoryStream(Stream streamResponse)
        {
            MemoryStream _stream = new MemoryStream();
            int Length = 256;
            Byte[] buffer = new Byte[Length];
            int bytesRead = streamResponse.Read(buffer, 0, Length);
            while (bytesRead > 0)
            {
                _stream.Write(buffer, 0, bytesRead);
                bytesRead = streamResponse.Read(buffer, 0, Length);
            }
            return _stream;
        }
        public static string GetResponseStr(HttpWebResponse response)
        {
            MemoryStream _stream = new MemoryStream();
            //GZIIP处理
            if (response.ContentEncoding != null && response.ContentEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase))
            {
                //开始读取流并设置编码方式
                //new GZipStream(response.GetResponseStream(), CompressionMode.Decompress).CopyTo(_stream, 10240);
                //.net4.0以下写法
                _stream = GetMemoryStream(new GZipStream(response.GetResponseStream(), CompressionMode.Decompress));

            }
            else
            {
                //开始读取流并设置编码方式
                //response.GetResponseStream().CopyTo(_stream, 10240);
                //.net4.0以下写法
                _stream = GetMemoryStream(response.GetResponseStream());
            }
            byte[] result = _stream.GetBuffer();
            System.Text.UTF8Encoding converter = new System.Text.UTF8Encoding();
            return converter.GetString(result);
        }
        public static long GetCurrentTimestamp()
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));

            DateTime nowTime = DateTime.UtcNow;

            long unixTime = (long)Math.Round((nowTime - startTime).TotalMilliseconds, MidpointRounding.AwayFromZero);
            return unixTime;
        }
    }
}
