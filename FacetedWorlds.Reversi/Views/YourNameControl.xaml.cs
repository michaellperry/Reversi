using System.Windows;
using System.Windows.Controls;
using FacetedWorlds.Reversi.ViewModels;
using UpdateControls.XAML;
using System;

namespace FacetedWorlds.Reversi.Views
{
    public partial class YourNameControl : UserControl
    {
        public YourNameControl()
        {
            InitializeComponent();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            YourNameViewModel viewModel = ForView.Unwrap<YourNameViewModel>(DataContext);
            MessageBoxResult confirmationResult = MessageBox.Show(
                String.Format("Your name will be {0}. This cannot be changed.", viewModel.YourName),
                "Your name",
                MessageBoxButton.OKCancel);
            if (confirmationResult == MessageBoxResult.OK)
                viewModel.ClaimName();
        }

        private void YourName_TextChanged(object sender, TextChangedEventArgs e)
        {
            ((TextBox)sender).GetBindingExpression(TextBox.TextProperty).UpdateSource();
        }
    }
}
