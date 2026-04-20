using BulkBuddy.Business.Models.Domain;
using BulkBuddy.Business.Models.ViewModels;
using BulkBuddy.Business.Repositories;
using BulkBuddy.DataAccess.Data;
using Dapper;

namespace BulkBuddy.DataAccess.Repositories;

// Repository voor user-gerelateerde databasequeries.
public class UserRepository : IUserRepository
{
    private readonly DbConnectionFactory _dbConnectionFactory;

    public UserRepository(DbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<User?> GetFirstUserAsync()
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        const string sql = """
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
            ORDER BY id
            LIMIT 1;
            """;

        return await connection.QueryFirstOrDefaultAsync<User>(sql);
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        const string sql = """
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
            WHERE id = @Id
            LIMIT 1;
            """;

        return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        const string sql = """
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
            WHERE LOWER(username) = LOWER(@Username)
            LIMIT 1;
            """;

        return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Username = username });
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        const string sql = """
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
            WHERE LOWER(email) = LOWER(@Email)
            LIMIT 1;
            """;

        return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });
    }

    public async Task<User?> GetByUsernameOrEmailAsync(string usernameOrEmail)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        const string sql = """
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
            WHERE LOWER(username) = LOWER(@Value)
               OR LOWER(email) = LOWER(@Value)
            LIMIT 1;
            """;

        return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Value = usernameOrEmail });
    }

    public async Task<int> CreateAsync(RegisterViewModel model, string passwordHash, string passwordSalt)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        const string sql = """
            INSERT INTO users
            (
                username,
                email,
                password_hash,
                password_salt,
                age,
                height_cm,
                weight_kg,
                target_weight_kg,
                goal,
                goal_phase,
                sex,
                training_frequency_per_week,
                activity_multiplier,
                onboarding_completed
            )
            VALUES
            (
                @Username,
                @Email,
                @PasswordHash,
                @PasswordSalt,
                @Age,
                @HeightCm,
                @WeightKg,
                @TargetWeightKg,
                @Goal,
                @GoalPhase,
                @Sex,
                @TrainingFrequencyPerWeek,
                @ActivityMultiplier,
                true
            )
            RETURNING id;
            """;

        return await connection.ExecuteScalarAsync<int>(sql, new
        {
            model.Username,
            model.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            model.Age,
            model.HeightCm,
            model.WeightKg,
            model.TargetWeightKg,
            model.Goal,
            model.GoalPhase,
            model.Sex,
            model.TrainingFrequencyPerWeek,
            model.ActivityMultiplier
        });
    }

    public async Task UpdateProfileAsync(int userId, EditProfileViewModel model)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        const string sql = """
            UPDATE users
            SET
                username = @Username,
                email = @Email,
                age = @Age,
                height_cm = @HeightCm,
                weight_kg = @WeightKg,
                target_weight_kg = @TargetWeightKg,
                goal = @Goal,
                goal_phase = @GoalPhase,
                sex = @Sex,
                training_frequency_per_week = @TrainingFrequencyPerWeek,
                activity_multiplier = @ActivityMultiplier,
                updated_at = NOW()
            WHERE id = @UserId;
            """;

        await connection.ExecuteAsync(sql, new
        {
            UserId = userId,
            model.Username,
            model.Email,
            model.Age,
            model.HeightCm,
            model.WeightKg,
            model.TargetWeightKg,
            model.Goal,
            model.GoalPhase,
            model.Sex,
            model.TrainingFrequencyPerWeek,
            model.ActivityMultiplier
        });
    }
}
