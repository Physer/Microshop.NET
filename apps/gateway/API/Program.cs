var builder = WebApplication.CreateBuilder(args);

// Reverse proxy
builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();
app.MapReverseProxy();

app.Run();