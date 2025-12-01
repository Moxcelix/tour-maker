using UnityEngine;

public class Modules : MonoBehaviour
{
    [SerializeField] private PanoramaTextureService panoramaTextureService;

    private PanoramaPresenter panoramaPresenter;

    private void Start()
    {
        panoramaPresenter = new PanoramaPresenter(panoramaTextureService);

        panoramaPresenter.Present(new Panorama("test"));
    }
}
