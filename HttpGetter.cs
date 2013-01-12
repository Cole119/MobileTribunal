using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MobileTribunal
{
    class HttpGetter
    {
        public void createRequest(String url, AsyncCallback callback)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            System.Diagnostics.Debug.WriteLine("GET - Number of Cookies before: " + MobileTribunal.Instance.cookies.Count);
            request.CookieContainer = MobileTribunal.Instance.cookies;
            //request.Headers = MobileTribunal.Instance.headers;
            request.BeginGetResponse(new AsyncCallback(callback), request);
        }
    }
}
