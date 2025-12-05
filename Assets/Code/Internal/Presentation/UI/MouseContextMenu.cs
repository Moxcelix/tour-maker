using UnityEngine;

public class MouseContextMenu : MonoBehaviour
{
    private bool showMenu = false;
    private Vector2 menuPosition;
    private Rect menuRect = new Rect(0, 0, 150, 100);

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) 
        {
            showMenu = true;
            menuPosition = Input.mousePosition;
            menuRect.position = new Vector2(
                menuPosition.x,
                Screen.height - menuPosition.y 
            );
        }

        if (Input.GetMouseButtonDown(0) && showMenu)
        {
            Vector2 mousePos = new Vector2(
                Input.mousePosition.x,
                Screen.height - Input.mousePosition.y
            );

            if (!menuRect.Contains(mousePos))
            {
                showMenu = false;
            }
        }
    }

    void OnGUI()
    {
        if (showMenu)
        {
            menuRect = GUI.Window(0, menuRect, DrawMenu, "Menu");
        }
    }

    void DrawMenu(int windowID)
    {
        if (GUI.Button(new Rect(10, 20, 130, 20), "Action 1"))
        {
            Debug.Log("Action 1 clicked");
            showMenu = false;
        }

        if (GUI.Button(new Rect(10, 45, 130, 20), "Action 2"))
        {
            Debug.Log("Action 2 clicked");
            showMenu = false;
        }

        if (GUI.Button(new Rect(10, 70, 130, 20), "Close"))
        {
            showMenu = false;
        }

        GUI.DragWindow();
    }
}