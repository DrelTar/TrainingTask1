using System;
using System.Collections.Generic;
using System.IO;

// Hello, Stranger. This is my program. It's not good, it's not bad. Well... maybe a little bit bad. 
// There is no extra functions and such things. I just have no ideas about sometinhg intresting. So... Go, have a look.

class Program
{
    // Variable which contains main warehouse. 
    static Warehouse warehouse;

    /// <summary>
    /// Do nothing except starting program.
    /// </summary>
    public static void Main()
    {
        Start();
    }

    /// <summary>
    /// Ask how user wanna input data and split program into two ways.
    /// </summary>
    public static void Start()
    {
        Console.Clear();
        Console.WriteLine("Choose how you will input necessary information and let's go.");
        Console.WriteLine("0 - via console");
        Console.WriteLine("1 - via files");
        Console.WriteLine("If it's your first time you surely should at first do it through console.");
        uint inputType;
        do
        {
            Console.WriteLine("So...");
            inputType = ReadUInt("type of input");
        } while (inputType > 1);
        if (inputType == 0)
        {
            // Path of console input.
            CreateWarehouseConsole();
        }
        else
        {
            // Path of file input.
            FileRules();
        }
    }

    /// <summary>
    /// Tell user about formats for file input and some another information about it.
    /// </summary>
    public static void FileRules()
    {
        Console.WriteLine("Make sure you have three files.");
        Console.WriteLine("First one, named 'warehouse.txt' which contains two numbers, each on its own line. Size of warehouse and fee.");
        Console.WriteLine("Second one, named 'actions.txt' which contains any amount of numbers 0 and 1.");
        Console.WriteLine("After '1' line should go line with number (from 1 to amount of containers) of container to delete.");
        Console.WriteLine("Last one, named 'containers.txt' which contains data about containers. One line contains data about boxes in container");
        Console.WriteLine("Example: {price1 weight1 price2 weight2 ...}");
        Console.WriteLine("Remember: 1) All numbers should be positive.");
        Console.WriteLine("          2) Containers max weight is random. Some of your boxes can be losen.");
        Console.WriteLine("          3) Any mistake will lead to immediate shutdown.");
        CreateWarehouseFile();
    }

    /// <summary>
    /// Create warehouse through console input.
    /// </summary>
    public static void CreateWarehouseConsole()
    {
        Console.Clear();
        Console.WriteLine("Hello there. Welcome to your personal vegetables warehouse.");
        Console.WriteLine("At first, let's create your warehouse. Input its max amount of containers and fee for one container.");
        Console.WriteLine("Remember, we doin' business not charity. Only positive numbers, okay?");
        // Creating warehouse. 
        warehouse = new Warehouse(ReadUInt("size of warehouse"), ReadUInt("fee for container"));
        CalculateActionsConsole();
    }

    /// <summary>
    /// Create warehouse through file input.
    /// </summary>
    public static void CreateWarehouseFile()
    {
        try
        {
            // Input - lines from warehouse.txt.
            string[] input = File.ReadAllLines("warehouse.txt");
            // Creating warehpuse, input[0] - size, input[1] - fee.
            warehouse = new Warehouse(uint.Parse(input[0]), uint.Parse(input[1]));
            CalculateActionsFile();
        }
        catch
        {
            // Telling user something wrong, will be in every "File" method
            Console.WriteLine("Something gone wrong. Check every number you use is positive and all files exist.");
            Start();
        }
    }

    /// <summary>
    /// Ask for amount of actions from user and calculate those actions.
    /// </summary>
    public static void CalculateActionsConsole()
    {
        Console.Clear();
        Console.WriteLine("Okay, now you should input amount of actions you want to do with this warehouse.");
        // Reading amount of actions.
        uint amountOfActions = ReadUInt("amount of actions");
        Console.WriteLine("Now, you have two actions. Adding new container and deleting some container.");
        Console.WriteLine("0 - add");
        Console.WriteLine("1 - delete");
        Console.WriteLine("Any other input will lead to loss of your action.");
        Console.WriteLine("If you try to put extra container into full warehouse oldest container will be removed.");
        // Calculating actions.
        uint userInput;
        for (int i = 0; i < amountOfActions; ++i)
        {
            userInput = ReadUInt("action number");
            if (userInput == 0)
            {
                AddContainerConsole();
            }
            else if (userInput == 1)
            {
                DeleteContainerConsole();
            }
        }
        // Go to the end of program.
        Finish();
    }

