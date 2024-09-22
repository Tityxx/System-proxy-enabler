using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace ProxyEnabler;

public class WinProxy : IProxy
{
    [DllImport("wininet.dll")]
    public static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int dwBufferLength);

    private const string SUB_KEY_PATH = "Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings";
    private const string PROXY_ENABLE = "ProxyEnable";
    private const string PROXY_SERVER = "ProxyServer";
    private const int ENABLE = 1;
    private const int DISABLE = 0;

    private readonly string _host;
    private readonly string _port;

    private RegistryKey? _registry;
    
    public WinProxy(string host, string port)
    {
        _host = host;
        _port = port;
        _registry = Registry.CurrentUser.OpenSubKey(SUB_KEY_PATH, true);
    }

    public void Enable()
    {
        _registry.SetValue(PROXY_ENABLE, ENABLE);
        _registry.SetValue(PROXY_SERVER, $"{_host}:{_port}");
        LogEnable();
        UpdateInternetOptions();
    }

    public void Disable()
    {
        _registry.SetValue(PROXY_ENABLE, DISABLE);
        _registry.SetValue(PROXY_SERVER, DISABLE);
        LogDisable();
        _registry.Close();
        UpdateInternetOptions();
    }

    private static void UpdateInternetOptions()
    {
        int internetOptionSettingsChanged = 39;
        int internetOptionRefresh = 37;
        InternetSetOption(IntPtr.Zero, internetOptionSettingsChanged, IntPtr.Zero, 0);
        InternetSetOption(IntPtr.Zero, internetOptionRefresh, IntPtr.Zero, 0);
    }

    private void LogEnable()
    {
        bool isSuccess = (int) _registry.GetValue(PROXY_ENABLE, DISABLE) == ENABLE;
        Console.ForegroundColor = isSuccess ? ConsoleColor.Green : ConsoleColor.Red;
        
        if (isSuccess)
            Console.WriteLine("The proxy has been turned on.");
        else
            Console.WriteLine("Unable to enable the proxy.");
        Console.ForegroundColor = ConsoleColor.White;
    }

    private void LogDisable()
    {
        bool isSuccess = (int) _registry.GetValue(PROXY_ENABLE, ENABLE) == DISABLE;
        Console.ForegroundColor = isSuccess ? ConsoleColor.Green : ConsoleColor.Red;
        
        if (isSuccess)
            Console.WriteLine("The proxy has been turned off.");
        else
            Console.WriteLine("Unable to disable the proxy.");
        Console.ForegroundColor = ConsoleColor.White;
    }
}