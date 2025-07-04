using MediaService.Model.Context;
using MediaService.Shared;
using Microsoft.EntityFrameworkCore;
using Minio;
using Minio.AspNetCore;

namespace MediaService.Bootstraper
{
    public static class ServiceRegistration
    {

        public static void RegisterCommonCases(this WebApplicationBuilder builder)
        {
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
        }


        public static void RegisterInMemoryDatabase(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<MediaServiceDbContext>(configure =>
            {
                configure.UseInMemoryDatabase("MediaServiceDb");
            });
        }


        public static void RegisterMinio(this WebApplicationBuilder builder)
        {

            var minioSetting = builder.Configuration.GetSection(MinioSetting.SectionName).Get<MinioSetting>();

            if (minioSetting is null)
                throw new Exception("Unable to get Minio Setting");

            builder.Services.AddMinio(configureClient => configureClient
                            .WithEndpoint(minioSetting.Endpoint)
                            .WithCredentials(minioSetting.AccessKey, minioSetting.SecretKey)
                            .WithSSL(false)
                            .Build());

        }
    }
}
