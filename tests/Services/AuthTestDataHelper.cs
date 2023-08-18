using Models;
using Utility;

namespace Tests.Services;
public static class AuthTestDataHelper
{
    public static List<Auth> GetDistinctAuthData()
    {
        return new List<Auth>()
        {
            new Auth() {
                Id = 1,
                Email = "serkozz@mail.ru",
                Password = PasswordUtility.HashPassword("TestPassword_1"),
                IsAdmin = false
            },
            new Auth() {
                Id = 2,
                Email = "salvage@mail.ru",
                Password = PasswordUtility.HashPassword("TestPassword_2"),
                IsAdmin = true
            },
            new Auth() {
                Id = 3,
                Email = "example@mail.ru",
                Password = PasswordUtility.HashPassword("TestPassword_3"),
                IsAdmin = false
            }
        };
    }

    public static List<Auth> GetDuplicatedEmailsAuthData()
    {
        return new List<Auth>()
        {
            new Auth() {
                Id = 1,
                Email = "serkozz@mail.ru",
                Password = PasswordUtility.HashPassword("TestPassword_1"),
                IsAdmin = false
            },
            new Auth() {
                Id = 2,
                Email = "serkozz@mail.ru",
                Password = PasswordUtility.HashPassword("TestPassword_2"),
                IsAdmin = true
            }
        };
    }
}