using System.Collections.Generic;
using UnityEngine;

public class GetTourController
{
    private readonly GetTourUsecase getTourUsecase;

    private readonly UIButton getTourButton;
    private readonly TourMapView tourMapView;
    private readonly PanoramaTextureService panoramaTextureService;

    public GetTourController(
        GetTourUsecase getTourUsecase,
        UIButton getTourButton,
        TourMapView tourMapView,
        PanoramaTextureService panoramaTextureService)
    {
        this.getTourUsecase = getTourUsecase;

        this.getTourButton = getTourButton;
        this.tourMapView = tourMapView;
        this.panoramaTextureService = panoramaTextureService;
        this.getTourButton.Clicked += GetTour;
    }

    private void GetTour()
    {
        tourMapView.ClearAll();

        var tour = getTourUsecase.Execute();

        var textures = new Dictionary<string, Texture>();

        foreach (var panorama in tour.Panoramas)
        {
            textures.Add(panorama.Id, panoramaTextureService.GetPanoramaTexture(panorama.Id));
        }

        tourMapView.ViewTourMap(tour, textures);
    }
}
