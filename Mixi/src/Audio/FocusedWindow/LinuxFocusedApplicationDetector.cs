using Mixi.Audio.Utils;
namespace Mixi.Audio.FocusedWindow;

/**
 * The linux specific implementation of application focus detection
 */
class LinuxFocusedApplicationDetector {

    public static string? GetFocusedApplication() {
        var getWmClassCommand = "xdotool getwindowfocus";
        var getPidCommand = "xdotool getwindowpid";
        var winId = long.Parse(ShellUtils.ExecuteCommand(getWmClassCommand) ?? string.Empty);
        var pid = ShellUtils.ExecuteCommand($"{getPidCommand} {winId}")?.Replace("\n", "") ?? string.Empty;

        return pid;
    }
}
