using Spectre.Console;

namespace SimpleSQL
{
    internal static class Utils
    {
        internal static void printBanner() 
        {
            AnsiConsole.Write(new FigletText("SimpleSQL").Color(Color.Yellow).LeftJustified());
            AnsiConsole.MarkupLine($"[yellow dim](c)2023{(DateTime.Now.Year != 2023 ? DateTime.Now.Year.ToString() : "")} Gerald Weinberger | MIT licence[/]");
            AnsiConsole.MarkupLine(" ");
        }

        internal static string checkConnectionString(string connectionString)
        {
            string result = connectionString;
            string checkString = connectionString.ToLower().Replace(" ", "");

            if (checkString.Contains("database=") == false)
            {
                string db = AnsiConsole.Ask<string>("Enter Database: ");
                if (db != null && db.Trim() != "") { result += $";Database = {db};"; }
            }

            if (checkString.Contains("userid") == false && checkString.Contains("integratedsecurity=true") == false)
            {
                string username = AnsiConsole.Ask<string>("Enter Username: ");
                if (username != null && username.Trim() != "") { result += $";User ID = {username};"; }
            }

            if (checkString.Contains("password=") == false && checkString.Contains("integratedsecurity=true") == false)
            {
                string pwd = AnsiConsole.Prompt(new TextPrompt<string>("Enter password: ").PromptStyle("red").Secret());
                if (pwd != null && pwd.Trim() != "") { result += $";Password = {pwd};"; }
            }

            return result;
        }

        internal static void printConnectionInfo(string connectionString)
        {
            AnsiConsole.Write(new Rule("Connection Info") { Style = "yellow" });
            AnsiConsole.MarkupLine($"\n\r[yellow dim]{new DBGateway(connectionString).GetDBVersion()}[/]");
            AnsiConsole.Write(new Rule() { Style = "yellow" });
            AnsiConsole.MarkupLine(" ");
        }

        internal static void printHelptext()
        {
            AnsiConsole.MarkupLine($" [green]h|help[/] ...... [white]display help[/]");
            AnsiConsole.MarkupLine($" [green]cls|clear[/] ... [white]clear display[/]");
            AnsiConsole.MarkupLine($" [green]r|l[/] ......... [white]list of recent commands[/]");
            AnsiConsole.MarkupLine($" [green]Key up[/] ...... [white]scroll through last command[/]");
            AnsiConsole.MarkupLine($" [green]quit|exit[/] ... [white]quit program[/]");
        }

        internal static List<string> formatResult(List<List<string>> contentList)
        {
            if (contentList.Count == 1) return contentList[0];
            List<string> result = new List<string>();
            try
            {
                //get max width for each row
                List<int> maxWidthList = new List<int>();
                for (int recordCounter = 0; recordCounter < contentList.Count; recordCounter++)
                {
                    for (int columnCounter = 0; columnCounter < contentList[recordCounter].Count; columnCounter++)
                    {
                        if (maxWidthList.Count <= columnCounter) maxWidthList.Add(0);
                        int len = contentList[recordCounter][columnCounter].Trim().Length;
                        if (len > maxWidthList[columnCounter]) maxWidthList[columnCounter] = len;
                    }
                }
                //build formated string
                for (int recordCounter = 0; recordCounter < contentList.Count; recordCounter++)
                {
                    string content = "";
                    for (int columnCounter = 0; columnCounter < contentList[recordCounter].Count; columnCounter++)
                    {
                        content += contentList[recordCounter][columnCounter].PadRight(maxWidthList[columnCounter]) + " ";
                    }
                    if (content.Trim() != "") result.Add(content);
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Error: {ex.Message}[/]");
            }
            return result;
        }
    }
}
