namespace MediaService.Infrastructure.IntegrationEvents;

public record MediaUploadedEvent(string FileName, string Url, string CatalogId, DateTime OccurredOn);
