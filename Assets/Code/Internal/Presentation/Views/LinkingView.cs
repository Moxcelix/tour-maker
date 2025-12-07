using System;
using UnityEngine;

public class LinkingView : MonoBehaviour
{
    [SerializeField] private UILineRenderer previewLineRenderer;
    [SerializeField] private TourMapView tourMapView;

    private bool isLinking = false;
    private string sourcePanoramaId;

    public event Action<string, string> OnLinkingCompleted;

    public void StartLinking(string panoramaId)
    {
        if (isLinking) return;

        sourcePanoramaId = panoramaId;
        isLinking = true;
        previewLineRenderer.gameObject.SetActive(true);
    }


    public void CompleteLinking(string targetPanoramaId)
    {
        if (!isLinking || sourcePanoramaId == null) return;

        var sourceId = sourcePanoramaId;
        CancelLinking();
        OnLinkingCompleted?.Invoke(sourceId, targetPanoramaId);
    }

    public void CancelLinking()
    {
        if (!isLinking) return;

        isLinking = false;
        sourcePanoramaId = null;
        previewLineRenderer.gameObject.SetActive(false);
    }

    private void Update()
    {
        previewLineRenderer.gameObject.SetActive(isLinking);

        if (isLinking)
        {
            var panoPos = tourMapView.GetPanoramaViewPosition(sourcePanoramaId);
            var mousePos = Input.mousePosition;

            previewLineRenderer.UpdatePoints(new Vector2[] { panoPos, mousePos });
        }
    }
}