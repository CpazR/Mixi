using Mixi.Audio;
namespace Mixi.MidiController;

public record InputConfiguration(int Id, InputDefinition Definition, MediaElement Media) {

    public void Invoke(float value) {
        var action = Definition.ActionType;
        switch (action) {
            case InputActionType.VOLUME:
                AudioManager.SetVolume(Media.Id, value);
                break;
            case InputActionType.MUTE:
                if ((int)value == AbstractMidiController.MaxAnalogValue) {
                    Media.IsMuted = !Media.IsMuted;
                    AudioManager.SetMute(Media.Id, Media.IsMuted);
                }
                break;
        }
    }
}
