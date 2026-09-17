using CommunityToolkit.Mvvm.ComponentModel;
using MediCoreHospital.Application.Contracts;
using MediCoreHospital.Application.DTOs;
using System.Collections.ObjectModel;

namespace MediCoreHospital.UI.ViewModels.Appointments;

public partial class AppointmentsViewModel : ObservableObject
{
    private readonly IAppointmentService _appointmentService;
    [ObservableProperty] private DateTime selectedDate = DateTime.Today;
    [ObservableProperty] private bool isLoading;
    [ObservableProperty] private string errorMessage = string.Empty;
    [ObservableProperty] private string successMessage = string.Empty;
    public ObservableCollection<AppointmentDto> Appointments { get; } = new();

    public AppointmentsViewModel(IAppointmentService appointmentService) => _appointmentService = appointmentService;

    public async Task LoadAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            IsLoading = true;
            ErrorMessage = string.Empty;
            var items = await _appointmentService.GetByDateAsync(SelectedDate, cancellationToken);
            Appointments.Clear();
            foreach (var item in items) Appointments.Add(item);
        }
        catch (OperationCanceledException) { }
        catch (Exception) { ErrorMessage = "تعذر تحميل مواعيد اليوم. تحقق من اتصال قاعدة البيانات."; }
        finally { IsLoading = false; }
    }
}
