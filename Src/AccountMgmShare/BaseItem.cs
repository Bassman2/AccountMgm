using System.Net.Mail;

namespace AccountMgm;

/// <summary>
/// Represents a base class for directory service items, providing common properties and operations.
/// </summary>
public abstract class BaseItem
{
    private const BindingFlags ConstructorDefault = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance;

    /// <summary>
    /// The domain name of the current environment.
    /// </summary>
    protected static readonly string domainName = IPGlobalProperties.GetIPGlobalProperties().DomainName;

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseItem"/> class.
    /// </summary>
    /// <param name="item">The <see cref="Principal"/> object representing the directory service item.</param>
    internal BaseItem(Principal item)
    {
        Description = item.Description;
        DisplayName = item.DisplayName;
        DistinguishedName = item.DistinguishedName;
        Guid = item.Guid;
        Name = item.Name;
        SamAccountName = item.SamAccountName;
        Sid = item.Sid.Value;
        UserPrincipalName = item.UserPrincipalName;
    }

    public BaseItem(string name, string sid) 
    {
        Name = name;
        Sid = sid;
        Description = string.Empty;
        DisplayName = string.Empty;
        DistinguishedName = string.Empty;
        SamAccountName = string.Empty;
        UserPrincipalName = string.Empty;

    }

    public static IEnumerable<T> GetPrinciples<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicConstructors)] T, P>() where T : BaseItem, new() where P : Principal, new()
    {
        using var context = new PrincipalContext(ContextType.Domain, domainName);
        using var principal = new P();
        using var searcher = new PrincipalSearcher(principal);
        using var results = searcher.FindAll();
        foreach (Principal result in results)
        {
            if (result is P item)
            {
                yield return (T)Activator.CreateInstance(typeof(T), ConstructorDefault, null, [item], null)!;
            }
        }
    }

    public IEnumerable<Group> GetGroups()
    {
        using var context = new PrincipalContext(ContextType.Domain, domainName);
        using var item = Principal.FindByIdentity(context, IdentityType.Sid, Sid);
        using var results = item.GetGroups(context);
        foreach (var result in results)
        {
            if (result is GroupPrincipal group)
            {
                yield return new Group(group);
            }
        }
    }

    /// <summary>
    /// Deletes the directory service item.
    /// </summary>
    public void Delete()
    {
        using var context = new PrincipalContext(ContextType.Domain, domainName);
        using var principal = Principal.FindByIdentity(context, IdentityType.Sid, Sid);
        principal?.Delete();
    }

    /// <summary>
    /// Determines whether the current item is a member of the specified group.
    /// </summary>
    /// <param name="group">The <see cref="Group"/> to check membership against.</param>
    /// <returns><c>true</c> if the item is a member of the group; otherwise, <c>false</c>.</returns>
    public bool IsMemberOf(Group group)
    {
        using var context = new PrincipalContext(ContextType.Domain, domainName);
        using var principal = Principal.FindByIdentity(context, IdentityType.Sid, Sid);
        using var groupPrincipal = GroupPrincipal.FindByIdentity(context, IdentityType.Sid, group.Sid);
        return principal?.IsMemberOf(groupPrincipal) ?? false;
    }

    public virtual string Info => $"Name: {Name}\r\nDescription: {Description}\r\nSid: {Sid}\r\nDisplayName: {DisplayName}\r\nDistinguishedName: {DistinguishedName}\r\nSamAccountName: {SamAccountName}\r\nUserPrincipalName: {UserPrincipalName}";

    public override int GetHashCode() => Sid.GetHashCode();

    public override bool Equals(object? obj) => /*obj != null &&*/ obj is BaseItem item && Sid == item.Sid;

    /// <summary>
    /// Gets the description of the directory service item.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Gets the display name of the directory service item.
    /// </summary>
    public string DisplayName { get; }

    /// <summary>
    /// Gets the distinguished name of the directory service item.
    /// </summary>
    public string DistinguishedName { get; }

    /// <summary>
    /// Gets the globally unique identifier (GUID) of the directory service item.
    /// </summary>
    public Guid? Guid { get; }

    /// <summary>
    /// Gets the name of the directory service item.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the SAM account name of the directory service item.
    /// </summary>
    public string SamAccountName { get; }

    /// <summary>
    /// Gets the security identifier (SID) of the directory service item.
    /// </summary>
    public string Sid { get; }

    /// <summary>
    /// Gets the user principal name (UPN) of the directory service item.
    /// </summary>
    public string UserPrincipalName { get; }
}
