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

            client.Connected += Client_Connected;
            client.Log += Client_Log;

            await Task.Delay(-1);
        }

        private Task Client_Log(LogMessage arg)
        {
            Console.WriteLine($"{arg.Message}");
            return Task.CompletedTask;
        }

        private Task Client_Connected()
        {
            client.GetGuild(959414997321134080).GetTextChannel(959414997321134084).SendMessageAsync("am not ready for some chaos yet!!!\nwe need few minutes");
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
