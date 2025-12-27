using System.Runtime.InteropServices;
using System.Text;
namespace Mixi.Audio.FocusedWindow;

public class FocusedApplicationDetector {
    public static string GetFocusedApplicationName() {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
            return GetWindowsFocusedApp();
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
            return GetLinuxFocusedApp();
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) {
            return GetMacFocusedApp();
        }

        throw new PlatformNotSupportedException("Unsupported operating system");
    }

    private static string GetWindowsFocusedApp() {
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        IntPtr handle = GetForegroundWindow();
        StringBuilder sb = new StringBuilder(256);
        GetWindowText(handle, sb, 256);
        return sb.ToString();
    }

    private static string GetLinuxFocusedApp() {
        // Implement using X11 or Wayland APIs
        // Requires additional libraries like X11.NET
        var details = LinuxFocusedApplicationDetector.GetFocusedApplicationName();
        Console.WriteLine(details);
        return details;
    }

    private static string GetMacFocusedApp() {
        // Implement using macOS-specific APIs
        throw new NotImplementedException("macOS focused app detection not implemented");
    }
}
