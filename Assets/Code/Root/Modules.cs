using UnityEngine;

public class Modules : MonoBehaviour
{
    [SerializeField] private PanoramaTextureService panoramaTextureService;
    [SerializeField] private TextureViewService textureViewService;

    private PanoramaPresenter panoramaPresenter;
    private FileDialogService fileDialogService;

    private void Start()
    {
        panoramaPresenter = new PanoramaPresenter(panoramaTextureService, textureViewService);
        fileDialogService = new FileDialogService();

        fileDialogService.OpenFileDialog();

        panoramaPresenter.Present(new Panorama("test"));
    }
}
