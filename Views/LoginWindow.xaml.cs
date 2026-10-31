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
using System.Windows.Shapes;
using AutoSchool.Services;

namespace AutoSchool.Views
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            AutoSchool.Data.Database.Initialize();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginBox.Text.Trim();
            string password = PasswordBox.Password;

            if (login.Length < 3 || password.Length < 6)
            {
                ErrorText.Text = "Логин от 3 символов, пароль от 6.";
                return;
            }

            if (AuthService.Login(login, password))
            {
                var main = new MainWindow();  
                main.Show();
                Close();
            }
            else
            {
                ErrorText.Text = "Неверный логин или пароль.";
            }
        }
    }
}
