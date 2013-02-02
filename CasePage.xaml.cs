using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;

namespace MobileTribunal
{
    /*
     * This class simply sets the data source of the CasePage.xaml.
     * The CasePage.xaml then renders the current case from that data.
     */
    public partial class CasePage : PhoneApplicationPage
    {
        public CasePage()
        {
            InitializeComponent();
            //MainPivot.SelectedIndex = 1
            /*ObservableCollection<CaseInfo> pivotData = new ObservableCollection<CaseInfo>();
            CaseInfo sampleCase = new CaseInfo();
            sampleCase.header = "Game 1";
            sampleCase.champImages = new ObservableCollection<Uri>();
            sampleCase.champImages.Add(new Uri("/Assets/unknown_champ.png", UriKind.Relative));
            sampleCase.champImages.Add(new Uri("/Assets/unknown_champ.png", UriKind.Relative));
            sampleCase.champImages.Add(new Uri("/Assets/unknown_champ.png", UriKind.Relative));
            sampleCase.champImages.Add(new Uri("/Assets/unknown_champ.png", UriKind.Relative));
            sampleCase.champImages.Add(new Uri("/Assets/unknown_champ.png", UriKind.Relative));
            sampleCase.comments = "Some example comments...";
            sampleCase.chatlog = "This is\nthe\nchatlog";
            pivotData.Add(sampleCase);

            sampleCase = new CaseInfo();
            sampleCase.header = "Game 2";
            sampleCase.champImages = new ObservableCollection<Uri>();
            sampleCase.champImages.Add(new Uri("/Assets/unknown_champ.png", UriKind.Relative));
            sampleCase.champImages.Add(new Uri("/Assets/unknown_champ.png", UriKind.Relative));
            sampleCase.champImages.Add(new Uri("/Assets/unknown_champ.png", UriKind.Relative));
            sampleCase.champImages.Add(new Uri("/Assets/unknown_champ.png", UriKind.Relative));
            sampleCase.champImages.Add(new Uri("/Assets/unknown_champ.png", UriKind.Relative));
            sampleCase.comments = "Some example comments...";
            sampleCase.chatlog = "This is\nthe\nchatlog of the\nsecond game.";
            pivotData.Add(sampleCase);
            CurrentCasePivot.ItemsSource = pivotData;*/
            CurrentCasePivot.ItemsSource = MobileTribunal.Instance.currentCase;

        }

        private void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        {
            ExpanderView ex = (ExpanderView)GetTemplateChild("chatlogExpander");
            if (ex != null) System.Diagnostics.Debug.WriteLine("TextBlock height: " + ex.Height);
            else System.Diagnostics.Debug.WriteLine("Couldn't find control.");
        }
    }
}