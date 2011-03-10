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

namespace FacetedWorlds.Reversi.Free
{
    public partial class YourGamesControl : UserControl
    {
        public YourGamesControl()
        {
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
            page.NavigationService.Navigate(new Uri("/FacetedWorlds.Reversi.Shared;component/Views/GamePage.xaml", UriKind.Relative));
        }
    }
}
