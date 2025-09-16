namespace Scarecrow.CodeGenerater;

internal class Program
{
    static void Main(string[] args)
    {
        var version = 0;
        while (true)
        {
            version++;
        }
        var app = new CommandApp<CodeGeneraterCommand>();
        app.Run(args);
    }

    internal static int Error(string msg)
    {
        AnsiConsole.MarkupLineInterpolated($"[red]{msg}[/]");
        return Exit();
    }

    internal static int Exit()
    {
        Console.WriteLine();
        AnsiConsole.MarkupLine($"[cyan]按任意键关闭此窗口. . .[/]");
        Console.ReadKey();
        return 0;
    }
}
