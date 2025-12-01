using UnityEngine;

public class AddPanoramaButton : MonoBehaviour
{
    private AddPanoramaController controller;
    public void Initialize(AddPanoramaController controller)
    {
        this.controller = controller;
    }

    public void Handler()
    {
        controller.AddPanorama();
        print("click");
    }
}
