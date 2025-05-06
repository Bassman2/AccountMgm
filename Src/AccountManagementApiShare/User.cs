namespace AccountManagementApiShare;

/// <summary>
/// Represents a user in the account management system.
/// </summary>
public class User : Authenticable
{
    /// <summary>
    /// Initializes a new instance of the <see cref="User"/> class.
    /// </summary>
    /// <param name="item">The <see cref="UserPrincipal"/> object representing the user.</param>
    internal User(UserPrincipal item) : base(item)
    {
        EmailAddress = item.EmailAddress;
        EmployeeId = item.EmployeeId;
        GivenName = item.GivenName;
        MiddleName = item.MiddleName;
        Surname = item.Surname;
        VoiceTelephoneNumber = item.VoiceTelephoneNumber;
    }

    /// <summary>
    /// Retrieves all users in the domain.
    /// </summary>
    /// <returns>An enumerable collection of <see cref="User"/> objects.</returns>
    public static IEnumerable<User> GetUsers()
    {
        using var context = new PrincipalContext(ContextType.Domain, domainName);
        using var userPrincipal = new UserPrincipal(context);
        using var searcher = new PrincipalSearcher(userPrincipal);
        using var results = searcher.FindAll();
        foreach (Principal result in results)
        {
            if (result is UserPrincipal item)
            {
                yield return new User(item);
            }
        }
    }

    /// <summary>
    /// Gets the email address of the user.
    /// </summary>
    public string EmailAddress { get; }

    /// <summary>
    /// Gets the employee ID of the user.
    /// </summary>
    public string EmployeeId { get; }

    /// <summary>
    /// Gets the given name (first name) of the user.
    /// </summary>
    public string GivenName { get; }

    /// <summary>
    /// Gets the middle name of the user.
    /// </summary>
    public string MiddleName { get; }

    /// <summary>
    /// Gets the surname (last name) of the user.
    /// </summary>
    public string Surname { get; }

    /// <summary>
    /// Gets the voice telephone number of the user.
    /// </summary>
    public string VoiceTelephoneNumber { get; }
}
