using System.Windows;
using System.Windows.Controls;
using MediCoreHospital.Application.DTOs;
using Microsoft.Extensions.DependencyInjection;

namespace MediCoreHospital.UI.Views.Login;

public partial class LoginView : UserControl
{
    public LoginView()
    {
        InitializeComponent();
    }

    private async void LoginButton_Click(object sender, RoutedEventArgs e)
    {
        var username = UsernameTextBox.Text.Trim();
        var password = PasswordBox.Password;

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            StatusText.Text = "يرجى إدخال اسم المستخدم وكلمة المرور.";
            return;
        }

        try
        {
            var app = Application.Current as App;
            var authenticationService = app?.Services.GetService<MediCoreHospital.Application.Contracts.IAuthenticationService>();

            if (authenticationService is null)
            {
                StatusText.Text = "تعذر تهيئة الخدمات. يرجى التحقق من إعدادات التطبيق.";
                return;
            }

            var result = await authenticationService.LoginAsync(new LoginRequest
            {
                Username = username,
                Password = password,
                RememberMe = true
            });

            if (result.Success)
            {
                StatusText.Text = $"مرحباً {result.FullName} - {result.RoleName}";
                StatusText.Foreground = System.Windows.Media.Brushes.Green;
            }
            else
            {
                StatusText.Text = result.Message;
                StatusText.Foreground = System.Windows.Media.Brushes.Red;
            }
        }
        catch (Exception ex)
        {
            StatusText.Text = "تعذر الاتصال بقاعدة البيانات. يرجى التأكد من تشغيل SQL Server.";
            System.Diagnostics.Debug.WriteLine(ex.Message);
        }
    }
}
