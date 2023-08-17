using FluentAssertions.Execution;
using Microsoft.AspNetCore.Http;
using Moq.EntityFrameworkCore;
using Services;
using Models;
using EF;
using Tests.Services;
using Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Tests.Controllers;

public class AuthControllerTesting
{
    // var controller = new HomeController(mockRepo.Object);

    [Fact]
    public void CreateExerlastingToken_ReturnsOk()
    {
        // Arrange
        var mockContext = new Mock<ApplicationContext>();
        mockContext.Setup(c => c.Auth).ReturnsDbSet(AuthServiceTestDataHelper.GetDistinctAuthData());
        var authUser = new Auth()
        {
            Email = "serkozz@mail.ru",
            Password = "TestPassword_1"
        };

        var authAdmin = new Auth()
        {
            Email = "salvage@mail.ru",
            Password = "TestPassword_2",
            IsAdmin = true
        };

        //Act
        AuthService authService = new(mockContext.Object);
        AuthController sut = new(authService);

        var resultUser = sut.CreateEverlastingToken(authUser);
        var resultAdmin = sut.CreateEverlastingToken(authAdmin);

        //Assert
        using (new AssertionScope())
        {
            resultUser.ToString().Should().Contain(nameof(Ok));
            resultAdmin.ToString().Should().Contain(nameof(Ok));
        }
    }

    [Fact]
    public void Register_WithValidAuthData_ReturnsOk()
    {
        // Arrange
        var mockContext = new Mock<ApplicationContext>();
        mockContext.Setup(c => c.Auth).ReturnsDbSet(AuthServiceTestDataHelper.GetDistinctAuthData());

        var auth = new Auth()
        {
            Email = "salvage_1@mail.ru",
            Password = "TestPassword_2",
            IsAdmin = true
        };

        //Act
        AuthService authService = new(mockContext.Object);
        AuthController sut = new(authService);

        var result = sut.Register(auth);

        //Assert
        using (new AssertionScope())
        {
            result.ToString().Should().Contain(nameof(Ok));
        }
    }

    [Fact]
    public void Register_WithInvalidAuthData_ReturnsNotFound()
    {
        // Arrange
        var mockContext = new Mock<ApplicationContext>();
        mockContext.Setup(c => c.Auth).ReturnsDbSet(AuthServiceTestDataHelper.GetDistinctAuthData());

        var auth = new Auth()
        {
            Email = "salvagemail.ru",
            Password = "TestPassword_2",
            IsAdmin = true
        };

        //Act
        AuthService authService = new(mockContext.Object);
        AuthController sut = new(authService);

        var result = sut.Register(auth);

        //Assert
        using (new AssertionScope())
        {
            result.ToString().Should().Contain(nameof(NotFound));
        }
    }

    [Fact]
    public void Login_WithInvalidAuthData_ReturnsNotFound()
    {
        // Arrange
        var mockContext = new Mock<ApplicationContext>();
        mockContext.Setup(c => c.Auth).ReturnsDbSet(AuthServiceTestDataHelper.GetDistinctAuthData());

        var auth = new Auth()
        {
            Email = "salvagemail.ru",
            Password = "TestPassword_2",
            IsAdmin = true
        };

        //Act
        AuthService authService = new(mockContext.Object);
        AuthController sut = new(authService);

        var result = sut.Login(auth);

        //Assert
        using (new AssertionScope())
        {
            result.ToString().Should().Contain(nameof(NotFound));
        }
    }

    [Fact]
    public void Login_WithValidAuthData_ReturnsOk()
    {
        // Arrange
        var mockContext = new Mock<ApplicationContext>();
        mockContext.Setup(c => c.Auth).ReturnsDbSet(AuthServiceTestDataHelper.GetDistinctAuthData());

        var auth = new Auth()
        {
            Email = "salvage@mail.ru",
            Password = "TestPassword_2",
            IsAdmin = true
        };

        //Act
        AuthService authService = new(mockContext.Object);
        AuthController sut = new(authService);

        var result = sut.Login(auth);

        //Assert
        using (new AssertionScope())
        {
            result.ToString().Should().Contain(nameof(Ok));
        }
    }
}