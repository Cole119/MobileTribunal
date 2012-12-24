﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MobileTribunal
{
    class MobileTribunal
    {
        public static MobileTribunal Instance;
        public MainPage mainPage;
        public HttpPoster poster;
        public HttpGetter getter;
        public ObservableCollection<CaseInfo> currentCase;
        public CookieContainer cookies;
        public String region;

        public MobileTribunal(MainPage page)
        {
            Instance = this;
            this.mainPage = page;
            poster = new HttpPoster();
            getter = new HttpGetter();
            currentCase = new ObservableCollection<CaseInfo>();
            cookies = new CookieContainer();
            region = "na";
        }
    }
}
