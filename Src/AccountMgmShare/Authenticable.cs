namespace AccountMgm;

/// <summary>
/// Represents an authenticable directory service item, providing properties related to authentication and account management.
/// </summary>
public abstract class Authenticable : BaseItem
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Authenticable"/> class.
    /// </summary>
    /// <param name="item">The <see cref="AuthenticablePrincipal"/> object representing the directory service item.</param>
    internal Authenticable(AuthenticablePrincipal item) : base(item)
    {
        AccountExpirationDate = item.AccountExpirationDate;
        BadLogonCount = item.BadLogonCount;
        HomeDirectory = item.HomeDirectory;
        Enabled = item.Enabled ?? false;
        LastLogon = item.LastLogon;
        LastPasswordSet = item.LastPasswordSet;
        PasswordNeverExpires = item.PasswordNeverExpires;
        PasswordNotRequired = item.PasswordNotRequired;
        ScriptPath = item.ScriptPath;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Authenticable"/> class with the specified name and SID.
    /// </summary>
    /// <param name="name">The name of the authenticable item.</param>
    /// <param name="sid">The security identifier (SID) of the authenticable item.</param>
    public Authenticable(string name, string sid) : base(name, sid)
    {
        HomeDirectory = string.Empty;
        ScriptPath = string.Empty;
    }

    /// <summary>
    /// Gets a string containing information about the authenticable item.
    /// </summary>
    public override string Info => $"{base.Info}\r\nEnabled: {Enabled}";

    /// <summary>
    /// Gets the account expiration date of the directory service item.
    /// </summary>
    public DateTime? AccountExpirationDate { get; }

    /// <summary>
    /// Gets the number of failed logon attempts for the directory service item.
    /// </summary>
    public int BadLogonCount { get; }

    /// <summary>
    /// Gets the home directory path of the directory service item.
    /// </summary>
    public string HomeDirectory { get; }

    /// <summary>
    /// Gets a value indicating whether the account is enabled.
    /// </summary>
    public bool Enabled { get; }

    /// <summary>
    /// Gets the date and time of the last logon for the directory service item.
    /// </summary>
    public DateTime? LastLogon { get; }

    /// <summary>
    /// Gets the date and time when the password was last set for the directory service item.
    /// </summary>
    public DateTime? LastPasswordSet { get; }

    /// <summary>
    /// Gets a value indicating whether the password for the directory service item never expires.
    /// </summary>
    public bool PasswordNeverExpires { get; }

    /// <summary>
    /// Gets a value indicating whether a password is not required for the directory service item.
    /// </summary>
    public bool PasswordNotRequired { get; }

    /// <summary>
    /// Gets the script path associated with the directory service item.
    /// </summary>
    public string ScriptPath { get; }
}
