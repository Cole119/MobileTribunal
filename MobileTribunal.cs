using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MobileTribunal
{
    class MobileTribunal
    {
        public MainPage mainPage;
        public HttpPoster poster;

        public MobileTribunal(MainPage page)
        {
            this.mainPage = page;
            poster = new HttpPoster();
        }
    }
}
