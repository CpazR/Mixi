using Avalonia;
using Avalonia.Controls;
namespace Mixi.UI.MidiManager.MidiControl;

public static class MidiInputComboBoxHelper {

    public static readonly AttachedProperty<int> MidiInputIdProperty =
        AvaloniaProperty.RegisterAttached<object, ComboBox, int>("MidiInputId", -1);

    public static int GetMidiInputId(ComboBox element) => element.GetValue(MidiInputIdProperty);

    public static void SetMidiInputId(ComboBox element, int value) => element.SetValue(MidiInputIdProperty, value);
}
