using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class NavigationArrow : MonoBehaviour, IPointerClickHandler
{
    private string targetPanoramaId;
    private float angle;

    public event Action<string> OnClick;

    public void Initialize(string targetPanoramaId, float angle)
    {
        this.targetPanoramaId = targetPanoramaId;
        
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
}
