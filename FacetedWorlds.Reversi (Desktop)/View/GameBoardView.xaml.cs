using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FacetedWorlds.Reversi.ViewModel;
using UpdateControls.XAML;

namespace FacetedWorlds.Reversi.View
{
    /// <summary>
    /// Interaction logic for GameBoardView.xaml
    /// </summary>
    public partial class GameBoardView : UserControl
    {
        public GameBoardView()
        {
            InitializeComponent();
        }

        private void Square_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement control = sender as FrameworkElement;
            if (control != null)
            {
                SquareViewModel viewModel = ForView.Unwrap<SquareViewModel>(control.DataContext);
                if (viewModel != null)
                    viewModel.OnClick();
            }
        }
    }
}
