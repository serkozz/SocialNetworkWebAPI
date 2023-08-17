using Microsoft.AspNetCore.Mvc;
using Services;
using Models;

namespace Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [Route("create_everlasting_token")]
    [HttpPost]
    public IResult CreateEverlastingToken([FromBody] Auth authInfo)
    {
        var JWT = JwtHelper.CreateJWT(authInfo.Email, authInfo.IsAdmin ? "Admin" : "User", expirationTimeInDays: 360);

        return Results.Ok(new {
            authInfo.Email,
            Role = authInfo.IsAdmin ? "Admin" : "User",
            TokenType = "Everlasting",
            JsonWebToken = JWT
        });
    }

    [Route("register")]
    [HttpPost]
    public IResult Register([FromBody] Auth authInfo)
    {
        var jwtOrError = _authService.Register(authInfo);

        if (jwtOrError.IsT1)
            return Results.NotFound(jwtOrError.AsT1);

        return Results.Ok(new {
            authInfo.Email,
            Registered = true,
            JsonWebToken = jwtOrError.AsT0
        });
    }

    [Route("login")]
    [HttpPost]
    public IResult Login([FromBody] Auth authInfo)
    {
        var jwtOrError = _authService.Login(authInfo);

        if (jwtOrError.IsT1)
            return Results.NotFound(jwtOrError.AsT1);
            
        return Results.Ok(new {
            authInfo.Email,
            LoggedIn = true,
            JsonWebToken = jwtOrError.AsT0
        });
    }
}