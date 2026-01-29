using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Auth.Pages
{
    public partial class LoginPage : Page
    {
        private int attempts = 0;
        private DispatcherTimer timer = new DispatcherTimer();
        private int timeLeft = 10;

        public LoginPage()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
             string login = TbLogin.Text.Trim(); 
            string password = CbShowPass.IsChecked == true ? TbPasswordView.Text : PbPassword.Password; 

            if (login == "admin" && password == "1234")
            {
                attempts = 0;
                 this.NavigationService.Navigate(new AdminPage()); 
            }
            else if (login == "user" && password == "1111")
            {
                attempts = 0;
                this.NavigationService.Navigate(new HomePage()); 
            }
            else
            {
                attempts++;
                if (attempts >= 3)
                {
                    BtnLogin.IsEnabled = false; 
                    timer.Start();
                     MessageBox.Show("Превышено количество попыток! Блокировка 10 сек."); 
                }
                else { MessageBox.Show("Неверный логин или пароль!"); }
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (--timeLeft <= 0)
            {
                timer.Stop();
                BtnLogin.IsEnabled = true;
                BtnLogin.Content = "Войти";
                timeLeft = 10;
                attempts = 0;
            }
            else { BtnLogin.Content = $"Ждите {timeLeft}с"; }
        }

        private void CbShowPass_Toggle(object sender, RoutedEventArgs e)
        {
            if (CbShowPass.IsChecked == true)
            {
                TbPasswordView.Text = PbPassword.Password;
                PbPassword.Visibility = Visibility.Collapsed;
                TbPasswordView.Visibility = Visibility.Visible;
            }
            else
            {
                PbPassword.Password = TbPasswordView.Text;
                TbPasswordView.Visibility = Visibility.Collapsed;
                PbPassword.Visibility = Visibility.Visible;
            }
        }
    }
}