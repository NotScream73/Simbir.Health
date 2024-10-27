using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Simbir.Health.Timetable.Configurations;
using Simbir.Health.Timetable.Data;
using Simbir.Health.Timetable.Filters;
using Simbir.Health.Timetable.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<DataContextInitializer>();
// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new GlobalExceptionFilter());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.Configure<ExternalServiceBaseUrlConfig>(builder.Configuration.GetSection("ExternalServiceBaseUrl"));

builder.Services.AddAuthentication().AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
   options =>
   {
       options.RequireHttpsMetadata = false;
       options.MapInboundClaims = false;
       options.TokenValidationParameters =
          new TokenValidationParameters
          {
              ValidateAudience = false,
              ValidateLifetime = false,
              ValidateIssuer = false
          };
       options.TokenValidationParameters.SignatureValidator = (token, _) => new JsonWebToken(token);
   });
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TimeTableService API", Version = "v1" });

    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter JWT Bearer token **_only_**",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);

    var securityRequirement = new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    };

    c.AddSecurityRequirement(securityRequirement);

});
builder.Services.AddScoped<ITimeTableService, TimeTableService>();
builder.Services.AddHttpClient<ExternalClientService>();

var app = builder.Build();

await app.InitializeDatabaseAsync();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
