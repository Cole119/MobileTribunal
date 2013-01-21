using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MobileTribunal
{
    /*
     * The CaseInfo class holds all of the necessary information
     * about a game in a case that will be used to create and populate
     * a CasePage.
     */
    public class CaseInfo
    {
        public String header { get; set; } //name of the pivot item (Generally something like "Game 1")
        public ObservableCollection<Uri> champImages { get; set; } //The URIs of the images for rendered champion icons
        public String comments { get; set; } //reporter comments
        //public String chatlog { get; set; }
        public ObservableCollection<ChatlogMessage> chatlog { get; set; } 

        public CaseInfo()
        {
            champImages = new ObservableCollection<Uri>();
            chatlog = new ObservableCollection<ChatlogMessage>();
        }
    }

    /*
     * The ChatlogMessage class holds information that dictates
     * how a specific message on the chatlog is displayed.
     */
    public class ChatlogMessage
    {
        public String player { get; set; }
        public String text { get; set; }
        public String color { get; set; }

        public ChatlogMessage()
        {
        }

        public ChatlogMessage(String player, String text, String color)
        {
            this.player = player;
            this.text = text;
            this.color = color;
        }
    }
}
