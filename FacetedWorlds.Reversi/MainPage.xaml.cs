using System;
using FacetedWorlds.Reversi.ViewModels;
using Microsoft.Phone.Controls;
using UpdateControls.XAML;
using Microsoft.Phone.Shell;
using System.Windows;
using Microsoft.Phone.Tasks;

namespace FacetedWorlds.Reversi
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();

            ApplicationBar = new ApplicationBar();
            AddButton("/Images/appbar.add.rest.png", "new game", NewGameButtonClick);
            AddButton("/Images/appbar.pass.rest.png", "pass phone", PassThePhoneButtonClick);
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

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            ViewModel.Synchronize();
            if (ViewModel.HasSelectedPlayer)
            {
                NavigationService.Navigate(new Uri("/Views/GamePage.xaml", UriKind.Relative));
            }
        }

        private void AddButton(string imageUrl, string text, EventHandler click)
        {
            ApplicationBarIconButton newGameButton = new ApplicationBarIconButton(new Uri(imageUrl, UriKind.Relative));
            newGameButton.Text = text;
            newGameButton.Click += click;
            ApplicationBar.Buttons.Add(newGameButton);
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
                NavigationService.Navigate(new Uri("/Views/NewGamePage.xaml", UriKind.Relative));
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
            NavigationService.Navigate(new Uri("/Views/GamePage.xaml", UriKind.Relative));
        }

        private MainViewModel ViewModel
        {
            get { return ForView.Unwrap<MainViewModel>(DataContext); }
        }
    }
}