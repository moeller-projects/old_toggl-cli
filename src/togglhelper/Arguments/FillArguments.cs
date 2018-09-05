using System.Collections.Generic;
using System.Linq;
using CommandLine;

namespace togglhelper.Arguments
{
    [Verb("fill", HelpText = "Übertragen der Tfs-Tasks zu Toggl")]
    public class FillArguments
    {
        [Option('c', "config", Required = true, HelpText = "Pfad zur Config-Datei")]
        public string ConfigFilePath { get; set; }

        //[Option('p', "project", Default = null, Required = false, HelpText = "Name des Projektes")]
        //public string ProjectName { get; set; }

        [Option('i', "items", Default = null, Required = false, HelpText = "Liste mit TaskIds zum synchronisieren")]
        public IEnumerable<int> ItemIds { get; set; }

        //public bool IsSpecificSync => ItemIds != null && ProjectName != null;
        public bool IsSpecificSync => ItemIds != null && ItemIds.Any();
    }
}
