using MassTransit;
using MediaService.Infrastructure.IntegrationEvents;
using MediaService.Model.Context;
using MediaService.Model.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Minio;
using Minio.DataModel.Args;

namespace MediaService.Endpoint
{
    public static class MediaServiceEndpoints
    {
        public static IEndpointRouteBuilder MapMediaServiceEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/{bucket_name}/{catalog_id}", Upload).DisableAntiforgery();
            app.MapGet("/{token:guid:required}", Download);

            return app;
        }


        public static async Task Upload(
                    [FromRoute(Name = "bucket_name")] string bucketName,
                    [FromRoute(Name = "catalog_id")] string catalogId,
                    IFormFile file,
                    MediaServiceDbContext dbContext,
                    IPublishEndpoint publisher,
                    IMinioClient minioClient,
                    CancellationToken cancellationToken)
        {

            var putObjectArgs = new PutObjectArgs()
                               .WithBucket(bucketName)
                               .WithObject(file.FileName)
                               .WithContentType(file.ContentType)
                               .WithStreamData(file.OpenReadStream())
                               .WithObjectSize(file.Length);

            try
            {

                await minioClient.PutObjectAsync(putObjectArgs, cancellationToken);

                var token = new UrlToken
                {
                    BucketName = bucketName,
                    ObjectName = file.FileName,
                    ContentType = file.ContentType,
                    ExpireOn = DateTime.UtcNow.AddMinutes(10),
                    Id = Guid.NewGuid()
                };

                dbContext.Tokens.Add(token);
                await dbContext.SaveChangesAsync(cancellationToken);

                var url = $"http://localhost:5085/{token.Id}";
                await publisher.Publish(new MediaUploadedEvent(file.FileName, url, catalogId, DateTime.UtcNow),cancellationToken);
            }
            catch (Exception)
            {

            }

        }

        public static async Task Download(
                      Guid token,
                      MediaServiceDbContext dbContext,
                      IMinioClient minioClient,
                      HttpContext httpContext,
                      CancellationToken cancellationToken)
        {

            var foundToken = await dbContext.Tokens.FirstOrDefaultAsync(x => x.Id == token && x.ExpireOn >= DateTime.UtcNow, cancellationToken);

            if (foundToken is null)
                throw new InvalidOperationException();

            foundToken.AccessCount++;
            await dbContext.SaveChangesAsync(cancellationToken);

            GetObjectArgs getObjectArgs = new GetObjectArgs()
                                       .WithBucket(foundToken.BucketName)
                                       .WithObject(foundToken.ObjectName)
                                       .WithCallbackStream(async (stream, cancellationToken) =>
                                       {
                                           httpContext.Response.ContentType = foundToken.ContentType;
                                           await stream.CopyToAsync(httpContext.Response.Body, cancellationToken);
                                       });

            await minioClient.GetObjectAsync(getObjectArgs, cancellationToken);
        }

    }
}
