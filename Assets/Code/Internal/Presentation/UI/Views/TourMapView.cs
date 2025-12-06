using UnityEngine;
using System.Collections.Generic;

public class TourMapView : MonoBehaviour
{
    [SerializeField] private PanoramaView panoramaViewPrefab;
    [SerializeField] private RectTransform mapArea;
    [SerializeField] private Vector2 defaultPanoramaSize = new Vector2(50, 50);

    private AddPanoramaController addPanoramaController;
    private GetPanoramaController getPanoramaController;
    private RenamePanoramaController renamePanoramaController;
    private SelectPanoramaController selectPanoramaController;

    private MouseContextMenu mouseContextMenu;
    private PanoramaDataMenu panoramaDataMenu;
    private List<PanoramaView> panoramaViews = new();
    private PanoramaView selectedPanorama;

    public void Initialize(
        AddPanoramaController addPanoramaController,
        GetPanoramaController getPanoramaController,
        RenamePanoramaController renamePanoramaController,
        SelectPanoramaController selectPanoramaController,
        MouseContextMenu mouseContextMenu,
        PanoramaDataMenu panoramaDataMenu)
    {
        this.addPanoramaController = addPanoramaController;
        this.addPanoramaController.AddPanoramaEvent += OnNewPanorama;

        this.getPanoramaController = getPanoramaController;

        this.renamePanoramaController = renamePanoramaController;
        this.renamePanoramaController.OnRenamePanoramaEvent += OnRenamePanorama;

        this.selectPanoramaController = selectPanoramaController;

        this.panoramaDataMenu = panoramaDataMenu;

        this.mouseContextMenu = mouseContextMenu;
    }

    private void OnPanoramaClicked(string panoramaId)
    {
        if (selectedPanorama != null)
        {
            if (selectedPanorama.GetPanoramaId() == panoramaId)
            {
                selectPanoramaController.SelectPanorama(panoramaId);
                return;
            }

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

        panoramaView.Initialize(mapArea, mouseContextMenu);

        Vector2 startPosition = CalculateStartPosition(panorama);
        panoramaView.SetPosition(startPosition);

        panoramaView.View(panorama.Id, panorama.Name, texture);

        panoramaView.OnPanoramaClickedEvent += OnPanoramaClicked;
        panoramaView.OnPanoramaMovedEvent += OnPanoramaMoved;

        panoramaViews.Add(panoramaView);
    }

    private Vector2 CalculateStartPosition(Panorama panorama)
    {
        return ConvertNormalizedToLocalPosition(
            panorama.PositionX,
            panorama.PositionY
        );
    }

    private void OnPanoramaMoved(string panoramaId, float x, float y)
    {
        Vector2 normalizedPosition = ConvertLocalToNormalizedPosition(new Vector2(x, y));

        Debug.Log($"Panorama {panoramaId} moved to normalized: {normalizedPosition}");
    }

    private Vector2 ConvertLocalToNormalizedPosition(Vector2 localPosition)
    {
        if (mapArea == null) return Vector2.zero;

        Vector2 boundarySize = mapArea.rect.size;
        Vector2 elementSize = defaultPanoramaSize;

        float maxOffsetX = Mathf.Max(0.001f, boundarySize.x / 2f - elementSize.x / 2f);
        float maxOffsetY = Mathf.Max(0.001f, boundarySize.y / 2f - elementSize.y / 2f);

        float normalizedX = Mathf.Clamp(localPosition.x / maxOffsetX, -1f, 1f);
        float normalizedY = Mathf.Clamp(localPosition.y / maxOffsetY, -1f, 1f);

        return new Vector2(normalizedX, normalizedY);
    }

    private Vector2 ConvertNormalizedToLocalPosition(float normalizedX, float normalizedY)
    {
        if (mapArea == null) return Vector2.zero;

        Vector2 boundarySize = mapArea.rect.size;
        Vector2 elementSize = defaultPanoramaSize;

        float maxOffsetX = Mathf.Max(0.001f, boundarySize.x / 2f - elementSize.x / 2f);
        float maxOffsetY = Mathf.Max(0.001f, boundarySize.y / 2f - elementSize.y / 2f);

        float localX = Mathf.Clamp(normalizedX * maxOffsetX, -maxOffsetX, maxOffsetX);
        float localY = Mathf.Clamp(normalizedY * maxOffsetY, -maxOffsetY, maxOffsetY);

        return new Vector2(localX, localY);
    }

    public void ClearAll()
    {
        foreach (var view in panoramaViews)
        {
            view.OnPanoramaMovedEvent -= OnPanoramaMoved;
            view.OnPanoramaClickedEvent -= OnPanoramaClicked;
            Destroy(view.gameObject);
        }

        panoramaViews.Clear();
    }

    private void OnDestroy()
    {
        addPanoramaController.AddPanoramaEvent -= OnNewPanorama;
        renamePanoramaController.OnRenamePanoramaEvent -= OnRenamePanorama;

        foreach (var view in panoramaViews)
        {
            view.OnPanoramaMovedEvent -= OnPanoramaMoved;
            view.OnPanoramaClickedEvent -= OnPanoramaClicked;
        }
    }
}