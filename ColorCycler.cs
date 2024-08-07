namespace AIArbiter;

static class ColorCycler
{
    private static int currentIndex = 0;
    private static ConsoleColor[] colors = new[] { ConsoleColor.Cyan, ConsoleColor.Yellow, ConsoleColor.Green, ConsoleColor.Magenta };

    public static ConsoleColor GetNextColor()
    {
        ConsoleColor color = colors[currentIndex];
        currentIndex = (currentIndex + 1) % colors.Length;
        return color;
    }
}