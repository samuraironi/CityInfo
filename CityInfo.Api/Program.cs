using CityInfo.Api;
using CityInfo.Api.DbContexts;
using CityInfo.Api.Services;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/cityinfo.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
//builder.Logging.ClearProviders(); - nothing being logged
//builder.Logging.AddConsole();

builder.Host.UseSerilog();

// Add services to the container.


builder.Services.AddControllers(options =>
{
    //to not accept the requests in xml or other types if we return back Json
    options.ReturnHttpNotAcceptable = true;
}).AddNewtonsoftJson()
.AddXmlDataContractSerializerFormatters();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<FileExtensionContentTypeProvider>();

#if DEBUG
builder.Services.AddTransient<IMailService, LocalMailService>();
#else
builder.Services.AddTransient<IMailService, CloudMailService>();
#endif

builder.Services.AddSingleton<CitiesDataStore>();

/*builder.Services.AddDbContext<CityInfoCintext>(
    dbContextOptions => dbContextOptions.UseSqlite("Data Source=CityInfo.db"));*/

builder.Services.AddDbContext<CityInfoCintext>(
    dbContextOptions => dbContextOptions.UseSqlite(
        builder.Configuration["ConnectionStrings:CityInfoDbConnectionString"]));

builder.Services.AddScoped<ISomeScopedService, SomeScopedService>();
builder.Services.AddTransient<ISomeService, SomeService>();
builder.Services.AddScoped<ICityCRUD, CityCRUD>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapControllers();

app.Run();

//app.Run(async (context) =>
//{
//    await context.Response.WriteAsync("Hello World");
//});
