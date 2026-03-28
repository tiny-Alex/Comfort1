using Comfort.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Comfort.Pages
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void EnterBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Login.Text) || string.IsNullOrEmpty(Password.Password))
            {
                MessageBox.Show("Заполните все поля!", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                var userObj = Connection.connect.Logins.FirstOrDefault(x => x.Login == Login.Text.Trim() && x.Password == Password.Password.Trim());
                if (userObj != null)
                {
                   CurrentUser.User = userObj;
                    NavigationService.Navigate(new ParthnerListPage());
                }
                else
                {
                     MessageBox.Show("Неверный логин или пароль!", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при подключении к бд! :{ex.Message}", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void RegistrBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegistrationPage());
        }
    }
}
