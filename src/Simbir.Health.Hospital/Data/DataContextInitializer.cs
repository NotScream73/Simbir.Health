using Microsoft.EntityFrameworkCore;

namespace Simbir.Health.Hospital.Data;

public static class InitializerExtensions
{
    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initializer = scope.ServiceProvider.GetRequiredService<DataContextInitializer>();

        await initializer.InitializeAsync();
    }
}

public class DataContextInitializer
{
    private readonly DataContext _context;

    public DataContextInitializer(DataContext context)
    {
        _context = context;
    }

    public async Task InitializeAsync()
    {
        await _context.Database.MigrateAsync();
    }
}