using Discord.Interactions;

namespace TwitchDocsSearch.Modules
{
    [Group("docs", "Twitch developer documentation commands")]
    public partial class DocsModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("link", "Get a link to the Twitch developer documentation.")]
        public Task GetLinkAsync()
            => RespondAsync(DocsConstants.DocsBaseUrl);
    }
}
