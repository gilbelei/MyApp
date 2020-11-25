using GetGitHub.Domain.Entities;
using MyApp.Domain.Entities;
using MyApp.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;


namespace MyApp.Application
{
    /// <summary>
    /// Responsible for all business rules and validations.
    /// </summary>
    public class GitHubResultAppService : AppServiceBase<GitHubResult>
    {
        private readonly IGitHubResultService _gitHubResultService;
        private string link = string.Empty;

        public GitHubResultAppService(IGitHubResultService gitHubResultervice)
            : base(gitHubResultervice)
        {
            _gitHubResultService = gitHubResultervice;
        }

        /// <summary>
        /// Returns the total number of lines and the total number of bytes of all files in the public Github repository, by file extension.
        /// </summary>
        /// <param name="user">Github user</param>
        /// <param name="repo">Github repository</param>
        /// <returns>Listing by file extension with the number of lines and total bytes.</returns>
        public List<WebScrapingResult> GetGitHub(string user, string repository)
        {
            var files = new List<FileContent>();
            try
            {
                string resultGitHub = string.Empty;
                string CommitToken = string.Empty;
                DateTime LastCommit = DateTime.MinValue;
                ReturnsLastCommit($"https://github.com/{user}/{repository}", $"https://github.com", out CommitToken, out LastCommit);
                GitHubResult gitHubResultSave = _gitHubResultService.SearchByTokenAndLastCommit(CommitToken, LastCommit);

                if(gitHubResultSave != null)
                {
                    if(LastCommit > gitHubResultSave.LastCommit || LastCommit == DateTime.MinValue)
                    {
                        AnalyzesContent($"https://github.com/{user}/{repository}", $"https://github.com", files);

                        var result = files.Where(x => x.Type == "file")
                             .GroupBy(x => x.Extension)
                             .Select(x => new WebScrapingResult
                             {
                                 FileExtension = x.Key,
                                 Size = x.Sum(y => ConvertToBytes(y)),
                                 NumberOfLines = x.Sum(y => y.NumberOfLines),
                                 Result = "Success"
                             });

                        List<WebScrapingResult> lst = result.OrderBy(x => x.FileExtension).ToList();
                        resultGitHub = Serialize<List<WebScrapingResult>>(lst);

                        GitHubResult gitHubResult = new GitHubResult
                        {
                            CommitToken = CommitToken,
                            LastCommit = LastCommit,
                            RepositoryGitHub = repository,
                            UserGitHub = user,
                            ResultGitHub = resultGitHub
                        };

                        _gitHubResultService.Add(gitHubResult);

                    }
                    else
                    {
                        resultGitHub = gitHubResultSave.ResultGitHub;
                    }
                }
                else
                {
                    AnalyzesContent($"https://github.com/{user}/{repository}", $"https://github.com", files);

                    var result = files.Where(x => x.Type == "file")
                         .GroupBy(x => x.Extension)
                         .Select(x => new WebScrapingResult
                         {
                             FileExtension = x.Key,
                             Size = x.Sum(y => ConvertToBytes(y)),
                             NumberOfLines = x.Sum(y => y.NumberOfLines),
                             Result = "Success"
                         });

                    List<WebScrapingResult> lst = result.OrderBy(x => x.FileExtension).ToList();
                    resultGitHub = Serialize<List<WebScrapingResult>>(lst);

                    GitHubResult gitHubResult = new GitHubResult
                    {
                        CommitToken = CommitToken,
                        LastCommit = LastCommit,
                        RepositoryGitHub = repository,
                        UserGitHub = user,
                        ResultGitHub = resultGitHub
                    };

                    _gitHubResultService.Add(gitHubResult);
                }

                List<WebScrapingResult> values = DeSerialize<List<WebScrapingResult>>(resultGitHub);
                return values;
            }
            catch (Exception ex)
            {
                var result = new List<WebScrapingResult>();
                WebScrapingResult wsr = new WebScrapingResult();
                wsr.Result = "An error occurred while trying to extract information from Github. If the error persists, contact your system administrator. ERROR: " + ex.Message;
                result.Add(wsr);
                return result;
            }
        }

