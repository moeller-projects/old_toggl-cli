using CommandLine;

namespace togglhelper.Arguments
{
    [Verb("fill", HelpText = "Übertragen der Tfs-Tasks zu Toggl")]
    public class FillArguments
    {
        [Option('c', "config", Required = true, HelpText = "Pfad zur Config-Datei")]
        public string ConfigFilePath { get; set; }
    }
}
