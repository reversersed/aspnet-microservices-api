using Extensions;
using Extensions.HttpExtension;
using Extensions.MediatorExtension;
using Extensions.MediatorExtension.Middlewares;
using FluentValidation;
using IdentityApi.src.Data;
using IdentityApi.src.Data.Entities;
using IdentityApi.src.Data.Seeds;
using IdentityApi.src.Queries;
using IdentityApi.src.Queries.Internal;
using IdentityApi.src.RabbitMq;
using IdentityApi.src.RabbitMq.Interfaces;
using IdentityApi.src.Repositories;
using IdentityApi.src.Repositories.Interfaces;
using IdentityApi.src.Services;
using IdentityApi.src.Services.Interfaces;
using IdentityApi.src.Validators;
using IdentityApi.src.Validators.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
// Add services to the container.

builder.Services.AddDbContext<DataContext>(opt => opt.UseNpgsql(builder.Configuration["ConnectionStrings:DefaultConnection"]), ServiceLifetime.Scoped);
builder.Services.AddIdentityCore<User>(option =>
{
    option.Password.RequiredLength = 1;
    option.Password.RequireNonAlphanumeric = false;
    option.Password.RequiredUniqueChars = 0;
    option.Password.RequireDigit = false;
    option.Password.RequireLowercase = false;
    option.Password.RequireUppercase = false;
})
    .AddRoles<IdentityRole<long>>()
    .AddRoleManager<RoleManager<IdentityRole<long>>>()
    .AddEntityFrameworkStores<DataContext>()
    .AddSignInManager<SignInManager<User>>()
    .AddDefaultTokenProviders();
builder.Services.AddMemoryCache();
builder.Services.UseMediatR(typeof(Program));
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers().AddJsonOptions(options => {
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
    options.AddDefaultPolicy(b =>
        b.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

builder.Services.AddJwtAuthentication();
//DI
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRabbitService, RabbitService>();
//Validators
builder.Services.AddScoped<IValidator<LoginQuery>, LoginQueryValidation>();
builder.Services.AddScoped<IValidator<RefreshQuery>, RefreshQueryValidation>();
builder.Services.AddScoped<IValidator<ChangeUsernameQuery>, ChangeUsernameValidator>();
builder.Services.AddScoped<IValidator<RegistrationQuery>, RegistrationValidator>();
builder.Services.AddScoped<IValidator<RecoveryPasswordQuery>, RecoveryPasswordValidator>();
builder.Services.AddScoped<IValidator<ChangePasswordQuery>, ChangePasswordValidator>();
//Internal validators
builder.Services.AddScoped<IValidator<FindUserQuery>, FindUserQueryValidation>();
//
var app = builder.Build();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DataContext>();
    try
    {
        context.Database.Migrate();

        var manager = scope.ServiceProvider.GetRequiredService<SignInManager<User>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<long>>>();
        await AdminCreationSeed.SeedAsync(manager, roleManager, app.Logger);
    }
    catch 
    { 
        app.Logger.LogWarning("Can't connect to database. Migrations are not applied"); 
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.Logger.LogInformation("[IdentityApi] Starting API as {environment}", builder.Environment.EnvironmentName);

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.UseCors();

app.Run();