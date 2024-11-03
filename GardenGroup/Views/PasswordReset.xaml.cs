using System.Windows;
using System.Windows.Controls;
using DAL.Extensions;
using GardenGroup.ViewModels;
using Model;
using Service;

namespace GardenGroup.Views;

public partial class PasswordReset : UserControl
{
    private PasswordResetViewModel ViewModel => DataContext as PasswordResetViewModel ?? throw new NullReferenceException();
    private string? TargetEmail { get; set; }
    private Employee TargetEmployee { get; set; }
    
    private IServiceManager _serviceManager;
    
    public PasswordReset()
    {
        Loaded += (s, _) => PrepareView();
        InitializeComponent();
    }
    
    private void PrepareView() => _serviceManager = ViewModel.ServiceManager;
    
    private void StartReset()
    {
        string mail;
        string password;
        TargetEmail = EmailTxt.Text;
        
        if (string.IsNullOrEmpty(TargetEmail) || !IsValidEmail(TargetEmail))
        {
            MessageBox.Show("Please enter a valid email address.");
            return;
        }

        try
        {
            RetrieveSmtpCredentials(out mail, out password);
        }
        catch (Exception)
        {
            MessageBox.Show("Failed to retrieve SMTP data.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        SendPinGrid.Visibility = Visibility.Hidden;
        PinGrid.Visibility = Visibility.Visible;
        
        ResetSmtpPassword(mail, password);
        
    }
    
    private bool IsValidEmail(string email)
    {
        try
        {
            new System.Net.Mail.MailAddress(email);
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }

    private void RetrieveSmtpCredentials(out string mail, out string password)
    {
        var connection = System.Configuration.ConfigurationManager.ConnectionStrings["SMTP"].ConnectionString;
        if (connection == null)
            throw new Exception("SMTP connection string not found!");
            
        var split = connection.Split(':');
        mail = split[0];
        password = split[1];
    }

    private void ResetSmtpPassword(string mail, string password)
    {
        var random = new Random();
        var number = random.Next(0, 1000000);
        var pin = number.ToString("000000");

        if (!CreatePasswordReset(pin))
        {
            MessageBox.Show("Email does not exist.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        var smtp = new Service.Mail.Smtp(mail, password);
        if (!smtp.SendEmail(TargetEmail!, TargetEmployee.FullName, TargetEmail!))
            MessageBox.Show("Failed to send email.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    private bool CreatePasswordReset(string pin)
    {
        var employee = _serviceManager.EmployeeService.GetEmployeeByEmail(TargetEmail!);
        if(employee == null)
            return false;
        
        var passwordReset = new Model.PasswordReset(employee.Id, pin);
        _serviceManager.PasswordService.PasswordReset(passwordReset);

        TargetEmployee = employee;

        return true;
    }

    private void CheckPin()
    {
        var pin = PinTxt.Text;
        if (!_serviceManager.PasswordService.VerifyPasswordReset(TargetEmployee.Id, pin))
        {
            MessageBox.Show("Invalid PIN.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        
        PinGrid.Visibility = Visibility.Hidden;
        ChangePasswordGrid.Visibility = Visibility.Visible;
    }

    private void ChangePassword()
    {
        var password = PasswordBox.Password;
        var confirmedPassword = ConfirmPasswordBox.Password;

        if (confirmedPassword != password)
        {
            MessageBox.Show("Passwords do not match.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        var hash = Encryption.Encrypt(password);
        _serviceManager.EmployeeService.ChangePassword(TargetEmployee.Id, hash);
        _serviceManager.PasswordService.DeletePasswordReset(TargetEmployee.Id);
        
        ViewModel.SwitchToLogin();
    }
    

    private void SendPinBtn_OnClick(object sender, RoutedEventArgs e)
    {
        EmailTxt.IsEnabled = false;
        StartReset();
    }

    private void ResetPasswordBtn_OnClick(object sender, RoutedEventArgs e) => CheckPin();

    private void ChangePasswordBtn_OnClick(object sender, RoutedEventArgs e) => ChangePassword();
}