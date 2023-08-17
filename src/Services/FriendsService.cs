using Microsoft.EntityFrameworkCore;
using Models;
using OneOf;
using EF;

namespace Services;

public class FriendsService
{
    private ApplicationContext Db { get; }

    public FriendsService(ApplicationContext db)
    {
        Db = db;
    }

    public OneOf<OneOf<Subscription, Friends>, ErrorInfo> Subscribe(Profile subscribingProfile, Profile subscribedProfile)
    {
        if (subscribingProfile.Id == subscribedProfile.Id)
            return new ErrorInfo($@"Unable to follow myself. Profile Name: {subscribingProfile.ProfileName}");

        Subscription? subs = new()
        {
            SubscribingProfile = subscribingProfile,
            SubscribingId = subscribingProfile.Id,
            SubscribedProfile = subscribedProfile,
            SubscribedId = subscribedProfile.Id,
        };

        // Is first subscribed to second
        var (isFirstSubscribedToSecond, _) = IsSubscriber(subscribingProfile, subscribedProfile);

        // If it is, return error
        if (isFirstSubscribedToSecond)
            return new ErrorInfo($@"A profile named '{subscribingProfile.ProfileName}' is already a subscriber of a profile named '{subscribedProfile.ProfileName}'");

        // If not, than check if second is subscriber of first
        var (isSecondSubscribedToFirst, _) = IsSubscriber(subscribedProfile, subscribingProfile);

        // If first and second are each other subscribers, than make them friends
        if (isSecondSubscribedToFirst && !isFirstSubscribedToSecond)
        {
            var friendsOrError = MakeFriends(subscribingProfile, subscribedProfile);
            if (friendsOrError.IsT1)
                return friendsOrError.AsT1;
            return OneOf<Subscription, Friends>.FromT1(friendsOrError.AsT0);
        }

        // If they are not and they are not friends also, then make first subscriber of second
        var (areFriends, _) = AreFriends(subscribingProfile, subscribedProfile);

        if (areFriends)
            return new ErrorInfo($@"A profile named '{subscribingProfile.ProfileName}' is already a friend with a profile named '{subscribedProfile.ProfileName}'");

        Db.Subscriptions.Add(subs);
        Db.SaveChanges();
        return OneOf<Subscription, Friends>.FromT0(subs);
    }

    public OneOf<Friends, ErrorInfo> MakeFriends(Profile firstProfile, Profile secondProfile)
    {
        if (firstProfile.Id == secondProfile.Id)
            return new ErrorInfo($@"Unable to make friends with myself. Profile Name: {firstProfile.ProfileName}");

        Friends friends = new()
        {
            FirstProfile = firstProfile,
            FirstProfileId = firstProfile.Id,
            SecondProfile = secondProfile,
            SecondProfileId = secondProfile.Id
        };

        using var transaction = Db.Database.BeginTransaction();
        try
        {
            var (areFriends, _) = AreFriends(firstProfile, secondProfile);

            if (areFriends)
                return new ErrorInfo("Profile already friends");

            /// Update two tables using transaction for data sync
            transaction.CreateSavepoint("BeforeDelete");

            Db.Subscriptions.Where(s => (s.SubscribingId == firstProfile.Id
                && s.SubscribedId == secondProfile.Id)
                || (s.SubscribingId == secondProfile.Id
                && s.SubscribedId == firstProfile.Id)).ExecuteDelete();

            Db.Friends.Add(friends);

            transaction.Commit();
            Db.SaveChanges();
            return friends;
        }
        catch (Exception)
        {
            transaction.RollbackToSavepoint("BeforeDelete");
            return new ErrorInfo<string>("Database transaction error",
                additionalInfo: new List<string>() { $@"Causing method name: {nameof(FriendsService)}.{nameof(FriendsService.MakeFriends)}" });
        }
    }

