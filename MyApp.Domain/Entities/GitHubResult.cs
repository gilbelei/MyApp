

using System;

namespace MyApp.Domain.Entities
{
    /// <summary>
    /// Context class, responsible for connecting to the database and also for mapping the database tables in the entities.
    /// </summary>
    public class GitHubResult
    {
        public int GitHubResultId { get; set; }
        public string UserGitHub { get; set; }
        public string RepositoryGitHub { get; set; }
        public string CommitToken { get; set; }
        public DateTime LastCommit { get; set; }
        public string ResultGitHub { get; set; }
        public DateTime InsertedAt { get; set; }
    }
}
