using Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Web.Services;
using Web.Utilities;

var builder = WebApplication.CreateBuilder(args);

// .NET infrastructure
builder.Services.AddHttpContextAccessor();
builder.Services.AddRazorPages();

// Custom services
builder.Services.AddScoped<CookieService>();
builder.Services.RegisterAuthenticationDependencies(builder.Configuration);

// Authentication and authorization
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    var signInPath = "/SignIn";
    options.SlidingExpiration = true;
    options.AccessDeniedPath = "/Forbidden";
    options.LoginPath = signInPath;
    options.LogoutPath = signInPath;
});
builder.Services.AddAuthorization(options => options.AddPolicy(AuthorizationDefaults.AdministratorPolicyName, policy => policy.RequireClaim("Role", AuthorizationDefaults.AdministratorRole)));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.Run();
