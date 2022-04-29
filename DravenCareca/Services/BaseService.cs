namespace DravenCareca.Services
{
    public class BaseService
    {
        public string showHelp()
        {
            return @"__Draven vai ensinar só essa vez:__
                **!partida <nick>**  -  Busca partida do invocador em andamento.
                **!historico <nick>**  -  Busca histórico do invocador.
                **!draven**  -  ???";
        }
    }
}
