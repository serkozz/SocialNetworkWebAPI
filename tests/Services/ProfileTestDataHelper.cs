using Models;
using Utility;

namespace Tests.Services;
public static class ProfileTestDataHelper
{
    public static List<Profile> GetDistinctProfileData()
    {
        var authData = AuthTestDataHelper.GetDistinctAuthData();
        var firstNames = new string[] {"Sergey", "Viktor"};
        var lastNames = new string[] {"Kozlov", "Petrov"};
        var nicknames = new string[] {"serkozz", "salvage"};
        var profiles = new List<Profile>();

        for (int i = 0; i < authData.Count - 1; i++)
        {
            profiles.Add(new Profile()
            {
                Id = i + 1,
                Auth = authData[i],
                AuthId = authData[i].Id,
                FirstName = firstNames[i],
                LastName = lastNames[i],
                Nickname = nicknames[i],
                ProfileName = nicknames[i],
            });
        }
        return profiles;
    }

    public static List<Profile> GetDuplicatednameProfileData()
    {
        var authData = AuthTestDataHelper.GetDistinctAuthData();
        var firstNames = new string[] {"Sergey", "Viktor"};
        var lastNames = new string[] {"Kozlov", "Petrov"};
        var nicknames = new string[] {"serkozz", "serkozz"};
        var profiles = new List<Profile>();

        for (int i = 0; i < authData.Count - 1; i++)
        {
            profiles.Add(new Profile()
            {
                Id = i + 1,
                Auth = authData[i],
                AuthId = authData[i].Id,
                FirstName = firstNames[i],
                LastName = lastNames[i],
                Nickname = nicknames[i],
                ProfileName = nicknames[i],
            });
        }
        return profiles;
    }
}