using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Browser;
using System.Text;
using System.Threading.Tasks;

namespace MobileTribunal
{
    /*
     * This class is used to hold a reference to shared 
     * objects that any class can access. The initial
     * MobileTribunal object is created at application
     * runtime and the static Instance object is set to it.
     */
    class MobileTribunal
    {
        private static MobileTribunal Instance; //This static object allows all classes to reference the other objects in this class
        //public MainPage mainPage;
        public HttpPoster poster;
        public HttpGetter getter;
        public ObservableCollection<CaseInfo> currentCase;
        public CookieContainer cookies;
        public String region;
        public WebHeaderCollection headers;
        public CaseLoader caseLoader;

        private MobileTribunal()
        {
            Instance = this;
            //this.mainPage = page;
            poster = new HttpPoster();
            getter = new HttpGetter();
            currentCase = new ObservableCollection<CaseInfo>();
            cookies = new CookieContainer();
            headers = new WebHeaderCollection();
            region = "na";
            caseLoader = new CaseLoader();
        }

        public static MobileTribunal GetInstance(){
            if (Instance == null)
            {
                Instance = new MobileTribunal();
            }

            return Instance;
        }
    }
}
