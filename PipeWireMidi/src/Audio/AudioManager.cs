using Fr.Wireplumber;
namespace PipeWireMidi;

/**
 * Wraps and abstracts different audio packages depending on the operating system 
 */
public class AudioManager {

    private IAudioWrapper audioWrapper;
    
    AudioManager() {
        // TODO: Conditionally standup a wrapper based on OS and available packages

        audioWrapper = new WirePlumberWrapper();
    }
    
    public static readonly AudioManager Instance = new AudioManager();

    public static void SetVolume(string id, float volume) {
        Instance.audioWrapper.SetVolume(id, volume);
    }

    public static void SetMute(string id, bool mute) {
        Instance.audioWrapper.SetMute(id, mute);
    }
}
