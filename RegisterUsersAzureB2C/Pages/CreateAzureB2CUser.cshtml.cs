﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using RegisterUsersAzureB2C.CreateUser;
using RegisterUsersAzureB2C.Services;
using System.Threading.Tasks;

namespace RegisterUsersAzureB2C.Pages;

[Authorize(Policy = "IsAdminPolicy")]
public class CreateAzureB2CUserModel : PageModel
{
    private readonly MsGraphService _msGraphService;

    public CreateAzureB2CUserModel(MsGraphService msGraphService,
        IConfiguration configuration)
    {
        _msGraphService = msGraphService;
        AadB2CIssuerDomain = configuration.GetValue<string>("AzureAdB2C:Domain");
    }

    public IActionResult OnGet()
    {
        return Page();
    }

    [BindProperty]
    public UserModelB2CTenant UserModel { get; set; } = new UserModelB2CTenant();

    [BindProperty]
    public string AadB2CIssuerDomain { get; set; }

    [BindProperty]
    public string UserPassword { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var (_, Password, _) = await _msGraphService.CreateAzureB2CSameDomainUserAsync(UserModel);

        UserPassword = Password;
        return OnGet();
    }

}