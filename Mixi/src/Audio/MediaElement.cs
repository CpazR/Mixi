namespace Mixi.Audio;

public record MediaElement(string Id, string Name, bool IsMuted, float Volume) {

    public bool IsMuted { get; set; } = IsMuted;
}
