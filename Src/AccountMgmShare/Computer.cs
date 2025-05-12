namespace AccountMgm;

/// <summary>
/// Represents a computer in the account management system.
/// </summary>
internal class Computer : Authenticable
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Computer"/> class.
    /// </summary>
    /// <param name="item">The <see cref="ComputerPrincipal"/> object representing the computer.</param>
    internal Computer(ComputerPrincipal item) : base(item)
    { }

    public Computer(string name, string sid) : base(name, sid)
    { }

    public static Computer? FindComputer(string name)
    {
        using var context = new PrincipalContext(ContextType.Domain, domainName);
        using var find = new ComputerPrincipal(context) { Name = name };
        using var searcher = new PrincipalSearcher(find);
        using var item = searcher.FindOne() as ComputerPrincipal;
        return item is null ? null : new Computer(item);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
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
    /// Retrieves computers in the domain whose names match the specified filter.
    /// </summary>
    /// <param name="filter">The name filter to apply when searching for computers.</param>
    /// <returns>An enumerable collection of <see cref="Computer"/> objects that match the filter.</returns>
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
