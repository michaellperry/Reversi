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
            ICommand challenge = ForView.Unwrap<ChallengeViewModel>(DataContext).Challenge;
            if (challenge.CanExecute(null))
            {
                challenge.Execute(null);
                PhoneApplicationPage page = this.GetAncestorOfType<PhoneApplicationPage>();
                page.NavigationService.GoBack();
            }
        }
    }
}