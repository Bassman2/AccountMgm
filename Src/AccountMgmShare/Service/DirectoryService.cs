using System.Diagnostics;
using System.Security.Cryptography;

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

    #region User

    public IEnumerable<User> FindUsersModel(
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
        return InvokeEnumerable(() => FindUsersModelInt(
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

    private IEnumerable<User> FindUsersModelInt(
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

    #endregion

    #region UserPrincipal

    public IEnumerable<UserPrincipal> FindUsers(
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
        using var df = new DebugFunc("FindUsers");

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

    private IEnumerable<UserPrincipal> FindUsersInt(
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
        using var df = new DebugFunc("FindUsersInt");

        using var filter = new UserPrincipal(context);
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
            if (result is UserPrincipal item)
            {
                yield return item;
            }
        }
    }

    #endregion

    #region UserPrincipalExt

    public IEnumerable<UserPrincipalExt> FindExtUsers(
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
        return InvokeEnumerable(() => FindExtUsersInt(
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

    private IEnumerable<UserPrincipalExt> FindExtUsersInt(
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
                yield return item;
            }
        }
    }

    #endregion


    public IEnumerable<GroupPrincipal> GetGroups(Principal principal)
    {
        return InvokeEnumerable(() => GetGroupsInt(principal));
    }

    private IEnumerable<GroupPrincipal> GetGroupsInt(Principal principal)
    {
        using var results = principal.GetGroups(context);
        foreach (var result in results)
        {
            if (result is GroupPrincipal group)
            {
                yield return group;
            }
            else
            {
                throw new Exception(result.DisplayName);
            }
        }
    }

    public IEnumerable<GroupPrincipal> GetGroups(string sid)
    {
        return InvokeEnumerable(() => GetGroupsInt(sid));
    }

    private IEnumerable<GroupPrincipal> GetGroupsInt(string sid)
    {
        using var principal = Principal.FindByIdentity(context, IdentityType.Sid, sid);
        using var results = principal.GetGroups(context);
        foreach (var result in results)
        {
            if (result is GroupPrincipal group)
            {
                yield return group;
            }
            else
            {
                throw new Exception(result.DisplayName);
            }
        }
    }

    public IEnumerable<Principal> GetMembersByGroupName(string groupName)
    {
        return InvokeEnumerable(() => GetMembersByGroupNameInt(groupName));
    }

    private IEnumerable<Principal> GetMembersByGroupNameInt(string groupName)
    {
        using var group = GroupPrincipal.FindByIdentity(context, IdentityType.Name, groupName);
        foreach (var member in group.Members)
        {
            yield return member;
        }
    }




    public void AddUserToGroup(UserPrincipal user, string groupName)
    {
        Invoke(() => AddUserToGroupInt(user, groupName));
    }

    private void AddUserToGroupInt(UserPrincipal user, string groupName)
    {
        using var group = GroupPrincipal.FindByIdentity(context, IdentityType.Name, groupName);
        group.Members.Add(user);
        group.Save();

    }

    public void AddUserToGroup(string userSid, string groupName)
    {
        Invoke(() => AddUserToGroupInt(userSid, groupName));
    }

    private void AddUserToGroupInt(string userSid, string groupName)
    {
        using var user = UserPrincipal.FindByIdentity(context, IdentityType.Sid, userSid);
        using var group = GroupPrincipal.FindByIdentity(context, IdentityType.Name, groupName);
        group.Members.Add(user);
        group.Save();

    }

    public void RemoveUserFromGroup(UserPrincipal user, string groupName)
    {
        Invoke(() => RemoveUserFromGroupint(user, groupName));
    }

    private void RemoveUserFromGroupint(UserPrincipal user, string groupName)
    {
        using var group = GroupPrincipal.FindByIdentity(context, IdentityType.Name, groupName);
        group.Members.Remove(user);
        group.Save();
    }
    public void RemoveUserFromGroup(string userSid, string groupName)
    {
        Invoke(() => RemoveUserFromGroupint(userSid, groupName));
    }

    private void RemoveUserFromGroupint(string userSid, string groupName)
    {
        using var user = UserPrincipal.FindByIdentity(context, IdentityType.Sid, userSid);
        using var group = GroupPrincipal.FindByIdentity(context, IdentityType.Name, groupName);
        group.Members.Remove(user);
        group.Save();
    }
}


