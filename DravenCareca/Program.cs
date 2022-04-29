using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DravenCareca.Dto;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace DravenCareca
{
    class Program
    {
        public static DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;
        private BotConfigDto config;

        static void Main(string[] args) => new Program().RunBot().GetAwaiter().GetResult();

        // Runbot task
        public async Task RunBot()
        {
            _client = new DiscordSocketClient();
            _commands = new CommandService();
            _services = new ServiceCollection().AddSingleton(_client).AddSingleton(_commands).BuildServiceProvider();

            try
            {
                config = JsonConvert.DeserializeObject<BotConfigDto>(File.ReadAllText("config.json"));
            }
            catch (Exception)
            {
                throw;
            }

            // Configura Log
            _client.Log += Log;

            // Configura Comandos
            _client.MessageReceived += HandleCommandAsync;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), null);

            // Login e Start
            await _client.LoginAsync(TokenType.Bot, config.token);
            await _client.StartAsync();

            // Seta Bot Status
            await _client.SetGameAsync(config.game);

            // Mantém console aberto
            await Task.Delay(-1);
        }

        private Task Log(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;

            if (message is null || message.Author.IsBot)
                return;

            int argumentPos = 0;
            if (message.HasStringPrefix(config.prefix, ref argumentPos) || message.HasMentionPrefix(_client.CurrentUser, ref argumentPos))
            {
                var context = new SocketCommandContext(_client, message);
                var result = await _commands.ExecuteAsync(context, argumentPos, _services);
                if (!result.IsSuccess)
                {
                    Console.WriteLine(result.ErrorReason);
                    await message.Channel.SendMessageAsync(this.ErrorMessage(result.Error));
                }
            }
        }

        private string ErrorMessage(CommandError? error)
        {
            return error switch
            {
                CommandError.UnknownCommand => "Draven não conhece isso.",
                _ => error.ToString()
            };
        }
    }
}
