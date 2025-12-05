using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class PanoramaView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image previewImage;
    [SerializeField] private TextMeshProUGUI nameText;

    private Canvas canvas; 

    private string panoramaId;
    private RectTransform rectTransform;
    private RectTransform parentRectTransform;

    private Vector2 originalLocalPointerPosition;
    private Vector3 originalPanelLocalPosition;
    private bool isDragging = false;

    public delegate void OnViewChanged(string panoramaId);
    public event OnViewChanged OnViewChangedEvent;

    public string GetPanoramaId() => panoramaId;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        canvas = GetComponentInParent<Canvas>();
    }

    public void Initialize(RectTransform boundary)
    {
        parentRectTransform = boundary;
    }

    public void View(string panoramaId, string panoramaName, Texture panoramaTexture)
    {
        this.panoramaId = panoramaId;

        var sprite = Sprite.Create((Texture2D)panoramaTexture,
            new Rect(0, 0, panoramaTexture.width, panoramaTexture.height),
            Vector2.zero);

        previewImage.sprite = sprite;
        nameText.text = panoramaName;
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

        OnViewChangedEvent?.Invoke(panoramaId);
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

    private void OnDestroy()
    {
        OnViewChangedEvent = null;
    }
}