using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using UpdateControls.XAML;
using FacetedWorlds.Reversi.ViewModels;

namespace FacetedWorlds.Reversi.Views
{
	public partial class ChatControl : UserControl
	{
		public ChatControl()
		{
			// Required to initialize variables
			InitializeComponent();
		}

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ((TextBox)sender).GetBindingExpression(TextBox.TextProperty).UpdateSource();
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            ForView.Unwrap<ChatViewModel>(DataContext).Send.Execute(null);
        }
    }
}