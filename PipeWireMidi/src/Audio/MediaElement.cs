namespace PipeWireMidi;

public record MediaElement(string id, string name, bool isMuted, float volume) {

    public bool isMuted { get; set; } = isMuted;
}
