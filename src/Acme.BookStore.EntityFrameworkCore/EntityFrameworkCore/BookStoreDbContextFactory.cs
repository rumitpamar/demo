using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
#if !DEBUG
using Acme.BookStore.SharedServices;
#endif

namespace Acme.BookStore.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class BookStoreDbContextFactory : IDesignTimeDbContextFactory<BookStoreDbContext>
{

    public BookStoreDbContext CreateDbContext(string[] args)
    {
        // https://www.npgsql.org/efcore/release-notes/6.0.html#opting-out-of-the-new-timestamp-mapping-logic
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        BookStoreEfCoreEntityExtensionMappings.Configure();

        var configuration = BuildConfiguration();
#if DEBUG
        var connectionString = configuration.GetConnectionString("Default");        
#else
        var connectionString = new AwsSecretsManagerService().GetSecretAsync(configuration).Result;
#endif

        var builder = new DbContextOptionsBuilder<BookStoreDbContext>()
            .UseNpgsql(connectionString);

        return new BookStoreDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Acme.BookStore.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }    
}



