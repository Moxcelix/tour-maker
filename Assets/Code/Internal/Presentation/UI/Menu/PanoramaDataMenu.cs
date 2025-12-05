using TMPro;
using UnityEngine;

public class PanoramaDataMenu : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    [SerializeField] private TMP_InputField inputField;

    private RenamePanoramaController renamePanoramaController;

    private string currentPanoramaId = null;

    public void Initialize(RenamePanoramaController renamePanoramaController)
    {
        this.renamePanoramaController = renamePanoramaController;

        inputField.onEndEdit.AddListener(OnNameValueChanged);
    }

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

    private void OnNameValueChanged(string name)
    {
        renamePanoramaController.RenamePanorama(currentPanoramaId, name);
    }

    private void OnDestroy()
    {
        inputField.onEndEdit.RemoveListener(OnNameValueChanged);
    }
}
