namespace Application.DataManagement;

public interface IApiClient
{
    Task ClearDataAsync();
    Task GenerateProductsAsync();
}
