using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanoramaDataMenu : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TMP_InputField angleInputField;
    [SerializeField] private Slider angleSlider;

    private string currentPanoramaId = null;

    public Action<string, string> OnNameValueChanged;
    public Action<string, float> OnAngleValueChanged;

    private bool eventLock = false;

    public void Show(Panorama panorama)
    {
        panel.SetActive(true);

        inputField.text = panorama.Name;

        eventLock = true;
        angleInputField.text = panorama.Rotation.ToString();
        angleSlider.value = (panorama.Rotation + 360.0f) / 720.0f;
        eventLock = false;

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

    private void OnAngleValueChangedInputFieldHandler(string value)
    {
        if (eventLock)
        {
            return;
        }

        eventLock = true;

        float newValue = Mathf.Clamp(float.Parse(value), - 360.0f, 360.0f);

        angleSlider.value = (newValue + 360.0f) / 720.0f;

        OnAngleValueChanged?.Invoke(currentPanoramaId, newValue);

        eventLock = false;
    }

    private void OnAngleValueChangedSliderHandler(float value)
    {
        if (eventLock)
        {
            return;
        }

        eventLock = true;

        float newValue = (value - 0.5f) * 360.0f;

        angleInputField.text = newValue.ToString();

        OnAngleValueChanged?.Invoke(currentPanoramaId, newValue);

        eventLock = false;
    }

    private void Start()
    {
        inputField.onEndEdit.AddListener(OnNameValueChangedHandler);
        angleSlider.onValueChanged.AddListener(OnAngleValueChangedSliderHandler);
        angleInputField.onValueChanged.AddListener(OnAngleValueChangedInputFieldHandler);
    }

    private void OnDestroy()
    {
        inputField.onEndEdit.RemoveListener(OnNameValueChangedHandler);
        angleSlider.onValueChanged.RemoveListener(OnAngleValueChangedSliderHandler);
        angleInputField.onValueChanged.RemoveListener(OnAngleValueChangedInputFieldHandler);
    }
}
