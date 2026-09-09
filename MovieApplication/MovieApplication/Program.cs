using System.Reflection;
using System.Threading.RateLimiting;
using Application;
using Application.Services;
using CoreApplication.Pipelines.Caching;
using CoreCrossingCuttingConcerns.Exceptions;
using CoreSecurity;
using CoreSecurity.Encryption;
using CoreSecurity.Hashing;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MovieApplication.Hubs;
using MovieApplication.Middlewares;
using Persistence;

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(policy =>
        {
            policy.SetIsOriginAllowed(origin => true)
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials(); // Often needed for frontend apps
        });
    });

    builder.Services.AddControllers();
    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    builder.Services.AddOpenApi();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    //builder.Services.AddStackExchangeRedisCache(opt => opt.Configuration = "localhost:6379");
    builder.Services.AddDistributedMemoryCache();
    builder.Services.AddSingleton(builder.Configuration.GetSection("CacheSettings").Get<CacheSettings>()!);

    builder.Services.AddPersistenceServices(builder.Configuration);
    builder.Services.AddApplicationServiceRegistration();
    builder.Services.AddInfrastructureServiceRegistration();
    builder.Services.AddCoreSecurityService();
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddSignalR();

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
      .AddJwtBearer(options =>
      {
          options.TokenValidationParameters = new TokenValidationParameters
          {
              ValidateIssuer = true,
              ValidateAudience = true,
              ValidateLifetime = true,
              ValidateIssuerSigningKey = true,
              ValidIssuer = builder.Configuration["TokenOptions:Issuer"],
              ValidAudience = builder.Configuration["TokenOptions:Audience"],
              IssuerSigningKey = SecurityKeyHelper.SecurityKey(
                  builder.Configuration["TokenOptions:SecurityKey"]!
              )
          };
      });

    builder.Services.AddSwaggerGen(opt =>
    {
        opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header
        });
        opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
    });

    builder.Services.AddScoped<INotificationHubService, NotificationHubService>();

    builder.Services.AddRateLimiter(opt =>
    {
        opt.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

        opt.AddFixedWindowLimiter("GeneralPolicy", opt =>
        {
            opt.Window = TimeSpan.FromMinutes(1);
            opt.QueueLimit = 0;
            opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            opt.PermitLimit = 1000;
        });

        opt.AddFixedWindowLimiter("AuthPolicy", opt =>
        {
            opt.Window = TimeSpan.FromMinutes(1);
            opt.QueueLimit = 0;
            opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            opt.PermitLimit = 50;
        });
    });

    FirebaseApp.Create(new AppOptions()
    {
        Credential = GoogleCredential.FromFile(Path.Combine(builder.Environment.ContentRootPath, "Firebase", "firebase-adminsdk.json")),
    });

    var app = builder.Build();


    if (app.Environment.IsDevelopment())
    {
        // Swagger JSON dosyasını oluşturur
        app.UseSwagger();

        // Swagger UI arayüzünü (renkli ekranı) aktif eder
        app.UseSwaggerUI(opt =>
        {
            opt.SwaggerEndpoint("/swagger/v1/swagger.json", "MovieApp API v1");
        });
    }
    if (!app.Environment.IsDevelopment())
    {
        app.UseMiddleware<HmacValidationMiddleware>();
    }
    app.UseHttpsRedirection();
    app.UseCors();
    app.UseRateLimiter();
    app.UseMiddleware<ExceptionMiddleware>();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
    app.MapHub<SignalRHub>("/signalrHub");

    app.Run();

}
catch (ReflectionTypeLoadException ex)
{
    foreach (var loaderException in ex.LoaderExceptions)
    {
        Console.WriteLine(loaderException?.Message);
    }
}
