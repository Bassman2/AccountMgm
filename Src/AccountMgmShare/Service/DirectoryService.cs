namespace AccountMgm.Service;

public class DirectoryService : WorkerThread, IDisposable
{
    protected static readonly string domainName = IPGlobalProperties.GetIPGlobalProperties().DomainName;
    protected PrincipalContext? context;
    private bool disposed;


    public DirectoryService()
    {
        context = Invoke(() => new PrincipalContext(ContextType.Domain, domainName));
    }

    public override void Dispose()
    {
        if (!disposed)
        {
            Invoke(() =>
            {
                context?.Dispose();
                context = null;
            });
            disposed = true;
        }
        base.Dispose();
    }

}


