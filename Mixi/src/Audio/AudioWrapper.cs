namespace Mixi.Audio;

/**
 * An interface that abstracts the OS audio layer
 */
public interface IAudioWrapper {

    public void SetVolume(string id, float volume);
    
    public void SetApplicationVolume(string applicationName, float volume);

    public void SetMute(string id, bool mute);
}
