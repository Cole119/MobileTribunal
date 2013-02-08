using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace MobileTribunal
{
    /*
     * The CaseLoader class is used to get case information
     * from the server, parse it, and prepare a new CaseInfo
     * object to hold that information
     */
    class CaseLoader
    {
        String caseId;
        int currentGame = 1;
        int numGames = 0;
        AsyncCallback callback; //used to notify the calling object that we are done.

        public bool loadNewCase(String caseId, int numGames, AsyncCallback callback)
        {
            if (String.IsNullOrEmpty(caseId) || numGames < 1)
            {
                return false;
            }

            this.caseId = caseId;
            this.numGames = numGames;
            this.callback = callback;

            MobileTribunal.GetInstance().currentCase.Clear();

            MobileTribunal.GetInstance().getter.createRequest("http://" + MobileTribunal.GetInstance().region +
                                                         ".leagueoflegends.com/tribunal/en/get_game/" + caseId + "/" +currentGame+"/", 
                                                         new AsyncCallback(GetResponseCallback));
            
            return true;
        }

        /*
         * Receives the response from the server in JSON format, parses it,
         * and stores the parsed data in a new CaseInfo object.
         */
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

                string responseString = streamRead.ReadToEnd(); //String will be in JSON format
                JObject json = JObject.Parse(responseString);
                JArray players = (JArray)json["players"];
                CaseInfo newCase = new CaseInfo();
                newCase.header = "Game " + currentGame;
                for (int i = 0; i < players.Count; i++)
                {
                    JObject player = (JObject)players[i];
                    string association = (String)player["association_to_offender"];
                    if (association.Equals("ally") || association.Equals("offender"))
                    {
                        newCase.champImages.Add(new Uri("http://" + MobileTribunal.GetInstance().region + ".leagueoflegends.com" + (string)player["champion_url"]));
                    }
                }
                parseChatlog((JArray)json["chat_log"], newCase);

                MobileTribunal.GetInstance().currentCase.Add(newCase);
            }
            catch (WebException ex)
            {
                System.Diagnostics.Debug.WriteLine("WebException occurred while trying to log in: " + ex.Status);
            }

            //Gets the next game in the case incrementally. In the future this will be done in parallel with the first request.
            if(currentGame < numGames){
                currentGame++;
                MobileTribunal.GetInstance().getter.createRequest("http://" + MobileTribunal.GetInstance().region +
                                                         ".leagueoflegends.com/tribunal/en/get_game/" + caseId + "/" + currentGame + "/",
                                                         new AsyncCallback(GetResponseCallback));
            }
            else{
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    callback.Invoke(null);
                });
            }
        }

        /*
         * Parses the JSON chat data into multiple ChatlogMessages.
         */
        public void parseChatlog(JArray chatlog, CaseInfo caseInfo)
        {
            for (int i = 0; i < chatlog.Count; i++)
            {
                String chatLine = "";
                String player = "";
                String association = "";
                ChatlogMessage line = new ChatlogMessage();
                JObject message = (JObject)chatlog[i];
                association = (String)message["association_to_offender"];
                player += (string)message["champion_name"];
                if (((string)message["sent_to"]).Equals("All"))
                {
                    player += " [All]";
                }
                line.player = player;

                chatLine += ": "+(string)message["message"];
                line.text = chatLine;
                if (association.Equals("offender"))
                {
                    line.color = "BlueViolet";
                }
                else if (association.Equals("enemy"))
                {
                    line.color = "Red";
                }
                else
                {
                    line.color = "LimeGreen";
                }
                caseInfo.chatlog.Add(line);
            }
            
        }
    }
}
