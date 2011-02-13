using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using FacetedWorlds.Reversi.Model;
using System.Linq;

namespace FacetedWorlds.Reversi.ViewModels
{
    public class SettingsViewModel
    {
        private Identity _identity;

        public SettingsViewModel(Identity identity)
        {
            _identity = identity;
        }

        public bool IsPushNotificationEnabled
        {
            get { return !_identity.IsToastNotificationDisabled.Any(); }
            set { _identity.EnableToastNotification(value); }
        }
    }
}
