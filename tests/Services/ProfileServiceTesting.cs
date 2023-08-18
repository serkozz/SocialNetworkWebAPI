using FluentAssertions.Execution;
using Moq.EntityFrameworkCore;
using Services;
using Models;
using EF;

namespace Tests.Services;

public class ProfileServiceTesting
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
        var email = "example@mail.ru";

        //Act
        ProfileService sut = new(mockContext.Object);
        var result = sut.Create(email, profileCreationInfo);
        //Assert
        using (new AssertionScope())
        {
            result.IsT0.Should().BeTrue();
        }
    }

    [Fact]
    public void Create_WithExistentEmail_ReturnsError()
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
        var email = "serkozz@mail.ru";

        //Act
        ProfileService sut = new(mockContext.Object);
        var result = sut.Create(email, profileCreationInfo);
        //Assert
        using (new AssertionScope())
        {
            result.IsT1.Should().BeTrue();
        }
    }

    [Fact]
    public void Get_ByExistentProfileName_ReturnsProfile()
    {
        // Arrange
        var mockContext = new Mock<ApplicationContext>();
        mockContext.Setup(c => c.Profiles).ReturnsDbSet(ProfileTestDataHelper.GetDistinctProfileData());
        mockContext.Setup(c => c.Auth).ReturnsDbSet(AuthTestDataHelper.GetDistinctAuthData());
        
        var profileName = "serkozz";

        //Act
        ProfileService sut = new(mockContext.Object);
        var result = sut.Get(GetByProfile.ProfileName, profileName);
        //Assert
        using (new AssertionScope())
        {
            result.IsT0.Should().BeTrue();
        }
    }

    [Fact]
    public void Get_ByExistentEmail_ReturnsProfile()
    {
        // Arrange
        var mockContext = new Mock<ApplicationContext>();
        mockContext.Setup(c => c.Profiles).ReturnsDbSet(ProfileTestDataHelper.GetDistinctProfileData());
        mockContext.Setup(c => c.Auth).ReturnsDbSet(AuthTestDataHelper.GetDistinctAuthData());
        
        var email = "serkozz@mail.ru";

        //Act
        ProfileService sut = new(mockContext.Object);
        var result = sut.Get(GetByProfile.Email, email);
        //Assert
        using (new AssertionScope())
        {
            result.IsT0.Should().BeTrue();
        }
    }

    [Fact]
    public void Get_ByExistentId_ReturnsProfile()
    {
        // Arrange
        var mockContext = new Mock<ApplicationContext>();
        mockContext.Setup(c => c.Profiles).ReturnsDbSet(ProfileTestDataHelper.GetDistinctProfileData());
        mockContext.Setup(c => c.Auth).ReturnsDbSet(AuthTestDataHelper.GetDistinctAuthData());
        
        var id = 1.ToString();

        //Act
        ProfileService sut = new(mockContext.Object);
        var result = sut.Get(GetByProfile.Id, id);
        //Assert
        using (new AssertionScope())
        {
            result.IsT0.Should().BeTrue();
        }
    }

    [Fact]
    public void Get_ByExistentIdWhereIdCantBeParsedToInt_ReturnsError()
    {
        // Arrange
        var mockContext = new Mock<ApplicationContext>();
        mockContext.Setup(c => c.Profiles).ReturnsDbSet(ProfileTestDataHelper.GetDistinctProfileData());
        mockContext.Setup(c => c.Auth).ReturnsDbSet(AuthTestDataHelper.GetDistinctAuthData());
        
        var id = "Hello".ToString();

        //Act
        ProfileService sut = new(mockContext.Object);
        var result = sut.Get(GetByProfile.Id, id);
        //Assert
        using (new AssertionScope())
        {
            result.IsT1.Should().BeTrue();
        }
    }

    [Fact]
    public void Get_ByNonExistentValue_ReturnsError()
    {
        // Arrange
        var mockContext = new Mock<ApplicationContext>();
        mockContext.Setup(c => c.Profiles).ReturnsDbSet(ProfileTestDataHelper.GetDistinctProfileData());
        mockContext.Setup(c => c.Auth).ReturnsDbSet(AuthTestDataHelper.GetDistinctAuthData());
        
        var id = 10000.ToString();
        var name = "SurelyNotExistableName";
        var email = "somerandomemail@MailAddress.ru";

        //Act
        ProfileService sut = new(mockContext.Object);
        var resultId = sut.Get(GetByProfile.Id, id);
        var resultEmail = sut.Get(GetByProfile.Email, email);
        var resultName = sut.Get(GetByProfile.ProfileName, name);
        var resultUnknown = sut.Get(GetByProfile.CASE_FOR_TESTING_ONLY_DONT_USE_IT, name);
        //Assert
        using (new AssertionScope())
        {
            resultId.IsT1.Should().BeTrue();
            resultEmail.IsT1.Should().BeTrue();
            resultName.IsT1.Should().BeTrue();
            resultUnknown.IsT1.Should().BeTrue();
        }
    }

    [Fact]
    public void Get_DuplicateProfileFound_ThrowsException()
    {
        // Arrange
        var mockContext = new Mock<ApplicationContext>();
        mockContext.Setup(c => c.Profiles).ReturnsDbSet(ProfileTestDataHelper.GetDuplicatednameProfileData());
        mockContext.Setup(c => c.Auth).ReturnsDbSet(AuthTestDataHelper.GetDistinctAuthData());
        
        var name = "serkozz";

        //Act
        ProfileService sut = new(mockContext.Object);
        Action act = () => sut.Get(GetByProfile.ProfileName, name);
        //Assert
        using (new AssertionScope())
        {
            act.Should().Throw<ProfileNameDuplicateFoundException>();
        }
    }
}