    /// <summary>
    /// Calculate list of actions from file input.
    /// </summary>
    public static void CalculateActionsFile()
    {
        try
        {
            string[] actions = File.ReadAllLines("actions.txt");
            // Index of current container which necessary in AddContainerFile.
            uint indexOfContainer = 0;
            for (int i = 0; i < actions.Length; ++i)
            {
                if (uint.Parse(actions[i]) == 0)
                {
                    AddContainerFile(indexOfContainer++);
                }
                else if (uint.Parse(actions[i]) == 1)
                {
                    // Delete container with index which placed on the next line of actions.txt.
                    DeleteContainerFile(uint.Parse(actions[++i]));
                }
            }
            Finish();
        }
        catch
        {
            Console.WriteLine("Something gone wrong. Check every number you use is positive and all files exist.");
            Start();
        }
    }

    /// <summary>
    /// Read boxes to container and add it to warehouse.
    /// </summary>
    public static void AddContainerConsole()
    {
        Console.Clear();
        // Creating random weight for container. 
        Random rand = new Random();
        uint maxWeight = (uint)rand.Next(50, 1001);
        Console.WriteLine("We gonna fill this container to the top, yeah?");
        Console.WriteLine($"Here is it. Best which I can find. Max weight of boxes in it is {maxWeight}");
        Console.WriteLine("Now, input information about boxes in {price weight} format.");
        Console.WriteLine("One line - one box, and no negative numbers, okay? All wrong input are counted as zeros.");
        Console.WriteLine("When you wanna stop just input two zeros. Also when container is full it will stopps automoticaly.");
        // Reading boxes and packing them into array.
        uint[,] boxes = new uint[maxWeight, 2];
        uint price = 1;
        uint weight = 1;
        uint index = 0;
        uint summWeight = 0;
        do
        {
            string[] userInput = Console.ReadLine().Split();
            if (userInput.Length == 2) {
                uint.TryParse(userInput[0], out price);
                uint.TryParse(userInput[1], out weight);
                // Checking that weight is normal.
                if ((price != 0 || weight != 0) && summWeight + weight <= maxWeight)
                {
                    boxes[index, 0] = price;
                    boxes[index, 1] = weight;
                    ++index;
                }
                summWeight += weight;
            }
        } while ((price != 0 || weight != 0) && summWeight < maxWeight);
        // If container unrenatble we not say to user that container created.
        if (warehouse.AddContainer(new Container(maxWeight, boxes)))
        {
            Console.WriteLine($"Container number {warehouse.CurrentSize} created");
        }
    }

    /// <summary>
    /// Read containers.txt and create container from one line of it.
    /// </summary>
    /// <param name="index"> Number of line in "container.txt" which contain data about containers. </param>
    public static void AddContainerFile(uint index)
    {
        try
        {
            // Parsinf from array to 2d array.
            string[] inputBoxes = File.ReadAllLines("containers.txt")[index].Split();
            uint[,] boxes = new uint[inputBoxes.Length / 2, 2];
            for (int i = 0; i < inputBoxes.Length; i += 2)
            {
                boxes[i / 2, 0] = uint.Parse(inputBoxes[i]);
                boxes[i / 2, 1] = uint.Parse(inputBoxes[i + 1]);
            }
            // Creating container.
            warehouse.AddContainer(new Container((uint)new Random().Next(50, 1001), boxes));
        }
        catch
        {
            Console.WriteLine("Something gone wrong. Check every number you use is positive and all files exist.");
            Start();
        }
    }

