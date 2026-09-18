using System.Windows;
using System.Windows.Input;
using MediCoreHospital.Application.Contracts;
using MediCoreHospital.Application.DTOs;
using MediCoreHospital.UI.Views.Login;
using Microsoft.Extensions.DependencyInjection;

namespace MediCoreHospital.UI.Views.Login;

public partial class LoginWindow : Window
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IServiceProvider _services;

    public LoginWindow(IAuthenticationService authenticationService, IServiceProvider services)
    {
        InitializeComponent();
        _authenticationService = authenticationService;
        _services = services;
    }

    private async void LoginButton_Click(object sender, RoutedEventArgs e) => await LoginAsync();

    private async void PasswordBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter) await LoginAsync();
    }

    private async Task LoginAsync()
    {
        if (string.IsNullOrWhiteSpace(UsernameTextBox.Text) || string.IsNullOrWhiteSpace(PasswordBox.Password))
        {
            StatusText.Text = "يرجى إدخال اسم المستخدم وكلمة المرور.";
            return;
        }

        LoginButton.IsEnabled = false;
        LoadingBar.Visibility = Visibility.Visible;
        StatusText.Text = string.Empty;
        try
        {
            var result = await _authenticationService.LoginAsync(new LoginRequest
            {
                Username = UsernameTextBox.Text.Trim(),
                Password = PasswordBox.Password,
                RememberMe = RememberCheckBox.IsChecked == true
            });

            if (!result.Success)
            {
                StatusText.Text = result.Message;
                return;
            }

            var shell = ActivatorUtilities.CreateInstance<MainWindow>(_services, result);
            Application.Current.MainWindow = shell;
            shell.Show();
            Close();
        }
        catch (Exception)
        {
            StatusText.Text = "تعذر الاتصال بقاعدة البيانات. تحقق من تشغيل SQL Server وإعدادات الاتصال.";
        }
        finally
        {
            LoginButton.IsEnabled = true;
            LoadingBar.Visibility = Visibility.Collapsed;
        }
    }
}
