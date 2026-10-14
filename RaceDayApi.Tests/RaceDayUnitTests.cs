using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayApi.Controllers;
using RaceDayApi.Data;
using RaceDayApi.Models;
using RaceDayApi.Server.DTOs;
using FluentAssertions;
using Xunit;

public class RaceDayUnitTests
{
    private RaceDayDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<RaceDayDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new RaceDayDbContext(options);
    }

    [Fact]
    public async Task Register_ShouldCreateUser_WhenDataIsValid()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var configMock = new Microsoft.Extensions.Configuration.ConfigurationBuilder().Build();
        var controller = new AuthController(context, configMock);

        var dto = new RegisterDto("Maria Silva", "maria@raceday.com", "Password123!", "Participant");

        // Act
        var result = await controller.Register(dto);

        // Assert
        var statusCodeResult = result.Should().BeOfType<ObjectResult>().Subject;
        statusCodeResult.StatusCode.Should().Be(201);

        var userInDb = await context.Users.FirstOrDefaultAsync(u => u.Email == "maria@raceday.com");
        userInDb.Should().NotBeNull();
        userInDb!.FullName.Should().Be("Maria Silva");
    }

    [Fact]
    public async Task CreateCategory_ShouldReturnBadRequest_WhenEventDoesNotExist()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var controller = new CategoriesController(context);

        //EventId
        var dto = new CreateCategoryDto("Senior 10km", 150.00m, 999);

        // Act
        var result = await controller.CreateCategory(dto);

        // Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }
}