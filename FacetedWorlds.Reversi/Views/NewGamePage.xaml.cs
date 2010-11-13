using System.Windows;
using System.Windows.Input;
using FacetedWorlds.Reversi.ViewModels;
using Microsoft.Phone.Controls;
using UpdateControls.XAML;
using System;

namespace FacetedWorlds.Reversi.Views
{
    public partial class NewGamePage : PhoneApplicationPage
    {
        public NewGamePage()
        {
            InitializeComponent();
        }

        private void Challenge_Click(object sender, RoutedEventArgs e)
        {
            ICommand challenge = ForView.Unwrap<NewGameViewModel>(DataContext).Challenge;
            if (challenge.CanExecute(null))
            {
                challenge.Execute(null);
                NavigationService.GoBack();
            }
        }
    }
}
