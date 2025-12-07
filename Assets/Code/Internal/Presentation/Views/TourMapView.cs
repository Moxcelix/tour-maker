using System;
using System.Collections.Generic;
using UnityEngine;

public class TourMapView : MonoBehaviour
{
    [SerializeField] private PanoramaView panoramaViewPrefab;
    [SerializeField] private UILineRenderer lineRendererPrefab;
    [SerializeField] private RectTransform mapArea;
    [SerializeField] private RectTransform lineArea;

    private string selectedPanoramaId = null;

    private PanoramaDataMenu panoramaDataMenu;
    private Dictionary<string, PanoramaView> panoramaViews = new();
    private Dictionary<Bridge, UILineRenderer> bridges = new();

    public event Action<string> OnPanoramaLeftClicked;
    public event Action<string> OnPanoramaRightClicked;
    public event Action<string, float, float> OnPanoramaMoved;

    public void ViewTourMap(Tour tour, Dictionary<string, Texture> textures)
    {
        foreach (var panorama in tour.Panoramas)
        {
            AddPanorama(panorama, textures[panorama.Id]);
        }
        foreach (var bridge in tour.Bridges)
        {
            AddBridge(bridge);
        }
    }

    public void AddPanorama(Panorama panorama, Texture texture)
    {
        var panoramaView = Instantiate(panoramaViewPrefab, mapArea.transform);
        panoramaView.Initialize(mapArea);
        panoramaView.View(panorama, texture);
        panoramaView.OnLeftClicked += OnPanoramaLeftClicked;
        panoramaView.OnRightClicked += OnPanoramaRightClicked;
        panoramaView.OnMoved += OnPanoramaMoved;

        panoramaViews.Add(panorama.Id, panoramaView);
    }

    public void RenamePanorama(string panoramaId, string panoramaName)
    {
        panoramaViews[panoramaId].SetName(panoramaName);
    }

    public void SelectPanorama(string panoramaId)
    {
        if(selectedPanoramaId != null && panoramaViews.ContainsKey(selectedPanoramaId))
        {
            panoramaViews[selectedPanoramaId].DeselectPanorama();
        }

        selectedPanoramaId = panoramaId;

        panoramaViews[selectedPanoramaId].SelectPanorama();
    }

    public void AddBridge(Bridge bridge)
    {
        var lineRenderer = Instantiate(
            lineRendererPrefab,
            Vector3.zero,
            Quaternion.identity,
            lineArea.transform);

        bridges.Add(bridge, lineRenderer);
    }

    public void ClearAll()
    {
        foreach (var view in panoramaViews.Values)
        {
            view.OnLeftClicked -= OnPanoramaLeftClicked;
            view.OnRightClicked -= OnPanoramaRightClicked;
            view.OnMoved -= OnPanoramaMoved;

            Destroy(view.gameObject);
        }

        panoramaViews.Clear();

        foreach (var bridge in bridges.Values)
        {
            Destroy(bridge.gameObject);
        }

        bridges.Clear();
    }

    public Vector2 GetPanoramaViewPosition(string panoramaId)
    {
        return panoramaViews[panoramaId].GetLocalPosition();
    }

    private void Update()
    {
        foreach (var bridge in bridges.Keys)
        {
            var pano1 = panoramaViews[bridge.Panorama1.Id];
            var pano2 = panoramaViews[bridge.Panorama2.Id];

            bridges[bridge].UpdatePoints(
                new Vector2[] {
                    pano1.GetLocalPosition(),
                    pano2.GetLocalPosition()
                });
        }
    }
}
