using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MobileTribunal
{
    public class CaseInfo
    {
        public String header { get; set; }
        /*public Uri champImage1 { get; set; }
        public Uri champImage2 { get; set; }
        public Uri champImage3 { get; set; }
        public Uri champImage4 { get; set; }
        public Uri champImage5 { get; set; }*/
        public ObservableCollection<Uri> champImages { get; set; }
        public String comments { get; set; }
        //public String chatlog { get; set; }
        public ObservableCollection<ChatlogMessage> chatlog { get; set; }

        public CaseInfo()
        {
            champImages = new ObservableCollection<Uri>();
            chatlog = new ObservableCollection<ChatlogMessage>();
        }
    }

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
