using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Services;
using Serilog;
using EF;

namespace Src;

public class Program
{
    public static void Main(string[] args)
    {
        #region Variables

        var builder = WebApplication.CreateBuilder(args);
        var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
        var secretProvider = config.Providers.First();
        secretProvider.TryGet("jwt-secret-key", out string? jwtSecretKey);
        ArgumentNullException.ThrowIfNull(jwtSecretKey);
        var postgreSQLConnection = builder.Configuration.GetConnectionString("PostgreSQL");

        #endregion

        #region Services

        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(
                policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                });
        });

        builder.Services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            options.JsonSerializerOptions.WriteIndented = true;
            options.JsonSerializerOptions.MaxDepth = 256;
        });

        builder.Services.AddDbContext<ApplicationContext>(options =>
            options.UseNpgsql(postgreSQLConnection));
        builder.Services.AddProblemDetails();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddScoped<ProfileService>();
        builder.Services.AddScoped<AuthService>();
        builder.Services.AddScoped<FriendsService>();
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(JwtHelper.JWTSecretKey)),
                ValidateAudience = false,
                ValidateIssuer = false,
                /// Allows to make token expire in less than 5 minutes 
                ClockSkew = TimeSpan.Zero,
            };
        });
        builder.Services.AddAuthorization();

        #endregion

        #region Logging

        var serilogLoggerConfig = new LoggerConfiguration()
            .WriteTo.Console(Serilog.Events.LogEventLevel.Information);
        // .WriteTo.Debug(Serilog.Events.LogEventLevel.Debug);

        var logFilePath = builder.Configuration.GetValue<string>("Logging:Serilog:LogFilePath");

        if (!Enum.TryParse(builder.Configuration.GetValue<string>("Logging:Serilog:FileRollingInterval"), out RollingInterval rollingInterval))
            rollingInterval = RollingInterval.Infinite;

        if (!string.IsNullOrEmpty(logFilePath))
            serilogLoggerConfig.WriteTo.File(path: logFilePath,
            restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning,
            rollingInterval: rollingInterval,
            outputTemplate: "{Timestamp:dd-MM-yyyy HH:mm:ss} [{Level}] {Message:lj}{NewLine}{Exception}{NewLine}");

        Log.Logger = serilogLoggerConfig.CreateLogger();
        builder.Host.UseSerilog();

        #endregion

        # region Run

        try
        {
            var app = builder.Build();

            // app.UseExceptionHandler();
            // app.UseStatusCodePages();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                // app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            Log.Information("Application Started");
            app.Run();
        }
        finally
        {
            Log.CloseAndFlush();
        }
        return;

        #endregion
    }
}
