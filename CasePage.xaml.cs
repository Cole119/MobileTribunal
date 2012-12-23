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
    public partial class CasePage : PhoneApplicationPage
    {
        public CasePage()
        {
            InitializeComponent();
            //MainPivot.SelectedIndex = 1
            ObservableCollection<CaseInfo> pivotData = new ObservableCollection<CaseInfo>();
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

            /*sampleCase = new CaseInfo();
            sampleCase.header = "Game 2";
            sampleCase.champImage1 = new Uri("/Assets/unknown_champ.png", UriKind.Relative);
            sampleCase.champImage2 = new Uri("/Assets/unknown_champ.png", UriKind.Relative);
            sampleCase.champImage3 = new Uri("/Assets/unknown_champ.png", UriKind.Relative);
            sampleCase.champImage4 = new Uri("/Assets/unknown_champ.png", UriKind.Relative);
            sampleCase.champImage5 = new Uri("/Assets/unknown_champ.png", UriKind.Relative);
            sampleCase.comments = "Some example comments...";
            sampleCase.chatlog = "This is\nthe\nchatlog of the second game.";
            pivotData.Add(sampleCase);*/
            //CurrentCasePivot.DataContext = pivotData;
            CurrentCasePivot.ItemsSource = pivotData;
        }
    }
}