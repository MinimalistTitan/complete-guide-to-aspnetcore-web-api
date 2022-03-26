using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using my_books.Data;
using my_books.Data.Services;
using my_books.EntityModels;
using my_books.Exceptions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));
try
{
    Log.Logger = new LoggerConfiguration()
        .CreateLogger();
}
finally
{
    Log.CloseAndFlush();
   
}
string connString = builder.Configuration.GetConnectionString("DefaultConnectionString");

// Add services to the container.
builder.Services.AddDbContext<MyBooksDbContext>(options => options.UseSqlServer(connString));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddTransient<BooksService>();
builder.Services.AddTransient<AuthorsService>();
builder.Services.AddTransient<PublishersService>();
builder.Services.AddTransient<LogsService>();
var loggerFactory = new LoggerFactory();
//builder.Services.AddApiVersioning(config =>
//{
//    //config.DefaultApiVersion = new ApiVersion(1,0);
//    //config.AssumeDefaultVersionWhenUnspecified = true;
//    //config.ApiVersionReader = new HeaderApiVersionReader("custom-version-header");
//    //config.ApiVersionReader = new MediaTypeApiVersionReader();
//});

var app = builder.Build();

//AppDbInitializer.Seed(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Exception Handling
app.ConfigureBuildInExceptionHandler(loggerFactory);
//app.ConfigureCustomExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


