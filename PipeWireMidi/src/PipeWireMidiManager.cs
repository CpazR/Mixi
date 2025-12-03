using Commons.Music.Midi;
using NLog;
using NLog.Targets;
namespace PipeWireMidi;

class PipeWireMidiManager {
    
    protected static readonly Logger Logger = BuildLogger();
    
    private static Logger BuildLogger() {
        // TODO: Figure out why config file isn't recognized here 
        return new LogFactory().Setup().LoadConfiguration(builder => {
                var logconsole = new ConsoleTarget("logconsole"); 
                builder.Configuration.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);
            })
            .GetCurrentClassLogger();
    }

   static void Main(string[] args) {

        // Instantiate the MIDI input device
        var midiDeviceNumber = 16; // I think this is the client number from "aconnect -l"?

        var access = MidiAccessManager.Default;
        Logger.Debug(access.Inputs);
        Logger.Debug(access.Outputs);

        var midiPortDetail = access.Inputs.First(details => details.Id ==  midiDeviceNumber.ToString());

        var controller = new KorgNanoKontrolController(midiPortDetail);

        Logger.Info("Listening for MIDI input... Press any key to exit.");

        Console.ReadKey();
        controller.Close();
    }
}