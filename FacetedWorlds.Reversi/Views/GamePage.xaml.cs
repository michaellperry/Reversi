using System.Windows.Navigation;
using FacetedWorlds.Reversi.ViewModels;
using Microsoft.Phone.Controls;
using UpdateControls.XAML;
using System;
using System.Windows;

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
                IGameViewModel viewModel = ForView.Unwrap<IGameViewModel>(DataContext);
                if (viewModel != null)
                {
                    viewModel.CommitMove();
                    viewModel.ClearSelectedPlayer();
                    viewModel.AcknowledgeOutcome();
                }
            }
        }

        private void Resign_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            IGameViewModel viewModel = ForView.Unwrap<IGameViewModel>(DataContext);
            if (viewModel.Resign.CanExecute(null))
            {
                MessageBoxResult resignResult = MessageBox.Show(
                    "You are about to resign.",
                    "Resign",
                    MessageBoxButton.OKCancel);
                if (resignResult == MessageBoxResult.OK)
                    viewModel.Resign.Execute(null);
            }
        }
    }
}
