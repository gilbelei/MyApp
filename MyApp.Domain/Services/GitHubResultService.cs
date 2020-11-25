using MyApp.Domain.Entities;
using MyApp.Domain.Interfaces.Repositories;
using MyApp.Domain.Interfaces.Services;
using System;

namespace MyApp.Domain.Services
{
    /// <summary>
    /// Responsible for all business rules and validations, before data persist in the database.
    /// </summary>
    public class GitHubResultService : ServiceBase<GitHubResult>, IGitHubResultService
    {
        private readonly IGitHubResultRepository _gitHubResultRepository;

        public GitHubResultService(IGitHubResultRepository gitHubResultRepository)
            : base(gitHubResultRepository)
        {
            _gitHubResultRepository = gitHubResultRepository;
        }

        public GitHubResult SearchByTokenAndLastCommit(string token, DateTime lastCommit)
        {
            return _gitHubResultRepository.SearchByTokenAndLastCommit(token, lastCommit);
        }
    }
}
