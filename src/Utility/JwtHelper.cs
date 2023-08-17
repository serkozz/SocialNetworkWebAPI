using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Serilog;
using Src;

static class JwtHelper
{
    public static readonly string JWTSecretKey;

    static JwtHelper()
    {
        var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
        var secretProvider = config.Providers.First();
        secretProvider.TryGet("jwt-secret-key", out JWTSecretKey!);
        ArgumentNullException.ThrowIfNull(JWTSecretKey);
    }

    /// <summary>
    /// Creates JWT used to authorize users in application
    /// </summary>
    /// <returns>Json Web Token string representation</returns>
    public static string CreateJWT(string email = "", string role = "", int expirationTimeInDays = 1)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(JWTSecretKey);
        var identity = new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, role),
        });
        var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = identity,
            Expires = DateTime.Now.AddDays(expirationTimeInDays),
            SigningCredentials = credentials
        };
        var token = jwtTokenHandler.CreateToken(tokenDescriptor);
        return jwtTokenHandler.WriteToken(token);
    }

    public static (bool Validated, ClaimsPrincipal? Claims, SecurityToken? SecurityToken) ValidateJWT(string jwtToken)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = GetDefaultValidationParameters();

            ClaimsPrincipal principal = tokenHandler.ValidateToken(jwtToken, validationParameters, out SecurityToken validatedToken);
            return (true, principal, validatedToken);
        }
        catch (Exception)
        {
            Log.Information("Invalid token");
            return (false, null, null);
        }
    }

    private static TokenValidationParameters GetDefaultValidationParameters()
    {
        return new TokenValidationParameters()
        {
            ValidateLifetime = true,
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidIssuer = "Sample",
            ValidAudience = "Sample",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTSecretKey))
        };
    }
}