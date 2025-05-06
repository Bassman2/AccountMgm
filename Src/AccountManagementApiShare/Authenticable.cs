using System;
using System.Collections.Generic;
using System.Text;

namespace AccountManagementApiShare;

public abstract class Authenticable : BaseItem
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Authenticable"/> class.
    /// </summary>
    /// <param name="item">The <see cref="Principal"/> object representing the directory service item.</param>
    internal Authenticable(AuthenticablePrincipal item) : base(item)
    {
        IsAccountEnabled = item.Enabled ?? false;
        LastLogon = item.LastLogon;
        LastPasswordSet = item.LastPasswordSet;
        PasswordNeverExpires = item.PasswordNeverExpires;
        PasswordNotRequired = item.PasswordNotRequired;
    }
    public bool IsAccountEnabled { get; }
    public DateTime? LastLogon { get; }
    public DateTime? LastPasswordSet { get; }
    public bool PasswordNeverExpires { get; }
    public bool PasswordNotRequired { get; }
}
{
}
