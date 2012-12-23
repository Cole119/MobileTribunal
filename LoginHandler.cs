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

        public LoginHandler(MobileTribunal tribunal)
        {
            this.mobileTribunal = tribunal;
        }

        public void login(String username, String password)
        {
            username = Uri.EscapeUriString(username);
            password = Uri.EscapeUriString(password);
            String url = "https://na.leagueoflegends.com/user/login";
            String content = "name=" + username + "&pass=" + password + "&form_id=user_login";
            mobileTribunal.poster.createRequest(url, content, false, new AsyncCallback(GetResponseCallback));
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
            
                // Close the stream object
                streamResponse.Close();
                streamRead.Close();
                // Release the HttpWebResponse
                response.Close();
            }
            catch (WebException ex)
            {
                System.Diagnostics.Debug.WriteLine("WebException occurred while trying to log in: " + ex.Status);
            }
        }
    }
}
