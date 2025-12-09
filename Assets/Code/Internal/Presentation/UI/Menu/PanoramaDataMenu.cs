using System;
using TMPro;
using UnityEngine;

public class PanoramaDataMenu : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    [SerializeField] private TMP_InputField inputField;

    private string currentPanoramaId = null;

    public Action<string, string> OnNameValueChanged;


    public void Show(Panorama panorama)
    {
        panel.SetActive(true);

        inputField.text = panorama.Name;

        currentPanoramaId = panorama.Id;
    }

    public void Hide()
    {
        panel.SetActive(false);

        currentPanoramaId = null;
    }

    private void OnNameValueChangedHandler(string name)
    {
        OnNameValueChanged?.Invoke(currentPanoramaId, name);
    }

    private void Start()
    {
        inputField.onEndEdit.AddListener(OnNameValueChangedHandler);
    }

    private void OnDestroy()
    {
        inputField.onEndEdit.RemoveListener(OnNameValueChangedHandler);
    }
}
