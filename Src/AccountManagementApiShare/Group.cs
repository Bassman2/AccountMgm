namespace AccountManagementApiShare;

internal class Group : BaseItem
{
    internal Group(GroupPrincipal item) : base(item)
    {
        IsSecurityGroup = item.IsSecurityGroup;
    }

    public IEnumerable<User> Members()
    {

    }

    public bool? IsSecurityGroup { get; }

}
