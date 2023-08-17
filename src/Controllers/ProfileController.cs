using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using Models;

namespace Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class ProfileController : ControllerBase
{
    private readonly ProfileService _profileService;

    public ProfileController(ProfileService profileService)
    {
        _profileService = profileService;
    }

    [Route("create")]
    [HttpPost]
    public IResult Create(ProfileCreationInfo profileCreationInfo)
    {
        var email = User.Identities.ToList().FirstOrDefault()?.Claims
            .Where(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")
            .FirstOrDefault()?.Value;

        ArgumentNullException.ThrowIfNull(email);

        var profileOrError = _profileService.Create(email, profileCreationInfo);

        if (profileOrError.IsT1)
            return Results.NotFound(profileOrError.AsT1);

        return Results.Ok(profileOrError.AsT0);
    }

    [Route("{profileName}")]
    [HttpGet]
    public IResult Get(string profileName)
    {
        var profileOrError = _profileService.Get(GetByProfile.ProfileName, profileName);

        if (profileOrError.IsT1)
            return Results.NotFound(profileOrError.AsT1);

        return Results.Ok(profileOrError.AsT0);
    }

    [Route("modify")]
    [HttpPost]
    public IResult Modify(ProfileModifyInfo _)
    {
        var email = User.Identities.ToList().FirstOrDefault()?.Claims
            .Where(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")
            .FirstOrDefault()?.Value;

        throw new NotImplementedException();
    }

    [Route("modify/{profileName}")]
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IResult Modify(string profileName, ProfileModifyInfo profileModifyInfo)
    {
        throw new NotImplementedException();
    }
}