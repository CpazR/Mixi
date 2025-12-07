using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Commons.Music.Midi;
using Mixi.Audio;
using Mixi.MidiController;
using Mixi.UI.MidiManager.MidiControl;
using NLog;
using NLog.Targets;
namespace Mixi.UI.MidiManager;

public partial class MidiManagerWindow : Window {

    private static readonly Logger Logger = BuildLogger();

    private static Logger BuildLogger() {
        // TODO: Figure out why config file isn't recognized here 
        return new LogFactory().Setup().LoadConfiguration(builder => {
                var logconsole = new ConsoleTarget("logconsole");
                builder.Configuration.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);
            })
            .GetCurrentClassLogger();
    }

    private List<IMidiPortDetails> portDetails;

    private AbstractMidiController midiController;

    public MidiControl.MidiControl? midiControlPanel;

    public MidiManagerWindow() {
        AvaloniaXamlLoader.Load(this);
        var midiAccess = MidiAccessManager.Default;
        portDetails = midiAccess.Inputs.ToList();

        var midiDeviceDropdown = this.Find<ComboBox>("MidiDevices");
        midiDeviceDropdown?.SelectionChanged += SelectedMidiDevice;
        midiDeviceDropdown?.ItemsSource = midiAccess.Inputs;
    }

    private void SelectedMidiDevice(object? sender, SelectionChangedEventArgs e) {
        if (e.AddedItems[0] is not IMidiPortDetails portDetail) {
            Logger.Error("Valid midi device not found: " + e.AddedItems[0]);
            return;
        }

        Logger.Info("Selected MIDI Device: " + portDetail.Name);
        midiController = MixiMidiManager.GetMidiController(portDetail);
        var mediaElements = AudioManager.GetMediaElements();

        midiControlPanel = new KorgNanoKontrolControl(midiController, mediaElements);
        var controlPanel = this.Find<Panel>("MidiControlPanelContainer");

        if (controlPanel != null) {
            controlPanel.Children.Add(midiControlPanel);
            Logger.Info("Midi control panel loaded");
        }
        else {
            Logger.Error("Midi control panel could not be loaded");
        }
    }

    private void CmdClose_OnClick(object? sender, RoutedEventArgs e) {
        Close();
    }
}
