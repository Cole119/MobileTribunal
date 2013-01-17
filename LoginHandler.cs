using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace MobileTribunal
{
    class LoginHandler
    {
        MobileTribunal mobileTribunal;
        String username;
        String password;

        public LoginHandler(MobileTribunal tribunal)
        {
            this.mobileTribunal = tribunal;
        }

        public void login(String username, String password)
        {
            this.username = Uri.EscapeUriString(username);
            this.password = Uri.EscapeUriString(password);
            String url = "https://"+MobileTribunal.Instance.region+".leagueoflegends.com/user/login";
            String content = "name=" + username + "&pass=" + password + "&form_id=user_login";
            mobileTribunal.poster.createRequest(url, content, true, new AsyncCallback(GetResponseCallback));
            return;
        }

        private void GetResponseCallback(IAsyncResult asynchronousResult)
        {
            HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;
            
            // End the operation
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
                HttpStatusCode rcode = response.StatusCode;
                Stream streamResponse = response.GetResponseStream();
                StreamReader streamRead = new StreamReader(streamResponse);

                string responseString = streamRead.ReadToEnd();

                //System.Diagnostics.Debug.WriteLine("Number of Cookies after: " + MobileTribunal.Instance.cookies.Count);
                System.Diagnostics.Debug.WriteLine("Number of Cookies: " + response.Cookies.Count);
                IEnumerator<Cookie> e = response.Cookies.Cast<Cookie>().GetEnumerator();
                while (e.MoveNext())
                {
                    System.Diagnostics.Debug.WriteLine(e.Current.Name);
                }
                System.Diagnostics.Debug.WriteLine("Index of Immobilon: " + responseString.IndexOf("Immobilon"));
                System.Diagnostics.Debug.WriteLine("Index of immobilon: " + responseString.IndexOf("immobilon"));
                //System.Diagnostics.Debug.WriteLine(responseString);

                // Close the stream object
                streamResponse.Close();
                streamRead.Close();
                // Release the HttpWebResponse
                response.Close();

                if ((int)rcode == 302)
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        mobileTribunal.mainPage.loginSucceeded();
                    });
                }
                else
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        mobileTribunal.mainPage.loginFailed();
                    });
                }
            }
            catch (WebException ex)
            {
                System.Diagnostics.Debug.WriteLine("WebException occurred while trying to log in: " + ex.Status);
            }
        }
    }
}
