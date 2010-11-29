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
	public partial class SquareControl : UserControl
	{
        public SquareControl()
		{
			// Required to initialize variables
			InitializeComponent();
		}

        private void Square_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ForView.Unwrap<ISquareViewModel>(DataContext).MakeMove();
            this.GetAncestorOfType<GameControl>().CommitMoveInThreeSeconds();
        }
	}
}