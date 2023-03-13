using Discord;
using Discord.Interactions;

namespace TwitchDocsSearch.Modules
{
    public partial class DocsModule : InteractionModuleBase<SocketInteractionContext>
    {
        public class InfoModule : InteractionModuleBase<SocketInteractionContext>
        {
            [SlashCommand("appinfo", "Get information about the application")]
            public async Task GetInfoAsync()
            {
                var app = await Context.Client.GetApplicationInfoAsync();

                var embed = new EmbedBuilder()
                    .WithTitle(app.Name)
                    .WithDescription(app.Description)
                    .AddField("Source", app.TermsOfService)
                    .WithFooter($"by {app.Owner} ({app.Owner.Id})")
                    .Build();

                await RespondAsync(embed: embed, ephemeral: true);
            }
        }
    }
}