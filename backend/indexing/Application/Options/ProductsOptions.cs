namespace Application.Options;

public class ProductsOptions
{
    public const string ConfigurationEntry = "Products";

    public string? BaseUrl { get; set; }
    public string? ManagementUsername { get; set; }
    public string? ManagementPassword { get; set; }
}
