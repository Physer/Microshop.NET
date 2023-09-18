using Application.Authentication;
using Domain;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages;

public class ManageUsersModel : PageModel
{
    public IAsyncEnumerable<User> Users = AsyncEnumerable.Empty<User>();

    private readonly IUserClient _userClient;

    public ManageUsersModel(IUserClient userClient) => _userClient = userClient;

    public void OnGet() => Users = _userClient.GetUsers();
}
