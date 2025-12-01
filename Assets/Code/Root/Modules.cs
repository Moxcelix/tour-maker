using UnityEngine;

public class Modules : MonoBehaviour
{
    [SerializeField] private PanoramaTextureService panoramaTextureService;
    [SerializeField] private TextureViewService textureViewService;

    private PanoramaPresenter panoramaPresenter;

    private void Start()
    {
        panoramaPresenter = new PanoramaPresenter(panoramaTextureService, textureViewService);

        panoramaPresenter.Present(new Panorama("test"));
    }
}
