using System.Collections.Generic;
using UnityEngine;

public class GetTourController
{
    private readonly GetTourUsecase getTourUsecase;
    private readonly MovePanoramaUsecase movePanoramaUsecase;
    private readonly LinkPanoramasUsecase linkPanoramasUsecase;
    private readonly SelectPanoramaUsecase selectPanoramaUsecase;

    private readonly MouseContextMenu menu;
    private readonly UIButton getTourButton;
    private readonly TourMapView tourMapView;
    private readonly LinkingView linkingView;
    private readonly PanoramaTextureService panoramaTextureService;
    private readonly NavigationArrowsView navigationArrowsView;

    public GetTourController(
        GetTourUsecase getTourUsecase,
        MovePanoramaUsecase movePanoramaUsecase,
        LinkPanoramasUsecase linkPanoramasUsecase,
        SelectPanoramaUsecase selectPanoramaUsecase,
        UIButton getTourButton,
        TourMapView tourMapView,
        LinkingView linkingView,
        PanoramaTextureService panoramaTextureService,
        MouseContextMenu menu,
        NavigationArrowsView navigationArrowsView)
    {
        this.getTourUsecase = getTourUsecase;
        this.movePanoramaUsecase = movePanoramaUsecase;

        this.getTourButton = getTourButton;
        this.tourMapView = tourMapView;
        this.linkingView = linkingView;
        this.panoramaTextureService = panoramaTextureService;
        this.menu = menu;
        this.navigationArrowsView = navigationArrowsView;
        this.linkPanoramasUsecase = linkPanoramasUsecase;
        this.selectPanoramaUsecase = selectPanoramaUsecase;

        this.tourMapView.OnPanoramaLeftClicked += OnClick;
        this.tourMapView.OnPanoramaRightClicked += ShowContextMenu;
        this.tourMapView.OnPanoramaMoved += OnMove;
        this.linkingView.OnLinkingCompleted += OnLink;
        this.navigationArrowsView.OnNavigationArrowClicked += OnNavigate;
        this.getTourButton.Clicked += GetTour;
    }

    private void OnNavigate(string panoramaId)
    {
        selectPanoramaUsecase.Execute(panoramaId);
    }

    public void GetTour()
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

    private void OnLink(string panorama1Id, string panorama2Id)
    {
        Debug.Log("link!");
        var bridge = linkPanoramasUsecase.Execute(panorama1Id, panorama2Id);

        navigationArrowsView.NewBridge(bridge);

        tourMapView.AddBridge(bridge);
    }

    private void OnMove(string panoramaId, float  x, float y)
    {
        var panorama = movePanoramaUsecase.Execute(panoramaId, x, y);

        navigationArrowsView.MovePanorama(panorama);
    }

    private void OnClick(string panoramaId)
    {
        tourMapView.SelectPanorama(panoramaId);

        linkingView.CompleteLinking(panoramaId);
    }

    private void ShowContextMenu(string panoramaId)
    {
        var list = GetContextMenu(panoramaId);

        menu.Show(list);
    }

    private MouseContextMenuList GetContextMenu(string panoramaId)
    {
        var items = new MouseContextMenuList.Item[]
        {
            new("Make transition", () => linkingView.StartLinking(panoramaId)),
        };

        return new MouseContextMenuList("Panorama", items);
    }
}
