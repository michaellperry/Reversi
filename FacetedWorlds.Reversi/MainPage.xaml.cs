using System;
using FacetedWorlds.Reversi.ViewModels;
using Microsoft.Phone.Controls;
using UpdateControls.XAML;

namespace FacetedWorlds.Reversi
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (YourName.Visibility == System.Windows.Visibility.Visible && YourName.HandleBack())
                e.Cancel = true;
            else
                base.OnBackKeyPress(e);
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
    }
}