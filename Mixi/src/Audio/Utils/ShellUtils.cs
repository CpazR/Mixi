using System.Diagnostics;
namespace Mixi.Audio.Utils;

public class ShellUtils {
    public static string? ExecuteCommand(string command) {
        var processInfo = new ProcessStartInfo("bash", "-c \"" + command + "\"") {
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        var process = Process.Start(processInfo);
        var reader = process?.StandardOutput;
        return reader?.ReadToEnd();
    }
}
