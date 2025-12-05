using TMPro;
using UnityEngine;

public class PanoramaDataMenu : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    [SerializeField] private TMP_InputField inputField;

    public void Show(Panorama panorama)
    {
        panel.SetActive(true);

        inputField.text = panorama.Name;
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
}
