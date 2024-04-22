using ApplicationCore.Models;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Services;

namespace AfsCMS.Controllers;

public class MembersController : Controller
{
    private readonly IMemberManager _memberManager;
    private readonly IMemberService _memberService;

    public MembersController(IMemberService memberService, IMemberManager memberManager)
    {
        _memberService = memberService;
        _memberManager = memberManager;
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
}