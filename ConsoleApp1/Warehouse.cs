using System;
using System.Collections.Generic;

/// <summary>
/// Class for warehouse for containers.
/// </summary>
class Warehouse
{
    // Max amount of containers in warehouse.
    private uint _maxSize;
    // Current amount of containers in warehouse.
    private uint _currentSize;
    // Fee for each container in warehouse.
    private uint _fee;
    // List of all containers.
    private List<Container> _containers;

    /// <summary>
    /// Create warehouse.
    /// </summary>
    /// <param name="size"> Max amount of containers. </param>
    /// <param name="fee"> Fee for each container. </param>
    public Warehouse(uint size, uint fee)
    {
        _maxSize = size;
        _currentSize = 0;
        _fee = fee;
        _containers = new List<Container>();
    }

    /// <summary>
    /// Property for _currentSize field. Only get available.
    /// </summary>
    public uint CurrentSize
    {
        get
        {
            return _currentSize;
        }
    }
    
    /// <summary>
    /// Property for _containers field. Only get avaialble.
    /// </summary>
    public List<Container> Containers
    {
        get
        {
            return _containers;
        }
    }

    /// <summary>
    /// Delete container form warehouse.
    /// </summary>
    /// <param name="index"> Index of delete container in _containers field. </param>
    public void DeleteContainer(uint index)
    {
        // Changing every container from index to last-1 on next container.
        _currentSize--;
        for (int i = (int)index; i < _currentSize; ++i)
        {
            _containers[i] = _containers[i + 1];
        }
        // Deleting last container.
        _containers.RemoveAt((int)_currentSize);
    }

    /// <summary>
    /// Add container to _containers field.
    /// </summary>
    /// <param name="container"> Container to add. </param>
    /// <returns> True if container added, False if it's unrenatble. </returns>
    public bool AddContainer(Container container)
    {
        // Creating random fee.
        Random rand = new Random();
        // Double damage. Heh. 
        double damage = rand.NextDouble() / 2;
        // Checking if rentable.
        if (container.TotalPrice * (1 - damage) > _fee)
        {
            _containers.Add(container);
            _currentSize++;
            // If warehouse is full deleting last.
            if (_currentSize > _maxSize)
            {
                DeleteContainer(0);
                // If you say me why _currentSize--; didn't work I will be thankful for it. 
                _currentSize = _maxSize;
                Console.WriteLine("Oldest container been removed to free some space for new one.");
            }
            return true;
        }
        else
        {
            // Telling user it's unrentable. Absolutely not unseen with file input. Thanks for Console.Clear() same with previous text printing. 
            Console.WriteLine("Hm, sorry, but this is unrentable to store this container.");
            return false;
        }
    }
}
