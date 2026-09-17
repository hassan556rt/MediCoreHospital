using CommunityToolkit.Mvvm.ComponentModel;
using MediCoreHospital.Application.Contracts;
using MediCoreHospital.Application.DTOs;

namespace MediCoreHospital.UI.ViewModels.Dashboard;

public partial class DashboardViewModel : ObservableObject
{
    private readonly IDashboardService _dashboardService;

    [ObservableProperty] private bool isLoading;
    [ObservableProperty] private string errorMessage = string.Empty;
    [ObservableProperty] private DashboardStatistics statistics = new();
    [ObservableProperty] private IReadOnlyList<UpcomingAppointment> upcomingAppointments = Array.Empty<UpcomingAppointment>();

    public DashboardViewModel(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    public async Task LoadAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            IsLoading = true;
            ErrorMessage = string.Empty;
            Statistics = await _dashboardService.GetStatisticsAsync(cancellationToken);
            UpcomingAppointments = await _dashboardService.GetUpcomingAppointmentsAsync(8, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            // Cancellation is expected when the view closes or a refresh is superseded.
        }
        catch (Exception)
        {
            ErrorMessage = "تعذر تحميل بيانات لوحة التحكم. يرجى التحقق من اتصال قاعدة البيانات.";
        }
        finally
        {
            IsLoading = false;
        }
    }
}
