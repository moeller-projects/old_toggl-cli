using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;

namespace togglhelper
{
    public class Tfs
    {
        private readonly WorkItemTrackingHttpClient _witClient;

        public Tfs(string tfsCollectionUri)
        {
            var connection = new VssConnection(new Uri(tfsCollectionUri), new VssCredentials());
            _witClient = connection.GetClient<WorkItemTrackingHttpClient>();
        }

        public void Setup(string queryName, IEnumerable<string> tfsProjectNames, string query)
        {
            foreach (var tfsProjectName in tfsProjectNames)
            {
                var queryHierarchyItems = _witClient.GetQueriesAsync(tfsProjectName, depth: 2).Result;
                var myQueriesFolder = queryHierarchyItems.FirstOrDefault(qhi => qhi.Name.Equals("My Queries"));
                if (myQueriesFolder == null) continue;

                QueryHierarchyItem togglQuery = null;
                if (myQueriesFolder.Children != null)
                {
                    togglQuery = myQueriesFolder.Children.FirstOrDefault(qhi => qhi.Name.Equals(queryName));
                }

                if (togglQuery != null) continue;

                togglQuery = new QueryHierarchyItem
                {
                    Name = queryName,
                    Wiql = query,
                    IsFolder = false
                };
                var result = _witClient.CreateQueryAsync(togglQuery, tfsProjectName, myQueriesFolder.Name).Result;
            }
        }

        public IEnumerable<WorkItem> GetTasks(string queryName, string tfsProjectName)
        {
            var resultList = new List<WorkItem>();
            var queryHierarchyItems = _witClient.GetQueriesAsync(tfsProjectName, depth: 2).Result;
            var myQueriesFolder = queryHierarchyItems.FirstOrDefault(qhi => qhi.Name.Equals("My Queries"));

            if (myQueriesFolder == null) return resultList;

            var toggleQuery = myQueriesFolder.Children.FirstOrDefault(qhi => qhi.Name.Equals(queryName));

            if (toggleQuery == null) return resultList;

            var result = _witClient.QueryByIdAsync(toggleQuery.Id).Result;

            if (!result.WorkItems.Any()) return resultList;

            var skip = 0;
            const int batchSize = 100;
            IEnumerable<WorkItemReference> workItemRefs;
            do
            {
                workItemRefs = result.WorkItems.Skip(skip).Take(batchSize).ToList();
                if (workItemRefs.Any())
                {
                    resultList.AddRange(_witClient.GetWorkItemsAsync(workItemRefs.Select(wir => wir.Id)).Result);

                }
                skip += batchSize;
            }
            while (workItemRefs.Count() == batchSize);

            return resultList;
        }

        //public void CreateWorkItem(ReportTimeEntry timeEntry, string projectName)
        //{
        //    var patchDocument = new JsonPatchDocument
        //    {
        //        new JsonPatchOperation()
        //        {
        //            Operation = Operation.Add,
        //            Path = "/fields/System.Title",
        //            Value = "Authorization Errors"
        //        }
        //    };

        //    var result = _witClient.CreateWorkItemAsync(patchDocument, projectName, "Bug").Result;
        //}
    }
}