    /// <summary>
    /// Delete container from warehouse on index with console input.
    /// </summary>
    public static void DeleteContainerConsole()
    {
        Console.Clear();
        Console.WriteLine("Just input number of container and it will be removed.");
        Console.WriteLine($"Amount of existed containers: {warehouse.CurrentSize}");
        uint numberOfContainer = ReadUInt("number of container");
        // Deleting container from index - 1, as user have numeration from 1 and we from 0.
        warehouse.DeleteContainer(numberOfContainer - 1);
        Console.WriteLine("If container existed it was removed.");
    }

    /// <summary>
    /// Delete container from warehouse on index with file input.
    /// </summary>
    /// <param name="index"> Index of deleted container. </param>
    public static void DeleteContainerFile(uint index)
    {
        try
        {
            // Index - 1, as user have numeration started by 1.
            warehouse.DeleteContainer(index - 1);
        }
        catch
        {
            Console.WriteLine("Something gone wrong. Check every number you use is positive and all files exist.");
            Start();
        }
    }

    /// <summary>
    /// Ask type of output and end program if user said so.
    /// </summary>
    public static void Finish()
    {
        Console.Clear();
        Console.WriteLine("Okay, your actions are gone. Where you want to have information about warehouse, containers and boxes?");
        Console.WriteLine("0 - at console");
        Console.WriteLine("1 - in file");
        uint outputType;
        do
        {
            Console.Write("So...");
            outputType = ReadUInt("type of output");
        } while (outputType > 1);
        if (outputType == 0)
        {
            LastOutputConsole();
        }
        else
        {
            LastOutputFile();
        }
        Console.WriteLine("Wanna create another warehouse? Press any key except Escape. Or press Escape to leave.");
        if (Console.ReadKey().Key != ConsoleKey.Escape)
        {
            Start();
        }
    }

    /// <summary>
    /// Print data about warehouse, containers and boxes to console.
    /// </summary>
    public static void LastOutputConsole() 
    {
        Console.WriteLine($"Number of containers in warehouse: {warehouse.CurrentSize}");
        List<Container> containers = warehouse.Containers;
        for (int i = 0; i < containers.Count; ++i)
        {
            Console.WriteLine($"Container number {i + 1}. Total price: {containers[i].TotalPrice}. Total weight: {containers[i].CurrentWeight}");
            List<Box> boxes = containers[i].Boxes;
            for (int j = 0; j < boxes.Count; ++j)
            {
                Console.WriteLine($"Box number {j + 1}. Price: {boxes[j].Price}. Weight: {boxes[j].Weight}");
            } 
        }
    }

    /// <summary>
    /// Print data about warehouse, containers and boxes to file.
    /// </summary>
    public static void LastOutputFile()
    {
        Console.Write("Input name of file: ");
        string file = Console.ReadLine();
        // Data about warehouse, containers and boxes.
        string data = "";
        data += $"Number of containers in warehouse: {warehouse.CurrentSize}\n";
        List<Container> containers = warehouse.Containers;
        for (int i = 0; i < containers.Count; ++i)
        {
            data += $"Container number {i + 1}. Total price: {containers[i].TotalPrice}. Total weight: {containers[i].CurrentWeight}\n";
            List<Box> boxes = containers[i].Boxes;
            for (int j = 0; j < boxes.Count; ++j)
            {
                data += $"Box number {j + 1}. Price: {boxes[j].Price}. Weight: {boxes[j].Weight}\n";
            }
        }
        File.WriteAllText(file, data);
    }

    /// <summary>
    /// Read string and parse it to uint.
    /// </summary>
    /// <param name="explanationString"> String which explain what exactly user need to input. </param>
    /// <returns> Uint variable. </returns>
    public static uint ReadUInt(string explanationString)
    {
        uint number;
        do
        {
            Console.Write($"Input {explanationString}: ");
        } while (!uint.TryParse(Console.ReadLine(), out number));
        // Very stupid easter egg. Don't mind it. There is nothing to see.
        if (number == 69)
        {
            Console.WriteLine("Nice");
        }
        return number;
    }
}