using DiscordRPC;

namespace DisRPC;

public class RpcManager
{
    private static DiscordRpcClient? _client;

    public static void InitializeClient(string clientId)
    {
        _client = new DiscordRpcClient(clientId);
        _client.Initialize();
    }

    public static void Dispose()
    {
        _client?.Dispose();
    }

    public static void SetRpc(string details, string state, string largeImageKey, string largeImageText,
        string smallImageKey,
        string smallImageText, bool showTime)
    {
        _client?.SetPresence(new RichPresence
        {
            Details = details,
            State = state,
            Timestamps = showTime ? new Timestamps { Start = DateTime.UtcNow } : null,
            Assets = new Assets
            {
                LargeImageKey = largeImageKey,
                LargeImageText = largeImageText,
                SmallImageKey = smallImageKey,
                SmallImageText = smallImageText
            }
        });
    }
}