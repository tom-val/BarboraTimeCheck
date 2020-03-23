using BarboraTimeCheck.Services;
using Notifications.Wpf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace BarboraTimeCheck
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var configurationService = new SettingsService();

            if (configurationService.AuthenticationExists())
            {
                var mainWindow = new MainWindow();
                mainWindow.Show();
            }
            else
            {
                var loginWindow = new LoginWindow();
                loginWindow.Show();
            }

            TimerManager.StartTimer();
        }
    }
}
