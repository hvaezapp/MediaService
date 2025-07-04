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
