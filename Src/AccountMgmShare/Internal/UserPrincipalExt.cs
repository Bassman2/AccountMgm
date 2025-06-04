namespace AccountMgm;

[DirectoryObjectClass("user")]
[DirectoryRdnPrefix("CN")]
public class UserPrincipalExt : UserPrincipal
{
    public UserPrincipalExt(PrincipalContext context)
       : base(context)
    { }

    [DirectoryProperty("title")]
    public string? Title
    {
        get => GetString("title");
        set => SetString("title", value);
    }

    [DirectoryProperty("department")]
    public string? Department
    {
        get => GetString("department");
        set => SetString("department", value);
    }
    
    [DirectoryProperty("company")]
    public string? Company
    {
        get => GetString("company");
        set => SetString("company", value);
    }


    [DirectoryProperty("streetAddress")]
    public string? Street
    {
        get => GetString("streetAddress");
        set => SetString("streetAddress", value);
    }

    [DirectoryProperty("postalCode")]
    public string? PostalCode
    {
        get => GetString("postalCode");
        set => SetString("postalCode", value);
    }

    [DirectoryProperty("l")]
    public string? City
    {
        get => GetString("l");
        set => SetString("l", value);
    }

    [DirectoryProperty("co")]
    public string? Country
    {
        get => GetString("co");
        set => SetString("co", value);
    }

    [DirectoryProperty("c")]
    public string? CountryAbbreviation
    {
        get => GetString("c");
        set => SetString("c", value);
    }

    [DirectoryProperty("st")]
    public string? State
    {
        get => GetString("st");
        set => SetString("st", value);
    }

    [DirectoryProperty("mobile")]
    public string? MobileTelephoneNumber
    {
        get => GetString("mobile");
        set => SetString("mobile", value);
    }

    [DirectoryProperty("roomNumber")]
    public string? RoomNumber
    {
        get => GetString("roomNumber");
        set => SetString("roomNumber", value);
    }

    public new static UserPrincipalExt Current
    {
        get
        {
            UserPrincipal current = UserPrincipal.Current;
            return FindByIdentity(current.Context, IdentityType.Sid, current.Sid.Value);
        }
    }

    public new static UserPrincipalExt FindByIdentity(PrincipalContext context, string identityValue)
    {
        return (UserPrincipalExt)FindByIdentityWithType(context, typeof(UserPrincipalExt), identityValue);
    }

    public new static UserPrincipalExt FindByIdentity(PrincipalContext context, IdentityType identityType, string identityValue)
    {
        return (UserPrincipalExt)FindByIdentityWithType(context, typeof(UserPrincipalExt), identityType, identityValue);
    }

    private string? GetString(string name)
    {
        var res = (string?)ExtensionGet(name).SingleOrDefault();
        return res; //.Length != 1 ? null : (string)res[0];
    }

    private void SetString(string name, string? value)
    {
        this.ExtensionSet(name, value);
    }
}
