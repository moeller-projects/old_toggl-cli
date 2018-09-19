using System;
using CommandLine;

namespace togglhelper.Arguments
{
    [Verb("batch", HelpText = "Ausführen von Batch-Operationen")]
    public class BatchArguments
    {
        [Option('c', "config", Required = true, HelpText = "Pfad zur Config-Datei")]
        public string ConfigFilePath { get; set; }
        [Option('f', "from", Required = true, HelpText = "Von (Datum)")]
        public DateTime From { get; set; }
        [Option('t', "to", Required = true, HelpText = "Bis (Datum)")]
        public DateTime To { get; set; }
        [Option('d', "description", Required = true, HelpText = "Task Beschreibung")]
        public string TaskDescription { get; set; }
        [Option('e', "elapsed", Required = true, HelpText = "benötigte Stunden/Tag")]
        public double ElapsedHoursPerDay { get; set; }
        [Option('w', "with-weekend", Required = false, Default = false, HelpText = "mit Wochenende?")]
        public bool WithWeekend { get; set; }
    }
}
