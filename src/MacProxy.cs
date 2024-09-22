using System.Diagnostics;

namespace ProxyEnabler;

public class MacProxy : IProxy
{
    private readonly string _host;
    private readonly string _port;
    private readonly string _username;
    private readonly string _password;

    public MacProxy(string host, string port, string username, string password)
    {
        _host = host;
        _port = port;
        _username = username;
        _password = password;
    }
    
    public void Enable()
    {
        string commandHttpsFormat = "networksetup -setsecurewebproxy Wi-Fi {0} {1} on {2} {3}";
        string commandHttps = string.Format(commandHttpsFormat, _host, _port, _username, _password);
        string commandHttpsOn = "networksetup –setsecurewebproxystate Wi-Fi on";

        ExecuteBashCommand(commandHttps, commandHttpsOn);
    }

    public void Disable()
    {
        string commandHttps = "networksetup -setsecurewebproxystate Wi-Fi off";
        
        ExecuteBashCommand(commandHttps);
    }

    private void ExecuteBashCommand(params string[] commands)
    {
        string allCommands = commands[0];
        for (int i = 1; i < commands.Length; i++) 
            allCommands += $" && {commands[i]}";
        
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "/bin/bash",
                Arguments = "-c \""+ allCommands + "\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            }
        };

        process.Start();
    }
}