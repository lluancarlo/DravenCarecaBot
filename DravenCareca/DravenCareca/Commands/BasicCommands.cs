using DravenCareca.Services;
using Discord.Commands;
using System.Threading.Tasks;
using Discord;

namespace DravenCareca.Commands
{
    public class BasicCommands : ModuleBase<SocketCommandContext>
    {
        private readonly BaseService _baseService;
        private readonly SpeakService _speakService;

        public BasicCommands()
        {
            _baseService = new BaseService();
            _speakService = new SpeakService();
        }

        [Command("ajuda", RunMode = RunMode.Async)]
        private async Task help()
        {
            var result = _baseService.showHelp();
            await ReplyAsync(result);

            var channel = (Context.User as IGuildUser)?.VoiceChannel;
            if (channel != null) 
            {
                var audioClient = await channel.ConnectAsync();
                await _speakService.AudioAjuda(audioClient);
                await channel.DisconnectAsync();
            }
        }

        [Command("draven", RunMode = RunMode.Async)]
        private async Task draven()
        {
            var channel = (Context.User as IGuildUser)?.VoiceChannel;
            if (channel != null)
            {
                var audioClient = await channel.ConnectAsync();
                await _speakService.AudioDraven(audioClient);
                await channel.DisconnectAsync();
            }
        }
    }
}
