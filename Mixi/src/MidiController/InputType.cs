namespace Mixi.MidiController;

public enum InputType {
    /**
     * A binary input that instantly steps between 0 to <see cref="AbstractMidiController.MaxAnalogValue"/> with a button press
     */
    BUTTON,
    /**
     * An analog input that can vary between 0 and <see cref="AbstractMidiController.MaxAnalogValue"/>
     */
    SLIDER,
    /**
     * An analog input that can vary between 0 and <see cref="AbstractMidiController.MaxAnalogValue"/>
     */
    KNOB
}
