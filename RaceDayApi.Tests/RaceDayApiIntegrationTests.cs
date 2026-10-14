using FluentAssertions;

public class RaceDayApiBasicTests
{
    [Fact]
    public void True_ShouldBeTrue()
    {
        // Unit test to make the xUnit 
        bool testResult = true;
        testResult.Should().BeTrue();
    }
}