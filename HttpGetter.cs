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
        public void createRequest(String url, AsyncCallback callback, bool allowAutoRedirect=true)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.AllowAutoRedirect = allowAutoRedirect;
            request.CookieContainer = MobileTribunal.Instance.cookies;
            //request.Headers = MobileTribunal.Instance.headers;
            request.BeginGetResponse(new AsyncCallback(callback), request);
        }
    }
}
