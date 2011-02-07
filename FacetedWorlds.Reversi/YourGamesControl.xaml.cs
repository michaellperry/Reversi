using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;

namespace FacetedWorlds.Reversi
{
	public partial class YourGamesControl : UserControl
	{
		public YourGamesControl()
		{
			// Required to initialize variables
			InitializeComponent();
		}

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ListBox)sender).SelectedItem != null)
                GoToGamePage();
        }

        private void GoToGamePage()
        {
            PhoneApplicationPage page = this.GetAncestorOfType<PhoneApplicationPage>();
            page.NavigationService.Navigate(new Uri("/Views/GamePage.xaml", UriKind.Relative));
        }
    }
}