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

namespace MobileTribunal
{
    public partial class MainPage : PhoneApplicationPage
    {
        private MobileTribunal mobileTribunal;

        private LoginHandler loginHandler;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();

            mobileTribunal = new MobileTribunal(this);
            loginHandler = new LoginHandler(mobileTribunal);
        }

        private void LoginButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            loginHandler.login(UsernameField.Text, PasswordField.Password);
            /*bool successful = loginHandler.login(UsernameField.Text, PasswordField.Password);
            if (!successful)
            {
                LoginFailedText.Visibility = System.Windows.Visibility.Visible;
            }*/
        }

        public void loginFailed()
        {
            LoginFailedText.Visibility = System.Windows.Visibility.Visible;
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