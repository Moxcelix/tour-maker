using System.Collections.Generic;
using System.Linq;

public class Tour
{
    public string Name { get; private set; }
    public List<Panorama> Panoramas { get; private set; }
    public List<Bridge> Bridges { get; private set; }

    private string defaultPanoramaId = null;

    public Tour(string name, Panorama[] panoramas, Bridge[] bridges)
    {
        Name = name;
        Panoramas = new List<Panorama>(panoramas);
        Bridges = new List<Bridge>(bridges);
    }

    public void AddPanorama(Panorama panorama)
    {
        if (Panoramas.Count == 0)
        {
            SetDeafultPanorama(panorama.Id);
        }

        Panoramas.Add(panorama);
    }

    public void SetDeafultPanorama(string panoramaId)
    {
        defaultPanoramaId = panoramaId; 
    }

    public Panorama GetDeafultPanorama()
    {
        return GetPanorama(defaultPanoramaId);
    }

    public Panorama GetPanorama(string panoramaId)
    {
        return Panoramas.Find(pano => pano.Id == panoramaId);
    }

    public Bridge Link(string panorama1Id, string panorama2Id)
    {
        var pano1 = GetPanorama(panorama1Id);
        var pano2 = GetPanorama(panorama2Id);

        if (pano1 == null || pano2 == null)
        {
            throw new System.Exception("Both panoramas must exist in the tour");
        }

        if (panorama1Id == panorama2Id)
        {
            throw new System.Exception("Can not link panorama with it self");
        }

        if (BridgeExists(panorama1Id, panorama2Id))
        {
            throw new System.Exception("Panoramas already connected");
        }

        var bridge = new Bridge(pano1, pano2);

        Bridges.Add(bridge);

        return bridge;
    }

    public bool BridgeExists(string panorama1Id, string panorama2Id)
    {
        return Bridges.Any(b =>
            (b.Panorama1.Id == panorama1Id && b.Panorama2.Id == panorama2Id) ||
            (b.Panorama1.Id == panorama2Id && b.Panorama2.Id == panorama1Id));
    }

    public List<Bridge> GetBridgesForPanorama(string panoramaId)
    {
        return Bridges.Where(b =>
            b.Panorama1.Id == panoramaId || b.Panorama2.Id == panoramaId).ToList();
    }

    public Panorama[] GetConnectedPanoramas(string panoramaId)
    {
        var connectedPanoramas = new List<Panorama>();

        foreach (var bridge in Bridges)
        {
            if (bridge.Panorama1.Id == panoramaId)
            {
                connectedPanoramas.Add(bridge.Panorama2);
            }
            else if (bridge.Panorama2.Id == panoramaId)
            {
                connectedPanoramas.Add(bridge.Panorama1);
            }
        }

        return connectedPanoramas.ToArray();
    }
}
