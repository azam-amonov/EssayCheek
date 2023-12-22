using EssayCheek.Api.Brokers.DateTimes;
using EssayCheek.Api.Brokers.Logging;
using EssayCheek.Api.Brokers.OpenAis;
using EssayCheek.Api.Brokers.StorageBroker;
using EssayCheek.Api.Services.EssayAnalysis;
using EssayCheek.Api.Services.EssayResults;
using EssayCheek.Api.Services.Essays;
using EssayCheek.Api.Services.TextFormatterService;
using EssayCheek.Api.Services.Users;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.InputFormatters.Add(new TextInputFormatter());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<StorageBroker>();

// Life cycles
builder.Services.AddScoped<IStorageBroker, StorageBroker>();
builder.Services.AddScoped<ILoggingBroker, LoggingBroker>();
builder.Services.AddScoped<IDateTimeBroker, DateTimeBroker>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEssayService, EssayService>();
builder.Services.AddScoped<IEssayResultService, EssayResultService>();
builder.Services.AddScoped<IOpenAiBroker, OpenAiBroker>();
builder.Services.AddScoped<IEssayAnalysisService, EssayAnalysisService>();

var app = builder.Build();

app.Use(async (context, next) =>
{
    Console.WriteLine(context.Request.ContentType);
    await next(context);
});

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();