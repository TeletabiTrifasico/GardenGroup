using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GardenGroup.ViewModels;

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
            
            var employee = ((LoginViewModel)DataContext).Login(username, password);
            
            if (employee != null)
            {
                ((LoginViewModel)DataContext).SetLoggedInEmployee(employee);
                ((LoginViewModel)DataContext).LoginCommand.Execute(this);
            }
            else
            {
                errorLabel.Content = "Invalid username or password!";
                errorLabel.Visibility = Visibility.Visible;
            }
        }
        
        private void Control_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ((LoginViewModel)DataContext).ResetPasswordCommand.Execute(this);
        }
    }
}
