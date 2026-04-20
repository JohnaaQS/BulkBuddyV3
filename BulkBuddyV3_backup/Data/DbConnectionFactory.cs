using Npgsql;

namespace BulkBuddy.Data;

// Centraliseert het maken van databaseverbindingen.
// Zo staat de connectielogica niet verspreid door het project.
public class DbConnectionFactory
{
    private readonly IConfiguration _configuration;

    public DbConnectionFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public NpgsqlConnection CreateConnection()
    {
        // Haal de connection string op uit appsettings.json.
        var connectionString = _configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Connection string 'DefaultConnection' ontbreekt.");
        }

        return new NpgsqlConnection(connectionString);
    }
}
