using UnityEngine;
using System.Collections.Generic;

public class TourMapView : MonoBehaviour
{
    [SerializeField] private PanoramaView panoramaViewPrefab;
    [SerializeField] private RectTransform mapArea;

    private PanoramaDataMenu panoramaDataMenu;
    private AddPanoramaController addPanoramaController;
    private GetPanoramaController getPanoramaController;
    private List<PanoramaView> panoramaViews = new();
    private PanoramaView selectedPanorama;

    public void Initialize(
        AddPanoramaController addPanoramaController,
        GetPanoramaController getPanoramaController,
        PanoramaDataMenu panoramaDataMenu)
    {
        this.addPanoramaController = addPanoramaController;
        this.addPanoramaController.AddPanoramaEvent += OnNewPanorama;

        this.getPanoramaController = getPanoramaController;

        this.panoramaDataMenu = panoramaDataMenu;
    }

    private void OnPanoramaClicked(string panoramaId)
    {
        if (selectedPanorama != null)
        {
            selectedPanorama.DeselectPanorama();
        }

        foreach (var panoramaView in panoramaViews)
        {
            if (panoramaView.GetPanoramaId() == panoramaId)
            {
                panoramaView.SelectPanorama();

                selectedPanorama = panoramaView;

                var panorama = getPanoramaController.GetPanorama(panoramaId);

                panoramaDataMenu.Show(panorama);

                break;
            }
        }
    }

    public void OnNewPanorama(Panorama panorama, Texture texture)
    {
        var panoramaView = Instantiate(panoramaViewPrefab, mapArea.transform);

        panoramaView.Initialize(mapArea);

        Vector2 startPosition = CalculateStartPosition(panorama);

        panoramaView.SetPosition(startPosition);

        panoramaView.View(panorama.Id, panorama.Name, texture);

        panoramaView.OnPanoramaClickedEvent += OnPanoramaClicked;
        panoramaView.OnViewChangedEvent += OnPanoramaMoved;

        panoramaViews.Add(panoramaView);
    }

    private Vector2 CalculateStartPosition(Panorama panorama)
    {
        float x = panorama.PositionX;
        float y = panorama.PositionY;

        return new Vector2(x, y);
    }

    private void OnPanoramaMoved(string panoramaId)
    {
        Debug.Log($"Panorama {panoramaId} moved");
    }

    public void ClearAll()
    {
        foreach (var view in panoramaViews)
        {
            view.OnViewChangedEvent -= OnPanoramaMoved;

            Destroy(view.gameObject);
        }

        panoramaViews.Clear();
    }

    private void OnDestroy()
    {
        if (this.addPanoramaController != null)
        {
            this.addPanoramaController.AddPanoramaEvent -= OnNewPanorama;
        }

        foreach (var view in panoramaViews)
        {
            view.OnViewChangedEvent -= OnPanoramaMoved;
        }
    }
}