using UnityEngine;

public class NewTourButton : MonoBehaviour
{
    private NewTourController controller;
    public void Initialize(NewTourController controller)
    {
        this.controller = controller;
    }

    public void Handler()
    {
        controller.NewTour();
    }
}
