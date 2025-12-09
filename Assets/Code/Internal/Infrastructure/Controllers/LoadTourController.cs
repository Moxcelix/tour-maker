using System.Collections.Generic;
using UnityEngine;

public class LoadTourController
{
    private readonly OpenTourUsecase openTourUsecase;

    private readonly UIButton button;
    private readonly TourMapView tourMapView;
    private readonly PanoramaTextureService panoramaTextureService;
    private readonly TexturePathService texturePathService;
    private readonly TextureLoadService textureLoadService;

    public LoadTourController(
        OpenTourUsecase openTourUsecase,
        UIButton button,
        TourMapView tourMapView,
        PanoramaTextureService panoramaTextureService,
        TexturePathService texturePathService,
        TextureLoadService textureLoadService)
    {
        this.openTourUsecase = openTourUsecase;

        this.button = button;
        this.tourMapView = tourMapView;
        this.panoramaTextureService = panoramaTextureService;
        this.texturePathService = texturePathService;
        this.textureLoadService = textureLoadService;

        this.button.Clicked += OpenTour;
    }

    private void OpenTour()
    {
        tourMapView.ClearAll();

        var tour = openTourUsecase.Execute("test");

        var textures = new Dictionary<string, Texture>();

        foreach (var panorama in tour.Panoramas)
        {
            var path = texturePathService.GetPath(panorama.Id);

            var texture = textureLoadService.LoadTexture(path);

            panoramaTextureService.AddTexture(panorama.Id, texture);

            textures.Add(panorama.Id, texture);
        }

        tourMapView.ViewTourMap(tour, textures);
    }
}
