namespace ProxyEnabler;

internal class Program
{
    private static Proxy _proxy;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="args">
    /// 0 - host:port
    /// </param>
    private static void Main(string[] args)
    {
        Console.WriteLine("*************PROXY ENABLER*************\n");
        
        AppDomain.CurrentDomain.ProcessExit += OnExit;

        try
        {
            _proxy = new Proxy();
            _proxy.Enable(args[0]);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }

    private static void OnExit(object? sender, EventArgs e)
    {
        _proxy.Disable();
    }
}