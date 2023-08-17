using Models;
using Utility;

namespace Tests.Services;
public static class AuthServiceTestDataHelper
{
    public static List<Auth> GetDistinctAuthData()
    {
        return new List<Auth>()
        {
            new Auth() {
                Email = "serkozz@mail.ru",
                Password = PasswordUtility.HashPassword("TestPassword_1"),
                IsAdmin = false
            },
            new Auth() {
                Email = "salvage@mail.ru",
                Password = PasswordUtility.HashPassword("TestPassword_2"),
                IsAdmin = true
            }
        };
    }

    public static List<Auth> GetDuplicatedEmailsAuthData()
    {
        return new List<Auth>()
        {
            new Auth() {
                Email = "serkozz@mail.ru",
                Password = PasswordUtility.HashPassword("TestPassword_1"),
                IsAdmin = false
            },
            new Auth() {
                Email = "serkozz@mail.ru",
                Password = PasswordUtility.HashPassword("TestPassword_2"),
                IsAdmin = true
            }
        };
    }
}