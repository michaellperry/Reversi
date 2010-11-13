using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FacetedWorlds.Reversi.ViewModels;
using UpdateControls;
using UpdateControls.XAML;
using System.Windows.Threading;
using System;

namespace FacetedWorlds.Reversi.Views
{
	public partial class GameControl : UserControl
	{
        private const int SquareSize = 57;
        private static Point NewPieceRestingPosition = new Point(200, 473);
        private static Point FloatingPiecePosition = new Point(26, -27);

        private bool _dragging = false;
        private Point _relativeToNewPiece;

        private Dependent _depNewPieceState;

		public GameControl()
		{
            _depNewPieceState = new Dependent(UpdateNewPieceState);
            _depNewPieceState.Invalidated += TriggerUpdateNewPieceState;
            TriggerUpdateNewPieceState();

			// Required to initialize variables
			InitializeComponent();
		}

        private void NewPiece_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            VisualStateManager.GoToState(this, "PickedUp", true);
            _relativeToNewPiece = e.GetPosition(this).Minus(NewPieceRestingPosition);
            _dragging = true;
            CaptureMouse();
            e.Handled = true;
        }

        private void NewPiece_MouseMove(object sender, MouseEventArgs e)
        {
            if (_dragging)
            {
                Point newPoint = e.GetPosition(this).Minus(_relativeToNewPiece);
                Canvas.SetLeft(NewPiece, newPoint.X);
                Canvas.SetTop(NewPiece, newPoint.Y);

                Point hitPoint = newPoint.Plus(FloatingPiecePosition);
                int hitRow = (int)(hitPoint.Y) / SquareSize;
                int hitColumn = (int)(hitPoint.X) / SquareSize;
                ViewModel.PreviewMove(hitRow, hitColumn);
            }
        }

        private void NewPiece_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_dragging)
            {
                // Make the move.
                Point hitPoint = e.GetPosition(this).Minus(_relativeToNewPiece).Plus(FloatingPiecePosition);
                int hitRow = (int)(hitPoint.Y) / SquareSize;
                int hitColumn = (int)(hitPoint.X) / SquareSize;
                ViewModel.MakeMove(hitRow, hitColumn);

                // Commit the move in 3 seconds.
                DispatcherTimer timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(3.0) };
                timer.Tick += delegate(object s2, EventArgs e2)
                {
                    if (ViewModel != null)
                        ViewModel.CommitMove();
                    timer.Stop();
                };
                timer.Start();

                // Reset the new piece.
                Reset();
            }
            e.Handled = true;
        }

        private void NewPiece_MouseLeave(object sender, MouseEventArgs e)
        {
            if (_dragging)
            {
                Reset();
            }
        }

        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.CancelMove();
        }

        private void TriggerUpdateNewPieceState()
        {
            Dispatcher.BeginInvoke(() => _depNewPieceState.OnGet());
        }

        private void UpdateNewPieceState()
        {
            if (ViewModel != null)
            {
                bool myTurn = ViewModel.MyTurn;
                VisualStateManager.GoToState(this, myTurn ? "MyTurn" : "NotMyTurn", myTurn);
            }
        }

        private void Reset()
        {
            ViewModel.ClearPreviewMove();
            bool myTurn = ViewModel.MyTurn;
            VisualStateManager.GoToState(this, myTurn ? "MyTurn" : "NotMyTurn", myTurn);
            Canvas.SetLeft(NewPiece, NewPieceRestingPosition.X);
            Canvas.SetTop(NewPiece, NewPieceRestingPosition.Y);
            _dragging = false;
            ReleaseMouseCapture();
        }

        private GameViewModel ViewModel
        {
            get { return ForView.Unwrap<GameViewModel>(DataContext); }
        }
    }
}