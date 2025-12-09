using System;
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

    public event Action<string> OnLeftClicked;
    public event Action<string> OnRightClicked;
    public event Action<string, float, float> OnMoved;

    public string GetPanoramaId() => panoramaId;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnDestroy()
    {
        OnMoved = null;
        OnLeftClicked = null;
        OnRightClicked = null;
    }

    public void Initialize(RectTransform boundary)
    {
        parentRectTransform = boundary;
    }

    public void View(Panorama panorama, Texture panoramaTexture)
    {
        panoramaId = panorama.Id;

        var sprite = Sprite.Create((Texture2D)panoramaTexture,
            new Rect(0, 0, panoramaTexture.width, panoramaTexture.height),
            Vector2.zero);

        previewImage.sprite = sprite;
        nameText.text = panorama.Name;

        SetPosition(panorama.PositionX, panorama.PositionY);

        backgroundImage.color = normalColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isDragging) return;

        if (!eventData.pointerCurrentRaycast.gameObject.transform.IsChildOf(transform))
            return;

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClicked?.Invoke(panoramaId);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClicked?.Invoke(panoramaId);
        }
    }

    public void SelectPanorama()
    {
        if (isSelected) return;

        isSelected = true;

        backgroundImage.color = selectedColor;
    }

    public void DeselectPanorama()
    {
        if (!isSelected) return;

        isSelected = false;

        backgroundImage.color = normalColor;
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

        Vector2 currentPosition = rectTransform.localPosition;

        (float x, float y) = ConvertLocalToNormalizedPosition(currentPosition);

        OnMoved?.Invoke(panoramaId, x, y);
    }
    public void SetPosition(float x, float y)
    {
        var localPosition = ConvertNormalizedToLocalPosition(x, y);

        rectTransform.localPosition = ClampToBounds(localPosition);
    }

    public Vector2 GetLocalPosition()
    {
        return rectTransform.position;
    }

    public void SetName(string name)
    {
        nameText.text = name;
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

    private (float x, float y) ConvertLocalToNormalizedPosition(Vector2 localPosition)
    {
        Vector2 boundarySize = parentRectTransform.rect.size;
        Vector2 elementSize = rectTransform.rect.size;

        float maxOffsetX = Mathf.Max(0.001f, boundarySize.x / 2f - elementSize.x / 2f);
        float maxOffsetY = Mathf.Max(0.001f, boundarySize.y / 2f - elementSize.y / 2f);

        float normalizedX = Mathf.Clamp(localPosition.x / maxOffsetX, -1f, 1f);
        float normalizedY = Mathf.Clamp(localPosition.y / maxOffsetY, -1f, 1f);

        return (normalizedX, normalizedY);
    }

    private Vector2 ConvertNormalizedToLocalPosition(float normalizedX, float normalizedY)
    {
        Vector2 boundarySize = parentRectTransform.rect.size;
        Vector2 elementSize = rectTransform.rect.size;

        float maxOffsetX = Mathf.Max(0.001f, boundarySize.x / 2f - elementSize.x / 2f);
        float maxOffsetY = Mathf.Max(0.001f, boundarySize.y / 2f - elementSize.y / 2f);

        float localX = Mathf.Clamp(normalizedX * maxOffsetX, -maxOffsetX, maxOffsetX);
        float localY = Mathf.Clamp(normalizedY * maxOffsetY, -maxOffsetY, maxOffsetY);

        return new Vector2(localX, localY);
    }
}