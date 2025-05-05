using System.Net.NetworkInformation;

namespace AccountManagementApiShare;

internal static class Management
{
    internal static readonly string domainName; 
    internal static PrincipalContext context; 

    static Management()
    {
        domainName = IPGlobalProperties.GetIPGlobalProperties().DomainName; 
        context = new PrincipalContext(ContextType.Domain, domainName);
    }

    public static IEnumerable<User> GetUsers()
    {
        using UserPrincipal u = new UserPrincipal(context);
        using PrincipalSearcher search = new PrincipalSearcher(u);
        using var results = search.FindAll();
        foreach (Principal result in results)
        {
            if (result is UserPrincipal user) 
            { 
                yield return new User()
                {
                    Name = user.Name,
                    SamAccountName = user.SamAccountName,
                    UserPrincipalName = user.UserPrincipalName,
                    EmailAddress = user.EmailAddress,
                    DisplayName = user.DisplayName,
                    Description = user.Description,
                    LastLogon = user.LastLogon,
                    LastPasswordSet = user.LastPasswordSet,
                    IsAccountDisabled = user.IsAccountDisabled,
                };
            }
        }

    }
}
