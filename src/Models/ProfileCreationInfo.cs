namespace Models;

public class ProfileCreationInfo
{
    public required string ProfileName { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public ProfileCreationInfo(string profileName,
                               string firstName,
                               string lastName) => 
    (ProfileName, FirstName, LastName)
    = (profileName, firstName, lastName);
}