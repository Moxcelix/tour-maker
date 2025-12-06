using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class TourMapView : MonoBehaviour
{
    [SerializeField] private PanoramaView panoramaViewPrefab;
    [SerializeField] private UILineRenderer lineRendererPrefab;
    [SerializeField] private RectTransform mapArea;
    [SerializeField] private RectTransform lineArea;
    [SerializeField] private UILineRenderer currentLineRenderer;
    [SerializeField] private Vector2 defaultPanoramaSize = new Vector2(50, 50);

    private AddPanoramaController addPanoramaController;
    private GetPanoramaController getPanoramaController;
    private RenamePanoramaController renamePanoramaController;
    private SelectPanoramaController selectPanoramaController;
    private MovePanoramaController movePanoramaController;
    private LinkPanoramasController linkPanoramaController;

    private MouseContextMenu mouseContextMenu;
    private PanoramaDataMenu panoramaDataMenu;
    private List<PanoramaView> panoramaViews = new();
    private PanoramaView selectedPanorama;
    private Dictionary<Bridge, UILineRenderer> bridges = new();

    private bool isLinking = false;
    private string currentLinkPanoramaId = null;

    public void Initialize(
        AddPanoramaController addPanoramaController,
        GetPanoramaController getPanoramaController,
        RenamePanoramaController renamePanoramaController,
        SelectPanoramaController selectPanoramaController,
        MovePanoramaController movePanoramaController,
        LinkPanoramasController linkPanoramaController,
        MouseContextMenu mouseContextMenu,
        PanoramaDataMenu panoramaDataMenu)
    {
        this.addPanoramaController = addPanoramaController;
        this.addPanoramaController.AddPanoramaEvent += OnNewPanorama;

        this.getPanoramaController = getPanoramaController;

        this.renamePanoramaController = renamePanoramaController;
        this.renamePanoramaController.OnRenamePanoramaEvent += OnRenamePanorama;

        this.selectPanoramaController = selectPanoramaController;

        this.movePanoramaController = movePanoramaController;

        this.linkPanoramaController = linkPanoramaController;
        this.linkPanoramaController.OnLinkPanoramaEvent += OnPanoramasLink;

        this.panoramaDataMenu = panoramaDataMenu;

        this.mouseContextMenu = mouseContextMenu;
    }

    private void OnPanoramasLink(Bridge bridge)
    {
        foreach (var b in bridges.Keys)
        {
            if (b.Panorama1Id == bridge.Panorama1Id && b.Panorama2Id == bridge.Panorama2Id)
            {
                return;
            }
        }

        var lineRenderer = Instantiate(lineRendererPrefab, lineArea.transform);

        lineRenderer.transform.position = Vector3.zero;

        bridges.Add(bridge, lineRenderer);
    }

    private void OnPanoramaClicked(string panoramaId)
    {
        if (isLinking)
        {
            StopLinking(panoramaId);
        }

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

    private void CancelLinking()
    {
        isLinking = false;
        currentLinkPanoramaId = null;
    }

    private void StartLinking(string panoramaId)
    {
        currentLinkPanoramaId = panoramaId;
        isLinking = true;
    }

    private void StopLinking(string panoramaId)
    {
        if(!isLinking)
        {
            return;
        }

        try
        {
            linkPanoramaController.LinkPanoramas(currentLinkPanoramaId, panoramaId);

            isLinking = false;
            currentLinkPanoramaId = null;
        }
        catch
        {

        }
    }
    private void HandleClickOutsidePanoramas()
    {
        if (!isLinking) return;

        if (EventSystem.current.IsPointerOverGameObject())
        {
            var pointerEventData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, results);

            bool clickedOnPanorama = false;
            foreach (var result in results)
            {
                if (result.gameObject.GetComponentInParent<PanoramaView>() != null)
                {
                    clickedOnPanorama = true;

                    break;
                }
            }

            if (!clickedOnPanorama)
            {
                CancelLinking();
            }
        }
    }

    private void OnNewPanorama(Panorama panorama, Texture texture)
    {
        var panoramaView = Instantiate(panoramaViewPrefab, mapArea.transform);

        var mouseContextMenuList = new MouseContextMenuList("Panorama", new MouseContextMenuList.Item[]
        {
            new("Make transition", ()=>{ StartLinking(panorama.Id); })
        });

        panoramaView.Initialize(mapArea, mouseContextMenu, mouseContextMenuList);

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

        movePanoramaController.MovePanorama(panoramaId, x, y);

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

    private PanoramaView GetPanoramaView(string panoramaId)
    {
        foreach(var pano in panoramaViews)
        {
            if (pano.GetPanoramaId().Equals(panoramaId))
            {
                return pano;
            }
        }

        return null;
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

    private void Update()
    {
        currentLineRenderer.gameObject.SetActive(isLinking);

        if (isLinking)
        {
            var pano = GetPanoramaView(currentLinkPanoramaId);
            var mousePos = Input.mousePosition;

            currentLineRenderer.UpdatePoints(new Vector2[] { pano.GetPosition(), mousePos });

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CancelLinking();
            }

            if (Input.GetMouseButtonDown(0))
            {
                HandleClickOutsidePanoramas();
            }
        }

        foreach(var bridge in bridges.Keys)
        {
            var pano1 = GetPanoramaView(bridge.Panorama1Id);
            var pano2 = GetPanoramaView(bridge.Panorama2Id);

            bridges[bridge].UpdatePoints(
                new Vector2[] {
                    pano1.GetPosition(), 
                    pano2.GetPosition()
                });
        }
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