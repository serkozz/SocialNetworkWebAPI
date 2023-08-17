using Extensions;
using Utility;
using Models;
using OneOf;
using EF;

namespace Services;

public class AuthService
{
    private ApplicationContext Db { get; }

    public AuthService(ApplicationContext db)
    {
        Db = db;
    }

    /// <summary>
    /// Register user
    /// </summary>
    /// <param name="authInfo">User authentication info</param>
    public OneOf<string, ErrorInfo> Register(Auth authInfo)
    {
        if (!authInfo.Email.IsValidEmail())
            return new ErrorInfo($@"'{authInfo.Email}' has invalid email format");

        if (Db.EmailExists(authInfo.Email))
            return new ErrorInfo($@"User with email '{authInfo.Email}' already exists");

        var (IsStrong, StrengthIssues) = PasswordUtility.IsPasswordStrong(authInfo.Password);

        if (!IsStrong)
            return new ErrorInfo<ErrorInfo>("Password validation error", additionalInfo: StrengthIssues)!;

        var auth = InsertAuthInfo(authInfo);

        return JwtHelper.CreateJWT(email: auth.Email, role: auth.IsAdmin ? "Admin" : "User");
    }

    /// <summary>
    /// Login user
    /// </summary>
    /// <param name="authInfo">User authentication info</param>
    public OneOf<string, ErrorInfo> Login(Auth authInfo)
    {
        if (!authInfo.Email.IsValidEmail())
            return new ErrorInfo($@"'{authInfo.Email}' has invalid email format");

        var authOrError = GetAuthInfo(authInfo.Email);

        if (authOrError.IsT1)
            return authOrError.AsT1;

        if (!PasswordUtility.PasswordVerified(authInfo.Password, authOrError.AsT0.Password))
            return new ErrorInfo($@"Password does not match");

        return JwtHelper.CreateJWT(email: authOrError.AsT0.Email,
                role: authOrError.AsT0.IsAdmin ? "Admin" : "User");
    }

    /// <summary>
    /// Adds user authentication info to db
    /// </summary>
    /// <param name="authInfo">User authentication info</param>
    private Auth InsertAuthInfo(Auth authInfo)
    {
        authInfo.Password = PasswordUtility.HashPassword(authInfo.Password);
        Db.Auth.Add(authInfo);
        Db.SaveChanges();
        return authInfo;
    }

    /// <summary>
    /// Gets user authentication info from db
    /// </summary>
    /// <param name="email">Email of the requested user</param>
    private OneOf<Auth, ErrorInfo> GetAuthInfo(string email)
    {
        var authInfoList = Db.Auth.Where(auth => auth.Email == email).ToList();

        if (!authInfoList.Any())
            return new ErrorInfo($@"User with email '{email}' does not exists");

        if (authInfoList.Count > 1)
            throw new UserEmailDuplicateFoundException(email);

        return authInfoList.First();
    }
}