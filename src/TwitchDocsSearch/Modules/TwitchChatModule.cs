using Discord.Interactions;

namespace TwitchDocsSearch.Modules
{
    public partial class DocsModule : InteractionModuleBase<SocketInteractionContext>
    {
        [Group("chat", "Get information about specific sections of the Twitch Chat API.")]
        public class TwitchChatModule : InteractionModuleBase<SocketInteractionContext>
        {

        }
    }
}
