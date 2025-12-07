using Mixi.MidiController;
namespace Mixi.Audio;

/**
 * Wraps and abstracts different audio packages depending on the operating system
 */
public class AudioManager {

    private IAudioWrapper audioWrapper;

    AudioManager() {
        // TODO: Conditionally standup a wrapper based on OS and available packages

        audioWrapper = new WirePlumberWrapper();
    }

    public static readonly AudioManager Instance = new();

    public static void SetVolume(string id, float volume) {
        var normalizedVolume = ((volume - 0) / AbstractMidiController.MaxAnalogValue) * 100;
        Instance.audioWrapper.SetVolume(id, normalizedVolume);
    }

    public static void SetMute(string id, bool mute) {
        Instance.audioWrapper.SetMute(id, mute);
    }

    /**
     * TODO: Need to make this solution detect platform and delegate out to appropriate wrapper.
     */
    public static List<MediaElement> GetMediaElements() {
        return WirePlumberWrapper.GetMediaElements();
    }
}
