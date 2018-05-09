using System;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using togglhelper.Models;

namespace togglhelper.Commands
{
    public class Export
    {
        private readonly Config _config;

        public Export(Config config)
        {
            _config = config;
        }

        public void AsExcel()
        {
            var toggl = new Toggl(_config.TogglApikey);
            var reportTimeEntries = toggl.GetTimeEntries(_config.ReportDaysBack);

            var columns = new[]
            {
                "Start",
                "Time",
                "Stop",
                "Time",
                "Client",
                "Project",
                "Description"
            };

            var list = reportTimeEntries
                .OrderBy(o => Convert.ToDateTime(o.Start))
                .Select(s => new ExportEntry
                {
                    Start = Convert.ToDateTime(s.Start),
                    Stop = Convert.ToDateTime(s.Stop),
                    Client = s.ClientName,
                    Project = s.ProjectName,
                    Description = s.Description
                }).ToList();

            using (var xl = SpreadsheetDocument.Create($"C:\\temp\\togglreport_{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}.xlsx", SpreadsheetDocumentType.Workbook))
            {

                xl.AddWorkbookPart();
                var wsp = xl.WorkbookPart.AddNewPart<WorksheetPart>();

                var oxw = OpenXmlWriter.Create(wsp);
                oxw.WriteStartElement(new Worksheet());
                oxw.WriteStartElement(new SheetData());

                for (var i = 0; i < list.Count; ++i)
                {
                    var row = i + 2;
                    var entry = list[i];

                    if (i == 0)
                    {
                        var header = new List<OpenXmlAttribute> { new OpenXmlAttribute("r", null, "1") };
                        oxw.WriteStartElement(new Row(), header);
                        foreach (var column in columns)
                        {
                            header = new List<OpenXmlAttribute> {new OpenXmlAttribute("t", null, "str")};
                            oxw.WriteStartElement(new Cell(), header);
                            oxw.WriteElement(new CellValue(column));
                            oxw.WriteEndElement();
                        }
                    }

                    var oxa = new List<OpenXmlAttribute> {new OpenXmlAttribute("r", null, row.ToString())};
                    oxw.WriteStartElement(new Row(), oxa);
                    
                    for (var r = 1; r <= columns.Length; ++r)
                    {
                        oxa = new List<OpenXmlAttribute> {new OpenXmlAttribute("t", null, "str")};
                        oxw.WriteStartElement(new Cell(), oxa);
                        switch (r)
                        {
                            case 1:
                                oxw.WriteElement(new CellValue(entry.Start.ToString("dd.MM.yyyy")));
                                break;
                            case 2:
                                oxw.WriteElement(new CellValue(entry.Start.ToString("HH:mm")));
                                break;
                            case 3:
                                oxw.WriteElement(new CellValue(entry.Stop.ToString("dd.MM.yyyy")));
                                break;
                            case 4:
                                oxw.WriteElement(new CellValue(entry.Stop.ToString("dd.MM.yyyy")));
                                break;
                            case 5:
                                oxw.WriteElement(new CellValue(entry.Client));
                                break;
                            case 6:
                                oxw.WriteElement(new CellValue(entry.Project));
                                break;
                            case 7:
                                oxw.WriteElement(new CellValue(entry.Description));
                                break;
                            default:
                                oxw.WriteElement(new CellValue(""));
                                break;
                        }
                        
                        oxw.WriteEndElement();
                    }
                    oxw.WriteEndElement();
                }
                oxw.WriteEndElement();
                oxw.WriteEndElement();
                oxw.Close();

                oxw = OpenXmlWriter.Create(xl.WorkbookPart);
                oxw.WriteStartElement(new Workbook());
                oxw.WriteStartElement(new Sheets());
                oxw.WriteElement(new Sheet()
                {
                    Name = "Report 1",
                    SheetId = 1,
                    Id = xl.WorkbookPart.GetIdOfPart(wsp)
                });

                oxw.WriteEndElement();
                oxw.WriteEndElement();
                oxw.Close();

                xl.Close();
            }
        }
    }

    public class ExportEntry
    {
        public DateTime Start { get; set; }
        public DateTime Stop { get; set; }
        public string Client { get; set; }
        public string Project { get; set; }
        public string Description { get; set; }
    }
}
