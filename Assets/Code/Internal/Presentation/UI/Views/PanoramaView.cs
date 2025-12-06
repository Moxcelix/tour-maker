using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class PanoramaView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    [SerializeField] private Image previewImage;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color selectedColor = Color.yellow;

    private string panoramaId;
    private RectTransform rectTransform;
    private RectTransform parentRectTransform;

    private Vector2 originalLocalPointerPosition;
    private Vector3 originalPanelLocalPosition;
    private bool isDragging = false;
    private bool isSelected = false;

    private MouseContextMenu mouseContextMenu;
    private MouseContextMenuList mouseContextMenuList;

    public delegate void OnPanoramaClicked(string panoramaId);
    public event OnPanoramaClicked OnPanoramaClickedEvent;

    public delegate void OnPanoramaMoved(string panoramaId, float x, float y);
    public event OnPanoramaMoved OnPanoramaMovedEvent;

    public string GetPanoramaId() => panoramaId;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void Initialize(RectTransform boundary, MouseContextMenu mouseContextMenu)
    {
        parentRectTransform = boundary;

        this.mouseContextMenu = mouseContextMenu;

        mouseContextMenuList = new MouseContextMenuList("Panorama", new MouseContextMenuList.Item[]
        {
            new("Delete", ()=>{ }),
            new("Make transition", ()=>{ }),
            new("Set as default", ()=>{ }),
            new("Rotate", ()=>{ }),
        });
    }

    public void View(string panoramaId, string panoramaName, Texture panoramaTexture)
    {
        this.panoramaId = panoramaId;

        var sprite = Sprite.Create((Texture2D)panoramaTexture,
            new Rect(0, 0, panoramaTexture.width, panoramaTexture.height),
            Vector2.zero);

        previewImage.sprite = sprite;
        nameText.text = panoramaName;
        backgroundImage.color = normalColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isDragging) return;

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            SelectPanorama();

            OnPanoramaClickedEvent?.Invoke(panoramaId);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            ShowContextMenu();

            SelectPanorama();

            OnPanoramaClickedEvent?.Invoke(panoramaId);
        }
    }

    public void SelectPanorama()
    {
        if (isSelected) return;

        isSelected = true;
        backgroundImage.color = selectedColor;

        Debug.Log($"Panorama {panoramaId} selected");
    }

    public void DeselectPanorama()
    {
        if (!isSelected) return;

        isSelected = false;
        backgroundImage.color = normalColor;
    }

    private void ShowContextMenu()
    {
        mouseContextMenu.Show(mouseContextMenuList);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        originalPanelLocalPosition = rectTransform.localPosition;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out originalLocalPointerPosition
        );

        OnPanoramaClickedEvent?.Invoke(panoramaId);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPointerPosition))
        {
            Vector3 offsetToOriginal = localPointerPosition - originalLocalPointerPosition;
            Vector3 newPosition = originalPanelLocalPosition + offsetToOriginal;
            newPosition = ClampToBounds(newPosition);

            rectTransform.localPosition = newPosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;

        Vector2 currentPosition = rectTransform.localPosition;

        OnPanoramaMovedEvent?.Invoke(panoramaId, currentPosition.x, currentPosition.y);
    }

    private Vector3 ClampToBounds(Vector3 position)
    {
        if (parentRectTransform == null) return position;

        Vector2 elementSize = rectTransform.rect.size * rectTransform.localScale;

        Vector2 boundarySize = parentRectTransform.rect.size;

        float halfElementWidth = elementSize.x / 2f;
        float halfElementHeight = elementSize.y / 2f;

        float halfBoundaryWidth = boundarySize.x / 2f;
        float halfBoundaryHeight = boundarySize.y / 2f;

        float clampedX = Mathf.Clamp(
            position.x,
            -halfBoundaryWidth + halfElementWidth,
            halfBoundaryWidth - halfElementWidth
        );

        float clampedY = Mathf.Clamp(
            position.y,
            -halfBoundaryHeight + halfElementHeight,
            halfBoundaryHeight - halfElementHeight
        );

        return new Vector3(clampedX, clampedY, position.z);
    }
    public void SetPosition(Vector2 localPosition)
    {
        if (parentRectTransform != null)
        {
            rectTransform.localPosition = ClampToBounds(localPosition);
        }
        else
        {
            rectTransform.localPosition = localPosition;
        }
    }

    public Vector2 GetPosition()
    {
        return rectTransform.localPosition;
    }

    public void SetName(string name)
    {
        nameText.text = name;
    }

    private void OnDestroy()
    {
        OnPanoramaMovedEvent = null;
    }
}