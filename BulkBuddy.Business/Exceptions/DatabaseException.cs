namespace BulkBuddy.Business.Exceptions;

// Eigen exception voor database-fouten.
// Hierdoor hoeft de Web-laag geen Npgsql te kennen.
// Business vangt NpgsqlException op en gooit DatabaseException — Web vangt alleen dit.
public class DatabaseException : Exception
{
    public DatabaseException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
