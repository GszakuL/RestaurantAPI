using RestaurantAPI;
using RestaurantDAL.Entities;
using AutoMapper;
using RestaurantAPI.Services;
using NLog.Web;
using NLog;
using RestaurantAPI.Middleware;
using Microsoft.AspNetCore.Identity;
using RestaurantAPI.Models;
using RestaurantAPI.Validators;
using FluentValidation;
using RestaurantAPI.Models.Validators;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using RestaurantAPI.Authorization;
using Microsoft.AspNetCore.Authorization;

var authenticationSettings = new AuthenticationSettings();
var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = "Bearer";
    option.DefaultScheme = "Bearer";
    option.DefaultChallengeScheme = "Bearer";
}).AddJwtBearer(cfg =>
{
    cfg.RequireHttpsMetadata = false;
    cfg.SaveToken = true;
    cfg.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = authenticationSettings.JwtIssuer,
        ValidAudience = authenticationSettings.JwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey)),
    };
});

builder.Services.AddAuthorization(option =>
{
    option.AddPolicy("HasNationality", builder => builder.RequireClaim("Nationality"));
    option.AddPolicy("Atleast20", builder => builder.AddRequirements(new MinimumAgeRequirement(20)));
});

builder.Services.AddScoped<IAuthorizationHandler, MinimumAgeRequirementHandler>();
builder.Services.AddSingleton(authenticationSettings);
builder.Services.AddControllers();
builder.Services.AddDbContext<RestaurantDbContext>();
builder.Services.AddScoped<RestaurantSeeder>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IRestaurantService, RestaurantService>();
builder.Services.AddScoped<IDishService, DishService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IPasswordHasher<RegisterUserDto>,PasswordHasher<RegisterUserDto>>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddScoped<RequestTimeMiddleware>();
builder.Services.AddScoped<IValidator<CreateRestaurantDto>, CreateRestaurantDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateRestaurantDto>, UpdateRestaurantDtoValidator>();
builder.Services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
builder.Services.AddSwaggerGen();

// NLog: Setup NLog for Dependency injection
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
builder.Host.UseNLog();

var app = builder.Build();

// Configure the HTTP request pipeline.
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<RestaurantSeeder>();
    seeder.Seed();
}

app.Configuration.GetSection("Authentication").Bind(authenticationSettings);


app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<RequestTimeMiddleware>();
app.UseAuthentication();

app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI(c => 
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Restaurant API");
});

app.UseAuthorization();

app.MapControllers();

app.Run();
