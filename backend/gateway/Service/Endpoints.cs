namespace Service;

public static class Endpoints
{
    public static async Task<IResult> GenerateProducts() => await Task.FromResult(Results.Accepted());
}
