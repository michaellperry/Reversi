using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using Microsoft.Phone.Shell;
using FacetedWorlds.Reversi.ViewModels;
using UpdateControls.XAML;

namespace FacetedWorlds.Reversi.Free
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            ApplicationBar = new ApplicationBar();
            AddButton("/Images/appbar.add.rest.png", "new game", NewGameButtonClick);
            AddButton("/Images/appbar.pass.rest.png", "pass phone", PassThePhoneButtonClick);
            AddMenuItem("Settings", SettingsClick);
            ApplicationBar.IsVisible = true;
        }

        void NewGameButtonClick(object sender, EventArgs e)
        {
            CreateRemoteGame();
        }

        void PassThePhoneButtonClick(object sender, EventArgs e)
        {
            CreateLocalGame();
        }

        void SettingsClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/FacetedWorlds.Reversi.Shared;component/Views/SettingsPage.xaml", UriKind.Relative));
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            ViewModel.Synchronize();
            if (ViewModel.HasSelectedPlayer)
            {
                NavigationService.Navigate(new Uri("/FacetedWorlds.Reversi.Shared;component/Views/GamePage.xaml", UriKind.Relative));
            }
        }

        private void AddButton(string imageUrl, string text, EventHandler click)
        {
            ApplicationBarIconButton newGameButton = new ApplicationBarIconButton(new Uri(imageUrl, UriKind.Relative));
            newGameButton.Text = text;
            newGameButton.Click += click;
            ApplicationBar.Buttons.Add(newGameButton);
        }

        private void AddMenuItem(string text, EventHandler click)
        {
            ApplicationBarMenuItem menuItem = new ApplicationBarMenuItem(text);
            menuItem.Click += click;
            ApplicationBar.MenuItems.Add(menuItem);
        }

        private void NewGame_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CreateRemoteGame();
        }

        private void PassThePhone_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CreateLocalGame();
        }

        private void CreateRemoteGame()
        {
            if (ViewModel.CanChallenge)
                NavigationService.Navigate(new Uri("/FacetedWorlds.Reversi.Shared;component/Views/NewGamePage.xaml", UriKind.Relative));
            else
            {
                MessageBoxResult trialResult = MessageBox.Show(
                    "Your trial period has expired. Please purchase the game.",
                    "Trial expired",
                    MessageBoxButton.OKCancel);
                if (trialResult == MessageBoxResult.OK)
                {
                    // Show the application details.
                    new MarketplaceDetailTask
                    {
                        ContentType = MarketplaceContentType.Applications
                    }.Show();
                }
            }
        }

        private void CreateLocalGame()
        {
            ViewModel.CreateLocalGame();
            NavigationService.Navigate(new Uri("/FacetedWorlds.Reversi.Shared;component/Views/GamePage.xaml", UriKind.Relative));
        }

        private MainViewModel ViewModel
        {
            get { return ForView.Unwrap<MainViewModel>(DataContext); }
        }
    }
}