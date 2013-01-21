using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Collections;
using System.IO;

namespace MobileTribunal
{
    public partial class BrowserLoginPage : PhoneApplicationPage
    {
        public BrowserLoginPage()
        {
            InitializeComponent();
            if (MobileTribunal.Instance == null)
            {
                MobileTribunal.Instance = new MobileTribunal(null);
            }
        }

        private void BrowserDoneButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            bool loggedIn = false;
            CookieCollection cookies = WebBrowserControl.GetCookies();
            //System.Diagnostics.Debug.WriteLine("Num cookies: " + cookies.Count);
            IEnumerator enumerator = cookies.GetEnumerator();
            while (enumerator.MoveNext())
            {
                //System.Diagnostics.Debug.WriteLine("Name: " + ((Cookie)enumerator.Current).Name);
                Cookie cookie = new Cookie();
                cookie.Name = ((Cookie)enumerator.Current).Name;
                cookie.Value = ((Cookie)enumerator.Current).Value;
                MobileTribunal.Instance.cookies.Add(new Uri("http://"+MobileTribunal.Instance.region+".leagueoflegends.com"), cookie);
                if (cookie.Name.Equals("bbuserid", StringComparison.OrdinalIgnoreCase))
                {
                    loggedIn = true;
                }
            }
            //testCookies();
            WebBrowserControl.ClearCookiesAsync();

            if (loggedIn)
            {
                //MobileTribunal.Instance.mainPage.loginSucceeded();
                NavigationService.Navigate(new Uri("/LoadingPage.xaml", UriKind.Relative));
            }
            else
            {
                //MobileTribunal.Instance.mainPage.loginFailed();
                MessageBox.Show("You don't appear to be logged in. Try again.");
            }
        }

        private void testCookies()
        {
            HttpWebRequest request = HttpWebRequest.CreateHttp("http://na.leagueoflegends.com");
            request.CookieContainer = MobileTribunal.Instance.cookies;
            request.BeginGetResponse(new AsyncCallback(GetResponse), request);
        }

        private void GetResponse(IAsyncResult result)
        {
            HttpWebRequest request = (HttpWebRequest)result.AsyncState;

            HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result);
            Stream streamResponse = response.GetResponseStream();
            StreamReader streamRead = new StreamReader(streamResponse);

            string responseString = streamRead.ReadToEnd();
            System.Diagnostics.Debug.WriteLine("Index of immobilon: "+responseString.IndexOf("immobilon"));
            System.Diagnostics.Debug.WriteLine("Index of Immobilon: " + responseString.IndexOf("Immobilon"));
        }
    }
}