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
}
