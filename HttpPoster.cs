using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MobileTribunal
{
    /*
     * HttpPoster is used to POST data to a uri
     */
    class HttpPoster
    {
        private String content;
        private CookieContainer cookieContainer;
        private AsyncCallback responseCallback;

        public HttpPoster()
        {
            //cookieContainer = new CookieContainer();
        }

        public void setContent(String newContent)
        {
            this.content = newContent;
        }

        /*
         * POSTs content to the given url
         */ 
        public void createRequest(String url, String content, bool allowAutoRedirect, AsyncCallback responseCallback)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            //System.Diagnostics.Debug.WriteLine("POST - Number of Cookies before: " + MobileTribunal.Instance.cookies.Count);
            request.CookieContainer = MobileTribunal.Instance.cookies;
            //request.Headers = MobileTribunal.Instance.headers;
            request.Headers["Referer"] = "http://na.leagueoflegends.com/";
            this.responseCallback = responseCallback;
            this.content = content;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = content.Length;
            request.AllowAutoRedirect = allowAutoRedirect;
            request.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallback), request);
        }

        /*
         * Gets a stream from the response and writes the previously supplied 
         * content to it
         */
        private void GetRequestStreamCallback(IAsyncResult asynchronousResult)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;
                // End the operation
                Stream postStream = request.EndGetRequestStream(asynchronousResult);

                // Convert the string into a byte array.
                byte[] postBytes = Encoding.UTF8.GetBytes(content);
                //System.Diagnostics.Debug.WriteLine("Content: " + content);

                // Write to the request stream.
                postStream.Write(postBytes, 0, postBytes.Length);
                postStream.Close();

                // Start the asynchronous operation to get the response
                request.BeginGetResponse(responseCallback, request);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }
    }
}
