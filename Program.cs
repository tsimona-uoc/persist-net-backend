using Microsoft.EntityFrameworkCore;
using persist_net_backend.Data;
using persist_net_backend.Repositories;
using persist_net_backend.Services;
using DotNetEnv;

public class Program {
    public static void Main(string[] args)
    {
        // Cargar variables de entorno desde .env
        Env.Load();

        // Create the WebApplication builder
        var builder = WebApplication.CreateBuilder(args);

        #region INYECCIÓN DE DEPENDENCIAS
        
        Program.RegisterRepositories(builder);
        Program.RegisterServices(builder);
        Program.RegisterControllers(builder);

        #endregion

        var app = builder.Build();

        Program.AutoMigrate(app);
        Program.Run(app);        
    }

    public static void RegisterRepositories(WebApplicationBuilder builder)
    {
        #region DB CONTEXT

        var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING") 
            ?? builder.Configuration.GetConnectionString("DefaultConnection");

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                connectionString,
                x => x.MigrationsAssembly("persist_net_backend")));

        #endregion

        builder.Services.AddScoped<IUserRepository, UserRepository>();
    }

    public static void RegisterServices(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IAuthService, AuthService>();
    }

    public static void RegisterControllers(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddOpenApi();
    }

    public static void AutoMigrate(WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.Migrate();
        }
    }

    public static void Run(WebApplication app)
    {
        app.MapControllers();
        if (app.Environment.IsDevelopment()) {
            app.MapOpenApi();
        }
        app.UseHttpsRedirection();
        app.Run();
    }
}