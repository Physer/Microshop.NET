namespace Application.DataGeneration;

public interface IApiClient
{
    Task ClearData();
    Task GenerateProducts();
}
