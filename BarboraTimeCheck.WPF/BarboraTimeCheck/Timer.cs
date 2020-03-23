using BarboraTimeCheck.Services;
using Notifications.Wpf;
using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;

namespace BarboraTimeCheck
{
    public static class TimerManager
    {
        private static string _lastCheckString = "Not checked yet";

        public static string LastCheck
        {
            get
            {
                return _lastCheckString;
            }

            set
            {
                _lastCheckString = value;
                OnStaticPropertyChanged("LastCheck");
            }
        }

        private static int _deliveriesFound = 0;

        public static int DeliveriesFound
        {
            get
            {
                return _deliveriesFound;
            }

            set
            {
                _deliveriesFound = value;
                OnStaticPropertyChanged("DeliveriesFound");
            }
        }

        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged = delegate { };
        private static void OnStaticPropertyChanged(string staticPropertyName)
        {
            StaticPropertyChanged(null, new PropertyChangedEventArgs(staticPropertyName));
        }

        private static Timer gAppTimer;
        private static object lockObject = new object();
        private static SettingsService configurationService = new SettingsService();
        private static BarboraService barboraService = new BarboraService();
        private static NotificationManager notificationManager = new NotificationManager();
        private static EmailService emailService = new EmailService();

        public static void StartTimer()
        {          
            if (gAppTimer == null)
            {
                lock (lockObject)
                {
                    if (gAppTimer == null)
                    {
                        gAppTimer = new Timer(OnTimerTick, null, configurationService.GetInterval() * 1000, configurationService.GetInterval() * 1000);
                    }
                }
            }
        }

        public static void StopTimer()
        {
            if (gAppTimer != null)
            {
                lock (lockObject)
                {
                    if (gAppTimer != null)
                    {
                        gAppTimer.Change(Timeout.Infinite, Timeout.Infinite);
                        gAppTimer = null;
                    }
                }
            }
        }

        private static void OnTimerTick(object state)
        {
            Action();
        }

        public static void Action()
        {
            if (configurationService.AuthenticationExists())
            {
                LastCheck = DateTime.Now.ToString();

                var settings = configurationService.GetSettings();
                var deliveries = barboraService.GetAvailableDeliveries();

                if (!deliveries.Any())
                {
                    return;
                }

                DeliveriesFound = deliveries.Count;

                var deliveryText = new StringBuilder();
                deliveryText.AppendLine("Available delivery times:");
                foreach (var delivery in deliveries)
                {
                    if (settings.PushNotifications)
                    {
                        deliveryText.AppendLine($"Available delivery time: {delivery.deliveryTime}");
                        notificationManager.Show(new NotificationContent
                        {
                            Title = "Delivery time found",
                            Message = $"Found delivery time: {delivery.deliveryTime}",
                            Type = NotificationType.Information
                        }, expirationTime: System.TimeSpan.FromMinutes(60));
                    }
                }

                if (settings.EmailNotifications)
                {
                    emailService.SendEmail("Delivery", deliveryText.ToString());
                }
            }
        }
    }
}
