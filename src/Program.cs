namespace ProxyEnabler;

internal class Program
{
    private static IProxy _proxy;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="args">
    /// 0 - host
    /// 1 - port
    /// 2 - username (can be empty)
    /// 3 - password (can be empty)
    /// </param>
    private static void Main(string[] args)
    {
        Console.WriteLine("*************PROXY ENABLER*************\n");
        
        AppDomain.CurrentDomain.ProcessExit += OnExit;

        try
        {
            _proxy = Proxy.Create(args);
            _proxy.Enable();
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