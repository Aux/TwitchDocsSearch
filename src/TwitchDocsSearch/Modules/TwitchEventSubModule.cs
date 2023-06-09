﻿using Discord.Interactions;

namespace TwitchDocsSearch.Modules
{
    public partial class DocsModule : InteractionModuleBase<SocketInteractionContext>
    {
        [Group("eventsub", "Get information about specific sections of the Twitch EventSub API.")]
        public class TwitchEventSubModule : InteractionModuleBase<SocketInteractionContext>
        {

        }
    }
}
