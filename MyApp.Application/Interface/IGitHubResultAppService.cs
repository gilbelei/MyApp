using GetGitHub.Domain.Entities;
using MyApp.Domain.Entities;
using System;
using System.Collections.Generic;

namespace MyApp.Application.Interface
{
    public interface IGitHubResultAppService : IAppServiceBase<GitHubResult>
    {
        List<WebScrapingResult> GetGitHub(string user, string repository);
        GitHubResult SearchByTokenAndLastCommit(string token, DateTime lastCommit);
    }
}
