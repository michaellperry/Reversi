using System.Windows.Controls;
using System.Windows.Input;
using FacetedWorlds.Reversi.ViewModels;
using UpdateControls.XAML;

namespace FacetedWorlds.Reversi.Views
{
	public partial class GameSummaryControl : UserControl
	{
		public GameSummaryControl()
		{
			// Required to initialize variables
			InitializeComponent();
		}

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ((Grid)sender).Background.Opacity = 0.3;
        }

        private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            GameSummaryViewModel viewModel = ForView.Unwrap<GameSummaryViewModel>(DataContext);
            if (viewModel != null && viewModel.OpenGame.CanExecute(null))
            {
                viewModel.OpenGame.Execute(null);
            }
            ((Grid)sender).Background.Opacity = 0.0;
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Grid)sender).Background.Opacity = 0.0;
        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Grid)sender).Background.Opacity = 0.3;
        }
	}
}