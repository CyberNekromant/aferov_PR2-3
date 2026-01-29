using System.Windows;
using System.Windows.Controls;

namespace Auth.Pages
{
    public partial class AdminPage : Page
    {
        public AdminPage() { InitializeComponent(); }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new LoginPage()); 
            while (this.NavigationService.CanGoBack) { this.NavigationService.RemoveBackEntry(); }
            
        }
    }
}