using System.Collections.Generic;

public class Tour
{
    public List<Panorama> Panoramas { get; private set; }

    public Tour(Panorama[] panoramas)
    {
        Panoramas = new List<Panorama>(panoramas);
    }

    public void AddPanorama(Panorama panorama)
    {
        Panoramas.Add(panorama);
    }
}
