using FluentAssertions.Execution;
using Moq.EntityFrameworkCore;
using Services;
using Models;
using EF;
using Controllers;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Tests.Services;

public class ProfileControllerTesting
{
    [Fact]
    public void Create_WithNonExistentEmail_ReturnsProfile()
    {
        // Arrange
        var mockContext = new Mock<ApplicationContext>();
        mockContext.Setup(c => c.Profiles).ReturnsDbSet(ProfileTestDataHelper.GetDistinctProfileData());
        mockContext.Setup(c => c.Auth).ReturnsDbSet(AuthTestDataHelper.GetDistinctAuthData());
        
        ProfileCreationInfo profileCreationInfo = new()
        {
            FirstName = "FirstName",
            LastName = "LastName",
            ProfileName = "serkozz"
        };

        //Act
        ProfileService profileService = new(mockContext.Object);
        ProfileController sut = new ProfileController(profileService);
        var result = sut.Create(profileCreationInfo);
        //Assert
        using (new AssertionScope())
        {
            result.Should().BeOfType<Ok>();
        }
    }
}