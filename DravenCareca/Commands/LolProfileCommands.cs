using DravenCareca.Services;
using Discord.Commands;
using System.Threading.Tasks;
using Discord;

namespace DravenCareca.Commands
{
    public class LolProfileCommands : ModuleBase<SocketCommandContext>
    {
        private readonly LolProfileService _lolProfileService;
        private readonly SpeakService _speakService;
        public LolProfileCommands()
        {
            _lolProfileService = new LolProfileService();
            _speakService = new SpeakService();
        }

        [Command("partida", RunMode = RunMode.Async)]
        private async Task PlayerMatch(string name)
        {
            var result = await _lolProfileService.searchUserMatch(name);
            await ReplyAsync(result);

            var channel = (Context.User as IGuildUser)?.VoiceChannel;
            if (channel != null)
            {
                var audioClient = await channel.ConnectAsync();
                await _speakService.AudioPartida(audioClient);
                await channel.DisconnectAsync();
            }
        }

        [Command("historico", RunMode = RunMode.Async)]
        private async Task PlayerHistoric(string name)
        {
            var result = await _lolProfileService.searchUserHistoric(name);
            await ReplyAsync(result);

            var channel = (Context.User as IGuildUser)?.VoiceChannel;
            if (channel != null)
            {
                var audioClient = await channel.ConnectAsync();
                await _speakService.AudioHistorico(audioClient);
                await channel.DisconnectAsync();
            }
        }
    }
}
