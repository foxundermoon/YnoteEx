using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Security.Cryptography;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Text.RegularExpressions;


namespace YnoteEx
{
    public class MyWebClient
    {
        YnoteUser user;
        CookieContainer cookieContainer;
        Poster poster;
        Uri YnoteUri = new Uri("http://note.youdao.com");
        public ILoger Log { set; get; }

        public MyWebClient()
            : this(new YnoteUser("fox@vvfox.com", "232381204"))
        {
        }
        public MyWebClient(YnoteUser user)
        {
            this.user = user;
            cookieContainer = new CookieContainer();
            poster = new Poster();
            poster.YnoteCookieContainer = cookieContainer;
        }
        public bool Login()
        {
            //step  1

            //List<HttpWebRequest> requests = new List<HttpWebRequest>();
            HttpWebRequest request1 = WebRequest.CreateHttp(new Uri(LoginUrls.url1));
            //request1.Headers["Accept"] = "image/png, image/svg+xml, image/*;q=0.8, */*;q=0.5";
            request1 = initBaseRequest(request1);
            //cookieContainer.Add(new Cookie("YNOTE_USER","1"));
            cookieContainer.SetCookies(YnoteUri, "YNOTE_USER=1");
            //Log.i("support cookie:"+request1.SupportsCookieContainer);
            request1.CookieContainer = cookieContainer;

            #region
            //request1.Accept = "image/png, image/svg+xml, image/*;q=0.8, */*;q=0.5";
            //request1.Referer = "http://note.youdao.com/";
            //request1.Headers["Referer"] =
            //request1.Headers["Accept-Language"] = "zh-CN";
            //request1.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; Trident/7.0; rv:11.0) like Gecko";
            //request1.Headers["User-Agent"] = "Mozilla/5.0 (Windows NT 6.3; WOW64; Trident/7.0; rv:11.0) like Gecko";
            //request1.Headers["Accept-Encoding"] = "gzip, deflate";
            //request1.Host = "note.youdao.com";
            //request1.Headers["Host"] = "note.youdao.com";
            //request1.Headers["DNT"] = "1";
            //request1.KeepAlive = true;
            //request1.Headers["Connection"] = "Keep-Alive";
            #endregion


            //  step 2
            HttpWebResponse response1 = request1.GetResponse() as HttpWebResponse;
            cookieContainer.Add(response1.Cookies);
            foreach (Cookie cookie in response1.Cookies)
            {
                Log.i("cookie1:" + cookie.ToString());
            }
            //HttpWebRequest request3 = WebRequest.Create() 
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            Log.i("login-url:" + wrapLoginUrl());
            HttpWebRequest request2 = WebRequest.Create(new Uri(wrapLoginUrl())) as HttpWebRequest;
            request2 = initBaseRequest(request2);
            request2.Host = "reg.163.com";
            request2.CookieContainer = cookieContainer;
            HttpWebResponse response2 = request2.GetResponse() as HttpWebResponse;
            cookieContainer.Add(response2.Cookies);
            Log.i("char-set:" + response2.CharacterSet);
            Log.i("encoding:" + response2.ContentEncoding);
            Log.i("content-length:" + response2.ContentLength);
            string content = WebTool.GetResponseStr(response2);
            Log.i("content:" + content);

            //foreach(Cookie cookie in response2.Cookies)
            //{
            //    Log.i(cookie.ToString());
            //    cookieContainer.Add(cookie);
            //    //cookieContainer.SetCookies(YnoteUri, cookie);
            //}
            //cookieContainer.Add(YnoteUri, response2.Cookies);
            Log.i("\n\r\n\rresponse2");
            foreach (var cookie in response2.Cookies)
            {
                Log.i("cookie2:" + cookie.ToString());
            }


            //step  3
            Regex myregex = new Regex("window\\.location\\.replace\\(\"(?<url>[^\"]*?)\"\\);", RegexOptions.None);
            Match myMatch = myregex.Match(content);

            string loaction = myregex.Match(content).Result("${url}");
            Log.i("location-url:" + loaction);


            HttpWebRequest request3 = WebRequest.CreateHttp(new Uri(loaction));
            request3.CookieContainer = cookieContainer;
            request3 = initBaseRequest(request3);
            request3.Host = "passport.126.com";
            HttpWebResponse response3 = request3.GetResponse() as HttpWebResponse;
            cookieContainer.Add(response3.Cookies);
            string content3 = WebTool.GetResponseStr(response3);

            Regex myRegex2 = new Regex("<link rel=\"stylesheet\" href=\"(?<url2>[^\"]*?)\" type=", RegexOptions.None);
            Match myMatch2 = myRegex2.Match(content3);
            string location2 = myMatch2.Result("${url2}");
            myMatch2 = myMatch2.NextMatch();
            string location2_1 = myMatch2.Result("${url2}");

            doGet(location2);
            doGet(location2_1);



            return true;







        }
        public bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        { // Always accept
            return true; //总是接受
        }



