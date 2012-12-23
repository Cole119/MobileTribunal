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

namespace MobileTribunal
{
    public partial class CasePage : PhoneApplicationPage
    {
        public CasePage()
        {
            InitializeComponent();
            //MainPivot.SelectedIndex = 1
            ObservableCollection<CaseInfo> pivotData = new ObservableCollection<CaseInfo>();
        }
    }
}