using Spectre.Console;

namespace SimpleSQL
{
    internal class AppManager
    {
        private const int MAX_COMMANDLIST_COUNT = 15;

        internal void Run(string[] args)
        {
            try
            {
                if (args.Length == 0 ) { throw new ArgumentException("Arguments missing"); };
                string input = "";
                string connectionString = args[0];
                List<string> commandList = new List<string>();
                DBGateway db = new DBGateway(connectionString);

                AnsiConsole.Clear();
                Utils.printBanner();

                connectionString = Utils.checkConnectionString(connectionString);  //ask for username, password, database if needed

                //check connection state
                AnsiConsole.Status().Start("[green]Check connection...[/]", checkDBState =>
                {
                    using (DBConnection db = new DBConnection(connectionString)) { }
                    checkDBState.Status("T[green]Check connection...[/]");
                    checkDBState.Spinner(Spinner.Known.Star);
                    checkDBState.SpinnerStyle(Style.Parse("green"));
                });

                Utils.printConnectionInfo(connectionString);

                while (true)
                {
                    try
                    {
                        Console.ForegroundColor = Color.Yellow;
                        Console.Write("SQL>");  //dont use Spectre.Console: KeyUp not working
                        input += Console.ReadLine()!.Trim();

                        if (input.ToLower() == "h" || input.ToLower() == "help") Utils.printHelptext();
                        
                        //list recent commands
                        if (input.ToLower() == "l" || input.ToLower() == "r")
                        {
                            string lastComment = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("Previous commands").PageSize(15).AddChoices(commandList.ToArray().Append("back")));
                            input = lastComment.ToLower() == "back" ? lastComment : "";
                        }

                        //execute SELECT statement
                        if (input.ToLower().Trim().StartsWith("select") && input.ToLower().Trim().EndsWith(";"))
                        {
                            foreach (string item in Utils.formatResult(db.ExecuteReader(input))) { AnsiConsole.MarkupLine($"[green]{item}[/]"); }
                            addCommandToList(commandList, input);
                            input = "";
                        }

                        //execute all OTHER statement
                        if (input.Length > 1 && input.EndsWith(";"))
                        {
                            AnsiConsole.MarkupLine($"[green]{db.ExecuteNonQuery(input)} affected[/]");
                            addCommandToList(commandList, input);
                            input = "";
                        }

                        if (input.ToLower().EndsWith("cls") || input.ToLower().EndsWith("clear")) { input = ""; AnsiConsole.Clear(); }
                        if (input.ToLower().EndsWith("exit") || input.ToLower().EndsWith("quit")) break;
                        if (input.EndsWith(";")) { input = ""; AnsiConsole.MarkupLine($"[yellow]Unknown command[/]"); }
                    }
                    catch (Exception ex)
                    {
                        input = "";
                        AnsiConsole.MarkupLine($"[red]Error: {ex.Message}[/]");
                    }
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Error: {ex.Message}[/]");
            }
        }

        private void addCommandToList(List<string> commandList, string input)
        {
            if (commandList.Count >= MAX_COMMANDLIST_COUNT) commandList.RemoveAt(0);
            commandList.Add(input);
        }
    }
}
