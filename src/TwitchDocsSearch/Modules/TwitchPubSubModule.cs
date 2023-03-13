using Discord.Interactions;

namespace TwitchDocsSearch.Modules
{
    public partial class DocsModule : InteractionModuleBase<SocketInteractionContext>
    {
        [Group("pubsub", "Get information about specific sections of the Twitch PubSub API.")]
        public class TwitchPubSubModule : InteractionModuleBase<SocketInteractionContext>
        {
            [SlashCommand("link", "Get a link to the Twitch PubSub API documentation.")]
            public Task GetLinkAsync()
                => RespondAsync(DocsConstants.PubSubUrl);
        }
    }
}
