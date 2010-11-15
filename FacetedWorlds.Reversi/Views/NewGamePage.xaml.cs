using Microsoft.Phone.Controls;

namespace FacetedWorlds.Reversi.Views
{
    public partial class NewGamePage : PhoneApplicationPage
    {
        public NewGamePage()
        {
            InitializeComponent();
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (yourNameControl.HandleBack())
                e.Cancel = true;
        }
    }
}
