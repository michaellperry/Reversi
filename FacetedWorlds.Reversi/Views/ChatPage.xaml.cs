using System.Windows.Navigation;
using FacetedWorlds.Reversi.ViewModels;
using Microsoft.Phone.Controls;
using UpdateControls.XAML;

namespace FacetedWorlds.Reversi.Views
{
    public partial class ChatPage : PhoneApplicationPage
    {
        public ChatPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            var viewModel = ForView.Unwrap<ChatViewModel>(DataContext);
            viewModel.OnClose();
            base.OnNavigatedFrom(e);
        }
    }
}
