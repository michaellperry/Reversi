using System;
using FacetedWorlds.Reversi.ViewModels;
using Microsoft.Phone.Controls;
using UpdateControls.XAML;
using Microsoft.Phone.Shell;

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
            MainViewModel viewModel = ForView.Unwrap<MainViewModel>(DataContext);
            viewModel.Synchronize();
            if (viewModel.HasSelectedPlayer)
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
            NavigationService.Navigate(new Uri("/Views/NewGamePage.xaml", UriKind.Relative));
        }

        private void CreateLocalGame()
        {
            MainViewModel viewModel = ForView.Unwrap<MainViewModel>(DataContext);
            viewModel.CreateLocalGame();
            NavigationService.Navigate(new Uri("/Views/GamePage.xaml", UriKind.Relative));
        }
    }
}