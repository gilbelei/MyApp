using MyApp.Domain.Entities;
using System;

namespace MyApp.Domain.Interfaces.Services
{
    public interface IGitHubResultService : IServiceBase<GitHubResult>
    {
        GitHubResult SearchByTokenAndLastCommit(string token, DateTime lastCommit);
    }
}
