using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Extensions;
using Services;

namespace Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class FriendsController : ControllerBase
{
    private readonly FriendsService _friendsService;
    private readonly ProfileService _profileService;

    public FriendsController(FriendsService friendsService, ProfileService profileService)
    {
        _friendsService = friendsService;
        _profileService = profileService;
    }

    [HttpGet]
    public IResult Get()
    {
        var profileOrError = _profileService.Get(GetByProfile.Email, User.GetEmail());

        if (profileOrError.IsT1)
            return Results.NotFound(profileOrError.AsT1);

        var subscriptions = _friendsService.GetSubscriptions(profileOrError.AsT0);
        var subscribers = _friendsService.GetSubscribers(profileOrError.AsT0);
        var friends = _friendsService.GetFriends(profileOrError.AsT0);
        return Results.Ok(
            new {
                Subscriptions = subscriptions,
                Subscribers = subscribers,
                Friends = friends!,
            }
        );
    }

    [Route("{profileName}")]
    [HttpGet]
    [AllowAnonymous]
    public IResult Get(string profileName)
    {
        var profileOrError = _profileService.Get(GetByProfile.ProfileName, profileName);

        if (profileOrError.IsT1)
            return Results.NotFound(profileOrError.AsT1);

        var subscriptions = _friendsService.GetSubscriptions(profileOrError.AsT0);
        var subscribers = _friendsService.GetSubscribers(profileOrError.AsT0);
        var friends = _friendsService.GetFriends(profileOrError.AsT0);
        return Results.Ok(
            new {
                Subscriptions = subscriptions,
                Subscribers = subscribers,
                Friends = friends,
            }
        );
    }

    [Route("add/{profileName}")]
    [HttpPost]
    public IResult Subscribe(string profileName)
    {
        var subscribingProfileOrError =_profileService.Get(GetByProfile.Email, User.GetEmail());
        if (subscribingProfileOrError.IsT1)
            return Results.NotFound(subscribingProfileOrError.AsT1);
        var subscribedProfileOrError = _profileService.Get(GetByProfile.ProfileName, profileName);
        if (subscribedProfileOrError.IsT1)
            return Results.NotFound(subscribedProfileOrError.AsT1);
        var subscriptionOrFriendsOrError = _friendsService.Subscribe(subscribingProfileOrError.AsT0, subscribedProfileOrError.AsT0);
        
        if (subscriptionOrFriendsOrError.IsT1)
            return Results.NotFound(subscriptionOrFriendsOrError.AsT1);
        
        return subscriptionOrFriendsOrError.AsT0.Match(
            subscription => Results.Ok(
                new {
                    Subscription = subscription,
                }),
            friends => Results.Ok(
                new {
                    Friends = friends
                }
            ) 
        );
    }

    [Route("remove/{profileName}")]
    [HttpPost]
    public IResult Unsubscribe(string profileName)
    {
        var unsubscribingProfileOrError =_profileService.Get(GetByProfile.Email, User.GetEmail());
        if (unsubscribingProfileOrError.IsT1)
            return Results.NotFound(unsubscribingProfileOrError.AsT1);
        var unsubscribedProfileOrError = _profileService.Get(GetByProfile.ProfileName, profileName);
        if (unsubscribedProfileOrError.IsT1)
            return Results.NotFound(unsubscribedProfileOrError.AsT1);
        var subscriptionOrError = _friendsService.Unsubscribe(unsubscribingProfileOrError.AsT0, unsubscribedProfileOrError.AsT0);
        
        if (subscriptionOrError.IsT1)
            return Results.NotFound(subscriptionOrError.AsT1);
        
        return Results.Ok(subscriptionOrError.AsT0);
    }
}