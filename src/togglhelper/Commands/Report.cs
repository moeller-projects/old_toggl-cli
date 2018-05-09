using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleTableExt;
using togglhelper.Models;

namespace togglhelper.Commands
{
    public class Report
    {
        private readonly Config _config;

        public Report(Config config)
        {
            _config = config;
        }

        public void GetReport()
        {
            var toggl = new Toggl(_config.TogglApikey);
            var reportTimeEntries = toggl.GetTimeEntries(_config.ReportDaysBack);

            var groupedReportTimeEntries = reportTimeEntries
                                            .GroupBy(g => Convert.ToDateTime(g.Start).Date)
                                            .OrderBy(o => o.Key);

            foreach (var entries in groupedReportTimeEntries)
            {
                var minDate = Convert.ToDateTime(entries.Min(m => m.Start)).Date;
                var maxDate = Convert.ToDateTime(entries.Max(m => m.Stop)).Date;

                if (minDate == maxDate)
                {
                    Console.WriteLine($"{Environment.NewLine}" +
                                      $"{minDate:dd.MM.yyyy}" +
                                      $"{Environment.NewLine}");

                    ConsoleTableBuilder
                        .From(entries
                            .OrderBy(o => Convert.ToDateTime(o.Start))
                            .Select(s => new List<object>
                            {
                                Convert.ToDateTime(s.Start).ToString("HH:mm"),
                                Convert.ToDateTime(s.Stop).ToString("HH:mm"),
                                s.ClientName,
                                s.ProjectName,
                                s.Description
                            }).ToList())
                        .WithColumn("Start", "End", "Client", "Project", "Note")
                        .WithFormat(ConsoleTableBuilderFormat.MarkDown)
                        .WithOptions(new ConsoleTableBuilderOption
                        {
                            Delimiter = "|",
                            DividerString = "-",
                            TrimColumn = true
                        })
                        .ExportAndWriteLine();
                }
                else
                {
                    Console.WriteLine($"{Environment.NewLine}" +
                                      $"{minDate:dd.MM.yyyy} - {maxDate:dd.MM.yyyy}" +
                                      $"{Environment.NewLine}");

                    ConsoleTableBuilder
                        .From(reportTimeEntries
                            .OrderBy(o => Convert.ToDateTime(o.Start))
                            .Select(s => new List<object>
                            {
                                Convert.ToDateTime(s.Start).ToString("dd.MM.yyyy"),
                                Convert.ToDateTime(s.Start).ToString("HH:mm"),
                                Convert.ToDateTime(s.Stop).ToString("dd.MM.yyyy"),
                                Convert.ToDateTime(s.Stop).ToString("HH:mm"),
                                s.ClientName,
                                s.ProjectName,
                                s.Description
                            }).ToList())
                        .WithColumn("Start Date", "Time", "Stop Date", "Time", "Client", "Project", "Note")
                        .WithFormat(ConsoleTableBuilderFormat.MarkDown)
                        .WithOptions(new ConsoleTableBuilderOption
                        {
                            Delimiter = "|",
                            DividerString = "-",
                            TrimColumn = true
                        })
                        .ExportAndWriteLine();
                }
            }
        }
    }
}
