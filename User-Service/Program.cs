using FirebaseAdmin;
using Microsoft.EntityFrameworkCore;
using User_Service.Models;
using User_Service.Services;

var builder = WebApplication.CreateBuilder(args);
//cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecific",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000", "https://www.note-manager.nl", "https://api.note-manager.nl")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

//env
var dbConfig = new ConfigurationBuilder()
    .AddJsonFile(builder.Configuration["DATABASE_CONNECTION"]!, optional: false, reloadOnChange: true)
    .Build();
string db_connection = dbConfig.GetConnectionString("Default")!;

var googleApisConfig = new ConfigurationBuilder()
    .AddJsonFile(builder.Configuration["GOOGLE_APIS_JWT"]!, optional: false, reloadOnChange: true)
    .Build();
string googleApisJwtPath = googleApisConfig["GOOGLE_APIS_JWT"]!;


// connect to db
builder.Services.AddDbContext<UserContext>(options =>
    options.UseMySql(db_connection, ServerVersion.AutoDetect(db_connection)));
//

// add firebase
FirebaseApp.Create();
//

// add interfaces
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

builder.Services.AddHttpClient<IJwtProvider, JwtProvider>((sp, httpClient) =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    httpClient.BaseAddress = new Uri(googleApisJwtPath);
});
//


// add controlleres
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//

var app = builder.Build();
app.UseCors("AllowLocalhost3000");

// auto-apply migrations
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<UserContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}
//

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
