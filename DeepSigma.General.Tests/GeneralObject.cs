using DeepSigma.General;

namespace DeepSigma.General.Tests;

public class GeneralObject : IJSONSerializer<GeneralObject>
{
    public int ID { get; set; }
    public string Name { get; set; }

    public GeneralObject()
    {
        
    }
}
