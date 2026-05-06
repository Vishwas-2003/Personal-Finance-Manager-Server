using WebApp.Common.Enums;

namespace UnitTests;

public class SmokeTests
{
    [Fact]
    public void CategoryType_has_expected_members()
    {
        Assert.Equal(5, Enum.GetValues<CategoryType>().Length);
    }
}
