namespace BlazorPatchDemo.Server.Settings;

public class MongoDbSettings
{
    public string Host { get; init; } = string.Empty;

    public int Port { get; init; } = 27021;

    public string ConnectionString => $"mongodb://{Host}:{Port}";
}
