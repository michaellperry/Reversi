using System;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using FacetedWorlds.Reversi.Model;
using FacetedWorlds.Reversi.ViewModel;
using UpdateControls.Correspondence;
using UpdateControls.Correspondence.SSCE;
using UpdateControls.Correspondence.WebServiceClient;
using UpdateControls.XAML;
using UpdateControls.Correspondence.Memory;

namespace FacetedWorlds.Reversi
{
    public partial class App : Application
    {
        private Community _community;
        private MachineViewModel _machineViewModel;
        private SynchronizationThread _synchronizationThread;

        private IDisposable _currentDuration;

        private void Application_Startup(
            object sender, StartupEventArgs e)
        {
            // Load the correspondence database.
            string databaseFileName = Directory.ApplicationData / "FacetedWorlds" / "Reversi" / "Correspondence.sdf";

            _community = new Community(new SSCEStorageStrategy(databaseFileName))
                .AddCommunicationStrategy(new WebServiceCommunicationStrategy())
                .Register<CorrespondenceModule>()
                .Subscribe(() => _machineViewModel.User)
                .Subscribe(() => _machineViewModel.User == null
                    ? Enumerable.Empty<Game>()
                    : _machineViewModel.User.ActivePlayers.Select(player => player.Game));

            // Recycle durations when the application is idle.
            DispatcherTimer timer = new DispatcherTimer(DispatcherPriority.ApplicationIdle);
            timer.Interval = TimeSpan.FromSeconds(1.0);
            timer.Tick += Timer_Tick;
            timer.Start();

            BeginDuration();

            // Display the main window.
            MainWindow = new View.MainWindow();
            MachineNavigationModel navigation = new MachineNavigationModel();
            _machineViewModel = new MachineViewModel(_community, navigation);
            MainWindow.DataContext = ForView.Wrap(_machineViewModel);
            MainWindow.Show();

            _synchronizationThread = new SynchronizationThread(_community);
            _community.FactAdded += () => _synchronizationThread.Wake();
            _synchronizationThread.Start();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            EndDuration();
            _synchronizationThread.Stop();
        }

        void Timer_Tick(object sender, EventArgs e)
        {
            // End the last duration and begin the next.
            EndDuration();
            BeginDuration();
        }

        private void BeginDuration()
        {
            if (_currentDuration == null)
            {
                _currentDuration = _community.BeginDuration();
            }
        }

        private void EndDuration()
        {
            if (_currentDuration != null)
            {
                _currentDuration.Dispose();
                _currentDuration = null;
            }
        }
    }
}
