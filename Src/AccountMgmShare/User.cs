using System.Diagnostics;

namespace AccountMgm;

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

    public User(string name, string sid) : base(name, sid)
    {
        EmailAddress = string.Empty;
        EmployeeId = string.Empty;
        GivenName = string.Empty;
        MiddleName = string.Empty;
        Surname = string.Empty;
        VoiceTelephoneNumber = string.Empty;
    }

    public static User Current()
    {
        //using var context = new PrincipalContext(ContextType.Domain, domainName);
        using var item = UserPrincipal.Current;
        return new User(item);
    }

    public static User? FindUser(string name)
    {
        using var context = new PrincipalContext(ContextType.Domain, domainName);
        using var find = new ComputerPrincipal(context) { Name = name };
        using var searcher = new PrincipalSearcher(find);
        using var item = searcher.FindOne() as UserPrincipal;
        return item is null ? null : new User(item);
    }

    /// <summary>
    /// Retrieves all users in the domain.
    /// </summary>
    /// <returns>An enumerable collection of <see cref="User"/> objects.</returns>
    public static IEnumerable<User> FindUsers()
    {
        using var context = new PrincipalContext(ContextType.Domain, domainName);
        using var filter = new UserPrincipal(context);
        using var searcher = new PrincipalSearcher(filter);
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
    /// Retrieves users in the domain whose names match the specified filter.
    /// </summary>
    /// <param name="name">The name filter to apply when searching for users.</param>
    /// <returns>An enumerable collection of <see cref="User"/> objects that match the filter.</returns>
    public static IEnumerable<User> FindUsers(
        // BaseItem
        string? displayName = null,
        string? name = null,
        string? samAccountName = null,
        string? userPrincipalName = null,
        // Authenticable
        bool? enabled = null, 
        // User
        string? emailAddress = null,
        string? employeeId = null,
        string? givenName = null,
        string? middleName = null,
        string? surname = null,
        string? voiceTelephoneNumber = null)
    {
        using var context = new PrincipalContext(ContextType.Domain, domainName);
        using var filter = new UserPrincipal(context);
        if (displayName != null) filter.DisplayName = displayName;
        if (name != null)   filter.Name = name;
        if (samAccountName != null) filter.SamAccountName = samAccountName;
        if (userPrincipalName != null) filter.UserPrincipalName = userPrincipalName;
        if (enabled != null) filter.Enabled = enabled;
        if (emailAddress != null) filter.EmailAddress = emailAddress;
        if (employeeId != null) filter.EmployeeId = employeeId;
        if (givenName != null) filter.GivenName = givenName;
        if (middleName != null) filter.MiddleName = middleName;
        if (surname != null) filter.Surname = surname;
        if (voiceTelephoneNumber != null) filter.VoiceTelephoneNumber = voiceTelephoneNumber; 
        using var searcher = new PrincipalSearcher(filter);
        using var results = searcher.FindAll();
        foreach (Principal result in results)
        {
            if (result is UserPrincipal item)
            {
                yield return new User(item);
            }
        }
    }

    public override string Info => $"{base.Info}\r\nGivenName: {GivenName}\r\nMiddleName: {MiddleName}\r\nSurname: {Surname}\r\nPhone: {VoiceTelephoneNumber}\r\nEmployeeId: {EmployeeId}\r\nEmailAddress: {EmailAddress}";


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
