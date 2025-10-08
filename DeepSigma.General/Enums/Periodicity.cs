﻿
namespace DeepSigma.General.Enums;

/// <summary>
/// Represents the time period periodicity.
/// </summary>
public enum Periodicity
{
    /// <summary>
    /// Daily periodicity.
    /// </summary>
    Daily = 0,
    /// <summary>
    /// Weekly periodicity.
    /// </summary>
    Weekly = 1,
    /// <summary>
    /// Monthly periodicity.
    /// </summary>
    Monthly = 2,
    /// <summary>
    /// Quarterly periodicity.
    /// </summary>
    Quarterly = 3,
    /// <summary>
    /// Semi-annual periodicity.
    /// </summary>
    SemiAnnual = 4,
    /// <summary>
    /// Annually periodicity.
    /// </summary>
    Annually = 5,
    /// <summary>
    /// Intraday periodicity, typically used for high-frequency trading or minute-by-minute data.
    /// </summary>
    Intraday = 6,
}