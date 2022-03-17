namespace PodcastManager.Administration.CrossCutting.Rabbit;

public static class RabbitConfiguration
{
    public static readonly string Host =
        Environment.GetEnvironmentVariable("RabbitUrl")
        ?? "localhost";
    public static string PublishAllFromPlaylistQueue { get; } =
        Environment.GetEnvironmentVariable("PublishAllFromPlaylists")
        ?? "PodcastManager.PublishAllFromPlaylists";
}