using UnityEngine;
using System.Collections.Generic;

public class TourMapView : MonoBehaviour
{
    [SerializeField] private PanoramaView panoramaViewPrefab;
    [SerializeField] private RectTransform mapArea;

    private AddPanoramaController addPanoramaController;
    private GetPanoramaController getPanoramaController;
    private RenamePanoramaController renamePanoramaController;

    private PanoramaDataMenu panoramaDataMenu;
    private List<PanoramaView> panoramaViews = new();
    private PanoramaView selectedPanorama;

    public void Initialize(
        AddPanoramaController addPanoramaController,
        GetPanoramaController getPanoramaController,
        RenamePanoramaController renamePanoramaController,
        PanoramaDataMenu panoramaDataMenu)
    {
        this.addPanoramaController = addPanoramaController;
        this.addPanoramaController.AddPanoramaEvent += OnNewPanorama;

        this.getPanoramaController = getPanoramaController;

        this.renamePanoramaController = renamePanoramaController;
        this.renamePanoramaController.OnRenamePanoramaEvent += OnRenamePanorama;

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

    private void OnRenamePanorama(string panoramaId, string panoramaName)
    {
        foreach (var panoramaView in panoramaViews)
        {
            if (panoramaView.GetPanoramaId() == panoramaId)
            {
                panoramaView.SetName(panoramaName);

                break;
            }
        }
    }

    private void OnNewPanorama(Panorama panorama, Texture texture)
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
        this.addPanoramaController.AddPanoramaEvent -= OnNewPanorama;
        this.renamePanoramaController.OnRenamePanoramaEvent -= OnRenamePanorama;

        foreach (var view in panoramaViews)
        {
            view.OnViewChangedEvent -= OnPanoramaMoved;
        }
    }
}