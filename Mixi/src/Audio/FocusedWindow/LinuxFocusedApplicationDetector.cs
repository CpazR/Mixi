using Mixi.Audio.Utils;
using System.Text.RegularExpressions;
namespace Mixi.Audio.FocusedWindow;

/**
 * The linux specific implementation of application focus detection
 */
class LinuxFocusedApplicationDetector {

    public static string? GetFocusedApplicationName() {
        var getWmClassCommand = "xdotool getwindowfocus";
        var winId = long.Parse(ShellUtils.ExecuteCommand(getWmClassCommand) ?? string.Empty);

        var re = new Regex("\"(.*?)\"");

        var getClassNameCommand = $"xprop WM_CLASS -id 0x{winId:X}";
        var match = re.Match(ShellUtils.ExecuteCommand(getClassNameCommand) ?? string.Empty);
        return match.Value;
    }
}
