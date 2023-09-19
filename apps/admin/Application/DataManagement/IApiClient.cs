namespace Application.DataManagement;

public interface IApiClient
{
    Task ClearData();
    Task GenerateProducts();
}
