using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Commons.Music.Midi;
namespace PipeWireMidi.UI.MidiManager;

public partial class MidiManagerWindow : Window {

    private List<IMidiPortDetails> portDetails;

    public MidiManagerWindow() {
        AvaloniaXamlLoader.Load(this);
        var midiAccess = MidiAccessManager.Default;
        portDetails = midiAccess.Inputs.ToList();
        
        var midiDeviceDropdown = this.Find<ComboBox>("MidiDevices");
        midiDeviceDropdown?.ItemsSource = midiAccess.Inputs;
    }

    private void CmdClose_OnClick(object? sender, RoutedEventArgs e) {
        Close();
    }
}
