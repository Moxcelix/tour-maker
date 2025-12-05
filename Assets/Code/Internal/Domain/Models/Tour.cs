using System.Collections.Generic;

public class Tour
{
    public string Name { get; private set; }
    public List<Panorama> Panoramas { get; private set; }
    public List<Bridge> Bridges { get; private set; }

    public Tour(string name, Panorama[] panoramas, Bridge[] bridges)
    {
        Name = name;   
        Panoramas = new List<Panorama>(panoramas);
        Bridges = new List<Bridge>(bridges);
    }

    public void AddPanorama(Panorama panorama)
    {
        Panoramas.Add(panorama);
    }

    public Panorama GetPanorama(string panoramaId)
    {
        return Panoramas.Find(pano => pano.Id == panoramaId);
    }

    public void Link(string panorama1Id, string panorama2Id)
    {
        var pano1 = GetPanorama(panorama1Id);
        var pano2 = GetPanorama(panorama2Id);

        if(pano1 == null || pano2 == null)
        {
            throw new System.Exception("Both panoramas must exist in the tour");
        }


    }
}
