namespace MediaService.Shared;

public class AppOptions
{
}

public sealed class MinioSetting
{
    public const string SectionName = "MinioSetting";

    public required string Endpoint { get; set; }
    public required string AccessKey { get; set; }
    public required string SecretKey { get; set; }
}


public sealed class BrokerSetting
{
    public const string SectionName = "BrokerSetting";

    public required string Host { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
}