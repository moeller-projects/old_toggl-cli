using CommandLine;

namespace togglhelper.Arguments
{
    [Verb("report", HelpText = "Erhalte die Reports von Toggl")]
    public class ReportArguments
    {
        [Option('c', "config", Required = true, HelpText = "Pfad zur Config-Datei")]
        public string ConfigFilePath { get; set; }
    }
}
