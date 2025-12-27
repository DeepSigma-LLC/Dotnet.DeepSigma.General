using Xunit;
using DeepSigma.General.Extensions;
using DeepSigma.General.DateTimeUnification;
using DeepSigma.General.TimeStepper;

namespace DeepSigma.General.Tests.Tests.Extension;

public class Dictionary_Tests
{
    [Fact]
    public void Test_SortedDictionary_Roll()
    {
        // Arrange
        SortedDictionary<DateOnlyCustom, decimal?> dict = new()
        {
            { new DateOnlyCustom(2025, 12, 10), 100.0m },
            { new DateOnlyCustom(2025, 12, 11), 200.0m },
            { new DateOnlyCustom(2025, 12, 30), 300.0m }
        };

        PeriodicityConfiguration configuration = new(Enums.Periodicity.Daily, Enums.DaySelectionType.Any);
        var results = dict.FillMissingValuesByRolling(new SelfAligningTimeStepper<DateOnlyCustom>(configuration));

        Assert.Equal(21, results.Count);
        Assert.Equal(100.0m, results[new DateOnlyCustom(2025, 12, 10)]);
        Assert.Equal(200.0m, results[new DateOnlyCustom(2025, 12, 11)]);
        Assert.Equal(200.0m, results[new DateOnlyCustom(2025, 12, 12)]);
        Assert.Equal(200.0m, results[new DateOnlyCustom(2025, 12, 13)]);
        Assert.Equal(200.0m, results[new DateOnlyCustom(2025, 12, 14)]);
        Assert.Equal(200.0m, results[new DateOnlyCustom(2025, 12, 15)]);
        Assert.Equal(300.0m, results[new DateOnlyCustom(2025, 12, 30)]);
    }

    [Fact]
    public void Test_SortedDictionary_FillWithNull()
    {
        // Arrange
        SortedDictionary<DateOnlyCustom, decimal?> dict = new()
        {
            { new DateOnlyCustom(2025, 12, 10), 100.0m },
            { new DateOnlyCustom(2025, 12, 11), 200.0m },
            { new DateOnlyCustom(2025, 12, 30), 300.0m }
        };

        PeriodicityConfiguration configuration = new(Enums.Periodicity.Daily, Enums.DaySelectionType.Any);
        var results = dict.FillMissingValuesWithNull(new SelfAligningTimeStepper<DateOnlyCustom>(configuration));

        Assert.Equal(21, results.Count);
        Assert.Equal(100.0m, results[new DateOnlyCustom(2025, 12, 10)]);
        Assert.Equal(200.0m, results[new DateOnlyCustom(2025, 12, 11)]);
        Assert.Null(results[new DateOnlyCustom(2025, 12, 12)]);
        Assert.Null(results[new DateOnlyCustom(2025, 12, 13)]);
        Assert.Null(results[new DateOnlyCustom(2025, 12, 14)]);
        Assert.Null(results[new DateOnlyCustom(2025, 12, 15)]);
        Assert.Equal(300.0m, results[new DateOnlyCustom(2025, 12, 30)]);
    }
}
