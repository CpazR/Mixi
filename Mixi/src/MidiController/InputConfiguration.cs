using Mixi.Audio;
using Mixi.Audio.FocusedWindow;
namespace Mixi.MidiController;

public record InputConfiguration(int Id, InputDefinition Definition, MediaElement Media) {

    public const int FOCUSED_APPLICATION_INDEX = -1;

    public void Invoke(float value) {
        var action = Definition.ActionType;
        switch (action) {
            case InputActionType.VOLUME:

                // Select negative indices are proprietary functions
                var mediaIntId = int.Parse(Media.Id);
                if (mediaIntId < 0) {
                    switch (mediaIntId) {
                        case FOCUSED_APPLICATION_INDEX:
                            var focusedApplication = FocusedApplicationDetector.GetFocusedApplicationName();
                            AudioManager.SetApplicationVolume(focusedApplication, value);
                            break;
                    }
                }
                else {
                    AudioManager.SetVolume(Media.Id, value);
                }

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
