using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml;
using Mixi.Audio;
using Mixi.MidiController;
namespace Mixi.UI.MidiManager.MidiControl;

/**
 * The UI interface for a KorgNanoKontrol2
 */
public partial class KorgNanoKontrolControl : MidiControl {

    private KorgNanoKontrolController? midiController;

    private List<MediaElement> elements;

    public KorgNanoKontrolControl(AbstractMidiController midiController, List<MediaElement> elements) {
        if (midiController is not KorgNanoKontrolController korgNanoKontrolController) {
            Logger.Error("Invalid midi controller passed to midi user control");
            return;
        }
        this.midiController = korgNanoKontrolController;
        this.elements = elements;
        AvaloniaXamlLoader.Load(this);

        InitDropdowns();
    }

    private void InitDropdowns() {
        var dropdownGrid = this.Find<Grid>("DropdownGrid");
        var dropdownCounter = 0;

        // Fetch analog inputs. These will be used for the dropdowns 
        var sliderInputs = midiController.Definitions
            .Where(pair => pair.Value.Type.Equals(InputType.SLIDER))
            .Select(pair => AddComboBoxInput(pair, ref dropdownCounter));

        foreach (var sliderInput in sliderInputs) {
            dropdownGrid?.Children.Add(sliderInput);
            dropdownGrid?.RowDefinitions.Add(new RowDefinition {
                Height = GridLength.Auto
            });
        }

        var knobInputs = midiController.Definitions
            .Where(pair => pair.Value.Type.Equals(InputType.KNOB))
            .Select(pair => AddComboBoxInput(pair, ref dropdownCounter));
        foreach (var knobInput in knobInputs) {
            dropdownGrid?.Children.Add(knobInput);
            dropdownGrid?.RowDefinitions.Add(new RowDefinition {
                Height = GridLength.Auto
            });
        }


    }

    private ComboBox AddComboBoxInput(KeyValuePair<int, InputDefinition> pair, ref int counter) {
        var inputDropdown = new ComboBox {
            ItemsSource = elements,
            [MidiInputComboBoxHelper.MidiInputIdProperty] = pair.Key
        };
        inputDropdown.SelectionChanged += SelectedMedia;

        inputDropdown.SetValue(Grid.ColumnProperty, 0);
        inputDropdown.SetValue(Grid.RowProperty, counter++);

        var dataTemplate = new FuncDataTemplate<MediaElement>((element, scope) => new TextBlock {
            // IDE isn't catching it, but initially, before data is loaded, this can be null
            Text = element?.Name,
        });
        inputDropdown.ItemTemplate = dataTemplate;

        inputDropdown.PlaceholderText = $"Input {counter} -  {pair.Value.Type.ToString()}";
        return inputDropdown;
    }

    private void SelectedMedia(object? sender, SelectionChangedEventArgs e) {
        if (e.AddedItems[0] is not MediaElement selectedElement) {
            Logger.Error("Valid midi device not found: " + e.AddedItems[0]);
            return;
        }

        if (sender is not ComboBox inputDropdown) {
            Logger.Error("Invalid sender: " + sender);
            return;
        }
        var dropdownInputId = inputDropdown.GetValue(MidiInputComboBoxHelper.MidiInputIdProperty);

        midiController?.BindElement(dropdownInputId, selectedElement);
        Logger.Info("Selected media " + selectedElement.Name);
    }
}
