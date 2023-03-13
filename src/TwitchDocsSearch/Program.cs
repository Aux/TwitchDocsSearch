using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TwitchDocsSearch.Services;

using var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(config =>
    {
        config.AddYamlFile("_config.yml");
    })
    .ConfigureServices(services =>
    {
        services.AddSingleton(new DiscordSocketConfig 
        { 
            LogGatewayIntentWarnings = false
        });
        services.AddSingleton(new InteractionServiceConfig 
        { 
            LogLevel = Discord.LogSeverity.Verbose,
            DefaultRunMode = RunMode.Async
        });

        services.AddSingleton<HttpClient>();
        services.AddSingleton<DiscordSocketClient>();
        services.AddSingleton<InteractionService>();
        services.AddHostedService<DiscordStartupService>();
        services.AddHostedService<InteractionHandlingService>();
        services.AddHostedService<TwitchDocsDownloader>();
    })
    .Build();

await host.RunAsync();