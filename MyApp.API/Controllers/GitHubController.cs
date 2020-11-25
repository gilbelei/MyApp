using GetGitHub.Domain.Entities;
using MyApp.Application;
using MyApp.Domain.Services;
using MyApp.Infra.Data.Repositories;
using System.Collections.Generic;
using System.Web.Http;

namespace MyApp.API.Controllers
{
    public class GitHubController : ApiController
    {

        /// <summary>
        /// Returns the total number of lines and the total number of bytes of all files in the public Github repository, by file extension.
        /// </summary>
        /// <param name="user">Github user</param>
        /// <param name="repo">Github repository</param>
        /// <returns>Listing by file extension with the number of lines and total bytes.</returns>
        public List<WebScrapingResult> Get(string user, string repo)
        {
            GitHubResultAppService _gitHubResultApp = new GitHubResultAppService(new GitHubResultService(new GitHubResultRepository()));
            return _gitHubResultApp.GetGitHub(user, repo);
        }
    }
}
