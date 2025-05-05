namespace AccountManagementApiShare;

public class BaseItem
{
    internal BaseItem(Principal item)
    {

        Description = item.Description;
        DisplayNme = item.DisplayName;
        DistinguishedName = item.DistinguishedName;
        Name = item.Name;
        SamAccountName = item.SamAccountName;
        Sid = item.Sid.Value;
    }
    public string Description { get; }
    public string DisplayNme { get; }
    public string DistinguishedName { get; }
    public string Name { get; }
    public string SamAccountName { get; }

    public string Sid { get; }
}
