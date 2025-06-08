using DesafioInvestimentosItau.Application.User.User.Client.DTOs;
using DesafioInvestimentosItau.Application.User.User.Contracts.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DesafioInvestimentosItau.Api.Controller;

[ApiController]
[Route("users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IUserService userService, ILogger<UsersController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestDto dto)
    {
        try
        {
            var user = await _userService.CreateAsync(dto);
            return Ok(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create user with email: {Email}", dto.Email);
            return BadRequest(new { error = ex.Message });
        }
    }
}