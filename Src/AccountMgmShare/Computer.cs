namespace AccountMgm;

/// <summary>
/// Represents a computer in the account management system.
/// </summary>
public class Computer : Authenticable
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Computer"/> class.
    /// </summary>
    /// <param name="item">The <see cref="ComputerPrincipal"/> object representing the computer.</param>
    internal Computer(ComputerPrincipal item) : base(item)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Computer"/> class with the specified name and security identifier (SID).
    /// </summary>
    /// <param name="name">The name of the computer.</param>
    /// <param name="sid">The security identifier (SID) of the computer.</param>
    public Computer(string name, string sid) : base(name, sid)
    { }

    /// <summary>
    /// Finds and returns a computer in the domain by the specified name.
    /// </summary>
    /// <param name="name">The name of the computer to search for.</param>
    /// <returns>
    /// A <see cref="Computer"/> object representing the found computer, or <c>null</c> if no computer with the specified name exists.
    /// </returns>
    public static Computer? FindComputer(string name)
    {
        using var context = new PrincipalContext(ContextType.Domain, domainName);
        using var find = new ComputerPrincipal(context) { Name = name };
        using var searcher = new PrincipalSearcher(find);
        using var item = searcher.FindOne() as ComputerPrincipal;
        return item is null ? null : new Computer(item);
    }

    /// <summary>
    /// Retrieves all computers in the domain.
    /// </summary>
    /// <returns>
    /// An enumerable collection of <see cref="Computer"/> objects representing all computers in the domain.
    /// </returns>
    public static IEnumerable<Computer> FindComputers()
    {
        using var context = new PrincipalContext(ContextType.Domain, domainName);
        using var computerPrincipal = new ComputerPrincipal(context);
        using var searcher = new PrincipalSearcher(computerPrincipal);
        using var results = searcher.FindAll();
        foreach (Principal result in results)
        {
            if (result is ComputerPrincipal item)
            {
                yield return new Computer(item);
            }
        }
    }

    /// <summary>
    /// Retrieves computers in the domain that match the specified filter criteria.
    /// </summary>
    /// <param name="displayName">The display name to filter computers by, or <c>null</c> to ignore this filter.</param>
    /// <param name="name">The name to filter computers by, or <c>null</c> to ignore this filter.</param>
    /// <param name="samAccountName">The SAM account name to filter computers by, or <c>null</c> to ignore this filter.</param>
    /// <param name="userPrincipalName">The user principal name (UPN) to filter computers by, or <c>null</c> to ignore this filter.</param>
    /// <param name="enabled">A value indicating whether to filter computers by enabled status, or <c>null</c> to ignore this filter.</param>
    /// <returns>
    /// An enumerable collection of <see cref="Computer"/> objects that match the specified filter criteria.
    /// </returns>
    public static IEnumerable<Computer> FindComputers(
        // BaseItem
        string? displayName = null,
        string? name = null,
        string? samAccountName = null,
        string? userPrincipalName = null,
        // Authenticable
        bool? enabled = null)
    {
        using var context = new PrincipalContext(ContextType.Domain, domainName);
        using var filter = new ComputerPrincipal(context);
        if (displayName != null) filter.DisplayName = displayName;
        if (name != null) filter.Name = name;
        if (samAccountName != null) filter.SamAccountName = samAccountName;
        if (userPrincipalName != null) filter.UserPrincipalName = userPrincipalName;
        if (enabled != null) filter.Enabled = enabled;
        using var searcher = new PrincipalSearcher(filter);
        using var results = searcher.FindAll();
        foreach (Principal result in results)
        {
            if (result is ComputerPrincipal item)
            {
                yield return new Computer(item);
            }
        }
    }
}
