using Discord;
using Discord.Interactions;
using System.Text;
using System.Text.Json;
using static System.Formats.Asn1.AsnWriter;

namespace TwitchDocsSearch.Modules
{
    public partial class DocsModule : InteractionModuleBase<SocketInteractionContext>
    {
        [Group("rest", "Get information about specific sections of the Twitch REST API.")]
        public class TwitchRestModule : InteractionModuleBase<SocketInteractionContext>
        {
            [SlashCommand("scoped", "Get endpoints that require the specified scope.")]
            public Task GetScopedEndpointsAsync(string scope)
            {
                var endpoints = GetRestReferences();

                IEnumerable<RestReference>? selected;
                if (scope.EndsWith('*'))
                {
                    var wildcard = scope.Substring(0, scope.Length - 2);
                    selected = endpoints?.Where(x => x.Scopes?.Any(x => x.StartsWith(wildcard)) == true);
                }
                else
                    selected = endpoints?.Where(x => x.Scopes?.Contains(scope.ToLower()) == true);

                if (selected == null || selected.Count() == 0)
                    return RespondAsync($"There are no endpoints that require the scope `{scope}`.");

                var builder = new StringBuilder();
                foreach (var endpoint in selected.Take(10))
                {
                    builder.AppendLine(Format.Url(endpoint.Name,
                        $"{DocsConstants.RestReferenceUrl}#{endpoint.Id}")
                        + Environment.NewLine
                        + endpoint.Description);
                    builder.AppendLine();
                }

                string footer;
                if (selected.Count() > 10)
                    footer = $"And {selected.Count() - 10} more";
                else
                    footer = $"Required scope: {scope}";

                var embed = new EmbedBuilder()
                    .WithTitle($"Results")
                    .WithDescription(builder.ToString())
                    .WithFooter(footer)
                    .Build();

                return RespondAsync(text: $"Endpoints that require the `{scope}` scope", embed: embed);
            }


            [SlashCommand("search", "Find sections of the api related to the specified query.")]
            public Task SearchAsync(string query)
            {
                var endpoints = GetRestReferences();
                var lowerQ = query.ToLower();

                var selected = endpoints?.Where(x => 
                    x.Name.ToLower().Contains(lowerQ) ||
                    x.Remarks.ToLower().Contains(lowerQ));

                if (selected == null || selected.Count() == 0)
                    return RespondAsync($"There are no endpoints like `{query}`.");

                var builder = new StringBuilder();
                foreach (var endpoint in selected.Take(10))
                {
                    builder.AppendLine(Format.Url(endpoint.Name,
                        $"{DocsConstants.RestReferenceUrl}#{endpoint.Id}")
                        + Environment.NewLine
                        + endpoint.Description);
                    builder.AppendLine();
                }

                string footer;
                if (selected.Count() > 10)
                    footer = $"And {selected.Count() - 10} more";
                else
                    footer = $"From query: {query}";

                var embed = new EmbedBuilder()
                    .WithTitle($"Results")
                    .WithDescription(builder.ToString())
                    .WithFooter(footer)
                    .Build();

                return RespondAsync(text: $"Endpoints like `{query}`", embed: embed);
            }

            private IEnumerable<RestReference>? GetRestReferences()
            {
                var cacheDir = Path.Combine(AppContext.BaseDirectory, "cache");
                var uri = new Uri(DocsConstants.RestReferenceUrl);
                var uriPath = uri.ToString()
                .Replace(uri.GetLeftPart(UriPartial.Authority), "")
                .Replace('/', '_')
                .Substring(1);
                var cacheFile = Path.Combine(cacheDir, uriPath + ".html.json");

                var json = File.ReadAllText(cacheFile);
                return JsonSerializer.Deserialize<List<RestReference>>(json);
            }
        }
    }
}
