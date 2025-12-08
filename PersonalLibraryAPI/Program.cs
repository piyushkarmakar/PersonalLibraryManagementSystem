using PersonalLibraryManagementSystem.Services;
using Microsoft.OpenApi.Models;
using PersonalLibraryAPI.Services;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using System.IO;
using PersonalLibraryAPI.Logging;


var builder = WebApplication.CreateBuilder(args);

// ----------------------------------------------------
// 1. Read Configuration
// ----------------------------------------------------
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

// ----------------------------------------------------
// 2. Configure Logging (Console + File + Database)
// ----------------------------------------------------
builder.Logging.ClearProviders();

// Console Logging
builder.Logging.AddConsole();

// File Logging → logs/app-log-2025-12-08.txt (daily rolling)
builder.Logging.AddFile("Logs/app-log-.txt", append: true);

// Database Logging → Custom Provider
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Logging.AddProvider(new DatabaseLoggerProvider(connectionString));


// ----------------------------------------------------
// 3. Add Services
// ----------------------------------------------------
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

// DatabaseService DI
builder.Services.AddSingleton(new DatabaseService(connectionString));

// Wrapper services
builder.Services.AddSingleton<LibraryManager>();
builder.Services.AddSingleton<FriendManager>();
builder.Services.AddSingleton<LendingManager>();


// ----------------------------------------------------
// 4. Swagger
// ----------------------------------------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PersonalLibraryAPI", Version = "v1" });
});

var app = builder.Build();

// ----------------------------------------------------
// 5. Middleware
// ----------------------------------------------------
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