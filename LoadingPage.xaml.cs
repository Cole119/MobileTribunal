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
            /*progress = TRIBUNAL;
            MobileTribunal.Instance.getter.createRequest(
                "http://" + MobileTribunal.Instance.region + ".leagueoflegends.com/tribunal/", 
                new AsyncCallback(GetResponseCallback));*/
            progress = GUIDELINES;
            MobileTribunal.Instance.getter.createRequest(
                "http://" + MobileTribunal.Instance.region + ".leagueoflegends.com/tribunal/en/guidelines/",
                new AsyncCallback(GetResponseCallback), false);
        }

        private void GetResponseCallback(IAsyncResult asynchronousResult)
        {
            HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;
            HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
            //System.Diagnostics.Debug.WriteLine("Number of Cookies after: " + MobileTribunal.Instance.cookies.Count);
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
                        String html = new StreamReader(response.GetResponseStream()).ReadToEnd();
                        //System.Diagnostics.Debug.WriteLine("Index of Immobilon: "+html.IndexOf("Immobilon"));
                        progress = GUIDELINES;
                        MobileTribunal.Instance.getter.createRequest(
                            "http://" + MobileTribunal.Instance.region + ".leagueoflegends.com/tribunal/en/guidelines/",
                            new AsyncCallback(GetResponseCallback));
                        break;
                    case GUIDELINES:
                        progress = ACCEPT;
                        MobileTribunal.Instance.getter.createRequest(
                            "http://" + MobileTribunal.Instance.region + ".leagueoflegends.com/tribunal/accept/",
                            new AsyncCallback(GetAcceptCallback), true);
                        break;
                }
                
            }
            
        }

        private void GetAcceptCallback(IAsyncResult asynchronousResult)
        {
            HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;
            HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
            String html = new StreamReader(response.GetResponseStream()).ReadToEnd();
            //System.Diagnostics.Debug.WriteLine("Number of Cookies after: " + MobileTribunal.Instance.cookies.Count);
            //System.Diagnostics.Debug.WriteLine("Response: " + (int)response.StatusCode);
            //System.Diagnostics.Debug.WriteLine("Length: " + html.Length + "\nTitle: " + html.Substring(html.IndexOf("<title>")));
            /*the case id will be found in the title of the page
            it will look something like: <title>The Tribunal -  Reviewing Case CASEID</title>*/
            String caseId;
            int numGames;
            int startIndex = html.IndexOf("Reviewing Case ") + "Reviewing Case ".Length;
            caseId = html.Substring(startIndex, html.IndexOf("</title>") - startIndex);
            System.Diagnostics.Debug.WriteLine("Case Id: " + caseId);

            startIndex = html.IndexOf("'game_count': ") + "'game_count': ".Length;
            bool gotNumGames = int.TryParse(html.Substring(startIndex, html.IndexOf(",", startIndex) - startIndex), out numGames);
            System.Diagnostics.Debug.WriteLine("Number of games: " + numGames);

            if (gotNumGames && !(String.IsNullOrEmpty(caseId) || numGames < 1))
            {

                MobileTribunal.Instance.caseLoader.loadNewCase(caseId, numGames, new AsyncCallback(CaseLoadedCallback));
                
            }
            else
            {
                MessageBox.Show("An error occurred while trying to load a case.");
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                return;
            }
        }

        private void CaseLoadedCallback(IAsyncResult result)
        {
            NavigationService.Navigate(new Uri("/CasePage.xaml", UriKind.Relative));
        }
        /*protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            LoadInitialCase();
        }*/
    }
}