using Application;
using Application.UserManagement;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages;

[Authorize(Policy = Globals.Authorization.AdministratorPolicyName)]
public class ManageUsersModel(IUserClient userClient) : PageModel
{
    public IAsyncEnumerable<User> Users = AsyncEnumerable.Empty<User>();

    private readonly IUserClient _userClient = userClient;

    public void OnGet() => Users = _userClient.GetUsersAsync();
}
