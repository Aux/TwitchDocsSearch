using Discord;
using Discord.Interactions;

namespace TwitchDocsSearch.Modules
{
    public partial class DocsModule : InteractionModuleBase<SocketInteractionContext>
    {
        [Group("rest", "Get information about specific sections of the Twitch REST API.")]
        public class TwitchRestModule : InteractionModuleBase<SocketInteractionContext>
        {
            [SlashCommand("link", "Get a link to the Twitch REST API documentation.")]
            public Task GetLinkAsync()
                => RespondAsync(DocsConstants.RestUrl);
        }
    }
}
