using System.Text;
using Models;

public class UserEmailDuplicateFoundException : Exception
{
    public UserEmailDuplicateFoundException(string email) : base($@"Multiple users with same email detected in {nameof(Auth)} table. Duplicate email: {email}"){ }
}

public class ProfileNameDuplicateFoundException : Exception
{
    public ProfileNameDuplicateFoundException(string profileName) : base($@"Multiple profiles with same name detected in {nameof(Profile)} table. Duplicate profileName: {profileName}"){ }
}

public class SubscriptionDuplicateFoundException : Exception
{
    public SubscriptionDuplicateFoundException(IEnumerable<Subscription> subs) : base(CreateMessage(subs)) { }

    private static string CreateMessage(IEnumerable<Subscription> subs)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($@"Multiple subscriptions with same subscribers detected in {nameof(Subscription)} table.{Environment.NewLine}{Environment.NewLine}Duplicate subscriptions");
        foreach (var sub in subs)
        {
            sb.AppendLine($@"Subscribing Profile Name: {sub.SubscribingProfile.ProfileName} --- Subscribed Profile Name: {sub.SubscribedProfile.ProfileName}");
        }
        return sb.ToString();
    }
}

public class FriendshipDuplicateFoundException : Exception
{
    public FriendshipDuplicateFoundException(IEnumerable<Friends> friends) : base(CreateMessage(friends)) { }

    private static string CreateMessage(IEnumerable<Friends> friendships)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($@"Multiple friendships with same friends detected in {nameof(Friends)} table.{Environment.NewLine}{Environment.NewLine}Duplicate Friendships");
        foreach (var friendship in friendships)
        {
            sb.AppendLine($@"First Profile Name: {friendship.FirstProfile.ProfileName} --- Second Profile Name: {friendship.SecondProfile.ProfileName}");
        }
        return sb.ToString();
    }
}