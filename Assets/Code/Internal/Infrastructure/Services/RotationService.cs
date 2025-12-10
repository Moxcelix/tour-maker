using UnityEngine;

public class RotationService : MonoBehaviour
{
    [SerializeField] private Transform origin;

    public void Rotate(float angle)
    {
        origin.eulerAngles = Vector3.up * angle;
    }
}
