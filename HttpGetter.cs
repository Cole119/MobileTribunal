using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MobileTribunal
{
    /*
     * HttpGetter performs a GET request for a given url
     */
    class HttpGetter
    {
        public void createRequest(String url, AsyncCallback callback, bool allowAutoRedirect=true)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.AllowAutoRedirect = allowAutoRedirect;
            request.CookieContainer = MobileTribunal.GetInstance().cookies;
            //request.Headers = MobileTribunal.Instance.headers;
            request.BeginGetResponse(new AsyncCallback(callback), request);
        }
    }
}
