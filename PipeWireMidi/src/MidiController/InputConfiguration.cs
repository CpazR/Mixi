namespace PipeWireMidi.MidiController;

public record InputConfiguration(int id, InputType type, InputActionType action, MediaElement media) {
    
    public void Invoke(float value) {
        switch (action) {
            case InputActionType.VOLUME:
                AudioManager.SetVolume(media.id, value);
                break;
            case InputActionType.MUTE:
                if ((int)value == AbstractMidiController.MaxAnalogValue) {
                    media.isMuted = !media.isMuted;
                    AudioManager.SetMute(media.id, media.isMuted);
                }
                break;
        }
    }
}