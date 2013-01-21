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
    class CaseLoader
    {
        String caseId;
        int currentGame = 1;
        int numGames = 0;
        AsyncCallback callback;

        public bool loadNewCase(String caseId, int numGames, AsyncCallback callback)
        {
            if (String.IsNullOrEmpty(caseId) || numGames < 1)
            {
                return false;
            }

            this.caseId = caseId;
            this.numGames = numGames;
            this.callback = callback;

            MobileTribunal.Instance.currentCase.Clear();

            MobileTribunal.Instance.getter.createRequest("http://" + MobileTribunal.Instance.region +
                                                         ".leagueoflegends.com/tribunal/en/get_game/" + caseId + "/" +currentGame+"/", 
                                                         new AsyncCallback(GetResponseCallback));
            
            return true;
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
                JObject json = JObject.Parse(responseString);
                JArray players = (JArray)json["players"];
                //System.Diagnostics.Debug.WriteLine("Game " + currentGame + ": " + responseString.Substring(0, 100) + "\n");
                CaseInfo newCase = new CaseInfo();
                newCase.header = "Game " + currentGame;
                for (int i = 0; i < players.Count; i++)
                {
                    JObject player = (JObject)players[i];
                    string association = (String)player["association_to_offender"];
                    if (association.Equals("ally") || association.Equals("offender"))
                    {
                        newCase.champImages.Add(new Uri("http://" + MobileTribunal.Instance.region + ".leagueoflegends.com" + (string)player["champion_url"]));
                    }
                }
                parseChatlog((JArray)json["chat_log"], newCase);

                MobileTribunal.Instance.currentCase.Add(newCase);
            }
            catch (WebException ex)
            {
                System.Diagnostics.Debug.WriteLine("WebException occurred while trying to log in: " + ex.Status);
            }

            if(currentGame < numGames){
                currentGame++;
                MobileTribunal.Instance.getter.createRequest("http://" + MobileTribunal.Instance.region +
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
