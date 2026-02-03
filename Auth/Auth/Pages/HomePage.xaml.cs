using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Auth.Pages
{
    public partial class HomePage : Page
    {
        
        public HomePage(string login = "Гость")
        {
            InitializeComponent();
            
        }

       

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
             if (this.NavigationService.CanGoBack) 
    {
                 this.NavigationService.GoBack();
    }
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
             this.NavigationService.Navigate(new LoginPage()); 
     while (this.NavigationService.CanGoBack) 
    {
                this.NavigationService.RemoveBackEntry(); 
    }
        }
    }
}