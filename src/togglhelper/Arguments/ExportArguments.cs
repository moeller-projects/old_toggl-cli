using CommandLine;

namespace togglhelper.Arguments
{
    [Verb("export", HelpText = "Exportieren des TogglReports")]
    public class ExportArguments
    {
        [Option('c', "config", Required = true, HelpText = "Pfad zur Config-Datei")]
        public string ConfigFilePath { get; set; }
    }
}
