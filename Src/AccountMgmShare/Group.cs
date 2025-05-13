namespace AccountMgm;

/// <summary>
/// Represents a group in the account management system.
/// </summary>
public class Group : BaseItem
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Group"/> class.
    /// </summary>
    /// <param name="item">The <see cref="GroupPrincipal"/> object representing the group.</param>
    internal Group(GroupPrincipal item) : base(item)
    {
        IsSecurityGroup = item.IsSecurityGroup;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Group"/> class with the specified name and security identifier (SID).
    /// </summary>
    /// <param name="name">The name of the group.</param>
    /// <param name="sid">The security identifier (SID) of the user.</param>
    public Group(string name, string sid) : base(name, sid)
    { }

    /// <summary>
    /// Finds and returns a group in the domain by the specified name.
    /// </summary>
    /// <param name="name">The name of the group to search for.</param>
    /// <returns>
    /// A <see cref="Group"/> object representing the found group, or <c>null</c> if no group with the specified name exists.
    /// </returns>
    public static Group? FindGroup(string name)
    {
        using var context = new PrincipalContext(ContextType.Domain, domainName);
        using var find = new GroupPrincipal(context) { Name = name };
        using var searcher = new PrincipalSearcher(find);
        using var item = searcher.FindOne() as GroupPrincipal;
        return item is null ? null : new Group(item);
    }

    /// <summary>
    /// Retrieves all groups in the domain.
    /// </summary>
    /// <returns>An enumerable collection of <see cref="Group"/> objects.</returns>
    public static IEnumerable<Group> FindGroups()
    {
        using var context = new PrincipalContext(ContextType.Domain, domainName);
        using var principal = new GroupPrincipal(context);
        using var searcher = new PrincipalSearcher(principal);
        using var results = searcher.FindAll();
        foreach (Principal result in results)
        {
            if (result is GroupPrincipal item)
            {
                yield return new Group(item);
            }
        }
    }

    /// <summary>
    /// Retrieves groups in the domain that match the specified filter criteria.
    /// </summary>
    /// <param name="displayName">The display name to filter groups by, or <c>null</c> to ignore this filter.</param>
    /// <param name="name">The name to filter groups by, or <c>null</c> to ignore this filter.</param>
    /// <param name="samAccountName">The SAM account name to filter groups by, or <c>null</c> to ignore this filter.</param>
    /// <param name="userPrincipalName">The user principal name (UPN) to filter groups by, or <c>null</c> to ignore this filter.</param>
    /// <param name="isSecurityGroup">A value indicating whether to filter groups by security group status, or <c>null</c> to ignore this filter.</param>
    /// <returns>
    /// An enumerable collection of <see cref="Group"/> objects that match the specified filter criteria.
    /// </returns>
    public static IEnumerable<Group> FindGroups(
        // BaseItem
        string? displayName = null,
        string? name = null,
        string? samAccountName = null,
        string? userPrincipalName = null,
        // Group
        bool? isSecurityGroup = null)
    {
        using var context = new PrincipalContext(ContextType.Domain, domainName);
        using var filter = new GroupPrincipal(context);
        if (displayName != null) filter.DisplayName = displayName;
        if (name != null) filter.Name = name;
        if (samAccountName != null) filter.SamAccountName = samAccountName;
        if (userPrincipalName != null) filter.UserPrincipalName = userPrincipalName;
        if (isSecurityGroup != null) filter.IsSecurityGroup = isSecurityGroup;
        using var searcher = new PrincipalSearcher(filter);
        using var results = searcher.FindAll();
        foreach (Principal result in results)
        {
            if (result is GroupPrincipal item)
            {
                yield return new Group(item);
            }
        }
    }

    /// <summary>
    /// Retrieves the members of the group.
    /// </summary>
    /// <returns>An enumerable collection of <see cref="User"/> objects representing the group members.</returns>
    public IEnumerable<User> GetMembers()
    {
        using var context = new PrincipalContext(ContextType.Domain, domainName);
        using var group = GroupPrincipal.FindByIdentity(context, IdentityType.Sid, Sid);
        using var results = group.GetMembers();
        foreach (var result in results)
        {
            if (result is UserPrincipal user)
            {
                yield return new User(user);
            }
        }
    }

    /// <summary>
    /// Adds a user to the group.
    /// </summary>
    /// <param name="user">The <see cref="User"/> to add to the group.</param>
    public void AddUser(User user)
    {
        using var context = new PrincipalContext(ContextType.Domain, domainName);
        using var group = GroupPrincipal.FindByIdentity(context, IdentityType.Sid, Sid);
        group.Members.Add(context, IdentityType.Sid, user.Sid);
        group.Save();
    }

    /// <summary>
    /// Removes a user from the group.
    /// </summary>
    /// <param name="user">The <see cref="User"/> to remove from the group.</param>
    public void RemoveUser(User user)
    {
        using var context = new PrincipalContext(ContextType.Domain, domainName);
        using var group = GroupPrincipal.FindByIdentity(context, IdentityType.Sid, Sid);
        group.Members.Remove(context, IdentityType.Sid, user.Sid);
        group.Save();
    }

    /// <summary>
    /// Gets a value indicating whether the group is a security group.
    /// </summary>
    public bool? IsSecurityGroup { get; }
}
