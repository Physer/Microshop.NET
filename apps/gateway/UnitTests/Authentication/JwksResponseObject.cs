namespace UnitTests.Authentication;

public class JwksResponseObject
{
    public required Key[] Keys { get; init; }

}
public class Key
{
    public required string Kty { get; init; }
    public required string Kid { get; init; }
    public required string N { get; init; }
    public required string E { get; init; }
    public required string Alg { get; init; }
    public required string Use { get; init; }
}
