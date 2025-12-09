using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseContextMenuList
{
    public class Item
    {
        public delegate void OnClickHandler();

        private event OnClickHandler OnClickEvent;

        public string Name { private set; get; }

        public Item(string name, OnClickHandler onClickHandler)
        {
            Name = name;
            OnClickEvent += onClickHandler;
        }

        public void Select()
        {
            OnClickEvent?.Invoke();
        }

        public void Clear()
        {
            OnClickEvent = null;
        }
    }

    private readonly List<Item> _items;

    public IEnumerable<Item> Items => _items;
    public int ItemCount => _items.Count;
    public string Name { private set; get; }

    public MouseContextMenuList(string name, Item[] items)
    {
        _items = new List<Item>(items);
        Name = name;
    }

    public void ClearAll()
    {
        foreach (var item in _items)
        {
            item.Clear();
        }
    }
}

public class MouseContextMenu : MonoBehaviour
{
    private bool showMenu = false;
    private Vector2 menuPosition;
    private Rect menuRect;
    private MouseContextMenuList mouseContextMenuList;
    private EventSystem eventSystem;

    private const float WINDOW_WIDTH = 150f;
    private const float BUTTON_HEIGHT = 25f;
    private const float BUTTON_SPACING = 5f;
    private const float PADDING = 10f;
    private const float HEADER_HEIGHT = 20f;

    private void Update()
    {
        if (!showMenu) return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = new Vector2(
                Input.mousePosition.x,
                Screen.height - Input.mousePosition.y
            );

            if (!menuRect.Contains(mousePos))
            {
                Hide();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Hide();
        }
    }

    public void Show(MouseContextMenuList mouseContextMenuList)
    {
        eventSystem = EventSystem.current;
        eventSystem.enabled = false;

        this.mouseContextMenuList = mouseContextMenuList;

        float windowHeight = CalculateWindowHeight(mouseContextMenuList.ItemCount);

        menuPosition = Input.mousePosition;

        menuRect = new Rect(
            menuPosition.x,
            Screen.height - menuPosition.y,
            WINDOW_WIDTH,
            windowHeight
        );

        menuRect = AdjustRectToScreenBounds(menuRect);

        showMenu = true;
    }

    private float CalculateWindowHeight(int itemCount)
    {
        return HEADER_HEIGHT + PADDING * 2 + (BUTTON_HEIGHT + BUTTON_SPACING) * itemCount - BUTTON_SPACING;
    }

    private Rect AdjustRectToScreenBounds(Rect rect)
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        if (rect.x + rect.width > screenWidth)
        {
            rect.x = screenWidth - rect.width;
        }

        if (rect.y + rect.height > screenHeight)
        {
            rect.y = screenHeight - rect.height;
        }

        if (rect.x < 0)
        {
            rect.x = 0;
        }

        if (rect.y < 0)
        {
            rect.y = 0;
        }

        return rect;
    }

    private void OnGUI()
    {
        if (!showMenu) return;

        var originalBackgroundColor = GUI.backgroundColor;
        var originalContentColor = GUI.contentColor;

        GUI.backgroundColor = new Color(0.9f, 0.9f, 0.9f, 0.95f);

        menuRect = GUI.Window(
            0,
            menuRect,
            DrawMenuWindow,
            mouseContextMenuList.Name
        );
    }

    private void DrawMenuWindow(int windowID)
    {
        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button)
        {
            alignment = TextAnchor.MiddleLeft,
            padding = new RectOffset(10, 10, 0, 0)
        };

        float startY = HEADER_HEIGHT + PADDING;
        float buttonWidth = WINDOW_WIDTH - PADDING * 2;

        int counter = 0;

        foreach (var item in mouseContextMenuList.Items)
        {
            if (item == null) continue;

            float yPos = startY + (BUTTON_HEIGHT + BUTTON_SPACING) * counter;

            if (GUI.Button(
                new Rect(PADDING, yPos, buttonWidth, BUTTON_HEIGHT),
                item.Name,
                buttonStyle))
            {
                item.Select();
                Hide();
                return;
            }

            counter++;
        }
    }

    public void Hide()
    {
        showMenu = false;
        eventSystem.enabled = true;
        mouseContextMenuList = null;
    }
}