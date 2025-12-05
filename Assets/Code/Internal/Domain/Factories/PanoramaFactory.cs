public class PanoramaFactory
{
    public Panorama CreateEmptyPanorama(string panoramaId, string panoramaName)
    {
        return new Panorama(panoramaId, panoramaName);
    }
}
