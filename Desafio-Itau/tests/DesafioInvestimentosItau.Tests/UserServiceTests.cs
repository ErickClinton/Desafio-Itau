using Moq;
using FluentAssertions;
using DesafioInvestimentosItau.Application.User.User.Client;
using DesafioInvestimentosItau.Application.User.User.Client.DTOs;
using DesafioInvestimentosItau.Application.User.User.Contracts.Interfaces;
using DesafioInvestimentosItau.Domain.Entities;
using Microsoft.Extensions.Logging;
using Xunit;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<ILogger<UserService>> _loggerMock;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _loggerMock = new Mock<ILogger<UserService>>();
        _userService = new UserService(_userRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateUser_WhenEmailDoesNotExist()
    {
        // Arrange
        var dto = new CreateUserRequestDto
        {
            Name = "Test User",
            Email = "test@example.com",
            BrokerageFee = 5.5m
        };

        _userRepositoryMock.Setup(r => r.ExistsAsync(dto.Email)).ReturnsAsync(false);
        _userRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<UserEntity>()))
            .ReturnsAsync((UserEntity u) => {
                u.Id = 1;
                return u;
            });

        // Act
        var result = await _userService.CreateAsync(dto);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.Email.Should().Be(dto.Email);
        result.Name.Should().Be(dto.Name);
        result.BrokerageFee.Should().Be(dto.BrokerageFee);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowException_WhenEmailAlreadyExists()
    {
        // Arrange
        var dto = new CreateUserRequestDto
        {
            Name = "Existing User",
            Email = "exists@example.com",
            BrokerageFee = 4.2m
        };

        _userRepositoryMock.Setup(r => r.ExistsAsync(dto.Email)).ReturnsAsync(true);

        // Act
        Func<Task> act = async () => await _userService.CreateAsync(dto);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("*already exists*");
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnUser_WhenExists()
    {
        // Arrange
        var user = new UserEntity { Id = 1, Name = "User", Email = "user@example.com", BrokerageFee = 2.5m };
        _userRepositoryMock.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);

        // Act
        var result = await _userService.GetByIdAsync(user.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Email.Should().Be("user@example.com");
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnListOfUsers()
    {
        // Arrange
        var users = new List<UserEntity>
        {
            new UserEntity { Name = "A", Email = "a@x.com", BrokerageFee = 1.1m },
            new UserEntity { Name = "B", Email = "b@x.com", BrokerageFee = 2.2m }
        };
        _userRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(users);

        // Act
        var result = await _userService.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(r => r.Email == "a@x.com");
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnTrue_WhenUserExists()
    {
        // Arrange
        var email = "x@x.com";
        _userRepositoryMock.Setup(r => r.ExistsAsync(email)).ReturnsAsync(true);

        // Act
        var exists = await _userService.ExistsAsync(email);

        // Assert
        exists.Should().BeTrue();
    }
}
