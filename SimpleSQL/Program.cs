using Spectre.Console;

namespace SimpleSQL
{
    internal class Program
    {
        static void Main(string[] args)
        {
            AnsiConsole.Clear();
            AnsiConsole.Background = ConsoleColor.Black;
            AnsiConsole.MarkupLine("[green]---------[/]");
            AnsiConsole.MarkupLine("[green]SimpleSQL[/]");
            AnsiConsole.MarkupLine("[green]---------[/]");
            AnsiConsole.MarkupLine($"[white](c)2023{(DateTime.Now.Year != 2023 ? DateTime.Now.Year.ToString() : "")} Gerald Weinberger[/]");

            if (args.Length < 1)
            {
                AnsiConsole.MarkupLine($"[white]  Usage: [/][yellow]SimpleSQL ConnectionString[/]");
                AnsiConsole.MarkupLine($"[white]Example: [/][yellow]SimpleSQL \"Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=master; Database=Master;Integrated Security=True;Encrypt=False;\"[/]");
                AnsiConsole.MarkupLine($"[white]Example: [/][yellow]SimpleSQL \"Data Source=xxxxx.xxxxx.rds.amazonaws.com;Database=Master; User ID=xxx;Password=xxx;\"[/]");
                AnsiConsole.MarkupLine($" ");
                AnsiConsole.MarkupLine($"[white]Information: Username and/or password will be requested if it is not specified.[/]");
                AnsiConsole.MarkupLine($" ");
                Environment.Exit(1);
            }

            AppManager worker = new AppManager();
            worker.Run(args);
        }
    }
}