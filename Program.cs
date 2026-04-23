using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using persist_net_backend.Data;
using persist_net_backend.Repositories;
using persist_net_backend.Services;
using DotNetEnv;

public class Program {
    private const string FrontendCorsPolicy = "FrontendCorsPolicy";

    public static void Main(string[] args)
    {
        Env.Load();

        var builder = WebApplication.CreateBuilder(args);

        Program.RegisterRepositories(builder);
        Program.RegisterServices(builder);
        Program.RegisterAuthentication(builder);
        Program.RegisterCors(builder);
        Program.RegisterControllers(builder);

        var app = builder.Build();

        Program.AutoMigrate(app);
        Program.Run(app);        
    }

    public static void RegisterRepositories(WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        builder.Services.AddDbContext<AppDbContext>(options => {
            options.UseSqlServer(connectionString, x => x.MigrationsAssembly("persist_net_backend"));
            options.UseLazyLoadingProxies();
        });

        builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();
        builder.Services.AddScoped<IUserSessionRepository, UserSessionRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();

        builder.Services.AddScoped<IHotelRepository, HotelRepository>();
        builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
        builder.Services.AddScoped<ITipoHabitacionRepository, TipoHabitacionRepository>();
        builder.Services.AddScoped<IHabitacionRepository, HabitacionRepository>();

        builder.Services.AddScoped<ITemporadaRepository, TemporadaRepository>();
        builder.Services.AddScoped<ITarifaRepository, TarifaRepository>();
        builder.Services.AddScoped<IReservaRepository, ReservaRepository>();
        builder.Services.AddScoped<IEstanciaRepository, EstanciaRepository>();

        builder.Services.AddScoped<IServicioExtraRepository, ServicioExtraRepository>();
        builder.Services.AddScoped<IConsumoExtraRepository, ConsumoExtraRepository>();

        builder.Services.AddScoped<IFacturaRepository, FacturaRepository>();
        builder.Services.AddScoped<IPagoRepository, PagoRepository>();
        builder.Services.AddScoped<IMetodoPagoRepository, MetodoPagoRepository>();
        builder.Services.AddScoped<IRegimenRepository, RegimenRepository>();

        builder.Services.AddScoped<IEstadoHabitacionRepository, EstadoHabitacionRepository>();
        builder.Services.AddScoped<IEstadoReservaRepository, EstadoReservaRepository>();
        builder.Services.AddScoped<IEstadoEstanciaRepository, EstadoEstanciaRepository>();

        builder.Services.AddScoped<IExportRepository, ExportRepository>();
    }

    public static void RegisterServices(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IEntityReferenceValidator, EntityReferenceValidator>();

        builder.Services.AddScoped<IHotelService, HotelService>();
        builder.Services.AddScoped<IClienteService, ClienteService>();
        builder.Services.AddScoped<ITipoHabitacionService, TipoHabitacionService>();
        builder.Services.AddScoped<IHabitacionService, HabitacionService>();

        builder.Services.AddScoped<ITemporadaService, TemporadaService>();
        builder.Services.AddScoped<ITarifaService, TarifaService>();
        builder.Services.AddScoped<IReservaService, ReservaService>();
        builder.Services.AddScoped<IEstanciaService, EstanciaService>();

        builder.Services.AddScoped<IServicioExtraService, ServicioExtraService>();
        builder.Services.AddScoped<IConsumoExtraService, ConsumoExtraService>();

        builder.Services.AddScoped<IEstadoHabitacionService, EstadoHabitacionService>();
        builder.Services.AddScoped<IEstadoReservaService, EstadoReservaService>();
        builder.Services.AddScoped<IEstadoEstanciaService, EstadoEstanciaService>();

        builder.Services.AddScoped<IFacturaService, FacturaService>();
        builder.Services.AddScoped<IPagoService, PagoService>();
        builder.Services.AddScoped<IMetodoPagoService, MetodoPagoService>();
        builder.Services.AddScoped<IRegimenService, RegimenService>();

        builder.Services.AddScoped<IExportService, ExportService>();
        builder.Services.AddScoped<IImportService, ImportService>();
    }

    public static void RegisterAuthentication(WebApplicationBuilder builder)
    {
        var jwtSecret = builder.Configuration["Jwt:SecretKey"];
        var jwtIssuer = builder.Configuration["Jwt:Issuer"];
        var jwtAudience = builder.Configuration["Jwt:Audience"];

        if (string.IsNullOrEmpty(jwtSecret))
        {
            throw new InvalidOperationException("JWT Secret is not configured");
        }

        var key = Encoding.ASCII.GetBytes(jwtSecret);

        builder.Services
            .AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = !string.IsNullOrEmpty(jwtIssuer),
                    ValidIssuer = jwtIssuer,
                    ValidateAudience = !string.IsNullOrEmpty(jwtAudience),
                    ValidAudience = jwtAudience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

        builder.Services.AddAuthorization();
    }

    public static void RegisterCors(WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(FrontendCorsPolicy, policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });
    }

    public static void RegisterControllers(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
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
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors(FrontendCorsPolicy);
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.MapControllers();

        app.Run();
    }
}