        /// <summary>
        /// Returns Last Commit Information
        /// </summary>
        /// <param name="pageUrl">Github repository link</param>
        /// <param name="hostName">Github link</param>
        /// <param name="commitToken">Commit Token</param>
        /// <param name="lastCommit">Last commit update</param>
        private void ReturnsLastCommit(string pageUrl, string hostName, out string commitToken, out DateTime lastCommit)
        {
            var client = new WebClient();
            string dataStr = string.Empty;
            
            commitToken = string.Empty;
            lastCommit = DateTime.MinValue;


            DateTime Tthen = DateTime.Now.AddSeconds(1);//Minimum time in seconds to make a new request to Github
            while (Tthen > DateTime.Now) {}

            byte[] data = client.DownloadData(pageUrl);
            dataStr = client.Encoding.GetString(data);
                        

            string[] lines = dataStr.Split(new string[] { "\n" }, StringSplitOptions.None);

            for (int i = 0; i < lines.Length; i++)
            {
                string l = lines[i];

                int commit = l.IndexOf("<time-ago datetime=");
                if (commit > 0)
                {
                    string date = lines[i].Remove(0, lines[i].IndexOf("datetime=") + 10);

                    DateTime.TryParse(date.Remove(date.IndexOf("\"")), out lastCommit);
                    //commitToken = lines[i - 5].Remove(0, lines[i - 5].IndexOf("/commit/") + 8);
                    //commitToken = commitToken.Remove(commitToken.IndexOf("\">"));

                    if (commitToken != string.Empty && lastCommit > DateTime.MinValue)
                    {
                        break;
                    }
                }
                commit = l.IndexOf("<relative-time datetime=");
                if (commit > 0)
                {
                    string date = lines[i].Remove(0, lines[i].IndexOf("datetime=") + 10);

                    DateTime.TryParse(date.Remove(date.IndexOf("\"")), out lastCommit);
                    //commitToken = lines[i - 1].Remove(0, lines[i - 1].IndexOf("/commit/") + 8);
                    //commitToken = commitToken.Remove(commitToken.IndexOf("\""));

                    if (commitToken != string.Empty && lastCommit > DateTime.MinValue)
                    {
                        break;
                    }
                }
                commit = l.IndexOf("/commit/");
                if (commit > 0)
                {
                    commitToken = lines[i - 1].Remove(0, lines[i - 1].IndexOf("/commit/") + 8);
                    commitToken = commitToken.Remove(commitToken.IndexOf("\""));


                    if (commitToken != string.Empty && lastCommit > DateTime.MinValue)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Serializes the specified obj to json.  
        /// </summary>
        /// <typeparam name="T">Type of obj</typeparam>
        /// <param name="obj">The obj</param>
        /// <returns>Json</returns>
        private string Serialize<T>(T obj)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream();
            serializer.WriteObject(ms, obj);
            return Encoding.UTF8.GetString(ms.ToArray());
        }

        /// <summary>
        /// Deserializes the specified json to obj.
        /// </summary>
        /// <typeparam name="T">Type of obj</typeparam>
        /// <param name="json">Json</param>
        /// <returns>The obj</returns>
        private T DeSerialize<T>(string json)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            return (T)ser.ReadObject(ms);
        }

        /// <summary>
        /// Analyzes the content of the repository on the user's Github and returns the list of files with their information.
        /// </summary>
        /// <param name="pageUrl">Github repository link</param>
        /// <param name="hostName">Github link</param>
        /// <param name="filesContent">List of files</param>
        private void AnalyzesContent(string pageUrl, string hostName, List<FileContent> filesContent)
        {
            try
            {
                var client = new WebClient();
                string dataStr = string.Empty;
                DateTime Tthen = DateTime.Now.AddSeconds(2);//Minimum time in seconds to make a new request to Github
                while (Tthen > DateTime.Now) { }

                byte[] data = client.DownloadData(pageUrl);
                dataStr = client.Encoding.GetString(data);
                
                string[] lines = dataStr.Split(new string[] { "\n" }, StringSplitOptions.None);

                for (int i = 0; i < lines.Length; i++)
                {
                    string l = lines[i];

                                   
                    int Directory = l.IndexOf("<svg aria-label=\"Directory\"");
                    if (Directory > 0)
                    {
                        link = GetLink(hostName, lines[i + 5]);
                        AnalyzesContent(link, hostName, filesContent);
                    }
                    int File = l.IndexOf("<svg aria-label=\"File\"");
                    if (File > 0)
                    {
                        link = GetLink(hostName, lines[i + 5]);
                        string file = GetDescriptionLink(lines[i + 5]);
                        string extension = GetExtension(file, true);
                        filesContent.Add(GetFileData(link, file, extension));
                    }

                    link = string.Empty;
                }
            }
            catch (System.Net.WebException wex)
            {

                DateTime Tthen = DateTime.Now.AddSeconds(5);//Minimum time in seconds to make a new request to Github
                while (Tthen > DateTime.Now) { }
                
                AnalyzesContent(link, hostName, filesContent);
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Returns the file entity filled with the data
        /// </summary>
        /// <param name="link">File link</param>
        /// <param name="file">File name</param>
        /// <param name="extension">File extension</param>
        /// <returns>File entity with data</returns>
        private FileContent GetFileData(string link, string file, string extension)
        {
            FileContent fileContent = new FileContent();
            fileContent.Url = link;
            fileContent.Name = file;
            fileContent.Extension = extension;
            fileContent.Type = "file";

            var client = new WebClient();
            byte[] data = client.DownloadData(link);
            string dataStr = client.Encoding.GetString(data);

            string[] lines = dataStr.Split(new string[] { "\n" }, StringSplitOptions.None);

            for (int i = 0; i < lines.Length; i++)
            {
                string l = lines[i];
                int Header = l.IndexOf("<div class=\"text-mono");
                if (Header > 0)
                {
                    decimal size = 0;
                    string sizeUnit = string.Empty;
                    GetSizeTheHeader(lines[i + 4], out size, out sizeUnit);
                    if (size > 0)
                    {
                        fileContent.Size = size;
                        fileContent.NumberOfLines = GetNumberOfLinesTheHeader(lines[i + 2]);
                    }
                    else
                    {
                        GetSizeTheHeader(lines[i + 2], out size, out sizeUnit);
                        fileContent.Size = size;
                    }
                    fileContent.SizeUnit = sizeUnit;
                }
            }

            return fileContent;

        }

        /// <summary>
        /// Returns the file size and drive type from the header
        /// </summary>
        /// <param name="line">Line with file size</param>
        /// <param name="size">File size</param>
        /// <param name="sizeUnit">Unit of measurement of file size</param>
        private void GetSizeTheHeader(string line, out decimal size, out string sizeUnit)
        {
            string[] lines = line.Split(Convert.ToChar(" "));
            decimal sizeTemp = 0;
            sizeUnit = string.Empty;
            size = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                if (!lines[i].Equals("") && decimal.TryParse(lines[i], out sizeTemp))
                {
                    size = sizeTemp;
                }
                else if (!lines[i].Equals(""))
                {
                    sizeUnit = lines[i].ToLower();
                    break;
                }
            }
        }

        /// <summary>
        /// Return the number of lines coming from the file header
        /// </summary>
        /// <param name="line">Line with number of lines</param>
        /// <returns>Number of lines</returns>
        private long GetNumberOfLinesTheHeader(string line)
        {
            string[] lines = line.Split(Convert.ToChar(" "));
            long NumberOfLines = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                if (!lines[i].Equals(""))
                {
                    long.TryParse(lines[i], out NumberOfLines);
                    return NumberOfLines;
                }
            }
            return NumberOfLines;
        }

        /// <summary>
        /// Retrieves the URL from an HTML line
        /// </summary>
        /// <param name="line">Line containing the HTML link</param>
        /// <returns>File or folder link</returns>
        private string GetLink(string hostName, string line)
        {
            string link = line.Remove(0, line.IndexOf("href=") + 6);
            link = link.Remove(link.IndexOf("\">"));
            return hostName + link;
        }

        /// <summary>
        /// Retrieves the link description from an HTML tag
        /// </summary>
        /// <param name="line">Line containing the HTML tag</param>
        /// <returns>Link description</returns>
        private string GetDescriptionLink(string line)
        {
            string file = line.Remove(0, line.IndexOf("\">") + 2);
            file = file.Remove(0, file.IndexOf("\">") + 2);
            file = file.Remove(file.IndexOf("</a>"));
            return file;
        }

        /// <summary>
        /// Recovers file extensions based on name.
        /// </summary>
        /// <param name="file">File name</param>
        /// <param name="hiddenFiles">Consider hidden files as an extension?</param>
        /// <returns>File extension</returns>
        private string GetExtension(string file, bool hiddenFiles)
        {
            string extention = string.Empty;
            if (hiddenFiles)
            {
                string[] Arrfile = file.Split(Convert.ToChar("."));
                extention = Arrfile[Arrfile.Count() - 1];
            }
            else
            {
                var count = file.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                extention = count.Length == 1 ? string.Empty : count[1];
            }

            return extention;
        }

        /// <summary>
        /// Converts the file size to bytes.
        /// </summary>
        /// <param name="fileContent">File data</param>
        /// <returns>Data in bytes</returns>
        private long ConvertToBytes(FileContent fileContent)
        {
            if (fileContent.SizeUnit == "byte" || fileContent.SizeUnit == "bytes")
            {
                return (long)fileContent.Size;
            }

            if (fileContent.SizeUnit == "kb")
            {
                return (long)fileContent.Size * 1024;
            }

            if (fileContent.SizeUnit == "mb")
            {
                return (long)fileContent.Size * 1024 * 1024;
            }

            return 0;

           // throw new Exception("Invalid value:" + fileContent.SizeUnit);

        }
    }
}
