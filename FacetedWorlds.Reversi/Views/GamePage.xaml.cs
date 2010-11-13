using System.Windows.Navigation;
using FacetedWorlds.Reversi.ViewModels;
using Microsoft.Phone.Controls;
using UpdateControls.XAML;
using System;

namespace FacetedWorlds.Reversi.Views
{
    public partial class GamePage : PhoneApplicationPage
    {
        public GamePage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (e.Uri == new Uri("/MainPage.xaml", UriKind.Relative))
            {
                GameViewModel viewModel = ForView.Unwrap<GameViewModel>(DataContext);
                viewModel.CommitMove();
                viewModel.ClearSelectedPlayer();
            }
        }
    }
}