    public OneOf<Subscription, ErrorInfo> Unsubscribe(Profile unsubscribingProfile, Profile unsubscribedProfile)
    {
        if (unsubscribingProfile.Id == unsubscribedProfile.Id)
            return new ErrorInfo($@"Unable to unfollow myself. Profile Name: {unsubscribingProfile.ProfileName}");

        // If they are friends make second profile be subscriber to first
        var (areFriends, Friends) = AreFriends(unsubscribingProfile, unsubscribedProfile);

        if (areFriends)
            return StopBeingFriends(unsubscribingProfile, unsubscribedProfile, Friends!);

        // If they not friends, than check if first are subscriber to second
        var (isFirstSubscribedToSecond, Subscription) = IsSubscriber(unsubscribingProfile, unsubscribedProfile);

        if (!isFirstSubscribedToSecond)
            return new ErrorInfo($@"A profile named '{unsubscribingProfile.ProfileName}' is not a subscriber of a profile named '{unsubscribedProfile.ProfileName}'");

        // If it is, than delete this subscription
        Db.Subscriptions.Remove(Subscription!);
        Db.SaveChanges();
        return OneOf<Subscription, ErrorInfo>.FromT0(Subscription!);
    }

    public OneOf<Subscription, ErrorInfo> StopBeingFriends(Profile unsubscribingProfile, Profile unsubscribedProfile, Friends friends)
    {
        if (unsubscribingProfile.Id == unsubscribedProfile.Id)
            return new ErrorInfo($@"Unable to stop being friends with myself. Profile Name: {unsubscribingProfile.ProfileName}");

        using var transaction = Db.Database.BeginTransaction();
        try
        {
            /// Update two tables using transaction for data sync
            transaction.CreateSavepoint("BeforeDelete");
            Db.Friends.Where(f => f.Id == friends.Id).ExecuteDelete();

            Subscription subscription = new Subscription()
            {
                SubscribingProfile = unsubscribedProfile,
                SubscribingId = unsubscribedProfile.Id,
                SubscribedProfile = unsubscribingProfile,
                SubscribedId = unsubscribingProfile.Id,
            };

            var createdSubscription = Db.Subscriptions.Add(subscription).Entity;

            transaction.Commit();
            Db.SaveChanges();
            return createdSubscription;
        }
        catch (Exception)
        {
            transaction.RollbackToSavepoint("BeforeDelete");
            return new ErrorInfo<string>("Database transaction error",
                additionalInfo: new List<string>() { $@"Causing method name: {nameof(FriendsService)}.{nameof(FriendsService.StopBeingFriends)}" });
        }
    }

    public (bool Success, Friends? Friends) AreFriends(Profile firstProfile, Profile secondProfile)
    {
        var existingFriendship = Db.Friends.Where
            (f => (f.FirstProfileId == firstProfile.Id
            && f.SecondProfileId == secondProfile.Id)
            || (f.FirstProfileId == secondProfile.Id
            && f.SecondProfileId == firstProfile.Id));

        if (existingFriendship is null || !existingFriendship.Any())
            return (false, null);

        if (existingFriendship.Count() > 1)
            throw new FriendshipDuplicateFoundException(existingFriendship);

        return (true, existingFriendship.First());
    }

    public (bool Success, Subscription? Subscription) IsSubscriber(Profile subscribingProfile, Profile subscribedProfile)
    {
        var existingSubscription = Db.Subscriptions.Where
            (s => s.SubscribingId == subscribingProfile.Id
            && s.SubscribedId == subscribedProfile.Id);

        if (existingSubscription is null || !existingSubscription.Any())
            return (false, null);

        if (existingSubscription.Count() > 1)
            throw new SubscriptionDuplicateFoundException(existingSubscription);

        return (true, existingSubscription.First());
    }


    public List<Subscription> GetSubscriptions(Profile profile) => Db.Subscriptions.Where(s => s.SubscribingProfile.Id == profile.Id).ToList();

    public List<Subscription> GetSubscribers(Profile profile) => Db.Subscriptions.Where(s => s.SubscribedProfile.Id == profile.Id).ToList();

    public List<Friends> GetFriends(Profile profile) => Db.Friends.Where(f => f.FirstProfileId == profile.Id || f.SecondProfileId == profile.Id).ToList();
}
