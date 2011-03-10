using System.Windows.Controls;
using UpdateControls.XAML;
using FacetedWorlds.Reversi.ViewModels;
using System.Windows.Input;

namespace FacetedWorlds.Reversi.Views
{
	public partial class ChatControl : UserControl
	{
        private double _oldTop = 0.0;

		public ChatControl()
		{
			// Required to initialize variables
			InitializeComponent();
		}

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ((TextBox)sender).GetBindingExpression(TextBox.TextProperty).UpdateSource();
        }

        private void ChatControl_LayoutUpdated(object sender, System.EventArgs e)
        {
            double top = ListContent.ActualHeight - LayoutRoot.ActualHeight;
            if (top > 0.0 && top != _oldTop)
            {
                LayoutRoot.ScrollToVerticalOffset(top);
                _oldTop = top;
            }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ICommand sendCommand = ForView.Unwrap<ChatViewModel>(DataContext).Send;
                if (sendCommand.CanExecute(null))
                {
                    sendCommand.Execute(null);
                    e.Handled = true;
                }
            }
        }
    }
}