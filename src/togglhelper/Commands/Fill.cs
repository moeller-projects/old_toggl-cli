using togglhelper.Models;

namespace togglhelper.Commands
{
    public class Fill
    {
        private readonly Config _config;

        public Fill(Config config)
        {
            _config = config;
        }

        public void FillToggl()
        {
            var toggl = new Toggl(_config.TogglApikey);
            toggl.Setup(_config.WorkspaceName, _config.TfsProjects);

            var tfs = new Tfs(_config.TfsUri);
            tfs.Setup(_config.TfsQueryName, _config.TfsProjects, _config.TfsQuery);

            foreach (var tfsProjectName in _config.TfsProjects)
            {
                var tasks = tfs.GetTasks(_config.TfsQueryName, tfsProjectName);
                toggl.CreateProjectsIfNotExist(tfsProjectName, tasks);
            }
        }
    }
}
