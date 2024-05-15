using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Extensions;
using Extensions.HttpExtension;
using Extensions.MediatorExtension;
using Extensions.MediatorExtension.Middlewares;
using CoworkingApi.src.Data;
using CoworkingApi.src.Repositories.Interfaces;
using CoworkingApi.src.Repositories;
using FluentValidation;
using CoworkingApi.src.Queries.Rooms;
using CoworkingApi.src.Validators.Rooms;
using CoworkingApi.src.Services.Interfaces;
using CoworkingApi.src.Services;
using CoworkingApi.src.Data.Seeds;
using Mapster;
using CoworkingApi.src.Services.Entities;
using CoworkingApi.src.Data.Entities;
using System.Reflection;
using CoworkingApi.src.Queries.Reservations;
using CoworkingApi.src.Validators.Reservations;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
// Add services to the container.
builder.Services.AddDbContext<DataContext>(opt => {
    opt.UseNpgsql(builder.Configuration["ConnectionStrings:DefaultConnection"]);
    opt.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
}, ServiceLifetime.Scoped);

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
TypeAdapterConfig<Room, RoomDTO>
    .NewConfig()
    .Map(destination => destination.RoomImage, src => Path.GetFileName(src.RoomImage));

TypeAdapterConfig<Reservation, ReservationDTO>
    .NewConfig()
    .Map(destination => destination.Holder, src => src.Holder.Username);

TypeAdapterConfig<ReservationDTO, Reservation>
    .NewConfig()
    .IgnoreMember((member, side) => member.Name == "Holder" && side == MemberSide.Source);

TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
//DI
builder.Services.AddScoped<IHolderRepository, HolderRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();

builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
//Validators
builder.Services.AddScoped<IValidator<GetRoomByIdQuery>, GetRoomByIdValidator>();
builder.Services.AddScoped<IValidator<AddRoomQuery>, AddRoomValidator>();
builder.Services.AddScoped<IValidator<DeleteRoomQuery>, DeleteRoomValidator>();
builder.Services.AddScoped<IValidator<UpdateRoomQuery>, UpdateRoomValidator>();

builder.Services.AddScoped<IValidator<GetReservationsByRoomQuery>, GetReservationsByRoomValidator>();
builder.Services.AddScoped<IValidator<GetReservationHoursQuery>, GetReservationHoursValidator>();
builder.Services.AddScoped<IValidator<CreateReservationQuery>, CreateReservationValidator>();
//Internal validators

//Rabbit listeners

//
var app = builder.Build();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DataContext>();
    try
    {
        context.Database.Migrate();

        await DataSeed.SeedRooms(context, app.Logger);
        //await SeedDataContext.SeedEvents(context, app.Logger);
    }
    catch (Exception ex)
    {
        app.Logger.LogWarning("Can't connect to database. Migrations are not applied. Exception message: {exception}: {inner}", ex.Message, ex.InnerException?.Message);
    }
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.Logger.LogInformation("[CoworkingApi] Starting API as {environment}", builder.Environment.EnvironmentName);

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.UseCors();

app.Run();