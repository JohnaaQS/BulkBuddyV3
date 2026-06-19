using BulkBuddy.Business.Exceptions;
using BulkBuddy.Business.Models;
using BulkBuddy.Business.Models.Domain;
using BulkBuddy.Business.Repositories;
using BulkBuddy.DataAccess.Data;
using Dapper;
using Npgsql;

namespace BulkBuddy.DataAccess.Repositories;

// Repository voor user-gerelateerde databasequeries.
// NpgsqlException wordt hier omgezet naar DatabaseException zodat Business Npgsql niet hoeft te kennen.
public class UserRepository : IUserRepository
{
    private readonly DbConnectionFactory _dbConnectionFactory;

    public UserRepository(DbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    // Helper: vangt NpgsqlException op en gooit DatabaseException.
    // Zo hoeft Business Npgsql niet te kennen — alle DB-fouten komen als DatabaseException binnen.
    private static async Task<T> ExecuteAsync<T>(Func<Task<T>> operation)
    {
        try { return await operation(); }
        catch (NpgsqlException ex) { throw new DatabaseException("Database onbereikbaar.", ex); }
    }

    private static async Task ExecuteAsync(Func<Task> operation)
    {
        try { await operation(); }
        catch (NpgsqlException ex) { throw new DatabaseException("Database onbereikbaar.", ex); }
    }

    private const string UserSelectSql = """
        SELECT
            id,
            username,
            email,
            password_hash AS PasswordHash,
            password_salt AS PasswordSalt,
            age,
            height_cm AS HeightCm,
            weight_kg AS WeightKg,
            target_weight_kg AS TargetWeightKg,
            goal AS Goal,
            goal_phase AS GoalPhase,
            sex,
            training_frequency_per_week AS TrainingFrequencyPerWeek,
            activity_multiplier AS ActivityMultiplier,
            onboarding_completed AS OnboardingCompleted
        FROM users
        """;

    public Task<User?> GetFirstUserAsync() => ExecuteAsync(async () =>
    {
        using var connection = _dbConnectionFactory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<User>(UserSelectSql + " ORDER BY id LIMIT 1;");
    });

    public Task<User?> GetByIdAsync(int id) => ExecuteAsync(async () =>
    {
        using var connection = _dbConnectionFactory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<User>(UserSelectSql + " WHERE id = @Id LIMIT 1;", new { Id = id });
    });

    public Task<User?> GetByUsernameAsync(string username) => ExecuteAsync(async () =>
    {
        using var connection = _dbConnectionFactory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<User>(UserSelectSql + " WHERE LOWER(username) = LOWER(@Username) LIMIT 1;", new { Username = username });
    });

    public Task<User?> GetByEmailAsync(string email) => ExecuteAsync(async () =>
    {
        using var connection = _dbConnectionFactory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<User>(UserSelectSql + " WHERE LOWER(email) = LOWER(@Email) LIMIT 1;", new { Email = email });
    });

    public Task<User?> GetByUsernameOrEmailAsync(string usernameOrEmail) => ExecuteAsync(async () =>
    {
        using var connection = _dbConnectionFactory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<User>(UserSelectSql + " WHERE LOWER(username) = LOWER(@Value) OR LOWER(email) = LOWER(@Value) LIMIT 1;", new { Value = usernameOrEmail });
    });

    public Task<int> CreateAsync(RegisterRequest model, string passwordHash, string passwordSalt) => ExecuteAsync(async () =>
    {
        using var connection = _dbConnectionFactory.CreateConnection();
        const string sql = """
            INSERT INTO users (username, email, password_hash, password_salt, age, height_cm, weight_kg,
                target_weight_kg, goal, goal_phase, sex, training_frequency_per_week, activity_multiplier, onboarding_completed)
            VALUES (@Username, @Email, @PasswordHash, @PasswordSalt, @Age, @HeightCm, @WeightKg,
                @TargetWeightKg, @Goal, @GoalPhase, @Sex, @TrainingFrequencyPerWeek, @ActivityMultiplier, true)
            RETURNING id;
            """;
        return await connection.ExecuteScalarAsync<int>(sql, new
        {
            model.Username, model.Email,
            PasswordHash = passwordHash, PasswordSalt = passwordSalt,
            model.Age, model.HeightCm, model.WeightKg, model.TargetWeightKg,
            model.Goal, model.GoalPhase, model.Sex, model.TrainingFrequencyPerWeek, model.ActivityMultiplier
        });
    });

    public Task UpdateProfileAsync(int userId, UpdateProfileRequest model) => ExecuteAsync(async () =>
    {
        using var connection = _dbConnectionFactory.CreateConnection();
        const string sql = """
            UPDATE users SET username = @Username, email = @Email, age = @Age, height_cm = @HeightCm,
                weight_kg = @WeightKg, target_weight_kg = @TargetWeightKg, goal = @Goal, goal_phase = @GoalPhase,
                sex = @Sex, training_frequency_per_week = @TrainingFrequencyPerWeek,
                activity_multiplier = @ActivityMultiplier, updated_at = NOW()
            WHERE id = @UserId;
            """;
        await connection.ExecuteAsync(sql, new
        {
            UserId = userId, model.Username, model.Email, model.Age, model.HeightCm,
            model.WeightKg, model.TargetWeightKg, model.Goal, model.GoalPhase,
            model.Sex, model.TrainingFrequencyPerWeek, model.ActivityMultiplier
        });
    });
}
