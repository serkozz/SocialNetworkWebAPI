using FluentAssertions.Execution;
using Moq.EntityFrameworkCore;
using Services;
using Models;
using EF;

namespace Tests.Services;

public class AuthServiceTesting
{
    [Fact]
    public void Register_WithValidUniqueEmail_ReturnsJWTString()
    {
        // Arrange
        var mockContext = new Mock<ApplicationContext>();
        mockContext.Setup(c => c.Auth).ReturnsDbSet(AuthServiceTestDataHelper.GetDistinctAuthData());
        var authUser = new Auth()
        {
            Email = "example@mail.ru",
            Password = "TestPassword_2"
        };

        var authAdmin = new Auth()
        {
            Email = "example@mail.ru",
            Password = "TestPassword_2",
            IsAdmin = true
        };

        //Act
        AuthService sut = new(mockContext.Object);
        var userResult = sut.Register(authUser);
        var adminResult = sut.Register(authAdmin);

        //Assert
        using (new AssertionScope())
        {
            userResult.IsT0.Should().BeTrue();
            adminResult.IsT0.Should().BeTrue();
        }

    }

    [Fact]
    public void Register_WithValidDuplicateEmail_ReturnsError()
    {
        // Arrange
        var mockContext = new Mock<ApplicationContext>();
        var authDataList = AuthServiceTestDataHelper.GetDistinctAuthData();
        mockContext.Setup(c => c.Auth).ReturnsDbSet(authDataList);
        var auth = authDataList.First();

        //Act
        AuthService sut = new(mockContext.Object);
        var result = sut.Register(auth);

        //Assert
        result.IsT1.Should().BeTrue();
    }

    [Fact]
    public void Register_WithInvalidEmail_ReturnsError()
    {

        // Arrange
        var mockContext = new Mock<ApplicationContext>();
        var authDataList = AuthServiceTestDataHelper.GetDistinctAuthData();
        mockContext.Setup(c => c.Auth).ReturnsDbSet(authDataList);
        var auth = new Auth()
        {
            Email = "Invalidemail.com",
            Password = "ExamplePassword_0"
        };

        //Act
        AuthService sut = new(mockContext.Object);
        var result = sut.Register(auth);

        //Assert
        result.IsT1.Should().BeTrue();
    }

    [Fact]
    public void Register_WithInvalidPassword_ReturnsError()
    {
        // Arrange
        var mockContext = new Mock<ApplicationContext>();
        var authDataList = AuthServiceTestDataHelper.GetDistinctAuthData();
        mockContext.Setup(c => c.Auth).ReturnsDbSet(authDataList);
        var shortPasswordAuth = new Auth()
        {
            Email = "valid_email1@mail.ru",
            Password = "Ivy_02"
        };
        var numberlessPasswordAuth = new Auth()
        {
            Email = "valid_email2@mail.ru",
            Password = "Hello_World"
        };
        var symbollessPasswordAuth = new Auth()
        {
            Email = "valid_email3@mail.ru",
            Password = "HelloWorld02"
        };
        var letterlessPasswordAuth = new Auth()
        {
            Email = "valid_email4@mail.ru",
            Password = "228_48_186_08"
        };

        //Act
        AuthService sut = new(mockContext.Object);
        var shortPasswordAuthResult = sut.Register(shortPasswordAuth);
        var numberlessPasswordAuthResult = sut.Register(numberlessPasswordAuth);
        var symbollessPasswordAuthResult = sut.Register(symbollessPasswordAuth);
        var letterlessPasswordAuthResult = sut.Register(letterlessPasswordAuth);

        //Assert
        using (new AssertionScope())
        {
            shortPasswordAuthResult.IsT1.Should().BeTrue();
            numberlessPasswordAuthResult.IsT1.Should().BeTrue();
            symbollessPasswordAuthResult.IsT1.Should().BeTrue();
            letterlessPasswordAuthResult.IsT1.Should().BeTrue();
        }
    }

    [Fact]
    public void Login_WithInvalidEmail_ReturnsError()
    {
        // Arrange
        var mockContext = new Mock<ApplicationContext>();
        var authDataList = AuthServiceTestDataHelper.GetDistinctAuthData();
        mockContext.Setup(c => c.Auth).ReturnsDbSet(authDataList);
        var auth = new Auth()
        {
            Email = "serkozzmail.ru",
            Password = "TestPassword_1"
        };

        //Act
        AuthService sut = new(mockContext.Object);
        var result = sut.Login(auth);

        //Assert
        using (new AssertionScope())
        {
            result.IsT1.Should().BeTrue();
        }
    }

    [Fact]
    public void Login_WithValidData_ReturnsJWTString()
    {
        // Arrange
        var mockContext = new Mock<ApplicationContext>();
        var authDataList = AuthServiceTestDataHelper.GetDistinctAuthData();
        mockContext.Setup(c => c.Auth).ReturnsDbSet(authDataList);
        var authUser = new Auth()
        {
            Email = "serkozz@mail.ru",
            Password = "TestPassword_1"
        };

        var authAdmin = new Auth()
        {
            Email = "salvage@mail.ru",
            Password = "TestPassword_2"
        };

        //Act
        AuthService sut = new(mockContext.Object);
        var resultUser = sut.Login(authUser);
        var resultAdmin = sut.Login(authAdmin);

        //Assert
        using (new AssertionScope())
        {
            resultUser.IsT0.Should().BeTrue();
            resultAdmin.IsT0.Should().BeTrue();
        }
    }

    [Fact]
    public void Login_WithNonExistentEmail_ReturnsError()
    {
        // Arrange
        var mockContext = new Mock<ApplicationContext>();
        var authDataList = AuthServiceTestDataHelper.GetDistinctAuthData();
        mockContext.Setup(c => c.Auth).ReturnsDbSet(authDataList);
        var auth = new Auth()
        {
            Email = "serkozznew@mail.ru",
            Password = "TestPassword_1"
        };

        //Act
        AuthService sut = new(mockContext.Object);
        var result = sut.Login(auth);

        //Assert
        using (new AssertionScope())
        {
            result.IsT1.Should().BeTrue();
        }
    }

    [Fact]
    public void Login_WithWrongPassword_ReturnsError()
    {
        // Arrange
        var mockContext = new Mock<ApplicationContext>();
        var authDataList = AuthServiceTestDataHelper.GetDistinctAuthData();
        mockContext.Setup(c => c.Auth).ReturnsDbSet(authDataList);
        var auth = new Auth()
        {
            Email = "serkozz@mail.ru",
            Password = "TestPassword_12"
        };

        //Act
        AuthService sut = new(mockContext.Object);
        var result = sut.Login(auth);

        //Assert
        using (new AssertionScope())
        {
            result.IsT1.Should().BeTrue();
        }
    }

    [Fact]
    public void Login_WhenDbFoundContainingDuplicateEmails_ThrowsException()
    {
        // Arrange
        var mockContext = new Mock<ApplicationContext>();
        var authDataList = AuthServiceTestDataHelper.GetDuplicatedEmailsAuthData();
        mockContext.Setup(c => c.Auth).ReturnsDbSet(authDataList);
        var auth = new Auth()
        {
            Email = "serkozz@mail.ru",
            Password = "TestPassword_1"
        };

        //Act
        AuthService sut = new(mockContext.Object);
        Action act = () => sut.Login(auth);

        //Assert
        using (new AssertionScope())
        {
            act.Should().Throw<UserEmailDuplicateFoundException>();
        }
    }
}