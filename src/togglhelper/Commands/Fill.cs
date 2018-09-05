using System.Collections.Generic;
using System.Linq;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using togglhelper.Arguments;
using togglhelper.Models;

namespace togglhelper.Commands
{
    public class Fill
    {
        private readonly Config _config;
        private readonly FillArguments _arguments;

        public Fill(Config config, FillArguments arguments)
        {
            _config = config;
            _arguments = arguments;
        }

        public void FillToggl()
        {
            if (_arguments.IsSpecificSync)
            {
                SyncOnlySpecificItemsToToggl();
            }
            else
            {
                FullSyncToToggl();
            }
        }

        public void FullSyncToToggl()
        {
            var toggl = new Toggl(_config.TogglApikey);
            toggl.Setup(_config.WorkspaceName, _config.TfsProjects);

            var tfs = new Tfs(_config.TfsUri);
            tfs.Setup(_config.TfsQueryName, _config.TfsProjects, _config.TfsQuery);

            var queryTasks = new List<WorkItem>();
            _config.TfsProjects.ForEach(project => queryTasks.AddRange(tfs.GetTasks(_config.TfsQueryName, project).ToList()));

            if (queryTasks.Any())
            {
                toggl.CreateProjectsIfNotExist(queryTasks);
            }
        }

        public void SyncOnlySpecificItemsToToggl()
        {
            var toggl = new Toggl(_config.TogglApikey);
            toggl.Setup(_config.WorkspaceName, _config.TfsProjects);

            var tfs = new Tfs(_config.TfsUri);

            var specificTasks = tfs.GetSpecificWorkItem(_arguments.ItemIds).ToList();
            if (specificTasks.Any())
            {
                toggl.CreateProjectsIfNotExist(specificTasks);
            }
        }
    }
}
