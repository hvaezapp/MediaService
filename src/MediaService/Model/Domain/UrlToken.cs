namespace MediaService.Model.Domain
{
    public class UrlToken
    {
        public Guid Id { get; set; }
        public required string BucketName { get; set; }
        public required string ObjectName { get; set; }
        public required string ContentType { get; set; }
        public DateTime ExpireOn { get; set; }
        public int AccessCount { get; set; }
    }
}
