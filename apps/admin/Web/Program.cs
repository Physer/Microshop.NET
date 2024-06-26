using Application;
using Authentication;
using DataManagement;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using UserManagement;
using Web.Services;

var builder = WebApplication.CreateBuilder(args);

// .NET infrastructure
builder.Services.AddHttpContextAccessor();
builder.Services.AddRazorPages();

// Custom services
builder.Services.AddScoped<CookieService>();
builder.Services.RegisterApplicationDependencies();
builder.Services.RegisterAuthenticationDependencies(builder.Configuration);
builder.Services.RegisterUserManagementDependencies(builder.Configuration);
builder.Services.RegisterDataManagementDependencies(builder.Configuration);

// Authentication and authorization
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    var signInPath = "SignIn";
    options.SlidingExpiration = true;
    options.Events = new CookieAuthenticationEvents()
    {
        OnRedirectToLogin = RedirectToAbsoluteUri(signInPath),
        OnRedirectToAccessDenied = RedirectToAbsoluteUri("Forbidden"),
        OnRedirectToLogout = RedirectToAbsoluteUri(signInPath)
    };
});
builder.Services.AddAuthorizationBuilder().AddPolicy(Globals.Authorization.AdministratorPolicyName, policy => policy.RequireClaim(ClaimTypes.Role, Globals.Authorization.AdministratorRole));

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

static Func<RedirectContext<CookieAuthenticationOptions>, Task> RedirectToAbsoluteUri(string internalPath)
{
    return (context) =>
    {
        var redirectToHost = context.HttpContext.Request.Headers.TryGetValue("X-Forwarded-Host", out var forwardedHostHeader) && !string.IsNullOrWhiteSpace((string?)forwardedHostHeader)
            ? (string?)forwardedHostHeader
            : context.HttpContext.Request.Host.Value;

        context.HttpContext.Response.Redirect($"{context.HttpContext.Request.Scheme}://{redirectToHost}/{internalPath}");
        return Task.CompletedTask;
    };
};

public partial class Program { }