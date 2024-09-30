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
using GardenGroup.ViewModels;
using Service;

namespace GardenGroup.Views
{
    /// Interaction logic for Login.xaml
    public partial class Login : UserControl
    {
        public Login()
        {
            InitializeComponent();
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            var username = usernameTxt.Text;
            var password = passwordTxt.Password;

            if(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                errorLabel.Content = "Username or password is missing!";
                errorLabel.Visibility = Visibility.Visible;
            }
            
            // Add login using database
            if (((LoginViewModel)DataContext).ValidateLogin(username, password))
                ((LoginViewModel)DataContext).LoginCommand.Execute(this);
            else if (password != "password" && username != "admin")
            {
                errorLabel.Content = "Invalid username or password!";
                errorLabel.Visibility = Visibility.Visible;
            }
            else if (password != "password")
            {
                errorLabel.Content = "Invalid password!";
                errorLabel.Visibility = Visibility.Visible;
            }
        }
    }
}
