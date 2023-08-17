using System.Security.Claims;
using Utility;
using EF;

namespace Extensions;
public static class StringExtensions
{
    /// <summary>
    /// Checks if email is valid
    /// </summary>
    /// <param name="email">Email to check</param>
    public static bool IsValidEmail(this string email) => CompiledRegex.ValidEmail().IsMatch(email);

    public static string RandomString(this string _, int length)
    {
        string chars = "aAbBcCdDeEfFgGhHiIjJkKlLmMnNoOpPqQrRsStTuUvVwWxXyYzZ0123456789";
        return new string(Enumerable.Repeat(chars, length).Select(s => s[Random.Shared.Next(s.Length)]).ToArray());
    }
}

public static class ClaimsPrincipalExtensions
{
    public static string GetEmail(this ClaimsPrincipal claims)
    {
        var email = claims.Identities.ToList().FirstOrDefault()?.Claims
            .Where(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")
            .FirstOrDefault()?.Value;

        return email is null ? throw new ArgumentNullException(email) : email;
    }
}

public static class DbContextExtensions
{
    /// <summary>
    /// Checks if email already exists in DB
    /// </summary>
    /// <param name="email">Email to check</param>
    public static bool EmailExists(this ApplicationContext Db, string email)
    {
        var authInfoList = Db.Auth.Where(auth => auth.Email == email).ToList();

        if (authInfoList is null || authInfoList.Count == 0)
            return false;

        return true;
    }
}