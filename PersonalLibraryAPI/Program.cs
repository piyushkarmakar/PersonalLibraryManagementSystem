using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using PersonalLibraryAPI.Logging;
using PersonalLibraryAPI.Middleware;
using PersonalLibraryAPI.Services;
using PersonalLibraryManagementSystem.Services;
using System.Linq;

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

// File Logging
builder.Logging.AddFile("Logs/app-log-.txt", append: true);

// Database Logging
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Logging.AddProvider(new DatabaseLoggerProvider(connectionString));

// ----------------------------------------------------
// 3. Custom Validation (IMPORTANT)
// ----------------------------------------------------
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = string.Join(" | ",
                context.ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));

            // Save validation errors for middleware
            context.HttpContext.Items["InvalidModelState"] = errors;

            // Log directly from here
            var logger = context.HttpContext.RequestServices
                .GetRequiredService<ILoggerFactory>()
                .CreateLogger("ModelValidation");

            logger.LogWarning("MODEL VALIDATION FAILED (ApiBehaviorOptions) → {Errors}", errors);

            return new BadRequestObjectResult(new
            {
                Status = 400,
                Message = "Validation failed",
                Errors = errors
            });
        };
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters
            .Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

// ----------------------------------------------------
// 4. Register Services
// ----------------------------------------------------
builder.Services.AddSingleton(new DatabaseService(connectionString));
builder.Services.AddSingleton<LibraryManager>();
builder.Services.AddSingleton<FriendManager>();
builder.Services.AddSingleton<LendingManager>();

// ----------------------------------------------------
// 5. Swagger
// ----------------------------------------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => { 
    c.SwaggerDoc("v1", new OpenApiInfo 
    { Title = "PersonalLibraryAPI", Version = "v1" }); });

var app = builder.Build();

// ----------------------------------------------------
// 6. Middleware Pipeline
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

// Custom Validation Middleware
app.UseMiddleware<ValidationLoggingMiddleware>();

app.UseAuthorization();
app.MapControllers();

app.Run();
