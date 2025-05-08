namespace AccountManagementApiShare;

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

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<Computer> GetComputers()
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
    public static IEnumerable<Computer> GetComputersByName(string filter)
    {
        using var context = new PrincipalContext(ContextType.Domain, domainName);
        using var principal = new ComputerPrincipal(context) { Name = filter };
        using var searcher = new PrincipalSearcher(principal);
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