        public void DoPostTest()
        {
            string postData = "p=%2F" + WebTool.To64String(WebTool.GetCurrentTimestamp()) + "&tl=222&r=true&ng=&v=-1";
            //postData = "p=%2FWfeaffm&tl=111111_111111111111&r=true&ng=&v=-1";
            //p=%2FWffnrdz&tl=1111111111111&r=true&ng=&v=-1
            byte[] data = UTF8Encoding.UTF8.GetBytes(postData);
            Log.i(DoPost("http://note.youdao.com/yws/mapi/filemeta?method=update&keyfrom=web", data));
        }
        public string doGet(string url)
        {
            HttpWebRequest request = WebRequest.Create(new Uri(url)) as HttpWebRequest;
            request.CookieContainer = cookieContainer;
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            cookieContainer.Add(response.Cookies);
            //Log.i(response.Cookies);
            Stream sm = response.GetResponseStream();
            StreamReader sr = new StreamReader(sm);
            return sr.ReadToEnd();
        }

        public string DoPost(string url, byte[] data)
        {
            HttpWebRequest postRequest = WebRequest.Create(LoginUrls.testUrl1) as HttpWebRequest;
            postRequest = initBaseRequest(postRequest);
            postRequest.CookieContainer = cookieContainer;
            postRequest.Accept = "application/json, text/javascript, */*; q=0.01";
            postRequest.Method = "POST";
            postRequest.Headers["Origin"] = "http://note.youdao.com";
            postRequest.ContentType = "application/x-www-form-urlencoded";
            postRequest.ContentLength = data.Length;
            postRequest.Headers["X-Requested-With"] = "XMLHttpRequest";
            postRequest.Referer = "http://note.youdao.com/web/list?version=529952&notebook=%2F&sortMode=0&note=%2FWfbVp_K%2Fweb" + WebTool.GetCurrentTimestamp();
            postRequest.ContentLength = data.Length;
            //StreamWriter sw = new StreamWriter(postRequest.GetRequestStream());
            //sw.Write(data);
            //sw.WriteAsync(data);
            //async sw.FlushAsync();
            Stream sm = postRequest.GetRequestStream();
            sm.Write(data, 0, data.Length);
            HttpWebResponse response;
            try
            {
                response = postRequest.GetResponse() as HttpWebResponse;
                cookieContainer.Add(response.Cookies);
            }
            catch (WebException we)
            {
                response = we.Response as HttpWebResponse;
            }
            return GetStringContentFromResponse(response);

        }



        public string DoPost(string url, string data)
        {
            return DoPost(url, UTF8Encoding.UTF8.GetBytes(data));
        }
        private string GetStringContentFromResponse(HttpWebResponse response)
        {
            Log.i("encoding:" + response.ContentEncoding);
            Log.i("content-type:" + response.ContentType);
            Log.i("content-length:" + response.ContentLength);
            StreamReader sr = new StreamReader(response.GetResponseStream());
            string result = sr.ReadToEnd();
            Log.i("content:\n" + result);

            return result;

        }



        HttpWebRequest initBaseRequest(HttpWebRequest r)
        {
            r.Accept = "image/png, image/svg+xml, image/*;q=0.8, */*;q=0.5";
            r.Referer = "http://note.youdao.com/";
            r.Headers["Accept-Language"] = "zh-CN";
            r.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; Trident/7.0; rv:11.0) like Gecko";
            r.Headers["Accept-Encoding"] = "gzip, deflate";
            r.Host = "note.youdao.com";
            r.Headers["DNT"] = "1";
            r.KeepAlive = true;


            return r;
        }

