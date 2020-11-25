

using MyApp.Domain.Entities;
using MyApp.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyApp.Infra.Data.Repositories
{
    public class GitHubResultRepository : RepositoryBase<GitHubResult>, IGitHubResultRepository
    {
        public GitHubResult SearchByTokenAndLastCommit(string token, DateTime lastCommit)
        {
            return Db.GitHubResults.Where(p => p.CommitToken == token && p.LastCommit == lastCommit).FirstOrDefault();
        }
    }
}
