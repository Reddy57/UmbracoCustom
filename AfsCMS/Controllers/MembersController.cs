using System.Security.Claims;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.Common.Security;

namespace AfsCMS.Controllers;

public class MembersController : Controller
{
    private readonly IMemberGroupService _memberGroupService;
    private readonly IMemberManager _memberManager;
    private readonly IMemberService _memberService;
    private readonly IMemberSignInManager _memberSignInManager;

    public MembersController(IMemberService memberService, IMemberManager memberManager,
        IMemberSignInManager memberSignInManager, IMemberGroupService memberGroupService)
    {
        _memberService = memberService;
        _memberManager = memberManager;
        _memberSignInManager = memberSignInManager;
        _memberGroupService = memberGroupService;
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
        if (_memberService.GetByEmail(model.Email) != null)
            return BadRequest("A member with this email already exists.");

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
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        // Attempt to find the member identity user first
        var memberIdentityUser = await _memberManager.FindByEmailAsync(model.Email);
        if (memberIdentityUser != null)
        {
            // Use MemberSignInManager to handle the sign-in process
            var result = await _memberSignInManager.PasswordSignInAsync(model.Email, model.Password, true, false);
            if (result.Succeeded)
            {
                // Fetch and add role claims
                var roles = await _memberManager.GetRolesAsync(memberIdentityUser);

                // Add roles to claims after successful login
                foreach (var role in roles)
                    memberIdentityUser?.Claims.Add(new IdentityUserClaim<string>
                        { ClaimType = ClaimTypes.Role, ClaimValue = role, UserId = memberIdentityUser?.Id ?? string.Empty });
                if (memberIdentityUser != null) await _memberSignInManager.SignInAsync(memberIdentityUser, true);


                return RedirectToAction("Index"); // Redirect to the target page after login
            }
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


    [HttpGet]
    public IActionResult CreateGroup()
    {
        return View();
    }


    [HttpPost("CreateGroup")]
    public IActionResult CreateGroup(string groupName)
    {
        if (string.IsNullOrEmpty(groupName)) return BadRequest("Group name cannot be empty.");

        var existingGroup = _memberGroupService.GetByName(groupName);
        if (existingGroup != null) return BadRequest("A group with this name already exists.");

        var newGroup = new MemberGroup { Name = groupName };
        _memberGroupService.Save(newGroup);

        return Ok($"Member group '{groupName}' created successfully.");
    }


    [HttpGet]
    public IActionResult CreateMemberGroup()
    {
        return View();
    }

    [HttpPost("CreateMemberGroup")]
    public IActionResult CreateMemberGroup(string memberEmail, string groupName)
    {
        var member = _memberService.GetByEmail(memberEmail);
        if (member == null) return NotFound("Member not found.");

        var group = _memberGroupService.GetByName(groupName);
        if (group == null) return NotFound("Member group not found.");

        // Check if the member is already in the group
        var memberGroups = _memberService.GetAllRoles(member.Id);
        if (memberGroups.Contains(groupName)) return BadRequest("Member is already in this group.");

        _memberService.AssignRole(member.Id, groupName);

        return Ok($"Member '{memberEmail}' added to group '{groupName}'.");
    }
}