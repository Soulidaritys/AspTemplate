using AspTemplate.API.Contracts.Users;
using AspTemplate.Application.Models.Users;
using AspTemplate.Core.Interfaces.Services;
using AspTemplate.Core.Models;
using AspTemplate.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspTemplate.API.Controllers;

[ApiController]
[ApiExplorerSettings(GroupName = "v1")]
[Route("api/v1/users/[action]")]
[Tags("Users")]
public sealed class UsersController : ControllerBase
{
	[HttpPost]
	[AllowAnonymous]
	public async Task<IActionResult> Register(
		[FromBody] RegisterUserRequest request,
		IUserService usersService)
	{
		var registerUserModel = request.ToModel();
		await usersService.Register(registerUserModel);
        return Ok();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(
		[FromBody] LoginUserRequest request,
        [FromServices] IUserService usersService)
	{
		var token = await usersService.Login(request.Email, request.Password);

		HttpContext.Response.Cookies.Append("secretCookie", token.Token);

		return Ok(token);
	}

    [HttpGet]
	[Authorize(PolicyConstants.AdminPolicy)]
    public async Task<IActionResult> GetByEmail(
        [FromQuery] string email,
        IUserService usersService)
    {
		var currentUserId = 
            new UserId(Guid.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == CustomClaims.UserId)?.Value));
        var user = await usersService.GetUserByEmail(email);
        return Ok(user);
    }
}