public class PanoramaFactory
{
    public Panorama CreateEmptyPanorama(string panoramaId)
    {
        return new Panorama(panoramaId);
    }
}
