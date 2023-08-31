using Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Web.Utilities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterAuthenticationDependencies(builder.Configuration);
builder.Services.AddRazorPages();
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
