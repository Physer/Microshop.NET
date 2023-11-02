namespace Application.DataManagement;

public interface IApiClient
{
    Task<HttpResponseMessage> ClearDataAsync();
    Task<HttpResponseMessage> GenerateProductsAsync();
}
