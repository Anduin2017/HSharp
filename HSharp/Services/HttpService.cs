using System.IO;
using System.Net;
using System.Text;

namespace Obisoft.HSharp.Services
{
    public static class HTTPService
    {
        private static CookieContainer cc = new CookieContainer();
        private static HttpWebRequest Webrequest(string URL)
        {
            var request = WebRequest.Create(URL) as HttpWebRequest;
            if (cc.Count == 0)
            {
                request.CookieContainer = new CookieContainer();
                cc = request.CookieContainer;
            }
            else
            {
                request.CookieContainer = cc;
            }
            return request;
        }

        public static string Post(string Url, string postDataStr, string Decode = "utf-8")
        {
            var request = Webrequest(Url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postDataStr.Length;
            var myRequestStream = request.GetRequestStream();
            var myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("utf-8"));
            myStreamWriter.Write(postDataStr);
            myStreamWriter.Close();
            var myResponseStream = request.GetResponse().GetResponseStream();
            var myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding(Decode));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            return retString;
        }
        public static string Get(string Url, string Coding = "utf-8")
        {
            var request = Webrequest(Url);
            request.Method = "GET";
            request.ContentType = "text/html;charset=" + Coding;
            var myResponseStream = request.GetResponse().GetResponseStream();
            var myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding(Coding));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            return retString;
        }
    }
}
