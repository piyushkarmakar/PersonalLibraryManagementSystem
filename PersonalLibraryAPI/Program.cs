using PersonalLibraryManagementSystem.Services;
using Microsoft.OpenApi.Models;
using PersonalLibraryAPI.Services;
var builder = WebApplication.CreateBuilder(args);



builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

// Add services to DI container
builder.Services.AddControllers();

// Make sure logging is registered
builder.Services.AddLogging();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();


// read connection string from appsettings
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddSingleton(new DatabaseService(connectionString)); // DatabaseService has ctor(string)
// Register wrapper services: singletons are fine since underlying console services manage their own state.
builder.Services.AddSingleton<LibraryManager>();
builder.Services.AddSingleton<FriendManager>();
builder.Services.AddSingleton<LendingManager>();



// FileService methods in your project might be static; if you prefer an injectable wrapper,
// create a small FileServiceWrapper class. For simplicity we will access FileService statically in controllers.
// If your FileService is instance-based, register a singleton here instead.

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PersonalLibraryAPI", Version = "v1" });
});

var app = builder.Build();

// middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PersonalLibraryAPI v1");
});

app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();
