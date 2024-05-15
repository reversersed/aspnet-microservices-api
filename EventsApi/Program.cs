using EventsApi.src.Data;
using EventsApi.src.Data.Entities;
using EventsApi.src.Data.Seeds;
using EventsApi.src.Queries.Categories;
using EventsApi.src.Queries.Comments;
using EventsApi.src.Queries.Events;
using EventsApi.src.RabbitService.Listeners;
using EventsApi.src.Repositories;
using EventsApi.src.Repositories.Interfaces;
using EventsApi.src.Services;
using EventsApi.src.Services.Entities;
using EventsApi.src.Services.Interfaces;
using EventsApi.src.Validators.Categories;
using EventsApi.src.Validators.Comments;
using EventsApi.src.Validators.Events;
using Extensions;
using Extensions.HttpExtension;
using Extensions.MediatorExtension;
using Extensions.MediatorExtension.Middlewares;
using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
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
//Mapster configs
TypeAdapterConfig<Event, EventDTO>
    .NewConfig()
    .Map(destination => destination.Creator, src => src.Creator.Username)
    .Map(destination => destination.EventImage, src => Path.GetFileName(src.EventImage));
TypeAdapterConfig<Event, EventPreviewDTO>
    .NewConfig()
    .Map(destination => destination.EventImage, src => Path.GetFileName(src.EventImage));
TypeAdapterConfig<EventDTO, Event>
    .NewConfig()
    .IgnoreMember((member, side) => member.Name == "Creator" && side == MemberSide.Source);

TypeAdapterConfig<Comment, CommentDTO>
    .NewConfig()
    .Map(destination => destination.Creator, src => src.Creator.Username);
TypeAdapterConfig<CommentDTO, Comment>
    .NewConfig()
    .IgnoreMember((member, side) => member.Name == "Creator" && side == MemberSide.Source);

TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
//DI
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<ICreatorRepository, CreatorRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<ICommentService, CommentService>();
//Validators
builder.Services.AddScoped<IValidator<GetCategoryByIdQuery>, GetCategoryByIdValidator>();
builder.Services.AddScoped<IValidator<AddCategoryQuery>, AddCategoryValidator>();
builder.Services.AddScoped<IValidator<UpdateCategoryQuery>, UpdateCategoryValidator>();
builder.Services.AddScoped<IValidator<DeleteCategoryQuery>, DeleteCategoryValidator>();
builder.Services.AddScoped<IValidator<GetCategoriesQuery>, GetCategoriesValidator>();

builder.Services.AddScoped<IValidator<GetEventsWithOffsetQuery>, GetEventsWithOffsetValidator>();
builder.Services.AddScoped<IValidator<GetEventByIdQuery>, GetEventByIdValidator>();
builder.Services.AddScoped<IValidator<AddEventQuery>, AddEventValidator>();
builder.Services.AddScoped<IValidator<DeleteEventQuery>, DeleteEventValidator>();
builder.Services.AddScoped<IValidator<UpdateEventQuery>, UpdateEventValidator>();
builder.Services.AddScoped<IValidator<GetEventsPreviewQuery>, GetEventsPreviewValidator>();

builder.Services.AddScoped<IValidator<GetCommentByIdQuery>, GetCommentByIdValidator>();
builder.Services.AddScoped<IValidator<GetCommentByEventQuery>, GetCommentByEventValidator>();
builder.Services.AddScoped<IValidator<AddCommentQuery>, AddCommentValidator>();
builder.Services.AddScoped<IValidator<DeleteCommentQuery>, DeleteCommentValidator>();
builder.Services.AddScoped<IValidator<UpdateCommentQuery>, UpdateCommentValidator>();
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

        await SeedDataContext.SeedCategories(context, app.Logger);
        await SeedDataContext.SeedEvents(context, app.Logger);
    }
    catch(Exception ex)
    {
        app.Logger.LogWarning("Can't connect to database. Migrations are not applied. Exception message: {exception}: {inner}", ex.Message, ex.InnerException?.Message);
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.Logger.LogInformation("[EventsApi] Starting API as {environment}", builder.Environment.EnvironmentName);

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.UseCors();

app.Run();