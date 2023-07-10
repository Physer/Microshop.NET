using FluentAssertions;
using Xunit;

namespace UnitTests;

public class Temporary
{
    [Fact]
    public void TemporaryTest_ReturnsTrue() => true.Should().BeTrue();
}
