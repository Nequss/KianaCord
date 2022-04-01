using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using System.Runtime.InteropServices;

namespace KianaCord
{
    public class Bot
    {
        public DiscordSocketClient client;
        public ServiceProvider services;
        public CommandHandlingService commandHandler;

        private string token;

        public Bot(string token)
        {
            services = ConfigureServices();
            client = services.GetRequiredService<DiscordSocketClient>();
            commandHandler = services.GetRequiredService<CommandHandlingService>();

            this.token = token;
        }

        public async Task MainAsync()
        {
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            client.JoinedGuild += Client_JoinedGuild;

            await Task.Delay(-1);
        }

        private Task Client_JoinedGuild(SocketGuild arg)
        {
            foreach (var textChannel in arg.TextChannels)
                textChannel.SendMessageAsync($"OHAYO!!! MY NAME IS {client.CurrentUser.Username}\nNICE TO MEET YOU ALL!!! OWO!!!");


            return Task.CompletedTask;
        }

        private ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
                {
                    AlwaysDownloadUsers = true,
                    GatewayIntents = GatewayIntents.All,
                    LogLevel = LogSeverity.Debug
                }))
                .AddSingleton(new CommandService(new CommandServiceConfig
                {
                    LogLevel = LogSeverity.Debug
                }))
                .AddSingleton<CommandHandlingService>()
                .AddSingleton<HttpClient>()
                .BuildServiceProvider();
        }
    }
}
