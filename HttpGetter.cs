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
            request.CookieContainer = MobileTribunal.Instance.cookies;
            request.BeginGetResponse(new AsyncCallback(callback), request);
        }
    }
}
