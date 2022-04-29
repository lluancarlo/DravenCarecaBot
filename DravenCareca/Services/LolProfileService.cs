using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DravenCareca.Services
{
    public class LolProfileService
    {
        private HttpClient _client;

        public LolProfileService()
        {
            _client = new HttpClient();
        }

        public async Task<string> searchUserMatch(string name)
        {

            String formated_string = String.Format("https://lolprofile.net/index.php?page=summoner&ajax=livegames&region=br&name={0}", name);
            HttpResponseMessage response = await _client.GetAsync(formated_string);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return formatMatchHtml(responseBody);
        }
        private string formatMatchHtml(string html)
        {
            string formatedContent = string.Empty;
            var parser = new AngleSharp.Html.Parser.HtmlParser();
            var doc = parser.ParseDocument(html);

            var match = doc.GetElementsByTagName("div")[0].InnerHtml;

            if (match == "Summoner is not currently in a game!")
                return "Este jogador não está em partida, cabeçudo.";

            var time = doc.GetElementsByTagName("div")[1].GetElementsByTagName("span").First().InnerHtml;
            formatedContent += "**Partida** " + match + " **|** Em " + time + " minutos de jogo. \n";
            formatedContent += "---------------------------------------------------------- \n";

            var index = 0;
            var players = doc.GetElementsByClassName("cf lg-ss");
            foreach (var player in players)
            {
                if (index == 5)
                    formatedContent += "---------------------------------------------------------- \n";

                var c1 = player.GetElementsByClassName("c1").First();
                var champion = c1.GetElementsByTagName("img")[0].GetAttribute("alt");
                //var speel1 = c1.GetElementsByTagName("img")[1].GetAttribute("alt");
                //var speel2 = c1.GetElementsByTagName("img")[2].GetAttribute("alt");

                //var c2 = player.GetElementsByClassName("c2").First();
                //var perk = c2.GetAttribute("title");

                var c3 = player.GetElementsByClassName("c3").First();
                var name = c3.GetElementsByTagName("a").First().InnerHtml;
                var tier = c3.GetElementsByTagName("div").First().InnerHtml;

                //var c4 = player.GetElementsByClassName("c4").First();
                //var winlose = c4.GetElementsByTagName("div")[0].InnerHtml;
                //var winrate = c4.GetElementsByTagName("div")[1].InnerHtml;

                index++;
                formatedContent +=
                    (" **Invocador:** " + name).PadRight(40) +
                    (" **Campeão:** " + champion).PadRight(30) +
                    (" **Elo:** " + tier).PadRight(30) +
                    " \n";
            }

            return formatedContent;
        }

        public async Task<string> searchUserHistoric(string name)
        {

            String formated_string = String.Format("https://lolprofile.net//pt/summoner/br/{0}", name);
            HttpResponseMessage response = await _client.GetAsync(formated_string);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return formatHistoricHtml(responseBody);
        }

        public string formatHistoricHtml(string html)
        {
            return "NAO IMPLEMENTADO!";
        }

    }
}
