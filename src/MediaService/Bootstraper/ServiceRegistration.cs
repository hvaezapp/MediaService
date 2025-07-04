using MassTransit;
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
                throw new ArgumentNullException(nameof(minioSetting));

            builder.Services.AddMinio(configureClient => configureClient
                            .WithEndpoint(minioSetting.Endpoint)
                            .WithCredentials(minioSetting.AccessKey, minioSetting.SecretKey)
                            .WithSSL(false)
                            .Build());

        }


        public static void RegisterBroker(this WebApplicationBuilder builder)
        {
            builder.Services.AddMassTransit(configure =>
            {
                var brokerSetting = builder.Configuration.GetSection(BrokerSetting.SectionName)
                                                        .Get<BrokerSetting>();
                if (brokerSetting is null)
                    throw new ArgumentNullException(nameof(brokerSetting));

                configure.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(brokerSetting.Host, hostConfigure =>
                    {
                        hostConfigure.Username(brokerSetting.Username);
                        hostConfigure.Password(brokerSetting.Password);
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });
        }
    }
}
