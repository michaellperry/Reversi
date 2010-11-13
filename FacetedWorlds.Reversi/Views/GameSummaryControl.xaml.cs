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
	public partial class GameSummaryControl : UserControl
	{
		public GameSummaryControl()
		{
			// Required to initialize variables
			InitializeComponent();
		}

        private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            GameSummaryViewModel viewModel = ForView.Unwrap<GameSummaryViewModel>(DataContext);
            if (viewModel != null && viewModel.OpenGame.CanExecute(null))
            {
                viewModel.OpenGame.Execute(null);
            }
        }
	}
}