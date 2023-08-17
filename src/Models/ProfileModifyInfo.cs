namespace Models;

public class ProfileModifyInfo
{
    public string? ProfileName { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? About { get; set; }

    public ProfileModifyInfo() { }

    public ProfileModifyInfo(string profileName,
                             string firstName = "",
                             string lastName = "",
                             string about = "") => 
    (ProfileName, FirstName, LastName, About) = (profileName, firstName, lastName, about);
}