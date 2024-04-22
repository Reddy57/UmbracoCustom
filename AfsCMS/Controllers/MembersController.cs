using System.Security.Claims;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.Common.Security;

namespace AfsCMS.Controllers;

public class MembersController(
    IMemberService memberService,
    IMemberManager memberManager,
    IMemberSignInManager memberSignInManager,
    IMemberGroupService memberGroupService)
    : Controller
{
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterWithCredentialsViewModel model)
    {
        // Check if the member already exists
        if (memberService.GetByEmail(model.Email) != null)
            return BadRequest("A member with this email already exists.");
        
        // Create the member identity user

        var identityUser = MemberIdentityUser.CreateNew(model.Email, model.Email, "Member", true, "");
        var identityResult = await memberManager.CreateAsync(identityUser, model.Password);

        
        // Create the member
      //  var member = new Member(memberService.CreateMember(model.Email, model.Email, model.Email, "MemberAlias"));
        if (!identityResult.Succeeded) return BadRequest("Failed to set password.");

        return Ok("Member created successfully.");
    }
    
    
    [HttpGet]
    public IActionResult RegisterMember()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> RegisterMember(RegisterViewModel model)
    {
        // Check if the member already exists
        if (memberService.GetByEmail(model.Email) != null)
            return BadRequest("A member with this email already exists.");
        
        var member = memberService.CreateMember(model.Email, model.Email, $"{model.FirstName} {model.LastName}", "Member");
        if (member == null)
        {
            return BadRequest("Failed to create member.");
        }
        // Set strongly typed custom properties
        member.SetValue("firstName", model.FirstName);
        member.SetValue("lastName", model.LastName);
        
        // Save the member to persist the changes
        memberService.Save(member);
        
        
        // Create the identity for the member
        var memberIdentityUser = new MemberIdentityUser(member.Id)
        {
            Email = member.Email,
            UserName = member.Username
        };

        
        // Set the password
        var identityResult = await memberManager.AddPasswordAsync(memberIdentityUser, "Reddy5783!!");
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
        var memberIdentityUser = await memberManager.FindByEmailAsync(model.Email);
        if (memberIdentityUser != null)
        {
            // Use MemberSignInManager to handle the sign-in process
            var result = await memberSignInManager.PasswordSignInAsync(model.Email, model.Password, true, false);
            if (result.Succeeded)
            {
                // Fetch and add role claims
                var roles = await memberManager.GetRolesAsync(memberIdentityUser);

                // Add roles to claims after successful login
                foreach (var role in roles)
                    memberIdentityUser?.Claims.Add(new IdentityUserClaim<string>
                        { ClaimType = ClaimTypes.Role, ClaimValue = role, UserId = memberIdentityUser?.Id ?? string.Empty });
                if (memberIdentityUser != null) await memberSignInManager.SignInAsync(memberIdentityUser, true);


                return Redirect("/"); // Redirect to the target page after login
            }
        }

        ModelState.AddModelError("", "Invalid login attempt.");
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await memberSignInManager.SignOutAsync();
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

        var existingGroup = memberGroupService.GetByName(groupName);
        if (existingGroup != null) return BadRequest("A group with this name already exists.");

        var newGroup = new MemberGroup { Name = groupName };
        memberGroupService.Save(newGroup);

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
        var member = memberService.GetByEmail(memberEmail);
        if (member == null) return NotFound("Member not found.");

        var group = memberGroupService.GetByName(groupName);
        if (group == null) return NotFound("Member group not found.");

        // Check if the member is already in the group
        var memberGroups = memberService.GetAllRoles(member.Id);
        if (memberGroups.Contains(groupName)) return BadRequest("Member is already in this group.");

        memberService.AssignRole(member.Id, groupName);

        return Ok($"Member '{memberEmail}' added to group '{groupName}'.");
    }
}