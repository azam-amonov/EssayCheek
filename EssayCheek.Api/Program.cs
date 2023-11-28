using EssayCheek.Api.Brokers.StorageBroker;
using EssayCheek.Api.Model.EssayResult;
using EssayCheek.Api.Services.EssayResults;
using EssayCheek.Api.Services.Essays;
using EssayCheek.Api.Services.Users;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<StorageBroker>();
builder.Services.AddScoped<IStorageBroker, StorageBroker>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEssayService, EssayService>();
builder.Services.AddScoped<IEssayResultService, EssayResultService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();