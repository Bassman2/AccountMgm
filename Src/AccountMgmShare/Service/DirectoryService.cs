namespace AccountMgm.Service;

public class DirectoryService : WorkerThread, IDisposable
{
    protected static readonly string domainName = IPGlobalProperties.GetIPGlobalProperties().DomainName;
    protected PrincipalContext context;
    private bool disposed;


    public DirectoryService()
    {
        context = Invoke(() => new PrincipalContext(ContextType.Domain, domainName));
    }

    public override void Dispose()
    {
        if (!disposed)
        {
            Invoke(() =>
            {
                context?.Dispose();
            });
            disposed = true;
        }
        base.Dispose();
    }

    public IEnumerable<User> FindUsers(
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
        return InvokeEnumerable(() => FindUsersInt(
            // BaseItem
            displayName,
            name,
            samAccountName,
            userPrincipalName,
            // Authenticable
            enabled,
            // User
            emailAddress,
            employeeId,
            givenName,
            middleName,
            surname,
            voiceTelephoneNumber));
    }

    private IEnumerable<User> FindUsersInt(
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
        using var filter = new UserPrincipalExt(context);
        if (displayName != null) filter.DisplayName = displayName;
        if (name != null) filter.Name = name;
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
}


