using DeepSigma.General.Enums;

namespace DeepSigma.General.Inventory;

/// <summary>
/// Factory for creating inventory collections based on accounting cost methodology
/// </summary>
/// <typeparam name="T"></typeparam>
public static class InventoryCollectionFactory<T>
{
    /// <summary>
    /// Creates an inventory collection based on the specified accounting cost methodology
    /// </summary>
    /// <param name="costMethodology"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static InventoryCollectionAbstract<T> Create(AccountingCostMethodology costMethodology)
    {
        return costMethodology switch
        {
            AccountingCostMethodology.FirstInFirstOut => new InventoryQueueCollection<T>(),
            AccountingCostMethodology.LastInLastOut => new InventoryStackCollection<T>(),
            _ => throw new NotImplementedException(),
        };
    }
}