        string wrapLoginUrl()
        {
            string baseUrl = "https://reg.163.com/logins.jsp?username=fox%40vvfox.com&password=2727b9e3174f77e539c04ca3f741ccf3&type=0&url=http%3A%2F%2Fnote.youdao.com%2FloginCallback.html%3Fv%3D529952&url2=http%3A%2F%2Fnote.youdao.com%2FloginCallback.html%3Fv%3D529952&product=note&savelogin=1&domains=163.com%2C126.com%2Cyeah.net%2Cyoudao.com%2Cyodao.com&noRedirect=1&timestamp=1406613567452";
            StringBuilder sb = new StringBuilder();
            sb.Append("https://reg.163.com/logins.jsp?username=");
            sb.Append(WebUtility.UrlEncode(user.Name)).Append("&password=");
            sb.Append(WebTool.Md5(user.Password).ToLower()).Append("&type=0&url=http%3A%2F%2Fnote.youdao.com%2FloginCallback.html%3Fv%3D529952&url2=http%3A%2F%2Fnote.youdao.com%2FloginCallback.html%3Fv%3D529952&product=note&savelogin=1&domains=163.com%2C126.com%2Cyeah.net%2Cyoudao.com%2Cyodao.com&noRedirect=1&timestamp=").Append(WebTool.GetCurrentTimestamp());
            //Log.i(sb.ToString());
            return sb.ToString();
        }








        public class Poster
        {
            string url;
            public string Url { get { return url; } set { url = value; } }
            string result;
            public string Result { get { return result; } private set { result = value; } }
            public string Referer { get; set; }
            public string Accept { get; set; }
            public string ContentType { get; set; }
            public string UserAgent { get; set; }
            public string Host { get; set; }
            public long ContentLength { get; set; }

            public bool KeepAlive { get; set; }

            public bool PostSuccess { get; private set; }

            public HttpWebResponse Response { get; private set; }
            public CookieContainer YnoteCookieContainer { get; set; }

            public bool Post(string url)
            {
                this.Url = url;
                return Post();
            }
            public bool Post()
            {
                if(null == Url)
                {
                    throw new Exception("post  url is null");
                }
                else
                {
                    HttpWebRequest request= WebRequest.CreateHttp(new Uri(Url));
                    request.Method = "POST";
                    //Cookie
                    if(null == YnoteCookieContainer)
                        throw new Exception(" Cookie Container is null");
                    else
                        request.CookieContainer = YnoteCookieContainer;
                    //Accept
                    if(null == Accept)
                        request.Accept = DefaultYnoteRequestArgs.Accept;
                    else
                        request.Accept = Accept;
                    //ContentType
                    if(null ==ContentType)
                        request.ContentType = DefaultYnoteRequestArgs.ContentType;
                    else
                        request.ContentType = ContentType;

                    request.Headers["X-Requested-With"] = DefaultYnoteRequestArgs.XRequestedWith;

                    //Referer
                    if(null == Referer)
                        request.Referer= DefaultYnoteRequestArgs.Referer;
                    else
                        request.Referer = Referer;
                    request.Headers["Accept-Language"] = DefaultYnoteRequestArgs.AcceptLungage;
                    request.Headers["Accept-Encoding"] = DefaultYnoteRequestArgs.AcceptEncoding;
                    if(null == UserAgent)
                        request.UserAgent = DefaultYnoteRequestArgs.UserAgent;
                    else
                        request.UserAgent = UserAgent;
                    if(null == Host)
                        request.Host = DefaultYnoteRequestArgs.Host;
                    else
                        request.Host = Host;
                    if (null != ContentLength)
                        request.ContentLength = ContentLength;
                    request.Headers["DNT"] = DefaultYnoteRequestArgs.DNT;
                    if(KeepAlive)
                        request.KeepAlive = true;
                    //Do Post

                     HttpWebResponse _response;
                    try
                    {
                       _response = request.GetResponse() as HttpWebResponse;
                       PostSuccess = true;

                    }
                    catch(WebException we)
                    {
                       _response = we.Response as HttpWebResponse;
                        PostSuccess = false;
                    }
                    Response = _response;
                    return PostSuccess;

                }
            }

            public static class DefaultYnoteRequestArgs
            {
                public static string Referer = "http://note.youdao.com/";
                public static string Accept = "application/json, text/javascript, */*; q=0.01";
                public static string AcceptLungage = "zh-CN";
                public static string AcceptEncoding = "gzip";
                public static string UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; Trident/7.0; rv:11.0) like Gecko";
                public static string Host = "note.youdao.com";
                public static string ContentType = "application/x-www-form-urlencoded";
                public static string XRequestedWith = "XMLHttpRequest";
                public static string DNT = "1";
            }
            public class YnoteUser
            {
                public string Name { get; set; }
                public string Password { get; set; }
                public YnoteUser(string name, string psw)
                {
                    this.Name = name;
                    this.Password = psw;
                }
            }
        }

      
        public static class LoginUrls
        {
            public static string url1 = "http://note.youdao.com/yws/mapi/ilogrpt?method=putwcplog&keyfrom=wcp&login=login2";
            public static string url2 = "";
            public static string testUrl1 = "http://note.youdao.com/yws/mapi/share?method=publish&any=WEB HTTP/1.1";
        }

    }

}
