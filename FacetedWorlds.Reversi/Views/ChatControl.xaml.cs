using System.Windows.Controls;

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
    }
}