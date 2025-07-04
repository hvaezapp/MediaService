using MediaService.Model.Domain;
using Microsoft.EntityFrameworkCore;

namespace MediaService.Model.Context
{
    public class MediaServiceDbContext(DbContextOptions<MediaServiceDbContext> options) : DbContext(options)
    {
        public DbSet<UrlToken> Tokens { get; set; }
    }
}
