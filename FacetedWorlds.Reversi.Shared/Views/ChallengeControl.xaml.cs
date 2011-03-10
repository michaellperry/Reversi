using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FacetedWorlds.Reversi.ViewModels;
using UpdateControls.XAML;
using Microsoft.Phone.Controls;
using System;
using System.Windows.Media;

namespace FacetedWorlds.Reversi
{
    public partial class ChallengeControl : UserControl
	{
		public ChallengeControl()
		{
			// Required to initialize variables
			InitializeComponent();
		}

        private void Challenge_Click(object sender, RoutedEventArgs e)
        {
            ICommand challenge = ViewModel.Challenge;
            if (challenge.CanExecute(null))
            {
                challenge.Execute(null);
                GoBack();
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.OpponentName = (string)((ListBox)sender).SelectedItem;
        }

        private void RequestGame_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.RequestGame();
            GoBack();
        }

        private ChallengeViewModel ViewModel
        {
            get { return ForView.Unwrap<ChallengeViewModel>(DataContext); }
        }

        private void GoBack()
        {
            PhoneApplicationPage page = this.GetAncestorOfType<PhoneApplicationPage>();
            page.NavigationService.GoBack();
        }
    }
}