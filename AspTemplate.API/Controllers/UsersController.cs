using AspTemplate.API.Contracts.Users;
using AspTemplate.Application.Models.Users;
using AspTemplate.Core.Interfaces.Services;
using AspTemplate.Infrastructure.Authentication;
using MapsterMapper;
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
		[FromServices] IMapper mapper,
		IUserService usersService)
	{
		var registerUserModel = mapper.Map<RegisterUserModel>(request);
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
        var user = await usersService.GetUserByEmail(email);
        return Ok(user);
    }
}