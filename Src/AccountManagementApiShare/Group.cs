namespace AccountManagementApiShare;

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
    /// Retrieves all groups in the domain.
    /// </summary>
    /// <returns>An enumerable collection of <see cref="Group"/> objects.</returns>
    public static IEnumerable<Group> GetGroups()
    {
        using var context = new PrincipalContext(ContextType.Domain, domainName);
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

    /// <summary>
    /// Retrieves groups in the domain that match the specified name filter.
    /// </summary>
    /// <param name="filter">The name filter to apply when searching for groups.</param>
    /// <returns>An enumerable collection of <see cref="Group"/> objects.</returns>
    public static IEnumerable<Group> GetGroupsByName(string filter)
    {
        using var context = new PrincipalContext(ContextType.Domain, domainName);
        using var groupPrincipal = new GroupPrincipal(context) { Name = filter };
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

    /// <summary>
    /// Retrieves the members of the group.
    /// </summary>
    /// <returns>An enumerable collection of <see cref="User"/> objects representing the group members.</returns>
    public IEnumerable<User> GetGroupMembers()
    {
        using var context = new PrincipalContext(ContextType.Domain, domainName);
        using var groupPrincipal = GroupPrincipal.FindByIdentity(context, IdentityType.Sid, Sid);
        using var results = groupPrincipal.GetMembers();
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
