namespace ProxyEnabler;

public static class Proxy
{
    public static IProxy Create(string[] args)
    {
        switch (Environment.OSVersion.Platform)
        {
            case PlatformID.Win32S:
            case PlatformID.Win32Windows:
            case PlatformID.Win32NT:
            case PlatformID.WinCE:
                return new WinProxy(args[0], args[1]);
                break;
            case PlatformID.Unix:
            case PlatformID.MacOSX:
                return new MacProxy(args[0], args[1], args[2], args[3]);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}