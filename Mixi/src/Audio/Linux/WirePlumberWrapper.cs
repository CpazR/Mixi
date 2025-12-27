using DynamicData.Kernel;
using Fr.Wireplumber;
using Fr.Wireplumber.Model.Objects;
using Mixi.Audio.Utils;
namespace Mixi.Audio;

/**
 * A wrapper for WirePlumber commands.
 *
 * Inspired by XVolume's many generic interfaces for linux.
 */
public class WirePlumberWrapper : IAudioWrapper {

    private static readonly string VolumeToken = "[vol:";

    private const string CommandStatus = "wpctl status";

    private Dictionary<ulong, MediaElement> mediaMappings = new();

    private List<Node> nodeList = [];

    private List<Client> clientList = [];

    public WirePlumberWrapper() {
        Wireplumber.Start();

        // Load media elements from node registry events. These IDs can be used to access the Nodes directly if needed later.
        Wireplumber.NodeRegistry.Added += node => {
            nodeList.Add(node);
            mediaMappings[node.ObjectId] = new MediaElement($"{node.ObjectId}", node.Name, false, 1f, MediaType.NA);
        };

        Wireplumber.NodeRegistry.Updated += (node, type) => {
            mediaMappings[node.ObjectId] = new MediaElement($"{node.ObjectId}", node.Name, false, 1f, MediaType.NA);
        };

        Wireplumber.NodeRegistry.Deleted += node => {
            nodeList.Remove(node);
            mediaMappings.Remove(node.ObjectId);
        };

        Wireplumber.ClientRegistry.Added += client => {
            clientList.Add(client);
        };

        Wireplumber.ClientRegistry.Deleted += client => {
            clientList.Remove(client);
        };
    }

    public static List<MediaElement> GetMediaElements() {

        // Execute the command to get the status of sinks, sources and devices
        var output = ShellUtils.ExecuteCommand(CommandStatus);

        var elements = ParseAudioElements(output);
        return elements;
    }

    private static List<MediaElement> ParseAudioElements(string output) {
        var elements = new List<MediaElement>();
        var lines = output.Split([
            Environment.NewLine
        ], StringSplitOptions.RemoveEmptyEntries);

        var currentSection = MediaType.None;

        foreach (var line in lines) {
            // Set the current section based on the line content
            // TODO: Likely not scalable. Don't know how other distro's handle this.
            if (line.StartsWith(" ├─ Devices:")) {
                currentSection = MediaType.Devices;
                continue;
            }
            else if (line.StartsWith(" ├─ Sinks:")) {
                currentSection = MediaType.Sinks;
                continue;
            }
            else if (line.StartsWith(" ├─ Sources:")) {
                currentSection = MediaType.Sources;
                continue;
            }
            else if (line.StartsWith(" ├─ Filters:")) {
                currentSection = MediaType.Filters;
                continue;
            }
            else if (line.StartsWith(" └─ Streams:")) {
                currentSection = MediaType.Streams;
                continue;
            }
            else if (line.StartsWith("Video") || line.StartsWith("Settings")) {
                currentSection = MediaType.NA;
            }
            else if (line.StartsWith(" ├─") || line.StartsWith(" └─")) continue; // Skip section headers

            // Parse based on current section
            switch (currentSection) {
                case MediaType.Devices:
                case MediaType.NA:
                    // TODO: Determine if access to either of these are even useful in this context.
                    break;
                case MediaType.Sinks:
                case MediaType.Sources:
                case MediaType.Filters:
                    ParseAudio(elements, line, currentSection);
                    break;
                case MediaType.Streams:
                    ParseStreams(elements, line, currentSection);
                    break;
            }
        }

        return elements;
    }
    private static void ParseStreams(List<MediaElement> elements, string line, MediaType type) {
        var parts = line.Split([' '], StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length >= 2) { // Only ID and name
            // Don't want to include the individual channels of streams.
            if (parts[1].StartsWith("output_F")) {
                return;
            }
            elements.Add(new MediaElement(parts[0].Replace(".", ""), // ID
                string.Join(" ", parts.Skip(1)), // Name
                false, // Default muted state
                1f,
                type)); // Default volume
        }
    }

    private static void ParseDevice(List<MediaElement> elements, string line, MediaType type) {
        var parts = line.Split([' '], StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length >= 3) {
            elements.Add(new MediaElement(parts[1].Replace(".", ""), // ID
                string.Join(" ", parts.Skip(2)), // Name
                false, // Default muted state
                1f,
                type)); // Default volume
        }
    }


    private static void ParseAudio(List<MediaElement> elements, string line, MediaType type) {
        var parts = line.Split([' '], StringSplitOptions.RemoveEmptyEntries).AsList();
        // Check for volume information
        if (parts.Count < 5)
            return;
        var isCurrentlyActive = parts[1] == "*";
        // Remove volume tokens. Can change between sessions.
        var volumeString = parts.Last().Replace("[vol:", "").Replace("]", "").Trim(); // Extract volume
        var volume = float.TryParse(volumeString, out var parsedVolume) ? parsedVolume : 1f; // Fallback to default
        var volumeIndex = parts.IndexOf(VolumeToken);
        // Remove twice. Should always be two whitespace separated volume tokens. Ex: [vol: 0.31]
        parts.RemoveAt(volumeIndex);
        parts.RemoveAt(volumeIndex);
        var skipId = isCurrentlyActive ? 3 : 2;
        var id = (isCurrentlyActive ? parts[2] : parts[1]).Replace(".", "");
        var name = string.Join(" ", parts.Skip(skipId)); // Name is all parts except the last two

        elements.Add(new MediaElement(id, name, false, volume, type));
    }


    public void SetVolume(string id, float volume) {
        var command = $"wpctl set-volume {id} {volume}%";
        ShellUtils.ExecuteCommand(command);
        Console.WriteLine($"Set volume of {id} to {volume}%");
    }

    public void SetApplicationVolume(string applicationName, float volume) {
        // Get the ID of the application's pipewire input

        var client = clientList.FirstOrOptional(clientNode => clientNode.Application.Name != null && clientNode.Application.Name.Equals(applicationName));
        if (!client.HasValue) {
            return;
        }

        var node = nodeList.FirstOrOptional(node => node.Client.Id == client.Value.ObjectId);
        if (!node.HasValue) {
            return;
        }

        var nodeId = node.Value.ObjectId;
        // Based on results, update the volume
        var volumeCommand = $"wpctl set-volume {nodeId} {volume}%";
        ShellUtils.ExecuteCommand(volumeCommand);
    }

    public void SetMute(string id, bool mute) {
        var isMuteExpression = mute ? "1" : "0";
        var command = $"wpctl set-mute {id} {isMuteExpression}";
        ShellUtils.ExecuteCommand(command);
        Console.WriteLine(mute ? $"Muted {id}" : $"Unmuted {id}");
    }

}
