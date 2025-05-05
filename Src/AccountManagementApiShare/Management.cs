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

    public static IEnumerable<Group> GetGroups()
    {
        using var groupPrincipal = new GroupPrincipal(context);
        using var searcher = new PrincipalSearcher(groupPrincipal);
        using var results = searcher.FindAll();
        foreach (Principal result in results)
        {
            if (result is GroupPrincipal item)
            {
                yield return new Group(item);
            }
        }

    }

    public static IEnumerable<User> GetGroupMembers(Group group)
    {
        using var groupPrincipal = GroupPrincipal.FindByIdentity(context, IdentityType.Sid, group.Sid);
        using var results = groupPrincipal.GetMembers();
        foreach (var result in results)
        {
            if (result is UserPrincipal user)
            {
                yield return new User(user);
            }
        }
    }

    public static void AddUserToGroup(Group group, User user)
    {
        using var groupPrincipal = GroupPrincipal.FindByIdentity(context, IdentityType.Sid, group.Sid);
        //using var userPrincipal = UserPrincipal.FindByIdentity(context, IdentityType.Sid, user.Sid);
        //groupPrincipal.Members.Add(userPrincipal);

        groupPrincipal.Members.Add(context, IdentityType.Sid, user.Sid);
        
    }

    public static void RemoveUserFromGroup(Group group, User user)
    {
        using var groupPrincipal = GroupPrincipal.FindByIdentity(context, IdentityType.Sid, group.Sid);
        //using var userPrincipal = UserPrincipal.FindByIdentity(context, IdentityType.Sid, user.Sid);
        //groupPrincipal.Members.Add(userPrincipal);

        groupPrincipal.Members.Remove(context, IdentityType.Sid, user.Sid);

    }
}
