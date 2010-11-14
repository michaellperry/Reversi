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
            ShowConfirmationPopup();
        }

        private void Yes_Click(object sender, RoutedEventArgs e)
        {
            ForView.Unwrap<YourNameViewModel>(DataContext).ClaimName();
            HideConfirmationPopup();
        }

        private void No_Click(object sender, RoutedEventArgs e)
        {
            HideConfirmationPopup();
        }

        public bool HandleBack()
        {
            if (!InputPanel.IsHitTestVisible)
            {
                HideConfirmationPopup();
                return true;
            }
            else
                return false;
        }

        private void ShowConfirmationPopup()
        {
            ConfirmationPopup.IsHitTestVisible = true;
            InputPanel.IsHitTestVisible = false;
            VisualStateManager.GoToState(this, "PopupVisible", true);
        }

        private void HideConfirmationPopup()
        {
            ConfirmationPopup.IsHitTestVisible = false;
            InputPanel.IsHitTestVisible = true;
            VisualStateManager.GoToState(this, "PopupHidden", true);
        }

        private void YourName_TextChanged(object sender, TextChangedEventArgs e)
        {
            ((TextBox)sender).GetBindingExpression(TextBox.TextProperty).UpdateSource();
        }
    }
}
