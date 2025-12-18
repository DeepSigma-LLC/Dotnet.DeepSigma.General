namespace DeepSigma.General.Tests.Models;

internal class TradeTest()
{
    private readonly AbsoluteValue<decimal> _quantity = new();

    internal decimal Quantity
    {
        get => _quantity.Value;
        set => _quantity.Value = value;
    }
}
