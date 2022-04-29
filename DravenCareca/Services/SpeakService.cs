using System.Threading.Tasks;
using Discord.Audio;
using System.Diagnostics;

namespace DravenCareca.Services
{
    public class SpeakService
    {
        private readonly string fala_aprenda_com_o_mestre = "../../../src/aprenda_com_mestre.mp3";
        private readonly string fala_draven_brilha = "../../../src/draven_brilha.mp3";
        private readonly string fala_tenho_melhor_trabalho = "../../../src/tenho_melhor_trabalho.mp3";
        private readonly string fala_draven_faz_tudo_com_estilo = "../../../src/draven_faz_tudo_com_estilo.mp3";
        private readonly string fala_nao_e_draven = "../../../src/nao_e_draven.mp3";

        public async Task AudioAjuda(IAudioClient audioClient)
        {
            await Speak(audioClient, fala_aprenda_com_o_mestre);
        }

        public async Task AudioPartida(IAudioClient audioClient)
        {
            await Speak(audioClient, fala_draven_brilha);
        }

        public async Task AudioHistorico(IAudioClient audioClient)
        {
            await Speak(audioClient, fala_draven_faz_tudo_com_estilo);
        }

        public async Task AudioDraven(IAudioClient audioClient)
        {
            await Speak(audioClient, fala_nao_e_draven);
        }

        private async Task Speak(IAudioClient audioClient, string sound)
        {
            using (var ffmpeg = CreateProcess(sound))
            using (var stream = audioClient.CreatePCMStream(AudioApplication.Music))
            {
                await ffmpeg.StandardOutput.BaseStream.CopyToAsync(stream);
                await stream.FlushAsync();
            }
        }

        private Process CreateProcess(string path)
        {
            return Process.Start(new ProcessStartInfo
            {
                FileName = "../../../ffmpeg.exe",
                Arguments = $"-hide_banner -loglevel panic -i \"{path}\" -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true
            });
        }
    }
}
