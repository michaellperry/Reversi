using System.Windows.Controls;
using System.Windows.Input;
using FacetedWorlds.Reversi.ViewModels;
using UpdateControls.XAML;
using System.Windows;
using System.Windows.Media;

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
            VisualStateManager.GoToState(this, "Pressed", false);
            ContentPane.Background.Opacity = 0.3;
            Tilt(e.GetPosition(ContentPane));
        }

        private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            GameSummaryViewModel viewModel = ForView.Unwrap<GameSummaryViewModel>(DataContext);
            if (viewModel != null && viewModel.OpenGame.CanExecute(null))
            {
                viewModel.OpenGame.Execute(null);
            }
            Reset();
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            Reset();
        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            VisualStateManager.GoToState(this, "Pressed", false);
            ContentPane.Background.Opacity = 0.3;
            Tilt(e.GetPosition(ContentPane));
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            Tilt(e.GetPosition(ContentPane));
        }

        private void Tilt(Point finger)
        {
            double angleX = 30.0 * (finger.Y / ContentPane.ActualHeight) - 15.0;
            double angleY = -30.0 * (finger.X / ContentPane.ActualWidth) + 15.0;
            double zOffset = -25.0;

            SetProjection(angleX, angleY, zOffset);
        }

        private void Reset()
        {
            ContentPane.Background.Opacity = 0.0;
            VisualStateManager.GoToState(this, "Resting", true);
        }

        private void SetProjection(double angleX, double angleY, double zOffset)
        {
            PlaneProjection projection = ContentPane.Projection as PlaneProjection;
            if (projection == null)
            {
                projection = new PlaneProjection();
                ContentPane.Projection = projection;
            }
            projection.RotationX = angleX;
            projection.RotationY = angleY;
            projection.GlobalOffsetZ = zOffset;
        }
    }
}