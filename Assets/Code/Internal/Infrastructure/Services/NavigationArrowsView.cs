using System;
using System.Collections.Generic;
using UnityEngine;

public class NavigationArrowsView : MonoBehaviour
{
    [SerializeField] private NavigationArrow navigationArrowPrefab;
    [SerializeField] private Transform navigationArrowOrigin;

    private Panorama origin = null;
    private Panorama[] panoramaLinks = null;

    private Dictionary<string, NavigationArrow> navigationArrows = new ();

    public event Action<string> OnNavigationArrowClicked;

    public void NewBridge(Bridge bridge)
    {
        if(bridge.Panorama1.Id == origin.Id)
        {
            AddArrow(bridge.Panorama2);
        }
        else if (bridge.Panorama2.Id == origin.Id)
        {
            AddArrow(bridge.Panorama1);
        }
    }

    public void MovePanorama(Panorama panorama)
    {
        if (panorama.Id == origin.Id)
        {
            UpdateOrigin(panorama);
        }
        else
        {
            foreach (var link in panoramaLinks)
            {
                if (link.Id == panorama.Id)
                {
                    UpdateArrow(panorama);

                    break;
                }
            }
        }
    }

    public void Clear()
    {
        foreach (var arrow in navigationArrows.Values)
        {
            Destroy(arrow.gameObject);
        }

        navigationArrows.Clear();
    }

    public void NewArrows(Panorama origin, Panorama[] panoramaLinks)
    {
        this.origin = origin;
        this.panoramaLinks = panoramaLinks;

        foreach (var link in panoramaLinks)
        {
            AddArrow(link);
        }
    }

    public void UpdateOrigin(Panorama origin)
    {
        this.origin = origin;

        foreach (var link in panoramaLinks)
        {
            UpdateArrow(link);
        }
    }

    public void UpdateArrow(Panorama link)
    {
        var angle = CalculateAngle(origin, link);

        navigationArrows[link.Id].Rotate(angle);
    }

    public void AddArrow(Panorama link)
    {
        var navArrow = Instantiate(navigationArrowPrefab, navigationArrowOrigin);

        var angle = CalculateAngle(origin, link);

        navArrow.Initialize(link.Id, angle);

        navArrow.OnClick += OnNavigationArrowClicked;

        navigationArrows.Add(link.Id, navArrow);
    }

    public void RemoveArrow(Panorama link)
    {
        var navArrow = navigationArrows[link.Id];

        navArrow.OnClick -= OnNavigationArrowClicked;

        Destroy(navArrow.gameObject);

        navigationArrows.Remove(link.Id);
    }

    private float CalculateAngle(Panorama a, Panorama b)
    {
        Vector2 direction = new Vector2(
            b.PositionX - a.PositionX,
            b.PositionY - a.PositionY
        );

        float angleRadians = Mathf.Atan2(direction.x, direction.y);

        float angleDegrees = angleRadians * Mathf.Rad2Deg;

        if (angleDegrees < 0)
            angleDegrees += 360f;

        return angleDegrees;
    }
}
