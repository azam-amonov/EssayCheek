namespace EssayCheek.Api.Configuration;

public partial class HostConfiguration
{
  public static ValueTask<WebApplicationBuilder> ConfigureAsync(this WebApplicationBuilder builder)
  {
    builder
      .AddBrokers()
      .AddServices()
      .AddTextInputFormatter()
      .AddDevTools()
      .AddBot();
    return new(builder);
  }
  
  public static ValueTask<WebApplication> ConfigureAsync(this WebApplication app)
  {
    app
      .UseDevTools()
      .UseExposes();
    
    return new(app);
  }
}