using System;
using System.Data.SqlClient;
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
            string password = PbPassword.Password;

            // Database connection string - adjust according to your setup
            string connectionString = "Data Source=.;Initial Catalog=AuthNavDb;Integrated Security=True";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Create SQL query to check credentials
                    string query = "SELECT Role FROM Users WHERE Login = @login AND Password = @password";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add parameters to prevent SQL injection
                        command.Parameters.AddWithValue("@login", login);
                        command.Parameters.AddWithValue("@password", password);

                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null)
                        {
                            // Valid credentials - get the user role
                            string role = result.ToString();

                            attempts = 0;  // Reset attempts on successful login

                            // Navigate based on user role
                            if (role == "Admin")
                            {
                                this.NavigationService.Navigate(new AdminPage());
                            }
                            else
                            {
                                this.NavigationService.Navigate(new HomePage());
                            }
                        }
                        else
                        {
                            // Invalid credentials
                            attempts++;
                            if (attempts >= 3)
                            {
                                BtnLogin.IsEnabled = false;
                                timer.Start();
                                MessageBox.Show("Превышено количество попыток! Блокировка 10 сек.");
                            }
                            else
                            {
                                MessageBox.Show("Неверный логин или пароль!");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка подключения к базе данных: " + ex.Message);
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