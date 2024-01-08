//using Serilog;

using MagicVilla_VillaAPI.Logging;

var builder = WebApplication.CreateBuilder(args);

// NUGET PACKAGES

//Microsoft.AspNetCore.JsonPatch
//Microsoft.AspNetCore.Mvc.NewtonsoftJson
//Serilog.AspNetCore
//Serilog.Sinks.File
//Serilog.Sinks.Console


// Add services to the container.

//// register Serilog and create a log file with a rolling interval of daily and use CreateLogger function to edit it.
//// right now we will use the default logger but below is the code to use serilog

//Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
//    .WriteTo.File("log/villaLogs.txt", rollingInterval: RollingInterval.Day).CreateLogger();

//// tells the application that it does not have to use the built in logging but will now use serilog logging.
//builder.Host.UseSerilog();

// add .AddNewtonsoftJson(); to builder.Services.AddControllers() to register the nuget package added
// add to .AddControllers(option => { option.ReturnHttpNotAcceptable = true; }) this to make sure only Json will be accepted
// to accept xml also .AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();
builder.Services.AddControllers(option => { 
    //option.ReturnHttpNotAcceptable = true; 
    }).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// For logger use singleton
builder.Services.AddSingleton<ILogging, LoggingV2>();

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
