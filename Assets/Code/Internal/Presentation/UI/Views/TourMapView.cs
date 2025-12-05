using UnityEngine;
using System.Collections.Generic;

public class TourMapView : MonoBehaviour
{
    [SerializeField] private PanoramaView panoramaViewPrefab;
    [SerializeField] private RectTransform mapArea;

    private AddPanoramaController addPanoramaController;
    private List<PanoramaView> panoramaViews = new();

    public void Initialize(AddPanoramaController addPanoramaController)
    {
        this.addPanoramaController = addPanoramaController;
        this.addPanoramaController.AddPanoramaEvent += OnNewPanorama;
    }

    public void OnNewPanorama(Panorama panorama, Texture texture)
    {
        var panoramaView = Instantiate(panoramaViewPrefab, mapArea.transform);

        panoramaView.Initialize(mapArea);

        Vector2 startPosition = CalculateStartPosition(panorama);

        panoramaView.SetPosition(startPosition);

        panoramaView.View(panorama.Id, panorama.Name, texture);

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