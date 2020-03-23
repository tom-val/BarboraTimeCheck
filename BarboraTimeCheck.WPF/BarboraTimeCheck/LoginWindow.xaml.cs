using BarboraTimeCheck.Services;
using System;
using System.Collections.Generic;
using System.Text;
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
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButtonClick(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            var barboraService = new BarboraService();
            var settingsService = new SettingsService();
            try
            {
                var email = textBoxEmail.Text;
                var authCookie = barboraService.Login(textBoxEmail.Text, passwordBox.Password);

                settingsService.UpdateAuthInformation(textBoxEmail.Text, passwordBox.Password, authCookie);
                var mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            catch(Exception ex)
            {
                errormessage.Text = ex.Message;
            }
            
            Mouse.OverrideCursor = Cursors.Arrow;
        }
    }
}
