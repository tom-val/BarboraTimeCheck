using BarboraTimeCheck.Services;
using Hardcodet.Wpf.TaskbarNotification;
using log4net;
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
        private static readonly ILog log = LogManager.GetLogger(typeof(App));
        private TaskbarIcon tb;

        protected override void OnStartup(StartupEventArgs e)
        {
            log.Info("        =============  Started Logging  =============        ");
            base.OnStartup(e);
        }

        private void InitApplication()
        {
            //initialize NotifyIcon
            tb = (TaskbarIcon)FindResource("NotifyIcon");
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {     
            InitializeWindow();
            TimerManager.StartTimer();
        }

        private void InitializeWindow()
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
        }

        private void TaskbarIcon_TrayMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            InitializeWindow();
        }
    }
}
