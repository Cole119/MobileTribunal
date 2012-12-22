using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MobileTribunal
{
    class LoginHandler
    {
        MobileTribunal mobileTribunal;
        String content;

        public LoginHandler(MobileTribunal tribunal)
        {
            this.mobileTribunal = tribunal;
        }

        public void login(String username, String password)
        {
            string auth;
            username = Uri.EscapeUriString(username);
            password = Uri.EscapeUriString(password);
            auth = "https://leagueoflegends.com";
            auth += "?login";
            auth = Uri.EscapeUriString(auth);

            WebClient client = mobileTribunal.webClient;
            /*client.DownloadStringCompleted += client_DownloadStringCompleted;
            client.DownloadStringAsync(new Uri("http://www.google.com/"));
            client.UploadStringCompleted += client_UploadStringCompleted;
            client.UploadStringAsync(new Uri("http://na.leagueoflegends.com/user/login"), "POST", Uri.EscapeUriString("?name="+username+"&pass="+password+"&form_id=user_login"));*/

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("https://na.leagueoflegends.com/user/login");
            CookieContainer cookies = request.CookieContainer;
            content = "name=" + username + "&pass=" + password + "&form_id=user_login";
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = content.Length;
            request.AllowAutoRedirect = false;
            request.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallback), request);
            return;
        }

        private void GetRequestStreamCallback(IAsyncResult asynchronousResult)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;

                // End the operation
                Stream postStream = request.EndGetRequestStream(asynchronousResult);

                // Convert the string into a byte array.
                byte[] postBytes = Encoding.UTF8.GetBytes(content);

                // Write to the request stream.
                postStream.Write(postBytes, 0, postBytes.Length);
                postStream.Close();

                // Start the asynchronous operation to get the response
                request.BeginGetResponse(new AsyncCallback(GetResponseCallback), request);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        private void GetResponseCallback(IAsyncResult asynchronousResult)
        {
            HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;

            // End the operation
            HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
            HttpStatusCode rcode = response.StatusCode;
            Stream streamResponse = response.GetResponseStream();
            StreamReader streamRead = new StreamReader(streamResponse);

            string responseString = streamRead.ReadToEnd();

            System.Diagnostics.Debug.WriteLine("Response Code: " + (int)rcode);
            // Close the stream object
            streamResponse.Close();
            streamRead.Close();
            // Release the HttpWebResponse
            response.Close(); 
        }
    }
}
