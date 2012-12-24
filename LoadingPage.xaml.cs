using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Threading;
using System.IO;

namespace MobileTribunal
{
    public partial class LoadingPage : PhoneApplicationPage
    {
        int progress;
        const int TRIBUNAL = 1, GUIDELINES = 2, ACCEPT = 3;

        public LoadingPage()
        {
            InitializeComponent();
            LoadInitialCase();
        }

        public void LoadInitialCase()
        {
            progress = TRIBUNAL;
            MobileTribunal.Instance.getter.createRequest(
                "http://" + MobileTribunal.Instance.region + ".leagueoflegends.com/tribunal/", 
                new AsyncCallback(GetResponseCallback));
        }

        private void GetResponseCallback(IAsyncResult asynchronousResult)
        {
            HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;
            HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
            if ((int)response.StatusCode != 200)
            {
                MessageBox.Show("An error occurred while trying to load a case.");
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                return;
            }
            else
            {
                switch (progress)
                {
                    case TRIBUNAL:
                        progress = GUIDELINES;
                        MobileTribunal.Instance.getter.createRequest(
                            "http://" + MobileTribunal.Instance.region + ".leagueoflegends.com/tribunal/en/guidelines/",
                            new AsyncCallback(GetResponseCallback));
                        break;
                    case GUIDELINES:
                        progress = ACCEPT;
                        MobileTribunal.Instance.getter.createRequest(
                            "http://" + MobileTribunal.Instance.region + ".leagueoflegends.com/tribunal/accept/",
                            new AsyncCallback(GetAcceptCallback));
                        break;
                }
                
            }
            
        }

        private void GetAcceptCallback(IAsyncResult asynchronousResult)
        {
            HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;
            HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
            String html = new StreamReader(response.GetResponseStream()).ReadToEnd();
            System.Diagnostics.Debug.WriteLine("Response: " + (int)response.StatusCode);
            System.Diagnostics.Debug.WriteLine("Length: "+html.Length+"\nTitle: "+html.Substring(html.IndexOf("<title>")));
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri("/CasePage.xaml", UriKind.Relative));
            });
        }
        /*protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            LoadInitialCase();
        }*/
    }
}