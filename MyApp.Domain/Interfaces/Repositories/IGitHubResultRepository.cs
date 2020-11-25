using MyApp.Domain.Entities;
using System;

namespace MyApp.Domain.Interfaces.Repositories
{
    public interface IGitHubResultRepository : IRepositoryBase<GitHubResult>
    {
        GitHubResult SearchByTokenAndLastCommit(string token, DateTime lastCommit);
    }
}
