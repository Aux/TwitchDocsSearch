using HtmlAgilityPack;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace TwitchDocsSearch.Services
{
    public class TwitchDocsDownloader : BackgroundService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<TwitchDocsDownloader> _logger;

        public TwitchDocsDownloader(HttpClient httpClient, ILogger<TwitchDocsDownloader> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var page = await GetOrDownloadPageAsync(new Uri(DocsConstants.RestReferenceUrl));

            var html = new HtmlDocument();
            html.LoadHtml(page.Content);

            // Get a list of all endpoints from the table of contents
            var toc = html.DocumentNode.SelectSingleNode("//section[@class='left-docs']");
            var tocItems = toc.SelectNodes("table/tbody/tr");

            var partialEndpoints = new List<RestPartialReference>();
            foreach (var item in tocItems)
            {
                string category = item.SelectSingleNode("td").InnerText;
                string id = item.SelectSingleNode("td/a").Attributes["href"].Value.Substring(1);
                string name = item.SelectSingleNode("td/a").InnerText;
                string description = item.SelectSingleNode("td/p").InnerText;

                partialEndpoints.Add(new()
                {
                    Category = category,
                    Id = id,
                    Name = name,
                    Description = description,
                });
            }

            // Get endpoint information from each section
            var endpoints = new List<RestReference>();
            foreach (var item in partialEndpoints)
            {
                try
                {
                    var section = html.DocumentNode
                        .SelectSingleNode($"//section[@class='left-docs'][./h2[@id='{item.Id}']]");

                    // Description and remarks
                    var endpoint = new RestReference(item);
                    foreach (var node in section.ChildNodes)
                    {
                        if (node.InnerText == "Authorization")
                            break;
                        if (node.Name != "p")
                            continue;

                        endpoint.Remarks += node.InnerText + Environment.NewLine;
                    }
                    endpoint.Remarks = endpoint.Remarks.Trim();

                    var authorization = section.ChildNodes.FirstOrDefault(x => x.InnerText.ToLower().Contains("access token"))?.InnerHtml;
                    if (authorization == null)
                        authorization = section.ChildNodes.FirstOrDefault(x => x.InnerText.ToLower().Contains("json web token"))?.InnerHtml;

                    if (authorization != null)
                    {
                        var authHtml = new HtmlDocument();
                        authHtml.LoadHtml(authorization);
                        var scopes = authHtml.DocumentNode.SelectNodes("strong");
                        if (scopes != null)
                            endpoint.Scopes = scopes.Select(x => x.InnerText).ToArray();
                    }

                    endpoint.Authorization = authorization;
                    endpoint.ApiUrl = section.SelectSingleNode("p/code").InnerText;

                    // Request Body
                    // Response Body
                    // Response Codes

                    endpoints.Add(endpoint);

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, null);
                }
            }

            var json = JsonSerializer.Serialize(endpoints, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText(page.Info.FilePath + ".json", json);
            _logger.LogInformation($"Saved parsed json in `{page.Info.FilePath + ".json"}`");
        }

        private async Task<(PageInfo Info, string Content)> GetOrDownloadPageAsync(Uri uri)
        {
            var cacheDir = Path.Combine(AppContext.BaseDirectory, "cache");
            if (!Directory.Exists(cacheDir))
            {
                _logger.LogInformation($"Cache directory `{Path.GetFileName(cacheDir)}` not found, creating...");
                Directory.CreateDirectory(cacheDir);
            }

            var uriPath = uri.ToString()
                .Replace(uri.GetLeftPart(UriPartial.Authority), "")
                .Replace('/', '_')
                .Substring(1);
            var cacheFile = Path.Combine(cacheDir, uriPath + ".html");

            var info = new PageInfo
            {
                Url = uri.ToString(),
                FilePath = cacheFile
            };

            if (File.Exists(cacheFile))
                return (info, File.ReadAllText(cacheFile));

            _logger.LogInformation($"Cache file `{Path.GetFileName(cacheFile)}` for `{uri}` not found, downloading page...");
            var content = await _httpClient.GetStringAsync(uri);
            File.WriteAllText(cacheFile, content);

            _logger.LogInformation($"Created cache file `{Path.GetFileName(cacheFile)}` for `{uri}`");
            return (info, content);
        }
    }
}
