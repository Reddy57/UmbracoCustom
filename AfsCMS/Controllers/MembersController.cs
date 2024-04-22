using ApplicationCore.Models;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.Common.Security;

namespace AfsCMS.Controllers;

public class MembersController : Controller
{
    private readonly IMemberManager _memberManager;
    private readonly IMemberService _memberService;
    private readonly IMemberSignInManager _memberSignInManager;
    public MembersController(IMemberService memberService, IMemberManager memberManager, IMemberSignInManager memberSignInManager)
    {
        _memberService = memberService;
        _memberManager = memberManager;
        _memberSignInManager = memberSignInManager;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Register(RegisterWithCredentialsViewModel model)
    {
        // Check if the member already exists
        if (_memberService.GetByEmail(model.Email) != null) return BadRequest("A member with this email already exists.");

        var identityUser =
            MemberIdentityUser.CreateNew(model.Email, model.Email, "Member", true, "");
        var identityResult = await _memberManager.CreateAsync(identityUser, model.Password);


        if (!identityResult.Succeeded) return BadRequest("Failed to set password.");

        return Ok("Member created successfully.");
    }
    
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
    {
        var result = await _memberSignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
        if (result.Succeeded)
        {
            return Redirect(returnUrl ?? "/");
        }

        ModelState.AddModelError("", "Invalid login attempt.");
        return View();
    }
    

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _memberSignInManager.SignOutAsync();
        return Redirect("/");
    }


}