using Microsoft.EntityFrameworkCore;
using Extensions;
using Models;
using OneOf;
using EF;

namespace Services;
public class ProfileService
{
    private readonly ApplicationContext Db;

    public ProfileService(ApplicationContext db)
    {
        Db = db;
    }

    public OneOf<Profile, ErrorInfo> Create(string email, ProfileCreationInfo profileCreationInfo)
    {
        var existingProfilesByEmail = Db.Profiles.Where(profile => profile.Auth.Email == email);

        if (existingProfilesByEmail is not null && existingProfilesByEmail.Any())
            return new ErrorInfo($@"Profile for email '{email}' is already registered");

        var authInfo = Db.Auth.First(authInfo => authInfo.Email == email);

        var profile = new Profile()
        {
            Auth = authInfo,
            ProfileName = GenerateUniqueProfileName(),
            FirstName = profileCreationInfo.FirstName,
            LastName = profileCreationInfo.LastName,
        };

        Db.Profiles.Add(profile);
        Db.SaveChanges();
        return profile;

    }

    private string GenerateUniqueProfileName()
    {
        bool uniqueGenerated = false;
        string profileName = string.Empty;
        var existingProfileNames = Db.Profiles.Select(profile => profile.ProfileName);

        while (!uniqueGenerated)
        {
            profileName = string.Empty.RandomString(9);

            if (!existingProfileNames.Contains(profileName))
                uniqueGenerated = !uniqueGenerated;
        }
        return profileName;
    }

    public OneOf<Profile, ErrorInfo> Get(GetByProfile by, string value)
    {
        List<Profile>? existingProfile = null;

        if (by == GetByProfile.ProfileName)
            existingProfile = Db.Profiles.Where(profile => profile.ProfileName == value)
                .Include(p => p.Auth).ToList();

        else if (by == GetByProfile.Email)
            existingProfile = Db.Profiles.Where(profile => profile.Auth.Email == value)
                .Include(p => p.Auth).ToList();

        else if (by == GetByProfile.Id)
        {
            if (!Int32.TryParse(value, out var id))
                return new ErrorInfo<string>("Can not get profile by id. Can not convert string value to integer id", additionalInfo: new List<string>()
                    { $@"CausedErrorMethodName: {nameof(Profile)}.{nameof(Get)}"});

            existingProfile = Db.Profiles.Where(profile => profile.Id == id)
                .Include(p => p.Auth).ToList();
        }

        if (existingProfile is null || !existingProfile.Any())
            return new ErrorInfo($@"Profile with {(by == GetByProfile.Email ? "email" : "name")} '{value} was not found'");

        if (existingProfile.Count() > 1)
            throw new ProfileNameDuplicateFoundException(value);

        return existingProfile.First();
    }
}

public enum GetByProfile
{
    Id,
    Email,
    ProfileName
}
