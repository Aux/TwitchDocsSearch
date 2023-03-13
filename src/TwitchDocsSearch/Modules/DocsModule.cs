using Discord;
using Discord.Interactions;

namespace TwitchDocsSearch.Modules
{
    [Group("docs", "Twitch developer documentation commands")]
    public partial class DocsModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("link", "Get a link to the Twitch developer documentation.")]
        public Task GetLinkAsync(
            [Choice(nameof(DocsLinkChoice.MainPage), (int)DocsLinkChoice.MainPage)]
            [Choice(nameof(DocsLinkChoice.Rest), (int)DocsLinkChoice.Rest)]
            [Choice(nameof(DocsLinkChoice.Chat), (int)DocsLinkChoice.Chat)]
            [Choice(nameof(DocsLinkChoice.EventSub), (int)DocsLinkChoice.EventSub)]
            [Choice(nameof(DocsLinkChoice.PubSub), (int)DocsLinkChoice.PubSub)]
            DocsLinkChoice section = DocsLinkChoice.MainPage)
        {
            var url = section switch
            {
                DocsLinkChoice.MainPage => DocsConstants.DocsBaseUrl,
                DocsLinkChoice.Rest => DocsConstants.RestUrl,
                DocsLinkChoice.Chat => DocsConstants.ChatUrl,
                DocsLinkChoice.EventSub => DocsConstants.EventSubUrl,
                DocsLinkChoice.PubSub => DocsConstants.PubSubUrl,
                _ => DocsConstants.DocsBaseUrl
            };

            return RespondAsync(Format.EscapeUrl(url));
        }
    }
}
