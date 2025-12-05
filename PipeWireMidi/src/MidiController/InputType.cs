namespace PipeWireMidi.MidiController;

public enum InputType {
    /**
     * A binary input the steps between 0 to <see cref="AbstractMidiController.MaxAnalogValue"/> with a button press
     */
    BUTTON,
    /**
     * An analog input (slider, knob, etc) that can vary between 0 and <see cref="AbstractMidiController.MaxAnalogValue"/>
     */
    ANALOG
}

