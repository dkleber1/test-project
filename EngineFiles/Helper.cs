using System;
using System.IO;
using System.Collections.Generic;

public class Helper
{
    private static StreamReader inFile;

    //All the possible User Interface locations
    public const int UI_RIGHT = 0;
    public const int UI_BOTTOM = 1;
    public const int UI_LEFT = 2;
    public const int UI_TOP = 3;
    public const int UI_NONE = 4;

    //All possible colours to simplify access
    public const ConsoleColor BLACK = ConsoleColor.Black;  //Default BackgroundColor
    public const ConsoleColor DARK_BLUE = ConsoleColor.DarkBlue;
    public const ConsoleColor DARK_GREEN = ConsoleColor.DarkGreen;
    public const ConsoleColor DARK_CYAN = ConsoleColor.DarkCyan;
    public const ConsoleColor DARK_RED = ConsoleColor.DarkRed;
    public const ConsoleColor DARK_MAGENTA = ConsoleColor.DarkMagenta;
    public const ConsoleColor DARK_YELLOW = ConsoleColor.DarkYellow;
    public const ConsoleColor GRAY = ConsoleColor.Gray;  //Default ForegroundColor (Text colour)
    public const ConsoleColor DARK_GRAY = ConsoleColor.DarkGray;
    public const ConsoleColor BLUE = ConsoleColor.Blue;
    public const ConsoleColor GREEN = ConsoleColor.Green;
    public const ConsoleColor CYAN = ConsoleColor.Cyan;
    public const ConsoleColor RED = ConsoleColor.Red;
    public const ConsoleColor MAGENTA = ConsoleColor.Magenta;
    public const ConsoleColor YELLOW = ConsoleColor.Yellow;
    public const ConsoleColor WHITE = ConsoleColor.White;

    public static ConsoleColor fgCol = Console.ForegroundColor;
    public static ConsoleColor bgCol = Console.BackgroundColor;

    private static Random rng = new Random();

    private static readonly Dictionary<string, ConsoleColor> colours = new Dictionary<string, ConsoleColor>
  {
    { "00", ConsoleColor.Black },
    { "01", ConsoleColor.DarkBlue },
    { "02", ConsoleColor.DarkGreen },
    { "03", ConsoleColor.DarkCyan },
    { "04", ConsoleColor.DarkRed },
    { "05", ConsoleColor.DarkMagenta },
    { "06", ConsoleColor.DarkYellow },
    { "07", ConsoleColor.Gray },
    { "08", ConsoleColor.DarkGray },
    { "09", ConsoleColor.Blue },
    { "10", ConsoleColor.Green },
    { "11", ConsoleColor.Cyan },
    { "12", ConsoleColor.Red },
    { "13", ConsoleColor.Magenta },
    { "14", ConsoleColor.Yellow },
    { "15", ConsoleColor.White }
  };

    public static bool IsTransparent(char ch, ColourSet colours)
    {
        return ch == ' ' && colours.IsTransparent();
    }

    public static bool FastIntersects(GameObject obj1, GameObject obj2)
    {
        // Create a rectangle from the object's bounding box
        Rectangle r1 = new Rectangle(obj1.GetPos().x, obj1.GetPos().y, obj1.GetWidth()-1, obj1.GetHeight()-1);
        Rectangle r2 = new Rectangle(obj2.GetPos().x, obj2.GetPos().y, obj2.GetWidth()-1, obj2.GetHeight()-1);

        return r1.Intersects(r2);
    }

    public static bool Intersects(GameObject obj1, GameObject obj2)
    {
        // Create a rectangle from the object's bounding box
        Rectangle r1 = new Rectangle(obj1.GetPos().x, obj1.GetPos().y, obj1.GetWidth(), obj1.GetHeight());
        Rectangle r2 = new Rectangle(obj2.GetPos().x, obj2.GetPos().y, obj2.GetWidth(), obj2.GetHeight());

        // Find the overlapping rectangle of intersection if it exists
        Rectangle r3 = r1.Intersection(r2);

        // First test if their overall rectangles collide
        if (r3 != null)
        {
            // if any "pixel" in the overlapping rectangle is not isEmpty
            // for both rectangles then it is a collision
            for (int y = r3.Y; y < r3.Y + r3.Height; y++)
            {
                for (int x = r3.X; x < r3.X + r3.Width; x++)
                {
                    if (!Helper.IsTransparent(obj1.GetGrid()[y - obj1.GetPos().y, x - obj1.GetPos().x],
                        obj1.GetColours()[y - obj1.GetPos().y, x - obj1.GetPos().x]) &&
                        !Helper.IsTransparent(obj2.GetGrid()[y - obj2.GetPos().y, x - obj2.GetPos().x],
                        obj2.GetColours()[y - obj2.GetPos().y, x - obj2.GetPos().x]))
                    {
                        return true;
                    }
                }
            }
        }

        // no collision found
        return false;
    }

    public static int Clamp(int value, int min, int max)
    {
        return Math.Min(max, Math.Max(min, value));
    }

    public static float Clamp(float value, float min, float max)
    {
        return Math.Min(max, Math.Max(min, value));
    }

    public static Image LoadImage(string fileName)
    {
        Image img = null;
        int width;
        int height;
        char[,] grid;
        ColourSet[,] colourGrid;

        string line;
        string[] data;
        string[] colourData;

        try
        {
            inFile = File.OpenText(fileName);

            //Read in grid dimensions
            data = inFile.ReadLine().Split(new string[] { "," }, StringSplitOptions.None);
            height = Convert.ToInt32(data[0]);
            width = Convert.ToInt32(data[1]);

            //Instantiate the grid sizes
            grid = new char[height, width];
            colourGrid = new ColourSet[height, width];

            //Begin reading in and parsing the image row by row
            for (int i = 0; i < height; i++)
            {
                line = inFile.ReadLine();
                data = line.Split(new string[] { "| |" }, StringSplitOptions.None);
                colourData = data[1].Split(new string[] { "," }, StringSplitOptions.None);
                data = data[0].Split(new string[] { "," }, StringSplitOptions.None);

                for (int j = 0; j < width; j++)
                {
                    grid[i, j] = data[j][0];
                    colourGrid[i, j] = GetColours(colourData[j]);
                }
            }

            img = new Image(grid, colourGrid);
        }
        catch (Exception e)
        {
            Console.WriteLine("\n\n===================================");
            Console.WriteLine("Error loading image " + fileName + ", check the file for formatting");
            Console.WriteLine(e.Message);
            Console.WriteLine("===================================\n\n");
        }
        finally
        {
            if (inFile != null)
            {
                inFile.Close();
            }

            if (img == null)
            {
                //Create a BAD IMG
                img = new Image();
            }
        }

        return img;
    }

    private static ColourSet GetColours(String text)
    {
        return new ColourSet(colours[text.Substring(0, 2)], colours[text.Substring(2, 2)]);
    }

    public static int GetRandomNum(int min, int max)
    {
        return rng.Next(min, max);
    }

    public static float GetRandomNum(float min, float max)
    {
        return (float)(rng.NextDouble() * (max - min)) + min;
    }
}