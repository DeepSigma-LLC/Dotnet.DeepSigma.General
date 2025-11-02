using Xunit;

namespace DeepSigma.General.Tests.Tests;

public class IJSONSerializer_Test
{
    [Fact]
    public void SerializableTest()
    {
        int Id = 1;
        GeneralObject obj = new() { 
            ID = Id,
            Name = "test",
        };

        string json = obj.ToJSON();
        GeneralObject? new_object = GeneralObject.Create(json);

        Assert.NotNull(new_object);
        Assert.True(new_object.ID == Id);
        Assert.NotNull(new_object.Name);
    }
}
