using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MobileTribunal.Resources;
using System.IO.IsolatedStorage;

namespace MobileTribunal
{
    public partial class MainPage : PhoneApplicationPage
    {
        private MobileTribunal mobileTribunal;
        private LoginHandler loginHandler;
        private IsolatedStorageSettings appSettings;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();

            mobileTribunal = new MobileTribunal(this);
            loginHandler = new LoginHandler(mobileTribunal);
            appSettings = IsolatedStorageSettings.ApplicationSettings;
            if (appSettings.Contains("Username"))
            {
                UsernameWatermark.Visibility = System.Windows.Visibility.Collapsed;
                UsernameField.Text = appSettings["Username"].ToString();
            }
            if (appSettings.Contains("Remember"))
            {
                RememberCheckBox.IsChecked = true;
            }
        }

        private void LoginButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            LoginProgressBar.Visibility = System.Windows.Visibility.Visible;
            if (RememberCheckBox.IsChecked.HasValue && RememberCheckBox.IsChecked.Value)
            {
                if (!(appSettings.Contains("Username")))
                {
                    appSettings.Add("Username", "");
                }
                appSettings["Username"] = UsernameField.Text;
            }

            if (!(appSettings.Contains("Remember")))
            {
                appSettings.Add("Remember", true);
            }
            appSettings["Remember"] = (RememberCheckBox.IsChecked.HasValue && RememberCheckBox.IsChecked.Value);
            appSettings.Save();

            loginHandler.login(UsernameField.Text, PasswordField.Password);
        }

        public void loginSucceeded()
        {
            LoginProgressBar.Visibility = System.Windows.Visibility.Collapsed;
            LoginFailedText.Visibility = System.Windows.Visibility.Collapsed;
            NavigationService.Navigate(new Uri("/CasePage.xaml", UriKind.Relative));
        }

        public void loginFailed()
        {
            LoginProgressBar.Visibility = System.Windows.Visibility.Collapsed;
            LoginFailedText.Visibility = System.Windows.Visibility.Visible;
        }

        private void UsernameField_GotFocus(object sender, RoutedEventArgs e)
        {
            UsernameWatermark.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void UsernameField_LostFocus(object sender, RoutedEventArgs e)
        {
            if (UsernameField.Text.Length < 1)
            {
                UsernameWatermark.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void PasswordField_GotFocus(object sender, RoutedEventArgs e)
        {
            PasswordWatermark.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void PasswordField_LostFocus(object sender, RoutedEventArgs e)
        {
            if (PasswordField.Password.Length < 1)
            {
                PasswordWatermark.Visibility = System.Windows.Visibility.Visible;
            }
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}