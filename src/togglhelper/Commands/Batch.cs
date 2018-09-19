using System;
using togglhelper.Arguments;
using togglhelper.Models;

namespace togglhelper.Commands
{
    public class Batch
    {
        private readonly Config _config;
        private readonly BatchArguments _arguments;
        private readonly Toggl _toggl;

        public Batch(Config config, BatchArguments arguments)
        {
            _config = config;
            _arguments = arguments;
            _toggl = new Toggl(_config.TogglApikey);
        }

        public void Execute()
        {
            for (var date = _arguments.From; date.Date <= _arguments.To.Date; date = date.AddDays(1))
            {
                if (!_arguments.WithWeekend
                    && (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday))
                {
                    continue;
                }
                _toggl.CreateTimeEntry(_config.WorkspaceName, date, date.AddHours(_arguments.ElapsedHoursPerDay), _arguments.TaskDescription);
            }
        }
    }
}
