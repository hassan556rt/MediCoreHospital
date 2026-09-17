using CommunityToolkit.Mvvm.ComponentModel;
using MediCoreHospital.Application.Contracts;
using MediCoreHospital.Application.DTOs;
using System.Collections.ObjectModel;

namespace MediCoreHospital.UI.ViewModels.Billing;

public partial class BillingViewModel : ObservableObject
{
    private readonly IInvoiceService _invoiceService;

    [ObservableProperty] private bool isLoading;
    [ObservableProperty] private string errorMessage = string.Empty;
    public ObservableCollection<InvoiceDto> Invoices { get; } = new();

    public BillingViewModel(IInvoiceService invoiceService) => _invoiceService = invoiceService;

    public async Task LoadAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            IsLoading = true;
            ErrorMessage = string.Empty;
            var items = await _invoiceService.GetOpenInvoicesAsync(cancellationToken);
            Invoices.Clear();
            foreach (var item in items) Invoices.Add(item);
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception)
        {
            ErrorMessage = "تعذر تحميل فواتير المرضى. تحقق من اتصال قاعدة البيانات.";
        }
        finally
        {
            IsLoading = false;
        }
    }
}
