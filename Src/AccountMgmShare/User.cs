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

    /// <summary>
    /// Initializes a new instance of the <see cref="User"/> class.
    /// </summary>
    /// <param name="item">The <see cref="UserPrincipalExt"/> object representing the user.</param>
    internal User(UserPrincipalExt item) : this((UserPrincipal) item)
    {
        Title = item.Title;
        Department = item.Department;
        Company = item.Company;
        Street = item.Street;
        PostalCode = item.PostalCode;
        City = item.City;
        Country = item.Country;
        CountryAbbreviation = item.CountryAbbreviation;
        State = item.State;
        MobileTelephoneNumber = item.MobileTelephoneNumber;
        RoomNumber = item.RoomNumber;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="User"/> class with the specified name and security identifier (SID).
    /// </summary>
    /// <param name="name">The name of the user.</param>
    /// <param name="sid">The security identifier (SID) of the user.</param>
    public User(string name, string sid) : base(name, sid)
    {
        EmailAddress = string.Empty;
        EmployeeId = string.Empty;
        GivenName = string.Empty;
        MiddleName = string.Empty;
        Surname = string.Empty;
        VoiceTelephoneNumber = string.Empty;
    }

    /// <summary>
    /// Gets the current user from the domain context.
    /// </summary>
    /// <returns>
    /// A <see cref="User"/> object representing the currently logged-in user.
    /// </returns>
    public static User Current()
    {
        //using var context = new PrincipalContext(ContextType.Domain, domainName);
        using var item = UserPrincipal.Current;
        return new User(item);
    }

    /// <summary>
    /// Finds and returns a user in the domain by the specified name.
    /// </summary>
    /// <param name="name">The name of the user to search for.</param>
    /// <returns>
    /// A <see cref="User"/> object representing the found user, or <c>null</c> if no user with the specified name exists.
    /// </returns>
    public static User? FindUser(string name)
    {
        using var context = new PrincipalContext(ContextType.Domain, domainName);
        using var find = new UserPrincipalExt(context) { Name = name };
        using var searcher = new PrincipalSearcher(find);
        using var item = searcher.FindOne() as UserPrincipalExt;
        return item is null ? null : new User(item);
    }

    /// <summary>
    /// Retrieves all users in the domain.
    /// </summary>
    /// <returns>An enumerable collection of <see cref="User"/> objects.</returns>
    public static IEnumerable<User> FindUsers()
    {
        using var context = new PrincipalContext(ContextType.Domain, domainName);
        using var filter = new UserPrincipalExt(context);
        using var searcher = new PrincipalSearcher(filter);
        using var results = searcher.FindAll();
        foreach (Principal result in results)
        {
            if (result is UserPrincipalExt item)
            {
                yield return new User(item);
            }
        }
    }

    /// <summary>
    /// Retrieves users in the domain that match the specified filter criteria.
    /// </summary>
    /// <param name="displayName">The display name to filter users by, or <c>null</c> to ignore this filter.</param>
    /// <param name="name">The name to filter users by, or <c>null</c> to ignore this filter.</param>
    /// <param name="samAccountName">The SAM account name to filter users by, or <c>null</c> to ignore this filter.</param>
    /// <param name="userPrincipalName">The user principal name (UPN) to filter users by, or <c>null</c> to ignore this filter.</param>
    /// <param name="enabled">A value indicating whether to filter users by enabled status, or <c>null</c> to ignore this filter.</param>
    /// <param name="emailAddress">The email address to filter users by, or <c>null</c> to ignore this filter.</param>
    /// <param name="employeeId">The employee ID to filter users by, or <c>null</c> to ignore this filter.</param>
    /// <param name="givenName">The given name (first name) to filter users by, or <c>null</c> to ignore this filter.</param>
    /// <param name="middleName">The middle name to filter users by, or <c>null</c> to ignore this filter.</param>
    /// <param name="surname">The surname (last name) to filter users by, or <c>null</c> to ignore this filter.</param>
    /// <param name="voiceTelephoneNumber">The voice telephone number to filter users by, or <c>null</c> to ignore this filter.</param>
    /// <returns>
    /// An enumerable collection of <see cref="User"/> objects that match the specified filter criteria.
    /// </returns>
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
        using var filter = new UserPrincipalExt(context);
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
            if (result is UserPrincipalExt item)
            {
                yield return new User(item);
            }
        }
    }

    /// <summary>
    /// Gets a string containing detailed information about the user, including base account information and user-specific properties
    /// such as given name, middle name, surname, phone number, employee ID, and email address.
    /// </summary>
    public override string Info => $"{base.Info}\r\nGivenName: {GivenName}\r\nMiddleName: {MiddleName}\r\nSurname: {Surname}\r\nPhone: {VoiceTelephoneNumber}\r\nEmployeeId: {EmployeeId}\r\nEmailAddress: {EmailAddress}\r\nRoomNumber: {RoomNumber}";

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

    // extention

    public string? Title { get; }
    public string? Department { get; }

    public string? Company { get; }

    public string? Street { get; }
    public string? PostalCode { get; }
    public string? City { get; }
    public string? Country { get; }
    public string? CountryAbbreviation { get; }

    public string? State { get; }

    public string? MobileTelephoneNumber { get; }

    public string? RoomNumber { get; }
}
