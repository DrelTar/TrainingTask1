using System;
using System.Collections.Generic;

/// <summary>
/// Class for container for boxes.
/// </summary>
public class Container
{
    // Max weight of boxes in container.
    private uint _maxWeight;
    // Current weight of boxes.
    private uint _currentWeight;
    // Total price of boxes.
    private uint _totalPrice;
    // List of boxes.
    private List<Box> _boxes;

    /// <summary>
    /// Create container for boxes.
    /// </summary>
    /// <param name="maxWeight"> Generated randomly outside of this class. </param>
    /// <param name="listOfBoxes"> 2D array of boxes which will be in container. </param>
    public Container(uint maxWeight, uint[,] listOfBoxes)
    {
        // Setting initial values.
        _maxWeight = maxWeight;
        _currentWeight = 0;
        _totalPrice = 0;
        _boxes = new List<Box>();
        for (int i = 0; i < listOfBoxes.GetLength(0); ++i)
        {
            // Checking if box exist and normal.
            if (listOfBoxes[i, 0] != 0 || listOfBoxes[i, 1] != 0)
            {
                AddBox(new Box(listOfBoxes[i, 0], listOfBoxes[i, 1]));
            }
        }
    }

    /// <summary>
    /// Property for _totalPrice field. Only get available.
    /// </summary>
    public uint TotalPrice
    {
        get
        {
            return _totalPrice;
        }
    }

    /// <summary>
    /// Property for _currentWeight field. Only get available.
    /// </summary>
    public uint CurrentWeight
    {
        get
        {
            return _currentWeight;
        }
    }

    /// <summary>
    /// Property for _boxes field. Only get available.
    /// </summary>
    public List<Box> Boxes
    {
        get
        {
            return _boxes;
        }
    }

    /// <summary>
    /// Add box to container and check if it fit in weight.
    /// </summary>
    /// <param name="box"></param>
    public void AddBox(Box box)
    {
        // Checking weight.
        if (box.Weight + _currentWeight <= _maxWeight)
        {
            _boxes.Add(box);
            _currentWeight += box.Weight;
            _totalPrice += box.Price;
        }
    }
}