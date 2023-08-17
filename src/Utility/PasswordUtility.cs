using System.Security.Cryptography;
using Models;

namespace Utility;
public static partial class PasswordUtility
{
    private static readonly RandomNumberGenerator _provider = RandomNumberGenerator.Create();
    private static readonly int _saltSize = 16;
    private static readonly int _hashSize = 20;
    private static readonly int _iterations = 10000;
    private static readonly int _minPasswordLength = 8;

    public static string HashPassword(string password)
    {
        byte[] salt;
        _provider.GetBytes(salt = new byte[_saltSize]);
        var key = new Rfc2898DeriveBytes(password, salt, _iterations, HashAlgorithmName.SHA256);
        var hash = key.GetBytes(_hashSize);

        var hashBytes = new byte[_saltSize + _hashSize];
        Array.Copy(salt, 0, hashBytes, 0, _saltSize);
        Array.Copy(hash, 0, hashBytes, _saltSize, _hashSize);

        var base64String = Convert.ToBase64String(hashBytes);
        return base64String;
    }

    public static bool PasswordVerified(string password, string base64Password)
    {
        var hashBytes = Convert.FromBase64String(base64Password);

        var salt = new byte[_saltSize];
        Array.Copy(hashBytes, 0, salt, 0, _saltSize);

        var key = new Rfc2898DeriveBytes(password, salt, _iterations, HashAlgorithmName.SHA256);
        byte[] hash = key.GetBytes(_hashSize);

        for (var i = 0; i < _hashSize; i++)
        {
            if (hashBytes[i + _saltSize] != hash[i])
                return false;
        }

        return true;
    }

    public static (bool IsStrong, List<ErrorInfo> StrengthIssues) IsPasswordStrong(string password)
    {
        List<ErrorInfo> strengthIssues = new ();
        if (password.Length < _minPasswordLength)
            strengthIssues.Add(new ErrorInfo($@"Minimal password length is {_minPasswordLength}"));
        if (!CompiledRegex.ContainsLetter().IsMatch(password))
            strengthIssues.Add(new ErrorInfo($@"Password must contain characters"));
        if (!CompiledRegex.ContainsNumber().IsMatch(password))
            strengthIssues.Add(new ErrorInfo($@"Password must contain numbers"));
        if (!CompiledRegex.ContainsSpecialCharacter().IsMatch(password))
            strengthIssues.Add(new ErrorInfo($@"Password must contain special characters"));
        return (strengthIssues.Count == 0, strengthIssues);
    }
}