using System;
using UnityEngine;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{
    [SerializeField] private Button button;

    public event Action Clicked;

    private void Start()
    {
        if(button == null) button = GetComponent<Button>();

        button.onClick.AddListener(() => Clicked?.Invoke());
    }
}
