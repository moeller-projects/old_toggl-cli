namespace togglhelper.Models
{
    public class Config
    {
        public string TogglApikey { get; set; }
        public int ReportDaysBack { get; set; }
        public string WorkspaceName { get; set; }
        public string[] TfsProjects { get; set; }
        public string TfsUri { get; set; }
        public string TfsQueryName { get; set; }
        public string TfsQuery { get; set; }
    }
}
