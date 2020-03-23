using BarboraTimeCheck.Services;
using BarboraTimeCheck.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BarboraTimeCheck
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private SettingsService settingsService;
        public SettingsWindow()
        {
            InitializeComponent();

            settingsService = new SettingsService();
            var settings = settingsService.GetSettings();

            SetSettings(settings);
            if (settings.EmailNotifications)
            {
                emailNotifications_Checked(null, null);
            }
            else
            {
                emailNotifications_Unchecked(null, null);
            }
        }

        private void SetSettings(Settings settings)
        {
            refreshInterval.Text = settings.CheckInterval.ToString();
            pushNotifications.IsChecked = settings.PushNotifications;
            emailNotifications.IsChecked = settings.EmailNotifications;
            emailUsername.Text = settings.EmailUsername;
            emailPassword.Text = settings.EmailPassword;
            deliveryEmail.Text = settings.DeliveryEmail;
            emailSmtpServer.Text = settings.EmailSmtpServer;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (emailNotifications.IsChecked.Value)
            {
                if (!EmailFieldsValid())
                {
                    return;
                }
                settingsService.UpdateEmailInformation(emailUsername.Text, emailPassword.Text, emailSmtpServer.Text, deliveryEmail.Text);
            }
            settingsService.UpdateCommonInformation(int.Parse(refreshInterval.Text), emailNotifications.IsChecked.Value, pushNotifications.IsChecked.Value);
            
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private bool EmailFieldsValid()
        {
            return (!string.IsNullOrEmpty(emailUsername.Text) &&
                    !string.IsNullOrEmpty(emailPassword.Text) &&
                    !string.IsNullOrEmpty(deliveryEmail.Text) &&
                     !string.IsNullOrEmpty(emailSmtpServer.Text));
        }

        private void emailNotifications_Checked(object sender, RoutedEventArgs e)
        {
            emailUsernameGrid.Visibility = Visibility.Visible;
            emailPasswordGrid.Visibility = Visibility.Visible;
            emailSmtpGrid.Visibility = Visibility.Visible;
            emailDeliveryGrid.Visibility = Visibility.Visible;
        }

        private void emailNotifications_Unchecked(object sender, RoutedEventArgs e)
        {
            emailUsernameGrid.Visibility = Visibility.Collapsed;
            emailPasswordGrid.Visibility = Visibility.Collapsed;
            emailSmtpGrid.Visibility = Visibility.Collapsed;
            emailDeliveryGrid.Visibility = Visibility.Collapsed;
        }
    }
}
