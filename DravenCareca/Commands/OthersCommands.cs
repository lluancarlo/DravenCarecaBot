using Discord;
using Discord.Commands;
using DravenCareca.Services;
using System.Threading.Tasks;

namespace DravenCareca.Commands
{
    public class OthersCommands : ModuleBase<SocketCommandContext>
    {
        private readonly BaseService _baseService;
        private readonly SpeakService _speakService;

        public OthersCommands() 
        {
            _baseService = new BaseService();
            _speakService = new SpeakService();
        }

        [Command("aipreto", RunMode = RunMode.Async)]
        private async Task AiPreto()
        {
            var channel = (Context.User as IGuildUser)?.VoiceChannel;
            if (channel != null)
            {
                var audioClient = await channel.ConnectAsync();
                await _speakService.AudioAiPreto(audioClient);
                await channel.DisconnectAsync();
            }
        }

    }
}
