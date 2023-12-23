using EssayCheek.Api.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Life cycles
await builder.ConfigureAsync();

var app = builder.Build();

app.Use(async (context, next) =>
{
    Console.WriteLine(context.Request.ContentType);
    await next(context);
});

await app.ConfigureAsync();
await app.RunAsync();