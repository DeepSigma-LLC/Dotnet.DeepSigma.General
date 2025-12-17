using DeepSigma.General.DateObjects;
using Xunit;

namespace DeepSigma.General.Tests.Tests;

public class DateTime_Tests
{
    [Fact]
    public void DateOnly_ToDateTime_Conversion_Works()
    {
        DateOnlyCustom date = new(DateOnly.Parse("1/1/2025"));
        DateOnlyCustom date2 = new(DateOnly.Parse("1/2/2025"));

        TimeSpan result = date - date2;
        Assert.Equal(-1, result.Days);
    }

    [Fact]
    public void DateOnly_Comparisons()
    {
        DateOnlyCustom date = new(DateOnly.Parse("1/1/2025"));
        DateOnlyCustom date2 = new(DateOnly.Parse("1/2/2025"));
        DateOnlyCustom date3 = new(DateOnly.Parse("1/3/2025"));
        DateOnlyCustom date4 = new(DateOnly.Parse("1/3/2025"));
        Assert.True(date < date2);
        Assert.True(date3 > date2);
        Assert.True(date != date3);
        Assert.True(date3 == date4);
    }
}
