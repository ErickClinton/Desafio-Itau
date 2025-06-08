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
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestDto createUserRequestDto)
    {
        _logger.LogInformation($"Start method CreateBuyTrade - Request - {createUserRequestDto}");
        try
        {
            var user = await _userService.CreateAsync(createUserRequestDto);
            return Ok(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create user with email: {Email}", createUserRequestDto.Email);
            return BadRequest(new { error = ex.Message });
        }
    }
}