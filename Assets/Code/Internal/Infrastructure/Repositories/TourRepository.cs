using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TourRepository : ITourRepository
{
    private const string saveFolder = "tours";

    private readonly TexturePathService texturePathService;

    public TourRepository(TexturePathService texturePathService)
    {
        this.texturePathService = texturePathService;
    }

    public void Create(Tour tour)
    {
        var json = JsonUtility.ToJson(ModelToSchema(tour));

        if (!Directory.Exists(saveFolder))
        {
            Directory.CreateDirectory(saveFolder);
        }

        File.WriteAllText(saveFolder + "/" + tour.Name + ".json", json);
    }

    public void Delete(string tourId)
    {

    }

    public Tour Get(string tourName)
    {
        if (!Directory.Exists(saveFolder))
        {
            Directory.CreateDirectory(saveFolder);
        }

        if(File.Exists(saveFolder + "/" + tourName + ".json"))
        {
            var json = File.ReadAllText(saveFolder + "/" + tourName + ".json");

            return SchemaToModel(JsonUtility.FromJson<TourSchema>(json));   
        }

        return null;
    }

    public void Update(Tour tour)
    {
        var json = JsonUtility.ToJson(ModelToSchema(tour));

        if (!Directory.Exists(saveFolder))
        {
            Directory.CreateDirectory(saveFolder);
        }

        File.WriteAllText(saveFolder + "/" + tour.Name + ".json", json);
    }

    private TourSchema ModelToSchema(Tour tour)
    {
        var panoramaSchemas = new PanoramaSchema[tour.Panoramas.Count];

        for (int i = 0; i < panoramaSchemas.Length; i++)
        {
            panoramaSchemas[i] = new PanoramaSchema
            {
                name = tour.Panoramas[i].Name,
                id = tour.Panoramas[i].Id,
                x = tour.Panoramas[i].PositionX,
                y = tour.Panoramas[i].PositionY,
                path = texturePathService.GetPath(tour.Panoramas[i].Id)
            };
        }

        var bridgeSchemas = new BridgeSchema[tour.Bridges.Count];

        for (int i = 0; i < bridgeSchemas.Length; i++)
        {
            bridgeSchemas[i] = new BridgeSchema
            {
                panorama_1_id = tour.Bridges[i].Panorama1.Id,
                panorama_2_id = tour.Bridges[i].Panorama2.Id
            };
        }

        var tourSchema = new TourSchema
        {
            name = tour.Name,
            default_panorama = tour.GetDeafultPanorama()?.Id,
            panoramas = panoramaSchemas,
            bridges = bridgeSchemas
        };

        return tourSchema;
    }

    private Tour SchemaToModel(TourSchema tourSchema)
    {
        var panoramas = new Panorama[tourSchema.panoramas.Length];
        var panoramaDictionary = new Dictionary<string, Panorama>();

        for (int i = 0; i < tourSchema.panoramas.Length; i++)
        {
            var panoramaSchema = tourSchema.panoramas[i];

            var panorama = new Panorama(panoramaSchema.id, panoramaSchema.name);

            panorama.Move(panoramaSchema.x, panoramaSchema.y);

            panoramas[i] = panorama;
            panoramaDictionary[panoramaSchema.id] = panorama;

            texturePathService.RegisterPath(panorama.Id, panoramaSchema.path);
        }


        var bridges = new Bridge[tourSchema.bridges.Length];

        for (int i = 0; i < tourSchema.bridges.Length; i++)
        {
            var bridgeSchema = tourSchema.bridges[i];

            var panorama1 = panoramaDictionary[bridgeSchema.panorama_1_id];
            var panorama2 = panoramaDictionary[bridgeSchema.panorama_2_id];

            bridges[i] = new Bridge(panorama1, panorama2);
        }

        var tour = new Tour(tourSchema.name, panoramas, bridges);

        tour.SetDeafultPanorama(tourSchema.default_panorama);

        return tour;
    }
}

