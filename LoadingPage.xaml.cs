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
    /*
     * The LoadingPage is displayed while case data is being downloaded
     * from the server and parsed.
     */
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
            progress = GUIDELINES;
            MobileTribunal.GetInstance().getter.createRequest(
                "http://" + MobileTribunal.GetInstance().region + ".leagueoflegends.com/tribunal/en/guidelines/",
                new AsyncCallback(GetResponseCallback), false);
        }

        /*
         * Gets a response back and checks if the tribunal is available.
         * If it is, then it requests the /tribunal/accept page.
         */
        private void GetResponseCallback(IAsyncResult asynchronousResult)
        {
            HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;
            HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
            String html = new StreamReader(response.GetResponseStream()).ReadToEnd();
            //System.Diagnostics.Debug.WriteLine("Number of Cookies after: " + MobileTribunal.Instance.cookies.Count);
            if ((int)response.StatusCode != 200)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    MessageBox.Show("An error occurred while trying to load a case.");
                    NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                });
                return;
            }
            else if (html.Contains("Tribunal in Recess"))
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    MessageBox.Show("The Tribunal is currently in recess. Try again later.");
                    NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                });
                return;
            }
            else
            {
                progress = ACCEPT;
                MobileTribunal.GetInstance().getter.createRequest(
                    "http://" + MobileTribunal.GetInstance().region + ".leagueoflegends.com/tribunal/accept/",
                    new AsyncCallback(GetAcceptCallback), true);
            }
            
        }

        /*
         * Gets a response from the /tribunal/accept request. This response will contain
         * the case id and the number of game in the case. 
         * These two pieces of info are used by the CaseLoader class to request the JSON data.
         */
        private void GetAcceptCallback(IAsyncResult asynchronousResult)
        {
            HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;
            HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
            String html = new StreamReader(response.GetResponseStream()).ReadToEnd();
            //System.Diagnostics.Debug.WriteLine("Number of Cookies after: " + MobileTribunal.Instance.cookies.Count);
            //System.Diagnostics.Debug.WriteLine("Response: " + (int)response.StatusCode);
            //System.Diagnostics.Debug.WriteLine("Length: " + html.Length + "\nTitle: " + html.Substring(html.IndexOf("<title>")));

            /* The case id will be found in the title of the page
             * it will look something like: <title>The Tribunal -  Reviewing Case CASEID</title>
             */
            String caseId;
            int numGames;
            int startIndex = html.IndexOf("Reviewing Case ") + "Reviewing Case ".Length;
            caseId = html.Substring(startIndex, html.IndexOf("</title>") - startIndex);
            System.Diagnostics.Debug.WriteLine("Case Id: " + caseId);

            /* The number of games is stored in a JSON structure.
             * it looks something like 'game_count': NUMGAMES,
             */
            startIndex = html.IndexOf("'game_count': ") + "'game_count': ".Length;
            bool gotNumGames = int.TryParse(html.Substring(startIndex, html.IndexOf(",", startIndex) - startIndex), out numGames);
            System.Diagnostics.Debug.WriteLine("Number of games: " + numGames);

            if (gotNumGames && !(String.IsNullOrEmpty(caseId) || numGames < 1))
            {

                MobileTribunal.GetInstance().caseLoader.loadNewCase(caseId, numGames, new AsyncCallback(CaseLoadedCallback));
            }
            else
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    MessageBox.Show("An error occurred while trying to load a case.");
                    NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                });
                return;
            }
        }

        /*
         * This is given to the CaseLoader object and will be called
         * after the CasePage is ready to display
         */
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