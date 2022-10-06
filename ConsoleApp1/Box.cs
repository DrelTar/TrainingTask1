using System;
using System.Collections.Generic;

/// <summary>
/// Class for box of vegetables.
/// </summary>
public class Box
{
    // Price of box.
    private uint _price;
    // Weight of box.
    private uint _weight;

    /// <summary>
    /// Create box.
    /// </summary>
    /// <param name="price"> Price of box. </param>
    /// <param name="weight"> Weight of box. </param>
    public Box(uint price, uint weight)
    {
        this._price = price;
        this._weight = weight;
    }

    /// <summary>
    /// Property for _price field. Only get available.
    /// </summary>
    public uint Price {
        get
        {
            return _price;
        }
    }

    /// <summary>
    /// Property for _weight field. Only get available.
    /// </summary>
    public uint Weight
    {
        get
        {
            return _weight;
        }
    }
}