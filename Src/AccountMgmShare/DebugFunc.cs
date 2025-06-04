using System.Diagnostics;

namespace AccountMgm;

public class DebugFunc : IDisposable
{
    private string name;

    public DebugFunc(string name)
    {
        this.name = name;
        Debug.WriteLine($"++ {name}");
        Debug.Indent();

    }

    public void Dispose()
    {
        Debug.Unindent();
        Debug.WriteLine($"-- {name}");
    }
}
