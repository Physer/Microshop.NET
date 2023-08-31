﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.Utilities;

namespace Web.Pages;

[Authorize(Policy = AuthorizationDefaults.AdministratorPolicyName)]
public class IndexModel : PageModel
{
    public void OnGet() { }
}