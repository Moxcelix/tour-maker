using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class NavigationArrow : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Color highlightColor = Color.blue;

    private string targetPanoramaId;
    private float angle;

    private Material originalMaterial;
    private Material highlightMaterial;
    private bool isHighlighted = false;

    public event Action<string> OnClick;

    private void Awake()
    {
        if (meshRenderer == null)
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }

        InitializeMaterials();
    }

    private void InitializeMaterials()
    {
        if (meshRenderer != null && originalMaterial == null)
        {
            // Сохраняем оригинальный материал
            originalMaterial = meshRenderer.material;

            // Создаем материал для подсветки
            highlightMaterial = new Material(originalMaterial);
            highlightMaterial.color = highlightColor;
        }
    }

    private void OnValidate()
    {
        // При изменении в инспекторе обновляем цвет подсветки
        if (highlightMaterial != null)
        {
            highlightMaterial.color = highlightColor;

            // Если подсветка активна, обновляем сразу
            if (isHighlighted && meshRenderer != null)
            {
                meshRenderer.material = highlightMaterial;
            }
        }
    }

    private void OnDestroy()
    {
        if (highlightMaterial != null)
        {
            Destroy(highlightMaterial);
        }
    }

    public void Initialize(string targetPanoramaId, float angle)
    {
        this.targetPanoramaId = targetPanoramaId;

        // Инициализируем материалы при необходимости
        if (originalMaterial == null)
        {
            InitializeMaterials();
        }

        Rotate(angle);
    }

    public void Rotate(float angle)
    {
        this.angle = angle;
        transform.localEulerAngles = Vector3.up * angle;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnClick?.Invoke(targetPanoramaId);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SetHighlighted(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetHighlighted(false);
    }

    public void SetHighlighted(bool highlighted)
    {
        if (meshRenderer == null || originalMaterial == null) return;

        isHighlighted = highlighted;
        meshRenderer.material = highlighted ? highlightMaterial : originalMaterial;
    }

    public void SetHighlightColor(Color color)
    {
        highlightColor = color;

        if (highlightMaterial != null)
        {
            highlightMaterial.color = color;

            if (isHighlighted && meshRenderer != null)
            {
                meshRenderer.material = highlightMaterial;
            }
        }
    }

    public Color GetHighlightColor()
    {
        return highlightColor;
    }

    public bool IsHighlighted => isHighlighted;
}