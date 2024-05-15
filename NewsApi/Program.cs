using Microsoft.EntityFrameworkCore;
using Extensions.MediatorExtension;
using Extensions.MediatorExtension.Middlewares;
using NewsApi.src.Data;
using NewsApi.src.Data.Seeds;
using NewsApi.src.Services.Interfaces;
using NewsApi.src.Services;
using NewsApi.src.Repositories.Interfaces;
using NewsApi.src.Repositories;
using FluentValidation;
using Extensions.HttpExtension;
using Extensions;
using NewsApi.src.RabbitService.Listeners;
using NewsApi.src.Queries.Articles;
using NewsApi.src.Queries.Comments;
using NewsApi.src.Validators.Articles;
using NewsApi.src.Validators.Comments;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
// Add services to the container.

builder.Services.AddDbContext<DataContext>(opt => opt.UseNpgsql(builder.Configuration["ConnectionStrings:DefaultConnection"]), ServiceLifetime.Scoped);

builder.Services.AddJwtAuthentication();
builder.Services.UseHttpClient();
builder.Services.UseMediatR(typeof(Program));
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
//DI
builder.Services.AddScoped<IArticleService, ArticleService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
//Validators
builder.Services.AddScoped<IValidator<FindArticleByIdQuery>, FindArticleByIdValidator>();
builder.Services.AddScoped<IValidator<CreateArticleQuery>, CreateArticleValidator>();
builder.Services.AddScoped<IValidator<GetArticlesWithOffsetQuery>, GetArticlesWithOffsetValidator>();
builder.Services.AddScoped<IValidator<DeleteArticleQuery>, DeleteArticleValidator>();
builder.Services.AddScoped<IValidator<PatchArticleQuery>, PatchArticleValidator>();
builder.Services.AddScoped<IValidator<CreateCommentQuery>, CreateCommentValidator>();
builder.Services.AddScoped<IValidator<PatchCommentQuery>, PatchCommentValidator>();
builder.Services.AddScoped<IValidator<FindCommentByIdQuery>, FindCommentByIdValidator>();
builder.Services.AddScoped<IValidator<GetArticleCommentQuery>, GetArticleCommentValidator>();
//Internal validators

//Rabbit listeners
builder.Services.AddHostedService<RabbitUsernameChangeListener>();
//
var app = builder.Build();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DataContext>();
    try
    {
        context.Database.Migrate();

        await CreateArticleSeed.SeedAsync(context, app.Logger);
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

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.UseCors();

app.Run();