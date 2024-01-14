//using Serilog;

using MagicVilla_VillaAPI;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Repository;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// NUGET PACKAGES

//Microsoft.AspNetCore.JsonPatch
//Microsoft.AspNetCore.Mvc.NewtonsoftJson
//Microsoft.EntityFrameworkCore.SqlServer
//Microsoft.EntityFrameworkCore.Tools
//AutoMapper
//AutoMapper.Extensions.Microsoft.DependencyInjection

//Serilog.AspNetCore
//Serilog.Sinks.File
//Serilog.Sinks.Console


// Add services to the container.

builder.Services.AddAutoMapper(typeof(MappingConfig));

// add services for Interface IVilla Repository, and add VillaRepository implementation
builder.Services.AddScoped<IVillaRepository, VillaRepository>();
builder.Services.AddScoped<IVillaNumberRepository, VillaNumberRepository>();
//// register Serilog and create a log file with a rolling interval of daily and use CreateLogger function to edit it.
//// right now we will use the default logger but below is the code to use serilog

//Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
//    .WriteTo.File("log/villaLogs.txt", rollingInterval: RollingInterval.Day).CreateLogger();

//// tells the application that it does not have to use the built in logging but will now use serilog logging.
//builder.Host.UseSerilog();



builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
});

// add .AddNewtonsoftJson(); to builder.Services.AddControllers() to register the nuget package added
// add to .AddControllers(option => { option.ReturnHttpNotAcceptable = true; }) this to make sure only Json will be accepted
// to accept xml also .AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();
builder.Services.AddControllers(option =>
{
    //option.ReturnHttpNotAcceptable = true; 
}).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
