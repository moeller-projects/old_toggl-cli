using System;
using System.Collections.Generic;
using CommandLine;
using togglhelper.Arguments;
using togglhelper.Commands;
using togglhelper.Helper;

namespace togglhelper
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Parser.Default.ParseArguments<ReportArguments, FillArguments, BatchArguments>(args)
                    .WithParsed<ReportArguments>(GetReport)
                    .WithParsed<FillArguments>(FillToggl)
                    .WithParsed<BatchArguments>(BatchOperation)
                    .WithParsed<ExportArguments>(ExportReport)
                    .WithNotParsed(Errors);
            }
            finally
            {
#if DEBUG
                Console.ReadLine();
#endif
            }
        }

        private static void ExportReport(ExportArguments arguments)
        {
            var config = ConfigHelper.GetConfig(arguments.ConfigFilePath);
            var export = new Export(config);
            export.AsExcel();
        }

        private static void FillToggl(FillArguments arguments)
        {
            var config = ConfigHelper.GetConfig(arguments.ConfigFilePath);
            var fill = new Fill(config, arguments);
            fill.FillToggl();
        }

        private static void GetReport(ReportArguments arguments)
        {
            var config = ConfigHelper.GetConfig(arguments.ConfigFilePath);
            var report = new Report(config);
            report.GetReport();
        }

        private static void BatchOperation(BatchArguments arguments)
        {
            var config = ConfigHelper.GetConfig(arguments.ConfigFilePath);
            var batch = new Batch(config, arguments);
            batch.Execute();
        }

        private static void Errors(IEnumerable<Error> errors)
        {
            foreach (var error in errors)
            {
                Console.WriteLine(error);
            }
            Environment.Exit(1);
        }
    }
}
